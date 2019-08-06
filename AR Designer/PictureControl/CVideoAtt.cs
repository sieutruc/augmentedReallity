using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel; 

namespace PictureControl
{
	public class CVideoAtt:CAttBase
	{
		public CVideoAtt()
		{
			videoURL = "(none)";
		}

		private string videoURL;
		[Editor(typeof(FileNameEditor), typeof(UITypeEditor))]
		[Description("Select video's path !")]
		[DefaultValue("(none)")]
		[Category("Data")]
		public string VideoURL
		{
			get { return videoURL; }
			set { videoURL = value; }
		}
        public void setVideoURL(string value)
        {
            videoURL = value;
        }
		public new string getVideo()
		{
			return videoURL;
		}
	}
}
