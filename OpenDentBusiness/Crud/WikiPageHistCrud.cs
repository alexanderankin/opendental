//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	internal class WikiPageHistCrud {
		///<summary>Gets one WikiPageHist object from the database using the primary key.  Returns null if not found.</summary>
		internal static WikiPageHist SelectOne(long wikiPageNum){
			string command="SELECT * FROM wikipagehist "
				+"WHERE WikiPageNum = "+POut.Long(wikiPageNum);
			List<WikiPageHist> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one WikiPageHist object from the database using a query.</summary>
		internal static WikiPageHist SelectOne(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<WikiPageHist> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of WikiPageHist objects from the database using a query.</summary>
		internal static List<WikiPageHist> SelectMany(string command){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<WikiPageHist> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		internal static List<WikiPageHist> TableToList(DataTable table){
			List<WikiPageHist> retVal=new List<WikiPageHist>();
			WikiPageHist wikiPageHist;
			for(int i=0;i<table.Rows.Count;i++) {
				wikiPageHist=new WikiPageHist();
				wikiPageHist.WikiPageNum  = PIn.Long  (table.Rows[i]["WikiPageNum"].ToString());
				wikiPageHist.UserNum      = PIn.Long  (table.Rows[i]["UserNum"].ToString());
				wikiPageHist.PageTitle    = PIn.String(table.Rows[i]["PageTitle"].ToString());
				wikiPageHist.PageContent  = PIn.String(table.Rows[i]["PageContent"].ToString());
				wikiPageHist.DateTimeSaved= PIn.DateT (table.Rows[i]["DateTimeSaved"].ToString());
				wikiPageHist.IsDeleted    = PIn.Bool  (table.Rows[i]["IsDeleted"].ToString());
				retVal.Add(wikiPageHist);
			}
			return retVal;
		}

		///<summary>Inserts one WikiPageHist into the database.  Returns the new priKey.</summary>
		internal static long Insert(WikiPageHist wikiPageHist){
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				wikiPageHist.WikiPageNum=DbHelper.GetNextOracleKey("wikipagehist","WikiPageNum");
				int loopcount=0;
				while(loopcount<100){
					try {
						return Insert(wikiPageHist,true);
					}
					catch(Oracle.DataAccess.Client.OracleException ex){
						if(ex.Number==1 && ex.Message.ToLower().Contains("unique constraint") && ex.Message.ToLower().Contains("violated")){
							wikiPageHist.WikiPageNum++;
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
				return Insert(wikiPageHist,false);
			}
		}

		///<summary>Inserts one WikiPageHist into the database.  Provides option to use the existing priKey.</summary>
		internal static long Insert(WikiPageHist wikiPageHist,bool useExistingPK){
			if(!useExistingPK && PrefC.RandomKeys) {
				wikiPageHist.WikiPageNum=ReplicationServers.GetKey("wikipagehist","WikiPageNum");
			}
			string command="INSERT INTO wikipagehist (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="WikiPageNum,";
			}
			command+="UserNum,PageTitle,PageContent,DateTimeSaved,IsDeleted) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(wikiPageHist.WikiPageNum)+",";
			}
			command+=
				     POut.Long  (wikiPageHist.UserNum)+","
				+"'"+POut.String(wikiPageHist.PageTitle)+"',"
				+"'"+POut.String(wikiPageHist.PageContent)+"',"
				+    POut.DateT (wikiPageHist.DateTimeSaved)+","
				+    POut.Bool  (wikiPageHist.IsDeleted)+")";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				wikiPageHist.WikiPageNum=Db.NonQ(command,true);
			}
			return wikiPageHist.WikiPageNum;
		}

		///<summary>Updates one WikiPageHist in the database.</summary>
		internal static void Update(WikiPageHist wikiPageHist){
			string command="UPDATE wikipagehist SET "
				+"UserNum      =  "+POut.Long  (wikiPageHist.UserNum)+", "
				+"PageTitle    = '"+POut.String(wikiPageHist.PageTitle)+"', "
				+"PageContent  = '"+POut.String(wikiPageHist.PageContent)+"', "
				+"DateTimeSaved=  "+POut.DateT (wikiPageHist.DateTimeSaved)+", "
				+"IsDeleted    =  "+POut.Bool  (wikiPageHist.IsDeleted)+" "
				+"WHERE WikiPageNum = "+POut.Long(wikiPageHist.WikiPageNum);
			Db.NonQ(command);
		}

		///<summary>Updates one WikiPageHist in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.</summary>
		internal static void Update(WikiPageHist wikiPageHist,WikiPageHist oldWikiPageHist){
			string command="";
			if(wikiPageHist.UserNum != oldWikiPageHist.UserNum) {
				if(command!=""){ command+=",";}
				command+="UserNum = "+POut.Long(wikiPageHist.UserNum)+"";
			}
			if(wikiPageHist.PageTitle != oldWikiPageHist.PageTitle) {
				if(command!=""){ command+=",";}
				command+="PageTitle = '"+POut.String(wikiPageHist.PageTitle)+"'";
			}
			if(wikiPageHist.PageContent != oldWikiPageHist.PageContent) {
				if(command!=""){ command+=",";}
				command+="PageContent = '"+POut.String(wikiPageHist.PageContent)+"'";
			}
			if(wikiPageHist.DateTimeSaved != oldWikiPageHist.DateTimeSaved) {
				if(command!=""){ command+=",";}
				command+="DateTimeSaved = "+POut.DateT(wikiPageHist.DateTimeSaved)+"";
			}
			if(wikiPageHist.IsDeleted != oldWikiPageHist.IsDeleted) {
				if(command!=""){ command+=",";}
				command+="IsDeleted = "+POut.Bool(wikiPageHist.IsDeleted)+"";
			}
			if(command==""){
				return;
			}
			command="UPDATE wikipagehist SET "+command
				+" WHERE WikiPageNum = "+POut.Long(wikiPageHist.WikiPageNum);
			Db.NonQ(command);
		}

		///<summary>Deletes one WikiPageHist from the database.</summary>
		internal static void Delete(long wikiPageNum){
			string command="DELETE FROM wikipagehist "
				+"WHERE WikiPageNum = "+POut.Long(wikiPageNum);
			Db.NonQ(command);
		}

	}
}