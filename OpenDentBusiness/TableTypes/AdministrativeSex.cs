using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness{
	///<summary>Code system used to classify gender, used in EHR-CQMs.  This is a table in the database so that queries will be easier.  It will only have 3 rows.</summary>
	[Serializable()]
	public class AdministrativeSex:TableBase{
		///<summary>Primary key..</summary>
		[CrudColumn(IsPriKey=true)]
		public long AdministrativeSexNum;
		///<summary>Codes used in this code system. M, F, or U</summary>
		public string CodeValue;
		///<summary>Long description of the code.</summary>
		public string Description;

		///<summary></summary>
		public AdministrativeSex Clone() {
			return (AdministrativeSex)this.MemberwiseClone();
		}

	}

	
}




