using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel; 

namespace PictureControl
{
	public class CPictureAtt:CAttBase
	{
		public CPictureAtt()
		{
			pictureURL1 = "(none)";			
		}

		private string pictureURL1;
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[Description("Select image's path !")] 
		[DefaultValue("none")]
		[Category("Data")]
		public string PictureURL1
		{
			get { return pictureURL1; }
			set { pictureURL1 = value; }
		}
		
		public new void setImageURL1(string value)
		{
			pictureURL1 = value;
		}

		public new string getImageURL1()
		{
			return pictureURL1;
		}
	}
}
