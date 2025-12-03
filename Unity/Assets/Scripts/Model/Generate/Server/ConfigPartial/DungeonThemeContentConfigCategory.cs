using System;

namespace ET
{
    public partial class DungeonThemeContentConfigCategory
    {
        public int GetRealContentId(string tag, int themePackageId)
        {
            foreach (var kv in this.dict)
            {
                DungeonThemeContentConfig config = kv.Value;
                Log.Debug($"配置表名: {nameof(DungeonThemeContentConfig)}，主题包ID: {config.ThemePackageId}，标签: {config.Tag}");
                if (config.ThemePackageId == themePackageId && config.Tag.Split("\"",StringSplitOptions.RemoveEmptyEntries)[0] == tag)
                {
                    return config.TotalId;
                }
            }

            throw new Exception($"配置找不到，配置表名: {nameof(DungeonThemeContentConfig)}，主题包ID: {themePackageId}，标签: {tag}");
            // return 0;
        }
    }
}