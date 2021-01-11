/*----------------------------------------------------------------
 // Copyright (C) 2007 ���˹��(����)ҽ�ƿƼ���չ���޹�˾
 // �ļ�����StringManage.cs
 // �ļ�����������
 //     �ַ���������
 //
 // ������ʶ��
 //     ��ռ��-2008-02-22
 // �޸ı�ʶ��
 // �޸�������
 //
 // �޸ı�ʶ��
 // �޸�������
----------------------------------------------------------------*/

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DST.Common.Helper
{
    /// <summary>
    /// �ַ�������
    /// </summary>
    public class StringManage
    {
        //Ĭ����Կ����
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// �������ݿ������ַ���
        /// </summary>
        private const string ENCRYPT_DECRYPT_KEY = "DSTSSPB";

        /*������Чȫ��Ӣ���ַ�ת����int�����ݵ�ֵ*/
        private const int MaxSBCcaseToInt = 65374;
        /*��С����Чȫ��Ӣ���ַ�ת����int�����ݵ�ֵ*/
        private const int MinSBCcaseToInt = 65281;
        /*��Ӧ��ȫ�ǺͰ�ǵĲ�*/
        private const int Margin = 65248;

        /// <summary>
        /// ����תƴ����д
        /// </summary>
        /// <param name="str">Ҫת���ĺ����ַ���</param>
        /// <returns>ƴ����д</returns>
        public static String GetPYString(String str)
        {
            String TempStr = "";
            foreach (char Chr in str)
            {
                if ((int)Chr >= 33 && (int)Chr <= 126)
                {
                    //��ĸ�ͷ���ԭ������
                    TempStr += Chr.ToString();
                }
                else if ((int)Chr == 12288)
                {
                    //��ȫ�ǿո�ת��Ϊ��ǿո�
                    TempStr += (char)32;
                }
                else if ((int)Chr > 65280 && (int)Chr < 65375)
                {
                    //��ȫ�Ƿ���ת��Ϊ��Ƿ���
                    TempStr += (char)((int)Chr - 65248);
                }
                else
                {//�ۼ�ƴ����ĸ
                    TempStr += PinYinConverter.GetFirst(Chr); //GetPYChar(Chr.ToString());
                }
            }
            return TempStr;
        }

        /// <summary>
        /// ����תƴ����д
        /// </summary>
        /// <param name="str">Ҫת���ĺ����ַ���</param>
        /// <param name="maxLength">ת������󳤶�</param>
        /// <returns>ƴ����д</returns>
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
        /// ȡ�����ַ���ƴ����ĸ
        /// </summary>
        /// <param name="c">Ҫת���ĵ�������</param>
        /// <returns>ƴ����ĸ</returns>
        public static String GetPYChar(String str)
        {
            if (str.CompareTo("߹") < 0) return str;
            if (str.CompareTo("��") < 0) return "A";
            if (str.CompareTo("��") < 0) return "B";
            if (str.CompareTo("��") < 0) return "C";
            if (str.CompareTo("��") < 0) return "D";
            if (str.CompareTo("��") < 0) return "E";
            if (str.CompareTo("�") < 0) return "F";
            if (str.CompareTo("��") < 0) return "G";
            if (str.CompareTo("��") < 0) return "H";
            if (str.CompareTo("��") < 0) return "J";
            if (str.CompareTo("��") < 0) return "K";
            if (str.CompareTo("�`") < 0) return "L";
            if (str.CompareTo("��") < 0) return "M";
            if (str.CompareTo("��") < 0) return "N";
            if (str.CompareTo("�r") < 0) return "O";
            if (str.CompareTo("��") < 0) return "P";
            if (str.CompareTo("��") < 0) return "Q";
            if (str.CompareTo("��") < 0) return "R";
            if (str.CompareTo("��") < 0) return "S";
            if (str.CompareTo("��") < 0) return "T";
            if (str.CompareTo("Ϧ") < 0) return "W";
            if (str.CompareTo("Ѿ") < 0) return "X";
            if (str.CompareTo("��") < 0) return "Y";
            if (str.CompareTo("��") < 0) return "Z";
            return str;
        }

        #region ��ȡ�ַ�����ʵ���ֽڳ��ȵķ���

        /// <summary>
        /// ��ȡ�ַ�����ʵ���ֽڳ��ȵķ���
        /// </summary>
        /// <param name="source">�ַ���</param>
        /// <returns>ʵ�ʳ���</returns>
        public static int GetRealLength(string source)
        {
            return Encoding.Default.GetByteCount(source);
        }

        #endregion ��ȡ�ַ�����ʵ���ֽڳ��ȵķ���

        #region ���ֽ�����ȡ�ַ����ķ���

        /// <summary>
        /// ���ֽ�����ȡ�ַ����ķ���
        /// </summary>
        /// <param name="source">Ҫ��ȡ���ַ���</param>
        /// <param name="n">Ҫ��ȡ���ֽ���</param>
        /// <param name="needEndDot">�Ƿ���Ҫ��β��ʡ�Ժ�</param>
        /// <returns>��ȡ����ַ���</returns>
        public static string SubString(string source, int n, bool needEndDot)
        {
            string Result = string.Empty;
            if (GetRealLength(source) <= n)//������ȱ���Ҫ�ĳ���nС,����ԭ�ַ���
            {
                return source;
            }
            else
            {
                int j = 0;
                char[] ChrList = source.ToCharArray();
                for (int i = 0; i < ChrList.Length && j < n; i++)
                {
                    if ((int)ChrList[i] > 127)//�Ƿ���
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

        #endregion ���ֽ�����ȡ�ַ����ķ���

        #region ȫ���ַ���ת��Ϊ����ַ���

        /// <summary>
        /// ȫ��ת��Ϊ���
        /// </summary>
        /// <param name="originalStr">Ҫ����ȫ�ǵ����ת�����ַ���</param>
        /// <param name="start">Ҫ����ȫ�ǵ����ת���Ŀ�ʼλ��,���ܴ���end</param>
        /// <param name="end">Ҫ����ȫ�ǵ����ת���Ľ���λ��,����С��start</param>
        /// <returns>ת���ɶ�Ӧ��ǵ��ַ���</returns>
        public static String ConvertSBCcaseToDBCcase(string originalStr, int start, int end)
        {
            #region "�쳣�ж�"

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

            #endregion "�쳣�ж�"

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
        /// ��ȫ���ַ�ת��Ϊ����ַ�
        /// </summary>
        /// <param name="originalChar">Ҫ����ȫ�ǵ����ת�����ַ�</param>
        /// <returns>ȫ���ַ�ת��Ϊ��Ǻ���ַ�</returns>
        public static char ConvertSBCcaseToDBCcase(char originalChar)
        {
            /*�ո��������,��ȫ�ǺͰ�ǵĲ�ֵҲ�������ַ���ͬ*/
            if ((int)originalChar == (int)'��')
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

        #endregion ȫ���ַ���ת��Ϊ����ַ���

        /// <summary>
        /// ���ܷ���
        /// </summary>
        /// <param name="Source">�����ܵĴ�</param>
        /// <returns>�������ܵĴ�</returns>
        public static string Encrypto(string Source)
        {
            byte[] bt = UTF8Encoding.UTF8.GetBytes(Source);//UTF8��Ҫ��Text������
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
        /// DES����
        /// </summary>
        /// <param name="encryptString">�����ܵ��ַ���</param>
        /// <returns>���ܳɹ����ؼ��ܺ���ַ�����ʧ�ܷ���Դ��</returns>
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
        ///  DES���ܷ���
        /// </summary>
        /// <param name="decryptString">�����ܵ��ַ���</param>
        /// <param name="decryptKey">������Կ,Ҫ��Ϊ8λ,�ͼ�����Կ��ͬ</param>
        /// <returns>���ܳɹ����ؽ��ܺ���ַ�����ʧ�ܷ�Դ��</returns>
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
        /// ��ȡ�ַ������������ֵ�ַ���
        /// </summary>
        /// <param name="sourec">��Ҫ��ȡ�ַ���</param>
        /// <param name="subCount">�����һ���ַ�������ֵ������ȡ����</param>
        /// <returns>��ֵ�ַ���</returns>
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