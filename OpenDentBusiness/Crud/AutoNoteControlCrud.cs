//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class AutoNoteControlCrud {
		///<summary>Gets one AutoNoteControl object from the database using the primary key.  Returns null if not found.</summary>
		internal static AutoNoteControl SelectOne(long autoNoteControlNum){
			string command="SELECT * FROM autonotecontrol "
				+"WHERE AutoNoteControlNum = "+POut.Long(autoNoteControlNum);
			List<AutoNoteControl> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one AutoNoteControl object from the database using a query.</summary>
		internal static AutoNoteControl SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<AutoNoteControl> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of AutoNoteControl objects from the database using a query.</summary>
		internal static List<AutoNoteControl> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<AutoNoteControl> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<AutoNoteControl> TableToList(DataTable table){
			List<AutoNoteControl> retVal=new List<AutoNoteControl>();
			AutoNoteControl autoNoteControl;
			for(int i=0;i<table.Rows.Count;i++) {
				autoNoteControl=new AutoNoteControl();
				autoNoteControl.AutoNoteControlNum= PIn.Long  (table.Rows[i]["AutoNoteControlNum"].ToString());
				autoNoteControl.Descript          = PIn.String(table.Rows[i]["Descript"].ToString());
				autoNoteControl.ControlType       = PIn.String(table.Rows[i]["ControlType"].ToString());
				autoNoteControl.ControlLabel      = PIn.String(table.Rows[i]["ControlLabel"].ToString());
				autoNoteControl.ControlOptions    = PIn.String(table.Rows[i]["ControlOptions"].ToString());
				retVal.Add(autoNoteControl);
			}
			return retVal;
		}

		///<summary>Inserts one AutoNoteControl into the database.  Returns the new priKey.</summary>
		internal static long Insert(AutoNoteControl autoNoteControl){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				autoNoteControl.AutoNoteControlNum=DbHelper.GetNextOracleKey("autonotecontrol","AutoNoteControlNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(autoNoteControl,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							autoNoteControl.AutoNoteControlNum++;
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
				return Insert(autoNoteControl,false);
			}
		}

		///<summary>Inserts one AutoNoteControl into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(AutoNoteControl autoNoteControl,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				autoNoteControl.AutoNoteControlNum=ReplicationServers.GetKey("autonotecontrol","AutoNoteControlNum");
			}
			string command="INSERT INTO autonotecontrol (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="AutoNoteControlNum,";
			}
			command+="Descript,ControlType,ControlLabel,ControlOptions) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(autoNoteControl.AutoNoteControlNum)+",";
			}
			command+=
				 "'"+POut.String(autoNoteControl.Descript)+"',"
				+"'"+POut.String(autoNoteControl.ControlType)+"',"
				+"'"+POut.String(autoNoteControl.ControlLabel)+"',"
				+"'"+POut.String(autoNoteControl.ControlOptions)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				autoNoteControl.AutoNoteControlNum=Db.NonQ(command,true);
			}
			return autoNoteControl.AutoNoteControlNum;
		}

		///<summary>Updates one AutoNoteControl in the database.</summary>
		internal static void Update(AutoNoteControl autoNoteControl){
			string command="UPDATE autonotecontrol SET "
				+"Descript          = '"+POut.String(autoNoteControl.Descript)+"', "
				+"ControlType       = '"+POut.String(autoNoteControl.ControlType)+"', "
				+"ControlLabel      = '"+POut.String(autoNoteControl.ControlLabel)+"', "
				+"ControlOptions    = '"+POut.String(autoNoteControl.ControlOptions)+"' "
				+"WHERE AutoNoteControlNum = "+POut.Long(autoNoteControl.AutoNoteControlNum);
			Db.NonQ(command);
		}

		///<summary>Updates one AutoNoteControl in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(AutoNoteControl autoNoteControl,AutoNoteControl oldAutoNoteControl){
			string command="";
			if(autoNoteControl.Descript != oldAutoNoteControl.Descript) {
				if(command!=""){ command+=",";}
				command+="Descript = '"+POut.String(autoNoteControl.Descript)+"'";
			}
			if(autoNoteControl.ControlType != oldAutoNoteControl.ControlType) {
				if(command!=""){ command+=",";}
				command+="ControlType = '"+POut.String(autoNoteControl.ControlType)+"'";
			}
			if(autoNoteControl.ControlLabel != oldAutoNoteControl.ControlLabel) {
				if(command!=""){ command+=",";}
				command+="ControlLabel = '"+POut.String(autoNoteControl.ControlLabel)+"'";
			}
			if(autoNoteControl.ControlOptions != oldAutoNoteControl.ControlOptions) {
				if(command!=""){ command+=",";}
				command+="ControlOptions = '"+POut.String(autoNoteControl.ControlOptions)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE autonotecontrol SET "+command
				+" WHERE AutoNoteControlNum = "+POut.Long(autoNoteControl.AutoNoteControlNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one AutoNoteControl from the database.</summary>
		internal static void Delete(long autoNoteControlNum){
			string command="DELETE FROM autonotecontrol "
				+"WHERE AutoNoteControlNum = "+POut.Long(autoNoteControlNum);
			Db.NonQ(command);
		}

	}
}