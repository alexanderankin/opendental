//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class SignalCrud {
		///<summary>Gets one Signal object from the database using the primary key.  Returns null if not found.</summary>
		internal static Signal SelectOne(long signalNum){
			string command="SELECT * FROM signal "
				+"WHERE SignalNum = "+POut.Long(signalNum)+" LIMIT 1";
			List<Signal> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Signal object from the database using a query.</summary>
		internal static Signal SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Signal> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Signal objects from the database using a query.</summary>
		internal static List<Signal> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Signal> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<Signal> TableToList(DataTable table){
			List<Signal> retVal=new List<Signal>();
			Signal signal;
			for(int i=0;i<table.Rows.Count;i++) {
				signal=new Signal();
				signal.SignalNum  = PIn.Long  (table.Rows[i]["SignalNum"].ToString());
				signal.FromUser   = PIn.String(table.Rows[i]["FromUser"].ToString());
				signal.ITypes     = PIn.String(table.Rows[i]["ITypes"].ToString());
				signal.DateViewing= PIn.Date  (table.Rows[i]["DateViewing"].ToString());
				signal.SigType    = (SignalType)PIn.Int(table.Rows[i]["SigType"].ToString());
				signal.SigText    = PIn.String(table.Rows[i]["SigText"].ToString());
				signal.SigDateTime= PIn.DateT (table.Rows[i]["SigDateTime"].ToString());
				signal.ToUser     = PIn.String(table.Rows[i]["ToUser"].ToString());
				signal.AckTime    = PIn.DateT (table.Rows[i]["AckTime"].ToString());
				signal.TaskNum    = PIn.Long  (table.Rows[i]["TaskNum"].ToString());
				retVal.Add(signal);
			}
			return retVal;
		}

		///<summary>Inserts one Signal into the database.  Returns the new priKey.</summary>
		internal static long Insert(Signal signal){
			return Insert(signal,false);
		}

		///<summary>Inserts one Signal into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(Signal signal,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				signal.SignalNum=ReplicationServers.GetKey("signal","SignalNum");
			}
			string command="INSERT INTO signal (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SignalNum,";
			}
			command+="FromUser,ITypes,DateViewing,SigType,SigText,SigDateTime,ToUser,AckTime,TaskNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(signal.SignalNum)+",";
			}
			command+=
				 "'"+POut.String(signal.FromUser)+"',"
				+"'"+POut.String(signal.ITypes)+"',"
				+    POut.Date  (signal.DateViewing)+","
				+    POut.Int   ((int)signal.SigType)+","
				+"'"+POut.String(signal.SigText)+"',"
				+"NOW(),"
				+"'"+POut.String(signal.ToUser)+"',"
				+    POut.DateT (signal.AckTime)+","
				+    POut.Long  (signal.TaskNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				signal.SignalNum=Db.NonQ(command,true);
			}
			return signal.SignalNum;
		}

		///<summary>Updates one Signal in the database.</summary>
		internal static void Update(Signal signal){
			string command="UPDATE signal SET "
				+"FromUser   = '"+POut.String(signal.FromUser)+"', "
				+"ITypes     = '"+POut.String(signal.ITypes)+"', "
				+"DateViewing=  "+POut.Date  (signal.DateViewing)+", "
				+"SigType    =  "+POut.Int   ((int)signal.SigType)+", "
				+"SigText    = '"+POut.String(signal.SigText)+"', "
				//SigDateTime not allowed to change
				+"ToUser     = '"+POut.String(signal.ToUser)+"', "
				+"AckTime    =  "+POut.DateT (signal.AckTime)+", "
				+"TaskNum    =  "+POut.Long  (signal.TaskNum)+" "
				+"WHERE SignalNum = "+POut.Long(signal.SignalNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Updates one Signal in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(Signal signal,Signal oldSignal){
			string command="";
			if(signal.FromUser != oldSignal.FromUser) {
				if(command!=""){ command+=",";}
				command+="FromUser = '"+POut.String(signal.FromUser)+"'";
			}
			if(signal.ITypes != oldSignal.ITypes) {
				if(command!=""){ command+=",";}
				command+="ITypes = '"+POut.String(signal.ITypes)+"'";
			}
			if(signal.DateViewing != oldSignal.DateViewing) {
				if(command!=""){ command+=",";}
				command+="DateViewing = "+POut.Date(signal.DateViewing)+"";
			}
			if(signal.SigType != oldSignal.SigType) {
				if(command!=""){ command+=",";}
				command+="SigType = "+POut.Int   ((int)signal.SigType)+"";
			}
			if(signal.SigText != oldSignal.SigText) {
				if(command!=""){ command+=",";}
				command+="SigText = '"+POut.String(signal.SigText)+"'";
			}
			//SigDateTime not allowed to change
			if(signal.ToUser != oldSignal.ToUser) {
				if(command!=""){ command+=",";}
				command+="ToUser = '"+POut.String(signal.ToUser)+"'";
			}
			if(signal.AckTime != oldSignal.AckTime) {
				if(command!=""){ command+=",";}
				command+="AckTime = "+POut.DateT(signal.AckTime)+"";
			}
			if(signal.TaskNum != oldSignal.TaskNum) {
				if(command!=""){ command+=",";}
				command+="TaskNum = "+POut.Long(signal.TaskNum)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE signal SET "+command
				+" WHERE SignalNum = "+POut.Long(signal.SignalNum)+" LIMIT 1";
			Db.NonQ(command);
		}

		///<summary>Deletes one Signal from the database.</summary>
		internal static void Delete(long signalNum){
			string command="DELETE FROM signal "
				+"WHERE SignalNum = "+POut.Long(signalNum)+" LIMIT 1";
			Db.NonQ(command);
		}

	}
}