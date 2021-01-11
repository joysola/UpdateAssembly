using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DST.Common.ZipHelper
{
    public class ZipHelper
    {
        private static object locker = new object();

        /// <summary>
        /// 递归压缩文件夹的内部方法
        /// </summary>
        /// <param name="folderToZip">要压缩的文件夹路径</param>
        /// <param name="zipStream">压缩输出流</param>
        /// <param name="parentFolderName">此文件夹的上级文件夹</param>
        private static async Task<bool> ZipDirectory(string folderToZip, ZipOutputStream zipStream, string parentFolderName, Action<long> zipPerCallback = null)
        {
            bool result = true;
            string[] folders, files;
            ZipEntry ent = null;
            FileStream fs = null;
            Crc32 crc = new Crc32();

            try
            {
                ent = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/"));
                zipStream.PutNextEntry(ent);
                zipStream.Flush();
                files = Directory.GetFiles(folderToZip);
                foreach (string file in files)
                {
                    fs = File.OpenRead(file);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    ent = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/" + Path.GetFileName(file)));
                    ent.DateTime = DateTime.Now;
                    ent.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    ent.Crc = crc.Value;
                    zipStream.PutNextEntry(ent);
                    zipStream.Write(buffer, 0, buffer.Length);

                    if (zipPerCallback != null)
                    {
                        zipPerCallback(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logger.Error("生成ZIP文件错误(ZipHelper.ZipDirectory)：" + ex.Message);
                result = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (ent != null)
                {
                    ent = null;
                }
                GC.Collect();
                GC.Collect(1);
            }

            folders = Directory.GetDirectories(folderToZip);
            foreach (string folder in folders)
            {
                DirectoryInfo tmpDir = new DirectoryInfo(folder);
                if (await ZipDirectory(folder, zipStream, tmpDir.Parent.Name, zipPerCallback).ConfigureAwait(false) == false)
                {
                    return false;
                }
            }

            return result;
        }

        /// <summary>
        /// 压缩文件夹，包含密码信息
        /// </summary>
        /// <param name="folderToZip">要压缩的文件夹路径</param>
        /// <param name="zipedFile">压缩文件完整路径</param>
        /// <param name="password">密码</param>
        /// <returns>是否压缩成功</returns>
        public static async Task<bool> ZipDirectoryContionuePwd(string folderToZip, string zipedFile, string password, Action<long> zipPerCallback = null)
        {
            bool result = false;
            if (!Directory.Exists(folderToZip))
            {
                Directory.CreateDirectory(folderToZip);
            }

            ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedFile));
            zipStream.SetLevel(9);
            if (!string.IsNullOrEmpty(password))
            {
                zipStream.Password = password;
            }

            result = await ZipDirectory(folderToZip, zipStream, "", zipPerCallback).ConfigureAwait(false);
            zipStream.Finish();
            zipStream.Close();
            return result;
        }

        /// <summary>
        /// 压缩文件夹，使用进程锁，只能一次生成一个zip文件
        /// </summary>
        /// <param name="folderToZip">要压缩的文件夹路径</param>
        /// <param name="zipedFile">压缩文件完整路径</param>
        /// <param name="zipPerCallback">压缩文件进度回调</param>
        /// <returns>是否压缩成功</returns>
        public static async Task<bool> ZipDirectory(string folderToZip, string zipedFile, Action<long> zipPerCallback = null)
        {
            lock (locker)
            {
                bool result = ZipDirectoryContionuePwd(folderToZip, zipedFile, null, zipPerCallback).Result;
                return result;
            }
        }

        /// <summary>
        /// 获取文件夹下的文件数量和整个文件大小（包含子目录）
        /// </summary>
        /// <param name="directory">文件夹完整路径</param>
        /// <returns>返回long数组[文件总数量, 文件总大小]</returns>
        public static async Task<long[]> GetFileCountFromDirectory(string directory)
        {
            long[] result = new long[2];
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            // 获取当前路径下的文件个数
            FileInfo[] files = dirInfo.GetFiles();
            result[0] += files.Length;

            foreach (FileInfo file in files)
            {
                result[1] += file.Length;
            }

            // 获取当前路径下子目录下的文件个数
            DirectoryInfo[] dirArr = dirInfo.GetDirectories();
            long[] tmp = new long[2];
            foreach (DirectoryInfo tmpDir in dirArr)
            {
                tmp = await GetFileCountFromDirectory(tmpDir.FullName);
                result[0] += tmp[0];
                result[1] += tmp[1];
            }

            return result;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件全名</param>
        /// <param name="zipedFile">压缩后的文件名</param>
        /// <param name="password">密码</param>
        /// <returns>压缩结果</returns>
        public static async Task<bool> ZipFile(string fileToZip, string zipedFile, string password)
        {
            bool result = true;
            ZipOutputStream zipStream = null;
            FileStream fs = null;
            ZipEntry ent = null;

            if (!File.Exists(fileToZip))
            {
                return false;
            }

            try
            {
                fs = File.OpenRead(fileToZip);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                fs = File.Create(zipedFile);
                zipStream = new ZipOutputStream(fs);
                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                ent = new ZipEntry(Path.GetFileName(fileToZip));
                zipStream.PutNextEntry(ent);
                zipStream.SetLevel(6);
                zipStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Logger.Logger.Error("生成ZIP文件错误(ZipHelper.ZipFile)：" + ex.Message);
                result = false;
            }
            finally
            {
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
                if (ent != null)
                {
                    ent = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }

            GC.Collect();
            GC.Collect(1);
            return result;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件全名</param>
        /// <param name="zipedFile">压缩后的文件名</param>
        /// <returns>压缩结果</returns>
        public static async Task<bool> ZipFile(string fileToZip, string zipedFile)
        {
            bool result = await ZipFile(fileToZip, zipedFile, null).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 压缩文件或文件夹
        /// </summary>
        /// <param name="fileToZip">要压缩的路径</param>
        /// <param name="zipedFile">压缩后的文件名</param>
        /// <param name="password">密码</param>
        /// <returns>压缩结果</returns>
        public static async Task<bool> Zip(string fileToZip, string zipedFile, string password)
        {
            bool result = false;
            if (Directory.Exists(fileToZip))
            {
                result = await ZipDirectoryContionuePwd(fileToZip, zipedFile, password).ConfigureAwait(false);
            }
            else if (File.Exists(fileToZip))
            {
                result = await ZipFile(fileToZip, zipedFile, password).ConfigureAwait(false);
            }

            return result;
        }

        /// <summary>
        /// 压缩文件或文件夹
        /// </summary>
        /// <param name="fileToZip">要压缩的路径</param>
        /// <param name="zipedFile">压缩后的文件名</param>
        /// <returns>压缩结果</returns>
        public static async Task<bool> Zip(string fileToZip, string zipedFile)
        {
            bool result = await Zip(fileToZip, zipedFile, null).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// 解压缩包（将压缩包解压到指定目录）
        /// </summary>
        /// <param name="zipedFileName">压缩包名称</param>
        /// <param name="unZipDirectory">解压缩目录</param>
        /// <param name="password">密码</param>
        public static async Task<bool> UnZipFiles(string zipedFileName, string unZipDirectory, string password = "")
        {
            FileStream fs = null;
            try
            {
                if (!File.Exists(zipedFileName))
                {
                    return false;
                }

                if (!Directory.Exists(unZipDirectory))
                {
                    Directory.CreateDirectory(unZipDirectory);
                }

                using (ZipInputStream zis = new ZipInputStream(File.Open(zipedFileName, FileMode.OpenOrCreate)))
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        zis.Password = password;//有加密文件的，可以设置密码解压
                    }

                    ZipEntry zipEntry;
                    while ((zipEntry = zis.GetNextEntry()) != null)
                    {
                        string directoryName = unZipDirectory;
                        string pathName = Path.GetDirectoryName(zipEntry.Name);
                        string fileName = Path.GetFileName(zipEntry.Name);
                        pathName = pathName.Replace(".", "$");
                        directoryName += "\\" + pathName;

                        if (!Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }

                        if (!string.IsNullOrEmpty(fileName))
                        {
                            fs = File.Create(Path.Combine(directoryName, fileName));
                            int size = 2048;
                            byte[] bytes = new byte[2048];
                            while (true)
                            {
                                size = zis.Read(bytes, 0, bytes.Length);
                                if (size > 0)
                                {
                                    fs.Write(bytes, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            fs.Close();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Logger.Error("解压ZIP文件错误(ZipHelper.UnZipFiles)：" + ex.Message);
                return false;
            }
            finally
            {
                fs.Dispose();
                fs.Close();
                GC.Collect();
                GC.Collect(1);
            }
        }
    }
}