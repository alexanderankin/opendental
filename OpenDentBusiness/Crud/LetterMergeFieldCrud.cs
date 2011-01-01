//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class LetterMergeFieldCrud {
		///<summary>Gets one LetterMergeField object from the database using the primary key.  Returns null if not found.</summary>
		internal static LetterMergeField SelectOne(long fieldNum){
			string command="SELECT * FROM lettermergefield "
				+"WHERE FieldNum = "+POut.Long(fieldNum)+" LIMIT 1";
			List<LetterMergeField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one LetterMergeField object from the database using a query.</summary>
		internal static LetterMergeField SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<LetterMergeField> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of LetterMergeField objects from the database using a query.</summary>
		internal static List<LetterMergeField> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<LetterMergeField> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<LetterMergeField> TableToList(DataTable table){
			List<LetterMergeField> retVal=new List<LetterMergeField>();
			LetterMergeField letterMergeField;
			for(int i=0;i<table.Rows.Count;i++) {
				letterMergeField=new LetterMergeField();
				letterMergeField.FieldNum      = PIn.Long  (table.Rows[i]["FieldNum"].ToString());
				letterMergeField.LetterMergeNum= PIn.Long  (table.Rows[i]["LetterMergeNum"].ToString());
				letterMergeField.FieldName     = PIn.String(table.Rows[i]["FieldName"].ToString());
				retVal.Add(letterMergeField);
			}
			return retVal;
		}

		///<summary>Inserts one LetterMergeField into the database.  Returns the new priKey.</summary>
		internal static long Insert(LetterMergeField letterMergeField){
			return Insert(letterMergeField,false);
		}

		///<summary>Inserts one LetterMergeField into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(LetterMergeField letterMergeField,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				letterMergeField.FieldNum=ReplicationServers.GetKey("lettermergefield","FieldNum");
			}
			string command="INSERT INTO lettermergefield (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="FieldNum,";
			}
			command+="LetterMergeNum,FieldName) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(letterMergeField.FieldNum)+",";
			}
			command+=
				     POut.Long  (letterMergeField.LetterMergeNum)+","
				+"'"+POut.String(letterMergeField.FieldName)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				letterMergeField.FieldNum=Db.NonQ(command,true);
			}
			return letterMergeField.FieldNum;
		}

		///<summary>Updates one LetterMergeField in the database.</summary>
		internal static void Update(LetterMergeField letterMergeField){
			string command="UPDATE lettermergefield SET "
				+"LetterMergeNum=  "+POut.Long  (letterMergeField.LetterMergeNum)+", "
				+"FieldName     = '"+POut.String(letterMergeField.FieldName)+"' "
				+"WHERE FieldNum = "+POut.Long(letterMergeField.FieldNum);
			Db.NonQ(command);
		}

		///<summary>Updates one LetterMergeField in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(LetterMergeField letterMergeField,LetterMergeField oldLetterMergeField){
			string command="";
			if(letterMergeField.LetterMergeNum != oldLetterMergeField.LetterMergeNum) {
				if(command!=""){ command+=",";}
				command+="LetterMergeNum = "+POut.Long(letterMergeField.LetterMergeNum)+"";
			}
			if(letterMergeField.FieldName != oldLetterMergeField.FieldName) {
				if(command!=""){ command+=",";}
				command+="FieldName = '"+POut.String(letterMergeField.FieldName)+"'";
			}
			if(command==""){
				return;
			}
			command="UPDATE lettermergefield SET "+command
				+" WHERE FieldNum = "+POut.Long(letterMergeField.FieldNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one LetterMergeField from the database.</summary>
		internal static void Delete(long fieldNum){
			string command="DELETE FROM lettermergefield "
				+"WHERE FieldNum = "+POut.Long(fieldNum);
			Db.NonQ(command);
		}

	}
}