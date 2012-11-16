package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.google.gwt.xml.client.XMLParser;
import com.opendental.odweb.client.remoting.Serializing;
import com.google.gwt.i18n.client.DateTimeFormat;
import java.util.Date;

public class Recall {
		/** Primary key. */
		public int RecallNum;
		/** FK to patient.PatNum. */
		public int PatNum;
		/** Not editable.  The calculated date due. Generated by the program and subject to change anytime the conditions change. It can be blank (0001-01-01) if no appropriate triggers.  */
		public Date DateDueCalc;
		/** This is the date that is actually used when doing reports for recall. It will usually be the same as DateDueCalc unless user has changed it. System will only update this field if it is the same as DateDueCalc.  Otherwise, it will be left alone.  Gets cleared along with DateDueCalc when resetting recall.  When setting disabled, this field will also be cleared.  This is the field to use if converting from another software. */
		public Date DateDue;
		/** Not editable. Previous date that procedures were done to trigger this recall. It is calculated and enforced automatically.  If you want to affect this date, add a procedure to the chart with a status of C, EC, or EO. */
		public Date DatePrevious;
		/** The interval between recalls.  The Interval struct combines years, months, weeks, and days into a single integer value. */
		public int RecallInterval;
		/** FK to definition.DefNum, or 0 for none. */
		public int RecallStatus;
		/** An administrative note for staff use. */
		public String Note;
		/** If true, this recall type will be disabled (there's only one type right now). This is usually used rather than deleting the recall type from the patient because the program must enforce the trigger conditions for all patients. */
		public boolean IsDisabled;
		/** Last datetime that this row was inserted or updated. */
		public Date DateTStamp;
		/** FK to recalltype.RecallTypeNum. */
		public int RecallTypeNum;
		/** Default is 0.  If a positive number is entered, then the family balance must be less in order for this recall to show in the recall list. */
		public double DisableUntilBalance;
		/** If a date is entered, then this recall will be disabled until that date. */
		public Date DisableUntilDate;
		/** This will only have a value if a recall is scheduled. */
		public Date DateScheduled;

		/** Deep copy of object. */
		public Recall Copy() {
			Recall recall=new Recall();
			recall.RecallNum=this.RecallNum;
			recall.PatNum=this.PatNum;
			recall.DateDueCalc=this.DateDueCalc;
			recall.DateDue=this.DateDue;
			recall.DatePrevious=this.DatePrevious;
			recall.RecallInterval=this.RecallInterval;
			recall.RecallStatus=this.RecallStatus;
			recall.Note=this.Note;
			recall.IsDisabled=this.IsDisabled;
			recall.DateTStamp=this.DateTStamp;
			recall.RecallTypeNum=this.RecallTypeNum;
			recall.DisableUntilBalance=this.DisableUntilBalance;
			recall.DisableUntilDate=this.DisableUntilDate;
			recall.DateScheduled=this.DateScheduled;
			return recall;
		}

		/** Serialize the object into XML. */
		public String SerializeToXml() {
			StringBuilder sb=new StringBuilder();
			sb.append("<Recall>");
			sb.append("<RecallNum>").append(RecallNum).append("</RecallNum>");
			sb.append("<PatNum>").append(PatNum).append("</PatNum>");
			sb.append("<DateDueCalc>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(DateDueCalc)).append("</DateDueCalc>");
			sb.append("<DateDue>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(DateDue)).append("</DateDue>");
			sb.append("<DatePrevious>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(DatePrevious)).append("</DatePrevious>");
			sb.append("<RecallInterval>").append(RecallInterval).append("</RecallInterval>");
			sb.append("<RecallStatus>").append(RecallStatus).append("</RecallStatus>");
			sb.append("<Note>").append(Serializing.EscapeForXml(Note)).append("</Note>");
			sb.append("<IsDisabled>").append((IsDisabled)?1:0).append("</IsDisabled>");
			sb.append("<DateTStamp>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(DateTStamp)).append("</DateTStamp>");
			sb.append("<RecallTypeNum>").append(RecallTypeNum).append("</RecallTypeNum>");
			sb.append("<DisableUntilBalance>").append(DisableUntilBalance).append("</DisableUntilBalance>");
			sb.append("<DisableUntilDate>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(DisableUntilDate)).append("</DisableUntilDate>");
			sb.append("<DateScheduled>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(DateScheduled)).append("</DateScheduled>");
			sb.append("</Recall>");
			return sb.toString();
		}

		/** Sets the variables for this object based on the values from the XML.
		 * @param xml The XML passed in must be valid and contain a node for every variable on this object.
		 * @throws Exception Deserialize is encased in a try catch and will pass any thrown exception on. */
		public void DeserializeFromXml(String xml) throws Exception {
			try {
				Document doc=XMLParser.parse(xml);
				if(Serializing.GetXmlNodeValue(doc,"RecallNum")!=null) {
					RecallNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"RecallNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"PatNum")!=null) {
					PatNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"PatNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"DateDueCalc")!=null) {
					DateDueCalc=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(Serializing.GetXmlNodeValue(doc,"DateDueCalc"));
				}
				if(Serializing.GetXmlNodeValue(doc,"DateDue")!=null) {
					DateDue=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(Serializing.GetXmlNodeValue(doc,"DateDue"));
				}
				if(Serializing.GetXmlNodeValue(doc,"DatePrevious")!=null) {
					DatePrevious=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(Serializing.GetXmlNodeValue(doc,"DatePrevious"));
				}
				if(Serializing.GetXmlNodeValue(doc,"RecallInterval")!=null) {
					RecallInterval=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"RecallInterval"));
				}
				if(Serializing.GetXmlNodeValue(doc,"RecallStatus")!=null) {
					RecallStatus=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"RecallStatus"));
				}
				if(Serializing.GetXmlNodeValue(doc,"Note")!=null) {
					Note=Serializing.GetXmlNodeValue(doc,"Note");
				}
				if(Serializing.GetXmlNodeValue(doc,"IsDisabled")!=null) {
					IsDisabled=(Serializing.GetXmlNodeValue(doc,"IsDisabled")=="0")?false:true;
				}
				if(Serializing.GetXmlNodeValue(doc,"DateTStamp")!=null) {
					DateTStamp=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(Serializing.GetXmlNodeValue(doc,"DateTStamp"));
				}
				if(Serializing.GetXmlNodeValue(doc,"RecallTypeNum")!=null) {
					RecallTypeNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"RecallTypeNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"DisableUntilBalance")!=null) {
					DisableUntilBalance=Double.valueOf(Serializing.GetXmlNodeValue(doc,"DisableUntilBalance"));
				}
				if(Serializing.GetXmlNodeValue(doc,"DisableUntilDate")!=null) {
					DisableUntilDate=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(Serializing.GetXmlNodeValue(doc,"DisableUntilDate"));
				}
				if(Serializing.GetXmlNodeValue(doc,"DateScheduled")!=null) {
					DateScheduled=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(Serializing.GetXmlNodeValue(doc,"DateScheduled"));
				}
			}
			catch(Exception e) {
				throw e;
			}
		}


}
