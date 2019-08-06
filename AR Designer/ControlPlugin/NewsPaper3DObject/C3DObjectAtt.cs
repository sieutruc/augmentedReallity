using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel;
using NewsPaperControl;

namespace NewsPaper3DObject
{
    public class C3DObjectAtt:CAttBase
    {
        public C3DObjectAtt()
		{
            textureURL = "(none)";
            md2URL = "(none)";
		}

        private string textureURL;
        private string md2URL;

		[Editor(@"System.Windows.Forms.Design.FileNameEditor, System.Design,Version=2.0.0.0, Culture=neutral,PublicKeyToken=b03f5f7f11d50a3a",
			@"System.Drawing.Design.UITypeEditor,
			System.Drawing, Version=2.0.0.0, Culture=neutral,
			PublicKeyToken=b03f5f7f11d50a3a")]
		//[Editor(typeof(CodeSmith.FileSystemEditors.CSharpFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[Description("Select texture's URL")] 
		[Category("Data")]
		[DisplayName("Texture URL")]
        public string TextureURL
        {
            get { return textureURL; }
            set { textureURL = value; }
        }

		[Editor(@"System.Windows.Forms.Design.FileNameEditor, System.Design,Version=2.0.0.0, Culture=neutral,PublicKeyToken=b03f5f7f11d50a3a",
			@"System.Drawing.Design.UITypeEditor,
			System.Drawing, Version=2.0.0.0, Culture=neutral,
			PublicKeyToken=b03f5f7f11d50a3a")]
		//[Editor(typeof(CodeSmith.FileSystemEditors.CSharpFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[Description("Select Md2's URL")]
		[Category("Data")]
		[DisplayName("Md2 URL")]
        public string Md2URL
        {
            get { return md2URL; }
            set { md2URL = value; }
        }

    }
}
