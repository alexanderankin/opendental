//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class RefAttachCrud {
		///<summary>Gets one RefAttach object from the database using the primary key.  Returns null if not found.</summary>
		internal static RefAttach SelectOne(long refAttachNum){
			string command="SELECT * FROM refattach "
				+"WHERE RefAttachNum = "+POut.Long(refAttachNum)+" LIMIT 1";
			List<RefAttach> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one RefAttach object from the database using a query.</summary>
		internal static RefAttach SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<RefAttach> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of RefAttach objects from the database using a query.</summary>
		internal static List<RefAttach> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<RefAttach> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<RefAttach> TableToList(DataTable table){
			List<RefAttach> retVal=new List<RefAttach>();
			RefAttach refAttach;
			for(int i=0;i<table.Rows.Count;i++) {
				refAttach=new RefAttach();
				refAttach.RefAttachNum= PIn.Long  (table.Rows[i]["RefAttachNum"].ToString());
				refAttach.ReferralNum = PIn.Long  (table.Rows[i]["ReferralNum"].ToString());
				refAttach.PatNum      = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				refAttach.ItemOrder   = PIn.Int   (table.Rows[i]["ItemOrder"].ToString());
				refAttach.RefDate     = PIn.Date  (table.Rows[i]["RefDate"].ToString());
				refAttach.IsFrom      = PIn.Bool  (table.Rows[i]["IsFrom"].ToString());
				refAttach.RefToStatus = (ReferralToStatus)PIn.Int(table.Rows[i]["RefToStatus"].ToString());
				refAttach.Note        = PIn.String(table.Rows[i]["Note"].ToString());
				retVal.Add(refAttach);
			}
			return retVal;
		}

		///<summary>Inserts one RefAttach into the database.  Returns the new priKey.</summary>
		internal static long Insert(RefAttach refAttach){
			return Insert(refAttach,false);
		}

		///<summary>Inserts one RefAttach into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(RefAttach refAttach,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				refAttach.RefAttachNum=ReplicationServers.GetKey("refattach","RefAttachNum");
			}
			string command="INSERT INTO refattach (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="RefAttachNum,";
			}
			command+="ReferralNum,PatNum,ItemOrder,RefDate,IsFrom,RefToStatus,Note) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(refAttach.RefAttachNum)+",";
			}
			command+=
				     POut.Long  (refAttach.ReferralNum)+","
				+    POut.Long  (refAttach.PatNum)+","
				+    POut.Int   (refAttach.ItemOrder)+","
				+    POut.Date  (refAttach.RefDate)+","
				+    POut.Bool  (refAttach.IsFrom)+","
				+    POut.Int   ((int)refAttach.RefToStatus)+","
				+"'"+POut.String(refAttach.Note)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				refAttach.RefAttachNum=Db.NonQ(command,true);
			}
			return refAttach.RefAttachNum;
		}

		///<summary>Updates one RefAttach in the database.</summary>
		internal static void Update(RefAttach refAttach){
			string command="UPDATE refattach SET "
				+"ReferralNum =  "+POut.Long  (refAttach.ReferralNum)+", "
				+"PatNum      =  "+POut.Long  (refAttach.PatNum)+", "
				+"ItemOrder   =  "+POut.Int   (refAttach.ItemOrder)+", "
				+"RefDate     =  "+POut.Date  (refAttach.RefDate)+", "
				+"IsFrom      =  "+POut.Bool  (refAttach.IsFrom)+", "
				+"RefToStatus =  "+POut.Int   ((int)refAttach.RefToStatus)+", "
				+"Note        = '"+POut.String(refAttach.Note)+"' "
				+"WHERE RefAttachNum = "+POut.Long(refAttach.RefAttachNum);
			Db.NonQ(command);
		}

		///<summary>Updates one RefAttach in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(RefAttach refAttach,RefAttach oldRefAttach){
			string command="";
			if(refAttach.ReferralNum != oldRefAttach.ReferralNum) {
				if(command!=""){ command+=",";}
				command+="ReferralNum = "+POut.Long(refAttach.ReferralNum)+"";
			}
			if(refAttach.PatNum != oldRefAttach.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(refAttach.PatNum)+"";
			}
			if(refAttach.ItemOrder != oldRefAttach.ItemOrder) {
				if(command!=""){ command+=",";}
				command+="ItemOrder = "+POut.Int(refAttach.ItemOrder)+"";
			}
			if(refAttach.RefDate != oldRefAttach.RefDate) {
				if(command!=""){ command+=",";}
				command+="RefDate = "+POut.Date(refAttach.RefDate)+"";
			}
			if(refAttach.IsFrom != oldRefAttach.IsFrom) {
				if(command!=""){ command+=",";}
				command+="IsFrom = "+POut.Bool(refAttach.IsFrom)+"";
			}
			if(refAttach.RefToStatus != oldRefAttach.RefToStatus) {
				if(command!=""){ command+=",";}
				command+="RefToStatus = "+POut.Int   ((int)refAttach.RefToStatus)+"";
			}
			if(refAttach.Note != oldRefAttach.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(refAttach.Note)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE refattach SET "+command
				+" WHERE RefAttachNum = "+POut.Long(refAttach.RefAttachNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one RefAttach from the database.</summary>
		internal static void Delete(long refAttachNum){
			string command="DELETE FROM refattach "
				+"WHERE RefAttachNum = "+POut.Long(refAttachNum);
			Db.NonQ(command);
		}

	}
}