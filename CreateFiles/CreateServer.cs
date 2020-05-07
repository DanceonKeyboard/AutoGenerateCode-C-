using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConTestODBC
{
     public class CreateServer
    {
        #region Variable  读取数据
          public static  string[] aInFields = ReadFiles.ReadInFields();
          public static  string[] aInTitleFields = ReadFiles.ReadInTitleFields();

          public static  string[] aInType = ReadFiles.ReadInType();
          public static  string[] aInTitleType = ReadFiles.ReadInTitleType();

          public static  string[] aOutFields = ReadFiles.ReadOutFields();
          public static  string[] aOutTitleFields = ReadFiles.ReadOutTitleFields();

          public static string[] aOutType = ReadFiles.ReadOutType();
          public static string[] aOutTitleType = ReadFiles.ReadOutTitleType();
        #endregion

        //==============================version-2
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sServerName"></param>
        /// <param name="listName"></param>
        /// <param name="sExplain"></param>
        public static void UserTypeCreateTxt(string sServerName, string listName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            string sTxt = "";
            // string sUserType = sServerName.Substring(0, 3);
            sb.Append(" #region " + sServerName + sExplain + Environment.NewLine);//查看**列表
            sb.Append(CreateInTagStructTxt(sServerName, listName, sExplain));
            sb.Append(CreateOutTagStructTxt(sServerName, listName, sExplain));
            sb.Append(CreateInTagTransformTxt(sServerName, listName, sExplain));
            sb.Append(CreateOutTagSerializeTxt(sServerName, listName, sExplain));

            sb.Append("#endregion" + Environment.NewLine);


            BaseCreateFiles.Save_txt(sb.ToString(), "CreateUserType");
        }
        public static void CreateUserTunerTxt(string sServerName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            string sUserType = sServerName.Substring(0, 3);
            string sTxt = "";
            sb.Append("case \"" + sServerName + "\":" + Environment.NewLine);
            sb.Append("return " + sServerName + "(session, request);" + Environment.NewLine);
            sb.Append("" + Environment.NewLine);
            sb.Append("#region " + sServerName + sExplain + Environment.NewLine);
            sb.Append(" public static int " + sServerName + "(MessageHandler session, MessageRequest request)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append(" try" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append(sUserType + "UserType." + sServerName + "_In_Tag in_tag = new " + sUserType + "UserType." + sServerName + "_In_Tag();" + Environment.NewLine);
            sb.Append(sServerName + "_Out_Tag out_tag = new " + sServerName + "_Out_Tag();" + Environment.NewLine);
            //BASCoreType.Cmn_Out_Tag out_tag = new BASCoreType.Cmn_Out_Tag();
            sb.Append("" + Environment.NewLine);
            sb.Append("TransferAPI.SetTransferResult(MessageConst.SUCCESS, \"\");" + Environment.NewLine);
            sb.Append(sUserType + "UserType.transform_" + sServerName + "_In_Tag(request, ref in_tag);" + Environment.NewLine);
            sb.Append("int iResult = " + sUserType + "UserRoutineImp." + sServerName + "(ref in_tag, ref out_tag);" + Environment.NewLine);
            sb.Append("MessageReply reply = session.CreateReply(request.RequestNode.GetString(MessageConst.MSG_SERVICE));" + Environment.NewLine);
            sb.Append(sUserType + "UserType.serialize_" + sServerName + "_Out_Tag(ref reply, ref out_tag);" + Environment.NewLine);
            sb.Append("reply.ReplyString = TRSConvertorDefine.Convertor.ToXmlString(reply.ReplyNode);" + Environment.NewLine);
            sb.Append("session.SendReply(reply);" + Environment.NewLine);
            sb.Append("return TransferAPI.SetTransferResult(MessageConst.SUCCESS, \"\");" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("catch (Exception ex)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append(" return TransferAPI.SetTransferResult(MessageConst.ERROR, ex.Message);" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);

            sb.Append("#endregion ");
            //sTxt = sb.ToString().Substring(0, sb.ToString().LastIndexOf("\r\n"));//去掉最后一个逗号sb.ToString().Length - 1

            BaseCreateFiles.Save_txt(sb.ToString(), "CreateUserTuner");
        }

        public static void CreateUserRoutineTxt(string sServerName, string listName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            string sUserType = sServerName.Substring(0, 3);
            string sTxt = "";
            string sCmnOut = "";
            if (sServerName.Contains("_View_") || sServerName.Contains("_Update_"))
            {
                sCmnOut = "_cmn_out.";
            }
            sb.Append("" + Environment.NewLine);
            sb.Append("#region " + sServerName + sExplain + Environment.NewLine);
            sb.Append(" public static int " + sServerName + "(ref " + sUserType + "UserType." + sServerName + "_In_Tag InTag, ref " + sServerName + "_Out_Tag OutTag)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);

            sb.Append("#region Variable Define" + Environment.NewLine);
            sb.Append("string sMsgCode = \"\";" + Environment.NewLine);
            sb.Append(" ModelContext ctx = new ModelContext();" + Environment.NewLine);
            if (!sServerName.Contains("_View_"))
            {
                sb.Append("var transaction = ctx.Database.BeginTransaction();" + Environment.NewLine);

            }
            sb.Append("string sFactory = InTag._cmn_in._factory;" + Environment.NewLine);
            if (!sServerName.ToLower().Contains("_item_list"))
            {
                for (int i = 0; i < aInFields.Length; i++)
                {
                    sb.Append(aInType[i] + " " + aInType[i].Substring(0, 1) + CommonUtils.FiledFirstToUpper(aInFields[i]) + " = InTag." + aInFields[i] + ";" + Environment.NewLine);
                }
            }
            if (sServerName.ToLower().Contains("_item_list"))
            {
                for (int i = 0; i < aInTitleFields.Length; i++)
                {
                    sb.Append(aInTitleType[i] + " " + aInTitleType[i].Substring(0, 1) + CommonUtils.FiledFirstToUpper(aInTitleFields[i]) + " = InTag." + aInTitleFields[i] + ";" + Environment.NewLine);
                }
            }
            sb.Append("int index;" + Environment.NewLine);
            sb.Append("#endregion " + Environment.NewLine);

            sb.Append("try" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);

            sb.Append("#region Validation" + Environment.NewLine);
            sb.Append("OutTag." + sCmnOut + "_status_value = GlobalConstant.RESULT_ERROR;" + Environment.NewLine);
            sb.Append("if (InTag._cmn_in._factory == \"\")" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("sMsgCode = \"" + sUserType + "-0004\";" + Environment.NewLine);
            sb.Append("OutTag." + sCmnOut + "_field_msg = \"FACTORY = \" + sFactory;" + Environment.NewLine);
            sb.Append("return GlobalConstant.FAIL;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            if (!sServerName.ToLower().Contains("_item_list"))
            {
                for (int i = 0; i < aInFields.Length; i++)
                {
                    if (aInType[i] == "string")
                    {
                        sb.Append("if (s" + CommonUtils.FiledFirstToUpper(aInFields[i]) + " == \"\")" + Environment.NewLine);
                        sb.Append("{" + Environment.NewLine);
                        sb.Append("s" + CommonUtils.FiledFirstToUpper(aInFields[i]) + " = \" \";" + Environment.NewLine);
                        sb.Append("}" + Environment.NewLine);
                    }
                }
            }
            sb.Append("#endregion " + Environment.NewLine);

            sb.Append("#region  Main" + Environment.NewLine);

            sb.Append(CreateUserRoutineTxt_PartialMain(sServerName, listName, sExplain));//***

            if (!sServerName.Contains("_View_"))
            {
                sb.Append("ctx.SaveChanges();" + Environment.NewLine);
                sb.Append("transaction.Commit();" + Environment.NewLine);

            }

            sb.Append("OutTag." + sCmnOut + "_status_value = GlobalConstant.RESULT_SUCCESS;" + Environment.NewLine);
            sb.Append("#endregion " + Environment.NewLine);

            sb.Append("}" + Environment.NewLine);
            sb.Append("catch (DbUpdateException ex)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("sMsgCode = \"" + sUserType + "-0004\";" + Environment.NewLine);
            sb.Append("OutTag." + sCmnOut + "_db_err_msg = ex.InnerException.Message;" + Environment.NewLine);
            sb.Append("OutTag." + sCmnOut + "_field_msg = \"FACTORY = \" + sFactory ;" + Environment.NewLine);
            sb.Append("OutTag." + sCmnOut + "_status_value = GlobalConstant.RESULT_ERROR;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("catch (Exception ex)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("sMsgCode = \"" + sUserType + "-9999\";" + Environment.NewLine);
            sb.Append("OutTag." + sCmnOut + "_status_value = GlobalConstant.RESULT_ERROR;" + Environment.NewLine);
            sb.Append("OutTag." + sCmnOut + "_db_err_msg = ex.Message;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("finally" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("if (sMsgCode != \"\")" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("OutTag." + sCmnOut + "_msg_code = sMsgCode;" + Environment.NewLine);
            sb.Append("CommonFunction.COM_GetErrorMsg(InTag._cmn_in._language, sMsgCode, ref OutTag." + sCmnOut + "_msg);" + Environment.NewLine);
            sb.Append("GlobalVariable.gLog.AddLog(sMsgCode, OutTag." + sCmnOut + "_msg);" + Environment.NewLine);
            sb.Append("GlobalVariable.gLog.AddLog(\"FIELD_MSG\", OutTag." + sCmnOut + "_field_msg);" + Environment.NewLine);
            sb.Append("GlobalVariable.gLog.LogWrite(\"ERROR \" + CommonFunction.gCOM_Msg_Title, \"E\");" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("else" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("sMsgCode = \"CMN-0000\";" + Environment.NewLine);
            sb.Append("CommonFunction.COM_GetErrorMsg(InTag._cmn_in._language, sMsgCode, ref OutTag." + sCmnOut + "_msg);" + Environment.NewLine);
            if (!sServerName.Contains("_View_"))
            {
                sb.Append("transaction.Dispose();" + Environment.NewLine);
            }
            sb.Append("}" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("return GlobalConstant.SUCCESS;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);


            sb.Append("#endregion ");
            //sTxt = sb.ToString().Substring(0, sb.ToString().LastIndexOf("\r\n"));//去掉最后一个逗号sb.ToString().Length - 1

            BaseCreateFiles.Save_txt(sb.ToString(), "CreateUserRoutine");
        }

        //================================================version-1
        /// <summary>
        /// UserType查看****列表
        /// </summary>
        /// <param name="sServerName"></param>
        /// <param name="listName"></param>
        /// <param name="sExplain"></param>
        /// <returns></returns>
        public static void UserTypeCreateTxtViewList(string sServerName, string listName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            string sTxt = "";
            string sUserType = sServerName.Substring(0, 3);
            //string sMat = System.IO.File.ReadAllText(@"E:\vs2015_project\ODBCtest\ConTestODBC\bin\Debug\MatText.txt");
            
            sb.Append(" #region " + sServerName + sExplain + Environment.NewLine);//查看**列表
            //In_Tag 结构体
            sb.Append(" public struct  " + sServerName + "_In_Tag" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("public BASCoreType.Cmn_In_Tag _cmn_in;" + Environment.NewLine);

            for (int i = 0; i < aInFields.Length; i++)
            {
                sb.Append("public " + aInType[i] + " " + aInFields[i] + ";" + Environment.NewLine);
            }
            sb.Append("}" + Environment.NewLine);
            //Out_Tag 结构体
            sb.Append(" public struct  " + sServerName + "_Out_Tag" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("public BASCoreType.Cmn_Out_Tag _cmn_out;" + Environment.NewLine);
            sb.Append("public int " + listName + "_count;" + Environment.NewLine);
            sb.Append("public int _size_" + listName + "_list;" + Environment.NewLine);
            sb.Append("public " + sServerName + "_Out_Tag_" + listName + "_list[] " + listName + "_list;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            //Out_Tag list
            sb.Append(" public struct  " + sServerName + "_Out_Tag" + "_" + listName + "_list" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            for (int i = 0; i < aOutFields.Length; i++)
            {
                sb.Append("public " + aOutType[i] + " " + aOutFields[i] + ";" + Environment.NewLine);
            }
            sb.Append("}" + Environment.NewLine);
            //In_Tag 转换
            sb.Append(" public static void transform_" + sServerName + "_In_Tag(MessageRequest request, ref " + sServerName + "_In_Tag targetType)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("BASCoreType.transform_Cmn_In_Tag(request, ref targetType._cmn_in);" + Environment.NewLine);
            for (int i = 0; i < aInFields.Length; i++)
            {
                if (aInType[i] == "string")
                {

                    sb.Append("targetType." + aInFields[i] + " = CommonFunction.WithoutSpace(request.RequestNode.GetString(\"" + aInFields[i] + "\".ToUpper()));" + Environment.NewLine);
                }
                else
                {
                    sb.Append("targetType." + aInFields[i] + " = request.RequestNode.Get" + CommonUtils.FirstToUpper(aInType[i]) + "(\"" + aInFields[i] + "\".ToUpper());" + Environment.NewLine);
                }
            }
            sb.Append("}" + Environment.NewLine);
            //TODO
            //In_Tag serialize序列化
            sb.Append(" public static void serialize_" + sServerName + "_Out_Tag(ref MessageReply reply, ref " + sServerName + "_Out_Tag sourceType)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("reply.ReplyNode.AddInt(\"" + listName + "_count\".ToUpper(), sourceType." + listName + "_count);" + Environment.NewLine);
            sb.Append("reply.ReplyNode.AddInt(\"_size_" + listName + "_list\".ToUpper(), sourceType._size_" + listName + "_list);" + Environment.NewLine);
           
            sb.Append("TRSNode " + listName + "_list;" + Environment.NewLine);
            sb.Append("for (int i = 0; i < sourceType._size_" + listName + "_list; i++)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append(listName + "_list = reply.ReplyNode.AddNode(\"" + listName + "_list\".ToUpper());" + Environment.NewLine);
            sb.Append(sUserType + "UserType.serialize_" + sServerName + "_Out_Tag_" + listName + "_list(ref " + listName + "_list, ref sourceType." + listName + "_list[i]);" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("BASCoreType.serialize_Cmn_Out_Tag(ref reply, ref sourceType._cmn_out);" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            


            sb.Append("public static void serialize_" + sServerName + "_Out_Tag_" + listName + "_list(ref TRSNode res_list, ref " + sServerName + "_Out_Tag_" + listName + "_list sourceType)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            if (sServerName.Contains("_View_"))
            {
                for (int i = 0; i < aOutFields.Length; i++)
                {
                    sb.Append("res_list.Add" + CommonUtils.FirstToUpper(aOutType[i]) + "(\"" + aOutFields[i] + "\".ToUpper(), sourceType." + aOutFields[i] + ");" + Environment.NewLine);
                }
            }
            else
            {
                sb.Append("res_list.AddString(\"MsgCode\".ToUpper(), CommonFunction.WithoutSpace(sourceType.MsgCode));" + Environment.NewLine);
            }

            sb.Append("}" + Environment.NewLine);
            sb.Append("#endregion" + Environment.NewLine);

            sTxt = sb.ToString().Substring(0, sb.ToString().LastIndexOf("\r\n"));//去掉最后一个逗号sb.ToString().Length - 1

            BaseCreateFiles.Save_txt(sTxt, "CreateUserType");
        }
        /// <summary>
        /// UserType查看****明细信息
        /// </summary>
        /// <param name="sServerName"></param>
        /// <param name="listName"></param>
        /// <param name="sExplain"></param>
        public static void UserTypeCreateTxtViewDetail(string sServerName, string listName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            string sTxt = "";
            string sUserType = sServerName.Substring(0, 3);
            //string sMat = System.IO.File.ReadAllText(@"E:\vs2015_project\ODBCtest\ConTestODBC\bin\Debug\MatText.txt");

            sb.Append(" #region " + sServerName + sExplain + Environment.NewLine);//查看**列表
            //In_Tag 结构体
            sb.Append(" public struct  " + sServerName + "_In_Tag" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("public BASCoreType.Cmn_In_Tag _cmn_in;" + Environment.NewLine);

            for (int i = 0; i < aInFields.Length; i++)
            {
                sb.Append("public " + aInType[i] + " " + aInFields[i] + ";" + Environment.NewLine);
            }
            sb.Append("}" + Environment.NewLine);
            //Out_Tag 结构体
            sb.Append(" public struct  " + sServerName + "_Out_Tag" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("public BASCoreType.Cmn_Out_Tag _cmn_out;" + Environment.NewLine);
            sb.Append("public int " + listName + "_count;" + Environment.NewLine);
            sb.Append("public int _size_" + listName + "_list;" + Environment.NewLine);
            sb.Append("public " + sServerName + "_Out_Tag_" + listName + "_list[] " + listName + "_list;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            //Out_Tag list
            sb.Append(" public struct  " + sServerName + "_Out_Tag" + "_" + listName + "_list" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            for (int i = 0; i < aOutFields.Length; i++)
            {
                sb.Append("public " + aOutType[i] + " " + aOutFields[i] + ";" + Environment.NewLine);
            }
            sb.Append("}" + Environment.NewLine);
            //In_Tag 转换
            sb.Append(" public static void transform_" + sServerName + "_In_Tag(MessageRequest request, ref " + sServerName + "_In_Tag targetType)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("BASCoreType.transform_Cmn_In_Tag(request, ref targetType._cmn_in);" + Environment.NewLine);
            for (int i = 0; i < aInFields.Length; i++)
            {
                if (aInType[i] == "string")
                {

                    sb.Append("targetType." + aInFields[i] + " = CommonFunction.WithoutSpace(request.RequestNode.GetString(\"" + aInFields[i] + "\".ToUpper()));" + Environment.NewLine);
                }
                else
                {
                    sb.Append("targetType." + aInFields[i] + " = request.RequestNode.Get" + CommonUtils.FirstToUpper(aInType[i]) + "(\"" + aInFields[i] + "\".ToUpper());" + Environment.NewLine);
                }
            }
            sb.Append("}" + Environment.NewLine);
            //TODO
            //In_Tag serialize序列化
            sb.Append(" public static void serialize_" + sServerName + "_Out_Tag(ref MessageReply reply, ref " + sServerName + "_Out_Tag sourceType)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("reply.ReplyNode.AddInt(\"" + listName + "_count\".ToUpper(), sourceType." + listName + "_count);" + Environment.NewLine);
            sb.Append("reply.ReplyNode.AddInt(\"_size_" + listName + "_list\".ToUpper(), sourceType._size_" + listName + "_list);" + Environment.NewLine);
            sb.Append(CreateDetailSerializeTxt());
           
            sb.Append("public static void serialize_" + sServerName + "_Out_Tag_" + listName + "_list(ref TRSNode res_list, ref " + sServerName + "_Out_Tag_" + listName + "_list sourceType)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            if (sServerName.Contains("_View_"))
            {
                for (int i = 0; i < aOutFields.Length; i++)
                {
                    sb.Append("res_list.Add" + CommonUtils.FirstToUpper(aOutType[i]) + "(\"" + aOutFields[i] + "\".ToUpper(), sourceType." + aOutFields[i] + ");" + Environment.NewLine);
                }
            }
            else
            {
                sb.Append("res_list.AddString(\"MsgCode\".ToUpper(), CommonFunction.WithoutSpace(sourceType.MsgCode));" + Environment.NewLine);
            }

            sb.Append("}" + Environment.NewLine);
            sb.Append("#endregion" + Environment.NewLine);

            sTxt = sb.ToString().Substring(0, sb.ToString().LastIndexOf("\r\n"));//去掉最后一个逗号sb.ToString().Length - 1

            BaseCreateFiles.Save_txt(sTxt, "CreateUserType");
        }
        public static void CreateUserTypeTxt(string sServerName, string listName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            string sTxt = "";
            string sUserType = sServerName.Substring(0, 3);
            //string sMat = System.IO.File.ReadAllText(@"E:\vs2015_project\ODBCtest\ConTestODBC\bin\Debug\MatText.txt");
            sb.Append(" #region " + sServerName + sExplain + Environment.NewLine);//查看**列表
            //In_Tag 结构体
            sb.Append(" public struct  " + sServerName + "_In_Tag" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("public BASCoreType.Cmn_In_Tag _cmn_in;" + Environment.NewLine);
            if(sServerName.ToLower().Contains("_item_list"))
            {
                sb.Append("public "+sServerName+ "_In_Tag_item_list[] " + listName + "_in_list;" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
                sb.Append(" public struct " + sServerName + "_In_Tag_item_list" + Environment.NewLine);

                sb.Append("{" + Environment.NewLine);
                for (int i = 0; i < aInFields.Length; i++)
                {
                    sb.Append("public " + aInType[i] + " " + aInFields[i] + ";" + Environment.NewLine);
                }
                sb.Append("}" + Environment.NewLine);
            }
            else
            {
                for (int i = 0; i < aInFields.Length; i++)
                {
                    sb.Append("public " + aInType[i] + " " + aInFields[i] + ";" + Environment.NewLine);
                }
                sb.Append("}" + Environment.NewLine);
            }
           

            //Out_Tag 结构体
            sb.Append(" public struct  " + sServerName + "_Out_Tag" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("public BASCoreType.Cmn_Out_Tag _cmn_out;" + Environment.NewLine);
            sb.Append("public int " + listName + "_count;" + Environment.NewLine);
            sb.Append("public int _size_" + listName + "_list;" + Environment.NewLine);
            sb.Append("public " + sServerName + "_Out_Tag_" + listName + "_list[] " + listName + "_list;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            //Out_Tag list
            sb.Append(" public struct  " + sServerName + "_Out_Tag" + "_" + listName + "_list" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            for (int i = 0; i < aOutFields.Length; i++)
            {
                sb.Append("public " + aOutType[i] + " " + aOutFields[i] + ";" + Environment.NewLine);
            }
            sb.Append("}" + Environment.NewLine);


            //In_Tag 
            sb.Append(" public static void transform_" + sServerName + "_In_Tag(MessageRequest request, ref " + sServerName + "_In_Tag targetType)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("BASCoreType.transform_Cmn_In_Tag(request, ref targetType._cmn_in);" + Environment.NewLine);
            for (int i = 0; i < aInFields.Length; i++)
            {
                if (aInType[i] == "string")
                {

                    sb.Append("targetType." + aInFields[i] + " = CommonFunction.WithoutSpace(request.RequestNode.GetString(\"" + aInFields[i] + "\".ToUpper()));" + Environment.NewLine);
                }
                else
                {
                    sb.Append("targetType." + aInFields[i] + " = request.RequestNode.Get" + CommonUtils.FirstToUpper(aInType[i]) + "(\"" + aInFields[i] + "\".ToUpper());" + Environment.NewLine);
                }
            }
            sb.Append("}" + Environment.NewLine);
            //TODO
            sb.Append(" public static void serialize_" + sServerName + "_Out_Tag(ref MessageReply reply, ref " + sServerName + "_Out_Tag sourceType)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("reply.ReplyNode.AddInt(\"" + listName + "_count\".ToUpper(), sourceType." + listName + "_count);" + Environment.NewLine);
            sb.Append("reply.ReplyNode.AddInt(\"_size_" + listName + "_list\".ToUpper(), sourceType._size_" + listName + "_list);" + Environment.NewLine);
            if (sExplain.Contains("明细信息"))
            {
                sb.Append( CreateDetailSerializeTxt());
            }
            else
            {
                sb.Append("TRSNode " + listName + "_list;" + Environment.NewLine);
                sb.Append("for (int i = 0; i < sourceType._size_" + listName + "_list; i++)" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                sb.Append(listName + "_list = reply.ReplyNode.AddNode(\"" + listName + "_list\".ToUpper());" + Environment.NewLine);
                sb.Append(sUserType + "UserType.serialize_" + sServerName + "_Out_Tag_" + listName + "_list(ref " + listName + "_list, ref sourceType." + listName + "_list[i]);" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
                sb.Append("BASCoreType.serialize_Cmn_Out_Tag(ref reply, ref sourceType._cmn_out);" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
            }


            sb.Append("public static void serialize_" + sServerName + "_Out_Tag_" + listName + "_list(ref TRSNode res_list, ref " + sServerName + "_Out_Tag_" + listName + "_list sourceType)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            if (sServerName.Contains("_View_"))
            {
                for (int i = 0; i < aOutFields.Length; i++)
                {
                    sb.Append("res_list.Add" + CommonUtils.FirstToUpper(aOutType[i]) + "(\"" + aOutFields[i] + "\".ToUpper(), sourceType." + aOutFields[i] + ");" + Environment.NewLine);
                }
            }
            else
            {
                sb.Append("res_list.AddString(\"MsgCode\".ToUpper(), CommonFunction.WithoutSpace(sourceType.MsgCode));" + Environment.NewLine);
            }

            sb.Append("BASCoreType.serialize_Cmn_Out_Tag(ref reply, ref sourceType._cmn_out);" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("#endregion" + Environment.NewLine);

            sTxt = sb.ToString().Substring(0, sb.ToString().LastIndexOf("\r\n"));//去掉最后一个逗号sb.ToString().Length - 1

            BaseCreateFiles.Save_txt(sTxt, "CreateUserType");
        }
        #region UserTypeCreateTxt 
        
        public static string CreateInTagStructTxt(string sServerName, string listName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" public struct  " + sServerName + "_In_Tag" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("public BASCoreType.Cmn_In_Tag _cmn_in;" + Environment.NewLine);

            if (sServerName.ToLower().Contains("_item_list")&& sServerName.ToLower().Contains("_update_"))
            {
                for (int i = 0; i < aInTitleFields.Length; i++)
                {
                    sb.Append(" public " + aInTitleType[i] + " " + aInTitleFields[i]+";" + Environment.NewLine);

                }
                sb.Append("public " + sServerName + "_In_Tag_item_list[] " + listName + "_in_list;" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);

                sb.Append(" public struct " + sServerName + "_In_Tag_item_list" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                sb.Append("public char cStep;" + Environment.NewLine);
                for (int i = 0; i < aInFields.Length; i++)
                {
                    sb.Append("public " + aInType[i] + " " + aInFields[i] + ";" + Environment.NewLine);
                }
                sb.Append("}" + Environment.NewLine);
            }
            else
            {
                for (int i = 0; i < aInFields.Length; i++)
                {
                    sb.Append("public " + aInType[i] + " " + aInFields[i] + ";" + Environment.NewLine);
                }
                sb.Append("}" + Environment.NewLine);
            }

            return sb.ToString();
        }

        public static string CreateInTagTransformTxt(string sServerName, string listName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" public static void transform_" + sServerName + "_In_Tag(MessageRequest request, ref " + sServerName + "_In_Tag targetType)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("BASCoreType.transform_Cmn_In_Tag(request, ref targetType._cmn_in);" + Environment.NewLine);
            if (sServerName.ToLower().Contains("_item_list") && sServerName.ToLower().Contains("_update_"))
            {
                for (int i = 0; i < aInTitleFields.Length; i++)
                {
                    //sb.Append(" public " + aInTitleType[i] + " " + aInTitleFields[i] + ";" + Environment.NewLine);
                    if (aInTitleType[i] == "string")
                    {
                        sb.Append("targetType." + aInTitleFields[i] + " = CommonFunction.WithoutSpace(request.RequestNode.GetString(\"" + aInTitleFields[i] + "\".ToUpper()));" + Environment.NewLine);
                    }
                    else
                    {
                        sb.Append("targetType." + aInTitleFields[i] + " = request.RequestNode.Get"+CommonUtils.FiledFirstToUpper(aInTitleType[i])+ "(\"" + aInTitleFields[i] + "\".ToUpper());" + Environment.NewLine);
                    }
                }
                sb.Append("TRSNode item_list;" + Environment.NewLine);
                sb.Append("if (request.RequestNode.GetList(0).Count > 0)" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                sb.Append("int p = request.RequestNode.GetList(0).Count;" + Environment.NewLine);
                sb.Append("Array.Resize(ref targetType." + listName + "_in_list, p);" + Environment.NewLine);
                sb.Append("for (int i = 0; i < p; i++)" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                sb.Append(" item_list = (TRSNode)request.RequestNode.GetList(0)[i];" + Environment.NewLine);
                sb.Append(" serialize_" + sServerName + "_In_Tag_item_list(ref item_list, ref targetType."+listName+"_in_list[i]);" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);

                sb.Append("public static void serialize_" + sServerName + "_In_Tag_item_list(ref TRSNode res_list, ref " + sServerName+"_In_Tag_item_list sourceType)" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                sb.Append("sourceType.cStep= res_list.ProcStep;" + Environment.NewLine);
                for (int i = 0; i < aInFields.Length; i++)
                {
                    sb.Append("sourceType." + aInFields[i] + " = res_list.Get" + CommonUtils.FirstToUpper(aInType[i]) + "(\"" + aInFields[i] + "\".ToUpper()); " + Environment.NewLine);
                }
                sb.Append("}" + Environment.NewLine);
            }
            else
            {
                for (int i = 0; i < aInFields.Length; i++)
                {
                    sb.Append("targetType." + aInFields[i] + " = request.RequestNode.Get" + CommonUtils.FirstToUpper(aInType[i]) + "(\"" + aInFields[i] + "\".ToUpper());" + Environment.NewLine);
                }
                sb.Append("}" + Environment.NewLine);
            }
            return sb.ToString();
        }

        public static string CreateOutTagStructTxt(string sServerName, string listName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" public struct  " + sServerName + "_Out_Tag" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("public BASCoreType.Cmn_Out_Tag _cmn_out;" + Environment.NewLine);
            sb.Append("public int " + listName + "_count;" + Environment.NewLine);
            sb.Append("public int _size_" + listName + "_list;" + Environment.NewLine);
            if (sExplain.Contains("明细信息"))
            {
                for (int i = 0; i < aOutFields.Length; i++)
                {
                    sb.Append("public " + aOutType[i] + " " + aOutFields[i] + ";" + Environment.NewLine);
                }
                sb.Append("}" + Environment.NewLine);
            }
            else
            {
                sb.Append("public " + sServerName + "_Out_Tag_" + listName + "_list[] " + listName + "_list;" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
                sb.Append("public struct " + sServerName + "_Out_Tag_" + listName + "_list" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                for (int i = 0; i < aOutFields.Length; i++)
                {
                    sb.Append("public " + aOutType[i] + " " + aOutFields[i] + ";" + Environment.NewLine);
                }
                sb.Append("}" + Environment.NewLine);
            }
            return sb.ToString();
        }

        public static string CreateOutTagSerializeTxt(string sServerName, string listName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            string sUserType = sServerName.Substring(0, 3);
            sb.Append("public static void serialize_"+ sServerName + "_Out_Tag(ref MessageReply reply, ref " + sServerName + "_Out_Tag sourceType)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("reply.ReplyNode.AddInt(\""+listName+ "_count\".ToUpper(), sourceType." + listName + "_count);" + Environment.NewLine);
            sb.Append("reply.ReplyNode.AddInt(\"_size_" + listName + "_list\".ToUpper(), sourceType._size_" + listName + "_list);" + Environment.NewLine);
            if (!sExplain.Contains("明细信息"))
            {
                sb.Append("TRSNode "+listName+"_list;" + Environment.NewLine);
                sb.Append("for (int i = 0; i < sourceType._size_" + listName + "_list; i++)" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                sb.Append("" + listName + "_list = reply.ReplyNode.AddNode(\"" + listName + "_list\".ToUpper());" + Environment.NewLine);
                sb.Append(sUserType+ "UserType.serialize_" + sServerName + "_Out_Tag_" + listName + "_list(ref " + listName + "_list, ref sourceType." + listName + "_list[i]);" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
                sb.Append(" BASCoreType.serialize_Cmn_Out_Tag(ref reply, ref sourceType._cmn_out);" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);


                sb.Append("public static void serialize_" + sServerName + "_Out_Tag_" + listName + "_list(ref TRSNode res_list, ref " + sServerName + "_Out_Tag_" + listName + "_list sourceType)" + Environment.NewLine);
                sb.Append("{" + Environment.NewLine);
                for (int i = 0; i < aOutFields.Length; i++)
                {
                    sb.Append("res_list.Add"+CommonUtils.FirstToUpper(aOutType[i]) +"(\""+ aOutFields[i] + "\".ToUpper(), sourceType."+aOutFields[i]+");" + Environment.NewLine);
                }
                sb.Append("}" + Environment.NewLine);

            }
            else
            {
                for (int i = 0; i < aOutFields.Length; i++)
                {
                    sb.Append("reply.ReplyNode.Add" + CommonUtils.FirstToUpper(aOutType[i]) + "(\""+aOutFields[i]+ "\".ToUpper(), sourceType." + aOutFields[i] + ");" + Environment.NewLine);
                }

                sb.Append(" BASCoreType.serialize_Cmn_Out_Tag(ref reply, ref sourceType._cmn_out);" + Environment.NewLine);
                sb.Append("}" + Environment.NewLine);
            }

            return sb.ToString();
        }
        #endregion
        public static string CreateDetailSerializeTxt()
        {
            string sTxt = "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < aOutFields.Length; i++)
            {
                sb.Append("reply.ReplyNode.Add"+ CommonUtils.FirstToUpper(aOutType[i])+ "(\""+ aOutFields[i] + "\".ToUpper(), sourceType." + aOutFields[i] + ");" + Environment.NewLine);
            }
            sb.Append("BASCoreType.serialize_Cmn_Out_Tag(ref reply, ref sourceType._cmn_out);" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            return sb.ToString();
        }
        public static string CreateUserRoutineTxt_PartialMain(string sServerName, string listName, string sExplain = "   ")
        {
            string Result = "";
            if (sServerName.ToLower().Contains("_view_")&&sServerName.ToLower().Contains("_list"))
            {
                Result = CreateUserRoutineV_listTxt_PartialMain(sServerName, listName, sExplain);
            }
            else if (sServerName.ToLower().Contains("_view_") && sExplain.Contains("明细信息"))
            {
                Result = CreateUserRoutineV_DetailTxt_PartialMain(sServerName, listName, sExplain);
            }
            else if (sServerName.ToLower().Contains("_update_") && sServerName.ToLower().Contains("_list"))
            {
                Result = CreateUserRoutineU_ListTxt_PartialMain(sServerName, listName, sExplain);
            }
            else if (sServerName.ToLower().Contains("_update_") && (!sServerName.ToLower().Contains("_list")))
            {
                Result = CreateUserRoutineU_DetailTxt_PartialMain(sServerName, listName, sExplain);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("WARNING:无法识别操作类型,请手动指定!");
            }
            return Result;
        }
        #region ViewList 查看**列表
        public static string CreateUserRoutineV_listTxt_PartialMain(string sServerName, string listName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            string sUserType = sServerName.Substring(0, 3);
            string sTxt = "";
            sb.Append("string TableName;" + Environment.NewLine);
            sb.Append("List<TableName> TableNamelist = ctx.TableName.Where(t => t.Factory == sFactory).ToList();" + Environment.NewLine);
            sb.Append("#endregion " + Environment.NewLine);

            sb.Append(" #region Output" + Environment.NewLine);

            sb.Append("if (TableNamelist != null&&TableNamelist.Count>0)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("int iRowCount = TableNamelist.Count;" + Environment.NewLine);
            sb.Append("index = 0;" + Environment.NewLine);

            sb.Append("Array.Resize(ref OutTag." + listName + "_list, iRowCount > GlobalVariable.giMaxRecordsRows ? GlobalVariable.giMaxRecordsRows : iRowCount);" + Environment.NewLine);
            sb.Append("for (index = 0; index < TableNamelist.Count; index++)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);

            for (int i = 0; i < aOutFields.Length; i++)
            {
                sb.Append("OutTag." + listName + "_list[index]." + aOutFields[i] + " = TableNamelist[index]." + CommonUtils.FiledFirstToUpper(aOutFields[i]) + ";" + Environment.NewLine);

            }
            sb.Append("}" + Environment.NewLine);

            sb.Append("OutTag." + listName + "_count = index;" + Environment.NewLine);
            sb.Append("OutTag._size_" + listName + "_list = index;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            return sb.ToString();
        }

        #endregion

        #region ViewDetail 查看**明细信息
        public static string CreateUserRoutineV_DetailTxt_PartialMain(string sServerName, string listName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            string sUserType = sServerName.Substring(0, 3);
            string sTxt = "";
            sb.Append("string TableName;" + Environment.NewLine);
            sb.Append("TableName TableName = ctx.TableName.FirstOrDefault(t=>t.Factory==sFactory);" + Environment.NewLine);
            sb.Append("#endregion " + Environment.NewLine);

            sb.Append(" #region Output" + Environment.NewLine);
            sb.Append("if (TableName != null)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            for (int i = 0; i < aOutFields.Length; i++)
            {
                sb.Append(" OutTag."+aOutFields[i]+ " = uWipprocconfset." + CommonUtils.FiledFirstToUpper(aOutFields[i]) + ";" + Environment.NewLine);
            }
            
            sb.Append("}" + Environment.NewLine);
            
            sb.Append("else" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("sMsgCode = \""+sUserType+"-0557\";//TODO" + Environment.NewLine);
            sb.Append("OutTag._cmn_out._field_msg = \"FACTORY=\" + sFactory;" + Environment.NewLine);
            sb.Append("return GlobalConstant.FAIL;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            return sb.ToString();
        }

        #endregion

        #region updateList 更新**列表
        public static string CreateUserRoutineU_ListTxt_PartialMain(string sServerName, string listName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            string sUserType = sServerName.Substring(0, 3);
            string sTxt = "";
            sb.Append("if (InTag."+listName+"_in_list != null) " + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("string TableName;" + Environment.NewLine);
            sb.Append("List<TableName> TableNamelist1 = ctx.TableName.Where(t =>t.Factory == sFactory).ToList();" + Environment.NewLine);

            sb.Append("if (TableNamelist1 != null&&TableNamelist1.Count>0)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("ctx.TableName.RemoveRange(TableNamelist1);" + Environment.NewLine);
            sb.Append("ctx.SaveChanges();" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("for (int i = 0; i < InTag."+listName+"_in_list.Length; i++)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);

            for (int i = 0; i < aInFields.Length; i++)
            {
                sb.Append(aInType[i]+" "+ aInType[i].Substring(0, 1) + CommonUtils.FiledFirstToUpper(aInFields[i]) + " = InTag."+listName+"_in_list[i]."+aInFields[i]+";" + Environment.NewLine);
            }
            sb.Append("if (InTag."+listName+"_in_list[i].cStep == 'A')" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            for (int i = 0; i < aInFields.Length; i++)
            {
                if (aInType[i] == "string")
                {
                    sb.Append("if (s" + CommonUtils.FiledFirstToUpper(aInFields[i]) + " == \"\")" + Environment.NewLine);
                    sb.Append("{" + Environment.NewLine);
                    sb.Append("s" + CommonUtils.FiledFirstToUpper(aInFields[i]) + " = \" \";" + Environment.NewLine);
                    sb.Append("}" + Environment.NewLine);
                }
            }
            sb.Append("TableNamerel tableNamerel = ctx.TableNamerel.Where(t => t.Factory == sFactory).FirstOrDefault();" + Environment.NewLine);
            sb.Append("" + Environment.NewLine);
            for (int i = 0; i < aInFields.Length; i++)
            {
               
                sb.Append("tableNamerel." + CommonUtils.FiledFirstToUpper(aInFields[i]) + " = " + aInType[i].Substring(0, 1) + CommonUtils.FiledFirstToUpper(aInFields[i]) + ";" + Environment.NewLine);
                
            }
            sb.Append("tableNamerel.UpdateTime = DBGV._dbc.DB_GetSysTime();" + Environment.NewLine);
            sb.Append("tableNamerel.UpdateUserId = InTag._cmn_in._user_id;" + Environment.NewLine);
            sb.Append("ctx.tableNamerel.Update(tableNamerel);" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("else" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("sMsgCode = \""+sUserType+"-0004\";" + Environment.NewLine);
            sb.Append("return GlobalConstant.FAIL;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("else" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("List<TableNamerel> tableNamerellist1 = ctx.TableNamerel.Where(t => t.Factory == sFactory ).ToList();" + Environment.NewLine);
            sb.Append("if (tableNamerellist1 != null && tableNamerellist1.Count > 0)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("ctx.TableNamerel.RemoveRange(tableNamerellist1);" + Environment.NewLine);
            sb.Append("ctx.SaveChanges();" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);

            return sb.ToString();
        }

        #endregion

        #region updateDetail 更新**明细信息
        public static string CreateUserRoutineU_DetailTxt_PartialMain(string sServerName, string listName, string sExplain = "   ")
        {
            StringBuilder sb = new StringBuilder();
            string sUserType = sServerName.Substring(0, 3);
            string sTxt = "";
            sb.Append("if (InTag._cmn_in._proc_step == \"I\")" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("string TableName;" + Environment.NewLine);
            sb.Append("TableName TableName = ctx.TableName.FirstOrDefault(t=>t.Factory==sFactory);" + Environment.NewLine);

            sb.Append("if (TableName != null)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("sMsgCode = \""+sUserType+"-0554\";//TODO 提示已存在" + Environment.NewLine);
            sb.Append("OutTag._cmn_out._field_msg = \"FACTORY = \" + sFactory;" + Environment.NewLine);
            sb.Append("return GlobalConstant.FAIL;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);

            sb.Append("else" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("TableName = new TableName()" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("Factory = sFactory," + Environment.NewLine);
            for (int i = 0; i < aInFields.Length; i++)
            {
                if (CommonUtils.FiledFirstToUpper(aInFields[i]).ToLower() != "createtime" || CommonUtils.FiledFirstToUpper(aInFields[i]).ToLower() != "createuserid")
                {
                    sb.Append(CommonUtils.FiledFirstToUpper(aInFields[i]) + " = "+ aInType[i].Substring(0, 1) + CommonUtils.FiledFirstToUpper(aInFields[i]) +","+ Environment.NewLine);
                }
            }
            sb.Append("CreateTime = DBGV._dbc.DB_GetSysTime(),"+ Environment.NewLine);
            sb.Append("CreateUserId = InTag._cmn_in._user_id" + Environment.NewLine);
            sb.Append("};" + Environment.NewLine);
            sb.Append("ctx.TableName.Add(TableName);" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);

            sb.Append("if (InTag._cmn_in._proc_step == \"U\")" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("TableName TableName = ctx.TableName.FirstOrDefault(t=>t.Factory==sFactory);" + Environment.NewLine);

            sb.Append("if (TableName == null)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("sMsgCode = \"" + sUserType + "-0554\";//TODO 提示不存在" + Environment.NewLine);
            sb.Append("OutTag._cmn_out._field_msg = \"FACTORY = \" + sFactory;" + Environment.NewLine);
            sb.Append("return GlobalConstant.FAIL;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            for (int i = 0; i < aInFields.Length; i++)
            {
                if (CommonUtils.FiledFirstToUpper(aInFields[i]).ToLower() != "createtime" || CommonUtils.FiledFirstToUpper(aInFields[i]).ToLower() != "createuserid")
                {
                    sb.Append("TableName."+CommonUtils.FiledFirstToUpper(aInFields[i]) + " = " + aInType[i].Substring(0, 1) + CommonUtils.FiledFirstToUpper(aInFields[i])+";" + Environment.NewLine);
                }
            }
            sb.Append("TableName.UpdateTime = DBGV._dbc.DB_GetSysTime();" + Environment.NewLine);
            sb.Append("TableName.UpdateUserId = InTag._cmn_in._user_id;" + Environment.NewLine);
            sb.Append("ctx.TableName.Update(TableName);" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);

            sb.Append("if (InTag._cmn_in._proc_step == \"D\")" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("TableName TableName = ctx.TableName.FirstOrDefault(t=>t.Factory==sFactory);" + Environment.NewLine);

            sb.Append("if (TableName == null)" + Environment.NewLine);
            sb.Append("{" + Environment.NewLine);
            sb.Append("sMsgCode = \"" + sUserType + "-0554\";//TODO 提示不存在" + Environment.NewLine);
            sb.Append("OutTag._cmn_out._field_msg = \"FACTORY = \" + sFactory;" + Environment.NewLine);
            sb.Append("return GlobalConstant.FAIL;" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            sb.Append("ctx.TableName.Remove(TableName);" + Environment.NewLine);
            sb.Append("}" + Environment.NewLine);
            return sb.ToString();
        }

        #endregion
    }
}
