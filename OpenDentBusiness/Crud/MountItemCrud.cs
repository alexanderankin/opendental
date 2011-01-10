//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class MountItemCrud {
		///<summary>Gets one MountItem object from the database using the primary key.  Returns null if not found.</summary>
		internal static MountItem SelectOne(long mountItemNum){
			string command="SELECT * FROM mountitem "
				+"WHERE MountItemNum = "+POut.Long(mountItemNum);
			List<MountItem> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one MountItem object from the database using a query.</summary>
		internal static MountItem SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MountItem> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of MountItem objects from the database using a query.</summary>
		internal static List<MountItem> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MountItem> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<MountItem> TableToList(DataTable table){
			List<MountItem> retVal=new List<MountItem>();
			MountItem mountItem;
			for(int i=0;i<table.Rows.Count;i++) {
				mountItem=new MountItem();
				mountItem.MountItemNum= PIn.Long  (table.Rows[i]["MountItemNum"].ToString());
				mountItem.MountNum    = PIn.Long  (table.Rows[i]["MountNum"].ToString());
				mountItem.Xpos        = PIn.Int   (table.Rows[i]["Xpos"].ToString());
				mountItem.Ypos        = PIn.Int   (table.Rows[i]["Ypos"].ToString());
				mountItem.OrdinalPos  = PIn.Int   (table.Rows[i]["OrdinalPos"].ToString());
				mountItem.Width       = PIn.Int   (table.Rows[i]["Width"].ToString());
				mountItem.Height      = PIn.Int   (table.Rows[i]["Height"].ToString());
				retVal.Add(mountItem);
			}
			return retVal;
		}

		///<summary>Inserts one MountItem into the database.  Returns the new priKey.</summary>
		internal static long Insert(MountItem mountItem){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				mountItem.MountItemNum=DbHelper.GetNextOracleKey("mountitem","MountItemNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(mountItem,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							mountItem.MountItemNum++;
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
				return Insert(mountItem,false);
			}
		}

		///<summary>Inserts one MountItem into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(MountItem mountItem,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				mountItem.MountItemNum=ReplicationServers.GetKey("mountitem","MountItemNum");
			}
			string command="INSERT INTO mountitem (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="MountItemNum,";
			}
			command+="MountNum,Xpos,Ypos,OrdinalPos,Width,Height) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(mountItem.MountItemNum)+",";
			}
			command+=
				     POut.Long  (mountItem.MountNum)+","
				+    POut.Int   (mountItem.Xpos)+","
				+    POut.Int   (mountItem.Ypos)+","
				+    POut.Int   (mountItem.OrdinalPos)+","
				+    POut.Int   (mountItem.Width)+","
				+    POut.Int   (mountItem.Height)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				mountItem.MountItemNum=Db.NonQ(command,true);
			}
			return mountItem.MountItemNum;
		}

		///<summary>Updates one MountItem in the database.</summary>
		internal static void Update(MountItem mountItem){
			string command="UPDATE mountitem SET "
				+"MountNum    =  "+POut.Long  (mountItem.MountNum)+", "
				+"Xpos        =  "+POut.Int   (mountItem.Xpos)+", "
				+"Ypos        =  "+POut.Int   (mountItem.Ypos)+", "
				+"OrdinalPos  =  "+POut.Int   (mountItem.OrdinalPos)+", "
				+"Width       =  "+POut.Int   (mountItem.Width)+", "
				+"Height      =  "+POut.Int   (mountItem.Height)+" "
				+"WHERE MountItemNum = "+POut.Long(mountItem.MountItemNum);
			Db.NonQ(command);
		}

		///<summary>Updates one MountItem in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(MountItem mountItem,MountItem oldMountItem){
			string command="";
			if(mountItem.MountNum != oldMountItem.MountNum) {
				if(command!=""){ command+=",";}
				command+="MountNum = "+POut.Long(mountItem.MountNum)+"";
			}
			if(mountItem.Xpos != oldMountItem.Xpos) {
				if(command!=""){ command+=",";}
				command+="Xpos = "+POut.Int(mountItem.Xpos)+"";
			}
			if(mountItem.Ypos != oldMountItem.Ypos) {
				if(command!=""){ command+=",";}
				command+="Ypos = "+POut.Int(mountItem.Ypos)+"";
			}
			if(mountItem.OrdinalPos != oldMountItem.OrdinalPos) {
				if(command!=""){ command+=",";}
				command+="OrdinalPos = "+POut.Int(mountItem.OrdinalPos)+"";
			}
			if(mountItem.Width != oldMountItem.Width) {
				if(command!=""){ command+=",";}
				command+="Width = "+POut.Int(mountItem.Width)+"";
			}
			if(mountItem.Height != oldMountItem.Height) {
				if(command!=""){ command+=",";}
				command+="Height = "+POut.Int(mountItem.Height)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE mountitem SET "+command
				+" WHERE MountItemNum = "+POut.Long(mountItem.MountItemNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one MountItem from the database.</summary>
		internal static void Delete(long mountItemNum){
			string command="DELETE FROM mountitem "
				+"WHERE MountItemNum = "+POut.Long(mountItemNum);
			Db.NonQ(command);
		}

	}
}