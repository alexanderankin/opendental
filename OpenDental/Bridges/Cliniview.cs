using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental.Bridges{
	/// <summary></summary>
	public class Cliniview{

		/// <summary></summary>
		public Cliniview() {
			
		}

		///<summary>Launches the program using command line.</summary>
		public static void SendData(Program ProgramCur, Patient pat){
			//usage: C:\Program Files\CliniView\CliniView.exe -123;John;Smith;123456789;01/01/1980
			//Critical not to have any spaces in argument string.
			//We got this info directly from the programmers at CliniView
			if(pat==null){
				MsgBox.Show("Cliniview","Please select a patient first.");
				return;
			}
			if(!File.Exists(ProgramCur.Path)){
				MessageBox.Show(ProgramCur.Path+" not found.");
				return;
			}
			string info="-";
			if(ProgramProperties.GetPropVal(ProgramCur.ProgramNum,"Enter 0 to use PatientNum, or 1 to use ChartNum")=="1") {
				if(pat.ChartNumber=="") {
					MsgBox.Show("Cliniview","This patient has no ChartNumber entered.");
					return;
				}
				info+=pat.ChartNumber;
			}
			else {
				info+=pat.PatNum.ToString();
			}
			info+=";"+Tidy(pat.FName)
				+";"+Tidy(pat.LName)
				+";"+pat.SSN//dashes already missing
				+";"+pat.Birthdate.ToString("MM/dd/yyyy");
			Process process=new Process();
			process.StartInfo=new ProcessStartInfo(ProgramCur.Path,info);
			try{
				process.Start();
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
			}
		}

		///<summary>Removes semicolons and spaces.</summary>
		private static string Tidy(string input){
			string retVal=input.Replace(";","");//get rid of any semicolons.
			retVal=retVal.Replace(" ","");
			return retVal;
		}

	}
}










