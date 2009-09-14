using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace OpenDentBusiness {
	///<summary></summary>
	public class RxAlerts {

		///<summary>Gets a list of all RxAlerts for one RxDef.</summary>
		public static RxAlert[] Refresh(long rxDefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<RxAlert[]>(MethodBase.GetCurrentMethod(),rxDefNum);
			}
			string command="SELECT * FROM rxalert WHERE RxDefNum="+POut.PInt(rxDefNum);
			DataTable table=Db.GetTable(command);
			RxAlert[] List=new RxAlert[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				List[i]=new RxAlert();
				List[i].RxAlertNum   = PIn.PInt(table.Rows[i][0].ToString());
				List[i].RxDefNum     = PIn.PInt(table.Rows[i][1].ToString());
				List[i].DiseaseDefNum= PIn.PInt(table.Rows[i][2].ToString());
			}
			return List;
		}
	

		///<summary></summary>
		public static void Update(RxAlert alert) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),alert);
				return;
			}
			string command="UPDATE rxalert SET " 
				+"RxDefNum = '"      +POut.PInt   (alert.RxDefNum)+"'"
				+",DiseaseDefNum = '"+POut.PInt   (alert.DiseaseDefNum)+"'"
				+" WHERE RxAlertNum  ='"+POut.PInt   (alert.RxAlertNum)+"'";
			Db.NonQ(command);
		}

		///<summary></summary>
		public static long Insert(RxAlert alert) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				alert.RxAlertNum=Meth.GetInt(MethodBase.GetCurrentMethod(),alert);
				return alert.RxAlertNum;
			}
			if(PrefC.RandomKeys) {
				alert.RxAlertNum=ReplicationServers.GetKey("rxalert","RxAlertNum");
			}
			string command="INSERT INTO rxalert (";
			if(PrefC.RandomKeys) {
				command+="RxAlertNum,";
			}
			command+="RxDefNum,DiseaseDefNum) VALUES(";
			if(PrefC.RandomKeys) {
				command+=POut.PInt(alert.RxAlertNum)+", ";
			}
			command+=
				 "'"+POut.PInt   (alert.RxDefNum)+"', "
				+"'"+POut.PInt   (alert.DiseaseDefNum)+"')";
			if(PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else{
				alert.RxAlertNum=Db.NonQ(command,true);
			}
			return alert.RxAlertNum;
		}

		///<summary></summary>
		public static void Delete(RxAlert alert) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),alert);
				return;
			}
			string command="DELETE FROM rxalert WHERE RxAlertNum ="+POut.PInt(alert.RxAlertNum);
			Db.NonQ(command);
		}

	
	

		
		
		
	}

		



		
	

	

	


}










