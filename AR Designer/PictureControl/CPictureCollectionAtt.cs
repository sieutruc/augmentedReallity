using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel; 

namespace PictureControl
{
	public class CPictureCollectionAtt : CPictureAtt
	{
		public CPictureCollectionAtt()
		{
			pictureURL2 = "(none)";
			pictureURL3 = "(none)";
			pictureURL4 = "(none)";
			pictureURL5 = "(none)";

			//pictureList = new List<string>();
		}

		//private List<string> pictureList;
		//[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
		//"System.Drawing.Design.UITypeEditor, System.Drawing, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")] 
		//public List<string> PictureList
		//{
		//    get { return pictureList; }
		//    set { pictureList = value; }
		//}


		private string pictureURL2;
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[Description("Select image's path !")]
		[DefaultValue("c:\\msdos.sys")]
		[Category("Data")]
		public string PictureURL2
		{
			get { return pictureURL2; }
			set { pictureURL2 = value; }
		}

		private string pictureURL3;
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[Description("Select image's path !")]
		[DefaultValue("c:\\msdos.sys")]
		[Category("Data")]
		public string PictureURL3
		{
			get { return pictureURL3; }
			set { pictureURL3 = value; }
		}

		private string pictureURL4;
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[Description("Select image's path !")]
		[DefaultValue("c:\\msdos.sys")]
		[Category("Data")]
		public string PictureURL4
		{
			get { return pictureURL4; }
			set { pictureURL4 = value; }
		}

		private string pictureURL5;
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[Description("Select image's path !")]
		[DefaultValue("c:\\msdos.sys")]
		[Category("Data")]
		public string PictureURL5
		{
			get { return pictureURL5; }
			set { pictureURL5 = value; }
		}

		public new void setImageURL2(string value)
		{
			pictureURL2 = value;
		}

		public new void setImageURL3(string value)
		{
			pictureURL3 = value;
		}

		public new void setImageURL4(string value)
		{
			pictureURL4 = value;
		}

		public new void setImageURL5(string value)
		{
			pictureURL5 = value;
		}

		public new string getImageURL2()
		{
			return pictureURL2;
		}

		public new string getImageURL3()
		{
			return pictureURL3;
		}

		public new string getImageURL4()
		{
			return pictureURL4;
		}

		public new string getImageURL5()
		{
			return pictureURL5;
		}
	}
}
