//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class ProcTPCrud {
		///<summary>Gets one ProcTP object from the database using the primary key.  Returns null if not found.</summary>
		internal static ProcTP SelectOne(long procTPNum){
			string command="SELECT * FROM proctp "
				+"WHERE ProcTPNum = "+POut.Long(procTPNum)+" LIMIT 1";
			List<ProcTP> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one ProcTP object from the database using a query.</summary>
		internal static ProcTP SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ProcTP> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of ProcTP objects from the database using a query.</summary>
		internal static List<ProcTP> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<ProcTP> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<ProcTP> TableToList(DataTable table){
			List<ProcTP> retVal=new List<ProcTP>();
			ProcTP procTP;
			for(int i=0;i<table.Rows.Count;i++) {
				procTP=new ProcTP();
				procTP.ProcTPNum   = PIn.Long  (table.Rows[i]["ProcTPNum"].ToString());
				procTP.TreatPlanNum= PIn.Long  (table.Rows[i]["TreatPlanNum"].ToString());
				procTP.PatNum      = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				procTP.ProcNumOrig = PIn.Long  (table.Rows[i]["ProcNumOrig"].ToString());
				procTP.ItemOrder   = PIn.Int   (table.Rows[i]["ItemOrder"].ToString());
				procTP.Priority    = PIn.Long  (table.Rows[i]["Priority"].ToString());
				procTP.ToothNumTP  = PIn.String(table.Rows[i]["ToothNumTP"].ToString());
				procTP.Surf        = PIn.String(table.Rows[i]["Surf"].ToString());
				procTP.ProcCode    = PIn.String(table.Rows[i]["ProcCode"].ToString());
				procTP.Descript    = PIn.String(table.Rows[i]["Descript"].ToString());
				procTP.FeeAmt      = PIn.Double(table.Rows[i]["FeeAmt"].ToString());
				procTP.PriInsAmt   = PIn.Double(table.Rows[i]["PriInsAmt"].ToString());
				procTP.SecInsAmt   = PIn.Double(table.Rows[i]["SecInsAmt"].ToString());
				procTP.PatAmt      = PIn.Double(table.Rows[i]["PatAmt"].ToString());
				procTP.Discount    = PIn.Double(table.Rows[i]["Discount"].ToString());
				retVal.Add(procTP);
			}
			return retVal;
		}

		///<summary>Inserts one ProcTP into the database.  Returns the new priKey.</summary>
		internal static long Insert(ProcTP procTP){
			return Insert(procTP,false);
		}

		///<summary>Inserts one ProcTP into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(ProcTP procTP,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				procTP.ProcTPNum=ReplicationServers.GetKey("proctp","ProcTPNum");
			}
			string command="INSERT INTO proctp (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ProcTPNum,";
			}
			command+="TreatPlanNum,PatNum,ProcNumOrig,ItemOrder,Priority,ToothNumTP,Surf,ProcCode,Descript,FeeAmt,PriInsAmt,SecInsAmt,PatAmt,Discount) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(procTP.ProcTPNum)+",";
			}
			command+=
				     POut.Long  (procTP.TreatPlanNum)+","
				+    POut.Long  (procTP.PatNum)+","
				+    POut.Long  (procTP.ProcNumOrig)+","
				+    POut.Int   (procTP.ItemOrder)+","
				+    POut.Long  (procTP.Priority)+","
				+"'"+POut.String(procTP.ToothNumTP)+"',"
				+"'"+POut.String(procTP.Surf)+"',"
				+"'"+POut.String(procTP.ProcCode)+"',"
				+"'"+POut.String(procTP.Descript)+"',"
				+"'"+POut.Double(procTP.FeeAmt)+"',"
				+"'"+POut.Double(procTP.PriInsAmt)+"',"
				+"'"+POut.Double(procTP.SecInsAmt)+"',"
				+"'"+POut.Double(procTP.PatAmt)+"',"
				+"'"+POut.Double(procTP.Discount)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				procTP.ProcTPNum=Db.NonQ(command,true);
			}
			return procTP.ProcTPNum;
		}

		///<summary>Updates one ProcTP in the database.</summary>
		internal static void Update(ProcTP procTP){
			string command="UPDATE proctp SET "
				+"TreatPlanNum=  "+POut.Long  (procTP.TreatPlanNum)+", "
				+"PatNum      =  "+POut.Long  (procTP.PatNum)+", "
				+"ProcNumOrig =  "+POut.Long  (procTP.ProcNumOrig)+", "
				+"ItemOrder   =  "+POut.Int   (procTP.ItemOrder)+", "
				+"Priority    =  "+POut.Long  (procTP.Priority)+", "
				+"ToothNumTP  = '"+POut.String(procTP.ToothNumTP)+"', "
				+"Surf        = '"+POut.String(procTP.Surf)+"', "
				+"ProcCode    = '"+POut.String(procTP.ProcCode)+"', "
				+"Descript    = '"+POut.String(procTP.Descript)+"', "
				+"FeeAmt      = '"+POut.Double(procTP.FeeAmt)+"', "
				+"PriInsAmt   = '"+POut.Double(procTP.PriInsAmt)+"', "
				+"SecInsAmt   = '"+POut.Double(procTP.SecInsAmt)+"', "
				+"PatAmt      = '"+POut.Double(procTP.PatAmt)+"', "
				+"Discount    = '"+POut.Double(procTP.Discount)+"' "
				+"WHERE ProcTPNum = "+POut.Long(procTP.ProcTPNum);
			Db.NonQ(command);
		}

		///<summary>Updates one ProcTP in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(ProcTP procTP,ProcTP oldProcTP){
			string command="";
			if(procTP.TreatPlanNum != oldProcTP.TreatPlanNum) {
				if(command!=""){ command+=",";}
				command+="TreatPlanNum = "+POut.Long(procTP.TreatPlanNum)+"";
			}
			if(procTP.PatNum != oldProcTP.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(procTP.PatNum)+"";
			}
			if(procTP.ProcNumOrig != oldProcTP.ProcNumOrig) {
				if(command!=""){ command+=",";}
				command+="ProcNumOrig = "+POut.Long(procTP.ProcNumOrig)+"";
			}
			if(procTP.ItemOrder != oldProcTP.ItemOrder) {
				if(command!=""){ command+=",";}
				command+="ItemOrder = "+POut.Int(procTP.ItemOrder)+"";
			}
			if(procTP.Priority != oldProcTP.Priority) {
				if(command!=""){ command+=",";}
				command+="Priority = "+POut.Long(procTP.Priority)+"";
			}
			if(procTP.ToothNumTP != oldProcTP.ToothNumTP) {
				if(command!=""){ command+=",";}
				command+="ToothNumTP = '"+POut.String(procTP.ToothNumTP)+"'";
			}
			if(procTP.Surf != oldProcTP.Surf) {
				if(command!=""){ command+=",";}
				command+="Surf = '"+POut.String(procTP.Surf)+"'";
			}
			if(procTP.ProcCode != oldProcTP.ProcCode) {
				if(command!=""){ command+=",";}
				command+="ProcCode = '"+POut.String(procTP.ProcCode)+"'";
			}
			if(procTP.Descript != oldProcTP.Descript) {
				if(command!=""){ command+=",";}
				command+="Descript = '"+POut.String(procTP.Descript)+"'";
			}
			if(procTP.FeeAmt != oldProcTP.FeeAmt) {
				if(command!=""){ command+=",";}
				command+="FeeAmt = '"+POut.Double(procTP.FeeAmt)+"'";
			}
			if(procTP.PriInsAmt != oldProcTP.PriInsAmt) {
				if(command!=""){ command+=",";}
				command+="PriInsAmt = '"+POut.Double(procTP.PriInsAmt)+"'";
			}
			if(procTP.SecInsAmt != oldProcTP.SecInsAmt) {
				if(command!=""){ command+=",";}
				command+="SecInsAmt = '"+POut.Double(procTP.SecInsAmt)+"'";
			}
			if(procTP.PatAmt != oldProcTP.PatAmt) {
				if(command!=""){ command+=",";}
				command+="PatAmt = '"+POut.Double(procTP.PatAmt)+"'";
			}
			if(procTP.Discount != oldProcTP.Discount) {
				if(command!=""){ command+=",";}
				command+="Discount = '"+POut.Double(procTP.Discount)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE proctp SET "+command
				+" WHERE ProcTPNum = "+POut.Long(procTP.ProcTPNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one ProcTP from the database.</summary>
		internal static void Delete(long procTPNum){
			string command="DELETE FROM proctp "
				+"WHERE ProcTPNum = "+POut.Long(procTPNum);
			Db.NonQ(command);
		}

	}
}