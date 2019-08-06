using System;
using System.Collections.Generic;
using System.Text;
using NewsPaperControl;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace NewsPaper3DObject
{
    public class NewsPaper3DObjectControl : NewsPaperControlBase
    {
        public NewsPaper3DObjectControl()
            : base()
        {
            Attributs = new C3DObjectAtt();
			StrDisplayName = "3D Object";
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

                System.IO.Stream file = thisExe.GetManifestResourceStream("NewsPaper3DObject." + StrName + "-ICO.png");
                Bitmap bmp = new Bitmap(file);
                ImgIcon = bmp;

                file = thisExe.GetManifestResourceStream("NewsPaper3DObject." + StrName + ".png");
                bmp = new Bitmap(file);
                ImgBackground = bmp;
            }
            catch { };
        }

        public override void setControlName()
        {
            StrName = "NewsPaper3DObject";
        }

        public override void updateAttributeValue(string strAttName, object objAttValue)
        {
            updateAttributeValueBase(strAttName, objAttValue);
            switch (strAttName)
            {
                case "TextureURL":
                    {
                        try
                        {
                            ((C3DObjectAtt)Attributs).TextureURL = objAttValue.ToString();
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
                         <uri>%uri_md2%</uri>
                         <uri>%uri_texture%</uri>
                         <x>%x%</x>
                         <y>%y%</y>
                         <width>%width%</width>
                         <height>%height%</height>
						 <marker>%marker%</marker>
                   </object> ");
            strXML.Replace("%type%", this.getName());
            strXML.Replace("%uri_md2%", ((C3DObjectAtt)(Attributs)).Md2URL);
            strXML.Replace("%uri_texture%", ((C3DObjectAtt)(Attributs)).TextureURL);
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
            XmlNodeList n = xml.SelectNodes("//uri");
			string strTemp = n[0].InnerText;
			if (strTemp != "(none)")
			{
				((C3DObjectAtt)(Attributs)).Md2URL = path + strTemp;
			}
			else
			{
				((C3DObjectAtt)(Attributs)).Md2URL = "(none)";
			}
			strTemp = n[1].InnerText;
			if (strTemp != "(none")
			{
				((C3DObjectAtt)(Attributs)).TextureURL = path + strTemp;
			}
			else
			{
				((C3DObjectAtt)(Attributs)).TextureURL = "(none)";
			}
            Attributs.setLocation(new Point(int.Parse(xml.SelectSingleNode("//x").InnerText), int.Parse(xml.SelectSingleNode("//y").InnerText)));
            Attributs.setSize(new Size(int.Parse(xml.SelectSingleNode("//width").InnerText), int.Parse(xml.SelectSingleNode("//height").InnerText)));
			Attributs.setMarkerName(xml.SelectSingleNode("//marker").InnerText);
        }

        public override INewsPaperControl clone()
        {
            NewsPaper3DObjectControl newControl = new NewsPaper3DObjectControl();

            newControl.Attributs.setLocation(this.Attributs.getLocation());
            newControl.Attributs.setSize(this.Attributs.getSize());
            ((C3DObjectAtt)newControl.Attributs).Md2URL = ((C3DObjectAtt)this.Attributs).Md2URL;
            ((C3DObjectAtt)newControl.Attributs).TextureURL = ((C3DObjectAtt)this.Attributs).TextureURL;
            try
            {
                Bitmap img = new Bitmap(((C3DObjectAtt)newControl.Attributs).TextureURL);
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
            ((C3DObjectAtt)(Attributs)).Md2URL = ((C3DObjectAtt)(control.getAttList())).Md2URL;
            ((C3DObjectAtt)(Attributs)).TextureURL = ((C3DObjectAtt)(control.getAttList())).TextureURL;
            try
            {
                Bitmap img = new Bitmap(((C3DObjectAtt)(Attributs)).TextureURL);
                ImgBackground = img;
                Ctrl.BackgroundImage = ImgBackground;
            }
            catch { };
        }
    }
}
