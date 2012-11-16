package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.google.gwt.xml.client.XMLParser;
import com.opendental.odweb.client.remoting.Serializing;

public class RecallTrigger {
		/** Primary key. */
		public int RecallTriggerNum;
		/** FK to recalltype.RecallTypeNum */
		public int RecallTypeNum;
		/** FK to procedurecode.CodeNum */
		public int CodeNum;

		/** Deep copy of object. */
		public RecallTrigger Copy() {
			RecallTrigger recalltrigger=new RecallTrigger();
			recalltrigger.RecallTriggerNum=this.RecallTriggerNum;
			recalltrigger.RecallTypeNum=this.RecallTypeNum;
			recalltrigger.CodeNum=this.CodeNum;
			return recalltrigger;
		}

		/** Serialize the object into XML. */
		public String SerializeToXml() {
			StringBuilder sb=new StringBuilder();
			sb.append("<RecallTrigger>");
			sb.append("<RecallTriggerNum>").append(RecallTriggerNum).append("</RecallTriggerNum>");
			sb.append("<RecallTypeNum>").append(RecallTypeNum).append("</RecallTypeNum>");
			sb.append("<CodeNum>").append(CodeNum).append("</CodeNum>");
			sb.append("</RecallTrigger>");
			return sb.toString();
		}

		/** Sets the variables for this object based on the values from the XML.
		 * @param xml The XML passed in must be valid and contain a node for every variable on this object.
		 * @throws Exception Deserialize is encased in a try catch and will pass any thrown exception on. */
		public void DeserializeFromXml(String xml) throws Exception {
			try {
				Document doc=XMLParser.parse(xml);
				if(Serializing.GetXmlNodeValue(doc,"RecallTriggerNum")!=null) {
					RecallTriggerNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"RecallTriggerNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"RecallTypeNum")!=null) {
					RecallTypeNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"RecallTypeNum"));
				}
				if(Serializing.GetXmlNodeValue(doc,"CodeNum")!=null) {
					CodeNum=Integer.valueOf(Serializing.GetXmlNodeValue(doc,"CodeNum"));
				}
			}
			catch(Exception e) {
				throw e;
			}
		}


}
