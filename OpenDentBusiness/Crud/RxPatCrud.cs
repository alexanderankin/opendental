//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class RxPatCrud {
		///<summary>Gets one RxPat object from the database using the primary key.  Returns null if not found.</summary>
		internal static RxPat SelectOne(long rxNum){
			string command="SELECT * FROM rxpat "
				+"WHERE RxNum = "+POut.Long(rxNum);
			List<RxPat> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one RxPat object from the database using a query.</summary>
		internal static RxPat SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<RxPat> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of RxPat objects from the database using a query.</summary>
		internal static List<RxPat> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<RxPat> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<RxPat> TableToList(DataTable table){
			List<RxPat> retVal=new List<RxPat>();
			RxPat rxPat;
			for(int i=0;i<table.Rows.Count;i++) {
				rxPat=new RxPat();
				rxPat.RxNum         = PIn.Long  (table.Rows[i]["RxNum"].ToString());
				rxPat.PatNum        = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				rxPat.RxDate        = PIn.Date  (table.Rows[i]["RxDate"].ToString());
				rxPat.Drug          = PIn.String(table.Rows[i]["Drug"].ToString());
				rxPat.Sig           = PIn.String(table.Rows[i]["Sig"].ToString());
				rxPat.Disp          = PIn.String(table.Rows[i]["Disp"].ToString());
				rxPat.Refills       = PIn.String(table.Rows[i]["Refills"].ToString());
				rxPat.ProvNum       = PIn.Long  (table.Rows[i]["ProvNum"].ToString());
				rxPat.Notes         = PIn.String(table.Rows[i]["Notes"].ToString());
				rxPat.PharmacyNum   = PIn.Long  (table.Rows[i]["PharmacyNum"].ToString());
				rxPat.IsControlled  = PIn.Bool  (table.Rows[i]["IsControlled"].ToString());
				rxPat.DateTStamp    = PIn.DateT (table.Rows[i]["DateTStamp"].ToString());
				rxPat.IsElectQueue  = PIn.Bool  (table.Rows[i]["IsElectQueue"].ToString());
				rxPat.IsDiscontinued= PIn.Bool  (table.Rows[i]["IsDiscontinued"].ToString());
				retVal.Add(rxPat);
			}
			return retVal;
		}

		///<summary>Inserts one RxPat into the database.  Returns the new priKey.</summary>
		internal static long Insert(RxPat rxPat){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				rxPat.RxNum=DbHelper.GetNextOracleKey("rxpat","RxNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(rxPat,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							rxPat.RxNum++;
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
				return Insert(rxPat,false);
			}
		}

		///<summary>Inserts one RxPat into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(RxPat rxPat,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				rxPat.RxNum=ReplicationServers.GetKey("rxpat","RxNum");
			}
			string command="INSERT INTO rxpat (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="RxNum,";
			}
			command+="PatNum,RxDate,Drug,Sig,Disp,Refills,ProvNum,Notes,PharmacyNum,IsControlled,IsElectQueue,IsDiscontinued) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(rxPat.RxNum)+",";
			}
			command+=
				     POut.Long  (rxPat.PatNum)+","
				+    POut.Date  (rxPat.RxDate)+","
				+"'"+POut.String(rxPat.Drug)+"',"
				+"'"+POut.String(rxPat.Sig)+"',"
				+"'"+POut.String(rxPat.Disp)+"',"
				+"'"+POut.String(rxPat.Refills)+"',"
				+    POut.Long  (rxPat.ProvNum)+","
				+"'"+POut.String(rxPat.Notes)+"',"
				+    POut.Long  (rxPat.PharmacyNum)+","
				+    POut.Bool  (rxPat.IsControlled)+","
				//DateTStamp can only be set by MySQL
				+    POut.Bool  (rxPat.IsElectQueue)+","
				+    POut.Bool  (rxPat.IsDiscontinued)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				rxPat.RxNum=Db.NonQ(command,true);
			}
			return rxPat.RxNum;
		}

		///<summary>Updates one RxPat in the database.</summary>
		internal static void Update(RxPat rxPat){
			string command="UPDATE rxpat SET "
				+"PatNum        =  "+POut.Long  (rxPat.PatNum)+", "
				+"RxDate        =  "+POut.Date  (rxPat.RxDate)+", "
				+"Drug          = '"+POut.String(rxPat.Drug)+"', "
				+"Sig           = '"+POut.String(rxPat.Sig)+"', "
				+"Disp          = '"+POut.String(rxPat.Disp)+"', "
				+"Refills       = '"+POut.String(rxPat.Refills)+"', "
				+"ProvNum       =  "+POut.Long  (rxPat.ProvNum)+", "
				+"Notes         = '"+POut.String(rxPat.Notes)+"', "
				+"PharmacyNum   =  "+POut.Long  (rxPat.PharmacyNum)+", "
				+"IsControlled  =  "+POut.Bool  (rxPat.IsControlled)+", "
				//DateTStamp can only be set by MySQL
				+"IsElectQueue  =  "+POut.Bool  (rxPat.IsElectQueue)+", "
				+"IsDiscontinued=  "+POut.Bool  (rxPat.IsDiscontinued)+" "
				+"WHERE RxNum = "+POut.Long(rxPat.RxNum);
			Db.NonQ(command);
		}

		///<summary>Updates one RxPat in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(RxPat rxPat,RxPat oldRxPat){
			string command="";
			if(rxPat.PatNum != oldRxPat.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(rxPat.PatNum)+"";
			}
			if(rxPat.RxDate != oldRxPat.RxDate) {
				if(command!=""){ command+=",";}
				command+="RxDate = "+POut.Date(rxPat.RxDate)+"";
			}
			if(rxPat.Drug != oldRxPat.Drug) {
				if(command!=""){ command+=",";}
				command+="Drug = '"+POut.String(rxPat.Drug)+"'";
			}
			if(rxPat.Sig != oldRxPat.Sig) {
				if(command!=""){ command+=",";}
				command+="Sig = '"+POut.String(rxPat.Sig)+"'";
			}
			if(rxPat.Disp != oldRxPat.Disp) {
				if(command!=""){ command+=",";}
				command+="Disp = '"+POut.String(rxPat.Disp)+"'";
			}
			if(rxPat.Refills != oldRxPat.Refills) {
				if(command!=""){ command+=",";}
				command+="Refills = '"+POut.String(rxPat.Refills)+"'";
			}
			if(rxPat.ProvNum != oldRxPat.ProvNum) {
				if(command!=""){ command+=",";}
				command+="ProvNum = "+POut.Long(rxPat.ProvNum)+"";
			}
			if(rxPat.Notes != oldRxPat.Notes) {
				if(command!=""){ command+=",";}
				command+="Notes = '"+POut.String(rxPat.Notes)+"'";
			}
			if(rxPat.PharmacyNum != oldRxPat.PharmacyNum) {
				if(command!=""){ command+=",";}
				command+="PharmacyNum = "+POut.Long(rxPat.PharmacyNum)+"";
			}
			if(rxPat.IsControlled != oldRxPat.IsControlled) {
				if(command!=""){ command+=",";}
				command+="IsControlled = "+POut.Bool(rxPat.IsControlled)+"";
			}
			//DateTStamp can only be set by MySQL
			if(rxPat.IsElectQueue != oldRxPat.IsElectQueue) {
				if(command!=""){ command+=",";}
				command+="IsElectQueue = "+POut.Bool(rxPat.IsElectQueue)+"";
			}
			if(rxPat.IsDiscontinued != oldRxPat.IsDiscontinued) {
				if(command!=""){ command+=",";}
				command+="IsDiscontinued = "+POut.Bool(rxPat.IsDiscontinued)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE rxpat SET "+command
				+" WHERE RxNum = "+POut.Long(rxPat.RxNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one RxPat from the database.</summary>
		internal static void Delete(long rxNum){
			string command="DELETE FROM rxpat "
				+"WHERE RxNum = "+POut.Long(rxNum);
			Db.NonQ(command);
		}

	}
}