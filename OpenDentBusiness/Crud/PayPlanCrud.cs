//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class PayPlanCrud {
		///<summary>Gets one PayPlan object from the database using the primary key.  Returns null if not found.</summary>
		internal static PayPlan SelectOne(long payPlanNum){
			string command="SELECT * FROM payplan "
				+"WHERE PayPlanNum = "+POut.Long(payPlanNum);
			List<PayPlan> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one PayPlan object from the database using a query.</summary>
		internal static PayPlan SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PayPlan> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of PayPlan objects from the database using a query.</summary>
		internal static List<PayPlan> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<PayPlan> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<PayPlan> TableToList(DataTable table){
			List<PayPlan> retVal=new List<PayPlan>();
			PayPlan payPlan;
			for(int i=0;i<table.Rows.Count;i++) {
				payPlan=new PayPlan();
				payPlan.PayPlanNum  = PIn.Long  (table.Rows[i]["PayPlanNum"].ToString());
				payPlan.PatNum      = PIn.Long  (table.Rows[i]["PatNum"].ToString());
				payPlan.Guarantor   = PIn.Long  (table.Rows[i]["Guarantor"].ToString());
				payPlan.PayPlanDate = PIn.Date  (table.Rows[i]["PayPlanDate"].ToString());
				payPlan.APR         = PIn.Double(table.Rows[i]["APR"].ToString());
				payPlan.Note        = PIn.String(table.Rows[i]["Note"].ToString());
				payPlan.PlanNum     = PIn.Long  (table.Rows[i]["PlanNum"].ToString());
				payPlan.CompletedAmt= PIn.Double(table.Rows[i]["CompletedAmt"].ToString());
				payPlan.InsSubNum   = PIn.Long  (table.Rows[i]["InsSubNum"].ToString());
				retVal.Add(payPlan);
			}
			return retVal;
		}

		///<summary>Inserts one PayPlan into the database.  Returns the new priKey.</summary>
		internal static long Insert(PayPlan payPlan){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				payPlan.PayPlanNum=DbHelper.GetNextOracleKey("payplan","PayPlanNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(payPlan,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							payPlan.PayPlanNum++;
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
				return Insert(payPlan,false);
			}
		}

		///<summary>Inserts one PayPlan into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(PayPlan payPlan,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				payPlan.PayPlanNum=ReplicationServers.GetKey("payplan","PayPlanNum");
			}
			string command="INSERT INTO payplan (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="PayPlanNum,";
			}
			command+="PatNum,Guarantor,PayPlanDate,APR,Note,PlanNum,CompletedAmt,InsSubNum) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(payPlan.PayPlanNum)+",";
			}
			command+=
				     POut.Long  (payPlan.PatNum)+","
				+    POut.Long  (payPlan.Guarantor)+","
				+    POut.Date  (payPlan.PayPlanDate)+","
				+"'"+POut.Double(payPlan.APR)+"',"
				+"'"+POut.String(payPlan.Note)+"',"
				+    POut.Long  (payPlan.PlanNum)+","
				+"'"+POut.Double(payPlan.CompletedAmt)+"',"
				+    POut.Long  (payPlan.InsSubNum)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				payPlan.PayPlanNum=Db.NonQ(command,true);
			}
			return payPlan.PayPlanNum;
		}

		///<summary>Updates one PayPlan in the database.</summary>
		internal static void Update(PayPlan payPlan){
			string command="UPDATE payplan SET "
				+"PatNum      =  "+POut.Long  (payPlan.PatNum)+", "
				+"Guarantor   =  "+POut.Long  (payPlan.Guarantor)+", "
				+"PayPlanDate =  "+POut.Date  (payPlan.PayPlanDate)+", "
				+"APR         = '"+POut.Double(payPlan.APR)+"', "
				+"Note        = '"+POut.String(payPlan.Note)+"', "
				+"PlanNum     =  "+POut.Long  (payPlan.PlanNum)+", "
				+"CompletedAmt= '"+POut.Double(payPlan.CompletedAmt)+"', "
				+"InsSubNum   =  "+POut.Long  (payPlan.InsSubNum)+" "
				+"WHERE PayPlanNum = "+POut.Long(payPlan.PayPlanNum);
			Db.NonQ(command);
		}

		///<summary>Updates one PayPlan in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(PayPlan payPlan,PayPlan oldPayPlan){
			string command="";
			if(payPlan.PatNum != oldPayPlan.PatNum) {
				if(command!=""){ command+=",";}
				command+="PatNum = "+POut.Long(payPlan.PatNum)+"";
			}
			if(payPlan.Guarantor != oldPayPlan.Guarantor) {
				if(command!=""){ command+=",";}
				command+="Guarantor = "+POut.Long(payPlan.Guarantor)+"";
			}
			if(payPlan.PayPlanDate != oldPayPlan.PayPlanDate) {
				if(command!=""){ command+=",";}
				command+="PayPlanDate = "+POut.Date(payPlan.PayPlanDate)+"";
			}
			if(payPlan.APR != oldPayPlan.APR) {
				if(command!=""){ command+=",";}
				command+="APR = '"+POut.Double(payPlan.APR)+"'";
			}
			if(payPlan.Note != oldPayPlan.Note) {
				if(command!=""){ command+=",";}
				command+="Note = '"+POut.String(payPlan.Note)+"'";
			}
			if(payPlan.PlanNum != oldPayPlan.PlanNum) {
				if(command!=""){ command+=",";}
				command+="PlanNum = "+POut.Long(payPlan.PlanNum)+"";
			}
			if(payPlan.CompletedAmt != oldPayPlan.CompletedAmt) {
				if(command!=""){ command+=",";}
				command+="CompletedAmt = '"+POut.Double(payPlan.CompletedAmt)+"'";
			}
			if(payPlan.InsSubNum != oldPayPlan.InsSubNum) {
				if(command!=""){ command+=",";}
				command+="InsSubNum = "+POut.Long(payPlan.InsSubNum)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE payplan SET "+command
				+" WHERE PayPlanNum = "+POut.Long(payPlan.PayPlanNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one PayPlan from the database.</summary>
		internal static void Delete(long payPlanNum){
			string command="DELETE FROM payplan "
				+"WHERE PayPlanNum = "+POut.Long(payPlanNum);
			Db.NonQ(command);
		}

	}
}