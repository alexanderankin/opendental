//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class MountItemDefCrud {
		///<summary>Gets one MountItemDef object from the database using the primary key.  Returns null if not found.</summary>
		internal static MountItemDef SelectOne(long mountItemDefNum){
			string command="SELECT * FROM mountitemdef "
				+"WHERE MountItemDefNum = "+POut.Long(mountItemDefNum);
			List<MountItemDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one MountItemDef object from the database using a query.</summary>
		internal static MountItemDef SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MountItemDef> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of MountItemDef objects from the database using a query.</summary>
		internal static List<MountItemDef> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<MountItemDef> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<MountItemDef> TableToList(DataTable table){
			List<MountItemDef> retVal=new List<MountItemDef>();
			MountItemDef mountItemDef;
			for(int i=0;i<table.Rows.Count;i++) {
				mountItemDef=new MountItemDef();
				mountItemDef.MountItemDefNum= PIn.Long  (table.Rows[i]["MountItemDefNum"].ToString());
				mountItemDef.MountDefNum    = PIn.Long  (table.Rows[i]["MountDefNum"].ToString());
				mountItemDef.Xpos           = PIn.Int   (table.Rows[i]["Xpos"].ToString());
				mountItemDef.Ypos           = PIn.Int   (table.Rows[i]["Ypos"].ToString());
				mountItemDef.Width          = PIn.Int   (table.Rows[i]["Width"].ToString());
				mountItemDef.Height         = PIn.Int   (table.Rows[i]["Height"].ToString());
				retVal.Add(mountItemDef);
			}
			return retVal;
		}

		///<summary>Inserts one MountItemDef into the database.  Returns the new priKey.</summary>
		internal static long Insert(MountItemDef mountItemDef){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				mountItemDef.MountItemDefNum=DbHelper.GetNextOracleKey("mountitemdef","MountItemDefNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(mountItemDef,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							mountItemDef.MountItemDefNum++;
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
				return Insert(mountItemDef,false);
			}
		}

		///<summary>Inserts one MountItemDef into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(MountItemDef mountItemDef,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				mountItemDef.MountItemDefNum=ReplicationServers.GetKey("mountitemdef","MountItemDefNum");
			}
			string command="INSERT INTO mountitemdef (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="MountItemDefNum,";
			}
			command+="MountDefNum,Xpos,Ypos,Width,Height) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(mountItemDef.MountItemDefNum)+",";
			}
			command+=
				     POut.Long  (mountItemDef.MountDefNum)+","
				+    POut.Int   (mountItemDef.Xpos)+","
				+    POut.Int   (mountItemDef.Ypos)+","
				+    POut.Int   (mountItemDef.Width)+","
				+    POut.Int   (mountItemDef.Height)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				mountItemDef.MountItemDefNum=Db.NonQ(command,true);
			}
			return mountItemDef.MountItemDefNum;
		}

		///<summary>Updates one MountItemDef in the database.</summary>
		internal static void Update(MountItemDef mountItemDef){
			string command="UPDATE mountitemdef SET "
				+"MountDefNum    =  "+POut.Long  (mountItemDef.MountDefNum)+", "
				+"Xpos           =  "+POut.Int   (mountItemDef.Xpos)+", "
				+"Ypos           =  "+POut.Int   (mountItemDef.Ypos)+", "
				+"Width          =  "+POut.Int   (mountItemDef.Width)+", "
				+"Height         =  "+POut.Int   (mountItemDef.Height)+" "
				+"WHERE MountItemDefNum = "+POut.Long(mountItemDef.MountItemDefNum);
			Db.NonQ(command);
		}

		///<summary>Updates one MountItemDef in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(MountItemDef mountItemDef,MountItemDef oldMountItemDef){
			string command="";
			if(mountItemDef.MountDefNum != oldMountItemDef.MountDefNum) {
				if(command!=""){ command+=",";}
				command+="MountDefNum = "+POut.Long(mountItemDef.MountDefNum)+"";
			}
			if(mountItemDef.Xpos != oldMountItemDef.Xpos) {
				if(command!=""){ command+=",";}
				command+="Xpos = "+POut.Int(mountItemDef.Xpos)+"";
			}
			if(mountItemDef.Ypos != oldMountItemDef.Ypos) {
				if(command!=""){ command+=",";}
				command+="Ypos = "+POut.Int(mountItemDef.Ypos)+"";
			}
			if(mountItemDef.Width != oldMountItemDef.Width) {
				if(command!=""){ command+=",";}
				command+="Width = "+POut.Int(mountItemDef.Width)+"";
			}
			if(mountItemDef.Height != oldMountItemDef.Height) {
				if(command!=""){ command+=",";}
				command+="Height = "+POut.Int(mountItemDef.Height)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE mountitemdef SET "+command
				+" WHERE MountItemDefNum = "+POut.Long(mountItemDef.MountItemDefNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one MountItemDef from the database.</summary>
		internal static void Delete(long mountItemDefNum){
			string command="DELETE FROM mountitemdef "
				+"WHERE MountItemDefNum = "+POut.Long(mountItemDefNum);
			Db.NonQ(command);
		}

	}
}