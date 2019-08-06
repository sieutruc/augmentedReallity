using System;
using System.Collections.Generic;
using System.Text;
using NewsPaperControl;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace NewsPaperPictureControl
{
	public class NewsPaperPictureControl:NewsPaperControlBase
	{
		public NewsPaperPictureControl():base()
		{
			Attributs = new CPictureAtt();
			StrDisplayName = "Image Object";
		}

		public override string getDisplayName()
		{
			return StrDisplayName;
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

		public override void loadBackgroundAndICO()
		{
			try
			{
				System.Reflection.Assembly thisExe;
				thisExe = System.Reflection.Assembly.GetExecutingAssembly();

				System.IO.Stream file = thisExe.GetManifestResourceStream("NewsPaperPicture." + StrName + "-ICO.png");
				Bitmap bmp = new Bitmap(file);
				ImgIcon = bmp;

				file = thisExe.GetManifestResourceStream("NewsPaperPicture." + StrName + ".png");
				bmp = new Bitmap(file);
				ImgBackground = bmp;
			}
			catch { };
		}

		public override void setControlName()
		{
			StrName = "NewsPaperPicture";
		}

		public override void updateAttributeValue(string strAttName, object objAttValue)
		{
			updateAttributeValueBase(strAttName, objAttValue);
			switch (strAttName)
			{
				case "PictureURL":
					{
						try
						{
							((CPictureAtt)Attributs).PictureURL = objAttValue.ToString();
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
			strXML.Replace("%uri%", ((CPictureAtt)(Attributs)).PictureURL);
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
			if (strTemp != "(none)")
			{
				((CPictureAtt)(Attributs)).PictureURL = path + strTemp;
			}
			else
			{
				((CPictureAtt)(Attributs)).PictureURL = "(none)";
			}
			Attributs.setLocation(new Point(int.Parse(xml.SelectSingleNode("//x").InnerText), int.Parse(xml.SelectSingleNode("//y").InnerText)));
			Attributs.setSize(new Size(int.Parse(xml.SelectSingleNode("//width").InnerText), int.Parse(xml.SelectSingleNode("//height").InnerText)));
			Attributs.setMarkerName(xml.SelectSingleNode("//marker").InnerText);

			try
			{
				Bitmap img = new Bitmap(((CPictureAtt)Attributs).PictureURL);
				this.ImgBackground = img;
				this.Ctrl.BackgroundImage = ImgBackground;
			}
			catch { };
		}

		public override INewsPaperControl clone()
		{
			NewsPaperPictureControl newControl = new NewsPaperPictureControl();

			newControl.Attributs.setLocation(this.Attributs.getLocation());
			newControl.Attributs.setSize(this.Attributs.getSize());
			((CPictureAtt)newControl.Attributs).PictureURL = ((CPictureAtt)this.Attributs).PictureURL;
			try
			{
				Bitmap img = new Bitmap(((CPictureAtt)newControl.Attributs).PictureURL);
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
			((CPictureAtt)(Attributs)).PictureURL = ((CPictureAtt)(control.getAttList())).PictureURL;
			try
			{
				if (((CPictureAtt)(Attributs)).PictureURL == "")
					loadBackgroundAndICO();
				Bitmap img = new Bitmap(((CPictureAtt)(Attributs)).PictureURL);
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
