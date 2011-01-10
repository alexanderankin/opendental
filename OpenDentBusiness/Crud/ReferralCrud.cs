//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class ReferralCrud {
		///<summary>Gets one Referral object from the database using the primary key.  Returns null if not found.</summary>
		internal static Referral SelectOne(long referralNum){
			string command="SELECT * FROM referral "
				+"WHERE ReferralNum = "+POut.Long(referralNum);
			List<Referral> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one Referral object from the database using a query.</summary>
		internal static Referral SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Referral> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of Referral objects from the database using a query.</summary>
		internal static List<Referral> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<Referral> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<Referral> TableToList(DataTable table){
			List<Referral> retVal=new List<Referral>();
			Referral referral;
			for(int i=0;i<table.Rows.Count;i++) {
				referral=new Referral();
				referral.ReferralNum   = PIn.Long  (table.Rows[i]["ReferralNum"].ToString());
				referral.LName         = PIn.String(table.Rows[i]["LName"].ToString());
				referral.FName         = PIn.String(table.Rows[i]["FName"].ToString());
				referral.MName         = PIn.String(table.Rows[i]["MName"].ToString());
				referral.SSN           = PIn.String(table.Rows[i]["SSN"].ToString());
				referral.UsingTIN      = PIn.Bool  (table.Rows[i]["UsingTIN"].ToString());
				referral.Specialty     = (DentalSpecialty)PIn.Int(table.Rows[i]["Specialty"].ToString());
				referral.ST            = PIn.String(table.Rows[i]["ST"].ToString());
				referral.Telephone     = PIn.String(table.Rows[i]["Telephone"].ToString());
				referral.Address       = PIn.String(table.Rows[i]["Address"].ToString());
				referral.Address2      = PIn.String(table.Rows[i]["Address2"].ToString());
				referral.City          = PIn.String(table.Rows[i]["City"].ToString());
				referral.Zip           = PIn.String(table.Rows[i]["Zip"].ToString());
				referral.Note          = PIn.String(table.Rows[i]["Note"].ToString());
				referral.Phone2        = PIn.String(table.Rows[i]["Phone2"].ToString());
				referral.IsHidden      = PIn.Bool  (table.Rows[i]["IsHidden"].ToString());
				referral.NotPerson     = PIn.Bool  (table.Rows[i]["NotPerson"].ToString());
				referral.Title         = PIn.String(table.Rows[i]["Title"].ToString());
				referral.EMail         = PIn.String(table.Rows[i]["EMail"].ToString());
				referral.PatNum        = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				referral.NationalProvID= PIn.String(table.Rows[i]["NationalProvID"].ToString());
				referral.Slip          = PIn.Long  (table.Rows[i]["Slip"].ToString());
				retVal.Add(referral);
			}
			return retVal;
		}

		///<summary>Inserts one Referral into the database.  Returns the new priKey.</summary>
		internal static long Insert(Referral referral){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				referral.ReferralNum=DbHelper.GetNextOracleKey("referral","ReferralNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(referral,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							referral.ReferralNum++;
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
				return Insert(referral,false);
			}
		}

		///<summary>Inserts one Referral into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(Referral referral,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				referral.ReferralNum=ReplicationServers.GetKey("referral","ReferralNum");
			}
			string command="INSERT INTO referral (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="ReferralNum,";
			}
			command+="LName,FName,MName,SSN,UsingTIN,Specialty,ST,Telephone,Address,Address2,City,Zip,Note,Phone2,IsHidden,NotPerson,Title,EMail,PatNum,NationalProvID,Slip) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(referral.ReferralNum)+",";
			}
			command+=
				 "'"+POut.String(referral.LName)+"',"
				+"'"+POut.String(referral.FName)+"',"
				+"'"+POut.String(referral.MName)+"',"
				+"'"+POut.String(referral.SSN)+"',"
				+    POut.Bool  (referral.UsingTIN)+","
				+    POut.Int   ((int)referral.Specialty)+","
				+"'"+POut.String(referral.ST)+"',"
				+"'"+POut.String(referral.Telephone)+"',"
				+"'"+POut.String(referral.Address)+"',"
				+"'"+POut.String(referral.Address2)+"',"
				+"'"+POut.String(referral.City)+"',"
				+"'"+POut.String(referral.Zip)+"',"
				+"'"+POut.String(referral.Note)+"',"
				+"'"+POut.String(referral.Phone2)+"',"
				+    POut.Bool  (referral.IsHidden)+","
				+    POut.Bool  (referral.NotPerson)+","
				+"'"+POut.String(referral.Title)+"',"
				+"'"+POut.String(referral.EMail)+"',"
				+    POut.Long  (referral.PatNum)+","
				+"'"+POut.String(referral.NationalProvID)+"',"
				+    POut.Long  (referral.Slip)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				referral.ReferralNum=Db.NonQ(command,true);
			}
			return referral.ReferralNum;
		}

		///<summary>Updates one Referral in the database.</summary>
		internal static void Update(Referral referral){
			string command="UPDATE referral SET "
				+"LName         = '"+POut.String(referral.LName)+"', "
				+"FName         = '"+POut.String(referral.FName)+"', "
				+"MName         = '"+POut.String(referral.MName)+"', "
				+"SSN           = '"+POut.String(referral.SSN)+"', "
				+"UsingTIN      =  "+POut.Bool  (referral.UsingTIN)+", "
				+"Specialty     =  "+POut.Int   ((int)referral.Specialty)+", "
				+"ST            = '"+POut.String(referral.ST)+"', "
				+"Telephone     = '"+POut.String(referral.Telephone)+"', "
				+"Address       = '"+POut.String(referral.Address)+"', "
				+"Address2      = '"+POut.String(referral.Address2)+"', "
				+"City          = '"+POut.String(referral.City)+"', "
				+"Zip           = '"+POut.String(referral.Zip)+"', "
				+"Note          = '"+POut.String(referral.Note)+"', "
				+"Phone2        = '"+POut.String(referral.Phone2)+"', "
				+"IsHidden      =  "+POut.Bool  (referral.IsHidden)+", "
				+"NotPerson     =  "+POut.Bool  (referral.NotPerson)+", "
				+"Title         = '"+POut.String(referral.Title)+"', "
				+"EMail         = '"+POut.String(referral.EMail)+"', "
				+"PatNum        =  "+POut.Long  (referral.PatNum)+", "
				+"NationalProvID= '"+POut.String(referral.NationalProvID)+"', "
				+"Slip          =  "+POut.Long  (referral.Slip)+" "
				+"WHERE ReferralNum = "+POut.Long(referral.ReferralNum);
			Db.NonQ(command);
		}

		///<summary>Updates one Referral in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(Referral referral,Referral oldReferral){
			string command="";
			if(referral.LName != oldReferral.LName) {
				if(command!=""){ command+=",";}
				command+="LName = '"+POut.String(referral.LName)+"'";
			}
			if(referral.FName != oldReferral.FName) {
				if(command!=""){ command+=",";}
				command+="FName = '"+POut.String(referral.FName)+"'";
			}
			if(referral.MName != oldReferral.MName) {
				if(command!=""){ command+=",";}
				command+="MName = '"+POut.String(referral.MName)+"'";
			}
			if(referral.SSN != oldReferral.SSN) {
				if(command!=""){ command+=",";}
				command+="SSN = '"+POut.String(referral.SSN)+"'";
			}
			if(referral.UsingTIN != oldReferral.UsingTIN) {
				if(command!=""){ command+=",";}
				command+="UsingTIN = "+POut.Bool(referral.UsingTIN)+"";
			}
			if(referral.Specialty != oldReferral.Specialty) {
				if(command!=""){ command+=",";}
				command+="Specialty = "+POut.Int   ((int)referral.Specialty)+"";
			}
			if(referral.ST != oldReferral.ST) {
				if(command!=""){ command+=",";}
				command+="ST = '"+POut.String(referral.ST)+"'";
			}
			if(referral.Telephone != oldReferral.Telephone) {
				if(command!=""){ command+=",";}
				command+="Telephone = '"+POut.String(referral.Telephone)+"'";
			}
			if(referral.Address != oldReferral.Address) {
				if(command!=""){ command+=",";}
				command+="Address = '"+POut.String(referral.Address)+"'";
			}
			if(referral.Address2 != oldReferral.Address2) {
				if(command!=""){ command+=",";}
				command+="Address2 = '"+POut.String(referral.Address2)+"'";
			}
			if(referral.City != oldReferral.City) {
				if(command!=""){ command+=",";}
				command+="City = '"+POut.String(referral.City)+"'";
			}
			if(referral.Zip != oldReferral.Zip) {
				if(command!=""){ command+=",";}
				command+="Zip = '"+POut.String(referral.Zip)+"'";
			}
			if(referral.Note != oldReferral.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(referral.Note)+"'";
			}
			if(referral.Phone2 != oldReferral.Phone2) {
				if(command!=""){ command+=",";}
				command+="Phone2 = '"+POut.String(referral.Phone2)+"'";
			}
			if(referral.IsHidden != oldReferral.IsHidden) {
				if(command!=""){ command+=",";}
				command+="IsHidden = "+POut.Bool(referral.IsHidden)+"";
			}
			if(referral.NotPerson != oldReferral.NotPerson) {
				if(command!=""){ command+=",";}
				command+="NotPerson = "+POut.Bool(referral.NotPerson)+"";
			}
			if(referral.Title != oldReferral.Title) {
				if(command!=""){ command+=",";}
				command+="Title = '"+POut.String(referral.Title)+"'";
			}
			if(referral.EMail != oldReferral.EMail) {
				if(command!=""){ command+=",";}
				command+="EMail = '"+POut.String(referral.EMail)+"'";
			}
			if(referral.PatNum != oldReferral.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(referral.PatNum)+"";
			}
			if(referral.NationalProvID != oldReferral.NationalProvID) {
				if(command!=""){ command+=",";}
				command+="NationalProvID = '"+POut.String(referral.NationalProvID)+"'";
			}
			if(referral.Slip != oldReferral.Slip) {
				if(command!=""){ command+=",";}
				command+="Slip = "+POut.Long(referral.Slip)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE referral SET "+command
				+" WHERE ReferralNum = "+POut.Long(referral.ReferralNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one Referral from the database.</summary>
		internal static void Delete(long referralNum){
			string command="DELETE FROM referral "
				+"WHERE ReferralNum = "+POut.Long(referralNum);
			Db.NonQ(command);
		}

	}
}