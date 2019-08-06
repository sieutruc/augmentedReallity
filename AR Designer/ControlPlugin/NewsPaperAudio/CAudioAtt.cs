using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel;
using NewsPaperControl;

namespace NewsPaperAudio
{
    public class CAudioAtt:CAttBase
    {
        public CAudioAtt()
		{			
			audioURL = "(none)";			
		}

        private string audioURL;
		[Editor(@"System.Windows.Forms.Design.FileNameEditor, System.Design,Version=2.0.0.0, Culture=neutral,PublicKeyToken=b03f5f7f11d50a3a",
			@"System.Drawing.Design.UITypeEditor,
			System.Drawing, Version=2.0.0.0, Culture=neutral,
			PublicKeyToken=b03f5f7f11d50a3a")]
		//[Editor(typeof(CodeSmith.FileSystemEditors.CSharpFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
		[Description("Select audio's URL")] 
		[Category("Data")]
		[DisplayName("Audio URL")]
        public string AudioURL
		{
            get { return audioURL; }
            set { audioURL = value; }
		}
    }
}
