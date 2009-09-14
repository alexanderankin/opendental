using System;
using System.Collections;
using System.Data;
using System.Drawing.Printing;
using System.Reflection;

namespace OpenDentBusiness{

	///<summary>Handles all the business logic for printers.  Used heavily by the UI.  Every single function that makes changes to the database must be completely autonomous and do ALL validation itself.</summary>
	public class Printers{
		///<summary>List of all printers.  Because of cache refresh, this gets properly refreshed on both ends.</summary>
		private static Printer[] list;

		public static DataTable RefreshCache() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM printer";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="Printer";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table) {
			//No need to check RemotingRole; no call to db.
			list=new Printer[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				list[i]=new Printer();
				list[i].PrinterNum=PIn.PInt(table.Rows[i][0].ToString());
				list[i].ComputerNum=PIn.PInt(table.Rows[i][1].ToString());
				list[i].PrintSit=(PrintSituation)PIn.PInt(table.Rows[i][2].ToString());
				list[i].PrinterName=PIn.PString(table.Rows[i][3].ToString());
				list[i].DisplayPrompt=PIn.PBool(table.Rows[i][4].ToString());
			}
		}

		///<summary></summary>
		public static Printer GetOnePrinter(PrintSituation sit,long compNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Printer>(MethodBase.GetCurrentMethod(),sit,compNum);
			}
			Printer[] tempList=list;
			string command="SELECT * FROM printer WHERE "
				+"PrintSit = '"      +POut.PInt((int)sit)+"' "
				+"AND ComputerNum ='"+POut.PInt(compNum)+"'";
			DataTable table=Db.GetTable(command);
			FillCache(table);
			if(list.Length==0){
				return null;
			}
			Printer result=list[0];
			list=tempList;
			return result;
		}

		///<summary></summary>
		private static long Insert(Printer cur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				cur.PrinterNum=Meth.GetInt(MethodBase.GetCurrentMethod(),cur);
				return cur.PrinterNum;
			}
			if(PrefC.RandomKeys){
				cur.PrinterNum=ReplicationServers.GetKey("printer","PrinterNum");
			}
			string command= "INSERT INTO printer (";
			if(PrefC.RandomKeys){
				command+="PrinterNum,";
			}
			command+="ComputerNum,PrintSit,PrinterName,"
				+"DisplayPrompt) VALUES(";
			if(PrefC.RandomKeys){
				command+="'"+POut.PInt(cur.PrinterNum)+"', ";
			}
			command+=
				 "'"+POut.PInt   (cur.ComputerNum)+"', "
				+"'"+POut.PInt   ((int)cur.PrintSit)+"', "
				+"'"+POut.PString(cur.PrinterName)+"', "
				+"'"+POut.PBool  (cur.DisplayPrompt)+"')";
 			if(PrefC.RandomKeys){
				Db.NonQ(command);
			}
			else{
 				cur.PrinterNum=Db.NonQ(command,true);
			}
			return cur.PrinterNum;
		}

		///<summary></summary>
		private static void Update(Printer cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),cur);
				return;
			}
			string command="UPDATE printer SET "
				+"ComputerNum = '"   +POut.PInt   (cur.ComputerNum)+"' "
				+",PrintSit = '"     +POut.PInt   ((int)cur.PrintSit)+"' "
				+",PrinterName = '"  +POut.PString(cur.PrinterName)+"' "
				+",DisplayPrompt = '"+POut.PBool  (cur.DisplayPrompt)+"' "
				+"WHERE PrinterNum = '"+POut.PInt(cur.PrinterNum)+"'";
 			Db.NonQ(command);
		}

		///<summary></summary>
		private static void Delete(Printer cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),cur);
				return;
			}
			string command="DELETE FROM printer "
				+"WHERE PrinterNum = "+POut.PInt(cur.PrinterNum);
			Db.NonQ(command);
		}

		public static bool PrinterIsInstalled(string name){
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<PrinterSettings.InstalledPrinters.Count;i++){
				if(PrinterSettings.InstalledPrinters[i]==name){
					return true;
				}
			}
			return false;
		}

		///<summary>Gets the set printer whether or not it is valid.</summary>
		public static Printer GetForSit(PrintSituation sit){
			//No need to check RemotingRole; no call to db.
			if(list==null) {
				RefreshCache();
			}
			Computer compCur=Computers.GetCur();
			for(int i=0;i<list.Length;i++){
				if(list[i].ComputerNum==compCur.ComputerNum
					&& list[i].PrintSit==sit)
				{
					return list[i];
				}
			}
			return null;
		}

		///<summary>Either does an insert or an update to the database if need to create a Printer object.  Or it also deletes a printer object if needed.</summary>
		public static void PutForSit(PrintSituation sit,string computerName, string printerName,bool displayPrompt){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),sit,computerName,printerName,displayPrompt);
				return;
			}
			//Computer[] compList=Computers.Refresh();
			//Computer compCur=Computers.GetCur();
			string command="SELECT ComputerNum FROM computer "
				+"WHERE CompName = '"+POut.PString(computerName)+"'";
 			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return;//computer not yet entered in db.
			}
			long compNum=PIn.PInt(table.Rows[0][0].ToString());
			Printer existing=GetOnePrinter(sit,compNum);   //GetForSit(sit);
			if(printerName=="" && !displayPrompt){//then should not be an entry in db
				if(existing!=null){//need to delete Printer
					Delete(existing);
				}
			}
			else if(existing==null){
				Printer cur=new Printer();
				cur.ComputerNum=compNum;
				cur.PrintSit=sit;
				cur.PrinterName=printerName;
				cur.DisplayPrompt=displayPrompt;
				Insert(cur);
			}
			else{
				existing.PrinterName=printerName;
				existing.DisplayPrompt=displayPrompt;
				Update(existing);
			}
		}

		///<summary>Called from FormPrinterSetup if user selects the easy option.  Since the other options will be hidden, we have to clear them.  User should be sternly warned before this happens.</summary>
		public static void ClearAll(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			//first, delete all entries
			string command="DELETE FROM printer";
 			Db.NonQ(command);
			//then, add one printer for each computer. Default and show prompt
			Computers.RefreshCache();
			Printer cur;
			for(int i=0;i<Computers.List.Length;i++){
				cur=new Printer();
				cur.ComputerNum=Computers.List[i].ComputerNum;
				cur.PrintSit=PrintSituation.Default;
				cur.PrinterName="";
				cur.DisplayPrompt=true;
				Insert(cur);
			}
		}

	}
}