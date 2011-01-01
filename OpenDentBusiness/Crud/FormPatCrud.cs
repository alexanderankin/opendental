//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class FormPatCrud {
		///<summary>Gets one FormPat object from the database using the primary key.  Returns null if not found.</summary>
		internal static FormPat SelectOne(long formPatNum){
			string command="SELECT * FROM formpat "
				+"WHERE FormPatNum = "+POut.Long(formPatNum)+" LIMIT 1";
			List<FormPat> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one FormPat object from the database using a query.</summary>
		internal static FormPat SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<FormPat> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of FormPat objects from the database using a query.</summary>
		internal static List<FormPat> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<FormPat> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<FormPat> TableToList(DataTable table){
			List<FormPat> retVal=new List<FormPat>();
			FormPat formPat;
			for(int i=0;i<table.Rows.Count;i++) {
				formPat=new FormPat();
				formPat.FormPatNum  = PIn.Long  (table.Rows[i]["FormPatNum"].ToString());
				formPat.PatNum      = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				formPat.FormDateTime= PIn.DateT (table.Rows[i]["FormDateTime"].ToString());
				retVal.Add(formPat);
			}
			return retVal;
		}

		///<summary>Inserts one FormPat into the database.  Returns the new priKey.</summary>
		internal static long Insert(FormPat formPat){
			return Insert(formPat,false);
		}

		///<summary>Inserts one FormPat into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(FormPat formPat,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				formPat.FormPatNum=ReplicationServers.GetKey("formpat","FormPatNum");
			}
			string command="INSERT INTO formpat (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="FormPatNum,";
			}
			command+="PatNum,FormDateTime) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(formPat.FormPatNum)+",";
			}
			command+=
				     POut.Long  (formPat.PatNum)+","
				+    POut.DateT (formPat.FormDateTime)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				formPat.FormPatNum=Db.NonQ(command,true);
			}
			return formPat.FormPatNum;
		}

		///<summary>Updates one FormPat in the database.</summary>
		internal static void Update(FormPat formPat){
			string command="UPDATE formpat SET "
				+"PatNum      =  "+POut.Long  (formPat.PatNum)+", "
				+"FormDateTime=  "+POut.DateT (formPat.FormDateTime)+" "
				+"WHERE FormPatNum = "+POut.Long(formPat.FormPatNum);
			Db.NonQ(command);
		}

		///<summary>Updates one FormPat in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(FormPat formPat,FormPat oldFormPat){
			string command="";
			if(formPat.PatNum != oldFormPat.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(formPat.PatNum)+"";
			}
			if(formPat.FormDateTime != oldFormPat.FormDateTime) {
				if(command!=""){ command+=",";}
				command+="FormDateTime = "+POut.DateT(formPat.FormDateTime)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE formpat SET "+command
				+" WHERE FormPatNum = "+POut.Long(formPat.FormPatNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one FormPat from the database.</summary>
		internal static void Delete(long formPatNum){
			string command="DELETE FROM formpat "
				+"WHERE FormPatNum = "+POut.Long(formPatNum);
			Db.NonQ(command);
		}

	}
}