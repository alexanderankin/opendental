using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class WikiPageHists{

		///<summary>Ordered by dateTimeSaved.</summary>
		public static List<WikiPageHist> GetByTitle(string pageTitle){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<WikiPageHist>>(MethodBase.GetCurrentMethod(),pageTitle);
			}
			string command="SELECT * FROM wikipagehist WHERE PageTitle = '"+POut.String(pageTitle)+"' ORDER BY DateTimeSaved";
			return Crud.WikiPageHistCrud.SelectMany(command);
		}

		///<summary></summary>
		public static List<string> GetForSearch(string searchText,bool ignoreContent) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<string>>(MethodBase.GetCurrentMethod(),searchText,ignoreContent);
			}
			List<string> retVal=new List<string>();
			DataTable tableResults=new DataTable();
			string command="";
				command=
				"SELECT PageTitle FROM wikiPageHist "
				+"WHERE PageTitle LIKE '%"+searchText+"%' "
				+"AND PageTitle NOT LIKE '\\_%' "
				+"AND PageTitle NOT IN (SELECT PageTitle FROM WikiPage) "//ignore pages that exist again...
				+"AND IsDeleted=1 "
				+"AND DateTimeSaved = (SELECT MAX(DateTimeSaved) FROM wikiPageHist WHERE PageTitle NOT LIKE '\\_%' AND IsDeleted=1) "
				//+"GROUP BY PageTitle "
				+"ORDER BY PageTitle";
				tableResults=Db.GetTable(command);
				for(int i=0;i<tableResults.Rows.Count;i++) {
					if(!retVal.Contains(tableResults.Rows[i]["PageTitle"].ToString())) {
						retVal.Add(tableResults.Rows[i]["PageTitle"].ToString());
					}
				}
				//Match Content Second-----------------------------------------------------------------------------------
				if(!ignoreContent) {
					command=
					"SELECT PageTitle FROM wikiPageHist "
					+"WHERE PageContent LIKE '%"+searchText+"%' "
					+"AND PageTitle NOT LIKE '\\_%' "
					+"AND PageTitle NOT IN (SELECT PageTitle FROM WikiPage) "//ignore pages that exist again...
					+"AND IsDeleted=1 "
					+"AND DateTimeSaved = (Select MAX(DateTimeSaved) FROM wikiPageHist WHERE PageTitle NOT LIKE '\\_%' AND IsDeleted=1) "
					//+"GROUP BY PageTitle "
					+"ORDER BY PageTitle";
					tableResults=Db.GetTable(command);
					for(int i=0;i<tableResults.Rows.Count;i++) {
						if(!retVal.Contains(tableResults.Rows[i]["PageTitle"].ToString())) {
							retVal.Add(tableResults.Rows[i]["PageTitle"].ToString());
						}
					}
				}
			return retVal;
		}

		///<summary>Only returns the most recently deleted version of the page. Returns null if not found.</summary>
		public static WikiPageHist GetDeletedByTitle(string pageTitle) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<WikiPageHist>(MethodBase.GetCurrentMethod(),pageTitle);
			}
			string command="SELECT * FROM wikipagehist "
										+"WHERE PageTitle = '"+POut.String(pageTitle)+"' "
										+"AND IsDeleted=1 "
										+"AND DateTimeSaved="
											+"(SELECT MAX(DateTimeSaved) "
											+"FROM wikipagehist "
											+"WHERE PageTitle = '"+POut.String(pageTitle)+"' "
											+"AND IsDeleted=1)"
											;
			return Crud.WikiPageHistCrud.SelectOne(command);
		}

		///<summary></summary>
		public static long Insert(WikiPageHist wikiPageHist){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				wikiPageHist.WikiPageNum=Meth.GetLong(MethodBase.GetCurrentMethod(),wikiPageHist);
				return wikiPageHist.WikiPageNum;
			}
			return Crud.WikiPageHistCrud.Insert(wikiPageHist);
		}



		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary>Gets one WikiPageHist from the db.</summary>
		public static WikiPageHist GetOne(long wikiPageNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<WikiPageHist>(MethodBase.GetCurrentMethod(),wikiPageNum);
			}
			return Crud.WikiPageHistCrud.SelectOne(wikiPageNum);
		}

		///<summary></summary>
		public static void Update(WikiPageHist wikiPageHist){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),wikiPageHist);
				return;
			}
			Crud.WikiPageHistCrud.Update(wikiPageHist);
		}

		///<summary></summary>
		public static void Delete(long wikiPageNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),wikiPageNum);
				return;
			}
			string command= "DELETE FROM wikipagehist WHERE WikiPageNum = "+POut.Long(wikiPageNum);
			Db.NonQ(command);
		}
		*/



	}
}