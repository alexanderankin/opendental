using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SigButDefElements {
		///<summary>A list of all elements for all buttons</summary>
		private static SigButDefElement[] list;

		public static SigButDefElement[] List {
			//No need to check RemotingRole; no call to db.
			get {
				if(list==null) {
					RefreshCache();
				}
				return list;
			}
			set {
				list=value;
			}
		}

		///<summary>Gets all SigButDefElements for all buttons, ordered by type: user,extras, message.</summary>
		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM sigbutdefelement";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="SigButDefElement";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			list=new SigButDefElement[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				list[i]=new SigButDefElement();
				list[i].ElementNum      = PIn.PInt(table.Rows[i][0].ToString());
				list[i].SigButDefNum    = PIn.PInt(table.Rows[i][1].ToString());
				list[i].SigElementDefNum= PIn.PInt(table.Rows[i][2].ToString());
			}
			//Array.Sort(List);
		}

		/*
		///<summary>This will never happen</summary>
		public void Update(){
			string command= "UPDATE SigButDefElement SET " 
				+"FromUser = '"    +POut.PString(FromUser)+"'"
				+",ITypes = '"     +POut.PInt   ((int)ITypes)+"'"
				+",DateViewing = '"+POut.PDate  (DateViewing)+"'"
				+",SigType = '"    +POut.PInt   ((int)SigType)+"'"
				+" WHERE SigButDefElementNum = '"+POut.PInt(SigButDefElementNum)+"'";
			DataConnection dcon=new DataConnection();
 			Db.NonQ(command);
		}*/

		///<summary></summary>
		public static long Insert(SigButDefElement element) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				element.ElementNum=Meth.GetInt(MethodBase.GetCurrentMethod(),element);
				return element.ElementNum;
			}
			if(PrefC.RandomKeys) {
				element.ElementNum=ReplicationServers.GetKey("sigbutdefelement","ElementNum");
			}
			string command="INSERT INTO sigbutdefelement (";
			if(PrefC.RandomKeys) {
				command+="ElementNum,";
			}
			command+="SigButDefNum,SigElementDefNum) VALUES(";
			if(PrefC.RandomKeys) {
				command+=POut.PInt(element.ElementNum)+", ";
			}
			command+=
				 "'"+POut.PInt   (element.SigButDefNum)+"', "
				+"'"+POut.PInt   (element.SigElementDefNum)+"')";
			if(PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				element.ElementNum=Db.NonQ(command,true);
			}
			return element.ElementNum;
		}

		///<summary></summary>
		public static void Delete(SigButDefElement element){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),element);
				return;
			}
			string command= "DELETE from sigbutdefelement WHERE ElementNum = '"+POut.PInt(element.ElementNum)+"'";
 			Db.NonQ(command);
		}



		///<summary>Loops through the SigButDefElement list and pulls out all elements for one specific button.</summary>
		public static SigButDefElement[] GetForButton(long sigButDefNum) {
			//No need to check RemotingRole; no call to db.
			ArrayList AL=new ArrayList();
			for(int i=0;i<List.Length;i++){
				if(List[i].SigButDefNum==sigButDefNum){
					AL.Add(List[i].Copy());
				}
			}
			SigButDefElement[] retVal=new SigButDefElement[AL.Count];
			AL.CopyTo(retVal);
			return retVal;
			//already ordered because List is ordered.
		}

		

	
	}

	

	


}




















