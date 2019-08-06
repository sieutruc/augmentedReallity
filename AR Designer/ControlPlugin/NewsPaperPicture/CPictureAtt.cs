using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel;
using NewsPaperControl;

namespace NewsPaperPictureControl
{
	public class CPictureAtt:CAttBase
	{
		public CPictureAtt()
		{			
			pictureURL = "(none)";			
		}

		private string pictureURL;
		[Editor(@"System.Windows.Forms.Design.FileNameEditor, System.Design,Version=2.0.0.0, Culture=neutral,PublicKeyToken=b03f5f7f11d50a3a",
			@"System.Drawing.Design.UITypeEditor,
			System.Drawing, Version=2.0.0.0, Culture=neutral,
			PublicKeyToken=b03f5f7f11d50a3a")]
		//[Editor(typeof(CodeSmith.FileSystemEditors.CSharpFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[Description("Select image's URL")] 
		[Category("Data")]
		[DisplayName("Picture URL")]
		public string PictureURL
		{
			get { return pictureURL; }
			set { pictureURL = value; }
		}
	}
}
