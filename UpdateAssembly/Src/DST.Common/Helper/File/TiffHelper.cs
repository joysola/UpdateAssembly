using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace DST.Common.Helper
{
    public class TiffHelper
    {
        /// <summary>
        /// 获取图像页数
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static int GetPageNumber(string imagePath)
        {
            using (Image image = Bitmap.FromFile(imagePath))
            {
                Guid objGuid = image.FrameDimensionsList[0];
                FrameDimension objDimension = new FrameDimension(objGuid);

                return image.GetFrameCount(objDimension);
            }
        }

        /// <summary>
        /// 将给定的文件 拼接输出到指定的tif文件路径
        /// </summary>
        /// <param name="imageFiles">文件路径列表</param>
        /// <param name="outFile">拼接后保存的 tif文件路径</param>
        /// <param name="compressEncoder">压缩方式</param>
        public static void JoinTiffImages(ArrayList imageFiles, string outFile, EncoderValue compressEncoder)
        {
            //如果只有一个文件，直接复制到目标
            if (imageFiles.Count == 1)
            {
                File.Copy((string)imageFiles[0], outFile, true);
                return;
            }

            Encoder enc = Encoder.SaveFlag;

            EncoderParameters ep = new EncoderParameters(2);
            ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.MultiFrame);
            ep.Param[1] = new EncoderParameter(Encoder.Compression, (long)compressEncoder);

            Bitmap pages = null;
            int frame = 0;
            ImageCodecInfo info = GetEncoderInfo("image/tiff");

            foreach (string strImageFile in imageFiles)
            {
                if (frame == 0)
                {
                    pages = (Bitmap)Image.FromFile(strImageFile);
                    //保存第一个tif文件 到目标处
                    pages.Save(outFile, info, ep);
                }
                else
                {
                    //保存好第一个tif文件后，其余 设置为添加一帧到 图像中
                    ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.FrameDimensionPage);

                    Bitmap bm = (Bitmap)Image.FromFile(strImageFile);
                    pages.SaveAdd(bm, ep);
                    bm.Dispose();
                }

                if (frame == imageFiles.Count - 1)
                {
                    //flush and close.
                    ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.Flush);
                    pages.SaveAdd(ep);
                }
                frame++;
            }
            pages.Dispose(); //释放资源
            return;
        }

        /// <summary>
        /// 拼接两个tif文件 保存到文件2中
        /// </summary>
        /// <param name="filePath">tif文件1</param>
        /// <param name="targetFile">tif文件2</param>
        public static void AppendToTiff(string filePath, string targetFile)
        {
            ArrayList list = new ArrayList(); //保存所有 tif文件路径

            #region 分割tif文件1

            string tempDirectory1 = string.Empty;
            list.AddRange(SplitTif(filePath, out tempDirectory1));

            #endregion 分割tif文件1

            #region 分割tif文件2

            string tempDirectory2 = string.Empty;
            list.AddRange(SplitTif(targetFile, out tempDirectory2));

            #endregion 分割tif文件2

            //2. 拼接所有tif页

            //2.1 删除原目标文件
            File.Delete(targetFile);
            //2.2 拼接 并按原路径生成tif文件
            JoinTiffImages(list, targetFile, EncoderValue.CompressionNone);

            //3. 删除临时目录
            DirectoryInfo di2 = new DirectoryInfo(tempDirectory2);
            di2.Delete(true);

            DirectoryInfo di1 = new DirectoryInfo(tempDirectory1);
            di1.Delete(true);
        }

        /// <summary>
        /// 将给定文件  分割成多个tif文件 到临时目录下
        /// </summary>
        /// <param name="targetFile">目标文件</param>
        /// <param name="tempDirectory">临时目录路径，删除用</param>
        /// <returns>分割后多个文件路径集合</returns>
        public static ArrayList SplitTif(string targetFile, out string tempDirectory)
        {
            ArrayList list = new ArrayList();
            using (Image img = Image.FromFile(targetFile))
            {
                Guid guid = img.FrameDimensionsList[0];

                System.Drawing.Imaging.FrameDimension dimension = new System.Drawing.Imaging.FrameDimension(guid);

                int nTotFrame = img.GetFrameCount(dimension); //tif总页数

                int nLoop = 0; //索引
                //生成临时目录 存放 单tif页
                tempDirectory = Path.Combine(Path.GetDirectoryName(targetFile), Guid.NewGuid().ToString());
                if (!Directory.Exists(tempDirectory))
                {
                    Directory.CreateDirectory(tempDirectory);
                }

                EncoderParameters ep = new EncoderParameters(2);
                ep.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                ep.Param[1] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionNone);//压缩方式  CompressionCCITT3主要用于传真

                ImageCodecInfo info = GetEncoderInfo("image/tiff");

                for (nLoop = 0; nLoop < nTotFrame; nLoop++)
                {
                    img.SelectActiveFrame(dimension, nLoop);
                    //保存 单tif页
                    string newfilePath = Path.Combine(tempDirectory, nLoop.ToString() + ".jpg");

                    img.Save(newfilePath, info, ep);
                    //将路径存入 list中
                    list.Add(newfilePath);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取支持的编码信息
        /// </summary>
        /// <param name="mimeType">协议描述</param>
        /// <returns>图像编码信息</returns>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < encoders.Length; j++)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }

            throw new Exception(mimeType + " mime type not found in ImageCodecInfo");
        }

        public static Image JoinImage(List<Image> imageList, int horOrver)
        {
            //图片列表
            if (imageList.Count <= 0)
                return null;
            if (horOrver == 0)
            {
                //横向拼接
                int width = 0;
                //计算总长度
                foreach (Image i in imageList)
                {
                    width += i.Width;
                }
                //高度不变
                int height = imageList.Max(x => x.Height);
                //构造最终的图片白板
                Bitmap tableChartImage = new Bitmap(width, height);
                Graphics graph = Graphics.FromImage(tableChartImage);
                //初始化这个大图
                graph.DrawImage(tableChartImage, width, height);
                //初始化当前宽
                int currentWidth = 0;
                foreach (Image i in imageList)
                {
                    //拼图
                    graph.DrawImage(i, currentWidth, 0);
                    //拼接改图后，当前宽度
                    currentWidth += i.Width;
                }
                graph.Dispose();
                return tableChartImage;
            }
            else if (horOrver == 1)
            {
                //纵向拼接
                int height = 0;
                //计算总长度
                foreach (Image i in imageList)
                {
                    height += i.Height;
                }
                //宽度不变
                int width = imageList.Max(x => x.Width);
                //构造最终的图片白板
                Bitmap tableChartImage = new Bitmap(width, height);
                Graphics graph = Graphics.FromImage(tableChartImage);
                //初始化这个大图
                graph.DrawImage(tableChartImage, width, height);
                //初始化当前宽
                int currentHeight = 0;
                foreach (Image i in imageList)
                {
                    //拼图
                    graph.DrawImage(i, 0, currentHeight);
                    //拼接改图后，当前宽度
                    currentHeight += i.Height;
                }
                graph.Dispose();
                return tableChartImage;
            }
            else
            {
                return null;
            }
        }
    }
}