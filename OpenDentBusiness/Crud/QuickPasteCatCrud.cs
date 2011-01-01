//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class QuickPasteCatCrud {
		///<summary>Gets one QuickPasteCat object from the database using the primary key.  Returns null if not found.</summary>
		internal static QuickPasteCat SelectOne(long quickPasteCatNum){
			string command="SELECT * FROM quickpastecat "
				+"WHERE QuickPasteCatNum = "+POut.Long(quickPasteCatNum)+" LIMIT 1";
			List<QuickPasteCat> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one QuickPasteCat object from the database using a query.</summary>
		internal static QuickPasteCat SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<QuickPasteCat> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of QuickPasteCat objects from the database using a query.</summary>
		internal static List<QuickPasteCat> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<QuickPasteCat> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<QuickPasteCat> TableToList(DataTable table){
			List<QuickPasteCat> retVal=new List<QuickPasteCat>();
			QuickPasteCat quickPasteCat;
			for(int i=0;i<table.Rows.Count;i++) {
				quickPasteCat=new QuickPasteCat();
				quickPasteCat.QuickPasteCatNum= PIn.Long  (table.Rows[i]["QuickPasteCatNum"].ToString());
				quickPasteCat.Description     = PIn.String(table.Rows[i]["Description"].ToString());
				quickPasteCat.ItemOrder       = PIn.Int   (table.Rows[i]["ItemOrder"].ToString());
				quickPasteCat.DefaultForTypes = PIn.String(table.Rows[i]["DefaultForTypes"].ToString());
				retVal.Add(quickPasteCat);
			}
			return retVal;
		}

		///<summary>Inserts one QuickPasteCat into the database.  Returns the new priKey.</summary>
		internal static long Insert(QuickPasteCat quickPasteCat){
			return Insert(quickPasteCat,false);
		}

		///<summary>Inserts one QuickPasteCat into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(QuickPasteCat quickPasteCat,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				quickPasteCat.QuickPasteCatNum=ReplicationServers.GetKey("quickpastecat","QuickPasteCatNum");
			}
			string command="INSERT INTO quickpastecat (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="QuickPasteCatNum,";
			}
			command+="Description,ItemOrder,DefaultForTypes) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(quickPasteCat.QuickPasteCatNum)+",";
			}
			command+=
				 "'"+POut.String(quickPasteCat.Description)+"',"
				+    POut.Int   (quickPasteCat.ItemOrder)+","
				+"'"+POut.String(quickPasteCat.DefaultForTypes)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				quickPasteCat.QuickPasteCatNum=Db.NonQ(command,true);
			}
			return quickPasteCat.QuickPasteCatNum;
		}

		///<summary>Updates one QuickPasteCat in the database.</summary>
		internal static void Update(QuickPasteCat quickPasteCat){
			string command="UPDATE quickpastecat SET "
				+"Description     = '"+POut.String(quickPasteCat.Description)+"', "
				+"ItemOrder       =  "+POut.Int   (quickPasteCat.ItemOrder)+", "
				+"DefaultForTypes = '"+POut.String(quickPasteCat.DefaultForTypes)+"' "
				+"WHERE QuickPasteCatNum = "+POut.Long(quickPasteCat.QuickPasteCatNum);
			Db.NonQ(command);
		}

		///<summary>Updates one QuickPasteCat in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(QuickPasteCat quickPasteCat,QuickPasteCat oldQuickPasteCat){
			string command="";
			if(quickPasteCat.Description != oldQuickPasteCat.Description) {
				if(command!=""){ command+=",";}
				command+="Description = '"+POut.String(quickPasteCat.Description)+"'";
			}
			if(quickPasteCat.ItemOrder != oldQuickPasteCat.ItemOrder) {
				if(command!=""){ command+=",";}
				command+="ItemOrder = "+POut.Int(quickPasteCat.ItemOrder)+"";
			}
			if(quickPasteCat.DefaultForTypes != oldQuickPasteCat.DefaultForTypes) {
				if(command!=""){ command+=",";}
				command+="DefaultForTypes = '"+POut.String(quickPasteCat.DefaultForTypes)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE quickpastecat SET "+command
				+" WHERE QuickPasteCatNum = "+POut.Long(quickPasteCat.QuickPasteCatNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one QuickPasteCat from the database.</summary>
		internal static void Delete(long quickPasteCatNum){
			string command="DELETE FROM quickpastecat "
				+"WHERE QuickPasteCatNum = "+POut.Long(quickPasteCatNum);
			Db.NonQ(command);
		}

	}
}