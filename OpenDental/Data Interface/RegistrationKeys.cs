using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;
using System.Data;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using CodeBase;
using System.IO;
using CDT;

namespace OpenDental {
	///<summary>Used to keep track of which product keys have been assigned to which customers. This class is only used if the program is being run from a distributor installation.</summary>
	class RegistrationKeys {
		///<summary>Retrieves all registration keys for a particular customer. There can be multiple keys assigned to a single customer, since the customer may have multiple physical locations of business.</summary>
		public static RegistrationKey[] GetForPatient(int patNum){
			string command="SELECT * FROM registrationkey WHERE PatNum='"+patNum+"'";
			DataTable table=General.GetTable(command);
			RegistrationKey[] keys=new RegistrationKey[table.Rows.Count];
			for(int i=0;i<keys.Length;i++){
				keys[i]=new RegistrationKey();
				keys[i].RegistrationKeyNum	=PIn.PInt(table.Rows[i][0].ToString());
				keys[i].PatNum							=PIn.PInt(table.Rows[i][1].ToString());
				keys[i].RegKey							=PIn.PString(table.Rows[i][2].ToString());
				keys[i].Note								=PIn.PString(table.Rows[i][3].ToString());
			}
			return keys;
		}

		///<summary>Returns true if the given registration key is currently in use by a customer, false otherwise.</summary>
		public static bool KeyIsInUse(string regKey){
			string command="SELECT RegKey FROM registrationkey WHERE RegKey='"+POut.PString(regKey)+"'";
			DataTable table=General.GetTable(command);
			return(table.Rows.Count>0);
		}

		///<summary>Inserts a new and unique registration key into the database.</summary>
		public static void Create(RegistrationKey registrationKey){
			do{
				registrationKey.RegKey=CDT.Class1.GenerateRandKey();
				if(registrationKey.RegKey==""){
					//Don't loop forever when software is unverified.
					return;
				}
			} while(KeyIsInUse(registrationKey.RegKey));
			string command="INSERT INTO registrationkey (PatNum,RegKey,Note) VALUES ("+
				"'"+POut.PInt(registrationKey.PatNum)+"',"+
				"'"+POut.PString(registrationKey.RegKey)+"',"+
				"'"+POut.PString(registrationKey.Note)+"')";
			General.NonQ(command);
		}

	}
}
