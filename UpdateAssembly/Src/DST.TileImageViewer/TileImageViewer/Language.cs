using System;
using System.Collections.Generic;
using System.Xml;

/// <summary>
/// 多语言处理类
/// </summary>
public class Language
{
    protected Dictionary<string, string> DicLanguage = new Dictionary<string, string>();

    public String LanguagText;

    public Language(String lang)
    {
        LanguagText = lang;
        XmlLoad(LanguagText);
    }

    /// <summary>
    /// 读取XML放到内存
    /// </summary>
    /// <param name="language"></param>
    protected void XmlLoad(string language)
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            System.Resources.ResourceManager rm =
                new System.Resources.ResourceManager("DST.TileImageViewer.Properties.Resources", this.GetType().Assembly);
            //文件引用指向“工程路径\language\*.xml ”
            string address = rm.GetString(language);
            doc.LoadXml(address);
            XmlElement root = doc.DocumentElement;

            XmlNodeList nodeLst1 = root.ChildNodes;
            foreach (XmlNode item in nodeLst1)
            {
                DicLanguage.Add(item.Name, item.InnerText);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public String Text(String key)
    {
        if (!DicLanguage.ContainsKey(key))
        {
            return key;
        }
        String aa = DicLanguage[key];
        if (LanguagText.Equals("English"))
        {
            aa = StrHelper.UpperEveryWord(aa);
        }

        return aa.Trim();
    }

    public static Language DefaultLanguage()
    {
        String lang = "English";

        if (IsChineseSimple())
        {
            lang = "Chinese";
        }
        return new Language(lang);
    }

    private static bool IsChineseSimple()
    {
        return System.Threading.Thread.CurrentThread.CurrentCulture.Name == "zh-CN";
    }

    private static bool IsEnglish()
    {
        return System.Threading.Thread.CurrentThread.CurrentCulture.Name == "en-US";
    }
}