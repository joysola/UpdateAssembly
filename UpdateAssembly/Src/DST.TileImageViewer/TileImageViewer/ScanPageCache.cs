using System;
using System.Collections.Generic;
using System.Drawing;

public class ScanPageCache
{
    private static Dictionary<string, Bitmap> bitmapCache = new Dictionary<string, Bitmap>();

    public static void PutFullPageCache(String scanpageId, int level, Bitmap bitmap)
    {
        string pid = scanpageId + "_" + level;
        if (bitmapCache.ContainsKey(pid))
        {
            bitmapCache.Remove(pid);
        }
        bitmapCache.Add(pid, bitmap);
    }

    public static Bitmap GetFullPageBitmap(String scanpageId, int level)
    {
        string pid = scanpageId + "_" + level;
        if (!bitmapCache.ContainsKey(pid))
        {
            return null;
        }
        return bitmapCache[pid];
    }

    /// <summary>
    /// 清理静态属性bitmapCache，释放非托管资源
    /// </summary>
    public static void Clear()
    {
        // 必须处理掉bitmap的资源
        foreach (var item in bitmapCache)
        {
            item.Value.Dispose();
        }
        bitmapCache.Clear();
    }
}