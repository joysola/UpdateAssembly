public class StrHelper
{
    /// <summary>
    /// 使用驼峰法处理一段英文
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string UpperEveryWord(string str)
    {
        string[] strArray = str.Split(" ".ToCharArray());
        string result = string.Empty;//定义一个空字符串
        foreach (string s in strArray)//循环处理数组里面每一个字符串
        {
            result += s.Substring(0, 1).ToUpper() + s.Substring(1) + " ";//.Substring(0, 1).ToUpper()把循环到的字符串第一个字母截取并转换为大写，并用s.Substring(1)得到循环到的字符串除第一个字符后的所有字符拼装到首字母后面。
        }
        return result;
    }
}