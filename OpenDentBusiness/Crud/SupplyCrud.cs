//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class SupplyCrud {
		///<summary>Gets one Supply object from the database using the primary key.  Returns null if not found.</summary>
		internal static Supply SelectOne(long supplyNum){
			string command="SELECT * FROM supply "
				+"WHERE SupplyNum = "+POut.Long(supplyNum);
			List<Supply> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Supply object from the database using a query.</summary>
		internal static Supply SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Supply> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Supply objects from the database using a query.</summary>
		internal static List<Supply> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Supply> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<Supply> TableToList(DataTable table){
			List<Supply> retVal=new List<Supply>();
			Supply supply;
			for(int i=0;i<table.Rows.Count;i++) {
				supply=new Supply();
				supply.SupplyNum    = PIn.Long  (table.Rows[i]["SupplyNum"].ToString());
				supply.SupplierNum  = PIn.Long  (table.Rows[i]["SupplierNum"].ToString());
				supply.CatalogNumber= PIn.String(table.Rows[i]["CatalogNumber"].ToString());
				supply.Descript     = PIn.String(table.Rows[i]["Descript"].ToString());
				supply.Category     = PIn.Long  (table.Rows[i]["Category"].ToString());
				supply.ItemOrder    = PIn.Int   (table.Rows[i]["ItemOrder"].ToString());
				supply.LevelDesired = PIn.Float (table.Rows[i]["LevelDesired"].ToString());
				supply.IsHidden     = PIn.Bool  (table.Rows[i]["IsHidden"].ToString());
				supply.Price        = PIn.Double(table.Rows[i]["Price"].ToString());
				retVal.Add(supply);
			}
			return retVal;
		}

		///<summary>Inserts one Supply into the database.  Returns the new priKey.</summary>
		internal static long Insert(Supply supply){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				supply.SupplyNum=DbHelper.GetNextOracleKey("supply","SupplyNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(supply,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							supply.SupplyNum++;
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
				return Insert(supply,false);
			}
		}

		///<summary>Inserts one Supply into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(Supply supply,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				supply.SupplyNum=ReplicationServers.GetKey("supply","SupplyNum");
			}
			string command="INSERT INTO supply (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="SupplyNum,";
			}
			command+="SupplierNum,CatalogNumber,Descript,Category,ItemOrder,LevelDesired,IsHidden,Price) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(supply.SupplyNum)+",";
			}
			command+=
				     POut.Long  (supply.SupplierNum)+","
				+"'"+POut.String(supply.CatalogNumber)+"',"
				+"'"+POut.String(supply.Descript)+"',"
				+    POut.Long  (supply.Category)+","
				+    POut.Int   (supply.ItemOrder)+","
				+    POut.Float (supply.LevelDesired)+","
				+    POut.Bool  (supply.IsHidden)+","
				+"'"+POut.Double(supply.Price)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				supply.SupplyNum=Db.NonQ(command,true);
			}
			return supply.SupplyNum;
		}

		///<summary>Updates one Supply in the database.</summary>
		internal static void Update(Supply supply){
			string command="UPDATE supply SET "
				+"SupplierNum  =  "+POut.Long  (supply.SupplierNum)+", "
				+"CatalogNumber= '"+POut.String(supply.CatalogNumber)+"', "
				+"Descript     = '"+POut.String(supply.Descript)+"', "
				+"Category     =  "+POut.Long  (supply.Category)+", "
				+"ItemOrder    =  "+POut.Int   (supply.ItemOrder)+", "
				+"LevelDesired =  "+POut.Float (supply.LevelDesired)+", "
				+"IsHidden     =  "+POut.Bool  (supply.IsHidden)+", "
				+"Price        = '"+POut.Double(supply.Price)+"' "
				+"WHERE SupplyNum = "+POut.Long(supply.SupplyNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Supply in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(Supply supply,Supply oldSupply){
			string command="";
			if(supply.SupplierNum != oldSupply.SupplierNum) {
				if(command!=""){ command+=",";}
				command+="SupplierNum = "+POut.Long(supply.SupplierNum)+"";
			}
			if(supply.CatalogNumber != oldSupply.CatalogNumber) {
				if(command!=""){ command+=",";}
				command+="CatalogNumber = '"+POut.String(supply.CatalogNumber)+"'";
			}
			if(supply.Descript != oldSupply.Descript) {
				if(command!=""){ command+=",";}
				command+="Descript = '"+POut.String(supply.Descript)+"'";
			}
			if(supply.Category != oldSupply.Category) {
				if(command!=""){ command+=",";}
				command+="Category = "+POut.Long(supply.Category)+"";
			}
			if(supply.ItemOrder != oldSupply.ItemOrder) {
				if(command!=""){ command+=",";}
				command+="ItemOrder = "+POut.Int(supply.ItemOrder)+"";
			}
			if(supply.LevelDesired != oldSupply.LevelDesired) {
				if(command!=""){ command+=",";}
				command+="LevelDesired = "+POut.Float(supply.LevelDesired)+"";
			}
			if(supply.IsHidden != oldSupply.IsHidden) {
				if(command!=""){ command+=",";}
				command+="IsHidden = "+POut.Bool(supply.IsHidden)+"";
			}
			if(supply.Price != oldSupply.Price) {
				if(command!=""){ command+=",";}
				command+="Price = '"+POut.Double(supply.Price)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE supply SET "+command
				+" WHERE SupplyNum = "+POut.Long(supply.SupplyNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one Supply from the database.</summary>
		internal static void Delete(long supplyNum){
			string command="DELETE FROM supply "
				+"WHERE SupplyNum = "+POut.Long(supplyNum);
			Db.NonQ(command);
		}

	}
}