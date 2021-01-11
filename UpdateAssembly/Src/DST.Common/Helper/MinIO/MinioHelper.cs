using Minio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace DST.Common.MinioHelper
{
    /// <summary>
    /// Minio帮助类
    /// </summary>
    public class MinioHelper
    {
        /// <summary>
        /// minio客户端
        /// </summary>
        private MinioClient minioClient = null;

        /// <summary>
        /// Minio访问客户端
        /// </summary>
        public static MinioHelper Client => new MinioHelper();

        /// <summary>
        /// minio服务器地址，不需要加http,也不要以"/"结尾
        /// </summary>
        public static Lazy<string> Endpoint { get; } = new Lazy<string>(() =>
        {
            var result = ConfigurationManager.AppSettings["MinIO_Endpoint"];
            if (string.IsNullOrEmpty(result))
            {
                result = "image02-sz.deepsight.cloud";
            }
            return result;
        });

        /// <summary>
        /// minio授权登录账号
        /// </summary>
        public static Lazy<string> AccessKey { get; } = new Lazy<string>(() =>
        {
            var result = ConfigurationManager.AppSettings["MinIO_AccessKey"];
            if (string.IsNullOrEmpty(result))
            {
                result = "minio";
            }
            return result;
        });

        /// <summary>
        /// minio授权登录密码
        /// </summary>
        public static Lazy<string> SecretKey { get; } = new Lazy<string>(() =>
        {
            var result = ConfigurationManager.AppSettings["MinIO_SecretKey"];
            if (string.IsNullOrEmpty(result))
            {
                result = "deepsight0110";
            }
            return result;
        });

        /// <summary>
        /// 单例构造
        /// </summary>
        private MinioHelper()
        {
            // 如果Endpoint是https，则需要加 WithSSL(), 不是则不需要
            this.minioClient = new MinioClient(Endpoint.Value, AccessKey.Value, SecretKey.Value).WithSSL();
        }

        /// <summary>
        /// 上传文件并获取objectName
        /// </summary>
        /// <param name="uploadFileFullName">上传文件的完整路径和文件名称</param>
        /// <param name="bucketName">Minio桶名称</param>
        /// <param name="contentType">文件的Content type，默认是"application/octet-stream"</param>
        /// <param name="bucketChildName">Minio桶下的子目录,示例 "first/"</param>
        /// <returns>返回minio的url链接地址</returns>
        public async Task<string> UploadFile(string uploadFileFullName, string bucketName, string contentType = "application/octet-stream", string bucketChildName = "", Action<int> processCallBack = null)
        {
            string result = string.Empty;
            try
            {
                if (!File.Exists(uploadFileFullName))
                {
                    return result;
                }

                // 判断桶是否存在
                if (!await this.minioClient.BucketExistsAsync(bucketName).ConfigureAwait(false))
                {
                    // 创建桶
                    await this.minioClient.MakeBucketAsync(bucketName).ConfigureAwait(false);
                }

                FileInfo fileInfo = new FileInfo(uploadFileFullName);
                string objectName = bucketChildName + fileInfo.Name;
                // 添加上传进度日志
                this.minioClient.SetTraceOn(new MinioLogHelper(processCallBack));
                await this.minioClient.PutObjectAsync(bucketName, objectName, uploadFileFullName, contentType).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(objectName))
                {
                    result = $"https://{MinioHelper.Endpoint}/{bucketName}/{objectName}";
                }
            }
            catch (Exception e)
            {
                Logger.Logger.Error("MinIO的UploadFile方法报错！", e);
            }
            finally
            {
                this.minioClient.SetTraceOff();
            }

            return result;
        }

        /// <summary>
        /// 根据minio链接地址，下载文件到指定位置
        /// </summary>
        /// <param name="fileMinioUrl">要下载的文件地址，完整的minio链接地址</param>
        /// https://image03-sz.deepsight.cloud/deepsight.cs/deepsight.report.system/PDF/345.pdf
        /// <param name="localPath">下载文件的保存地址，包含文件名和后缀：D:\11.pdf</param>
        /// <returns>下载成功与否</returns>
        public async Task<bool> DownloadFile(string fileMinioUrl, string localPath)
        {
            try
            {
                if (string.IsNullOrEmpty(fileMinioUrl))
                {
                    return false;
                }

                // 解析minio地址:$"https://{MinioHelper.Endpoint}/{bucketName}/{objectName}";
                int index = fileMinioUrl.IndexOf(Endpoint.Value);
                string tmp = fileMinioUrl.Substring(index + Endpoint.Value.Length + 1);
                index = tmp.IndexOf(@"/");
                string bucketName = tmp.Substring(0, index);
                string objectName = tmp.Substring(index + 1);
                if (System.IO.File.Exists(localPath))
                {
                    System.IO.File.Delete(localPath);
                }
                return this.DownloadFile(bucketName, objectName, localPath).Result;
            }
            catch (Exception ex)
            {
                Logger.Logger.Error("下载文件异常：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 根据桶名和对象路径，下载文件
        /// </summary>
        /// <param name="bucketName">桶名</param>
        /// <param name="objectName">对象路径</param>
        /// <param name="localPath">下载文件保存的地址，包含文件名和后缀名</param>
        /// <returns>下载成功状态</returns>
        public async Task<bool> DownloadFile(string bucketName, string objectName, string localPath)
        {
            if (string.IsNullOrEmpty(bucketName) || !await this.minioClient.BucketExistsAsync(bucketName).ConfigureAwait(false))
            {
                return false;
            }

            try
            {
                await this.minioClient.GetObjectAsync(bucketName, objectName, localPath).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Logger.Error("MinIO的DownloadFile方法出错！", ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取文件下载链接(这个有问题...)
        /// </summary>
        /// <param name="bucketName">存储桶名称</param>
        /// <param name="objectName">对象名称</param>
        /// <param name="expiresInt">失效时间（以秒为单位），默认是7天，不得大于七天</param>
        /// <returns></returns>
        private async Task<string> GetDownloadUrl(string bucketName, string objectName, int expiresInt = 60 * 60 * 24 * 5)
        {
            var result = await this.minioClient.PresignedGetObjectAsync(bucketName, objectName, expiresInt);
            return result;
        }

        /// 根据路径获取桶中文件名称
        /// </summary>
        /// <param name="prefix">桶中路径</param>
        /// <param name="bucketName">桶名</param>
        /// <returns></returns>
        public async Task<List<string>> GetFileNamesinBucket(string prefix, string bucketName)
        {
            var list = new List<string>();

            try
            {
                IObservable<Minio.DataModel.Item> observable = this.minioClient.ListObjectsAsync(bucketName, prefix, false);
                IDisposable subscription = observable.Subscribe(
                      item => list.Add(item.Key/*.ChangePathfromLinux()*/), // 将路径转换为window路径
                      ex => throw ex,
                      () =>
                      {
                          // 完成后需要执行的操作
                      });

                await observable;
                subscription.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Logger.Error("MinIO的GetFilesinBucket方法报错！", ex);
            }

            return list;
        }

        /// <summary>
        /// 根据url地址，直接下载文件
        /// </summary>
        /// <param name="url">url链接，必须是文件的链接</param>
        /// <param name="localPath">保存的本地位置</param>
        /// <returns>下载成功标志</returns>
        public async Task<bool> DownloadFileByUrl(string url, string localPath)
        {
            bool result = true;
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0)";
                request.Proxy = null;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                using (Stream responseStream = response.GetResponseStream())
                {
                    //创建本地文件写入流
                    using (Stream stream = new FileStream(localPath, FileMode.Create))
                    {
                        byte[] bArr = new byte[1024 * 1024];
                        int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                        while (size > 0)
                        {
                            stream.Write(bArr, 0, size);
                            size = responseStream.Read(bArr, 0, (int)bArr.Length);
                        }

                        stream.Close();
                        responseStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Logger.Logger.Error($"下载文件 {url} 异常：" + ex.Message);
            }

            return result;
        }
    }
}