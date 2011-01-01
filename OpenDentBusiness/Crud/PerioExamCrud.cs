//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class PerioExamCrud {
		///<summary>Gets one PerioExam object from the database using the primary key.  Returns null if not found.</summary>
		internal static PerioExam SelectOne(long perioExamNum){
			string command="SELECT * FROM perioexam "
				+"WHERE PerioExamNum = "+POut.Long(perioExamNum)+" LIMIT 1";
			List<PerioExam> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PerioExam object from the database using a query.</summary>
		internal static PerioExam SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PerioExam> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PerioExam objects from the database using a query.</summary>
		internal static List<PerioExam> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PerioExam> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<PerioExam> TableToList(DataTable table){
			List<PerioExam> retVal=new List<PerioExam>();
			PerioExam perioExam;
			for(int i=0;i<table.Rows.Count;i++) {
				perioExam=new PerioExam();
				perioExam.PerioExamNum= PIn.Long  (table.Rows[i]["PerioExamNum"].ToString());
				perioExam.PatNum      = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				perioExam.ExamDate    = PIn.Date  (table.Rows[i]["ExamDate"].ToString());
				perioExam.ProvNum     = PIn.Long  (table.Rows[i]["ProvNum"].ToString());
				retVal.Add(perioExam);
			}
			return retVal;
		}

		///<summary>Inserts one PerioExam into the database.  Returns the new priKey.</summary>
		internal static long Insert(PerioExam perioExam){
			return Insert(perioExam,false);
		}

		///<summary>Inserts one PerioExam into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(PerioExam perioExam,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				perioExam.PerioExamNum=ReplicationServers.GetKey("perioexam","PerioExamNum");
			}
			string command="INSERT INTO perioexam (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PerioExamNum,";
			}
			command+="PatNum,ExamDate,ProvNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(perioExam.PerioExamNum)+",";
			}
			command+=
				     POut.Long  (perioExam.PatNum)+","
				+    POut.Date  (perioExam.ExamDate)+","
				+    POut.Long  (perioExam.ProvNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				perioExam.PerioExamNum=Db.NonQ(command,true);
			}
			return perioExam.PerioExamNum;
		}

		///<summary>Updates one PerioExam in the database.</summary>
		internal static void Update(PerioExam perioExam){
			string command="UPDATE perioexam SET "
				+"PatNum      =  "+POut.Long  (perioExam.PatNum)+", "
				+"ExamDate    =  "+POut.Date  (perioExam.ExamDate)+", "
				+"ProvNum     =  "+POut.Long  (perioExam.ProvNum)+" "
				+"WHERE PerioExamNum = "+POut.Long(perioExam.PerioExamNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PerioExam in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(PerioExam perioExam,PerioExam oldPerioExam){
			string command="";
			if(perioExam.PatNum != oldPerioExam.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(perioExam.PatNum)+"";
			}
			if(perioExam.ExamDate != oldPerioExam.ExamDate) {
				if(command!=""){ command+=",";}
				command+="ExamDate = "+POut.Date(perioExam.ExamDate)+"";
			}
			if(perioExam.ProvNum != oldPerioExam.ProvNum) {
				if(command!=""){ command+=",";}
				command+="ProvNum = "+POut.Long(perioExam.ProvNum)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE perioexam SET "+command
				+" WHERE PerioExamNum = "+POut.Long(perioExam.PerioExamNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one PerioExam from the database.</summary>
		internal static void Delete(long perioExamNum){
			string command="DELETE FROM perioexam "
				+"WHERE PerioExamNum = "+POut.Long(perioExamNum);
			Db.NonQ(command);
		}

	}
}