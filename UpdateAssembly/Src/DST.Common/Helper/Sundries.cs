using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DST.Common.Helper
{
    public class Sundries
    {
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

        public static string EncodeWithString(Stream stream)
        {
            byte[] binaryData = FileHelper.StreamToBytes(stream);
            return System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
        }

        public static string EncodeWithString(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            string result = EncodeWithString(fs);
            fs.Close();
            return result;
        }

        public static string EncodeString(string source)
        {
            byte[] binaryData = StringHelper.Str2Arr(source);
            return System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
        }

        public static string DecodeString(string base64String)
        {
            byte[] binaryData;
            binaryData = System.Convert.FromBase64String(base64String);
            return StringHelper.Arr2Str(binaryData);
        }

        public static Stream DecodeWithString(string base64String)
        {
            byte[] binaryData;
            binaryData = System.Convert.FromBase64String(base64String);
            Stream stream = new MemoryStream(binaryData);
            return stream;
        }
    }
}