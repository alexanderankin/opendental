package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.google.gwt.xml.client.XMLParser;
import com.opendental.odweb.client.remoting.Serializing;
import com.google.gwt.i18n.client.DateTimeFormat;
import java.util.Date;

public class CustReference {
		/** Primary key. */
		public int CustReferenceNum;
		/** FK to patient.PatNum. */
		public int PatNum;
		/** Most recent date the reference was used, loosely kept updated. */
		public Date DateMostRecent;
		/** Notes specific to this customer as a reference. */
		public String Note;
		/** Set to true if this customer was a bad reference. */
		public boolean IsBadRef;

		/** Deep copy of object. */
		public CustReference Copy() {
			CustReference custreference=new CustReference();
			custreference.CustReferenceNum=this.CustReferenceNum;
			custreference.PatNum=this.PatNum;
			custreference.DateMostRecent=this.DateMostRecent;
			custreference.Note=this.Note;
			custreference.IsBadRef=this.IsBadRef;
			return custreference;
		}

		/** Serialize the object into XML. */
		public String SerializeToXml() {
			StringBuilder sb=new StringBuilder();
			sb.append("<CustReference>");
			sb.append("<CustReferenceNum>").append(CustReferenceNum).append("</CustReferenceNum>");
			sb.append("<PatNum>").append(PatNum).append("</PatNum>");
			sb.append("<DateMostRecent>").append(DateTimeFormat.getFormat("yyyyMMddHHmmss").format(DateMostRecent)).append("</DateMostRecent>");
			sb.append("<Note>").append(Serializing.EscapeForXml(Note)).append("</Note>");
			sb.append("<IsBadRef>").append((IsBadRef)?1:0).append("</IsBadRef>");
			sb.append("</CustReference>");
			return sb.toString();
		}

		/** Sets the variables for this object based on the values from the XML.
		 * @param xml The XML passed in must be valid and contain a node for every variable on this object.
		 * @throws Exception Deserialize is encased in a try catch and will pass any thrown exception on. */
		public void DeserializeFromXml(String xml) throws Exception {
			try {
				Document doc=XMLParser.parse(xml);
				if(Serializing.GetXmlNodeValue(doc,"CustReferenceNum")!=null) {
					CustReferenceNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"CustReferenceNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"PatNum")!=null) {
					PatNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"PatNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"DateMostRecent")!=null) {
					DateMostRecent=DateTimeFormat.getFormat("yyyyMMddHHmmss").parseStrict(Serializing.GetXmlNodeValue(doc,"DateMostRecent"));
				}
				if(Serializing.GetXmlNodeValue(doc,"Note")!=null) {
					Note=Serializing.GetXmlNodeValue(doc,"Note");
				}
				if(Serializing.GetXmlNodeValue(doc,"IsBadRef")!=null) {
					IsBadRef=(Serializing.GetXmlNodeValue(doc,"IsBadRef")=="0")?false:true;
				}
			}
			catch(Exception e) {
				throw e;
			}
		}


}
