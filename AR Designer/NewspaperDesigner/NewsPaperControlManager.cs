using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NewsPaperControl;

namespace NewspaperDesigner
{
	public class NewsPaperControlManager
	{
		List<INewsPaperControl> lstNewsPaperControl;
		public List<INewsPaperControl> LstNewsPaperControl
		{
			get { return lstNewsPaperControl; }
			set { lstNewsPaperControl = value; }
		}
		
		public NewsPaperControlManager()
		{
			lstNewsPaperControl = new List<INewsPaperControl>();
		}
	}
}
