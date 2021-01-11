using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace DST.Common.Helper
{
    public class FileHelper
    {
        [Serializable]
        public class Entity
        {
            private Dictionary<string, byte[]> _files = new Dictionary<string, byte[]>();

            public Dictionary<string, byte[]> Files
            {
                get
                {
                    return _files;
                }
            }

            public void AddFile(string fileName)
            {
                _files.Add(fileName, FileHelper.GetFileData(fileName));
            }

            public void AddFile(string fileName, byte[] fileContext)
            {
                _files.Add(fileName, fileContext);
            }
        }

        public static string GetFilePath(string fileName)
        {
            string path = fileName;
            if (path.Contains(@"\"))
            {
                int pos = path.LastIndexOf(@"\");
                path = path.Remove(pos + 1);
            }
            if (!path.EndsWith(@"\")) path += @"\";
            return path;
        }

        /// <summary>
        /// 获取文件名（不包含路径）
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutPath(string fileName)
        {
            string path = fileName;
            if (path.Contains(@"\"))
            {
                int pos = path.LastIndexOf(@"\");
                path = path.Remove(0, pos + 1);
            }
            return path;
        }

        /// <summary>
        /// 获取文件名（不包含路径和扩展名）
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutPathAndExt(string fileName)
        {
            string path = GetFileNameWithoutPath(fileName);
            if (path.Contains("."))
            {
                int pos = path.LastIndexOf(".");
                path = path.Remove(pos);
            }
            return path;
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] result = new byte[stream.Length];
            stream.Read(result, 0, (int)stream.Length);
            return result;
        }

        public static byte[] GetFileData(string fileName)
        {
            System.Diagnostics.Stopwatch aa = new System.Diagnostics.Stopwatch();
            aa.Start();
            FileStream fs = new FileStream(fileName, FileMode.Open);
            byte[] result = StreamToBytes(fs);
            fs.Close();
            aa.Stop();
            Debug.WriteLine(string.Format("FileHelper.GetFileData {0} MS", aa.ElapsedMilliseconds / 1000));
            return result;
        }

        public static MemoryStream ImageToStream(Image image)
        {
            MemoryStream stream = new MemoryStream();
            image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            stream.Position = 0;
            stream.Capacity = (int)stream.Length;
            return stream;
        }

        public static void CutFiles(string sourceFile, string destPath)
        {
            string[] allLines = File.ReadAllLines(sourceFile);
            if (allLines != null && allLines.Length > 0)
            {
                FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                allLines = allLines[allLines.Length - 1].Split(',');
                for (int i = 0; i < allLines.Length; i += 2)
                {
                    string destinationFile = destPath + allLines[i];
                    byte[] buffer = new byte[long.Parse(allLines[i + 1])];
                    sourceStream.Read(buffer, 0, buffer.Length);
                    FileStream destinationStream = new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write);
                    destinationStream.Write(buffer, 0, buffer.Length);
                    destinationStream.Close();
                }
            }
        }

        public static void WriteStreamToFile(Stream stream, string fileName)
        {
            WriteBufferToFile(StreamToBytes(stream), fileName);
        }

        public static void WriteBufferToFile(byte[] buffer, string fileName)
        {
            while (File.Exists(fileName))
            {
                File.Delete(fileName);
                System.Windows.Forms.Application.DoEvents();
            }
            FileStream destinationStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            destinationStream.Write(buffer, 0, buffer.Length);
            destinationStream.Close();
        }
    }
}