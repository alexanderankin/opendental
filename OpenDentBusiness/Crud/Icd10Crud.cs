//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class Icd10Crud {
		///<summary>Gets one Icd10 object from the database using the primary key.  Returns null if not found.</summary>
		public static Icd10 SelectOne(long icd10Num){
			string command="SELECT * FROM icd10 "
				+"WHERE Icd10Num = "+POut.Long(icd10Num);
			List<Icd10> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Icd10 object from the database using a query.</summary>
		public static Icd10 SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Icd10> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Icd10 objects from the database using a query.</summary>
		public static List<Icd10> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Icd10> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Icd10> TableToList(DataTable table){
			List<Icd10> retVal=new List<Icd10>();
			Icd10 icd10;
			for(int i=0;i<table.Rows.Count;i++) {
				icd10=new Icd10();
				icd10.Icd10Num   = PIn.Long  (table.Rows[i]["Icd10Num"].ToString());
				icd10.Icd10Code  = PIn.String(table.Rows[i]["Icd10Code"].ToString());
				icd10.Description= PIn.String(table.Rows[i]["Description"].ToString());
				icd10.IsCode     = PIn.String(table.Rows[i]["IsCode"].ToString());
				retVal.Add(icd10);
			}
			return retVal;
		}

		///<summary>Inserts one Icd10 into the database.  Returns the new priKey.</summary>
		public static long Insert(Icd10 icd10){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				icd10.Icd10Num=DbHelper.GetNextOracleKey("icd10","Icd10Num");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(icd10,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							icd10.Icd10Num++;
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
				return Insert(icd10,false);
			}
		}

		///<summary>Inserts one Icd10 into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Icd10 icd10,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				icd10.Icd10Num=ReplicationServers.GetKey("icd10","Icd10Num");
			}
			string command="INSERT INTO icd10 (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="Icd10Num,";
			}
			command+="Icd10Code,Description,IsCode) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(icd10.Icd10Num)+",";
			}
			command+=
				 "'"+POut.String(icd10.Icd10Code)+"',"
				+"'"+POut.String(icd10.Description)+"',"
				+"'"+POut.String(icd10.IsCode)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				icd10.Icd10Num=Db.NonQ(command,true);
			}
			return icd10.Icd10Num;
		}

		///<summary>Updates one Icd10 in the database.</summary>
		public static void Update(Icd10 icd10){
			string command="UPDATE icd10 SET "
				+"Icd10Code  = '"+POut.String(icd10.Icd10Code)+"', "
				+"Description= '"+POut.String(icd10.Description)+"', "
				+"IsCode     = '"+POut.String(icd10.IsCode)+"' "
				+"WHERE Icd10Num = "+POut.Long(icd10.Icd10Num);
			Db.NonQ(command);
		}

		///<summary>Updates one Icd10 in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		public static void Update(Icd10 icd10,Icd10 oldIcd10){
			string command="";
			if(icd10.Icd10Code != oldIcd10.Icd10Code) {
				if(command!=""){ command+=",";}
				command+="Icd10Code = '"+POut.String(icd10.Icd10Code)+"'";
			}
			if(icd10.Description != oldIcd10.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(icd10.Description)+"'";
			}
			if(icd10.IsCode != oldIcd10.IsCode) {
				if(command!=""){ command+=",";}
				command+="IsCode = '"+POut.String(icd10.IsCode)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE icd10 SET "+command
				+" WHERE Icd10Num = "+POut.Long(icd10.Icd10Num);
			Db.NonQ(command);
		}

		///<summary>Deletes one Icd10 from the database.</summary>
		public static void Delete(long icd10Num){
			string command="DELETE FROM icd10 "
				+"WHERE Icd10Num = "+POut.Long(icd10Num);
			Db.NonQ(command);
		}

	}
}