using SqlSugar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

/// <summary>
/// 扫描切片
/// </summary>
public class ScanPage : IDisposable
{
    public String BaseFilePath { get; set; }
    private Dictionary<int, ScanPageLevel> LevelColsRows;  // Length of <Column, Row>
    public int MaxLevel { get; set; }
    public List<Annotation> Annotations { get; set; }
    public int DbState { get => dbState; set => dbState = value; }

    private int dbState = 0;

    /// <summary>
    /// 清理资源
    /// </summary>
    public void Dispose()
    {
        LevelColsRows?.Clear();
        LevelColsRows = null;

        Annotations?.Clear();
        Annotations = null;
    }

    public SqlSugarClient GetScanPageDbClient()
    {
        string strDbPath = BaseFilePath;
        if (strDbPath.StartsWith("\\\\"))
            strDbPath = strDbPath.Replace("\\", "/");

        string connStr = @"Data Source=" + @"" + strDbPath + "\\info.db;Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10";
        SqlSugarClient db = new SqlSugarClient(
                    new ConnectionConfig()
                    {
                        ConnectionString = connStr,
                        DbType = SqlSugar.DbType.Sqlite,//设置数据库类型
                        IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                        InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
                    });
        return db;
    }

    public String GetSampleName()
    {
        string showText = BaseFilePath;
        int lastPosition = showText.LastIndexOf("\\");
        showText = showText.Substring(lastPosition + 1, showText.Length - lastPosition - 1);
        return showText;
    }

    public void LoadAnnotationFromDb()
    {
        using (SqlSugar.SqlSugarClient db = this.GetScanPageDbClient())
        {
            try
            {
                //根据模型建表
                db.CodeFirst.SetStringDefaultLength(200/*设置varchar默认长度为200*/).InitTables(typeof(Stock));
            }
            catch (SqlSugarException e)
            {
                if (e.Message.Contains("readonly"))
                    DbState = 1;
                else
                    DbState = 2;
            }

            if (DbState < 2)
            {
                List<Stock> stockAll = db.Queryable<Stock>().ToList();
                foreach (Stock st in stockAll)
                {
                    string Title = st.Title;
                    AnnotationType Type = (AnnotationType)Convert.ToInt32(st.Type);

                    float Width = Convert.ToSingle(st.Width);
                    int Red = Convert.ToInt32(st.Red);
                    int Green = Convert.ToInt32(st.Green);
                    int Blue = Convert.ToInt32(st.Blue);

                    Annotation an = new Annotation(Title, Type,
                        st.GetPoint0(), st.GetPoint1(), st.GetPoint2(), st.GetPoint3(), Color.FromArgb(Red, Green, Blue), Width);
                    an.SetLocation(st.GetPoint0(), st.GetPoint1(), st.GetPoint2(), st.GetPoint3(), this.ConvertUnit());
                    an.Id = st.Id;

                    if (an.Check())
                        this.Annotations.Add(an);
                }
            }
        }
    }

    public int InsertAnnotationToDb(Annotation an)
    {
        int id = 0;
        try
        {
            using (SqlSugarClient db = GetScanPageDbClient())
            {
                //根据模型建表
                db.CodeFirst.SetStringDefaultLength(200/*设置varchar默认长度为200*/).InitTables(typeof(Stock));
                Stock st = Stock.MakeStock(an);

                id = db.Insertable(st).ExecuteReturnIdentity();
            }
        }
        catch (SqlSugarException e)
        {
        }

        return id;
    }

    public void UpdateAnnotationToDb(Annotation an)
    {
        if (an != null)
        {
            try
            {
                using (SqlSugarClient db = GetScanPageDbClient())
                {
                    db.CodeFirst.SetStringDefaultLength(200/*设置varchar默认长度为200*/).InitTables(typeof(Stock));
                    Stock st = Stock.MakeStock(an);

                    int i = db.Updateable(st).ExecuteCommand();
                }
            }
            catch (SqlSugarException e)
            {
            }
        }
    }

    public Point MaxAbsPoint()
    {
        ScanPageLevel max = GetColsRows(MaxLevel);
        return new Point(max.Level * Constants.PicW, max.Level * Constants.PicH);
    }

    public Bitmap QrCode()
    {
        String path = BaseFilePath + Constants.ScanPageBarcodeFilePath;
        Image img = DST.TileImageViewer.Properties.Resources.barcode;
        if (File.Exists(path))
        {
            img = Image.FromFile(path);
        }
        Bitmap imgBit = new Bitmap(img);
        return imgBit;
    }

    public int DeleteAnnotationFromDb(Annotation an)
    {
        int i = 0;
        if (an.Id >= 0)
        {
            try
            {
                using (SqlSugarClient db = GetScanPageDbClient())
                {
                    i = db.Deleteable<Stock>(t => t.Id == an.Id).ExecuteCommand();
                }
            }
            catch (SqlSugarException e)
            {
            }
        }

        return i;
    }

    public ScanPage(string filePath)
    {
        BaseFilePath = filePath;
        LevelColsRows = new Dictionary<int, ScanPageLevel>();
        MaxLevel = CalcMaxLevel();

        Annotations = new List<Annotation>();
    }

    public double ConvertUnit()
    {
        ScanPageLevel maxLevelPage = GetColsRows(MaxLevel);
        double m = (maxLevelPage.EndRow / 32) * 0.176;

        return m;
    }

    public ScanPageLevel GetColsRows(int level)
    {
        if (level >= MaxLevel)
        {
            level = MaxLevel;
        }

        if (level <= 0)
        {
            return null;
        }

        if (CalcColsRows(level))
        {
            return this.LevelColsRows[level];
        }
        return null;
    }

    public static PageException IsValidScanPage(string path)
    {
        String reason = "";
        int code = 0;
        PageException exception = new PageException();

        if (!File.Exists(path + "\\1\\0\\0.jpg"))
        {
            reason = "错误的样本文件";
            code = PageException.ERR_NON_BASE_FILE;
        }
        else if (!File.Exists(path + "\\Slide.dat"))
        {
            reason = "样本缺少Slide.dat信息文件，无法打开";
            code = PageException.ERR_NON_SLIDE_DATA;
        }

        if (String.IsNullOrEmpty(reason))
        {
            exception = null;
        }
        else
        {
            exception.Message = reason;
            exception.ErrCode = code;
        }
        return exception;
    }

    private int CalcMaxLevel()
    {
        if (MaxLevel > 0)
        {
            return MaxLevel;
        }
        if (Directory.Exists(BaseFilePath))
        {
            string[] directories = Directory.GetDirectories(BaseFilePath);
            if (directories != null)
            {
                int[] array = new int[directories.Length];
                for (int i = 0; i < directories.Length; i++)
                {
                    try
                    {
                        array[i] = Convert.ToInt32(new DirectoryInfo(directories[i]).Name);
                    }
                    catch (Exception e)
                    {
                    }
                }
                Array.Sort(array);
                int maxLevel = array[array.Length - 1];
                return maxLevel;
            }
        }
        return 0;
    }

    public bool CalcColsRows(int level)
    {
        if (LevelColsRows.ContainsKey(level))
        {
            return true;
        }
        ScanPageLevel pageLevel = new ScanPageLevel
        {
            Level = level
        };
        if (!Directory.Exists(BaseFilePath + "\\" + level))
        {
            return false;
        }

        String file = BaseFilePath + "\\Slide.dat";
        if (File.Exists(file))
        {
            //couny by file
            pageLevel = GenPageLevelByIni(pageLevel);
        }
        else
        {
            // count automate
            pageLevel = GenPageLevelAutomate(pageLevel);
            //return false;
        }

        if (level > 1)
        {
            ScanPageLevel baseScanPageLevel = LevelColsRows[1];
            pageLevel.ToScale = Math.Max(pageLevel.EndCol / baseScanPageLevel.EndCol
                , pageLevel.EndRow / baseScanPageLevel.EndRow);
        }
        else
        {
            pageLevel.ToScale = Math.Max(pageLevel.EndCol, pageLevel.EndRow);
        }

        pageLevel.ToScale = Constants.PageScale(level);

        LevelColsRows.Add(level, pageLevel);
        return LevelColsRows.ContainsKey(level);
    }

    public float ImgScaleVal(float pagescale, int pagelevel)
    {
        if (pagelevel < 1)
        {
            return 0;
        }

        ScanPageLevel currLevel = GetColsRows(pagelevel);

        float ret = ret = (currLevel.ToScale) * pagescale;
        ret = (float)Math.Round(ret, 1);
        return ret;
    }

    public ScanPageLevel GetByScale(float scale)
    {
        ScanPageLevel ret = new ScanPageLevel();
        for (int i = 1; i <= MaxLevel; i++)
        {
            ScanPageLevel ctmp = GetColsRows(i);
            if (ctmp.ToScale >= scale)
            {
                ret.Level = ctmp.Level;
                ret.ToScale = ctmp.ToScale;
                ret.StartCol = ctmp.StartCol;
                ret.StartRow = ctmp.StartRow;
                ret.EndCol = ctmp.EndCol;
                ret.EndRow = ctmp.EndRow;
                break;
            }
        }
        ret.ToScale = scale / ret.ToScale;
        return ret;
    }

    /// <summary>
    /// 根据图片文件的rgb值确定是否为有效图片，非常慢！！！！
    /// 只有在slide.dat缺少layermsg时，才不得不调用此方法
    /// </summary>
    /// <param name="pageLevel"></param>
    /// <returns></returns>
    private ScanPageLevel GenPageLevelAutomate(ScanPageLevel pageLevel)
    {
        string[] directories = Directory.GetDirectories(BaseFilePath + "\\" + pageLevel.Level);
        if (directories != null)
        {
            int[] array = new int[directories.Length];
            for (int i = 0; i < directories.Length; i++)
            {
                array[i] = Convert.ToInt32(new DirectoryInfo(directories[i]).Name);
            }
            Array.Sort(array);
            int item = CalcRows(pageLevel.Level);
            pageLevel.EndCol = array.Length - 1;
            pageLevel.EndRow = item - 1;
            pageLevel = CountColRowDetail(pageLevel);
        }
        return pageLevel;
    }

    private ScanPageLevel GenPageLevelByIni(ScanPageLevel pageLevel)
    {
        String file = BaseFilePath + "\\Slide.dat";
        if (File.Exists(file))
        {
            StringBuilder sc = new StringBuilder();
            FileUtils.GetPrivateProfileString("LayerMag", pageLevel.Level + "C", "0", sc, 255, file);

            StringBuilder sr = new StringBuilder();
            FileUtils.GetPrivateProfileString("LayerMag", pageLevel.Level + "R", "0", sr, 255, file);

            pageLevel.StartCol = 0;
            pageLevel.StartRow = 0;
            try
            {
                pageLevel.EndCol = float.Parse(sc.ToString());
                pageLevel.EndRow = float.Parse(sr.ToString());
            }
            catch (Exception exp)
            {
            }
        }

        return pageLevel;
    }

    private ScanPageLevel CountColRowDetail(ScanPageLevel pageLevel)
    {
        float colStartAbs = pageLevel.EndCol * Constants.PicW;
        float rowStartAbs = pageLevel.EndRow * Constants.PicH;
        float colEndAbs = 0;
        float rowEndAbs = 0;

        for (int col = 0; col <= pageLevel.EndCol; col++)
        {
            for (int row = 0; row <= pageLevel.EndRow; row++)
            {
                FileInfo file = new FileInfo(BaseFilePath + "\\" + pageLevel.Level + "\\" + col + "\\" + row + ".jpg");
                Rectangle validRect = ImgHelper.ValidRect(file);
                if (validRect.Size.IsEmpty)
                {
                    continue;
                }

                colStartAbs = Math.Min(validRect.X + col * Constants.PicW, colStartAbs);
            }
            if (colStartAbs < pageLevel.EndCol * Constants.PicW)
            {
                break;
            }
        }

        for (int col = Convert.ToInt16(pageLevel.EndCol); col >= 0; col--)
        {
            for (int row = 0; row <= pageLevel.EndRow; row++)
            {
                FileInfo file = new FileInfo(BaseFilePath + "\\" + pageLevel.Level + "\\" + col + "\\" + row + ".jpg");
                Rectangle validRect = ImgHelper.ValidRect(file);
                if (validRect.Size.IsEmpty)
                {
                    continue;
                }

                colEndAbs = Math.Max(validRect.X + validRect.Size.Width + col * Constants.PicW, colEndAbs);
            }
            if (colEndAbs > 0)
            {
                break;
            }
        }

        for (int row = 0; row <= pageLevel.EndRow; row++)
        {
            for (int col = 0; col <= pageLevel.EndCol; col++)
            {
                FileInfo file = new FileInfo(BaseFilePath + "\\" + pageLevel.Level + "\\" + col + "\\" + row + ".jpg");
                Rectangle validRect = ImgHelper.ValidRect(file);
                if (validRect.Size.IsEmpty)
                {
                    continue;
                }

                rowStartAbs = Math.Min(validRect.Y + row * Constants.PicH, rowStartAbs);
            }
            if (rowStartAbs < pageLevel.EndRow * Constants.PicH)
            {
                break;
            }
        }

        for (int row = Convert.ToInt16(pageLevel.EndRow); row >= 0; row--)
        {
            for (int col = 0; col <= pageLevel.EndCol; col++)
            {
                FileInfo file = new FileInfo(BaseFilePath + "\\" + pageLevel.Level + "\\" + col + "\\" + row + ".jpg");
                Rectangle validRect = ImgHelper.ValidRect(file);
                if (validRect.Size.IsEmpty)
                {
                    continue;
                }
                rowEndAbs = Math.Max(validRect.Y + validRect.Size.Height + row * Constants.PicH, rowEndAbs);
            }
            if (rowEndAbs > 0)
            {
                break;
            }
        }

        pageLevel = new ScanPageLevel
        {
            EndCol = colEndAbs / Constants.PicW,
            EndRow = colEndAbs / Constants.PicH,
            StartRow = rowStartAbs / Constants.PicH,
            StartCol = colStartAbs / Constants.PicW,
            Level = pageLevel.Level
        };
        return pageLevel;
    }

    private int CalcRows(int level)
    {
        int ret = 0;
        if (Directory.Exists(BaseFilePath + "\\" + level))
        {
            string[] directories = Directory.GetDirectories(BaseFilePath + "\\" + level);
            if (directories != null)
            {
                for (int i = 0; i < directories.Length; i++)
                {
                    List<string> nameList = new List<string>();
                    FileUtils.DirFiles(directories[i], nameList);
                    foreach (string filename in nameList)
                    {
                        FileInfo f = new FileInfo(directories[i] + "\\" + filename);

                        string fileNoS = f.Name.ToLower().Replace(".jpg", "");
                        try
                        {
                            int fileNo = Convert.ToInt16(fileNoS);

                            ret = Math.Max(ret, fileNo);
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
            }
        }
        return ret + 1;
    }

    public Bitmap JoinImageByLevel(int level)
    {
        if (CalcColsRows(level))
        {
            int picCount;
            ScanPageLevel pageLevel = LevelColsRows[level];
            MapRectangle mapR = new MapRectangle(pageLevel);

            return ImgHelper.JoinImage(BaseFilePath, GetSampleName(), mapR);
        }

        return null;
    }
}