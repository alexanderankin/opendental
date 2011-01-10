//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class UserodCrud {
		///<summary>Gets one Userod object from the database using the primary key.  Returns null if not found.</summary>
		internal static Userod SelectOne(long userNum){
			string command="SELECT * FROM userod "
				+"WHERE UserNum = "+POut.Long(userNum);
			List<Userod> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Userod object from the database using a query.</summary>
		internal static Userod SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Userod> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Userod objects from the database using a query.</summary>
		internal static List<Userod> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Userod> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<Userod> TableToList(DataTable table){
			List<Userod> retVal=new List<Userod>();
			Userod userod;
			for(int i=0;i<table.Rows.Count;i++) {
				userod=new Userod();
				userod.UserNum          = PIn.Long  (table.Rows[i]["UserNum"].ToString());
				userod.UserName         = PIn.String(table.Rows[i]["UserName"].ToString());
				userod.Password         = PIn.String(table.Rows[i]["Password"].ToString());
				userod.UserGroupNum     = PIn.Long  (table.Rows[i]["UserGroupNum"].ToString());
				userod.EmployeeNum      = PIn.Long  (table.Rows[i]["EmployeeNum"].ToString());
				userod.ClinicNum        = PIn.Long  (table.Rows[i]["ClinicNum"].ToString());
				userod.ProvNum          = PIn.Long  (table.Rows[i]["ProvNum"].ToString());
				userod.IsHidden         = PIn.Bool  (table.Rows[i]["IsHidden"].ToString());
				userod.TaskListInBox    = PIn.Long  (table.Rows[i]["TaskListInBox"].ToString());
				userod.AnesthProvType   = PIn.Int   (table.Rows[i]["AnesthProvType"].ToString());
				userod.DefaultHidePopups= PIn.Bool  (table.Rows[i]["DefaultHidePopups"].ToString());
				userod.PasswordIsStrong = PIn.Bool  (table.Rows[i]["PasswordIsStrong"].ToString());
				retVal.Add(userod);
			}
			return retVal;
		}

		///<summary>Inserts one Userod into the database.  Returns the new priKey.</summary>
		internal static long Insert(Userod userod){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				userod.UserNum=DbHelper.GetNextOracleKey("userod","UserNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(userod,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							userod.UserNum++;
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
				return Insert(userod,false);
			}
		}

		///<summary>Inserts one Userod into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(Userod userod,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				userod.UserNum=ReplicationServers.GetKey("userod","UserNum");
			}
			string command="INSERT INTO userod (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="UserNum,";
			}
			command+="UserName,Password,UserGroupNum,EmployeeNum,ClinicNum,ProvNum,IsHidden,TaskListInBox,AnesthProvType,DefaultHidePopups,PasswordIsStrong) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(userod.UserNum)+",";
			}
			command+=
				 "'"+POut.String(userod.UserName)+"',"
				+"'"+POut.String(userod.Password)+"',"
				+    POut.Long  (userod.UserGroupNum)+","
				+    POut.Long  (userod.EmployeeNum)+","
				+    POut.Long  (userod.ClinicNum)+","
				+    POut.Long  (userod.ProvNum)+","
				+    POut.Bool  (userod.IsHidden)+","
				+    POut.Long  (userod.TaskListInBox)+","
				+    POut.Int   (userod.AnesthProvType)+","
				+    POut.Bool  (userod.DefaultHidePopups)+","
				+    POut.Bool  (userod.PasswordIsStrong)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				userod.UserNum=Db.NonQ(command,true);
			}
			return userod.UserNum;
		}

		///<summary>Updates one Userod in the database.</summary>
		internal static void Update(Userod userod){
			string command="UPDATE userod SET "
				+"UserName         = '"+POut.String(userod.UserName)+"', "
				+"Password         = '"+POut.String(userod.Password)+"', "
				+"UserGroupNum     =  "+POut.Long  (userod.UserGroupNum)+", "
				+"EmployeeNum      =  "+POut.Long  (userod.EmployeeNum)+", "
				+"ClinicNum        =  "+POut.Long  (userod.ClinicNum)+", "
				+"ProvNum          =  "+POut.Long  (userod.ProvNum)+", "
				+"IsHidden         =  "+POut.Bool  (userod.IsHidden)+", "
				+"TaskListInBox    =  "+POut.Long  (userod.TaskListInBox)+", "
				+"AnesthProvType   =  "+POut.Int   (userod.AnesthProvType)+", "
				+"DefaultHidePopups=  "+POut.Bool  (userod.DefaultHidePopups)+", "
				+"PasswordIsStrong =  "+POut.Bool  (userod.PasswordIsStrong)+" "
				+"WHERE UserNum = "+POut.Long(userod.UserNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Userod in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(Userod userod,Userod oldUserod){
			string command="";
			if(userod.UserName != oldUserod.UserName) {
				if(command!=""){ command+=",";}
				command+="UserName = '"+POut.String(userod.UserName)+"'";
			}
			if(userod.Password != oldUserod.Password) {
				if(command!=""){ command+=",";}
				command+="Password = '"+POut.String(userod.Password)+"'";
			}
			if(userod.UserGroupNum != oldUserod.UserGroupNum) {
				if(command!=""){ command+=",";}
				command+="UserGroupNum = "+POut.Long(userod.UserGroupNum)+"";
			}
			if(userod.EmployeeNum != oldUserod.EmployeeNum) {
				if(command!=""){ command+=",";}
				command+="EmployeeNum = "+POut.Long(userod.EmployeeNum)+"";
			}
			if(userod.ClinicNum != oldUserod.ClinicNum) {
				if(command!=""){ command+=",";}
				command+="ClinicNum = "+POut.Long(userod.ClinicNum)+"";
			}
			if(userod.ProvNum != oldUserod.ProvNum) {
				if(command!=""){ command+=",";}
				command+="ProvNum = "+POut.Long(userod.ProvNum)+"";
			}
			if(userod.IsHidden != oldUserod.IsHidden) {
				if(command!=""){ command+=",";}
				command+="IsHidden = "+POut.Bool(userod.IsHidden)+"";
			}
			if(userod.TaskListInBox != oldUserod.TaskListInBox) {
				if(command!=""){ command+=",";}
				command+="TaskListInBox = "+POut.Long(userod.TaskListInBox)+"";
			}
			if(userod.AnesthProvType != oldUserod.AnesthProvType) {
				if(command!=""){ command+=",";}
				command+="AnesthProvType = "+POut.Int(userod.AnesthProvType)+"";
			}
			if(userod.DefaultHidePopups != oldUserod.DefaultHidePopups) {
				if(command!=""){ command+=",";}
				command+="DefaultHidePopups = "+POut.Bool(userod.DefaultHidePopups)+"";
			}
			if(userod.PasswordIsStrong != oldUserod.PasswordIsStrong) {
				if(command!=""){ command+=",";}
				command+="PasswordIsStrong = "+POut.Bool(userod.PasswordIsStrong)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE userod SET "+command
				+" WHERE UserNum = "+POut.Long(userod.UserNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one Userod from the database.</summary>
		internal static void Delete(long userNum){
			string command="DELETE FROM userod "
				+"WHERE UserNum = "+POut.Long(userNum);
			Db.NonQ(command);
		}

	}
}