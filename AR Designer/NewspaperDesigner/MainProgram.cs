using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using NewsPaperControl;
using System.Xml;

namespace NewspaperDesigner
{

	public partial class MainProgram : Form
	{
		INewsPaperControl currentNewsPaperControl;
		public static NewsPaperControlManager m_MyControlList;
		List<INewsPaperControl> lstPreState;
		List<INewsPaperControl> lstNextState;
		List<INewsPaperControl> lstCurrentPointerControl;
		List<INewsPaperControl> lstNextPointerControl;
		Image imgAvaibleUndo;
		Image imgUnAvaibleUndo;
		Image imgAvaibleRedo;
		Image imgUnAvaibleRedo;
		CNewspaper currentNews;
		string filename = "";
		int curPageNum = 0;
		//CBackgroundAtt attBgk;
		int zoomDegree = 1;
		bool bIsZoom = true;
		List<INewsPaperControl> lstMarker;
		int iObjNumber = 0;

		public MainProgram()
		{
			InitializeComponent();
			loadNewsPaperBackgr();
			loadControlPlugin();
			loadToolBox();
			LoadImg();

			//init undo redo function
			reInitZoomUndoRedo();
			
			cbcPages.ComboBox.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			rbMenu.SelectedTab = rbTab1;

			MyCollectionEditor.MyCollectionEditor.MyPropertyValueChanged += new MyCollectionEditor.MyCollectionEditor.MyPropertyValueChangedEventHandler(MyCollectionEditor_MyPropertyValueChanged);

			lstMarker = new List<INewsPaperControl>();
		}
		
		//void initNewsPaperAttr()
		//{
		//    //
		//    currentNews = new CNewspaper();
		//    curPageNum = 0;
		//    attBgk = new CBackgroundAtt();
		//    attBgk.BackgrHeight = panelInnerScreen.Height;
		//    attBgk.BackgrWidth = panelInnerScreen.Width;
		//}

		void LoadImg()
		{
			System.Reflection.Assembly thisExe;
			thisExe = System.Reflection.Assembly.GetExecutingAssembly();

			System.IO.Stream file = thisExe.GetManifestResourceStream("NewspaperDesigner.edit_undo_.png");
			System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(file);
			imgUnAvaibleUndo = bmp;

			file = thisExe.GetManifestResourceStream("NewspaperDesigner.edit_undo.png");
			bmp = new System.Drawing.Bitmap(file);
			imgAvaibleUndo = bmp;

			file = thisExe.GetManifestResourceStream("NewspaperDesigner.edit_redo.png");
			bmp = new System.Drawing.Bitmap(file);
			imgAvaibleRedo = bmp;

			file = thisExe.GetManifestResourceStream("NewspaperDesigner.edit_redo_.png");
			bmp = new System.Drawing.Bitmap(file);
			imgUnAvaibleRedo = bmp;
		}

		void MyCollectionEditor_MyPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
			currentNewsPaperControl.updateAttributeValue(e.ChangedItem.PropertyDescriptor.Name, e.ChangedItem.Value);
			updateControlAccordingToAttribute();
			saveControlTraceAndSetRealLocationSize();
		}

		bool loadControlPlugin()
		{
			m_MyControlList = new NewsPaperControlManager();
			if (Directory.GetFiles(Path.Combine(Application.StartupPath, "Plugins"), "*.dll").Length > 0)
			{
				foreach (string Filename in Directory.GetFiles(Path.Combine(Application.StartupPath, "Plugins"), "*.dll"))
				{
					Assembly Asm = Assembly.LoadFile(Filename);
					foreach (Type AsmType in Asm.GetTypes())
					{
						if (AsmType.GetInterface("INewsPaperControl") != null && AsmType.Name != "NewsPaperControlBase")
						{
							NewsPaperControlBase Plugin = (NewsPaperControlBase)Activator.CreateInstance(AsmType);
							Plugin.setDllPath(Filename);
							m_MyControlList.LstNewsPaperControl.Add(Plugin);
						}
					}
				}
				return true;
			}
			return false;
		}

		Janus.Windows.ButtonBar.ButtonBarGroup buttonBar1_Group_0;
		public void loadToolBox()
		{
			//Load group
			buttonBar1_Group_0 = new Janus.Windows.ButtonBar.ButtonBarGroup();
			buttonBar1_Group_0.Key = "MediaControlGroup";
			buttonBar1_Group_0.Text = "Media Object";
			this.buttonBar1.Groups.AddRange(new Janus.Windows.ButtonBar.ButtonBarGroup[] {
            buttonBar1_Group_0});

			//Load toolbox
			foreach (INewsPaperControl item in m_MyControlList.LstNewsPaperControl)
			{
				Janus.Windows.ButtonBar.ButtonBarItem buttonBarItem = new Janus.Windows.ButtonBar.ButtonBarItem();
				buttonBarItem.Image = item.getICO();
				buttonBarItem.Text = item.getDisplayName();
				buttonBarItem.Tag = item;
				buttonBarItem.Enabled = false;
				buttonBarItem.Click += new Janus.Windows.ButtonBar.ItemEventHandler(buttonBarItem_Click);
				buttonBar1_Group_0.Items.AddRange(new Janus.Windows.ButtonBar.ButtonBarItem[] {
                buttonBarItem});
			}
		}

		void buttonBarItem_Click(object sender, Janus.Windows.ButtonBar.ItemEventArgs e)
		{
			INewsPaperControl item = (INewsPaperControl)((Janus.Windows.ButtonBar.ButtonBarItem)(sender)).Tag;
			if (Directory.GetFiles(Path.Combine(Application.StartupPath, "Plugins"), "*.dll").Length > 0)
			{
				string Filename = Path.Combine(Application.StartupPath, "Plugins") + @"\" + item.getName() + ".dll";
				Assembly Asm = Assembly.LoadFile(Filename);
				foreach (Type AsmType in Asm.GetTypes())
				{
					if (AsmType.GetInterface("INewsPaperControl") != null && AsmType.Name != "NewsPaperControlBase")
					{
						NewsPaperControlBase Plugin = (NewsPaperControlBase)Activator.CreateInstance(AsmType);
						currentNewsPaperControl = Plugin;
						Point p = new Point(currentNewsPaperControl.getAttList().getLocation().X * zoomDegree, currentNewsPaperControl.getAttList().getLocation().Y * zoomDegree);
						Size s = new System.Drawing.Size(currentNewsPaperControl.getAttList().getSize().Width * zoomDegree, currentNewsPaperControl.getAttList().getSize().Height * zoomDegree);
						Plugin.getCtrl().Location = p;
						Plugin.getCtrl().Size = s;

						iObjNumber++;
						Plugin.getAttList().setInstanceName("Obj" + iObjNumber.ToString());

						//Xử lý marker
						if (Plugin.getName() == "NewsPaperMarker")
						{
							lstMarker.Add(Plugin);
						}
						MarkerList.lstMarkerList = new List<string>();
						foreach (INewsPaperControl c in lstMarker)
						{
							MarkerList.lstMarkerList.Add(c.getAttList().getInstanceName());
						}
						break;
					}
				}
			}

			((Control)(currentNewsPaperControl)).MouseDown += new MouseEventHandler(MainProgram_MouseDown);
			((Control)(currentNewsPaperControl)).MouseUp += new MouseEventHandler(MainProgram_MouseUp);
			panelInnerScreen.Controls.Add((Control)currentNewsPaperControl);
			saveControlTraceAndSetRealLocationSize();
		}

		void MainProgram_MouseUp(object sender, MouseEventArgs e)
		{
			currentNewsPaperControl = (NewsPaperControlBase)sender;		
			if (currentNewsPaperControl.getAttList().getLocation() != currentNewsPaperControl.getCtrl().Location
				|| currentNewsPaperControl.getAttList().getSize() != currentNewsPaperControl.getCtrl().Size)
				saveControlTraceAndSetRealLocationSize();	
			loadProperties();
			btnClone.Enabled = true;
			//MessageBox.Show(currentNewsPaperControl.getAttList().getSize().Height.ToString());
		}

		void MainProgram_MouseDown(object sender, MouseEventArgs e)
		{
			currentNewsPaperControl = (NewsPaperControlBase)sender;

			btnDelete.Enabled = true;
			btnClone.Enabled = true;
			if (currentNewsPaperControl.getName() == "NewsPaperMarker")
			{
				MarkerList.lstMarkerList = new List<string>();
			}
			else
			{
				MarkerList.lstMarkerList = new List<string>();
				foreach (INewsPaperControl c in lstMarker)
				{
					MarkerList.lstMarkerList.Add(c.getAttList().getInstanceName());
				}
			}

			lbObjectName.Text = currentNewsPaperControl.getDisplayName();
			loadProperties();
		}


		//load bacground
		void loadNewsPaperBackgr()
		{
			//Stream s = this.GetType().Assembly.GetManifestResourceStream("NewspaperDesigner.frame.png");
			//Bitmap bmp = new Bitmap(s);
			//panelInnerScreen.BackgroundImage = bmp;
			//attBgk = new CBackgroundAtt();
			panelInnerScreen.BackColor = Color.White;
		}

		public void loadProperties()
		{
			//currentNewsPaperControl.getAttList().setZoomCoefficient(zoomDegree);
			//currentNewsPaperControl.setZoomCoefficient(zoomDegree);
			propertyGrid1.SelectedObject = currentNewsPaperControl.getAttList();
		}

		private void updateControlAccordingToAttribute()
		{
			Size s = currentNewsPaperControl.getAttList().getSize();
			Point p = currentNewsPaperControl.getAttList().getLocation();

			//if (bIsZoom)
			//{
				p = new Point(p.X * zoomDegree, p.Y * zoomDegree);
				s = new System.Drawing.Size(s.Width * zoomDegree, s.Height* zoomDegree);
			//}
			//else
			//{
			//    p = new Point(p.X / zoomDegree, p.Y / zoomDegree);
			//    s = new System.Drawing.Size(s.Width / zoomDegree, s.Height / zoomDegree);
			//}

			currentNewsPaperControl.getCtrl().Size = s;
			currentNewsPaperControl.getCtrl().Location = p;
		}

		private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			//MessageBox.Show(e.ChangedItem.PropertyDescriptor.Name);
			if (currentNewsPaperControl != null)
			{
				currentNewsPaperControl.updateAttributeValue(e.ChangedItem.PropertyDescriptor.Name, e.ChangedItem.Value);
				updateControlAccordingToAttribute();
				saveControlTraceAndSetRealLocationSize();
			}
			if (e.ChangedItem.Label.Contains("Instance Name") && currentNewsPaperControl.getName() == "NewsPaperMarker")
			{
				try
				{
					MarkerList.lstMarkerList.Remove(e.OldValue.ToString());
					MarkerList.lstMarkerList.Add(e.ChangedItem.Value.ToString());

				}
				catch (Exception exp) { MessageBox.Show("Select image file, please!"); }
			}
			if (e.ChangedItem.Label.Contains("BackgroundURL"))
			{
				try
				{
					Image img = Image.FromFile(e.ChangedItem.Value.ToString(), true);
					currentNews.AttBgk.BackgroundURL = e.ChangedItem.Value.ToString();
					panelInnerScreen.BackgroundImage = img;
				}
				catch (Exception exp) { MessageBox.Show("Select image file, please!"); }
			}
			if (e.ChangedItem.Label.Contains("Background Width"))
			{
				try
				{
					panelInnerScreen.Width = int.Parse(e.ChangedItem.Value.ToString());
					currentNews.AttBgk.BackgrWidth = int.Parse(e.ChangedItem.Value.ToString());
					//int xLocation = (panelMidScreen.Width - panelInnerScreen.Width) / 2;
					//panelInnerScreen.Location = new Point(xLocation, panelInnerScreen.Location.Y);
				}
				catch { };
			}

			if (e.ChangedItem.Label.Contains("Background Height"))
			{
				try
				{
					panelInnerScreen.Height = int.Parse(e.ChangedItem.Value.ToString());
					currentNews.AttBgk.BackgrHeight = int.Parse(e.ChangedItem.Value.ToString());
					//int xLocation = (uiPanelMidScreen.Width - uiMidPanel.Width) / 2;
					//uiMidPanel.Location = new Point(xLocation, uiMidPanel.Location.Y);
				}
				catch { };
			}
		}

		private void saveControlTraceAndSetRealLocationSize()
		{
			Point p = currentNewsPaperControl.getCtrl().Location;
			Size s = currentNewsPaperControl.getCtrl().Size;

			if (bIsZoom)
			{
				p = new Point(p.X / zoomDegree, p.Y / zoomDegree);
				s = new System.Drawing.Size(s.Width / zoomDegree, s.Height / zoomDegree);
			}
			else
			{
				p = new Point(p.X * zoomDegree, p.Y * zoomDegree);
				s = new System.Drawing.Size(s.Width * zoomDegree, s.Height * zoomDegree);
			}
			currentNewsPaperControl.getAttList().setLocation(p);
			currentNewsPaperControl.getAttList().setSize(s);
			//currentNewsPaperControl.setZoomCoefficient(zoomDegree);
			lstPreState.Add(currentNewsPaperControl.clone());
			lstCurrentPointerControl.Add(currentNewsPaperControl);
			checkUndoButtonState();
		}

		private void uiPanelInner_Click(object sender, EventArgs e)
		{
			if (currentNews != null)
			{
				propertyGrid1.SelectedObject = currentNews.AttBgk;
				btnClone.Enabled = false;
			}
			lbObjectName.Text = "(Newspaper Background)";
			btnDelete.Enabled = false;
			btnClone.Enabled = false;
		}

		private void uiPanelMidScreen_SizeChanged(object sender, EventArgs e)
		{
			initPage();
		}

		private void initPage()
		{
			int xLocation = ((panelMidScreen.Width - panelInnerScreen.Width-20) / 2);
			int yLocation = panelInnerBounder.Location.Y;
			if (xLocation < 0)
				xLocation = 0;
			if (yLocation < 0)
				yLocation = 0;
			panelInnerBounder.Location = new Point(xLocation, yLocation);

			int width = panelInnerScreen.Size.Width;
			int height = panelInnerScreen.Size.Height;
			if (width > panelMidScreen.Width-20)
				width = panelMidScreen.Width-20;
			if (height > panelMidScreen.Height-20)
				height = panelMidScreen.Height-20;
			panelInnerBounder.Size = new Size(width+20, height+20);
						
			//MessageBox.Show(panelInnerScreen.Location.X.ToString() + "/" +panelInnerScreen.Location.Y.ToString());
			//panelInnerScreen.Location = new Point(0,0);
			
		}

		void EnableButtonAfterCreateOrNew()
		{
			btnSavePage.Enabled = true;
			btnNewPage.Enabled = true;
			btnSave.Enabled = true;
			btnSaveAs.Enabled = true;
		}

		private void btnNew_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			btnNewPage.Enabled = true;
			EnableButtonAfterCreateOrNew();
			CNewspaper news = new CNewspaper();
			frmNewNewspaper frmNew = new frmNewNewspaper();
			frmNew.ShowDialog();
			if (frmNewNewspaper.news != null)
			{
				news = frmNewNewspaper.news;

				propertyGrid1.Enabled = true;
				currentNews = news;
				curPageNum = 1;
				filename = "";
				panelInnerScreen.Controls.Clear();
				cbcPages.ComboBox.Items.Clear();
				for (int i = 0; i < currentNews.NumPages; i++)
				{
					cbcPages.ComboBox.Items.Add("Page " + (i + 1));
					CPage p = new CPage();
                    p.HeightP = news.DefaultH;
                    p.WidthP = news.DefaultW;
					currentNews.LPages.Add(p);
				}				
				currentNews.AttBgk.BackgrHeight = currentNews.LPages[0].HeightP;
				currentNews.AttBgk.BackgrWidth = currentNews.LPages[0].WidthP;
				this.panelInnerScreen.BackgroundImage = null;
				this.panelInnerScreen.BackColor = Color.White;
				this.panelInnerScreen.Height = currentNews.AttBgk.BackgrHeight;
				this.panelInnerScreen.Width = currentNews.AttBgk.BackgrWidth;
				this.propertyGrid1.SelectedObject = currentNews.AttBgk;

				enableToolBar();
				btnZoomIn.Enabled = true;
				cbcPages.ComboBox.SelectedIndex = 0;

				currentNews.Directory = Directory.GetCurrentDirectory() +  @"\" + currentNews.KindOfNews+"-"+currentNews.Number.ToString();
				Directory.CreateDirectory(currentNews.Directory);
				reInitZoomUndoRedo();
				lstMarker = new List<INewsPaperControl>();
				MarkerList.lstMarkerList = new List<string>();
			}
		}

		private void enableToolBar()
		{
			foreach (Janus.Windows.ButtonBar.ButtonBarItem item in buttonBar1_Group_0.Items)
			{
				item.Enabled = true;
			}
		}

		private void disableToolBar()
		{
			foreach (Janus.Windows.ButtonBar.ButtonBarItem item in buttonBar1_Group_0.Items)
			{
				item.Enabled = false;
			}
		}

		private void buttonCommand1_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			saveAs();

		}
		bool saveAs()
		{
			//SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			//saveFileDialog1.Filter = "XML File|*.xml";
			//saveFileDialog1.Title = "Save an XML File";
			//saveFileDialog1.ShowDialog();

			// If the file name is not an empty string open it for saving.
			frmFileName frm = new frmFileName();
			frm.ShowDialog();
			filename = currentNews.Directory + "\\" + frmFileName.fileName + ".xml";
			return saveToXMLFile();
		}

		private bool saveToXMLFile()
		{
			if (filename != "")
			{
				//Delete all files and folder in the current directory
				try
				{
					System.IO.DirectoryInfo directory = new DirectoryInfo(currentNews.Directory);
					foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
					foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) directory.Delete(true);
				}
				catch { };

				CNewspaper.writeXML(currentNews,  filename);

				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(filename);

				foreach(XmlNode n in xmlDoc.SelectNodes("//object/marker"))
				{
					if (n.InnerText == null || n.InnerText == "")
					{
						MessageBox.Show("Some objects lack Marker element");
						return false;
					}
				}

				foreach (XmlNode n in xmlDoc.SelectNodes("//uri"))
				{
					try
					{
						string strFileName = n.InnerText.Substring(n.InnerText.LastIndexOf("\\") + 1);
						string strOldFileName = n.InnerText;
						n.InnerText = strFileName;
						System.IO.File.Copy(strOldFileName, currentNews.Directory + "\\" + strFileName);
						//n.InnerText = currentNews.Directory + "\\" + strFileName;
					}
					catch { };
				}

				foreach(XmlNode n in xmlDoc.SelectNodes("//page"))
				{
					try
					{
						string str = n.Attributes["backgroundURL"].Value;
						str = str.Substring(str.LastIndexOf("\\") + 1);
						string strOldFileName = n.Attributes["backgroundURL"].Value;
						n.Attributes["backgroundURL"].Value = str;
						System.IO.File.Copy(strOldFileName, currentNews.Directory + "\\" + str);
						//n.Attributes["backgroundURL"].Value = currentNews.Directory + "\\" + str;
						
					}
					catch { };
				}

				xmlDoc.Save( filename);

				System.Diagnostics.Process prc = new System.Diagnostics.Process();
				prc.StartInfo.FileName = currentNews.Directory;
				prc.Start();

				return true;
			}
			return false;
		}
		private void btnLoad_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{			
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Xml Files (.xml)|*.xml";
			ofd.FilterIndex = 1;
			DialogResult userClickedOK = ofd.ShowDialog();

			// Process input if the user clicked OK.
			if (userClickedOK == DialogResult.OK)
			{
				btnNewPage.Enabled = true;
				EnableButtonAfterCreateOrNew();
				iObjNumber = 0;

				// Open the selected file to read.
				currentNews = CNewspaper.readXML(ofd.FileName);
				if (currentNews == null)
				{
					MessageBox.Show("Please open another XML file", "Open file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				try
				{
					if (currentNews.LPages.Count > 0)
					{
						//reInitZoomUndoRedo();
						curPageNum = 1;
						filename = ofd.FileName;
						currentNews.Directory = filename.Substring(0, filename.LastIndexOf("\\"));
						loadPaper(curPageNum);
						cbcPages.ComboBox.Items.Clear();
						for (int i = 0; i < currentNews.NumPages; i++)
							cbcPages.ComboBox.Items.Add("Page " + (i + 1));
						cbcPages.ComboBox.SelectedIndex = 0;
						
						propertyGrid1.Enabled = true;
						//cbcPages.ComboBox.SelectedIndexChanged += new EventHandler(cbcPages_ComboBox_SelectedIndexChanged);
						btnZoomIn.Enabled = true;
						enableToolBar();
					}
				}
				catch (Exception exp)
				{
					MessageBox.Show("Please open another XML file", "Open file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			loadPaper(cbcPages.ComboBox.SelectedIndex);
			curPageNum = cbcPages.ComboBox.SelectedIndex;
		}

		private void dropDownCommand1_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			this.Close();
		}

		private void btcNext_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			if (curPageNum <= (currentNews.NumPages))
			{
				//loadPaper(++curPageNum);
				//++curPageNum; ;
				cbcPages.ComboBox.SelectedIndex = curPageNum;
			}
			checkNextBackBtn();
		}
		private void btcBack_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			if (curPageNum > 1)
			{
				//loadPaper(--curPageNum);
				//--curPageNum;
				cbcPages.ComboBox.SelectedIndex = curPageNum-2;
			}
			checkNextBackBtn();
		}

		void checkNextBackBtn()
		{
			if (curPageNum >= currentNews.NumPages)
				btnNext.Enabled = false;
			else
				btnNext.Enabled = true;

			if (curPageNum > 1 && curPageNum <= currentNews.NumPages)
				btnBack.Enabled = true;
			else
				btnBack.Enabled = false;
		}

		private void loadPaper(int pageNum)
		{
			pageNum--;
			this.panelInnerScreen.Controls.Clear();
			reInitZoomUndoRedo();
			CPage p = currentNews.LPages[pageNum];
			currentNews.AttBgk.BackgrHeight = p.HeightP;
			currentNews.AttBgk.BackgrWidth = p.WidthP;
			currentNews.AttBgk.BackgroundURL = p.StrBackground;
			this.panelInnerScreen.Size = new Size(currentNews.AttBgk.BackgrWidth, currentNews.AttBgk.BackgrHeight);
			try
			{
				Bitmap img = new Bitmap(currentNews.AttBgk.BackgroundURL);
				this.panelInnerScreen.BackgroundImage = img;
			}
			catch {
				this.panelInnerScreen.BackgroundImage = null;
				this.panelInnerScreen.BackColor = Color.White;
			};
			lstMarker = new List<INewsPaperControl>();
			MarkerList.lstMarkerList = new List<string>();
			for (int i = 0; i < p.lstNewsPaperControl.Count; i++)
			{
				currentNewsPaperControl = p.lstNewsPaperControl[i];

				//Xử lý marker				
				if (currentNewsPaperControl.getName() == "NewsPaperMarker")
				{
					lstMarker.Add(currentNewsPaperControl);
				}
				
				foreach (INewsPaperControl c in lstMarker)
				{
					MarkerList.lstMarkerList.Add(c.getAttList().getInstanceName());
				}

				currentNewsPaperControl.getCtrl().MouseDown+=new MouseEventHandler(MainProgram_MouseDown);
				currentNewsPaperControl.getCtrl().MouseUp += new MouseEventHandler(MainProgram_MouseUp);
				panelInnerScreen.Controls.Add(currentNewsPaperControl.getCtrl());
				updateControlAccordingToAttribute();
				saveControlTraceAndSetRealLocationSize();
			}
			checkUndoButtonState();
			checkNextBackBtn();
		}

		private void btnSave_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			if (savePage(curPageNum))
			{
				try
				{
					if (filename == "")
					{
						if(saveAs())
							MessageBox.Show("Save successfully");
					}
					else
					{
						if(saveToXMLFile())
							MessageBox.Show("Save successfully");
					}					
				}
				catch { MessageBox.Show("Save error"); };
			}
		}

		private void bcSavePage_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			savePage(curPageNum);
		}
		bool savePage(int numPage)
		{
			numPage--;
			try
			{
				currentNews.LPages[numPage].HeightP = currentNews.AttBgk.BackgrHeight;
				currentNews.LPages[numPage].WidthP = currentNews.AttBgk.BackgrWidth;
				currentNews.LPages[numPage].StrBackground = currentNews.AttBgk.BackgroundURL;
				currentNews.LPages[numPage].lstNewsPaperControl.Clear();
				foreach (Control c in panelInnerScreen.Controls)
				{
					currentNews.LPages[numPage].lstNewsPaperControl.Add((INewsPaperControl)c);
				}
			}
			catch { return false; };
			return true;
		}

		private void uiPanel0_SizeChanged(object sender, EventArgs e)
		{
			buttonBar1.Height = uiPanel0Container.Height - 5;
			buttonBar1.Width = uiPanel0Container.Width - 4;
		}

		private void panelInnerScreen_SizeChanged(object sender, EventArgs e)
		{
			initPage();
		}

		private void btnClone_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			if (currentNewsPaperControl != null)
			{
				INewsPaperControl newcontrol = currentNewsPaperControl.clone();
				Point p = new Point(newcontrol.getAttList().getLocation().X * zoomDegree, newcontrol.getAttList().getLocation().Y * zoomDegree);
				Size s = new System.Drawing.Size(newcontrol.getAttList().getSize().Width * zoomDegree, newcontrol.getAttList().getSize().Height * zoomDegree);
				//newcontrol.getCtrl().Location = p;
				newcontrol.getCtrl().Size = s;

				newcontrol.getCtrl().MouseDown += new MouseEventHandler(MainProgram_MouseDown);
				newcontrol.getCtrl().MouseUp += new MouseEventHandler(MainProgram_MouseUp);
				panelInnerScreen.Controls.Add(newcontrol.getCtrl());
				currentNewsPaperControl = newcontrol;

				iObjNumber++;
				currentNewsPaperControl.getAttList().setInstanceName("Obj" + iObjNumber.ToString());

				saveControlTraceAndSetRealLocationSize();

				//Xử lý marker
				if (currentNewsPaperControl.getName() == "NewsPaperMarker")
				{
					lstMarker.Add(currentNewsPaperControl);
					MarkerList.lstMarkerList = new List<string>();
					foreach (INewsPaperControl c in lstMarker)
					{
						MarkerList.lstMarkerList.Add(c.getAttList().getInstanceName());
					}
				}
			}
		}

		void checkCloneButtonState()
		{
			if (currentNewsPaperControl == null)
				btnClone.Enabled = false;
			else
				btnClone.Enabled = true;
		}

		private void btnUndo_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			UndoMethod();
		}

		private void UndoMethod()
		{
			if (lstPreState.Count > 0)
			{
				int index = lstPreState.Count - 2;
				if (index >= 0)
				{
					//Nếu bị del]
					bool bIsDel = true;
					foreach (Control c in panelInnerScreen.Controls)
					{
						if (c == lstCurrentPointerControl[index])
							bIsDel = false;
					}
					if (bIsDel)
					{
						INewsPaperControl lastControl = lstCurrentPointerControl[lstCurrentPointerControl.Count - 1];
						int iFoundControlIndex;
						for (iFoundControlIndex = lstCurrentPointerControl.Count - 2; iFoundControlIndex >= 0; iFoundControlIndex--)
						{
							if (lastControl == lstCurrentPointerControl[iFoundControlIndex])
							{
								break;
							}
						}
						if (iFoundControlIndex > 0)
						{
							lstCurrentPointerControl[iFoundControlIndex].copy(lstPreState[index]);
							panelInnerScreen.Controls.Add((Control)(lstCurrentPointerControl[iFoundControlIndex]));
							currentNewsPaperControl = lstCurrentPointerControl[iFoundControlIndex];
						}
					}
					else
					{
						INewsPaperControl lastControl = lstCurrentPointerControl[lstCurrentPointerControl.Count - 1];
						int iFoundControlIndex;
						for (iFoundControlIndex = lstCurrentPointerControl.Count - 2; iFoundControlIndex >= 0; iFoundControlIndex--)
						{
							if (lastControl == lstCurrentPointerControl[iFoundControlIndex])
							{
								break;
							}
						}
						if (iFoundControlIndex < 0)
						{
							lstCurrentPointerControl[lstCurrentPointerControl.Count - 1].removeFromPanel();
						}
						else
						{

							(lstCurrentPointerControl[iFoundControlIndex]).copy(lstPreState[iFoundControlIndex]);
							currentNewsPaperControl = lstCurrentPointerControl[iFoundControlIndex];
						}

						//Check xem control đó có còn tồn tại hay k sau khi undo
						//bool isNotValid = false;
						//for (int i = 0; i < lstCurrentPointerControl.Count - 1; i++)
						//{
						//    if (lstCurrentPointerControl[i] == lstCurrentPointerControl[lstCurrentPointerControl.Count - 1])
						//    {
						//        isNotValid = true;
						//        break;
						//    }
						//}
						//if (!isNotValid)
						//    lstCurrentPointerControl[lstCurrentPointerControl.Count - 1].removeFromPanel();
					}


					lstNextPointerControl.Add(lstCurrentPointerControl[lstCurrentPointerControl.Count - 1]);
					lstCurrentPointerControl.RemoveAt(lstCurrentPointerControl.Count - 1);
					currentNewsPaperControl.getCtrl().Location = currentNewsPaperControl.getAttList().getLocation();
					currentNewsPaperControl.getCtrl().Size = currentNewsPaperControl.getAttList().getSize();
					if (bIsDel)
					{
						lstNextState.Add(null);
					}
					else
					{
						lstNextState.Add(lstPreState[index + 1].clone());
					}
					lstPreState.RemoveAt(index + 1);
				}
				else if (currentNewsPaperControl != null)
				{
					currentNewsPaperControl.removeFromPanel();
					lstNextState.Add(lstPreState[index + 1].clone());
					lstPreState.RemoveAt(index + 1);
					lstNextPointerControl.Add(lstCurrentPointerControl[index + 1]);
					lstCurrentPointerControl.RemoveAt(index + 1);
				}
			}
			checkUndoButtonState();
			checkRedoButtonState();
		}

		private void checkUndoButtonState()
		{
			if (lstPreState.Count <= 0)
			{
				btnUndo.LargeImage = imgUnAvaibleUndo;
				btnUndo.Enabled = false;
			}
			else
			{
				btnUndo.LargeImage = imgAvaibleUndo;
				btnUndo.Enabled = true;
			}
		}

		private void checkRedoButtonState()
		{
			if (lstNextState.Count <= 0)
			{
				btnRedo.LargeImage = imgUnAvaibleRedo;
				btnRedo.Enabled = false;
			}
			else
			{
				btnRedo.LargeImage = imgAvaibleRedo;
				btnRedo.Enabled = true;
			}
		}

		private void btnRedo_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			RedoMethod();
		}

		private void RedoMethod()
		{
			if (lstNextState.Count > 0)
			{
				int index = lstNextState.Count - 1;
				if (index >= 0)
				{
					if (lstNextState[index] == null)
					{
						currentNewsPaperControl.removeFromPanel();
						lstPreState.Add(currentNewsPaperControl.clone());
					}
					else
					{
						lstNextPointerControl[index].copy(lstNextState[index]);
						currentNewsPaperControl = lstNextPointerControl[index];
						bool isNotAvaible = true;
						foreach (INewsPaperControl contr in lstCurrentPointerControl)
						{
							if (lstNextPointerControl[index] == contr)
								isNotAvaible = false;
						}
						if (isNotAvaible)
							panelInnerScreen.Controls.Add(currentNewsPaperControl.getCtrl());

						currentNewsPaperControl.getCtrl().Location = currentNewsPaperControl.getAttList().getLocation();
						currentNewsPaperControl.getCtrl().Size = currentNewsPaperControl.getAttList().getSize();

						lstPreState.Add(lstNextState[index].clone());
					}

					lstNextState.RemoveAt(index);
					lstCurrentPointerControl.Add(lstNextPointerControl[index]);
					lstNextPointerControl.RemoveAt(index);
				}
			}
			checkRedoButtonState();
			checkUndoButtonState();
		}

		private void btnNewPage_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			bcSavePage_Click(sender, e);
			currentNews.initAttBgk();

			CPage p = new CPage();
			currentNews.LPages.Add(p);
			this.panelInnerScreen.Controls.Clear();
			
			initPage();
			panelInnerScreen.BackgroundImage = null;
			panelInnerScreen.BackColor = Color.White;

			//initNewsPaperAttr();
			currentNews.NumPages++;
			cbcPages.ComboBox.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem("Page "+" "+currentNews.NumPages.ToString()));
			curPageNum = currentNews.NumPages;
			cbcPages.ComboBox.SelectedIndex = curPageNum-1;

			checkNextBackBtn();
			btnZoomIn.Enabled = true;
		}

		private void btnZoomIn_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			zoomDegree *= 2;
			if (zoomDegree == 4)
			{
				btnZoomIn.Enabled = false;
				btnZoomOut.Enabled = true;
			}
			this.panelInnerScreen.Width = this.panelInnerScreen.Width *= 2;
			this.panelInnerScreen.Height = this.panelInnerScreen.Height *= 2;
			foreach (Control c in this.panelInnerScreen.Controls)
			{
				c.Width = c.Width * 2;
				c.Height = c.Height * 2;
				c.Location = new Point(c.Location.X*2,c.Location.Y*2);
			}
			btnZoomOut.Enabled = true;
			bIsZoom = true;

			//if (zoomDegree > 1)
			//{
			//    disableToolBar();
			//    foreach (Control c in this.panelInnerScreen.Controls)
			//    {
			//        c.MouseDown -= new MouseEventHandler(MainProgram_MouseDown);
			//        c.MouseUp -= new MouseEventHandler(MainProgram_MouseUp);					
			//    }
			//    this.panelInnerScreen.Click -= new EventHandler(uiPanelInner_Click);
			//}
			//else
			//{
			//    enableToolBar();
			//    foreach (Control c in this.panelInnerScreen.Controls)
			//    {
			//        c.MouseDown += new MouseEventHandler(MainProgram_MouseDown);
			//        c.MouseUp += new MouseEventHandler(MainProgram_MouseUp);
			//        c.Location = ((INewsPaperControl)c).getAttList().getLocation();
			//        c.Size = ((INewsPaperControl)c).getAttList().getSize();
			//    }
			//    this.panelInnerScreen.Click += new EventHandler(uiPanelInner_Click);
			//}
		}

		private void uiPanel2_SizeChanged(object sender, EventArgs e)
		{
			panelMidScreen.Width = uiPanel2.Width - 10;
			panelMidScreen.Height = uiPanel2.Height - 40;
		}

		private void btnZoomOut_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			zoomDegree /= 2;
			if (zoomDegree == 1)
			{
				btnZoomOut.Enabled = false;
				btnZoomIn.Enabled = true;
			}
			this.panelInnerScreen.Width = this.panelInnerScreen.Width / 2;
			this.panelInnerScreen.Height = this.panelInnerScreen.Height / 2;
			foreach (Control c in this.panelInnerScreen.Controls)
			{
				c.Width = c.Width / 2;
				c.Height = c.Height / 2;
				c.Location = new Point(c.Location.X / 2, c.Location.Y / 2);
			}
			btnZoomIn.Enabled = true;
			bIsZoom = false;

			//if (zoomDegree > 1)
			//{
			//    disableToolBar();
			//    foreach (Control c in this.panelInnerScreen.Controls)
			//    {
			//        c.MouseDown -= new MouseEventHandler(MainProgram_MouseDown);
			//        c.MouseUp -= new MouseEventHandler(MainProgram_MouseUp);
			//    }
			//    this.panelInnerScreen.Click -= new EventHandler(uiPanelInner_Click);
			//}
			//else
			//{
			//    enableToolBar();
			//    foreach (Control c in this.panelInnerScreen.Controls)
			//    {
			//        c.MouseDown += new MouseEventHandler(MainProgram_MouseDown);
			//        c.MouseUp += new MouseEventHandler(MainProgram_MouseUp);
			//        c.Location = ((INewsPaperControl)c).getAttList().getLocation();
			//        c.Size = ((INewsPaperControl)c).getAttList().getSize();
			//    }
			//    this.panelInnerScreen.Click += new EventHandler(uiPanelInner_Click);
			//}
		}

		private void cbcPages_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			savePage(curPageNum);
			string strItem = cbcPages.ComboBox.SelectedItem.Text.ToString();
			if (strItem != null || strItem.Length > 0)
			{
				int iPage = int.Parse(strItem.Substring(5));
				loadPaper(iPage);
				curPageNum = iPage;
				//reInitZoomUndoRedo();
			}
		}

		void reInitZoomUndoRedo()
		{
			zoomDegree = 1;
			btnZoomOut.Enabled = false;
			lstPreState = new List<INewsPaperControl>();
			lstNextState = new List<INewsPaperControl>();
			lstCurrentPointerControl = new List<INewsPaperControl>();
			lstNextPointerControl = new List<INewsPaperControl>();
		}

		private void panelInnerBounder_SizeChanged(object sender, EventArgs e)
		{
			panelInnerScreen.Location = new Point(0, 0);
		}

		private void propertyGrid1_SelectedObjectsChanged(object sender, EventArgs e)
		{
			//propertyGrid1.
		}

		private void btnDelete_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			try
			{
				lstMarker.Remove(currentNewsPaperControl);
				MarkerList.lstMarkerList.Remove(currentNewsPaperControl.getAttList().getInstanceName());
			}
			catch { };
			saveControlTraceAndSetRealLocationSize();
			currentNewsPaperControl.removeFromPanel();
		}

		private void MainProgram_KeyPress(object sender, KeyPressEventArgs e)
		{

		}

		private void MainProgram_KeyDown(object sender, KeyEventArgs e)
		{
			//MessageBox.Show("12");
			//if (e.Control && e.KeyCode == Keys.Z)
			//{
			//    UndoMethod();
			//}
		}
	}
}



