using System;
using System.Drawing;
using TileImageViewer.Model;

/// <summary>
/// 常量类
/// </summary>
internal class Constants
{
    public static String ScanPageBarcodeFilePath = "\\AL_Barcode.jpg";

    public static int PicW { get; set; } = 256;

    public static int PicH { get; set; } = 256;

    public static int LoadMoreTriggerSpace { get; set; } = 256;

    public static int BigImgCols { get; set; } = 18;
    public static int BigImgRows { get; set; } = 18;

    public static int ImgRowsOutter { get; set; } = 5;
    public static int ImgColsOutter { get; set; } = 5;

    public static int ImgCtrlMaxSpace { get; set; } = 200;

    public static int MemoryCachedMinLevel { get; set; } = 7;

    public static int DefaultFullPageMaxLevel { get; set; } = 5;

    public enum Direction
    {
        Left, Right, Up, Down
    };

    public static Color AnnoationColor { get; set; } = Color.Blue;
    public static float AnnoationPenWidth { get; set; } = (float)4;
    public static int FixRectangleWidth { get; set; } = 1000;//um
    public static int FixRectangleHeight { get; set; } = 1000;//um
    public static int FixCircleR { get; set; } = 1000;//um

    public static double AbsLenToRealLen_um = 1.0d;
    public static double RealLenToAbsLan_um = 1.0d;

    public static long CurrentTimestamp()
    {
        return (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
    }

    public static Setting SettingDetail;

    public static Action OnSettingChange { get; set; }

    public static void LoadSettingInfo()
    {
        SettingManager stInstance = SettingManager.getInstance();
        SettingDetail = stInstance.getSettingDetail();

        if (OnSettingChange != null)
        {
            OnSettingChange();
        }
    }

    public static ColorCorrection ColorCorrectionDetail;

    public static Action OnColorChange { get; set; }

    public static void LoadColorInfo(String filePath)
    {
        ColorCorrectionManager colorManager = new ColorCorrectionManager(filePath);
        ColorCorrectionDetail = colorManager.getSettingDetail();
        if (OnColorChange != null)
        {
            OnColorChange();
        }
    }

    public static float PageScale(int level)
    {
        if (level < 1)
        {
            level = 1;
        }

        if (level > 9)
        {
            level = 9;
        }
        float ret = 0;
        float[] retarr = { 1, 2, 3, 4, 5, 8, 10, 20, 40 };
        ret = retarr[level - 1];
        return ret;
    }
}