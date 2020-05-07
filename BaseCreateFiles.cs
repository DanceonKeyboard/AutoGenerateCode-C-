using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConTestODBC
{
    public class BaseCreateFiles
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="names"></param>
        public static void Save_txt(string txt, string names)
        {
         
            string t = DateTime.Now.ToString("HH-mm-ss");//获取当天时间 并以”-“来定义格式yyyy-
            string path = t + names  + ".txt";//用当天时间 定义文件名
            if (File.Exists(path))//判断路径是否存在
            {
                Console.WriteLine("ok");
            }
            else
            {
                File.Create(path).Dispose();//不存在就创建文件名 并且为了防止文件占用 给他dispose
            }
            //            public StreamWriter (
            //            string path,  // 文件路径
            //            bool append,  // 是否在末尾添加
            //            Encoding encoding  // 编码方式
            //            )
            //Encoding.GetEncoding("GB2312")        Encoding.UTF8
            StreamWriter sw =  File.CreateText(path); //new StreamWriter(path, false, Encoding.GetEncoding("GB2312"));//添加数据AppendText追加
            sw.Write(txt);//写入文件
            sw.Flush();
            sw.Close();//关闭

        }




    }
}
