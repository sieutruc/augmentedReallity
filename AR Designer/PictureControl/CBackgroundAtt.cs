using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel;

namespace PictureControl
{
	public class CBackgroundAtt
	{
		public CBackgroundAtt()
		{
			
		}

		private int backgrWidth;
		[DisplayName("Background Width")]
		public int Width
		{
			get { return backgrWidth; }
			set { backgrWidth = value; }
		}
		private int backgrHeight;
		[DisplayName("Background Height")]
		public int Height
		{
			get { return backgrHeight; }
			set { backgrHeight = value; }
		}

		private string backgroundURL;
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[Description("Select image's path !")]
		[DefaultValue("none")]
		[Category("Data")]
		public string BackgroundURL
		{
			get { return backgroundURL; }
			set { backgroundURL = value; }
		}
	}
}
