using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;
using System.Collections;
using Ionic.Crc;
using System.IO.Compression;

namespace ConTestODBC
{
    /// <summary>
    /// 压缩解压文件
    /// </summary>
    public class ZipHelper
    { /// <summary>
      /// 使用GZIP压缩文件的方法
      /// </summary>
      /// <param name="sourcefilename"></param>
      /// <param name="zipfilename"></param>
      /// <returns></returns>
        public static bool GZipFile(string sourcefilename, string zipfilename)
        {
            bool blResult;//表示压缩是否成功的返回结果
                          //为源文件创建读取文件的流实例
            var srcFile = File.OpenRead(sourcefilename);
            //为压缩文件创建写入文件的流实例，
            var zipFile = new ZipOutputStream(File.Open(zipfilename, FileMode.Create));
            try
            {
                byte[] FileData = new byte[srcFile.Length];//创建缓冲数据
                srcFile.Read(FileData, 0, (int)srcFile.Length);//读取源文件
                zipFile.Write(FileData, 0, FileData.Length);//写入压缩文件

                blResult = true;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                blResult = false;
            }
            srcFile.Close();//关闭源文件
            zipFile.Close();//关闭压缩文件
            return blResult;
        }
        /// <summary>
        /// /使用GZIP解压文件的方法
        /// </summary>
        /// <param name="zipfilename"></param>
        /// <param name="unzipfilename"></param>
        /// <returns></returns>
        public static bool UnGzipFile(string zipfilename, string unzipfilename)
        {
            bool blResult;//表示解压是否成功的返回结果
            //创建压缩文件的输入流实例
            var zipFile = new ZipInputStream(File.OpenRead(zipfilename));
            //创建目标文件的流
            var destFile = File.Open(unzipfilename, FileMode.Create);
            try
            {
                int buffersize = 2048;//缓冲区的尺寸，一般是2048的倍数
                byte[] FileData = new byte[buffersize];//创建缓冲数据
                while (buffersize > 0)//一直读取到文件末尾
                {
                    buffersize = zipFile.Read(FileData, 0, buffersize);//读取压缩文件数据
                    destFile.Write(FileData, 0, buffersize);//写入目标文件
                }
                blResult = true;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                blResult = false;
            }
            destFile.Close();//关闭目标文件
            zipFile.Close();//关闭压缩文件
            return blResult;
        }

        public static byte[] CompressGZip(byte[] rawData)
        {
            MemoryStream ms = new MemoryStream();
            ZipOutputStream compressedzipStream = new ZipOutputStream(ms);
            //compressedzipStream.putNextEntry(new ZipEntry());
           
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }
        public static MemoryStream Compress(byte[] inBytes)
        {

            MemoryStream outStream = new MemoryStream();
            using (MemoryStream intStream = new MemoryStream(inBytes))
            {
                using (GZipStream Compress = new GZipStream(outStream, CompressionMode.Compress))
                {
                    intStream.CopyTo(Compress);
                }
            }
            return outStream;

        }
    }

}
