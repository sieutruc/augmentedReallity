using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NewsPaperControl;
using System.Reflection;
using System.Drawing;

namespace NewspaperDesigner
{
    public class CNewspaper
    {
        public static CNewspaper readXML(string filename)
        {			
			CNewspaper news = new CNewspaper();
			news.Directory = filename.Substring(0, filename.LastIndexOf("\\")); 

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(filename);
			
			news.KindOfNews = xmlDoc.SelectSingleNode("//title").InnerText;
			news.Datetime = xmlDoc.SelectSingleNode("//date").InnerText;
			news.NumPages = int.Parse(xmlDoc.SelectSingleNode("//numpages").InnerText);
			news.Number = int.Parse(xmlDoc.SelectSingleNode("//number").InnerText);
			foreach (XmlNode n in xmlDoc.SelectNodes("//page"))
			{
				CPage p = new CPage();
				p.HeightP = int.Parse(n.Attributes["height"].Value);
				p.WidthP = int.Parse(n.Attributes["width"].Value);
				string strBackURL = n.Attributes["backgroundURL"].Value;
				if(strBackURL == "")
				{
					p.StrBackground = "";
				}else{
					p.StrBackground = news.Directory + "\\" + strBackURL;
				}

				foreach(XmlNode xmlNodeObject in n.SelectNodes("object"))
				{
					string strControlName = xmlNodeObject.SelectSingleNode("type").InnerText;
					for(int i = 0; i < MainProgram.m_MyControlList.LstNewsPaperControl.Count;i++)
					{
						if (MainProgram.m_MyControlList.LstNewsPaperControl[i].getName() == strControlName)
						{
							Assembly Asm = Assembly.LoadFile(MainProgram.m_MyControlList.LstNewsPaperControl[i].getDllPath());
							foreach (Type AsmType in Asm.GetTypes())
							{
								if (AsmType.GetInterface("INewsPaperControl") != null && AsmType.Name != "NewsPaperControlBase")
								{
									NewsPaperControlBase Plugin = (NewsPaperControlBase)Activator.CreateInstance(AsmType);
									Plugin.loadfromXML(xmlNodeObject.OuterXml, news.Directory + "\\");
									p.lstNewsPaperControl.Add(Plugin);
									break;
								}
							}
							break;
						}
					}
				}

				foreach (XmlNode xmlNodeObject in n.SelectNodes("marker"))
				{
					string strControlName = "NewsPaperMarker";
					for (int i = 0; i < MainProgram.m_MyControlList.LstNewsPaperControl.Count; i++)
					{
						if (MainProgram.m_MyControlList.LstNewsPaperControl[i].getName() == strControlName)
						{
							Assembly Asm = Assembly.LoadFile(MainProgram.m_MyControlList.LstNewsPaperControl[i].getDllPath());
							foreach (Type AsmType in Asm.GetTypes())
							{
								if (AsmType.GetInterface("INewsPaperControl") != null && AsmType.Name != "NewsPaperControlBase")
								{
									NewsPaperControlBase Plugin = (NewsPaperControlBase)Activator.CreateInstance(AsmType);
									Plugin.loadfromXML(xmlNodeObject.OuterXml, news.Directory + "\\");
									p.lstNewsPaperControl.Add(Plugin);
									break;
								}
							}
							break;
						}
					}
				}

				news.LPages.Add(p);
			}

			foreach (CPage page in news.LPages)
			{
				List<INewsPaperControl> lstMarker = new List<INewsPaperControl>();
				List<INewsPaperControl> lstControl = new List<INewsPaperControl>();
				foreach (INewsPaperControl c in page.lstNewsPaperControl)
				{
					if (c.getName() == "NewsPaperMarker")
					{
						lstMarker.Add(c);
					}
				}
				ReCalculteRealLocation(page, lstMarker);
			}

			return news;
        }

		private static void ReCalculteRealLocation(CPage page, List<INewsPaperControl> lstMarker)
		{
			foreach (INewsPaperControl c in page.lstNewsPaperControl)
			{
				if (c.getName() != "NewsPaperMarker")
				{
					foreach (INewsPaperControl marker in lstMarker)
					{
						if (marker.getAttList().getInstanceName() == c.getAttList().getMarkerName())
						{
							Size SizeOfMarker = marker.getAttList().getSize();
							Point LocationOfMarker = marker.getAttList().getLocation();
							Point MedianLocationOfMarker = new Point(LocationOfMarker.X + SizeOfMarker.Width / 2, LocationOfMarker.Y + SizeOfMarker.Height / 2);

							Point NewLocationOfControl = new Point(c.getAttList().getLocation().X + MedianLocationOfMarker.X, (MedianLocationOfMarker.Y - c.getAttList().getLocation().Y));
							c.getAttList().setLocation(NewLocationOfControl);
							break;
						}
					}
				}
			}
		}

		

        public static bool writeXML(CNewspaper news,string filename)
        {
			List<INewsPaperControl> lstMarker = new List<INewsPaperControl>();
			foreach (CPage page in news.LPages)
			{
				List<INewsPaperControl> lstControl = new List<INewsPaperControl>();
				foreach (INewsPaperControl c in page.lstNewsPaperControl)
				{
					if (c.getName() == "NewsPaperMarker")
					{
						lstMarker.Add(c);
					}
				}
				foreach (INewsPaperControl c in page.lstNewsPaperControl)
				{
					if (c.getName() != "NewsPaperMarker")
					{
						foreach (INewsPaperControl marker in lstMarker)
						{
							if (marker.getAttList().getInstanceName() == c.getAttList().getMarkerName())
							{
								Size SizeOfMarker = marker.getAttList().getSize();
								Point LocationOfMarker = marker.getAttList().getLocation();
								Point MedianLocationOfMarker = new Point(LocationOfMarker.X+SizeOfMarker.Width/2,LocationOfMarker.Y+SizeOfMarker.Height/2);

								Point NewLocationOfControl = new Point(c.getAttList().getLocation().X - MedianLocationOfMarker.X, MedianLocationOfMarker.Y - c.getAttList().getLocation().Y);
								c.getAttList().setLocation(NewLocationOfControl);
								break;
							}
						}
					}
				}
			}

			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				XmlElement xmlEleRoot = xmlDoc.CreateElement("newspaper");
				XmlElement xmlElePages = xmlDoc.CreateElement("pages");

				XmlElement xmlEleTitle = xmlDoc.CreateElement("title");
				xmlEleTitle.InnerText = news.KindOfNews;
				xmlEleRoot.AppendChild(xmlEleTitle);

				XmlElement xmlEleDate = xmlDoc.CreateElement("date");
				xmlEleDate.InnerText = news.Datetime;
				xmlEleRoot.AppendChild(xmlEleDate);

				XmlElement xmlElePageNum = xmlDoc.CreateElement("numpages");
				xmlElePageNum.InnerText = news.numPages.ToString();
				xmlEleRoot.AppendChild(xmlElePageNum);

				XmlElement xmlEleNumber = xmlDoc.CreateElement("number");
				xmlEleNumber.InnerText = news.number.ToString();
				xmlEleRoot.AppendChild(xmlEleNumber);

				for (int i = 0; i < news.LPages.Count; i++)
				{
					XmlElement xmlElePage = xmlDoc.CreateElement("page");

					XmlAttribute xmlAttWidth = xmlDoc.CreateAttribute("width");
					xmlAttWidth.Value = news.LPages[i].WidthP.ToString();
					xmlElePage.Attributes.Append(xmlAttWidth);

					XmlAttribute xmlAttHeight = xmlDoc.CreateAttribute("height");
					xmlAttHeight.Value = news.LPages[i].HeightP.ToString();
					xmlElePage.Attributes.Append(xmlAttHeight);

					XmlAttribute xmlAttBackground = xmlDoc.CreateAttribute("backgroundURL");
					xmlAttBackground.Value = news.LPages[i].StrBackground;
					xmlElePage.Attributes.Append(xmlAttBackground);

					for (int j = 0; j < news.LPages[i].lstNewsPaperControl.Count; j++)
					{
						XmlDocument xmlDocPage = new XmlDocument();
						xmlDocPage.LoadXml(news.LPages[i].lstNewsPaperControl[j].toXMLString());

						XmlElement xmlEleObject = null;
						if (news.LPages[i].lstNewsPaperControl[j].getName() == "NewsPaperMarker")
						{
							xmlEleObject = xmlDoc.CreateElement("marker");
							foreach (XmlNode n in xmlDocPage.SelectNodes("marker//*"))
							{
								XmlElement xmlE = xmlDoc.CreateElement(n.Name);
								xmlE.InnerText = n.InnerText;
								xmlEleObject.AppendChild(xmlE);
							}
						}
						else
						{
							xmlEleObject = xmlDoc.CreateElement("object");
							foreach (XmlNode n in xmlDocPage.SelectNodes("object//*"))
							{
								XmlElement xmlE = xmlDoc.CreateElement(n.Name);
								xmlE.InnerText = n.InnerText;
								xmlEleObject.AppendChild(xmlE);
							}
						}												
						
						xmlElePage.AppendChild(xmlEleObject);
					}

					xmlElePages.AppendChild(xmlElePage);
				}

				xmlEleRoot.AppendChild(xmlElePages);
				xmlDoc.AppendChild(xmlEleRoot);

				xmlDoc.Save(filename);
			}
			catch { return false; }

			foreach (CPage page in news.LPages)
			{
				ReCalculteRealLocation(page, lstMarker);
			}
			return true;
        }


        string kindOfNews;
        int defaultH, defaultW;
		string directory;
		CBackgroundAtt attBgk;

		public CBackgroundAtt AttBgk
		{
			get { return attBgk; }
			set { attBgk = value; }
		}

		public string Directory
		{
			get { return directory; }
			set { directory = value; }
		}

        public int DefaultW
        {
            get { return defaultW; }
            set { defaultW = value; }
        }

        public int DefaultH
        {
            get { return defaultH; }
            set { defaultH = value; }
        }
        public string KindOfNews
        {
            get { return kindOfNews; }
            set { kindOfNews = value; }
        }
        int number;

        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        string datetime;

        public string Datetime
        {
            get { return datetime; }
            set { datetime = value; }
        }
        int numPages;

        public int NumPages
        {
            get { return numPages; }
            set { numPages = value; }
        }
        public CNewspaper()
        {
            kindOfNews = "Tuổi trẻ";
            number = 1;
            datetime = "";
            numPages = 1;
			defaultH = 405;
			defaultW = 276;
            LPages = new List<CPage>();
			attBgk = new CBackgroundAtt();
			
			initAttBgk();
        }
		public void initAttBgk()
		{
			attBgk = new CBackgroundAtt();
			attBgk.BackgrHeight = defaultH;
			attBgk.BackgrWidth = defaultW;
			attBgk.BackgroundURL = "";
		}
        public List<CPage> LPages;
    }

    public class CPage
    {
        public List<INewsPaperControl> lstNewsPaperControl;
        int width, height;
		string strBackgroundURL;

		public string StrBackground
		{
			get { return strBackgroundURL; }
			set { strBackgroundURL = value; }
		}

        public int HeightP
        {
            get { return height; }
            set { height = value; }
        }

        public int WidthP
        {
            get { return width; }
            set { width = value; }
        }
        public CPage()
        {
            lstNewsPaperControl = new List<INewsPaperControl>();
			//height = 479;
			//width = 276;
        }
    }

    public class CControl
    {
        string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public List<string> LUris;
        int xlocation;

        public int Xlocation
        {
            get { return xlocation; }
            set { xlocation = value; }
        }
        int ylocation;

        public int Ylocation
        {
            get { return ylocation; }
            set { ylocation = value; }
        }
        int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        public CControl()
        {
            xlocation = ylocation = width = height = 0;
            LUris = new List<string>();
        }
    }
}
