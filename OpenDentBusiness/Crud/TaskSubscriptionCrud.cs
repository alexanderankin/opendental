//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class TaskSubscriptionCrud {
		///<summary>Gets one TaskSubscription object from the database using the primary key.  Returns null if not found.</summary>
		internal static TaskSubscription SelectOne(long taskSubscriptionNum){
			string command="SELECT * FROM tasksubscription "
				+"WHERE TaskSubscriptionNum = "+POut.Long(taskSubscriptionNum);
			List<TaskSubscription> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one TaskSubscription object from the database using a query.</summary>
		internal static TaskSubscription SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<TaskSubscription> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of TaskSubscription objects from the database using a query.</summary>
		internal static List<TaskSubscription> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<TaskSubscription> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<TaskSubscription> TableToList(DataTable table){
			List<TaskSubscription> retVal=new List<TaskSubscription>();
			TaskSubscription taskSubscription;
			for(int i=0;i<table.Rows.Count;i++) {
				taskSubscription=new TaskSubscription();
				taskSubscription.TaskSubscriptionNum= PIn.Long  (table.Rows[i]["TaskSubscriptionNum"].ToString());
				taskSubscription.UserNum            = PIn.Long  (table.Rows[i]["UserNum"].ToString());
				taskSubscription.TaskListNum        = PIn.Long  (table.Rows[i]["TaskListNum"].ToString());
				retVal.Add(taskSubscription);
			}
			return retVal;
		}

		///<summary>Inserts one TaskSubscription into the database.  Returns the new priKey.</summary>
		internal static long Insert(TaskSubscription taskSubscription){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				taskSubscription.TaskSubscriptionNum=DbHelper.GetNextOracleKey("tasksubscription","TaskSubscriptionNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(taskSubscription,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							taskSubscription.TaskSubscriptionNum++;
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
				return Insert(taskSubscription,false);
			}
		}

		///<summary>Inserts one TaskSubscription into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(TaskSubscription taskSubscription,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				taskSubscription.TaskSubscriptionNum=ReplicationServers.GetKey("tasksubscription","TaskSubscriptionNum");
			}
			string command="INSERT INTO tasksubscription (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="TaskSubscriptionNum,";
			}
			command+="UserNum,TaskListNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(taskSubscription.TaskSubscriptionNum)+",";
			}
			command+=
				     POut.Long  (taskSubscription.UserNum)+","
				+    POut.Long  (taskSubscription.TaskListNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				taskSubscription.TaskSubscriptionNum=Db.NonQ(command,true);
			}
			return taskSubscription.TaskSubscriptionNum;
		}

		///<summary>Updates one TaskSubscription in the database.</summary>
		internal static void Update(TaskSubscription taskSubscription){
			string command="UPDATE tasksubscription SET "
				+"UserNum            =  "+POut.Long  (taskSubscription.UserNum)+", "
				+"TaskListNum        =  "+POut.Long  (taskSubscription.TaskListNum)+" "
				+"WHERE TaskSubscriptionNum = "+POut.Long(taskSubscription.TaskSubscriptionNum);
			Db.NonQ(command);
		}

		///<summary>Updates one TaskSubscription in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(TaskSubscription taskSubscription,TaskSubscription oldTaskSubscription){
			string command="";
			if(taskSubscription.UserNum != oldTaskSubscription.UserNum) {
				if(command!=""){ command+=",";}
				command+="UserNum = "+POut.Long(taskSubscription.UserNum)+"";
			}
			if(taskSubscription.TaskListNum != oldTaskSubscription.TaskListNum) {
				if(command!=""){ command+=",";}
				command+="TaskListNum = "+POut.Long(taskSubscription.TaskListNum)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE tasksubscription SET "+command
				+" WHERE TaskSubscriptionNum = "+POut.Long(taskSubscription.TaskSubscriptionNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one TaskSubscription from the database.</summary>
		internal static void Delete(long taskSubscriptionNum){
			string command="DELETE FROM tasksubscription "
				+"WHERE TaskSubscriptionNum = "+POut.Long(taskSubscriptionNum);
			Db.NonQ(command);
		}

	}
}