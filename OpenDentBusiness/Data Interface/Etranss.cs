using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using OpenDentBusiness;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Etranss {

		///<summary>Gets data for the history grid in the SendClaims window.</summary>
		public static DataTable RefreshHistory(DateTime dateFrom,DateTime dateTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateFrom,dateTo);
			}
			string command="Select CONCAT(CONCAT(patient.LName,', '),patient.FName) AS PatName,carrier.CarrierName,"
				+"clearinghouse.Description AS Clearinghouse,DateTimeTrans,etrans.OfficeSequenceNumber,"
				+"etrans.CarrierTransCounter,Etype,etrans.ClaimNum,etrans.EtransNum,etrans.AckCode,etrans.Note "
				+"FROM etrans "
				+"LEFT JOIN carrier ON etrans.CarrierNum=carrier.CarrierNum "
				+"LEFT JOIN patient ON patient.PatNum=etrans.PatNum "
				+"LEFT JOIN clearinghouse ON clearinghouse.ClearinghouseNum=etrans.ClearinghouseNum WHERE "
				//if(DataConnection.DBtype==DatabaseType.Oracle){
				//	command+="TO_";
				//}
				+"DATE(DateTimeTrans) >= "+POut.PDate(dateFrom)+" AND "
				//if(DataConnection.DBtype==DatabaseType.Oracle){
				//	command+="TO_";
				//}
				+"DATE(DateTimeTrans) <= "+POut.PDate(dateTo)+" "
				+"AND Etype!="+POut.PInt((int)EtransType.Acknowledge_997)+" "
				+"AND Etype!="+POut.PInt((int)EtransType.BenefitInquiry270)+" "
				+"AND Etype!="+POut.PInt((int)EtransType.BenefitResponse271)+" "
				+"ORDER BY DateTimeTrans";
			DataTable table=Db.GetTable(command);
			DataTable tHist=new DataTable("Table");
			tHist.Columns.Add("patName");
			tHist.Columns.Add("CarrierName");
			tHist.Columns.Add("Clearinghouse");
			tHist.Columns.Add("dateTimeTrans");
			tHist.Columns.Add("OfficeSequenceNumber");
			tHist.Columns.Add("CarrierTransCounter");
			tHist.Columns.Add("etype");
			tHist.Columns.Add("Etype");
			tHist.Columns.Add("ClaimNum");
			tHist.Columns.Add("EtransNum");
			tHist.Columns.Add("ack");
			tHist.Columns.Add("Note");
			DataRow row;
			string etype;
			for(int i=0;i<table.Rows.Count;i++) {
				row=tHist.NewRow();
				row["patName"]=table.Rows[i]["PatName"].ToString();
				row["CarrierName"]=table.Rows[i]["CarrierName"].ToString();
				row["Clearinghouse"]=table.Rows[i]["Clearinghouse"].ToString();
				row["dateTimeTrans"]=PIn.PDateT(table.Rows[i]["DateTimeTrans"].ToString()).ToShortDateString();
				row["OfficeSequenceNumber"]=table.Rows[i]["OfficeSequenceNumber"].ToString();
				row["CarrierTransCounter"]=table.Rows[i]["CarrierTransCounter"].ToString();
				row["Etype"]=table.Rows[i]["Etype"].ToString();
				etype=Lans.g("enumEtransType",((EtransType)PIn.PInt(table.Rows[i]["Etype"].ToString())).ToString());
				if(etype.EndsWith("_CA")){
					etype=etype.Substring(0,etype.Length-3);
				}
				row["etype"]=etype;
				row["ClaimNum"]=table.Rows[i]["ClaimNum"].ToString();
				row["EtransNum"]=table.Rows[i]["EtransNum"].ToString();
				if(table.Rows[i]["AckCode"].ToString()=="A"){
					row["ack"]=Lans.g("Etrans","Accepted");
				}
				else if(table.Rows[i]["AckCode"].ToString()=="R") {
					row["ack"]=Lans.g("Etrans","Rejected");
				}
				row["Note"]=table.Rows[i]["Note"].ToString();
				tHist.Rows.Add(row);
			}
			return tHist;
		}

		///<summary></summary>
		public static Etrans GetEtrans(int etransNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Etrans>(MethodBase.GetCurrentMethod(),etransNum);
			}
			string command="SELECT * FROM etrans WHERE EtransNum="+POut.PInt(etransNum);
			DataTable table=Db.GetTable(command);
			List<Etrans> list=SubmitAndFill(table);
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of all 270's for this plan.</summary>
		public static List<Etrans> GetList270ForPlan(int planNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Etrans>>(MethodBase.GetCurrentMethod(),planNum);
			}
			string command="SELECT * FROM etrans WHERE PlanNum="+POut.PInt(planNum)
				+" AND Etype="+POut.PInt((int)EtransType.BenefitInquiry270);
			DataTable table=Db.GetTable(command);
			return SubmitAndFill(table);
		}

		/*
		///<summary></summary>
		public static Etrans GetAckForTrans(int etransNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Etrans>(MethodBase.GetCurrentMethod(),etransNum);
			}
			//first, get the actual trans.
			string command="SELECT * FROM etrans WHERE EtransNum="+POut.PInt(etransNum);
			DataTable table=Db.GetTable(command);
			Etrans etrans=SubmitAndFill(table);
			command="SELECT * FROM etrans WHERE "
				+"Etype=21 "//ack997
				+"AND ClearinghouseNum="+POut.PInt(etrans.ClearinghouseNum)
				+" AND BatchNumber= "+POut.PInt(etrans.BatchNumber)
				+" AND DateTimeTrans < "+POut.PDateT(etrans.DateTimeTrans.AddDays(14))//less than 2wks in the future
				+" AND DateTimeTrans > "+POut.PDateT(etrans.DateTimeTrans.AddDays(-1));//and no more than one day before claim
			table=Db.GetTable(command);
			return SubmitAndFill(table);
		}*/

		private static List<Etrans> SubmitAndFill(DataTable table){
			//No need to check RemotingRole; no call to db.
			//if(table.Rows.Count==0){
			//	return null;
			//}
			List<Etrans> retVal=new List<Etrans>();
			Etrans etrans;
			for(int i=0;i<table.Rows.Count;i++) {
				etrans=new Etrans();
				etrans.EtransNum           =PIn.PInt(table.Rows[i][0].ToString());
				etrans.DateTimeTrans       =PIn.PDateT(table.Rows[i][1].ToString());
				etrans.ClearinghouseNum    =PIn.PInt(table.Rows[i][2].ToString());
				etrans.Etype               =(EtransType)PIn.PInt(table.Rows[i][3].ToString());
				etrans.ClaimNum            =PIn.PInt(table.Rows[i][4].ToString());
				etrans.OfficeSequenceNumber=PIn.PInt(table.Rows[i][5].ToString());
				etrans.CarrierTransCounter =PIn.PInt(table.Rows[i][6].ToString());
				etrans.CarrierTransCounter2=PIn.PInt(table.Rows[i][7].ToString());
				etrans.CarrierNum          =PIn.PInt(table.Rows[i][8].ToString());
				etrans.CarrierNum2         =PIn.PInt(table.Rows[i][9].ToString());
				etrans.PatNum              =PIn.PInt(table.Rows[i][10].ToString());
				etrans.BatchNumber         =PIn.PInt(table.Rows[i][11].ToString());
				etrans.AckCode             =PIn.PString(table.Rows[i][12].ToString());
				etrans.TransSetNum         =PIn.PInt(table.Rows[i][13].ToString());
				etrans.Note                =PIn.PString(table.Rows[i][14].ToString());
				etrans.EtransMessageTextNum=PIn.PInt(table.Rows[i][15].ToString());
				etrans.AckEtransNum        =PIn.PInt(table.Rows[i][16].ToString());
				etrans.PlanNum             =PIn.PInt(table.Rows[i][17].ToString());
				retVal.Add(etrans);
			}
			return retVal;
		}

		///<summary>DateTimeTrans can be handled automatically here.  No need to set it in advance, but it's allowed to do so.</summary>
		public static int Insert(Etrans etrans) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				etrans.EtransNum=Meth.GetInt(MethodBase.GetCurrentMethod(),etrans);
				return etrans.EtransNum;
			}
			if(PrefC.RandomKeys) {
				etrans.EtransNum=MiscData.GetKey("etrans","EtransNum");
			}
			string command="INSERT INTO etrans (";
			if(PrefC.RandomKeys) {
				command+="EtransNum,";
			}
			command+="DateTimeTrans,ClearinghouseNum,Etype,ClaimNum,OfficeSequenceNumber,CarrierTransCounter,"
				+"CarrierTransCounter2,CarrierNum,CarrierNum2,PatNum,BatchNumber,AckCode,TransSetNum,Note,EtransMessageTextNum,"
				+"AckEtransNum,PlanNum) VALUES(";
			if(PrefC.RandomKeys) {
				command+="'"+POut.PInt(etrans.EtransNum)+"', ";
			}
			if(etrans.DateTimeTrans.Year<1880) {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					command+=POut.PDateT(MiscData.GetNowDateTime());
				}
				else {//Assume MySQL
					command+="NOW()";
				}
			}
			else {
				command+=POut.PDateT(etrans.DateTimeTrans);
			}
			command+=", "
				+"'"+POut.PInt   (etrans.ClearinghouseNum)+"', "
				+"'"+POut.PInt   ((int)etrans.Etype)+"', "
				+"'"+POut.PInt   (etrans.ClaimNum)+"', "
				+"'"+POut.PInt   (etrans.OfficeSequenceNumber)+"', "
				+"'"+POut.PInt   (etrans.CarrierTransCounter)+"', "
				+"'"+POut.PInt   (etrans.CarrierTransCounter2)+"', "
				+"'"+POut.PInt   (etrans.CarrierNum)+"', "
				+"'"+POut.PInt   (etrans.CarrierNum2)+"', "
				+"'"+POut.PInt   (etrans.PatNum)+"', "
				+"'"+POut.PInt   (etrans.BatchNumber)+"', "
				+"'"+POut.PString(etrans.AckCode)+"', "
				+"'"+POut.PInt   (etrans.TransSetNum)+"', "
				+"'"+POut.PString(etrans.Note)+"', "
				+"'"+POut.PInt   (etrans.EtransMessageTextNum)+"', "
				+"'"+POut.PInt   (etrans.AckEtransNum)+"', "
				+"'"+POut.PInt   (etrans.PlanNum)+"')";
			if(PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				etrans.EtransNum=Db.NonQ(command,true);
			}
			return etrans.EtransNum;
		}

		///<summary></summary>
		public static void Update(Etrans etrans) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),etrans);
				return;
			}
			string command= "UPDATE etrans SET "
				+"ClearinghouseNum = '"   +POut.PInt   (etrans.ClearinghouseNum)+"', "
				+"Etype= '"               +POut.PInt   ((int)etrans.Etype)+"', "
				+"ClaimNum= '"            +POut.PInt   (etrans.ClaimNum)+"', "
				+"OfficeSequenceNumber= '"+POut.PInt   (etrans.OfficeSequenceNumber)+"', "
				+"CarrierTransCounter= '" +POut.PInt   (etrans.CarrierTransCounter)+"', "
				+"CarrierTransCounter2= '"+POut.PInt   (etrans.CarrierTransCounter2)+"', "
				+"CarrierNum= '"          +POut.PInt   (etrans.CarrierNum)+"', "
				+"CarrierNum2= '"         +POut.PInt   (etrans.CarrierNum2)+"', "
				+"PatNum= '"              +POut.PInt   (etrans.PatNum)+"', "
				+"BatchNumber= '"         +POut.PInt   (etrans.BatchNumber)+"', "
				+"AckCode= '"             +POut.PString(etrans.AckCode)+"', "
				+"TransSetNum= '"         +POut.PInt   (etrans.TransSetNum)+"', "
				+"Note= '"                +POut.PString(etrans.Note)+"', "
				+"EtransMessageTextNum= '"+POut.PInt   (etrans.EtransMessageTextNum)+"', "
				+"AckEtransNum= '"         +POut.PInt   (etrans.AckEtransNum)+"', "
				+"PlanNum= '"             +POut.PInt   (etrans.PlanNum)+"' "
				+"WHERE EtransNum = "+POut.PInt(etrans.EtransNum);
			Db.NonQ(command);
		}

		///<summary>Not for claim types, just other types, including Eligibility. This function gets run first.  Then, the messagetext is created and an attempt is made to send the message.  Finally, the messagetext is added to the etrans.  This is necessary because the transaction numbers must be incremented and assigned to each message before creating the message and attempting to send.  If it fails, we will need to roll back.  Provide EITHER a carrierNum OR a canadianNetworkNum.  Many transactions can be sent to a carrier or to a network.</summary>
		public static Etrans CreateCanadianOutput(int patNum, int carrierNum, int canadianNetworkNum, int clearinghouseNum, EtransType etype){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Etrans>(MethodBase.GetCurrentMethod(),patNum,carrierNum,canadianNetworkNum,clearinghouseNum,etype);
			}
			//validation of carrier vs network
			if(etype==EtransType.Eligibility_CA){
				//only carrierNum is allowed (and required)
				if(carrierNum==0){
					throw new ApplicationException("Carrier not supplied for Etranss.CreateCanadianOutput.");
				}
				if(canadianNetworkNum!=0){
					throw new ApplicationException("NetworkNum not allowed for Etranss.CreateCanadianOutput.");
				}
			}
			Etrans etrans=new Etrans();
			//etrans.DateTimeTrans handled automatically
			etrans.ClearinghouseNum=clearinghouseNum;
			etrans.Etype=etype;
			etrans.ClaimNum=0;//no claim involved
			etrans.PatNum=patNum;
			//CanadianNetworkNum?
			etrans.CarrierNum=carrierNum;
			//InsPlanNum? (for eligibility)
			//Get next OfficeSequenceNumber-----------------------------------------------------------------------------------------
			etrans.OfficeSequenceNumber=0;
			string command="SELECT MAX(OfficeSequenceNumber) FROM etrans";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count>0) {
				etrans.OfficeSequenceNumber=PIn.PInt(table.Rows[0][0].ToString());
				if(etrans.OfficeSequenceNumber==999999){//if the office has sent > 1 million messages, and has looped back around to 1.
					//get the date of the most recent max
					//This works, but it got even more complex for CarrierTransCounter, so we will just throw an exception for now.
					/*command="SELECT MAX(DateTimeTrans) FROM etrans WHERE OfficeSequenceNumber=999999";
					table=Db.GetTable(command);
					DateTime maxDateT=PIn.PDateT(table.Rows[0][0].ToString());
					//then, just get the max that's newer than that.
					command="SELECT MAX(OfficeSequenceNumber) FROM etrans WHERE DateTimeTrans > '"+POut.PDateT(maxDateT)+"'";
					table=Db.GetTable(command);
					if(table.Rows.Count>0) {
						etrans.OfficeSequenceNumber=PIn.PInt(table.Rows[0][0].ToString());
					}*/
					throw new ApplicationException("OfficeSequenceNumber has maxed out at 999999.  This program will need to be enhanced.");
				}
			}			
			etrans.OfficeSequenceNumber++;
			if(etype==EtransType.Eligibility_CA){
				//find the next CarrierTransCounter------------------------------------------------------------------------------------
				etrans.CarrierTransCounter=0;
				command="SELECT MAX(CarrierTransCounter) FROM etrans"
					+"WHERE CarrierNum="+POut.PInt(etrans.CarrierNum);
				table=Db.GetTable(command);
				int tempcounter=0;
				if(table.Rows.Count>0) {
					tempcounter=PIn.PInt(table.Rows[0][0].ToString());
				}
				if(tempcounter>etrans.CarrierTransCounter) {
					etrans.CarrierTransCounter=tempcounter;
				}
				command="SELECT MAX(CarrierTransCounter2) FROM etrans "
					+"WHERE CarrierNum2="+POut.PInt(etrans.CarrierNum);
				table=Db.GetTable(command);
				if(table.Rows.Count>0) {
					tempcounter=PIn.PInt(table.Rows[0][0].ToString());
				}
				if(tempcounter>etrans.CarrierTransCounter) {
					etrans.CarrierTransCounter=tempcounter;
				}
				if(etrans.CarrierTransCounter==99999){
					throw new ApplicationException("CarrierTransCounter has maxed out at 99999.  This program will need to be enhanced.");
					//maybe by adding a reset date to the preference table which will apply to all counters as a whole.
				}
				etrans.CarrierTransCounter++;
			}
			Insert(etrans);
			return etrans;
		}

		///<summary>Only used by Canadian code right now.</summary>
		public static void SetMessage(int etransNum, string messageText) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),etransNum,messageText);
				return;
			}
			EtransMessageText msg=new EtransMessageText();
			msg.MessageText=messageText;
			EtransMessageTexts.Insert(msg);
			//string command=
			string command= "UPDATE etrans SET EtransMessageTextNum="+POut.PInt(msg.EtransMessageTextNum)+" "
				+"WHERE EtransNum = '"+POut.PInt(etransNum)+"'";
			Db.NonQ(command);
		}

		///<summary>Deletes the etrans entry and changes the status of the claim back to W.  If it encounters an entry that's not a claim, it skips it for now.  Later, it will handle all types of undo.  It will also check Canadian claims to prevent alteration if an ack or EOB has been received.</summary>
		public static void Undo(int etransNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),etransNum);
				return;
			}
			//see if it's a claim.
			string command="SELECT ClaimNum FROM etrans WHERE EtransNum="+POut.PInt(etransNum);
			DataTable table=Db.GetTable(command);
			int claimNum=PIn.PInt(table.Rows[0][0].ToString());
			if(claimNum==0){//if no claim
				return;//for now
			}
			//future Canadian check will go here

			//Change the claim back to W.
			command="UPDATE claim SET ClaimStatus='W' WHERE ClaimNum="+POut.PInt(claimNum);
			Db.NonQ(command);
			//Delete this etrans
			command="DELETE FROM etrans WHERE EtransNum="+POut.PInt(etransNum);
			Db.NonQ(command);
		}

		///<summary>Deletes the etrans entry.  Mostly used when the etrans entry was created, but then the communication with the clearinghouse failed.  So this is just a rollback function.  Will not delete the message associated with the etrans.  That must be done separately.  Will throw exception if the etrans does not exist.</summary>
		public static void Delete(int etransNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),etransNum);
				return;
			}
			//see if there's a message
			string command;//="SELECT EtransMessageTextNum FROM etrans WHERE EtransNum="+POut.PInt(etransNum);
			//DataTable table=Db.GetTable(command);
			//if(table.Rows[0][0].ToString()!="0"){//this throws exception if 0 rows.
			//	throw new ApplicationException("Error. Etrans must not have messagetext attached yet.");
			//}
			command="DELETE FROM etrans WHERE EtransNum="+POut.PInt(etransNum);
			Db.NonQ(command);
		}

		///<summary>Sets the status of the claim to sent.  Also makes an entry in etrans.  If this is canadian eclaims, then this function gets run first.  Then, the messagetext is created and an attempt is made to send the claim.  Finally, the messagetext and added to the etrans.  This is necessary because the transaction numbers must be incremented and assigned to each claim before creating the message and attempting to send.  If it fails, Canadians will need to delete the etrans entries (or we will need to roll back the changes).</summary>
		public static Etrans SetClaimSentOrPrinted(int claimNum,int patNum,int clearinghouseNum,EtransType etype,
			string messageText,int batchNumber) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Etrans>(MethodBase.GetCurrentMethod(),claimNum,patNum,clearinghouseNum,etype,messageText,batchNumber);
			}
			string command="UPDATE claim SET ClaimStatus = 'S',"
				+"DateSent= "+POut.PDate(MiscData.GetNowDateTime())
				+" WHERE claimnum = "+POut.PInt(claimNum);
			Db.NonQ(command);
			Etrans etrans=new Etrans();
			//etrans.DateTimeTrans handled automatically
			etrans.ClearinghouseNum=clearinghouseNum;
			etrans.Etype=etype;
			etrans.ClaimNum=claimNum;
			etrans.PatNum=patNum;
			//Get the primary and secondary carrierNums for this claim.
			command="SELECT carrier1.CarrierNum,carrier2.CarrierNum AS CarrierNum2 FROM claim "
				+"LEFT JOIN insplan insplan1 ON insplan1.PlanNum=claim.PlanNum "
				+"LEFT JOIN carrier carrier1 ON carrier1.CarrierNum=insplan1.CarrierNum "
				+"LEFT JOIN insplan insplan2 ON insplan2.PlanNum=claim.PlanNum2 "
				+"LEFT JOIN carrier carrier2 ON carrier2.CarrierNum=insplan2.CarrierNum "
				+"WHERE claim.ClaimNum="+POut.PInt(claimNum);
			DataTable table=Db.GetTable(command);
			etrans.CarrierNum=PIn.PInt(table.Rows[0][0].ToString());
			etrans.CarrierNum2=PIn.PInt(table.Rows[0][1].ToString());//might be 0 if no secondary on this claim
			etrans.BatchNumber=batchNumber;
			if(X837.IsX12(messageText)) {
				X837 x837=new X837(messageText);
				etrans.TransSetNum=x837.GetTransNum(claimNum);
			}
			if(etype==EtransType.Claim_CA) {
				etrans.OfficeSequenceNumber=0;
				//find the next officeSequenceNumber
				command="SELECT MAX(OfficeSequenceNumber) FROM etrans";
				table=Db.GetTable(command);
				if(table.Rows.Count>0) {
					etrans.OfficeSequenceNumber=PIn.PInt(table.Rows[0][0].ToString());
					if(etrans.OfficeSequenceNumber==999999) {//if the office has sent > 1 million messages, and has looped back around to 1.
						throw new ApplicationException
							("OfficeSequenceNumber has maxed out at 999999.  This program will need to be enhanced.");
					}
				}
				etrans.OfficeSequenceNumber++;
				//find the next CarrierTransCounter for the primary carrier
				etrans.CarrierTransCounter=0;
				command="SELECT MAX(CarrierTransCounter) FROM etrans "
					+"WHERE CarrierNum="+POut.PInt(etrans.CarrierNum);
				table=Db.GetTable(command);
				int tempcounter=0;
				if(table.Rows.Count>0) {
					tempcounter=PIn.PInt(table.Rows[0][0].ToString());
				}
				if(tempcounter>etrans.CarrierTransCounter) {
					etrans.CarrierTransCounter=tempcounter;
				}
				command="SELECT MAX(CarrierTransCounter2) FROM etrans "
					+"WHERE CarrierNum2="+POut.PInt(etrans.CarrierNum);
				table=Db.GetTable(command);
				if(table.Rows.Count>0) {
					tempcounter=PIn.PInt(table.Rows[0][0].ToString());
				}
				if(tempcounter>etrans.CarrierTransCounter) {
					etrans.CarrierTransCounter=tempcounter;
				}
				if(etrans.CarrierTransCounter==99999) {
					throw new ApplicationException("CarrierTransCounter has maxed out at 99999.  This program will need to be enhanced.");
				}
				etrans.CarrierTransCounter++;
				if(etrans.CarrierNum2>0) {//if there is secondary coverage on this claim
					etrans.CarrierTransCounter2=1;
					command="SELECT MAX(CarrierTransCounter) FROM etrans "
						+"WHERE CarrierNum="+POut.PInt(etrans.CarrierNum2);
					table=Db.GetTable(command);
					if(table.Rows.Count>0) {
						tempcounter=PIn.PInt(table.Rows[0][0].ToString());
					}
					if(tempcounter>etrans.CarrierTransCounter2) {
						etrans.CarrierTransCounter2=tempcounter;
					}
					command="SELECT MAX(CarrierTransCounter2) FROM etrans "
						+"WHERE CarrierNum2="+POut.PInt(etrans.CarrierNum2);
					table=Db.GetTable(command);
					if(table.Rows.Count>0) {
						tempcounter=PIn.PInt(table.Rows[0][0].ToString());
					}
					if(tempcounter>etrans.CarrierTransCounter2) {
						etrans.CarrierTransCounter2=tempcounter;
					}
					if(etrans.CarrierTransCounter2==99999) {
						throw new ApplicationException("CarrierTransCounter has maxed out at 99999.  This program will need to be enhanced.");
					}
					etrans.CarrierTransCounter2++;
				}
			}
			EtransMessageText etransMessageText=new EtransMessageText();
			etransMessageText.MessageText=messageText;
			EtransMessageTexts.Insert(etransMessageText);
			etrans.EtransMessageTextNum=etransMessageText.EtransMessageTextNum;
			Etranss.Insert(etrans);
			return etrans;
		}

		///<summary>Etrans type will be figured out by this class.  Either TextReport, Acknowledge_997, or StatusNotify_277.</summary>
		public static void ProcessIncomingReport(DateTime dateTimeTrans,int clearinghouseNum,string messageText) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),dateTimeTrans,clearinghouseNum,messageText);
				return;
			}
			Etrans etrans=new Etrans();
			etrans.DateTimeTrans=dateTimeTrans;
			etrans.ClearinghouseNum=clearinghouseNum;
			EtransMessageText etransMessageText=new EtransMessageText();
			etransMessageText.MessageText=messageText;
			EtransMessageTexts.Insert(etransMessageText);
			etrans.EtransMessageTextNum=etransMessageText.EtransMessageTextNum;
			string command;
			if(X12object.IsX12(messageText)) {
				X12object Xobj=new X12object(messageText);
				if(Xobj.Is997()) {
					X997 x997=new X997(messageText);
					etrans.Etype=EtransType.Acknowledge_997;
					etrans.BatchNumber=x997.GetBatchNumber();
					Etranss.Insert(etrans);
					string batchack=x997.GetBatchAckCode();
					if(batchack=="A"||batchack=="R") {//accepted or rejected
						command="UPDATE etrans SET AckCode='"+batchack+"', "
							+"AckEtransNum="+POut.PInt(etrans.EtransNum)
							+" WHERE BatchNumber="+POut.PInt(etrans.BatchNumber)
							+" AND ClearinghouseNum="+POut.PInt(clearinghouseNum)
							+" AND DateTimeTrans > "+POut.PDateT(dateTimeTrans.AddDays(-14))
							+" AND DateTimeTrans < "+POut.PDateT(dateTimeTrans.AddDays(1))
							+" AND AckEtransNum=0";
						Db.NonQ(command);
					}
					else {//partially accepted
						List<int> transNums=x997.GetTransNums();
						string ack;
						for(int i=0;i<transNums.Count;i++) {
							ack=x997.GetAckForTrans(transNums[i]);
							if(ack=="A"||ack=="R") {//accepted or rejected
								command="UPDATE etrans SET AckCode='"+ack+"' "
									+"AckEtransNum="+POut.PInt(etrans.EtransNum)
									+"WHERE BatchNumber="+POut.PInt(etrans.BatchNumber)
									+" AND TransSetNum="+POut.PInt(transNums[i])
									+" AND ClearinghouseNum="+POut.PInt(clearinghouseNum)
									+" AND DateTimeTrans > "+POut.PDateT(dateTimeTrans.AddDays(-14))
									+" AND DateTimeTrans < "+POut.PDateT(dateTimeTrans.AddDays(1))
									+" AND AckEtransNum=0";
								Db.NonQ(command);
							}
						}
					}
					//none of the other fields make sense, because this ack could refer to many claims.
				}
				else if(X277U.Is277U(Xobj)) {
					etrans.Etype=EtransType.StatusNotify_277;
					//later: analyze to figure out which e-claim is being referenced.
					Etranss.Insert(etrans);
				}
				else {//unknown type of X12 report.
					etrans.Etype=EtransType.TextReport;
					Etranss.Insert(etrans);
				}
			}
			else {//not X12
				etrans.Etype=EtransType.TextReport;
				Etranss.Insert(etrans);
			}
		}

		public static DateTime GetLastDate270(int planNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<DateTime>(MethodBase.GetCurrentMethod(),planNum);
			}
			string command="SELECT MAX(DateTimeTrans) FROM etrans "
				+"WHERE Etype="+POut.PInt((int)EtransType.BenefitInquiry270)
				+" AND PlanNum="+POut.PInt(planNum);
			return PIn.PDate(Db.GetScalar(command));
		}



		
	}
}