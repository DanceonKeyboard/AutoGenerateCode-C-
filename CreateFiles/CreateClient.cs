using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConTestODBC
{
    public class CreateClient
    {

        #region Variable  读取数据
        public static string[] aInFields = ReadFiles.ReadInFields();
        public static string[] aInType = ReadFiles.ReadInType();
        public static string[] aOutFields = ReadFiles.ReadOutFields();
        public static string[] aOutType = ReadFiles.ReadOutType();
        #endregion
        public static void CreateListRoutineTxt(string sServerName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            string sUserType = sServerName.Substring(0, 3);
            string sTxt = "";
            if (sServerName.ToLower().Contains("_view_")&&sServerName.ToLower().Contains("_list"))
            {
                sb.Append("#region " + sServerName + sExplain + Environment.NewLine);
                sb.Append("public static DataTable "+ CommonUtils.CreateMethodName(sServerName) + "()" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                sb.Append("try" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                sb.Append("DataTable dtResult = new DataTable();" + Environment.NewLine);
                sb.Append("ArrayList a_list;" + Environment.NewLine);
                sb.Append("TRSNode in_node = new TRSNode(\"VIEW_LIST_IN\");" + Environment.NewLine);
                sb.Append("TRSNode out_node;" + Environment.NewLine);
                for (int i = 0; i < aOutFields.Length; i++)
                {
                    sb.Append("dtResult.Columns.Add(\""+aOutFields[i].ToUpper()+"\");" + Environment.NewLine);

                }
                sb.Append("a_list = new ArrayList();" + Environment.NewLine);
                sb.Append("CommonRoutine.SetInMsg(in_node);" + Environment.NewLine);
                sb.Append("in_node.ProcStep = '1';" + Environment.NewLine);
                sb.Append("in_node.AddString(\"NEXT_ITEM_ID\", \"\");" + Environment.NewLine);
                sb.Append("" + Environment.NewLine);
                sb.Append("do" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                sb.Append("out_node = new TRSNode(\"VIEW_LIST_OUT\");" + Environment.NewLine);
                sb.Append("if (CommonRoutine.CallService(\""+ sUserType + "\", \""+sServerName+"\", in_node, ref out_node) == false)" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                sb.Append("return null;" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
                sb.Append("a_list.Add(out_node);" + Environment.NewLine);
                sb.Append("in_node.SetString(\"NEXT_ITEM_ID\", out_node.GetString(\"NEXT_ITEM_ID\"));" + Environment.NewLine);
                sb.Append("} while (in_node.GetString(\"NEXT_ITEM_ID\") != \"\");" + Environment.NewLine);
                sb.Append("" + Environment.NewLine);
                sb.Append("foreach (object obj in a_list)" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                sb.Append("out_node = null;" + Environment.NewLine);
                sb.Append("out_node = (TRSNode)obj;" + Environment.NewLine);
                sb.Append("for (int i = 0; i < out_node.GetList(0).Count; i++)" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                sb.Append("dtResult.Rows.Add(" + Environment.NewLine);
                for (int i = 0; i < aOutFields.Length; i++)
                {
                    sb.Append("out_node.GetList(0)[i].Get"+CommonUtils.FirstToUpper(aOutType[i])+"(\""+aOutFields[i].ToUpper()+"\")," + Environment.NewLine);
                }
                sb.Remove(sb.Length - 3,1);//删除最后一个换行符（占两个字符）和逗号
                sb.Append(" );" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
                sb.Append("dtResult.AcceptChanges();//保存更改，便于界面判断是否有修改" + Environment.NewLine);
                sb.Append("return dtResult;" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
                sb.Append("catch (Exception ex)" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                sb.Append("CommonFunction.ShowMsgBox(ex.Message);" + Environment.NewLine);
                sb.Append("return null;" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
                sb.Append("#endregion ");

            }


            //sTxt = sb.ToString().Substring(0, sb.ToString().LastIndexOf("\r\n"));//去掉最后一个逗号sb.ToString().Length - 1

            BaseCreateFiles.Save_txt(sb.ToString(), "CreateClientListRoutine");
        }

        public static void CreateClientUpdateDetailTxt(string sServerName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            string sUserType = sServerName.Substring(0, 3);
            string sTxt = "";

            sb.Append("#region " + sServerName + sExplain + Environment.NewLine);
            sb.Append("public static bool " + CommonUtils.CreateMethodName(sServerName) + "(char cStep)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("try" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("if(cStep!=' ')" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("TRSNode in_node = new TRSNode(\"VIEW_DETAIL_IN\");" + Environment.NewLine);
            sb.Append("TRSNode out_node = new TRSNode(\"VIEW_DETAIL_OUT\");" + Environment.NewLine);
            sb.Append("" + Environment.NewLine);
            sb.Append("CommonRoutine.SetInMsg(in_node);" + Environment.NewLine);
            sb.Append("in_node.ProcStep =cStep;" + Environment.NewLine);
            for (int i = 0; i < aInFields.Length; i++)
            {
                sb.Append("in_node.Add"+CommonUtils.FirstToUpper(aInType[i])+"(\""+aInFields[i].ToUpper()+"\", txtDesc);" + Environment.NewLine);
            }
            sb.Append(" if (CommonRoutine.CallService(\""+sUserType+"\", \""+sServerName+"\", in_node, ref out_node) == false)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("return false;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("return true;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("else" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("return false;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("catch (Exception ex)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("CommonFunction.ShowMsgBox(ex.Message);" + Environment.NewLine);
            sb.Append("return false;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("#endregion ");
            BaseCreateFiles.Save_txt(sb.ToString(), "CreateClientUpdateDetail");

        }
    }
}
