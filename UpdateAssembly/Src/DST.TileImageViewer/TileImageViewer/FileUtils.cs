using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// 文件工具类
/// </summary>
internal class FileUtils
{
    public static void DirFiles(string dir, List<string> list)
    {
        DirectoryInfo d = new DirectoryInfo(dir);
        FileInfo[] files = d.GetFiles();//文件
        DirectoryInfo[] directs = d.GetDirectories();//文件夹
        foreach (FileInfo f in files)
        {
            list.Add(f.Name);//添加文件名到列表中
        }
        //获取子文件夹内的文件列表，递归遍历
        foreach (DirectoryInfo dd in directs)
        {
            DirFiles(dd.FullName, list);
        }
    }

    /// <summary>
    /// 修改INI配置文件
    /// </summary>
    /// <param name="section">段落</param>
    /// <param name="key">关键字</param>
    /// <param name="val">值</param>
    /// <param name="filepath">文件完整路径</param>
    /// <returns></returns>
    [DllImport("kernel32")]
    public static extern long WritePrivateProfileString(string section, string key, string val, string filepath);

    /// <summary>
    /// 读INI配置文件
    /// </summary>
    /// <param name="section"></param>
    /// <param name="key"></param>
    /// <param name="def">缺省值</param>
    /// <param name="retval"></param>
    /// <param name="size">指定装载到lpReturnedString缓冲区的最大字符数量</param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    [DllImport("kernel32")]
    public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);

    [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
    private static extern uint GetPrivateProfileStringArr(string section, string key,
            string def, Byte[] retVal, int size, string filePath);

    public static List<string> ReadSections(string iniFilename)
    {
        List<string> result = new List<string>();
        Byte[] buf = new Byte[65536];
        uint len = GetPrivateProfileStringArr(null, null, null, buf, buf.Length, iniFilename);
        int j = 0;
        for (int i = 0; i < len; i++)
            if (buf[i] == 0)
            {
                result.Add(Encoding.Default.GetString(buf, j, i - j));
                j = i + 1;
            }
        return result;
    }

    public static List<string> ReadKeys(string SectionName, string iniFilename)
    {
        List<string> result = new List<string>();
        Byte[] buf = new Byte[65536];
        uint len = GetPrivateProfileStringArr(SectionName, null, null, buf, buf.Length, iniFilename);
        int j = 0;
        for (int i = 0; i < len; i++)
            if (buf[i] == 0)
            {
                result.Add(Encoding.Default.GetString(buf, j, i - j));
                j = i + 1;
            }
        return result;
    }
}