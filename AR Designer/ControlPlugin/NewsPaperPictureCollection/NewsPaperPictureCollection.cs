using System;
using System.Collections.Generic;
using System.Text;
using NewsPaperControl;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace NewsPaperPictureCollectionControl
{
	public class NewsPaperPictureCollectionControl:NewsPaperControlBase
	{
		public NewsPaperPictureCollectionControl()
		{
			Attributs = new CPictureCollectionAtt();
			StrDisplayName = "Sequence of Images Object";
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

				System.IO.Stream file = thisExe.GetManifestResourceStream("NewsPaperPictureCollection." + StrName + "-ICO.png");
				System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(file);
				ImgIcon = bmp;

				file = thisExe.GetManifestResourceStream("NewsPaperPictureCollection." + StrName + ".png");
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
			Ctrl.MouseClick += new MouseEventHandler(Ctrl_MouseClick);

			base.Controls.Clear();
			base.Controls.Add(Ctrl);
			Ctrl.Bounds = base.Bounds;
			base.calculateEdges();
		}

		void Ctrl_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				if (((CPictureCollectionAtt)Attributs).PictureURLList.Count > 1)
				{
					int index = ++((CPictureCollectionAtt)Attributs).CurrentIndex;
					if (index > ((CPictureCollectionAtt)Attributs).PictureURLList.Count - 1)
					{
						index = 0;
						((CPictureCollectionAtt)Attributs).CurrentIndex = 0;
					}
					try
					{
						System.Drawing.Bitmap img = new System.Drawing.Bitmap(((CPictureCollectionAtt)Attributs).PictureURLList[index].PictureURL.ToString());
						ImgBackground = img;
						Ctrl.BackgroundImage = ImgBackground;
					}
					catch { };
				}
			}
		}

		public override void setControlName()
		{
			StrName = "NewsPaperPictureCollection";
		}

		public override void updateAttributeValue(string strAttName, object objAttValue)
		{
			updateAttributeValueBase(strAttName, objAttValue);
			switch (strAttName)
			{
				case "PictureURL":
					{
						if (((CPictureCollectionAtt)Attributs).PictureURLList == null)
							((CPictureCollectionAtt)Attributs).PictureURLList = new List<PictureList>();
						try
						{
							System.Drawing.Bitmap img = new System.Drawing.Bitmap(objAttValue.ToString());
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
                         <type>%type%</type>");
			strXML.Replace("%type%", this.getName());
			foreach (PictureList s in ((CPictureCollectionAtt)(Attributs)).PictureURLList)
			{
				strXML.Append("<uri>"+ s.PictureURL+"</uri>");
			}
			if (((CPictureCollectionAtt)(Attributs)).PictureURLList.Count == 0)
				strXML.Append("<uri></uri>");
			strXML.Append(@"<x>%x%</x>
                         <y>%y%</y>
                         <width>%width%</width>
                         <height>%height%</height>
						 <marker>%marker%</marker>
                   </object> ");
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

			((CPictureCollectionAtt)(Attributs)).PictureURLList = new List<PictureList>();

			foreach (XmlNode n in xml.SelectNodes("//uri"))
			{
				PictureList p = new PictureList();
				string strTemp = n.InnerText;
				if(strTemp != "(none)")
				{
					p.PictureURL = path + strTemp;
				}else{
					p.PictureURL = "(none)";
				}
				((CPictureCollectionAtt)(Attributs)).PictureURLList.Add(p);
			}
			try
			{
				System.Drawing.Bitmap img = new System.Drawing.Bitmap(((CPictureCollectionAtt)Attributs).PictureURLList[0].PictureURL);
				this.ImgBackground = img;
				this.Ctrl.BackgroundImage = ImgBackground;
			}
			catch { };
			Attributs.setLocation(new Point(int.Parse(xml.SelectSingleNode("//x").InnerText), int.Parse(xml.SelectSingleNode("//y").InnerText)));
			Attributs.setSize(new Size(int.Parse(xml.SelectSingleNode("//width").InnerText), int.Parse(xml.SelectSingleNode("//height").InnerText)));
			Attributs.setMarkerName(xml.SelectSingleNode("//marker").InnerText);
		}

		public override INewsPaperControl clone()
		{
			NewsPaperPictureCollectionControl newControl = new NewsPaperPictureCollectionControl();

			newControl.Attributs.setLocation(this.Attributs.getLocation());
			newControl.Attributs.setSize(this.Attributs.getSize());
			foreach (PictureList p in ((CPictureCollectionAtt)this.Attributs).PictureURLList)
			{
				((CPictureCollectionAtt)newControl.Attributs).PictureURLList.Add(p);
			}

			try
			{
				System.Drawing.Bitmap img = new System.Drawing.Bitmap(((CPictureCollectionAtt)newControl.Attributs).PictureURLList[0].PictureURL);
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
			foreach (PictureList p in ((CPictureCollectionAtt)control.getAttList()).PictureURLList)
			{
				((CPictureCollectionAtt)Attributs).PictureURLList.Add(p);
			}

			try
			{
				System.Drawing.Bitmap img = new System.Drawing.Bitmap(((CPictureCollectionAtt)Attributs).PictureURLList[0].PictureURL);
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
