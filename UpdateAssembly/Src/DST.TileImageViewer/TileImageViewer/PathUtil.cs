using Microsoft.Win32;
using System.IO;

/// <summary>
/// 路径处理工具类
/// </summary>
public class PathUtil
{
    //windows当前用户的注册表键
    private static RegistryKey folders;

    /// <summary>
    /// 全局指定当前获取注册表的根节点
    /// 一般只有管理员身份运行，才能操作Registry.LocalMachine 对应的文件
    /// </summary>
    public static RegistryKey RootKey { get; set; }

    static PathUtil()
    {
        SetAsCurrentUser();
    }

    /// <summary>
    /// 设置根节点为LocalMachine
    /// </summary>
    public static void SetAsLocalMachine()
    {
        RootKey = Registry.LocalMachine;
        folders = OpenRegistryKey(Registry.LocalMachine, @"/software/microsoft/windows/currentversion/explorer/shell folders");
    }

    /// <summary>
    /// 设置根节点为LocalMachine
    /// </summary>
    public static void SetAsCurrentUser()
    {
        RootKey = Registry.CurrentUser;
        folders = OpenRegistryKey(Registry.CurrentUser, @"/software/microsoft/windows/currentversion/explorer/shell folders");
    }

    #region 当前用户路径

    /// <summary>
    /// windows用户字体目录路径
    /// </summary>
    public static string FontsPath
    {
        get { return GetPath("Fonts"); }
    }

    /// <summary>
    /// windows用户网络邻居路径
    /// </summary>
    public static string NetHoodPath
    {
        get { return GetPath("Nethood"); }
    }

    /// <summary>
    /// windows用户我的文档路径
    /// </summary>
    public static string PersonalPath
    {
        get { return GetPath("Personal"); }
    }

    /// <summary>
    /// windows用户最近访问文档快捷方式目录
    /// </summary>
    public static string RecentPath
    {
        get { return GetPath("Recent"); }
    }

    /// <summary>
    /// windows用户发送到目录路径
    /// </summary>
    public static string SendToPath
    {
        get { return GetPath("Sendto"); }
    }

    /// <summary>
    /// windows用户收藏夹目录路径
    /// </summary>
    public static string FavoritesPath
    {
        get { return GetPath("Favorites"); }
    }

    /// <summary>
    /// windows用户网页历史目录路径
    /// </summary>
    public static string HistoryPath
    {
        get { return GetPath("History"); }
    }

    /// <summary>
    /// windows用户cookies目录路径
    /// </summary>
    public static string CookiePath
    {
        get { return GetPath("Cookies"); }
    }

    /// <summary>
    /// windows用户Cache目录路径
    /// </summary>
    public static string CachePath
    {
        get { return GetPath("Cache"); }
    }

    #endregion 当前用户路径

    #region //系统路径

    /// <summary>
    /// widnows用户桌面路径
    /// </summary>
    public static string DesktopPath
    {
        get { return GetPath("Desktop"); }
    }

    /// <summary>
    /// windows用户开始菜单程序路径
    /// </summary>
    public static string ProgramsPath
    {
        get { return GetPath("Programs"); }
    }

    /// <summary>
    /// windows用户开始菜单目录路径
    /// </summary>
    public static string StartMenuPath
    {
        get { return GetPath("StartMenu"); }
    }

    /// <summary>
    /// windows用户开始菜单启动项目路径
    /// </summary>
    public static string StartupPath
    {
        get { return GetPath("Startup"); }
    }

    /// <summary>
    /// windows用户应用程序数据目录
    /// </summary>
    public static string AppdataPath
    {
        get { return GetPath("Appdata"); }
    }

    /// <summary>
    /// 公共文档
    /// </summary>
    public static string Documents
    {
        get { return GetPath("Documents"); }
    }

    #endregion //系统路径

    /// <summary>
    /// 当前应用程序的工作目录，不是程序文件目录
    /// </summary>
    public static string CurrentProgramPath
    {
        get { return Directory.GetCurrentDirectory(); }
    }

    /// <summary>
    /// 当前应用程序解决方案路径
    /// </summary>
    public static string CurrentSolutionPath
    {
        get
        {
            string program = CurrentProgramPath;
            DirectoryInfo info = new DirectoryInfo(program);
            return info.Parent.Parent.FullName;
        }
    }

    #region //私有方法

    /// <summary>
    /// 获取键值对应的文件夹
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>值</returns>
    private static string GetPath(string key)
    {
        if (RootKey == Registry.LocalMachine)
            key = "Common " + key;
        string path = folders.GetValue(key).ToString();
        if (!string.IsNullOrEmpty(path))
        {
            if (Directory.Exists(path))
            {
                return path;
            }
        }
        return "'" + key + "'对应的文件夹不存在";
    }

    //打开，指定根节点和路径的注册表项
    private static RegistryKey OpenRegistryKey(RegistryKey root, string str)
    {
        str = str.Remove(0, 1) + @"/";
        while (str.IndexOf(@"/") != -1)
        {
            root = root.OpenSubKey(str.Substring(0, str.IndexOf(@"/")));
            str = str.Remove(0, str.IndexOf(@"/") + 1);
        }
        return root;
    }

    #endregion //私有方法
}