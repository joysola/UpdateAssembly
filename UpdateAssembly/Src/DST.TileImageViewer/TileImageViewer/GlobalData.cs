using System;

/// <summary>
/// 全局静态数据
/// </summary>
public class GlobalData
{
    //语言
    public static Language GlobalLanguage;

    public static void InitLanguage()
    {
        if (GlobalLanguage == null)
        {
            String langText = Constants.SettingDetail.Language;
            if (String.IsNullOrEmpty(langText))
            {
                GlobalLanguage = Language.DefaultLanguage();
                Constants.SettingDetail.Language = GlobalLanguage.LanguagText;
            }
            else
            {
                GlobalLanguage = new Language(langText);
            }
        }
    }
}