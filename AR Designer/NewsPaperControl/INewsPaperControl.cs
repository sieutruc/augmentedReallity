using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace NewsPaperControl
{
	public interface INewsPaperControl
	{
		Control getCtrl();
		IAtt getAttList();
		string getName();
		Image getICO();
		Image getBackground();
		void loadBackgroundAndICO();
		void updateAttributeValue(string strAttName, object objAttValue);
		string toXMLString();
		void loadfromXML(string strXML, string path);
		INewsPaperControl clone();
		void removeFromPanel();
		void copy(INewsPaperControl control);
		string getDllPath();
		void setDllPath(string strDllPath);
		void setZoomCoefficient(int Degree);
		string getDisplayName();
		void setSize(Size s);
	}
}
