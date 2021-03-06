//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class PopupCrud {
		///<summary>Gets one Popup object from the database using the primary key.  Returns null if not found.</summary>
		public static Popup SelectOne(long popupNum){
			string command="SELECT * FROM popup "
				+"WHERE PopupNum = "+POut.Long(popupNum);
			List<Popup> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Popup object from the database using a query.</summary>
		public static Popup SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Popup> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Popup objects from the database using a query.</summary>
		public static List<Popup> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Popup> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<Popup> TableToList(DataTable table){
			List<Popup> retVal=new List<Popup>();
			Popup popup;
			for(int i=0;i<table.Rows.Count;i++) {
				popup=new Popup();
				popup.PopupNum       = PIn.Long  (table.Rows[i]["PopupNum"].ToString());
				popup.PatNum         = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				popup.Description    = PIn.String(table.Rows[i]["Description"].ToString());
				popup.IsDisabled     = PIn.Bool  (table.Rows[i]["IsDisabled"].ToString());
				popup.PopupLevel     = (EnumPopupLevel)PIn.Int(table.Rows[i]["PopupLevel"].ToString());
				popup.UserNum        = PIn.Long  (table.Rows[i]["UserNum"].ToString());
				popup.DateTimeEntry  = PIn.DateT (table.Rows[i]["DateTimeEntry"].ToString());
				popup.IsArchived     = PIn.Bool  (table.Rows[i]["IsArchived"].ToString());
				popup.PopupNumArchive= PIn.Long  (table.Rows[i]["PopupNumArchive"].ToString());
				retVal.Add(popup);
			}
			return retVal;
		}

		///<summary>Inserts one Popup into the database.  Returns the new priKey.</summary>
		public static long Insert(Popup popup){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				popup.PopupNum=DbHelper.GetNextOracleKey("popup","PopupNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(popup,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							popup.PopupNum++;
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
				return Insert(popup,false);
			}
		}

		///<summary>Inserts one Popup into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(Popup popup,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				popup.PopupNum=ReplicationServers.GetKey("popup","PopupNum");
			}
			string command="INSERT INTO popup (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PopupNum,";
			}
			command+="PatNum,Description,IsDisabled,PopupLevel,UserNum,DateTimeEntry,IsArchived,PopupNumArchive) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(popup.PopupNum)+",";
			}
			command+=
				     POut.Long  (popup.PatNum)+","
				+"'"+POut.String(popup.Description)+"',"
				+    POut.Bool  (popup.IsDisabled)+","
				+    POut.Int   ((int)popup.PopupLevel)+","
				+    POut.Long  (popup.UserNum)+","
				+    DbHelper.Now()+","
				+    POut.Bool  (popup.IsArchived)+","
				+    POut.Long  (popup.PopupNumArchive)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				popup.PopupNum=Db.NonQ(command,true);
			}
			return popup.PopupNum;
		}

		///<summary>Updates one Popup in the database.</summary>
		public static void Update(Popup popup){
			string command="UPDATE popup SET "
				+"PatNum         =  "+POut.Long  (popup.PatNum)+", "
				+"Description    = '"+POut.String(popup.Description)+"', "
				+"IsDisabled     =  "+POut.Bool  (popup.IsDisabled)+", "
				+"PopupLevel     =  "+POut.Int   ((int)popup.PopupLevel)+", "
				+"UserNum        =  "+POut.Long  (popup.UserNum)+", "
				//DateTimeEntry not allowed to change
				+"IsArchived     =  "+POut.Bool  (popup.IsArchived)+", "
				+"PopupNumArchive=  "+POut.Long  (popup.PopupNumArchive)+" "
				+"WHERE PopupNum = "+POut.Long(popup.PopupNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Popup in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		public static void Update(Popup popup,Popup oldPopup){
			string command="";
			if(popup.PatNum != oldPopup.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(popup.PatNum)+"";
			}
			if(popup.Description != oldPopup.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(popup.Description)+"'";
			}
			if(popup.IsDisabled != oldPopup.IsDisabled) {
				if(command!=""){ command+=",";}
				command+="IsDisabled = "+POut.Bool(popup.IsDisabled)+"";
			}
			if(popup.PopupLevel != oldPopup.PopupLevel) {
				if(command!=""){ command+=",";}
				command+="PopupLevel = "+POut.Int   ((int)popup.PopupLevel)+"";
			}
			if(popup.UserNum != oldPopup.UserNum) {
				if(command!=""){ command+=",";}
				command+="UserNum = "+POut.Long(popup.UserNum)+"";
			}
			//DateTimeEntry not allowed to change
			if(popup.IsArchived != oldPopup.IsArchived) {
				if(command!=""){ command+=",";}
				command+="IsArchived = "+POut.Bool(popup.IsArchived)+"";
			}
			if(popup.PopupNumArchive != oldPopup.PopupNumArchive) {
				if(command!=""){ command+=",";}
				command+="PopupNumArchive = "+POut.Long(popup.PopupNumArchive)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE popup SET "+command
				+" WHERE PopupNum = "+POut.Long(popup.PopupNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one Popup from the database.</summary>
		public static void Delete(long popupNum){
			string command="DELETE FROM popup "
				+"WHERE PopupNum = "+POut.Long(popupNum);
			Db.NonQ(command);
		}

	}
}