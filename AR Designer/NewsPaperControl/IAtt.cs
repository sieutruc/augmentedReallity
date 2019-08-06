using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace NewsPaperControl
{
	public interface IAtt
	{
		Point getLocation();
		void setLocation(Point pLocation);

		Size getSize();
		void setSize(Size pSize);//Width and Height

		void setZoomCoefficient(int iZoomDegree);
		
		string getMarkerName();
		void setMarkerName(string name);

		string getInstanceName();
		void setInstanceName(string name);
	}
}
