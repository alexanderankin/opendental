﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness.HL7 {
	///<summary></summary>
	public class InternalEcwFull {

		public static HL7Def GetHL7Def() {
			HL7Def def=HL7Defs.GetInternalFromDb("eCWfull");

			return def;
		}


	}
}
