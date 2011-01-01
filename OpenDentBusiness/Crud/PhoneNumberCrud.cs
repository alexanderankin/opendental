//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class PhoneNumberCrud {
		///<summary>Gets one PhoneNumber object from the database using the primary key.  Returns null if not found.</summary>
		internal static PhoneNumber SelectOne(long phoneNumberNum){
			string command="SELECT * FROM phonenumber "
				+"WHERE PhoneNumberNum = "+POut.Long(phoneNumberNum)+" LIMIT 1";
			List<PhoneNumber> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PhoneNumber object from the database using a query.</summary>
		internal static PhoneNumber SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PhoneNumber> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PhoneNumber objects from the database using a query.</summary>
		internal static List<PhoneNumber> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PhoneNumber> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<PhoneNumber> TableToList(DataTable table){
			List<PhoneNumber> retVal=new List<PhoneNumber>();
			PhoneNumber phoneNumber;
			for(int i=0;i<table.Rows.Count;i++) {
				phoneNumber=new PhoneNumber();
				phoneNumber.PhoneNumberNum= PIn.Long  (table.Rows[i]["PhoneNumberNum"].ToString());
				phoneNumber.PatNum        = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				phoneNumber.PhoneNumberVal= PIn.String(table.Rows[i]["PhoneNumberVal"].ToString());
				retVal.Add(phoneNumber);
			}
			return retVal;
		}

		///<summary>Inserts one PhoneNumber into the database.  Returns the new priKey.</summary>
		internal static long Insert(PhoneNumber phoneNumber){
			return Insert(phoneNumber,false);
		}

		///<summary>Inserts one PhoneNumber into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(PhoneNumber phoneNumber,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				phoneNumber.PhoneNumberNum=ReplicationServers.GetKey("phonenumber","PhoneNumberNum");
			}
			string command="INSERT INTO phonenumber (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PhoneNumberNum,";
			}
			command+="PatNum,PhoneNumberVal) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(phoneNumber.PhoneNumberNum)+",";
			}
			command+=
				     POut.Long  (phoneNumber.PatNum)+","
				+"'"+POut.String(phoneNumber.PhoneNumberVal)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				phoneNumber.PhoneNumberNum=Db.NonQ(command,true);
			}
			return phoneNumber.PhoneNumberNum;
		}

		///<summary>Updates one PhoneNumber in the database.</summary>
		internal static void Update(PhoneNumber phoneNumber){
			string command="UPDATE phonenumber SET "
				+"PatNum        =  "+POut.Long  (phoneNumber.PatNum)+", "
				+"PhoneNumberVal= '"+POut.String(phoneNumber.PhoneNumberVal)+"' "
				+"WHERE PhoneNumberNum = "+POut.Long(phoneNumber.PhoneNumberNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PhoneNumber in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(PhoneNumber phoneNumber,PhoneNumber oldPhoneNumber){
			string command="";
			if(phoneNumber.PatNum != oldPhoneNumber.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(phoneNumber.PatNum)+"";
			}
			if(phoneNumber.PhoneNumberVal != oldPhoneNumber.PhoneNumberVal) {
				if(command!=""){ command+=",";}
				command+="PhoneNumberVal = '"+POut.String(phoneNumber.PhoneNumberVal)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE phonenumber SET "+command
				+" WHERE PhoneNumberNum = "+POut.Long(phoneNumber.PhoneNumberNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one PhoneNumber from the database.</summary>
		internal static void Delete(long phoneNumberNum){
			string command="DELETE FROM phonenumber "
				+"WHERE PhoneNumberNum = "+POut.Long(phoneNumberNum);
			Db.NonQ(command);
		}

	}
}