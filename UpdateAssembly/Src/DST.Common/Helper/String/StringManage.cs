/*----------------------------------------------------------------
 // Copyright (C) 2007 麦迪斯顿(北京)医疗科技发展有限公司
 // 文件名：StringManage.cs
 // 文件功能描述：
 //     字符串处理类
 //
 // 创建标识：
 //     于占涛-2008-02-22
 // 修改标识：
 // 修改描述：
 //
 // 修改标识：
 // 修改描述：
----------------------------------------------------------------*/

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DST.Common.Helper
{
    /// <summary>
    /// 字符串处理
    /// </summary>
    public class StringManage
    {
        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// 加密数据库连接字符串
        /// </summary>
        private const string ENCRYPT_DECRYPT_KEY = "DSTSSPB";

        /*最大的有效全角英文字符转换成int型数据的值*/
        private const int MaxSBCcaseToInt = 65374;
        /*最小的有效全角英文字符转换成int型数据的值*/
        private const int MinSBCcaseToInt = 65281;
        /*对应的全角和半角的差*/
        private const int Margin = 65248;

        /// <summary>
        /// 汉字转拼音缩写
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns>
        public static String GetPYString(String str)
        {
            String TempStr = "";
            foreach (char Chr in str)
            {
                if ((int)Chr >= 33 && (int)Chr <= 126)
                {
                    //字母和符号原样保留
                    TempStr += Chr.ToString();
                }
                else if ((int)Chr == 12288)
                {
                    //将全角空格转换为半角空格
                    TempStr += (char)32;
                }
                else if ((int)Chr > 65280 && (int)Chr < 65375)
                {
                    //将全角符号转换为半角符号
                    TempStr += (char)((int)Chr - 65248);
                }
                else
                {//累加拼音声母
                    TempStr += PinYinConverter.GetFirst(Chr); //GetPYChar(Chr.ToString());
                }
            }
            return TempStr;
        }

        /// <summary>
        /// 汉字转拼音缩写
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <param name="maxLength">转换的最大长度</param>
        /// <returns>拼音缩写</returns>
        public static String GetPYString(String str, int maxLength)
        {
            String TempStr = "";
            TempStr = GetPYString(str);
            if (TempStr.Length > maxLength)
            {
                TempStr = TempStr.Substring(0, str.Length);
            }
            return TempStr;
        }

        /// <summary>
        /// 取单个字符的拼音声母
        /// </summary>
        /// <param name="c">要转换的单个汉字</param>
        /// <returns>拼音声母</returns>
        public static String GetPYChar(String str)
        {
            if (str.CompareTo("吖") < 0) return str;
            if (str.CompareTo("八") < 0) return "A";
            if (str.CompareTo("嚓") < 0) return "B";
            if (str.CompareTo("咑") < 0) return "C";
            if (str.CompareTo("妸") < 0) return "D";
            if (str.CompareTo("发") < 0) return "E";
            if (str.CompareTo("旮") < 0) return "F";
            if (str.CompareTo("铪") < 0) return "G";
            if (str.CompareTo("讥") < 0) return "H";
            if (str.CompareTo("咔") < 0) return "J";
            if (str.CompareTo("垃") < 0) return "K";
            if (str.CompareTo("嘸") < 0) return "L";
            if (str.CompareTo("拏") < 0) return "M";
            if (str.CompareTo("噢") < 0) return "N";
            if (str.CompareTo("妑") < 0) return "O";
            if (str.CompareTo("七") < 0) return "P";
            if (str.CompareTo("亽") < 0) return "Q";
            if (str.CompareTo("仨") < 0) return "R";
            if (str.CompareTo("他") < 0) return "S";
            if (str.CompareTo("哇") < 0) return "T";
            if (str.CompareTo("夕") < 0) return "W";
            if (str.CompareTo("丫") < 0) return "X";
            if (str.CompareTo("帀") < 0) return "Y";
            if (str.CompareTo("咗") < 0) return "Z";
            return str;
        }

        #region 获取字符串的实际字节长度的方法

        /// <summary>
        /// 获取字符串的实际字节长度的方法
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns>实际长度</returns>
        public static int GetRealLength(string source)
        {
            return Encoding.Default.GetByteCount(source);
        }

        #endregion 获取字符串的实际字节长度的方法

        #region 按字节数截取字符串的方法

        /// <summary>
        /// 按字节数截取字符串的方法
        /// </summary>
        /// <param name="source">要截取的字符串</param>
        /// <param name="n">要截取的字节数</param>
        /// <param name="needEndDot">是否需要结尾的省略号</param>
        /// <returns>截取后的字符串</returns>
        public static string SubString(string source, int n, bool needEndDot)
        {
            string Result = string.Empty;
            if (GetRealLength(source) <= n)//如果长度比需要的长度n小,返回原字符串
            {
                return source;
            }
            else
            {
                int j = 0;
                char[] ChrList = source.ToCharArray();
                for (int i = 0; i < ChrList.Length && j < n; i++)
                {
                    if ((int)ChrList[i] > 127)//是否汉字
                    {
                        Result += ChrList[i];
                        j += 2;
                    }
                    else
                    {
                        Result += ChrList[i];
                        j++;
                    }
                }
                if (GetRealLength(Result) > n)
                {
                    Result = Result.Remove(Result.Length - 1, 1);
                }
                if (needEndDot)
                    Result += "...";
                return Result;
            }
        }

        #endregion 按字节数截取字符串的方法

        #region 全角字符串转换为半角字符串

        /// <summary>
        /// 全角转换为半角
        /// </summary>
        /// <param name="originalStr">要进行全角到半角转换的字符串</param>
        /// <param name="start">要进行全角到半角转换的开始位置,不能大于end</param>
        /// <param name="end">要进行全角到半角转换的结束位置,不能小于start</param>
        /// <returns>转换成对应半角的字符串</returns>
        public static String ConvertSBCcaseToDBCcase(string originalStr, int start, int end)
        {
            #region "异常判断"

            if (start < 0 || end < 0)
            {
                return string.Empty;
            }
            if (start > end)
            {
                return string.Empty;
            }
            if (start >= originalStr.Length || end >= originalStr.Length)
            {
                return string.Empty;
            }
            if (originalStr.Length == 0)
            {
                return string.Empty;
            }

            #endregion "异常判断"

            StringBuilder SB = new StringBuilder();
            for (int i = 0; i < originalStr.Length; i++)
            {
                if (i >= start && i <= end)
                {
                    SB.Append(ConvertSBCcaseToDBCcase(originalStr[i]));
                }
                else
                {
                    SB.Append(originalStr[i]);
                }
            }
            return SB.ToString();
        }

        /// <summary>
        /// 将全角字符转换为半角字符
        /// </summary>
        /// <param name="originalChar">要进行全角到半角转换的字符</param>
        /// <returns>全角字符转换为半角后的字符</returns>
        public static char ConvertSBCcaseToDBCcase(char originalChar)
        {
            /*空格是特殊的,其全角和半角的差值也与其他字符不同*/
            if ((int)originalChar == (int)'　')
            {
                return ' ';
            }
            else
            {
                if ((int)originalChar >= MinSBCcaseToInt && (int)originalChar <= MaxSBCcaseToInt)
                {
                    return (char)(originalChar - Margin);
                }
                else
                {
                    return originalChar;
                }
            }
        }

        #endregion 全角字符串转换为半角字符串

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="Source">待加密的串</param>
        /// <returns>经过加密的串</returns>
        public static string Encrypto(string Source)
        {
            byte[] bt = UTF8Encoding.UTF8.GetBytes(Source);//UTF8需要对Text的引用
            MD5CryptoServiceProvider objMD5;
            objMD5 = new MD5CryptoServiceProvider();
            byte[] output = objMD5.ComputeHash(bt);

            string[] password = BitConverter.ToString(output).Split(new char[] { '-' });
            string returnValue = "";
            for (int index = 0; index < password.Length; index++)
                returnValue += password[index];
            returnValue = returnValue.ToUpper();
            return returnValue;
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string DESEncrypt(string encryptString)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(ENCRYPT_DECRYPT_KEY);

                byte[] rgbIV = Keys;

                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);

                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();

                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);

                cStream.Write(inputByteArray, 0, inputByteArray.Length);

                cStream.FlushFinalBlock();

                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        ///  DES解密方法
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DESDecrypt(string decryptString)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(ENCRYPT_DECRYPT_KEY);

                byte[] rgbIV = Keys;

                byte[] inputByteArray = Convert.FromBase64String(decryptString);

                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();

                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);

                cStream.Write(inputByteArray, 0, inputByteArray.Length);

                cStream.FlushFinalBlock();

                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        /// <summary>
        /// 获取字符串左边所有数值字符串
        /// </summary>
        /// <param name="sourec">需要获取字符串</param>
        /// <param name="subCount">如果第一个字符不是数值则左侧截取个数</param>
        /// <returns>数值字符串</returns>
        public static string GetLeftNumString(string source, int subCount)
        {
            string Result = string.Empty;
            if (string.IsNullOrEmpty(source))
            {
                return Result;
            }
            int Temp;
            foreach (char Chr in source)
            {
                if (int.TryParse(Chr.ToString(), out Temp))
                {
                    Result += Chr.ToString();
                }
                else
                {
                    break;
                }
            }
            if (Result.Length == 0)
            {
                if (source.Length < subCount)
                {
                    Result = source;
                }
                else
                {
                    Result = source.Substring(0, subCount);
                }
            }
            return Result;
        }
    }
}