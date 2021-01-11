using Minio;
using Minio.DataModel.Tracing;
using System;
using System.Net;

namespace DST.Common.MinioHelper
{
    public class MinioLogHelper : IRequestLogger
    {
        private Action<int> callBack = null;

        public MinioLogHelper(Action<int> action)
        {
            this.callBack = action;
        }

        public void LogRequest(RequestToLog requestToLog, ResponseToLog responseToLog, double durationMs)
        {
            if (responseToLog.statusCode == HttpStatusCode.OK)
            {
                foreach (var header in requestToLog.parameters)
                {
                    if (!string.Equals(header.name, "partNumber") || header.value == null)
                    {
                        continue;
                    }

                    // minio遇到上传文件大于5MB时，会进行分块传输，这里就是当前块的编号（递增）
                    int.TryParse(header.value.ToString(), out var partNumber);
                    if (this.callBack != null)
                    {
                        this.callBack(partNumber);
                    }

                    break;
                }
            }
        }
    }
}