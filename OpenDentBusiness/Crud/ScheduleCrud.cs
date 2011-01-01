//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class ScheduleCrud {
		///<summary>Gets one Schedule object from the database using the primary key.  Returns null if not found.</summary>
		internal static Schedule SelectOne(long scheduleNum){
			string command="SELECT * FROM schedule "
				+"WHERE ScheduleNum = "+POut.Long(scheduleNum)+" LIMIT 1";
			List<Schedule> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Schedule object from the database using a query.</summary>
		internal static Schedule SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Schedule> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Schedule objects from the database using a query.</summary>
		internal static List<Schedule> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Schedule> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<Schedule> TableToList(DataTable table){
			List<Schedule> retVal=new List<Schedule>();
			Schedule schedule;
			for(int i=0;i<table.Rows.Count;i++) {
				schedule=new Schedule();
				schedule.ScheduleNum = PIn.Long  (table.Rows[i]["ScheduleNum"].ToString());
				schedule.SchedDate   = PIn.Date  (table.Rows[i]["SchedDate"].ToString());
				schedule.StartTime   = PIn.TimeSpan(table.Rows[i]["StartTime"].ToString());
				schedule.StopTime    = PIn.TimeSpan(table.Rows[i]["StopTime"].ToString());
				schedule.SchedType   = (ScheduleType)PIn.Int(table.Rows[i]["SchedType"].ToString());
				schedule.ProvNum     = PIn.Long  (table.Rows[i]["ProvNum"].ToString());
				schedule.BlockoutType= PIn.Long  (table.Rows[i]["BlockoutType"].ToString());
				schedule.Note        = PIn.String(table.Rows[i]["Note"].ToString());
				schedule.Status      = (SchedStatus)PIn.Int(table.Rows[i]["Status"].ToString());
				schedule.EmployeeNum = PIn.Long  (table.Rows[i]["EmployeeNum"].ToString());
				retVal.Add(schedule);
			}
			return retVal;
		}

		///<summary>Inserts one Schedule into the database.  Returns the new priKey.</summary>
		internal static long Insert(Schedule schedule){
			return Insert(schedule,false);
		}

		///<summary>Inserts one Schedule into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(Schedule schedule,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				schedule.ScheduleNum=ReplicationServers.GetKey("schedule","ScheduleNum");
			}
			string command="INSERT INTO schedule (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ScheduleNum,";
			}
			command+="SchedDate,StartTime,StopTime,SchedType,ProvNum,BlockoutType,Note,Status,EmployeeNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(schedule.ScheduleNum)+",";
			}
			command+=
				     POut.Date  (schedule.SchedDate)+","
				+    POut.Time  (schedule.StartTime)+","
				+    POut.Time  (schedule.StopTime)+","
				+    POut.Int   ((int)schedule.SchedType)+","
				+    POut.Long  (schedule.ProvNum)+","
				+    POut.Long  (schedule.BlockoutType)+","
				+"'"+POut.String(schedule.Note)+"',"
				+    POut.Int   ((int)schedule.Status)+","
				+    POut.Long  (schedule.EmployeeNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				schedule.ScheduleNum=Db.NonQ(command,true);
			}
			return schedule.ScheduleNum;
		}

		///<summary>Updates one Schedule in the database.</summary>
		internal static void Update(Schedule schedule){
			string command="UPDATE schedule SET "
				+"SchedDate   =  "+POut.Date  (schedule.SchedDate)+", "
				+"StartTime   =  "+POut.Time  (schedule.StartTime)+", "
				+"StopTime    =  "+POut.Time  (schedule.StopTime)+", "
				+"SchedType   =  "+POut.Int   ((int)schedule.SchedType)+", "
				+"ProvNum     =  "+POut.Long  (schedule.ProvNum)+", "
				+"BlockoutType=  "+POut.Long  (schedule.BlockoutType)+", "
				+"Note        = '"+POut.String(schedule.Note)+"', "
				+"Status      =  "+POut.Int   ((int)schedule.Status)+", "
				+"EmployeeNum =  "+POut.Long  (schedule.EmployeeNum)+" "
				+"WHERE ScheduleNum = "+POut.Long(schedule.ScheduleNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Schedule in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(Schedule schedule,Schedule oldSchedule){
			string command="";
			if(schedule.SchedDate != oldSchedule.SchedDate) {
				if(command!=""){ command+=",";}
				command+="SchedDate = "+POut.Date(schedule.SchedDate)+"";
			}
			if(schedule.StartTime != oldSchedule.StartTime) {
				if(command!=""){ command+=",";}
				command+="StartTime = "+POut.Time  (schedule.StartTime)+"";
			}
			if(schedule.StopTime != oldSchedule.StopTime) {
				if(command!=""){ command+=",";}
				command+="StopTime = "+POut.Time  (schedule.StopTime)+"";
			}
			if(schedule.SchedType != oldSchedule.SchedType) {
				if(command!=""){ command+=",";}
				command+="SchedType = "+POut.Int   ((int)schedule.SchedType)+"";
			}
			if(schedule.ProvNum != oldSchedule.ProvNum) {
				if(command!=""){ command+=",";}
				command+="ProvNum = "+POut.Long(schedule.ProvNum)+"";
			}
			if(schedule.BlockoutType != oldSchedule.BlockoutType) {
				if(command!=""){ command+=",";}
				command+="BlockoutType = "+POut.Long(schedule.BlockoutType)+"";
			}
			if(schedule.Note != oldSchedule.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(schedule.Note)+"'";
			}
			if(schedule.Status != oldSchedule.Status) {
				if(command!=""){ command+=",";}
				command+="Status = "+POut.Int   ((int)schedule.Status)+"";
			}
			if(schedule.EmployeeNum != oldSchedule.EmployeeNum) {
				if(command!=""){ command+=",";}
				command+="EmployeeNum = "+POut.Long(schedule.EmployeeNum)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE schedule SET "+command
				+" WHERE ScheduleNum = "+POut.Long(schedule.ScheduleNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one Schedule from the database.</summary>
		internal static void Delete(long scheduleNum){
			string command="DELETE FROM schedule "
				+"WHERE ScheduleNum = "+POut.Long(scheduleNum);
			Db.NonQ(command);
		}

	}
}