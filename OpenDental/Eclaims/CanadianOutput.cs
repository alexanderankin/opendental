using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental.Eclaims {
	public class CanadianOutput {
		///<summary>The result is the etransNum of the response message.  Or it might throw an exception if invalid data.  This class is also responsible for saving the returned message to the etrans table, and for printing out the required form.</summary>
		public static long SendElegibility(long patNum,InsPlan plan,DateTime date,Relat relat,string patID,bool doPrint,InsSub insSub){
			//string electID,long patNum,string groupNumber,string divisionNo,
			//string subscriberID,string patID,Relat patRelat,long subscNum,string dentaideCardSequence)
			//Note: This might be the only class of this kind that returns a string.  It's a special situation.
			//We are simply not going to bother with language translation here.
			Carrier carrier=Carriers.GetCarrier(plan.CarrierNum);
			if(carrier==null) {
				throw new ApplicationException("Invalid carrier.");
			}
			if((carrier.CanadianSupportedTypes&CanSupTransTypes.EligibilityTransaction_08)!=CanSupTransTypes.EligibilityTransaction_08) {
				throw new ApplicationException("The carrier does not support eligibility transactions.");
			}
			if(carrier.CanadianNetworkNum==0) {
				throw new ApplicationException("Carrier network not set.");
			}
			CanadianNetwork network=CanadianNetworks.GetNetwork(carrier.CanadianNetworkNum);
			Patient patient=Patients.GetPat(patNum);
			Patient subscriber=Patients.GetPat(insSub.Subscriber);
			Provider provDefaultTreat=Providers.GetProv(PrefC.GetLong(PrefName.PracticeDefaultProv));
			Clearinghouse clearhouse=Canadian.GetClearinghouse();
			if(clearhouse==null){
				throw new ApplicationException("Canadian clearinghouse not found.");
			}
			string saveFolder=clearhouse.ExportPath;
			if(!Directory.Exists(saveFolder)) {
				throw new ApplicationException(saveFolder+" not found.");
			}
			//validate----------------------------------------------------------------------------------------------------
			string error="";
			//if(carrier.CanadianNetworkNum==0){
			//	if(error!="") error+=", ";
			//	error+="Carrier does not have network specified";
			//}
			if(!Regex.IsMatch(carrier.ElectID,@"^[0-9]{6}$")){//not necessary, but nice
				if(error!="") error+=", ";
				error+="CarrierId 6 digits";
			}
			if(!provDefaultTreat.IsCDAnet) {
				error+="Prov not setup as CDA provider";
			}
			if(provDefaultTreat.NationalProvID.Length!=9) {
				if(error!="")	error+=", ";
				error+="Prov CDA num 9 digits";
			}
			if(provDefaultTreat.CanadianOfficeNum.Length!=4) {
				if(error!="") error+=", ";
				error+="Prov office num 4 char";
			}
			//if(plan.GroupNum.Length==0 || groupNumber.Length>12 || groupNumber.Contains(" ")){
			//	if(error!="") error+=", ";
			//	error+="Plan Number";
			//}
			//if(subscriberID==""){//already validated.  And it's allowed to be blank sometimes
			//	if(error!="") error+=", ";
			//	error+="SubscriberID";
			//}
			if(patNum != insSub.Subscriber && relat==Relat.Self) {//if patient is not subscriber, and relat is self
				if(error!="") error+=", ";
				error+="Relationship cannot be self";
			}
			if(patient.Gender==PatientGender.Unknown){
				if(error!="") error+=", ";
				error+="Patient gender";
			}
			if(patient.Birthdate.Year<1880 || patient.Birthdate>DateTime.Today) {
				if(error!="") error+=", ";
				error+="Patient birthdate";
			}
			if(patient.LName=="") {
				if(error!="") error+=", ";
				error+="Patient lastname";
			}
			if(patient.FName=="") {
				if(error!="") error+=", ";
				error+="Patient firstname";
			}
			if(patient.CanadianEligibilityCode==0) {
				if(error!="") error+=", ";
				error+="Patient eligibility exception code";
			}
			if(subscriber.Birthdate.Year<1880 || subscriber.Birthdate>DateTime.Today) {
				if(error!="") error+=", ";
				error+="Subscriber birthdate";
			}
			if(subscriber.LName=="") {
				if(error!="") error+=", ";
				error+="Subscriber lastname";
			}
			if(subscriber.FName=="") {
				if(error!="") error+=", ";
				error+="Subscriber firstname";
			}
			if(error!="") {
				throw new ApplicationException(error);
			}
			Etrans etrans=Etranss.CreateCanadianOutput(patNum,carrier.CarrierNum,0,
				clearhouse.ClearinghouseNum,EtransType.Eligibility_CA,plan.PlanNum,insSub.InsSubNum);
			StringBuilder strb=new StringBuilder();
			//create message----------------------------------------------------------------------------------------------
			//A01 transaction prefix 12 AN
			strb.Append(Canadian.TidyAN(network.CanadianTransactionPrefix,12));
			//A02 office sequence number 6 N
			strb.Append(Canadian.TidyN(etrans.OfficeSequenceNumber,6));
			//A03 format version number 2 N
			strb.Append(carrier.CDAnetVersion);//eg. "04", validated in UI
			//A04 transaction code 2 N
			if(carrier.CDAnetVersion=="02"){
				strb.Append("00");//eligibility
			}
			else{
				strb.Append("08");//eligibility
			}
			//A05 carrier id number 6 N
			strb.Append(carrier.ElectID);//already validated as 6 digit number.
			//A06 software system id 3 AN
			strb.Append(Canadian.SoftwareSystemId());
			if(carrier.CDAnetVersion=="04") {
				//A10 encryption method 1 N
				strb.Append(carrier.CanadianEncryptionMethod);//validated in UI
			}
			//A07 message length 5 N
			int len;
			bool C19PlanRecordPresent=false;
			if(carrier.CDAnetVersion=="02"){
				len=178;
				strb.Append(Canadian.TidyN(len,4));
			}
			else{
				len=214;
				if(plan.CanadianPlanFlag=="A"){// || plan.CanadianPlanFlag=="N"){
					C19PlanRecordPresent=true;
				}
				if(C19PlanRecordPresent){
					len+=30;
				}
				strb.Append(Canadian.TidyN(len,5));
				//A09 carrier transaction counter 5 N, only version 04
				strb.Append(Canadian.TidyN(etrans.CarrierTransCounter,5));
			}
			//B01 CDA provider number 9 AN
			strb.Append(Canadian.TidyAN(provDefaultTreat.NationalProvID,9));//already validated
			//B02 provider office number 4 AN
			strb.Append(Canadian.TidyAN(provDefaultTreat.CanadianOfficeNum,4));//already validated
			if(carrier.CDAnetVersion=="04"){
				//B03 billing provider number 9 AN
				Provider provBilling=Providers.GetProv(Providers.GetBillingProvNum(provDefaultTreat.ProvNum,patient.ClinicNum));
				strb.Append(Canadian.TidyAN(provBilling.NationalProvID,9));//already validated
			}
			if(carrier.CDAnetVersion=="02") {
				//C01 primary policy/plan number 8 AN (group number)
				//No special validation for version 02
				strb.Append(Canadian.TidyAN(plan.GroupNum,8));
			}
			else {
				//C01 primary policy/plan number 12 AN (group number)
				//only validated to ensure that it's not blank and is less than 12. Also that no spaces.
				strb.Append(Canadian.TidyAN(plan.GroupNum,12));
			}
			//C11 primary division/section number 10 AN
			strb.Append(Canadian.TidyAN(plan.DivisionNo,10));
			if(carrier.CDAnetVersion=="02") {
				//C02 subscriber id number 11 AN
				strb.Append(Canadian.TidyAN(insSub.SubscriberID.Replace("-",""),11));//no extra validation for version 02
			}
			else{
				//C02 subscriber id number 12 AN
				strb.Append(Canadian.TidyAN(insSub.SubscriberID.Replace("-",""),12));//validated
			}
			if(carrier.CDAnetVersion=="04") {
				//C17 primary dependant code 2 N. Optional
				strb.Append(Canadian.TidyN(patID,2));
			}
			//C03 relationship code 1 N
			//User interface does not only show Canadian options, but all options are handled.
			strb.Append(Canadian.GetRelationshipCode(relat));
			//C04 patient's sex 1 A
			//validated to not include "unknown"
			if(patient.Gender==PatientGender.Male) {
				strb.Append("M");
			}
			else {
				strb.Append("F");
			}
			//C05 patient birthday 8 N
			strb.Append(patient.Birthdate.ToString("yyyyMMdd"));//validated
			//C06 patient last name 25 AE
			strb.Append(Canadian.TidyAE(patient.LName,25,true));//validated
			//C07 patient first name 15 AE
			strb.Append(Canadian.TidyAE(patient.FName,15,true));//validated
			//C08 patient middle initial 1 AE
			strb.Append(Canadian.TidyAE(patient.MiddleI,1));
			//C09 eligibility exception code 1 N
			strb.Append(Canadian.GetEligibilityCode(patient.CanadianEligibilityCode,carrier.CDAnetVersion=="02"));//validated
			if(carrier.CDAnetVersion=="04") {
				//C12 plan flag 1 A
				strb.Append(Canadian.GetPlanFlag(plan.CanadianPlanFlag));
				//C18 plan record count 1 N
				if(C19PlanRecordPresent) {
					strb.Append("1");
				}
				else {
					strb.Append("0");
				}
				//C16 Eligibility date. 8 N.
				strb.Append(date.ToString("yyyyMMdd"));
			}
			//D01 subscriber birthday 8 N
			strb.Append(subscriber.Birthdate.ToString("yyyyMMdd"));//validated
			//D02 subscriber last name 25 AE
			strb.Append(Canadian.TidyAE(subscriber.LName,25,true));//validated
			//D03 subscriber first name 15 AE
			strb.Append(Canadian.TidyAE(subscriber.FName,15,true));//validated
			//D04 subscriber middle initial 1 AE
			strb.Append(Canadian.TidyAE(subscriber.MiddleI,1));
			if(carrier.CDAnetVersion=="04") {
				//D10 language of insured 1 A
				if(subscriber.Language=="fr") {
					strb.Append("F");
				}
				else {
					strb.Append("E");
				}
				//D11 card sequence/version number 2 N
				//Not validated against type of carrier.  Might need to check if Dentaide.
				strb.Append(Canadian.TidyN(plan.DentaideCardSequence,2));
				//C19 plan record 30 AN
				if(C19PlanRecordPresent) {
					//todo: what text goes here?  Not documented
					strb.Append(Canadian.TidyAN("",30));
				}
			}
			string result="";
			bool resultIsError=false;
			try {
				result=Canadian.PassToIca(strb.ToString(),clearhouse);
			}
			catch(ApplicationException ex) {
				result=ex.Message;
				resultIsError=true;
				//Etranss.Delete(etrans.EtransNum);//we don't want to do this, because we want the incremented etrans.OfficeSequenceNumber to be saved
				//Attach an ack indicating failure.
			}
			//Attach an ack to the etrans
			Etrans etransAck=new Etrans();
			etransAck.PatNum=etrans.PatNum;
			etransAck.PlanNum=etrans.PlanNum;
			etransAck.InsSubNum=etrans.InsSubNum;
			etransAck.CarrierNum=etrans.CarrierNum;
			etransAck.DateTimeTrans=DateTime.Now;
			CCDFieldInputter fieldInputter=null;
			if(resultIsError){
				etransAck.Etype=EtransType.AckError;
				etrans.Note="failed";
			}
			else{
				fieldInputter=new CCDFieldInputter(result);
				CCDField fieldG05=fieldInputter.GetFieldById("G05");
				if(fieldG05!=null) {
					etransAck.AckCode=fieldG05.valuestr;
				}
				etransAck.Etype=fieldInputter.GetEtransType();
			}
			Etranss.Insert(etransAck);
			Etranss.SetMessage(etransAck.EtransNum,result);
			etrans.AckEtransNum=etransAck.EtransNum;
			Etranss.Update(etrans);
			Etranss.SetMessage(etrans.EtransNum,strb.ToString());
			if(resultIsError){
				throw new ApplicationException(result);
			}
			if(doPrint) {
				new FormCCDPrint(etrans,result,true);//Print the form.
			}
			//Now we will process the 'result' here to extract the important data.  Basically Yes or No on the eligibility.
			//We might not do this for any other trans type besides eligibility.
			string strResponse="";//"Eligibility check on "+DateTime.Today.ToShortDateString()+"\r\n";
			//CCDField field=fieldInputter.GetFieldById("G05");//response status
			string valuestr=fieldInputter.GetValue("G05");//response status
			switch(valuestr){
				case "E":
					strResponse+="Patient is eligible.";
					break;
				case "R":
					strResponse+="Patient not eligible, or error in data.";
					break;
				case "M":
					strResponse+="Manual claimform should be submitted for employer certified plan.";
					break;
			}
			etrans=Etranss.GetEtrans(etrans.EtransNum);
			etrans.Note=strResponse;
			Etranss.Update(etrans);
			return etransAck.EtransNum;
			/*
			CCDField[] fields=fieldInputter.GetFieldsById("G08");//Error Codes
			for(int i=0;i<fields.Length;i++){
				retVal+="\r\n";
				retVal+=fields[i].valuestr;//todo: need to turn this into a readable string.
			}
			fields=fieldInputter.GetFieldsById("G32");//Display messages
			for(int i=0;i<fields.Length;i++) {
				retVal+="\r\n";
				retVal+=fields[i].valuestr;
			}
			return retVal;*/
		}

		///<summary></summary>
		public static long SendClaimReversal(Claim claim,InsPlan plan,InsSub insSub) {
			StringBuilder strb=new StringBuilder();
			Clearinghouse clearhouse=Canadian.GetClearinghouse();
			if(clearhouse==null) {
				throw new ApplicationException(Lan.g("CanadianOutput","Canadian clearinghouse not found."));
			}
			string saveFolder=clearhouse.ExportPath;
			if(!Directory.Exists(saveFolder)) {
				throw new ApplicationException(saveFolder+" not found.");
			}
			Carrier carrier=Carriers.GetCarrier(plan.CarrierNum);
			if((carrier.CanadianSupportedTypes&CanSupTransTypes.ClaimReversal_02)!=CanSupTransTypes.ClaimReversal_02) {
				throw new ApplicationException(Lan.g("CanadianOutput","The carrier does not support reversal transactions."));
			}
			if(carrier.CanadianNetworkNum==0) {
				throw new ApplicationException("Carrier network not set.");
			}
			CanadianNetwork network=CanadianNetworks.GetNetwork(carrier.CanadianNetworkNum);
			Etrans etrans=Etranss.CreateCanadianOutput(claim.PatNum,carrier.CarrierNum,carrier.CanadianNetworkNum,
				clearhouse.ClearinghouseNum,EtransType.ReverseResponse_CA,plan.PlanNum,insSub.InsSubNum);
			Patient patient=Patients.GetPat(claim.PatNum);
			Provider prov=Providers.GetProv(claim.ProvTreat);
			if(!prov.IsCDAnet) {
				throw new ApplicationException(Lan.g("CanadianOutput","Treating provider is not setup to use CDANet."));
			}
			Provider billProv=ProviderC.ListLong[Providers.GetIndexLong(claim.ProvBill)];
			if(!billProv.IsCDAnet) {
				throw new ApplicationException(Lan.g("CanadianOutput","Billing provider is not setup to use CDANet."));
			}
			InsPlan insPlan=InsPlans.GetPlan(claim.PlanNum,new List<InsPlan>());
			Patient subscriber=Patients.GetPat(insSub.Subscriber);
			//create message----------------------------------------------------------------------------------------------
			//A01 transaction prefix 12 AN
			strb.Append(Canadian.TidyAN(network.CanadianTransactionPrefix,12));
			//A02 office sequence number 6 N
			//We are required to use the same office sequence number as the original claim.
			etrans.OfficeSequenceNumber=0;//Clear the randomly generated office sequence number.
			List<Etrans> claimTransactions=Etranss.GetAllForOneClaim(claim.ClaimNum);
			DateTime originalEtransDateTime=DateTime.MinValue;//So we can get the latest matching transaction.
			for(int i=0;i<claimTransactions.Count;i++) {
				if(claimTransactions[i].Etype==EtransType.Claim_CA || claimTransactions[i].Etype==EtransType.ClaimCOB_CA) {
					Etrans ack=Etranss.GetEtrans(claimTransactions[i].AckEtransNum);
					if(ack==null) {//For those claims sent that didn't receive a response (i.e. when there is an exception while sending a claim).
						continue;
					}
					string messageText=EtransMessageTexts.GetMessageText(ack.EtransMessageTextNum);
					CCDFieldInputter messageData=new CCDFieldInputter(messageText);
					CCDField transRefNum=messageData.GetFieldById("G01");
					if(transRefNum!=null && transRefNum.valuestr==claim.CanadaTransRefNum && claimTransactions[i].DateTimeTrans>originalEtransDateTime) {
						etrans.OfficeSequenceNumber=PIn.Int(messageData.GetFieldById("A02").valuestr);
						originalEtransDateTime=claimTransactions[i].DateTimeTrans;
					}
				}
			}
			DateTime serverDate=MiscData.GetNowDateTime().Date;
			if(originalEtransDateTime.Date!=serverDate) {
				throw new ApplicationException(Lan.g("CanadianOutput","Claims can only be reversed on the day that they were sent. The claim can only be manually reversed."));
			}
			strb.Append(Canadian.TidyN(etrans.OfficeSequenceNumber,6));
			//A03 format version number 2 N
			strb.Append(carrier.CDAnetVersion);//eg. "04", validated in UI
			//A04 transaction code 2 N
			strb.Append("02");//Same for both versions 02 and 04.
			//A05 carrier id number 6 N
			strb.Append(carrier.ElectID);//already validated as 6 digit number.
			//A06 software system id 3 AN
			strb.Append(Canadian.SoftwareSystemId());
			if(carrier.CDAnetVersion!="02") { //version 04
				//A10 encryption method 1 N
				strb.Append(carrier.CanadianEncryptionMethod);//validated in UI
			}
			if(carrier.CDAnetVersion=="02") {
				//A07 message length N4
				strb.Append(Canadian.TidyN("133",4));
			}
			else { //version 04
				//A07 message length N 5
				strb.Append(Canadian.TidyN("164",5));
			}
			if(carrier.CDAnetVersion!="02") { //version 04
				//A09 carrier transaction counter 5 N
#if DEBUG
				strb.Append("11111");
#else				
				strb.Append(Canadian.TidyN(etrans.CarrierTransCounter,5));
#endif
			}
			//B01 CDA provider number 9 AN
			strb.Append(Canadian.TidyAN(prov.NationalProvID,9));//already validated
			//B02 provider office number 4 AN
			strb.Append(Canadian.TidyAN(prov.CanadianOfficeNum,4));//already validated
			if(carrier.CDAnetVersion!="02") { //version 04
				//B03 billing provider number 9 AN
				//might need to account for possible 5 digit prov id assigned by carrier
				strb.Append(Canadian.TidyAN(billProv.NationalProvID,9));//already validated
				//B04 billing provider office number 4 AN
				strb.Append(Canadian.TidyAN(billProv.CanadianOfficeNum,4));//already validated
			}
			if(carrier.CDAnetVersion=="02") {
				//C01 primary policy/plan number 8 AN
				//only validated to ensure that it's not blank and is less than 8. Also that no spaces.
				strb.Append(Canadian.TidyAN(insPlan.GroupNum,8));
			}
			else { //version 04
				//C01 primary policy/plan number 12 AN
				//only validated to ensure that it's not blank and is less than 12. Also that no spaces.
				strb.Append(Canadian.TidyAN(insPlan.GroupNum,12));
			}
			//C11 primary division/section number 10 AN
			strb.Append(Canadian.TidyAN(insPlan.DivisionNo,10));
			if(carrier.CDAnetVersion=="02") {
				//C02 subscriber id number 11 AN
				strb.Append(Canadian.TidyAN(insSub.SubscriberID.Replace("-",""),11));//validated
			}
			else { //version 04
				//C02 subscriber id number 12 AN
				strb.Append(Canadian.TidyAN(insSub.SubscriberID.Replace("-",""),12));//validated
			}
			//C03 relationship code 1 N
			//User interface does not only show Canadian options, but all options are handled.
			strb.Append(Canadian.GetRelationshipCode(claim.PatRelat));
			if(carrier.CDAnetVersion=="02") {
				//D02 subscriber last name 25 A
				strb.Append(Canadian.TidyA(subscriber.LName,25));//validated
			}
			else { //version 04
				//D02 subscriber last name 25 AE
				strb.Append(Canadian.TidyAE(subscriber.LName,25,true));//validated
			}
			if(carrier.CDAnetVersion=="02") {
				//D03 subscriber first name 15 A
				strb.Append(Canadian.TidyA(subscriber.FName,15));//validated
			}
			else { //version 04
				//D03 subscriber first name 15 AE
				strb.Append(Canadian.TidyAE(subscriber.FName,15,true));//validated
			}
			if(carrier.CDAnetVersion=="02") {
				//D04 subscriber middle initial 1 A
				strb.Append(Canadian.TidyA(subscriber.MiddleI,1));
			}
			else { //version 04
				//D04 subscriber middle initial 1 AE
				strb.Append(Canadian.TidyAE(subscriber.MiddleI,1));
			}
			if(carrier.CDAnetVersion!="02") { //version 04
				//For Future Use
				strb.Append("000000");
			}
			//G01 transaction reference number of original claim AN 14
			strb.Append(Canadian.TidyAN(claim.CanadaTransRefNum,14));
			string result="";
			bool resultIsError=false;
			try {
			  result=Canadian.PassToIca(strb.ToString(),clearhouse);
			}
			catch(ApplicationException ex) {
			  result=ex.Message;
			  resultIsError=true;
			  //Etranss.Delete(etrans.EtransNum);//we don't want to do this, because we want the incremented etrans.OfficeSequenceNumber to be saved
			  //Attach an ack indicating failure.
			}
			//Attach an ack to the etrans
			Etrans etransAck=new Etrans();
			etransAck.PatNum=etrans.PatNum;
			etransAck.PlanNum=etrans.PlanNum;
			etransAck.InsSubNum=etrans.InsSubNum;
			etransAck.CarrierNum=etrans.CarrierNum;
			etransAck.DateTimeTrans=DateTime.Now;
			CCDFieldInputter fieldInputter=null;
			if(resultIsError) {
			  etransAck.Etype=EtransType.AckError;
			  etrans.Note="failed";
			}
			else {
			  fieldInputter=new CCDFieldInputter(result);
				CCDField fieldG05=fieldInputter.GetFieldById("G05");
				if(fieldG05!=null) {
					etransAck.AckCode=fieldG05.valuestr;
				}
			  etransAck.Etype=fieldInputter.GetEtransType();
			}
			Etranss.Insert(etransAck);
			Etranss.SetMessage(etransAck.EtransNum,result);
			etrans.AckEtransNum=etransAck.EtransNum;
			Etranss.Update(etrans);
			Etranss.SetMessage(etrans.EtransNum,strb.ToString());
			if(resultIsError) {
			  throw new ApplicationException(result);
			}
			new FormCCDPrint(etrans,result,true);//Print the response.
			if(etrans.AckCode=="R") {
				throw new ApplicationException(Lan.g("CanadianOutput","Reversal was rejected by clearinghouse. The claim must be reversed manually."));
			}
			return etransAck.EtransNum;
		}

		///<summary>Returns the list of etrans requests. The etrans.AckEtransNum can be used to get the etrans ack. The following are the only possible formats that can be returned in the acks: 21 EOB Response, 11 Claim Ack, 14 Outstanding Transactions Response, 23 Predetermination EOB, 13 Predetermination Ack, 24 E-Mail Response. Set version2 to true if version 02 request and false for version 04 request. Set sendToItrans to true only when sending to carrier 999999 representing the entire ITRANS network. When version2 is false and sendToItrans is false then carrier must be set to a valid Canadian carrier, otherwise it can be set to null. Prov must be validated as a CDANet provider before calling this function.</summary>
		public static List <Etrans> GetOutstandingTransactions(bool version2,bool sendToItrans,Carrier carrier,Provider prov) {
			List<Etrans> etransAcks=new List<Etrans>();
			Clearinghouse clearhouse=Canadian.GetClearinghouse();
			if(clearhouse==null) {
				throw new ApplicationException("Canadian clearinghouse not found.");
			}
			string saveFolder=clearhouse.ExportPath;
			if(!Directory.Exists(saveFolder)) {
				throw new ApplicationException(saveFolder+" not found.");
			}
			//We are required to send the request for outstanding transactions over and over until we get back an outstanding transactions ack format (Transaction type 14), because
			//there may be more than one item in the mailbox and we can only get one item at time.
			bool exit=false;
			do {
				StringBuilder strb=new StringBuilder();
				Etrans etrans=null;
				if(version2 || sendToItrans) {
					etrans=Etranss.CreateCanadianOutput(0,0,0,clearhouse.ClearinghouseNum,EtransType.RequestOutstand_CA,0,0);
				}
				else {
					if((carrier.CanadianSupportedTypes&CanSupTransTypes.RequestForOutstandingTrans_04)!=CanSupTransTypes.RequestForOutstandingTrans_04) {
						throw new ApplicationException("The carrier does not support request for outstanding transactions.");
					}
					etrans=Etranss.CreateCanadianOutput(0,carrier.CarrierNum,carrier.CanadianNetworkNum,
						clearhouse.ClearinghouseNum,EtransType.RequestOutstand_CA,0,0);
				}
				//A01 transaction prefix 12 AN
				if(version2 || sendToItrans) {
					strb.Append("            ");
				}
				else {
					if(carrier.CanadianNetworkNum==0) {
						throw new ApplicationException("Carrier network not set.");
					}
					CanadianNetwork network=CanadianNetworks.GetNetwork(carrier.CanadianNetworkNum);
					strb.Append(Canadian.TidyAN(network.CanadianTransactionPrefix,12));
				}
				//A02 office sequence number 6 N
				strb.Append(Canadian.TidyN(etrans.OfficeSequenceNumber,6));
				//A03 format version number 2 N
				if(version2) {
					strb.Append("02");
				}
				else {
					strb.Append("04");
				}
				//A04 transaction code 2 N
				strb.Append("04");//outstanding transactions request
				if(!version2) {//version 04
					//A05 carrier id number 6 N
					if(sendToItrans) {
						strb.Append("999999");
					}
					else {
						strb.Append(carrier.ElectID);//already validated as 6 digit number.
					}
				}
				//A06 software system id 3 AN
				strb.Append(Canadian.SoftwareSystemId());
				if(!version2) { //version 04
					//A10 encryption method 1 N
					if(sendToItrans) {
						strb.Append("1");
					}
					else {
						strb.Append(carrier.CanadianEncryptionMethod);//validated in UI
					}
				}
				//A07 message length N4
				if(!version2) { //version 04
					strb.Append(Canadian.TidyN("64",5));
				}
				else {
					strb.Append(Canadian.TidyN("42",4));
				}
				if(!version2) { //version 04
					//A09 carrier transaction counter 5 N
					strb.Append(Canadian.TidyN(etrans.CarrierTransCounter,5));
				}
				//According to the documentation for the outstanding transactions ack format, B01 only has to be a valid provider for the practice,
				//and that will trigger acknowledgements for all providers of the practice. I am assuming here that the same is true for the 
				//billing provider in field B03, because there is no real reason to limit the request to any particular provider.
				//B01 CDA provider number 9 AN
				strb.Append(Canadian.TidyAN(prov.NationalProvID,9));//already validated
				//B02 (treating) provider office number 4 AN
				strb.Append(Canadian.TidyAN(prov.CanadianOfficeNum,4));//already validated
				if(!version2) { //version 04
					//B03 billing provider number 9 AN
					//might need to account for possible 5 digit prov id assigned by carrier
					strb.Append(Canadian.TidyAN(prov.NationalProvID,9));//already validated
				}
				string result="";
				bool resultIsError=false;
				try {
					result=Canadian.PassToIca(strb.ToString(),clearhouse);
				}
				catch(ApplicationException ex) {
					result=ex.Message;
					resultIsError=true;
					//Etranss.Delete(etrans.EtransNum);//we don't want to do this, because we want the incremented etrans.OfficeSequenceNumber to be saved
					//Attach an ack indicating failure.
				}
				//Attach an ack to the etrans
				Etrans etransAck=new Etrans();
				etransAck.PatNum=etrans.PatNum;
				etransAck.PlanNum=etrans.PlanNum;
				etransAck.InsSubNum=etrans.InsSubNum;
				etransAck.CarrierNum=etrans.CarrierNum;
				etransAck.DateTimeTrans=DateTime.Now;
				CCDFieldInputter fieldInputter=null;
				if(resultIsError) {
					etransAck.Etype=EtransType.AckError;
					etrans.Note="failed";
				}
				else {
					if(result.Substring(12).StartsWith("NO MORE ITEMS")) {
						etransAck.Etype=EtransType.OutstandingAck_CA;
						exit=true;
					}
					else {
						fieldInputter=new CCDFieldInputter(result);
						CCDField fieldG05=fieldInputter.GetFieldById("G05");
						if(fieldG05!=null) {
							etransAck.AckCode=fieldG05.valuestr;
						}
						etransAck.Etype=fieldInputter.GetEtransType();
					}
				}
				Etranss.Insert(etransAck);
				Etranss.SetMessage(etransAck.EtransNum,result);
				etrans.AckEtransNum=etransAck.EtransNum;
				Etranss.Update(etrans);
				Etranss.SetMessage(etrans.EtransNum,strb.ToString());
				etransAcks.Add(etransAck);
				if(resultIsError) {
					throw new ApplicationException(result);
				}
				if(fieldInputter==null) { //happens in version 02 when a terminating message containing the text "NO MORE ITEMS" is received.
					break;
				}
				CCDField fieldA04=fieldInputter.GetFieldById("A04");//message format
				if(version2) {
					//In this case, there are only 4 possible responses: EOB, Claim Ack, Claim Ack with an error code, or Claim Ack with literal "NO MORE ITEMS" starting at character 13.
					if(fieldA04.valuestr=="11") {
						CCDField fieldG08=fieldInputter.GetFieldById("G08");
						if(fieldG08!=null && (fieldG08.valuestr=="004" || fieldG08.valuestr=="049")) { //Exit conditions specified in the documentation.
							etransAck.Etype=EtransType.OutstandingAck_CA;
							exit=true;
						}
					}
				}
				else { //version 04
					//Remember, the only allowed response transaction types are: 21 EOB Response, 11 Claim Ack, 14 Outstanding Transactions Response, 23 Predetermination EOB, 13 Predetermination Ack, 24 E-Mail Response
					if(fieldA04.valuestr=="14") {//Outstanding Transaction Ack Format
						CCDField fieldG05=fieldInputter.GetFieldById("G05");//response status
						if(fieldG05.valuestr=="R") { //We only expect the result to be 'R' or 'X' as specified in the documentation.
							CCDField fieldG07=fieldInputter.GetFieldById("G07");//disposition message
							CCDField fieldG08=fieldInputter.GetFieldById("G08");//error code
							MessageBox.Show(Lan.g("","Failed to receive outstanding transactions. Messages from CDANet")+": "+Environment.NewLine+
								fieldG07.valuestr.Trim()+Environment.NewLine+((fieldG08!=null)?CCDerror.message(Convert.ToInt32(fieldG08.valuestr),false):""));
						}
						etransAck.Etype=EtransType.OutstandingAck_CA;
						exit=true;
					}
				}
				//Field A02 exists in all of the possible formats (21,11,14,23,13,24).
				CCDField fieldA02=fieldInputter.GetFieldById("A02");//office sequence number
				//We use the Office Sequence Number to find the original etrans entry so that we can discover which patient the response is referring to.
				Etrans etranOriginal=Etranss.GetForSequenceNumberCanada(fieldA02.valuestr);
				if(etranOriginal!=null) { //Null will happen when testing, but should not happen in production.
					etrans.PatNum=etranOriginal.PatNum;
					etrans.PlanNum=etranOriginal.PlanNum;
					etrans.InsSubNum=etranOriginal.InsSubNum;
					etrans.ClaimNum=etranOriginal.ClaimNum;
					Etranss.Update(etrans);
					etransAck.PatNum=etranOriginal.PatNum;
					etransAck.PlanNum=etranOriginal.PlanNum;
					etransAck.InsSubNum=etranOriginal.InsSubNum;
					etransAck.ClaimNum=etranOriginal.ClaimNum;
					Etranss.Update(etransAck);
					if(!exit) {
						if(etransAck.ClaimNum!=0) {
							Claim claim=Claims.GetClaim(etransAck.ClaimNum);
							if(etransAck.AckCode=="A") {
								claim.ClaimStatus="R";
								claim.DateReceived=MiscData.GetNowDateTime();
							}
							else if(etransAck.AckCode=="H" || etransAck.AckCode=="B" || etransAck.AckCode=="C" || etransAck.AckCode=="N") {
								claim.ClaimStatus="S";
							}
							else if(etransAck.AckCode=="M") {
								Canadian.PrintManualClaimForm(claim);
							}
							Claims.Update(claim);
						}						
					}
				}
				if(!exit) {
					try {
						new FormCCDPrint(etrans,result,true);
					}
					catch(Exception ex) {
						MessageBox.Show(Lan.g("CanadianOutput","Failed to display one of the ROT responses")+": "+ex.Message);
					}
				}
			} while(!exit);
			return etransAcks;
		}

		///<summary>Each payment reconciliation request can return up to 9 pages. This function will return one etrans ack for each page in the result, since each page must be requested individually. Only for version 04, no such transaction exists for version 02. The provTreat and provBilling must be validated as CDANet providers before calling this function.</summary>
		public static List<Etrans> GetPaymentReconciliations(Carrier carrier,Provider provTreat,Provider provBilling,DateTime reconciliationDate) {
			Clearinghouse clearhouse=Canadian.GetClearinghouse();
			if(clearhouse==null) {
				throw new ApplicationException("Canadian clearinghouse not found.");
			}
			string saveFolder=clearhouse.ExportPath;
			if(!Directory.Exists(saveFolder)) {
				throw new ApplicationException(saveFolder+" not found.");
			}
			List<Etrans> etransAcks=new List<Etrans>();
			int pageNumber=1;
			int totalPages=1;
			do{
				StringBuilder strb=new StringBuilder();
				if((carrier.CanadianSupportedTypes&CanSupTransTypes.RequestForPaymentReconciliation_06)!=CanSupTransTypes.RequestForPaymentReconciliation_06) {
					throw new ApplicationException("The carrier does not support payment reconciliation transactions.");
				}
				if(carrier.CanadianNetworkNum==0) {
					throw new ApplicationException("Carrier network not set.");
				}
				CanadianNetwork network=CanadianNetworks.GetNetwork(carrier.CanadianNetworkNum);
				Etrans etrans=Etranss.CreateCanadianOutput(0,carrier.CarrierNum,carrier.CanadianNetworkNum,clearhouse.ClearinghouseNum,EtransType.RequestPay_CA,0,0);
				//A01 transaction prefix 12 AN
				strb.Append(Canadian.TidyAN(network.CanadianTransactionPrefix,12));
				//A02 office sequence number 6 N
				strb.Append(Canadian.TidyN(etrans.OfficeSequenceNumber,6));
				//A03 format version number 2 N
				strb.Append("04");
				//A04 transaction code 2 N
				strb.Append("06");//payment reconciliation request
				//A05 carrier id number 6 N
				if(network.CanadianIsRprHandler) {
					strb.Append("999999");//Always 999999 if the network handles the RPR requests instead of the carriers in the network.
				}
				else {
					strb.Append(carrier.ElectID);//already validated as 6 digit number.
				}
				//A06 software system id 3 AN
				strb.Append(Canadian.SoftwareSystemId());
				//A10 encryption method 1 N
				if(carrier!=null) {
					strb.Append(carrier.CanadianEncryptionMethod);//validated in UI
				}
				else {
					strb.Append("1");//No encryption when sending to a network.
				}
				//A07 message length N4
				strb.Append(Canadian.TidyN("77",5));
				//A09 carrier transaction counter 5 N
				strb.Append(Canadian.TidyN(etrans.CarrierTransCounter,5));
				//B01 CDA provider number 9 AN
				strb.Append(Canadian.TidyAN(provTreat.NationalProvID,9));//already validated
				//B02 (treating) provider office number 4 AN
				strb.Append(Canadian.TidyAN(provTreat.CanadianOfficeNum,4));//already validated
				//B03 billing provider number 9 AN
				//might need to account for possible 5 digit prov id assigned by carrier
				strb.Append(Canadian.TidyAN(provBilling.NationalProvID,9));//already validated
				//B04 billing provider office number 4 AN
				strb.Append(Canadian.TidyAN(provBilling.CanadianOfficeNum,4));//already validated
				//F33 Reconciliation Date 8 N
				strb.Append(reconciliationDate.ToString("yyyyMMdd"));
				//F38 Current Reconciliation Page Number N 1
				strb.Append(Canadian.TidyN(pageNumber,1));
				//End of message construction.
				string result="";
				bool resultIsError=false;
				try {
					result=Canadian.PassToIca(strb.ToString(),clearhouse);
				}
				catch(ApplicationException ex) {
					result=ex.Message;
					resultIsError=true;
					//Etranss.Delete(etrans.EtransNum);//we don't want to do this, because we want the incremented etrans.OfficeSequenceNumber to be saved
					//Attach an ack indicating failure.
				}
				//Attach an ack to the etrans
				Etrans etransAck=new Etrans();
				etransAck.PatNum=etrans.PatNum;
				etransAck.PlanNum=etrans.PlanNum;
				etransAck.InsSubNum=etrans.InsSubNum;
				etransAck.CarrierNum=etrans.CarrierNum;
				etransAck.DateTimeTrans=DateTime.Now;
				CCDFieldInputter fieldInputter=null;
				if(resultIsError) {
					etransAck.Etype=EtransType.AckError;
					etrans.Note="failed";
				}
				else {
					fieldInputter=new CCDFieldInputter(result);
					CCDField fieldG05=fieldInputter.GetFieldById("G05");
					if(fieldG05!=null) {
						etransAck.AckCode=fieldG05.valuestr;
					}
					etransAck.Etype=fieldInputter.GetEtransType();
				}
				Etranss.Insert(etransAck);
				Etranss.SetMessage(etransAck.EtransNum,result);
				etrans.AckEtransNum=etransAck.EtransNum;
				Etranss.Update(etrans);
				Etranss.SetMessage(etrans.EtransNum,strb.ToString());
				etransAcks.Add(etransAck);
				if(resultIsError) {
					throw new ApplicationException(result);
				}
				CCDField fieldG62=fieldInputter.GetFieldById("G62");//Last reconciliation page number.
				totalPages=PIn.Int(fieldG62.valuestr);
				new FormCCDPrint(etrans,result,true);
				pageNumber++;
			} while(pageNumber<=totalPages);
			return etransAcks;
		}

		//THIS TRANSACTION TYPE IS NOT USED BY ANY CANADIAN CARRIERS, AND IS NOT PART OF CERTIFICATION, EVEN THOUGH IT IS IN THE SPECIFICATIONS. WE NEED TO FIX BELOW COMMENTS AND MAKE THIS CODE FUNCTION MORE LIKE THE GetPaymentReconciliations() FUNCTION ABOVE.
		///<summary>Does not exist in version 02 so only supported for version 04. Returns the request Etrans record. Usually pass in a carrier with network null.  If sending to a network, carrier will be null and we still don't see anywhere in the message format to specify network.  We expect to get clarification on this issue later. Validate provTreat as a CDANet provider before calling this function.</summary>
		public static Etrans GetSummaryReconciliation(Carrier carrier,CanadianNetwork network,Provider provTreat,DateTime reconciliationDate) {
			Clearinghouse clearhouse=Canadian.GetClearinghouse();
			if(clearhouse==null) {
				throw new ApplicationException("Canadian clearinghouse not found.");
			}
			string saveFolder=clearhouse.ExportPath;
			if(!Directory.Exists(saveFolder)) {
				throw new ApplicationException(saveFolder+" not found.");
			}
			StringBuilder strb=new StringBuilder();
			Etrans etrans=null;
			if(carrier!=null) {
				if((carrier.CanadianSupportedTypes&CanSupTransTypes.RequestForSummaryReconciliation_05)!=CanSupTransTypes.RequestForSummaryReconciliation_05) {
					throw new ApplicationException("The carrier does not support summary reconciliation transactions.");
				}
				etrans=Etranss.CreateCanadianOutput(0,carrier.CarrierNum,carrier.CanadianNetworkNum,
					clearhouse.ClearinghouseNum,EtransType.RequestSumm_CA,0,0);
			}
			else {//Assume network!=null
				etrans=Etranss.CreateCanadianOutput(0,0,network.CanadianNetworkNum,
					clearhouse.ClearinghouseNum,EtransType.RequestSumm_CA,0,0);
			}
			//A01 transaction prefix 12 AN
			strb.Append(Canadian.TidyAN(network.CanadianTransactionPrefix,12));
			//A02 office sequence number 6 N
			strb.Append(Canadian.TidyN(etrans.OfficeSequenceNumber,6));
			//A03 format version number 2 N
			strb.Append("04");
			//A04 transaction code 2 N
			strb.Append("05");//payment reconciliation request
			//A05 carrier id number 6 N
			if(carrier!=null) {
				strb.Append(carrier.ElectID);//already validated as 6 digit number.
			}
			else { //Assume network!=null
				strb.Append("999999");//Always 999999 when sending to a network.
			}
			//A06 software system id 3 AN
			strb.Append(Canadian.SoftwareSystemId());
			//A10 encryption method 1 N
			if(carrier!=null) {
				strb.Append(carrier.CanadianEncryptionMethod);//validated in UI
			}
			else { //Assume network!=null
				strb.Append("1");//No encryption when sending to a network.
			}
			//A07 message length N4
			strb.Append(Canadian.TidyN("63",5));
			//A09 carrier transaction counter 5 N
			strb.Append(Canadian.TidyN(etrans.CarrierTransCounter,5));
			//B01 CDA provider number 9 AN
			strb.Append(Canadian.TidyAN(provTreat.NationalProvID,9));//already validated
			//B02 (treating) provider office number 4 AN
			strb.Append(Canadian.TidyAN(provTreat.CanadianOfficeNum,4));//already validated
			//F33 Reconciliation Date 8 N
			strb.Append(reconciliationDate.ToString("yyyyMMdd"));
			//End of message construction.
			string result="";
			bool resultIsError=false;
			try {
				if(carrier!=null) {
					result=Canadian.PassToIca(strb.ToString(),clearhouse);
				}
				else { //Assume network!=null
					result=Canadian.PassToIca(strb.ToString(),clearhouse);
				}
			}
			catch(ApplicationException ex) {
				result=ex.Message;
				resultIsError=true;
				//Etranss.Delete(etrans.EtransNum);//we don't want to do this, because we want the incremented etrans.OfficeSequenceNumber to be saved
				//Attach an ack indicating failure.
			}
			//Attach an ack to the etrans
			Etrans etransAck=new Etrans();
			etransAck.PatNum=etrans.PatNum;
			etransAck.PlanNum=etrans.PlanNum;
			etransAck.InsSubNum=etrans.InsSubNum;
			etransAck.CarrierNum=etrans.CarrierNum;
			etransAck.DateTimeTrans=DateTime.Now;
			CCDFieldInputter fieldInputter=null;
			if(resultIsError) {
				etransAck.Etype=EtransType.AckError;
				etrans.Note="failed";
			}
			else {
				fieldInputter=new CCDFieldInputter(result);
				CCDField fieldG05=fieldInputter.GetFieldById("G05");
				if(fieldG05!=null) {
					etransAck.AckCode=fieldG05.valuestr;
				}
				etransAck.Etype=fieldInputter.GetEtransType();
			}
			Etranss.Insert(etransAck);
			Etranss.SetMessage(etransAck.EtransNum,result);
			etrans.AckEtransNum=etransAck.EtransNum;
			Etranss.Update(etrans);
			Etranss.SetMessage(etrans.EtransNum,strb.ToString());
			if(resultIsError) {
				throw new ApplicationException(result);
			}
			new FormCCDPrint(etrans,result,true);
			return etrans;
		}


	}
}
