using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConTestODBC
{
    public class CreateTestData
    {
        /// <summary>
        /// 根据字符串Mat生成i行
        /// </summary>
        /// <param name="sMat">每行文本</param>
        /// <param name="iRes">iRes行</param>
        /// <returns></returns>
        public static string CreateTxt(string sMat,int iRes)
        {
            StringBuilder sb = new StringBuilder();
            //int iRes = 14;
            string sTxt = "";
            for (int i = 0; i < iRes; i++)
            {
                sb.Append(sMat+ Environment.NewLine);
            }
            sTxt = sb.ToString().Substring(0, sb.ToString().LastIndexOf("\r\n"));
            return sTxt;
        }
        /// <summary>
        /// 根据Mat的行数生成重复iMat行的Res文本
        /// </summary>
        /// <param name="iMat"></param>
        public static void CreateRESTxt()
        {
            StringBuilder sb = new StringBuilder();
            //int iRes = 14;
            //string sMat = "XCPP-1816";
            string sRes = "";
            string[] aMat = ReadFiles.ReadMat();
            string[] aRes = ReadFiles.ReadRes();
            for (int j = 0; j < aRes.Length; j++)
            {
                for (int i = 0; i < aMat.Length; i++)
                {
                    sb.Append(aRes[j] + Environment.NewLine);
                }
            }
            sRes = sb.ToString().Substring(0, sb.ToString().LastIndexOf("\r\n"));//去掉最后一个逗号sb.ToString().Length - 1
            BaseCreateFiles.Save_txt(sRes, "CreateRes");
        }
        public static void CreateMATTxt()
        {
            StringBuilder sb = new StringBuilder();
            //int iRes = 14;
            //string sMat = "XCPP-1816";
            string sTxt = "";
            string sMat = System.IO.File.ReadAllText(@"E:\vs2015_project\ODBCtest\ConTestODBC\bin\Debug\MatText.txt");
            string[] aRes = ReadFiles.ReadRes();
            for (int i = 0; i < aRes.Length; i++)
            {
                sb.Append(sMat + Environment.NewLine);
            }
            sTxt = sb.ToString().Substring(0, sb.ToString().LastIndexOf("\r\n"));//去掉最后一个逗号sb.ToString().Length - 1

            BaseCreateFiles.Save_txt(sTxt, "CreateMat");
        }

        public static void CreateTimeTxt()
        {
            string sMat = "";
            string stxt = "";
            string[] arrayTime = ReadFiles.ReadTime();
            string[] arrayRes = ReadFiles.ReadRes();
            StringBuilder sTime =new StringBuilder();
            for (int i = 0; i < arrayTime.Length; i++)
            {
                sTime.Append((Convert.ToDouble(arrayTime[i]) * 60).ToString()+"\r\n");
                stxt = CreateTestData.CreateTxt(sTime.ToString().Substring(0, sTime.ToString().LastIndexOf("\r\n")), arrayRes.Length);
            }
            //stxt = stxt.ToString().Substring(0, stxt.LastIndexOf("\r\n"));

            BaseCreateFiles.Save_txt(stxt, "CreateTime");

        }
        // 24mTimeText.txt
       
       
    }
}
