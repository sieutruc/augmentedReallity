using System;
using System.Collections.Generic;
using System.Text;
using NewsPaperControl;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;

namespace NewsPaperMarkerControl
{
	public class NewsPaperMarkerControl: NewsPaperControlBase
	{
		public NewsPaperMarkerControl()
		{
			Attributs = new CMarkerAtt();
			StrDisplayName = "Marker Object";
			Attributs.setSize(new Size(30, 30));
		}

		public override string getDisplayName()
		{
			return StrDisplayName;
		}

		public override void loadBackgroundAndICO()
		{
			try
			{
				System.Reflection.Assembly thisExe;
				thisExe = System.Reflection.Assembly.GetExecutingAssembly();

				System.IO.Stream file = thisExe.GetManifestResourceStream("NewsPaperMarker." + StrName + "-ICO.png");
				System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(file);
				ImgIcon = bmp;

				file = thisExe.GetManifestResourceStream("NewsPaperMarker." + StrName + ".png");
				bmp = new System.Drawing.Bitmap(file);
				ImgBackground = bmp;
			}
			catch { };
		}

		public override void initCtrl()
		{
			Ctrl = new PictureBox();
			Ctrl.Name = "PictureBox";
			Ctrl.BackgroundImageLayout = ImageLayout.Center;
			Ctrl.BackgroundImage = ImgBackground;

			Ctrl.MouseMove += new System.Windows.Forms.MouseEventHandler(base.ctrl_MouseMove);
			Ctrl.MouseDown += new System.Windows.Forms.MouseEventHandler(base.ctrl_MouseDown);
			Ctrl.MouseUp += new System.Windows.Forms.MouseEventHandler(base.ctrl_MouseUp);

			base.Controls.Clear();
			base.Controls.Add(Ctrl);
			Ctrl.Bounds = base.Bounds;
			base.calculateEdges();
		}

		public override void setControlName()
		{
			StrName = "NewsPaperMarker";
		}

		public override void updateAttributeValue(string strAttName, object objAttValue)
		{
			updateAttributeValueBase(strAttName, objAttValue);
			//switch (strAttName)
			//{
			//    case "MarkerURL":
			//        {
			//            try
			//            {
			//                ((CMarkerAtt)Attributs).MarkerURL = objAttValue.ToString();
			//                Bitmap img = new Bitmap(objAttValue.ToString());
			//                ImgBackground = img;
			//                Ctrl.BackgroundImage = ImgBackground;
			//            }
			//            catch { };
			//            break;
			//        }
			//}
		}

		public override string toXMLString()
		{
			StringBuilder strXML = new StringBuilder();
			strXML.Append(@"<marker>
                         <name>%name%</name>
                         <x>%x%</x>
                         <y>%y%</y>
                         <width>%width%</width>
                         <height>%height%</height>
                   </marker> ");
			strXML.Replace("%name%", this.getAttList().getInstanceName());
			strXML.Replace("%x%", Attributs.getLocation().X.ToString());
			strXML.Replace("%y%", Attributs.getLocation().Y.ToString());
			strXML.Replace("%width%", Attributs.getSize().Width.ToString());
			strXML.Replace("%height%", Attributs.getSize().Height.ToString());
			return strXML.ToString();
		}

		//load đoạn xml từ chỗ <object>...
		public override void loadfromXML(string strXML, string path)
		{
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(strXML);
						
			Attributs.setLocation(new Point(int.Parse(xml.SelectSingleNode("//x").InnerText),int.Parse(xml.SelectSingleNode("//y").InnerText)));
			Attributs.setSize(new Size(int.Parse(xml.SelectSingleNode("//width").InnerText),int.Parse(xml.SelectSingleNode("//height").InnerText)));
			Attributs.setInstanceName(xml.SelectSingleNode("//name").InnerText);
		}

		public override INewsPaperControl clone()
		{
			NewsPaperMarkerControl newControl = new NewsPaperMarkerControl();

			newControl.Attributs.setLocation(this.Attributs.getLocation());
			newControl.Attributs.setSize(this.Attributs.getSize());
			//((CMarkerAtt)newControl.Attributs).MarkerURL = ((CMarkerAtt)this.Attributs).MarkerURL;

			//try
			//{
			//    Bitmap img = new Bitmap(((CMarkerAtt)newControl.Attributs).MarkerURL);
			//    newControl.ImgBackground = img;
			//    newControl.Ctrl.BackgroundImage = ImgBackground;
			//}
			//catch { };

			return newControl;
		}

		public override void copy(INewsPaperControl control)
		{
			Attributs.setLocation(control.getAttList().getLocation());
			Attributs.setSize(control.getAttList().getSize());
			//((CMarkerAtt)(Attributs)).MarkerURL = ((CMarkerAtt)(control.getAttList())).MarkerURL;
			//try
			//{
			//    Bitmap img = new Bitmap(((CMarkerAtt)(Attributs)).MarkerURL);
			//    ImgBackground = img;
			//    Ctrl.BackgroundImage = ImgBackground;
			//}
			//catch { 
			//    loadBackgroundAndICO();
			//    Ctrl.BackgroundImage = ImgBackground;
			//};
		}
	}
}
