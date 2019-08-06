using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace NewsPaperControl
{
	public class MarkerDropdownList: StringConverter 
	{
		//List<string> lstValues;

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			//if (lstValues == null)
			//    lstValues = new List<string>();
			return new StandardValuesCollection(MarkerList.lstMarkerList);
		} 

		//public void setValueList(List<string> lstValueList)
		//{
		//    lstValues = new List<string>();
		//    foreach (string s in lstValueList)
		//    {
		//        lstValues.Add(s);
		//    }
		//}

	}
}
