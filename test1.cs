using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;

namespace ConTestODBC
{
    public class test1
    {
        OdbcHelper odbcHelper = new OdbcHelper();
        /// <summary>
        /// 生成XML文件，压缩文件
        /// </summary>
        public static void test2()
        {
            DataTable dtTag = new DataTable();
            dtTag.Columns.Add("time");
            dtTag.Columns.Add("amp");
            dtTag.Rows.Add("2019/11/8 18:03:09", 0910104);
            dtTag.Rows.Add("2019/11/9 18:03:09", 0910704);
            dtTag.Rows.Add("2019/11/10 18:03:09", 1200102);
            dtTag.Rows.Add("2019/11/11 18:03:09", 1224005);
            dtTag.Rows.Add("2019/11/12 18:03:09", 1820101);
            //创建XmlDocument对象
            XmlDocument xmlDoc = new XmlDocument();
            //XML的声明<?xml version="1.0" encoding="gb2312"?> 
            XmlDeclaration xmlSM = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            //追加xmldecl位置
            xmlDoc.AppendChild(xmlSM);
            //添加一个名为TraceData的根节点
            XmlElement xml = xmlDoc.CreateElement("", "TraceData", "");
            //追加TraceData的根节点位置
            xmlDoc.AppendChild(xml);
            //添加另一个节点,与TraceData所匹配，查找<TraceData>
            XmlNode traceData = xmlDoc.SelectSingleNode("TraceData");
            //添加一个名为<Header>的节点   
            XmlElement Header = xmlDoc.CreateElement("Header");
            //为<Header>节点的属性
            //Header.SetAttribute("name", "博客园");
            //Header.SetAttribute("age", "26");
            XmlElement x1 = xmlDoc.CreateElement("ColumnHeader");
            //InnerText:获取或设置节点及其所有子节点的串连值
            x1.InnerText =dtTag.Columns[0].ToString()+","+ dtTag.Columns[1].ToString();//"time,amp"
            Header.AppendChild(x1);//添加到<Header>节点中
            traceData.AppendChild(Header);//添加到<TraceData>节点中   

            XmlElement Data = xmlDoc.CreateElement("Data");

            for (int i = 0; i < dtTag.Rows.Count; i++)
            {
                Data.InnerText += Environment.NewLine+"        ";
                for (int j = 0; j < dtTag.Columns.Count; j++)
                {
                    Data.InnerText +=  dtTag.Rows[i][j].ToString();//Environment.NewLine
                    if (j%2==0)
                    {
                        Data.InnerText += ",";
                    }
                }
            }
            Data.InnerText += Environment.NewLine;
           //为<Data>节点的属性
           //Data.SetAttribute("name", "博客园");
           //Data.SetAttribute("age", "26");
           //XmlElement x2 = xmlDoc.CreateElement("ColumnHeader");
           ////InnerText:获取或设置节点及其所有子节点的串连值
           //x1.InnerText = "time,amp";
           //Header.AppendChild(x1);//添加到<Header>节点中
            traceData.AppendChild(Data);//添加到<TraceData>节点中  

            //保存好创建的XML文档

            string sFilePath = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");// Application.StartupPath + "\\UserFile\\Label\\" + DateTime.Now.ToString();
            xmlDoc.Save("D:/OpcXML/data.xml");
            Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        }
        //
        public static void SaveopcXML()
        {
            try
            {
                string sPath = "D:/OpcXML/data.xml";
                ZipFile(sPath);

                FileStream fs = new FileStream("D:/OpcXML/data.zip", FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                Byte[] byData = br.ReadBytes((int)fs.Length);
                string sData = System.Text.Encoding.UTF8.GetString(byData);
                DateTime dt_value = DateTime.Now;
                string sCreateTime = dt_value.Year.ToString("000#") +
                       dt_value.Month.ToString("0#") +
                       dt_value.Day.ToString("0#") +
                       dt_value.Hour.ToString("0#") +
                       dt_value.Minute.ToString("0#") +
                       dt_value.Second.ToString("0#");
                //rawtohex()
                fs.Dispose();//释放资源
                DeleteFile("D:/OpcXML/data.zip");
                OdbcHelper.OracleExeQuery("update U_WIPPROCDATA set FILE_DATA=:zp, CREATE_TIME="+ sCreateTime + ",CREATE_USER_ID='1111' where FACTORY ='15车间' AND LOT_ID ='1'", byData);

                fs.Close();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            
        }
        public static bool DeleteFile(string path1)
        {
            try
            {
                if (File.Exists(path1))
                {
                    File.Delete(path1);
                }
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        /// <summary>
        /// ZIP:压缩单个文件
        /// add yuangang by 2016-06-13
        /// </summary>
        /// <param name="FileToZip">需要压缩的文件（绝对路径）</param>
        /// <param name="ZipedPath">压缩后的文件路径（绝对路径）</param>
        /// <param name="ZipedFileName">压缩后的文件名称（文件名，默认 同源文件同名）</param>
        /// <param name="CompressionLevel">压缩等级（0 无 - 9 最高，默认 5）</param>
        /// <param name="BlockSize">缓存大小（每次写入文件大小，默认 2048）</param>
        /// <param name="IsEncrypt">是否加密（默认 加密）</param>
        public static void ZipFile(string FileToZip,string ZipedFileName = "", int CompressionLevel = 9, int BlockSize = 2048, bool IsEncrypt = false)
        {
            //如果文件没有找到，则报错
            if (!System.IO.File.Exists(FileToZip))
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + FileToZip + " 不存在!");
            }
            string ZipedPath = FileToZip.Substring(0,FileToZip.LastIndexOf("/"));
            //文件名称（默认同源文件名称相同）
            string ZipFileName = string.IsNullOrEmpty(ZipedFileName) ? ZipedPath + "/" + new FileInfo(FileToZip).Name.Substring(0, new FileInfo(FileToZip).Name.LastIndexOf('.')) + ".zip" : ZipedPath + "\\" + ZipedFileName + ".zip";

            using (System.IO.FileStream ZipFile = System.IO.File.Create(ZipFileName))
            {
                using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                {
                    using (System.IO.FileStream StreamToZip = new System.IO.FileStream(FileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        string fileName = FileToZip.Substring(FileToZip.LastIndexOf("/") + 1);

                        ZipEntry ZipEntry = new ZipEntry(fileName);

                        if (IsEncrypt)
                        {
                            //压缩文件加密
                            ZipStream.Password = "123";
                        }

                        ZipStream.PutNextEntry(ZipEntry);

                        //设置压缩级别
                        ZipStream.SetLevel(CompressionLevel);

                        //缓存大小
                        byte[] buffer = new byte[BlockSize];

                        int sizeRead = 0;

                        try
                        {
                            do
                            {
                                sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                                ZipStream.Write(buffer, 0, sizeRead);
                            }
                            while (sizeRead > 0);
                        }
                        catch (System.Exception ex)
                        {
                            throw ex;
                        }

                        StreamToZip.Close();
                    }

                    ZipStream.Finish();
                    ZipStream.Close();
                }

                ZipFile.Close();
            }
        }


        static string HexString2BinString(string hexString)
        {
            string result = string.Empty;
            foreach (char c in hexString)
            {
                int v = Convert.ToInt32(c.ToString(), 16);
                int v2 = int.Parse(Convert.ToString(v, 2));
                // 去掉格式串中的空格，即可去掉每个4位二进制数之间的空格，
                result += string.Format("{0:d4} ", v2);
            }
            return result;
        }
        public static byte[] CompressGZip(byte[] rawData)
        {
            MemoryStream ms = new MemoryStream();
            GZipOutputStream compressedzipStream = new GZipOutputStream(ms);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }
        public static byte[] UnGZip(byte[] byteArray)
        {
            ZipInputStream gzi = new ZipInputStream(new MemoryStream(byteArray));

            MemoryStream re = new MemoryStream(50000);
            int count;
            byte[] data = new byte[50000];
            while ((count = gzi.Read(data, 0, data.Length)) != 0)
            {
                re.Write(data, 0, count);
            }
            byte[] overarr = re.ToArray();
            return overarr;
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

        /// <summary>   
        /// 压缩文件   
        /// </summary>   
        /// <param name="fileToZip">要压缩的文件全名</param>   
        /// <param name="zipedFile">压缩后的文件名</param>   
        /// <param name="password">密码</param>   
        /// <returns>压缩结果</returns>   
        public static bool ZipFile(string fileToZip, string zipedFile, string password)
        {
            bool result = true;
            ZipOutputStream zipStream = null;
            FileStream fs = null;
            ZipEntry ent = null;

            if (!File.Exists(fileToZip))
                return false;

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
                zipStream.SetLevel(9);

                zipStream.Write(buffer, 0, buffer.Length);

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                result = false;
            }
            finally
            {
                if (zipStream != null)
                {
                    zipStream.Flush();
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
        public static bool ZipFile(string fileToZip, string zipedFile)
        {
            bool result = ZipFile(fileToZip, zipedFile, null);
            return result;
        }

       
        }
}
