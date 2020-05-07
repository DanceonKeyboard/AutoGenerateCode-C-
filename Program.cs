using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ConTestODBC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //191219  生成服务方法
                string sServerName = "WIP_Custom_View_EngineNo_List";
                string sExplain = "  查看所有发动机号";
                string listName = "engine_no";
                CreateServer.UserTypeCreateTxt(sServerName, listName, sExplain);
                CreateServer.CreateUserTunerTxt(sServerName, sExplain);
                CreateServer.CreateUserRoutineTxt(sServerName, listName, sExplain);

                CreateClient.CreateListRoutineTxt(sServerName, sExplain);
                //CreateTransformation.CreateTransformText();
                //CreateClient.CreateClientUpdateDetailTxt(sServerName, sExplain);

                //0107
                //SaveopcXML();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("SUCESS!");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        public static void SaveopcXML()
        {
            try
            {
               
                DateTime dt_value = DateTime.Now;
                string sCreateTime = dt_value.Year.ToString("000#") +
                       dt_value.Month.ToString("0#") +
                       dt_value.Day.ToString("0#") +
                       dt_value.Hour.ToString("0#") +
                       dt_value.Minute.ToString("0#") +
                       dt_value.Second.ToString("0#");
                int itemID = 167;
                string[] aTestData = ReadFiles.ReadInTestData();
                string[] aTestType = ReadFiles.ReadInTestType();
                string[] aTestStep = ReadFiles.ReadInTestStep();
                string[] aTestOper = ReadFiles.ReadInTestOper();
                string sAcType = " ";
                //for (int i = 0; i < aTestData.Length; i++)
                //{
                //    if(aTestType[i]!="Q"&& aTestType[i] != "5S"&& aTestType[i] != "E")
                //    {
                //        sAcType = "P";
                //    }
                //    else
                //    {
                //        sAcType = aTestType[i];
                //    }
                //    OdbcHelper.SQLExeQuery("INSERT INTO U_WIPPROCCONFITEM (FACTORY,ITEM_ID,ITEM_DESC,ACTIVITY_DESC,ACTIVITY_TYPE,USER_QUALIFICATION, CREATE_TIME,CREATE_USER_ID,UPDATE_TIME,UPDATE_USER_ID) " +
                //   " VALUES ('CWEC','" + itemID + "',' ','" + aTestData[i].Trim() + "','" + sAcType.Trim() + "',' ','" + sCreateTime + "','ADMIN','" + sCreateTime + "','ADMIN')");
                //    itemID++;
                //}
                string sStep = " ";
                string sOper = " ";
                int index =-1;
                for (int i = 0; i < aTestType.Length; i++)
                {
                    if (aTestType[i] == "1")
                    {
                        index++;
                    }
                    OdbcHelper.SQLExeQuery("INSERT INTO U_WIPPROCCONFSETITEMREL (FACTORY,SET_ID,ITEM_ID,OPER,STEP,SEQ_NUM,CRITICAL_FLAG, CREATE_TIME,CREATE_USER_ID,UPDATE_TIME,UPDATE_USER_ID) " +
                   " VALUES ('CWEC','W6L34DFB','"+itemID+"',' ','" + aTestStep[index].Trim() + "',"+Convert.ToInt32(itemID)+",'N','" + sCreateTime + "','ADMIN',' ',' ')");
                    
                    itemID++;
                }
                //rawtohex()

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

        }
        public static void ExcuteHistory()
        {
            //生成XML文件并压缩
            //Console.WriteLine("Hello World!");
            //test1.test2();
            //test1.SaveopcXML();
            //string encode = getPwdEncrypt("123456");

            //生成测试数据
            //string[] arrayTime = Class1.ReadTime();
            //for (int i = 0; i < arrayTime.Length; i++)
            //{
            //    sMat = (Convert.ToDouble(arrayTime[i]) * 60).ToString();
            //    stxt = Class1.CreateTxt(sMat.Trim(),12);
            //    Class1.Save_txt(stxt, "Create15Time");

            //}
            //string[] arrayMat = Class1.ReadMat();
            //for (int i = 0; i < arrayMat.Length; i++)
            //{
            //    sMat = arrayMat[i];//"XCPP-1816"
            //    stxt = Class1.CreateTxt(sMat.Trim(),12);
            //    Class1.Save_txt(stxt, "Create15MAT");

            //}
            //191204  生成测试数据2
            //Class1.CreateMATTxt();
            //Class1.CreateRESTxt();
            //Class1.CreateTimeTxt();

            //191215  生成服务方法
            //string sServerName = "WIP_Custom_View_Proc_Conf_Set_Item_List";
            //string sExplain = "  查看工序操作确认集关联的确认项列表";
            //string listName = "conf_set_item";
            //CreateServer.CreateUserTypeTxt(sServerName, listName, sExplain);
            //CreateServer.CreateUserTunerTxt(sServerName, sExplain);
            //CreateServer.CreateUserRoutineTxt(sServerName,listName, sExplain);

            //191216  生成翻译
            // CreateTransformation.CreateTransformText();

            //191217  服务
            //string sServerName = "WIP_Custom_Update_Proc_Conf_Item";
            //string sExplain = "  添加/修改/删除工序操作确认项和工序检查点";
            //CreateServer.CreateUserRoutineTxt(sServerName, "conf_item", sExplain);


            //Console.WriteLine(CommonUtils.FiledFirstToUpper("conf_set_item"));
            //CommonUtils.CreateMethodName("WIP_Custom_View_Proc_Conf_Set_Item_List");
            //CreateClient.CreateListRoutineTxt(sServerName, sExplain);
            //foreach (var item in ReadFiles.ReadAndTranInType())
            //{
            //    Console.WriteLine(item);
            //}

            //191218 生成SQL脚步
            //CreateSQL.CreateSQLScript();
        }
    }
}
