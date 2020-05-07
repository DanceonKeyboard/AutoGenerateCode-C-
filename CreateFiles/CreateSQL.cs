using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConTestODBC
{
    public class CreateSQL
    {

        public static string[] aInType = ReadFiles.ReadInType();
        public static string sDBName = "INTRAFACTORYDB";
        public static string sTableName = "TEST_CALL";
        public static void CreateSQLScript()
        {
            string text = System.IO.File.ReadAllText(@"E:\vs2015_project\ODBCtest\ConTestODBC\bin\Debug\CreateServer\FieldsForSQLText.txt");
            string[] arrayInFieldsRow = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            List<string> Fieldlist = new List<string>();
            List<string> Typelist = new List<string>();
            List<string> PKlist = new List<string>();
            foreach (var item in arrayInFieldsRow)
            {
                string[] aFieldsDetail = item.Split(new string[] { " " }, StringSplitOptions.None);
                Fieldlist.Add(aFieldsDetail[1]);
                Typelist.Add(aFieldsDetail[3].ToUpper());
                if (aFieldsDetail[4]=="是")
                {
                    PKlist.Add(aFieldsDetail[1]);
                }
            }
            List<string> TypeTranlist = new List<string>();
            List<string> Defaultlist = new List<string>();
            foreach (var item in Typelist)
            {
                if (item.Contains("VARCHAR"))
                {
                    TypeTranlist.Add((item.Replace(")", " CHAR)")).Replace(("VARCHAR"), "VARCHAR2"));
                    Defaultlist.Add("' '");
                }
                else if (item.Contains("CHAR"))
                {
                    TypeTranlist.Add((item.Replace(")", " CHAR)")));
                    Defaultlist.Add("' '");
                }
                else
                {
                    TypeTranlist.Add(item);
                    Defaultlist.Add("0");

                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("ALTER TABLE "+sDBName+"."+ sTableName + Environment.NewLine);
            sb.Append(" DROP PRIMARY KEY CASCADE;" + Environment.NewLine);
            sb.Append("" + Environment.NewLine);
            sb.Append("DROP TABLE " + sDBName + "." + sTableName +" CASCADE CONSTRAINTS;" + Environment.NewLine);
            sb.Append("CREATE TABLE " + sDBName + "." + sTableName + Environment.NewLine);
            sb.Append("(" + Environment.NewLine);

           
            for (int i = 0; i < Fieldlist.Count; i++)
            {
                sb.Append(Fieldlist[i]+"            "+ TypeTranlist[i]+ "          DEFAULT "+Defaultlist[i]+"                   NOT NULL," + Environment.NewLine);
            }
            sb.Remove(sb.Length - 3, 1);//删除最后一个换行符（占两个字符）和逗号

            sb.Append(")" + Environment.NewLine);
            sb.Append("TABLESPACE " + sDBName  + Environment.NewLine);
            sb.Append("RESULT_CACHE (MODE DEFAULT)" + Environment.NewLine);
            sb.Append("PCTUSED    0" + Environment.NewLine);
            sb.Append("PCTFREE    10" + Environment.NewLine);
            sb.Append("INITRANS   1" + Environment.NewLine);
            sb.Append("MAXTRANS   255" + Environment.NewLine);
            sb.Append("STORAGE    (" + Environment.NewLine);
            sb.Append("             PCTINCREASE      0" + Environment.NewLine);
            sb.Append("             BUFFER_POOL      DEFAULT" + Environment.NewLine);
            sb.Append("             FLASH_CACHE      DEFAULT" + Environment.NewLine);
            sb.Append("             CELL_FLASH_CACHE DEFAULT" + Environment.NewLine);
            sb.Append("            )" + Environment.NewLine);
            sb.Append("LOGGING " + Environment.NewLine);
            sb.Append("NOCOMPRESS " + Environment.NewLine);
            sb.Append("NOCACHE" + Environment.NewLine);
            sb.Append("NOPARALLEL" + Environment.NewLine);
            sb.Append("MONITORING;" + Environment.NewLine);
            sb.Append("" + Environment.NewLine);
            sb.Append("" + Environment.NewLine);
            sb.Append("CREATE UNIQUE INDEX " + sDBName + "." + sTableName + "_PK ON " + sDBName + "." + sTableName  + Environment.NewLine);
            string PrimaryKey = "";
            string Result = "";
            foreach (var item in PKlist)
            {
                PrimaryKey += item+",";
            }
            Result = PrimaryKey.Substring(0, PrimaryKey.LastIndexOf(","));//去掉最后一个逗号sb.ToString().Length - 1
            sb.Append("("+Result+")" + Environment.NewLine);
            sb.Append("LOGGING" + Environment.NewLine);
            sb.Append("TABLESPACE " + sDBName + Environment.NewLine);
            sb.Append("PCTFREE    10" + Environment.NewLine);
            sb.Append("INITRANS   2" + Environment.NewLine);
            sb.Append("MAXTRANS   255" + Environment.NewLine);
            sb.Append("STORAGE    (" + Environment.NewLine);
            sb.Append("             PCTINCREASE      0" + Environment.NewLine);
            sb.Append("             BUFFER_POOL      DEFAULT" + Environment.NewLine);
            sb.Append("             FLASH_CACHE      DEFAULT" + Environment.NewLine);
            sb.Append("             CELL_FLASH_CACHE DEFAULT" + Environment.NewLine);
            sb.Append("            )" + Environment.NewLine);
            sb.Append("NOPARALLEL;" + Environment.NewLine);
            sb.Append("" + Environment.NewLine);
            sb.Append("" + Environment.NewLine);
            sb.Append("ALTER TABLE " + sDBName + "." + sTableName + " ADD (" + Environment.NewLine);
            sb.Append(" CONSTRAINT " + sTableName + "_PK" + Environment.NewLine);
            sb.Append(" PRIMARY KEY" + Environment.NewLine);
            sb.Append("("+Result+")" + Environment.NewLine);
            sb.Append("USING INDEX " + sDBName + "." + sTableName + "_PK" + Environment.NewLine);
            sb.Append("  ENABLE VALIDATE);" + Environment.NewLine);
            sb.Append("" + Environment.NewLine);
            sb.Append("" + Environment.NewLine);

            BaseCreateFiles.Save_txt(sb.ToString(), "CreateSQLScript");


        }

    }
}
