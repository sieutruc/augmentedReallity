using System;
using System.Collections.Generic;
using System.Text;
using NewsPaperControl;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;

namespace NewsPaperVideoControl
{
	public class NewsPaperVideoControl: NewsPaperControlBase
	{
		public NewsPaperVideoControl()
		{
			Attributs = new CVideoAtt();
			StrDisplayName = "Video Object";
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

				System.IO.Stream file = thisExe.GetManifestResourceStream("NewsPaperVideo." + StrName + "-ICO.png");
				System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(file);
				ImgIcon = bmp;

				file = thisExe.GetManifestResourceStream("NewsPaperVideo." + StrName + ".png");
				bmp = new System.Drawing.Bitmap(file);
				ImgBackground = bmp;
			}
			catch { };
		}

		public override void initCtrl()
		{
			Ctrl = new PictureBox();
			Ctrl.Name = "PictureBox";
			Ctrl.BackgroundImageLayout = ImageLayout.Stretch;
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
			StrName = "NewsPaperVideo";
		}

		public override void updateAttributeValue(string strAttName, object objAttValue)
		{
			updateAttributeValueBase(strAttName, objAttValue);
			switch (strAttName)
			{
				case "VideoURL":
					{
						try
						{
							((CVideoAtt)Attributs).VideoURL = objAttValue.ToString();
							Bitmap img = new Bitmap(objAttValue.ToString());
							ImgBackground = img;
							Ctrl.BackgroundImage = ImgBackground;
						}
						catch { };
						break;
					}
			}
		}

		public override string toXMLString()
		{
			StringBuilder strXML = new StringBuilder();
			strXML.Append(@"<object>
                         <type>%type%</type>
                         <uri>%uri%</uri>
                         <x>%x%</x>
                         <y>%y%</y>
                         <width>%width%</width>
                         <height>%height%</height>
						 <marker>%marker%</marker>
                   </object> ");
			strXML.Replace("%type%", this.getName());
			strXML.Replace("%uri%", ((CVideoAtt)(Attributs)).VideoURL);
			strXML.Replace("%x%", Attributs.getLocation().X.ToString());
			strXML.Replace("%y%", Attributs.getLocation().Y.ToString());
			strXML.Replace("%width%", Attributs.getSize().Width.ToString());
			strXML.Replace("%height%", Attributs.getSize().Height.ToString());
			strXML.Replace("%marker%", Attributs.getMarkerName());
			return strXML.ToString();
		}

		//load đoạn xml từ chỗ <object>...
		public override void loadfromXML(string strXML, string path)
		{
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(strXML);

			string strTemp = xml.SelectSingleNode("//uri").InnerText; 
			if(strTemp != "(none)")
			{
				((CVideoAtt)(Attributs)).VideoURL = path + strTemp;
			}else{
				((CVideoAtt)(Attributs)).VideoURL = "(none)";
			}
			Attributs.setLocation(new Point(int.Parse(xml.SelectSingleNode("//x").InnerText),int.Parse(xml.SelectSingleNode("//y").InnerText)));
			Attributs.setSize(new Size(int.Parse(xml.SelectSingleNode("//width").InnerText),int.Parse(xml.SelectSingleNode("//height").InnerText)));
			Attributs.setMarkerName(xml.SelectSingleNode("//marker").InnerText);
		}

		public override INewsPaperControl clone()
		{
			NewsPaperVideoControl newControl = new NewsPaperVideoControl();

			newControl.Attributs.setLocation(this.Attributs.getLocation());
			newControl.Attributs.setSize(this.Attributs.getSize());
			((CVideoAtt)newControl.Attributs).VideoURL = ((CVideoAtt)this.Attributs).VideoURL;

			try
			{
				Bitmap img = new Bitmap(((CVideoAtt)newControl.Attributs).VideoURL);
				newControl.ImgBackground = img;
				newControl.Ctrl.BackgroundImage = ImgBackground;
			}
			catch { };

			return newControl;
		}

		public override void copy(INewsPaperControl control)
		{
			Attributs.setLocation(control.getAttList().getLocation());
			Attributs.setSize(control.getAttList().getSize());
			((CVideoAtt)(Attributs)).VideoURL = ((CVideoAtt)(control.getAttList())).VideoURL;
			try
			{
				Bitmap img = new Bitmap(((CVideoAtt)(Attributs)).VideoURL);
				ImgBackground = img;
				Ctrl.BackgroundImage = ImgBackground;
			}
			catch { 
				loadBackgroundAndICO();
				Ctrl.BackgroundImage = ImgBackground;
			};
		}
	}
}
