//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class SigElementDefCrud {
		///<summary>Gets one SigElementDef object from the database using the primary key.  Returns null if not found.</summary>
		internal static SigElementDef SelectOne(long sigElementDefNum){
			string command="SELECT * FROM sigelementdef "
				+"WHERE SigElementDefNum = "+POut.Long(sigElementDefNum);
			List<SigElementDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one SigElementDef object from the database using a query.</summary>
		internal static SigElementDef SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SigElementDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of SigElementDef objects from the database using a query.</summary>
		internal static List<SigElementDef> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<SigElementDef> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<SigElementDef> TableToList(DataTable table){
			List<SigElementDef> retVal=new List<SigElementDef>();
			SigElementDef sigElementDef;
			for(int i=0;i<table.Rows.Count;i++) {
				sigElementDef=new SigElementDef();
				sigElementDef.SigElementDefNum= PIn.Long  (table.Rows[i]["SigElementDefNum"].ToString());
				sigElementDef.LightRow        = PIn.Byte  (table.Rows[i]["LightRow"].ToString());
				sigElementDef.LightColor      = Color.FromArgb(PIn.Int(table.Rows[i]["LightColor"].ToString()));
				sigElementDef.SigElementType  = (SignalElementType)PIn.Int(table.Rows[i]["SigElementType"].ToString());
				sigElementDef.SigText         = PIn.String(table.Rows[i]["SigText"].ToString());
				sigElementDef.Sound           = PIn.String(table.Rows[i]["Sound"].ToString());
				sigElementDef.ItemOrder       = PIn.Int   (table.Rows[i]["ItemOrder"].ToString());
				retVal.Add(sigElementDef);
			}
			return retVal;
		}

		///<summary>Inserts one SigElementDef into the database.  Returns the new priKey.</summary>
		internal static long Insert(SigElementDef sigElementDef){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				sigElementDef.SigElementDefNum=DbHelper.GetNextOracleKey("sigelementdef","SigElementDefNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(sigElementDef,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							sigElementDef.SigElementDefNum++;
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
				return Insert(sigElementDef,false);
			}
		}

		///<summary>Inserts one SigElementDef into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(SigElementDef sigElementDef,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				sigElementDef.SigElementDefNum=ReplicationServers.GetKey("sigelementdef","SigElementDefNum");
			}
			string command="INSERT INTO sigelementdef (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SigElementDefNum,";
			}
			command+="LightRow,LightColor,SigElementType,SigText,Sound,ItemOrder) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(sigElementDef.SigElementDefNum)+",";
			}
			command+=
				     POut.Byte  (sigElementDef.LightRow)+","
				+    POut.Int   (sigElementDef.LightColor.ToArgb())+","
				+    POut.Int   ((int)sigElementDef.SigElementType)+","
				+"'"+POut.String(sigElementDef.SigText)+"',"
				+DbHelper.ParamChar+"paramSound,"
				+    POut.Int   (sigElementDef.ItemOrder)+")";
			if(sigElementDef.Sound==null) {
				sigElementDef.Sound="";
			}
			OdSqlParameter paramSound=new OdSqlParameter("paramSound",OdDbType.Text,sigElementDef.Sound);
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command,paramSound);
			}
			else {
				sigElementDef.SigElementDefNum=Db.NonQ(command,true,paramSound);
			}
			return sigElementDef.SigElementDefNum;
		}

		///<summary>Updates one SigElementDef in the database.</summary>
		internal static void Update(SigElementDef sigElementDef){
			string command="UPDATE sigelementdef SET "
				+"LightRow        =  "+POut.Byte  (sigElementDef.LightRow)+", "
				+"LightColor      =  "+POut.Int   (sigElementDef.LightColor.ToArgb())+", "
				+"SigElementType  =  "+POut.Int   ((int)sigElementDef.SigElementType)+", "
				+"SigText         = '"+POut.String(sigElementDef.SigText)+"', "
				+"Sound           =  "+DbHelper.ParamChar+"paramSound, "
				+"ItemOrder       =  "+POut.Int   (sigElementDef.ItemOrder)+" "
				+"WHERE SigElementDefNum = "+POut.Long(sigElementDef.SigElementDefNum);
			if(sigElementDef.Sound==null) {
				sigElementDef.Sound="";
			}
			OdSqlParameter paramSound=new OdSqlParameter("paramSound",OdDbType.Text,sigElementDef.Sound);
			Db.NonQ(command,paramSound);
		}

		///<summary>Updates one SigElementDef in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(SigElementDef sigElementDef,SigElementDef oldSigElementDef){
			string command="";
			if(sigElementDef.LightRow != oldSigElementDef.LightRow) {
				if(command!=""){ command+=",";}
				command+="LightRow = "+POut.Byte(sigElementDef.LightRow)+"";
			}
			if(sigElementDef.LightColor != oldSigElementDef.LightColor) {
				if(command!=""){ command+=",";}
				command+="LightColor = "+POut.Int(sigElementDef.LightColor.ToArgb())+"";
			}
			if(sigElementDef.SigElementType != oldSigElementDef.SigElementType) {
				if(command!=""){ command+=",";}
				command+="SigElementType = "+POut.Int   ((int)sigElementDef.SigElementType)+"";
			}
			if(sigElementDef.SigText != oldSigElementDef.SigText) {
				if(command!=""){ command+=",";}
				command+="SigText = '"+POut.String(sigElementDef.SigText)+"'";
			}
			if(sigElementDef.Sound != oldSigElementDef.Sound) {
				if(command!=""){ command+=",";}
				command+="Sound = "+DbHelper.ParamChar+"paramSound";
			}
			if(sigElementDef.ItemOrder != oldSigElementDef.ItemOrder) {
				if(command!=""){ command+=",";}
				command+="ItemOrder = "+POut.Int(sigElementDef.ItemOrder)+"";
			}
			if(command==""){
				return;
			}
			if(sigElementDef.Sound==null) {
				sigElementDef.Sound="";
			}
			OdSqlParameter paramSound=new OdSqlParameter("paramSound",OdDbType.Text,sigElementDef.Sound);
			command="UPDATE sigelementdef SET "+command
				+" WHERE SigElementDefNum = "+POut.Long(sigElementDef.SigElementDefNum);
			Db.NonQ(command,paramSound);
		}

		///<summary>Deletes one SigElementDef from the database.</summary>
		internal static void Delete(long sigElementDefNum){
			string command="DELETE FROM sigelementdef "
				+"WHERE SigElementDefNum = "+POut.Long(sigElementDefNum);
			Db.NonQ(command);
		}

	}
}