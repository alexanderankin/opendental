//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class HL7MsgCrud {
		///<summary>Gets one HL7Msg object from the database using the primary key.  Returns null if not found.</summary>
		internal static HL7Msg SelectOne(long hL7MsgNum){
			string command="SELECT * FROM hl7msg "
				+"WHERE HL7MsgNum = "+POut.Long(hL7MsgNum);
			List<HL7Msg> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one HL7Msg object from the database using a query.</summary>
		internal static HL7Msg SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<HL7Msg> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of HL7Msg objects from the database using a query.</summary>
		internal static List<HL7Msg> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<HL7Msg> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<HL7Msg> TableToList(DataTable table){
			List<HL7Msg> retVal=new List<HL7Msg>();
			HL7Msg hL7Msg;
			for(int i=0;i<table.Rows.Count;i++) {
				hL7Msg=new HL7Msg();
				hL7Msg.HL7MsgNum= PIn.Long  (table.Rows[i]["HL7MsgNum"].ToString());
				hL7Msg.HL7Status= (HL7MessageStatus)PIn.Int(table.Rows[i]["HL7Status"].ToString());
				hL7Msg.MsgText  = PIn.String(table.Rows[i]["MsgText"].ToString());
				hL7Msg.AptNum   = PIn.Long  (table.Rows[i]["AptNum"].ToString());
				retVal.Add(hL7Msg);
			}
			return retVal;
		}

		///<summary>Inserts one HL7Msg into the database.  Returns the new priKey.</summary>
		internal static long Insert(HL7Msg hL7Msg){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				hL7Msg.HL7MsgNum=DbHelper.GetNextOracleKey("hl7msg","HL7MsgNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(hL7Msg,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							hL7Msg.HL7MsgNum++;
							loopcount++;
						}
						else{
							throw ex;
						}
					}
				}
				throw new ApplicationException("Insert failed.  Could not generate primary key.");
			}
			else {
				return Insert(hL7Msg,false);
			}
		}

		///<summary>Inserts one HL7Msg into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(HL7Msg hL7Msg,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				hL7Msg.HL7MsgNum=ReplicationServers.GetKey("hl7msg","HL7MsgNum");
			}
			string command="INSERT INTO hl7msg (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="HL7MsgNum,";
			}
			command+="HL7Status,MsgText,AptNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(hL7Msg.HL7MsgNum)+",";
			}
			command+=
				     POut.Int   ((int)hL7Msg.HL7Status)+","
				+DbHelper.ParamChar+"paramMsgText,"
				+    POut.Long  (hL7Msg.AptNum)+")";
			if(hL7Msg.MsgText==null) {
				hL7Msg.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,hL7Msg.MsgText);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramMsgText);
			}
			else {
				hL7Msg.HL7MsgNum=Db.NonQ(command,true,paramMsgText);
			}
			return hL7Msg.HL7MsgNum;
		}

		///<summary>Updates one HL7Msg in the database.</summary>
		internal static void Update(HL7Msg hL7Msg){
			string command="UPDATE hl7msg SET "
				+"HL7Status=  "+POut.Int   ((int)hL7Msg.HL7Status)+", "
				+"MsgText  =  "+DbHelper.ParamChar+"paramMsgText, "
				+"AptNum   =  "+POut.Long  (hL7Msg.AptNum)+" "
				+"WHERE HL7MsgNum = "+POut.Long(hL7Msg.HL7MsgNum);
			if(hL7Msg.MsgText==null) {
				hL7Msg.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,hL7Msg.MsgText);
			Db.NonQ(command,paramMsgText);
		}

		///<summary>Updates one HL7Msg in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(HL7Msg hL7Msg,HL7Msg oldHL7Msg){
			string command="";
			if(hL7Msg.HL7Status != oldHL7Msg.HL7Status) {
				if(command!=""){ command+=",";}
				command+="HL7Status = "+POut.Int   ((int)hL7Msg.HL7Status)+"";
			}
			if(hL7Msg.MsgText != oldHL7Msg.MsgText) {
				if(command!=""){ command+=",";}
				command+="MsgText = "+DbHelper.ParamChar+"paramMsgText";
			}
			if(hL7Msg.AptNum != oldHL7Msg.AptNum) {
				if(command!=""){ command+=",";}
				command+="AptNum = "+POut.Long(hL7Msg.AptNum)+"";
			}
			if(command==""){
				return;
			}
			if(hL7Msg.MsgText==null) {
				hL7Msg.MsgText="";
			}
			OdSqlParameter paramMsgText=new OdSqlParameter("paramMsgText",OdDbType.Text,hL7Msg.MsgText);
			command="UPDATE hl7msg SET "+command
				+" WHERE HL7MsgNum = "+POut.Long(hL7Msg.HL7MsgNum);
			Db.NonQ(command,paramMsgText);
		}

		///<summary>Deletes one HL7Msg from the database.</summary>
		internal static void Delete(long hL7MsgNum){
			string command="DELETE FROM hl7msg "
				+"WHERE HL7MsgNum = "+POut.Long(hL7MsgNum);
			Db.NonQ(command);
		}

	}
}