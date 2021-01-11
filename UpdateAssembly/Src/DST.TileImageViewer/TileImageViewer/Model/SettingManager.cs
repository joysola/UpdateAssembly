using System.Collections.Generic;

namespace TileImageViewer.Model
{
    /// <summary>
    /// 设置数据表管理器
    /// </summary>
    internal class SettingManager : DbContext<Setting>
    {
        private Setting DefaultSetting = new Setting
        {
            // 浏览Tab
            EnlargeSelect = 1,
            EnlargeRelationVlaue = 2,
            EnlargeAbsoluteVlaue = 1,
            ApplyZoomLimitSwitch = 0,

            SmoothSlideNavigationSwitch = 1,
            LabelOrientationSwitch = 1,
            ScaleBarSwitch = 1,
            ScaleBarColor = "Yellow",

            // 注释尺寸Tab
            RectAngleWidth = 2.50m,
            RectAngleHeight = 2.50m,
            RectAngleUnit = 1,
            CircularRadius = 2.40m,
            CircularUnit = 1,
            //CircularTwentyRadius = int.Parse(textBox3.Text),
            //CircularFortyRadius = int.Parse(textBox4.Text),
            CircularTwentyRadius = 2.50m,
            CircularFortyRadius = 2.50m,

            // 注释属性Tab
            ShowNameSwitch = 1,
            AutoNameSwitch = 1,
            AutoNumSwitch = 1,
            PrefixStr = "Annotation",
            TMAPrefixStr = "TMA",
            ShowAnnotationSizeSwitch = 1,
            SlicesFontSize = 14,
            AnnotationLabelSize = 14,
            BrightfieldColor = "#0080FF",
            FluorescentColor = "Aqua",

            // 高级设置Tab
            ShowDebugInfoSwitch = 0,
            MemCachedSwitch = 0,
            ReverseMouseSwitch = 0
        };

        private static SettingManager stInstance = new SettingManager();

        public SettingManager()
        {
        }

        public static SettingManager getInstance()
        {
            if (stInstance == null)
            {
                stInstance = new SettingManager();
            }
            return stInstance;
        }

        public Setting getSettingDetail()
        {
            Setting stPer = new Setting();
            //根据模型建表
            Db.CodeFirst.SetStringDefaultLength(200/*设置varchar默认长度为200*/).InitTables(typeof(Setting));
            List<Setting> stAll = this.GetList();
            if (stAll.Count > 0)
            {
                stPer = stAll[stAll.Count - 1];
            }
            else
            {
                stPer = this.DefaultSetting;
            }
            return stPer;
        }

        public int UpdateToDb(Setting st) //add
        {
            int id = 0;

            //根据模型建表
            Db.CodeFirst.SetStringDefaultLength(200/*设置varchar默认长度为200*/).InitTables(typeof(Setting));
            Db.Saveable(st).ExecuteReturnEntity();

            return id;
        }
    }
}