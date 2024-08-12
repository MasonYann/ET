﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ET
{
    public enum SchedulerType
    {
        Main,
        Thread,
        ThreadPool,
    }
    
    public class FiberManager: Singleton<FiberManager>, ISingletonAwake, ISingletonReverseDispose
    {
        private readonly IScheduler[] schedulers = new IScheduler[3];
        
        private int idGenerator = 10000000; // 10000000以下为保留的用于StartSceneConfig的fiber id, 1个区配置1000个纤程，可以配置10000个区
        private ConcurrentDictionary<int, Fiber> fibers = new();

        private MainThreadScheduler mainThreadScheduler;
        
        public void Awake()
        {
            this.mainThreadScheduler = new MainThreadScheduler(this);
            this.schedulers[(int)SchedulerType.Main] = this.mainThreadScheduler;
            
#if ENABLE_VIEW && UNITY_EDITOR
            this.schedulers[(int)SchedulerType.Thread] = this.mainThreadScheduler;
            this.schedulers[(int)SchedulerType.ThreadPool] = this.mainThreadScheduler;
#else
            this.schedulers[(int)SchedulerType.Thread] = new ThreadScheduler(this);
            this.schedulers[(int)SchedulerType.ThreadPool] = new ThreadPoolScheduler(this);
#endif
        }
        
        public void Update()
        {
            this.mainThreadScheduler.Update();
        }

        public void LateUpdate()
        {
            this.mainThreadScheduler.LateUpdate();
        }

        protected override void Destroy()
        {
            foreach (IScheduler scheduler in this.schedulers)
            {
                scheduler.Dispose();
            }
            
            foreach (var kv in this.fibers)
            {
                kv.Value.Dispose();
            }

            this.fibers = null;
        }

        public async ETTask<int> Create(SchedulerType schedulerType, int fiberId, int zone, SceneType sceneType, string name)
        {
            try
            {
                Fiber fiber = new(fiberId, zone, sceneType, name);

                if (!this.fibers.TryAdd(fiberId, fiber))
                {
                    throw new Exception($"same fiber already existed, if you remove, please await Remove then Create fiber! {fiberId}");
                }
                this.schedulers[(int) schedulerType].Add(fiberId);
                
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

                fiber.ThreadSynchronizationContext.Post(async () =>
                {
                    try
                    {
                        // 根据Fiber的SceneType分发Init,必须在Fiber线程中执行
                        await EventSystem.Instance.Invoke<FiberInit, ETTask>((long)sceneType, new FiberInit() {Fiber = fiber});
                        tcs.SetResult(true);
                    }
                    catch (Exception e)
                    {
                        Log.Error($"init fiber fail: {sceneType} {e}");
                    }
                });

                await tcs.Task;
                return fiberId;
            }
            catch (Exception e)
            {
                throw new Exception($"create fiber error: {fiberId} {sceneType}", e);
            }
        }
        
        public async ETTask<int> Create(SchedulerType schedulerType, int zone, SceneType sceneType, string name)
        {
            //Interlocked.Increment(ref this.idGenerator) 
            //防止多个线程同时修改生成的 Id，从而导致数据竞争或不一致问题。
            //这个方法是线程安全的，因为它使用特殊的CPU指令来确保递增操作不会被其他线程的操作所干扰。这使得 Interlocked.Increment 成为在多线程环境中进行简单同步的首选方法之一。
            int fiberId = Interlocked.Increment(ref this.idGenerator);
            return await this.Create(schedulerType, fiberId, zone, sceneType, name);
        }
        
        public async ETTask Remove(int id)
        {
            Fiber fiber = this.Get(id);
            TaskCompletionSource<bool> tcs = new();
            // 要扔到fiber线程执行，否则会出现线程竞争
            fiber.ThreadSynchronizationContext.Post(() =>
            {
                if (this.fibers.Remove(id, out Fiber f))
                {
                    f.Dispose();
                }
                tcs.SetResult(true);
            });
            await tcs.Task;
        }

        // 不允许外部调用，容易出现多线程问题, 只能通过消息通信，不允许直接获取其它Fiber引用
        internal Fiber Get(int id)
        {
            this.fibers.TryGetValue(id, out Fiber fiber);
            return fiber;
        }

        public int Count()
        {
            return this.fibers.Count;
        }
    }
}