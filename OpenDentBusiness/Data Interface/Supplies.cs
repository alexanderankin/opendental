using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using OpenDentBusiness.DataAccess;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Supplies {

		///<summary>Gets all Supplies, ordered by category and itemOrder.  Optionally hides those marked IsHidden.  FindText must only include alphanumeric characters.</summary>
		public static List<Supply> CreateObjects(bool includeHidden,int supplierNum,string findText) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Supply>>(MethodBase.GetCurrentMethod(),includeHidden,supplierNum,findText);
			}
			string command="SELECT supply.* FROM supply,definition "
				+"WHERE definition.DefNum=supply.Category "
				+"AND supply.SupplierNum="+POut.PInt(supplierNum)+" ";
			if(findText!=""){
				command+="AND (supply.CatalogNumber REGEXP '"+POut.PString(findText)+"' OR supply.Descript REGEXP '"+POut.PString(findText)+"') ";
			}
			if(!includeHidden){
				command+="AND supply.IsHidden=0 ";
			}
			command+="ORDER BY definition.ItemOrder,supply.ItemOrder";
			return new List<Supply>(DataObjectFactory<Supply>.CreateObjects(command));
		}

		///<Summary>Gets one supply from the database.  Used for display in SupplyOrderItemEdit window.</Summary>
		public static Supply CreateObject(int supplyNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Supply>(MethodBase.GetCurrentMethod(),supplyNum);
			}
			//string command="SELECT * FROM supply WHERE SupplyNum="+POut.PInt(supplyNum);
			return DataObjectFactory<Supply>.CreateObject(supplyNum);
		}

		///<summary></summary>
		public static void WriteObject(Supply supp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),supp);
				return;
			}
			DataObjectFactory<Supply>.WriteObject(supp);
		}

		///<summary>Surround with try-catch.</summary>
		public static void DeleteObject(Supply supp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),supp);
				return;
			}
			//validate that not already in use.
			string command="SELECT COUNT(*) FROM supplyorderitem WHERE SupplyNum="+POut.PInt(supp.SupplyNum);
			int count=PIn.PInt(Db.GetCount(command));
			if(count>0){
				throw new ApplicationException(Lans.g("Supplies","Supply is already in use on an order. Not allowed to delete."));
			}
			DataObjectFactory<Supply>.DeleteObject(supp);
		}

		///<Summary>Loops through the supplied list and verifies that the ItemOrders are not corrupted.  If they are, then it fixes them.  Returns true if fix was required.  It makes a few assumptions about the quality of the list supplied.  Specifically, that there are no missing items, and that categories are grouped and sorted.</Summary>
		public static bool CleanupItemOrders(List<Supply> listSupply){
			//No need to check RemotingRole; no call to db.
			int catCur=-1;
			int previousOrder=-1;
			bool retVal=false;
			for(int i=0;i<listSupply.Count;i++){
				if(listSupply[i].Category!=catCur){//start of new category
					catCur=listSupply[i].Category;
					previousOrder=-1;
				}
				if(listSupply[i].ItemOrder!=previousOrder+1){
					listSupply[i].ItemOrder=previousOrder+1;
					WriteObject(listSupply[i]);
					retVal=true;
				}
				previousOrder++;
			}
			return retVal;
		}

		///<Summary>Gets from the database the last itemOrder for the specified category.  Used to send un unhidden supply to the end of the list.</Summary>
		public static int GetLastItemOrder(int supplierNum,int catNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),supplierNum,catNum);
			}
			string command="SELECT MAX(ItemOrder) FROM supply WHERE SupplierNum="+POut.PInt(supplierNum)
				+" AND Category="+POut.PInt(catNum)+" AND IsHidden=0";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0){
				return -1;
			}
			return PIn.PInt(table.Rows[0][0].ToString());
		}

		

		
		


	}

	


	


}









