using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEmailInbox:Form {

		private EmailAddress Address;
		private List<EmailMessage> ListEmailMessages;

		public FormEmailInbox() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEmailInbox_Load(object sender,EventArgs e) {
			labelInboxComputerName.Text="Computer Name Where New Email Is Fetched: "+PrefC.GetString(PrefName.EmailInboxComputerName);
			labelThisComputer.Text+=Dns.GetHostName();
			Application.DoEvents();//Show the form contents before loading email into the grid.
			GetMessages();//If no new messages, then the user will know based on what shows in the grid.
		}

		private void menuItemSetup_Click(object sender,EventArgs e) {
			FormEmailAddresses formEA=new FormEmailAddresses();
			formEA.ShowDialog();
			labelInboxComputerName.Text="Computer Name Where New Email Is Fetched: "+PrefC.GetString(PrefName.EmailInboxComputerName);
			GetMessages();//Get new messages, just in case the user entered email information for the first time.
		}

		///<summary>Gets new messages from email inbox, as well as older messages from the db. Also fills the grid.</summary>
		private int GetMessages() {
			Address=EmailAddresses.GetByClinic(0);//Default for clinic/practice.
			if(Address.SMTPserverIncoming=="") {//Email address not setup.
				Text="Email Inbox";
				Address=null;
				MsgBox.Show(this,"Default email address has not been setup completely.");
				return 0;
			}			
			if(!CodeBase.ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.EmailInboxComputerName))) {//This is not the computer to get new messages from.
				Cursor=Cursors.WaitCursor;
				FillGridEmailMessages();//Get from db only.
				Cursor=Cursors.Default;
				return 0;
			}
			if(PrefC.GetString(PrefName.EmailInboxComputerName)=="") {
				MsgBox.Show(this,"Computer name to fetch new email from has not been setup.");
				return 0;
			}
			Cursor=Cursors.WaitCursor;
			Text="Email Inbox for "+Address.EmailUsername;
			//Get new emails
			int newMessagesCount=0;
			EmailMessage emailMessage;
			do {
				emailMessage=null;
				try {
					emailMessage=EhrEmail.ReceiveFromInbox(Address);
					newMessagesCount++;
				}
				catch(Exception ex) {
					if(ex.Message=="Inbox is empty.") {
						//Do not show message. Calling code decides to tell the user if no messages were found, because we do not want to show this message sometimes.
					}
					else {
						MessageBox.Show(Lan.g(this,"Failed to receive new email: ")+ex.Message);
						break;//Leave while loop.
					}
				}
				if(emailMessage!=null) {
					EmailMessages.Insert(emailMessage);
				}
			} while(emailMessage!=null);
			FillGridEmailMessages();
			Cursor=Cursors.Default;
			return newMessagesCount;
		}

		///<summary>Gets new emails and also shows older emails from the database.</summary>
		private void FillGridEmailMessages() {
			if(Address==null) {
				ListEmailMessages=new List<EmailMessage>();
			}
			else {
				ListEmailMessages=EmailMessages.GetReceivedForAddress(Address.EmailUsername);
			}
			gridEmailMessages.BeginUpdate();
			gridEmailMessages.Columns.Clear();
			int colReceivedDatePixCount=140;
			int colHasAttachPixCount=70;
			int colFromPixCount=200;
			int colSubjectPixCount=200;
			int colPatientPixCount=140;
			int colVariablePixCount=gridEmailMessages.Width-22-colReceivedDatePixCount-colHasAttachPixCount-colFromPixCount-colSubjectPixCount-colPatientPixCount;
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"ReceivedDate"),colReceivedDatePixCount,HorizontalAlignment.Center));
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"HasAttach"),colHasAttachPixCount,HorizontalAlignment.Center));
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Subject"),colSubjectPixCount,HorizontalAlignment.Left));
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"From"),colFromPixCount,HorizontalAlignment.Left));
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Patient"),colPatientPixCount,HorizontalAlignment.Left));
			gridEmailMessages.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Preview"),colVariablePixCount,HorizontalAlignment.Left));
			gridEmailMessages.Rows.Clear();
			for(int i=0;i<ListEmailMessages.Count;i++) {
				UI.ODGridRow row=new UI.ODGridRow();
				if(ListEmailMessages[i].SentOrReceived==EmailSentOrReceived.Received || ListEmailMessages[i].SentOrReceived==EmailSentOrReceived.WebMailReceived) {
					row.Bold=true;//unread
				}
				row.Cells.Add(new UI.ODGridCell(ListEmailMessages[i].MsgDateTime.ToString()));//ReceivedDate
				if(ListEmailMessages[i].Attachments.Count>0) {
					row.Cells.Add(new UI.ODGridCell("X"));//HasAttach
				}
				else {
					row.Cells.Add(new UI.ODGridCell(""));//HasAttach
				}
				row.Cells.Add(new UI.ODGridCell(ListEmailMessages[i].Subject));//Subject
				row.Cells.Add(new UI.ODGridCell(ListEmailMessages[i].FromAddress));//From
				if(ListEmailMessages[i].PatNum==0) {
					row.Cells.Add(new UI.ODGridCell(""));//Patient
				}
				else {
					Patient pat=Patients.GetPat(ListEmailMessages[i].PatNum);
					row.Cells.Add(new UI.ODGridCell(pat.GetNameLF()));//Patient
				}
				string preview=ListEmailMessages[i].BodyText.Replace("\r\n"," ").Replace('\n',' ');//Replace newlines with spaces, in order to compress the preview.
				if(preview.Length>50) {
					preview=preview.Substring(0,50)+"...";
				}
				row.Cells.Add(new UI.ODGridCell(preview));//Preview
				gridEmailMessages.Rows.Add(row);
			}
			gridEmailMessages.EndUpdate();
		}

		private void gridEmailMessages_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			if(e.Row==-1) {
				return;
			}
			EmailMessage emailMessage=ListEmailMessages[e.Row];
			MarkRead(emailMessage);
			FormEmailMessageEdit formEME=new FormEmailMessageEdit(emailMessage);
			formEME.ShowDialog();
			FillGridEmailMessages();//To show the email is read.
		}

		private void MarkRead(EmailMessage emailMessage) {
			if(emailMessage.SentOrReceived==EmailSentOrReceived.Received) {
				emailMessage.SentOrReceived=EmailSentOrReceived.Read;
				EmailMessages.Update(emailMessage);//Mark read.
			}
			else if(emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived) {
				emailMessage.SentOrReceived=EmailSentOrReceived.WebMailRecdRead;
				EmailMessages.Update(emailMessage);//Mark read.
			}
		}

		private void MarkUnread(EmailMessage emailMessage) {
			if(emailMessage.SentOrReceived==EmailSentOrReceived.Read) {
				emailMessage.SentOrReceived=EmailSentOrReceived.Received;
				EmailMessages.Update(emailMessage);//Mark read.
			}
			else if(emailMessage.SentOrReceived==EmailSentOrReceived.WebMailRecdRead) {
				emailMessage.SentOrReceived=EmailSentOrReceived.WebMailReceived;
				EmailMessages.Update(emailMessage);//Mark read.
			}
		}

		private void butChangePat_Click(object sender,EventArgs e) {
			if(gridEmailMessages.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an email message.");
				return;
			}
			FormPatientSelect form=new FormPatientSelect();
			if(form.ShowDialog()!=DialogResult.OK) {
				return;
			}
			for(int i=0;i<gridEmailMessages.SelectedIndices.Length;i++) {
				EmailMessage emailMessage=ListEmailMessages[gridEmailMessages.SelectedIndices[i]];
				emailMessage.PatNum=form.SelectedPatNum;
				EmailMessages.Update(emailMessage);
			}
			MessageBox.Show(Lan.g(this,"Email messages moved successfully")+": "+gridEmailMessages.SelectedIndices.Length);
			FillGridEmailMessages();//Refresh grid to show changed patient.
		}

		private void butMarkUnread_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			for(int i=0;i<gridEmailMessages.SelectedIndices.Length;i++) {
				EmailMessage emailMessage=ListEmailMessages[gridEmailMessages.SelectedIndices[i]];
				MarkUnread(emailMessage);
			}
			FillGridEmailMessages();
			Cursor=Cursors.Default;
		}

		private void butMarkRead_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			for(int i=0;i<gridEmailMessages.SelectedIndices.Length;i++) {
				EmailMessage emailMessage=ListEmailMessages[gridEmailMessages.SelectedIndices[i]];
				MarkRead(emailMessage);
			}
			FillGridEmailMessages();
			Cursor=Cursors.Default;
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			GetMessages();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}