package com.opendental.odweb.client.tabletypes;

import com.google.gwt.xml.client.Document;
import com.opendental.odweb.client.remoting.Serializing;

public class AutoCodeCond {
		/** Primary key. */
		public int AutoCodeCondNum;
		/** FK to autocodeitem.AutoCodeItemNum. */
		public int AutoCodeItemNum;
		/** Enum:AutoCondition  */
		public AutoCondition Cond;

		/** Deep copy of object. */
		public AutoCodeCond deepCopy() {
			AutoCodeCond autocodecond=new AutoCodeCond();
			autocodecond.AutoCodeCondNum=this.AutoCodeCondNum;
			autocodecond.AutoCodeItemNum=this.AutoCodeItemNum;
			autocodecond.Cond=this.Cond;
			return autocodecond;
		}

		/** Serialize the object into XML. */
		public String serialize() {
			StringBuilder sb=new StringBuilder();
			sb.append("<AutoCodeCond>");
			sb.append("<AutoCodeCondNum>").append(AutoCodeCondNum).append("</AutoCodeCondNum>");
			sb.append("<AutoCodeItemNum>").append(AutoCodeItemNum).append("</AutoCodeItemNum>");
			sb.append("<Cond>").append(Cond.ordinal()).append("</Cond>");
			sb.append("</AutoCodeCond>");
			return sb.toString();
		}

		/** Sets all the variables on this object based on the values in the XML document.  Variables that are not in the XML document will be null or their default values.
		 * @param doc A parsed XML document.  Must be valid XML.  Does not need to contain a node for every variable on this object.
		 * @throws Exception DeserializeFromXml is entirely encased in a try catch and will throw exceptions if anything goes wrong. */
		public void deserialize(Document doc) throws Exception {
			try {
				if(Serializing.getXmlNodeValue(doc,"AutoCodeCondNum")!=null) {
					AutoCodeCondNum=Integer.valueOf(Serializing.getXmlNodeValue(doc,"AutoCodeCondNum"));
				}
				if(Serializing.getXmlNodeValue(doc,"AutoCodeItemNum")!=null) {
					AutoCodeItemNum=Integer.valueOf(Serializing.getXmlNodeValue(doc,"AutoCodeItemNum"));
				}
				if(Serializing.getXmlNodeValue(doc,"Cond")!=null) {
					Cond=AutoCondition.values()[Integer.valueOf(Serializing.getXmlNodeValue(doc,"Cond"))];
				}
			}
			catch(Exception e) {
				throw e;
			}
		}

		/**  */
		public enum AutoCondition {
			/** 0 */
			Anterior,
			/** 1 */
			Posterior,
			/** 2 */
			Premolar,
			/** 3 */
			Molar,
			/** 4 */
			One_Surf,
			/** 5 */
			Two_Surf,
			/** 6 */
			Three_Surf,
			/** 7 */
			Four_Surf,
			/** 8 */
			Five_Surf,
			/** 9 */
			First,
			/** 10 */
			EachAdditional,
			/** 11 */
			Maxillary,
			/** 12 */
			Mandibular,
			/** 13 */
			Primary,
			/** 14 */
			Permanent,
			/** 15 */
			Pontic,
			/** 16 */
			Retainer,
			/** 17 */
			AgeOver18
		}


}