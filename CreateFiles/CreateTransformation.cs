using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConTestODBC
{
    public class CreateTransformation
    {

       
        public static void CreateTransformText()
        {
            StringBuilder sb = new StringBuilder();
            //int iRes = 14;
            //string sMat = "XCPP-1816";
            string sRes = "";
            string[] aEnglish = ReadFiles.ReadInText();
            string[] aChinese = ReadFiles.ReadChineseText();
            for (int i = 0; i < aEnglish.Length; i++)
            {
                sb.Append("<Text Key=\""+ aEnglish [i]+ "\">" + Environment.NewLine);
                sb.Append("<L>" + aEnglish[i] + "</L>" + Environment.NewLine);
                sb.Append("<L>" + aChinese[i] + "</L>" + Environment.NewLine);
                sb.Append("<L></L>" + Environment.NewLine);
                sb.Append("</Text>" + Environment.NewLine);
            }
            sRes = sb.ToString().Substring(0, sb.ToString().LastIndexOf("\r\n"));//去掉最后一个逗号sb.ToString().Length - 1
            BaseCreateFiles.Save_txt(sRes, "CreateTransformation");
        }
    }
}
