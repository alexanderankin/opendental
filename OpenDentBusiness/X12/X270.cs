using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
	///<summary></summary>
	public class X270:X12object{

		public X270(string messageText):base(messageText){
		
		}

		///<summary>This is only a temporary solution for testing.</summary>
		public static string GenerateMessageText() {
			return "ISA*00*          *00*          *30*AA0989922      *30*330989922      *030519*1608*U*00401*000012145*1*T*:~GS*HS*AA0989922*330989922*20030519*1608*12145*X*004010X092~ST*270*0001~BHT*0022*13*ASX012145WEB*20030519*1608~HL*1**20*1~NM1*PR*2*ACME DENTAL PLANS*****PI*12345~HL*2*1*21*1~NM1*1P*1*PROVLAST*PROVFIRST****FI*005558006~N3*JUNIT ROAD~N4*CHICAGO*IL*60602~PRV*PE*ZZ*1223G0001X~HL*3*2*22*0~TRN*1*12145*1AA0989922~NM1*IL*1*SUBLASTNAME*SUBFIRSTNAME****MI*123456789~DMG*D8*19750323~DTP*307*D8*20030519~EQ*30~SE*16*0001~GE*1*12145~IEA*1*000012145~";
		}
		


	}
}
