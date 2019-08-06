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

namespace NewspaperDesigner
{

	public partial class MainProgram : Form
	{
		INewsPaperControl currentNewsPaperControl;
		NewsPaperControlManager m_MyControlList;
		List<INewsPaperControl> preState;
		List<INewsPaperControl> nextState;
		List<INewsPaperControl> currentState;
		static newspaper currentNews;
		static string filename = "";
		static int curNumPage;

		public MainProgram()
		{
			InitializeComponent();
			loadNewsPaperBackgr();
			loadControlPlugin();
			loadToolBox();
			preState = new List<INewsPaperControl>();
			nextState = new List<INewsPaperControl>();
			currentState = new List<INewsPaperControl>();
			cbcPages.ComboBox.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			rbMenu.SelectedTab = rbTab1;
			//uiImage.Enabled = false;
			//uiSequenceImages.Enabled = false;
			//uiVideo.Enabled = false;

			MyCollectionEditor.MyCollectionEditor.MyPropertyValueChanged += new MyCollectionEditor.MyCollectionEditor.MyPropertyValueChangedEventHandler(MyCollectionEditor_MyPropertyValueChanged);

		}

		void MyCollectionEditor_MyPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
		{
			currentNewsPaperControl.updateAttributeValue(e.ChangedItem.PropertyDescriptor.Name, e.ChangedItem.Value);
			updateControlAccordingToAttribute();
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
			buttonBar1_Group_0.Key = "Group1";
			buttonBar1_Group_0.Text = "My newspaper control";
			this.buttonBar1.Groups.AddRange(new Janus.Windows.ButtonBar.ButtonBarGroup[] {
            buttonBar1_Group_0});

			//Load toolbox
			foreach (INewsPaperControl item in m_MyControlList.LstNewsPaperControl)
			{
				Janus.Windows.ButtonBar.ButtonBarItem buttonBarItem = new Janus.Windows.ButtonBar.ButtonBarItem();
				buttonBarItem.Image = item.getICO();
				buttonBarItem.Text = item.getName();
				buttonBarItem.Tag = item;
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
					}
				}
			}

			((Control)(currentNewsPaperControl)).MouseDown += new MouseEventHandler(MainProgram_MouseDown);
			((Control)(currentNewsPaperControl)).MouseUp += new MouseEventHandler(MainProgram_MouseUp);
			panelInnerScreen.Controls.Add((Control)currentNewsPaperControl);
			preState.Add(currentNewsPaperControl.clone());
			currentState.Add(currentNewsPaperControl);
		}

		void MainProgram_MouseUp(object sender, MouseEventArgs e)
		{
			currentNewsPaperControl = (NewsPaperControlBase)sender;
			currentNewsPaperControl.getAttList().setLocation(currentNewsPaperControl.getCtrl().Location);
			currentNewsPaperControl.getAttList().setSize(currentNewsPaperControl.getCtrl().Size);
			loadProperties();
			preState.Add(currentNewsPaperControl.clone());
			currentState.Add(currentNewsPaperControl);
		}

		void MainProgram_MouseDown(object sender, MouseEventArgs e)
		{
			currentNewsPaperControl = (NewsPaperControlBase)sender;
			loadProperties();
		}

		CBackgroundAtt attBgk = new CBackgroundAtt();

		//load bacground
		void loadNewsPaperBackgr()
		{
			Stream s = this.GetType().Assembly.GetManifestResourceStream("NewspaperDesigner.frame.png");
			Bitmap bmp = new Bitmap(s);
			panelInnerScreen.BackgroundImage = bmp;
			attBgk = new CBackgroundAtt();
		}

		public void loadProperties()
		{
			propertyGrid1.SelectedObject = currentNewsPaperControl.getAttList();
		}

		private void updateControlAccordingToAttribute()
		{
			currentNewsPaperControl.getCtrl().Size = currentNewsPaperControl.getAttList().getSize();
			currentNewsPaperControl.getCtrl().Location = currentNewsPaperControl.getAttList().getLocation();
		}

		private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			//MessageBox.Show(e.ChangedItem.PropertyDescriptor.Name);
			if (currentNewsPaperControl != null)
			{
				currentNewsPaperControl.updateAttributeValue(e.ChangedItem.PropertyDescriptor.Name, e.ChangedItem.Value);
				updateControlAccordingToAttribute();
				preState.Add(currentNewsPaperControl.clone());
				currentState.Add(currentNewsPaperControl);
			}
			if (e.ChangedItem.Label.Contains("BackgroundURL"))
			{
				try
				{
					Image img = Image.FromFile(e.ChangedItem.Value.ToString(), true);
					panelInnerScreen.BackgroundImage = img;
				}
				catch (Exception exp) { MessageBox.Show("Select image file, please!"); }
			}
			if (e.ChangedItem.Label.Contains("Background Width"))
			{
				try
				{
					panelInnerScreen.Width = int.Parse(e.ChangedItem.Value.ToString());
					int xLocation = (panelMidScreen.Width - panelInnerScreen.Width) / 2;
					panelInnerScreen.Location = new Point(xLocation, panelInnerScreen.Location.Y);
				}
				catch { };
			}

			if (e.ChangedItem.Label.Contains("Background Height"))
			{
				try
				{
					panelInnerScreen.Height = int.Parse(e.ChangedItem.Value.ToString());
					//int xLocation = (uiPanelMidScreen.Width - uiMidPanel.Width) / 2;
					//uiMidPanel.Location = new Point(xLocation, uiMidPanel.Location.Y);
				}
				catch { };
			}
		}

		private void uiMidPanel_Click(object sender, EventArgs e)
		{
			attBgk.BackgrWidth = panelInnerScreen.Width;
			attBgk.BackgrHeight = panelInnerScreen.Height;
			propertyGrid1.SelectedObject = attBgk;
		}

		private void uiPanelMidScreen_SizeChanged(object sender, EventArgs e)
		{
			initNewsPaper();
		}

		private void initNewsPaper()
		{
			
			int width = panelInnerScreen.Size.Width;
			int height = panelInnerScreen.Size.Height;
			if (width > panelMidScreen.Width)
				width = panelMidScreen.Width-20;
			if (height > panelMidScreen.Height)
				height = panelMidScreen.Height-20;
			panelInnerBounder.Size = new Size(width+20, height+20);

			int xLocation = ((panelMidScreen.Width - panelInnerScreen.Width) / 2);
			int yLocation = panelInnerBounder.Location.Y;
			if (xLocation < 0)
				xLocation = 0;
			panelInnerBounder.Location = new Point(xLocation, yLocation);
			//MessageBox.Show(panelInnerScreen.Location.X.ToString() + "/" +panelInnerScreen.Location.Y.ToString());
			panelInnerScreen.Location = new Point(0,0);
		}

		private void btnNew_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			newspaper news = new newspaper();
			New frmNew = new New();
			frmNew.setNewspaper(news);
			frmNew.ShowDialog();
			if (frmNew.Ret == 1)
			{
				//uiImage.Enabled = true;
				//uiSequenceImages.Enabled = true;
				//uiVideo.Enabled = true;
				propertyGrid1.Enabled = true;
				currentNews = news;
				curNumPage = 0;
				filename = "";
				panelInnerScreen.Controls.Clear();
				for (int i = 0; i < currentNews.NumPages; i++)
				{
					cbcPages.ComboBox.Items.Add("Page " + (i + 1));
					currentNews.LPages.Add(new page());
				}
				cbcPages.ComboBox.SelectedIndex = 0;
			}
		}

		private void buttonCommand1_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			saveAs();

		}
		void saveAs()
		{
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.Filter = "XML File|*.xml";
			saveFileDialog1.Title = "Save an XML File";
			saveFileDialog1.ShowDialog();

			// If the file name is not an empty string open it for saving.
			if (saveFileDialog1.FileName != "")
			{
				newspaper.writeXML(currentNews, saveFileDialog1.FileName);
				filename = saveFileDialog1.FileName;
			}
		}
		private void buttonCommand2_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "Xml Files (.xml)|*.xml";
			ofd.FilterIndex = 1;
			DialogResult userClickedOK = ofd.ShowDialog();

			// Process input if the user clicked OK.
			if (userClickedOK == DialogResult.OK)
			{
				// Open the selected file to read.
				currentNews = newspaper.readXML(ofd.FileName);
				if (currentNews == null)
				{
					MessageBox.Show("Please open another XML file", "Open file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				try
				{
					curNumPage = 0;
					loadPaper(curNumPage);
					for (int i = 0; i < currentNews.NumPages; i++)
						cbcPages.ComboBox.Items.Add("Page " + (i + 1));
					cbcPages.ComboBox.SelectedIndex = 0;
					filename = ofd.FileName;
					//uiImage.Enabled = true;
					//uiSequenceImages.Enabled = true;
					//uiVideo.Enabled = true;
					propertyGrid1.Enabled = true;
					cbcPages.ComboBox.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
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
			curNumPage = cbcPages.ComboBox.SelectedIndex;
		}

		private void dropDownCommand1_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			this.Close();
		}

		private void btcNext_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			if (curNumPage < (currentNews.NumPages - 1))
			{
				loadPaper(++curNumPage);
				cbcPages.ComboBox.SelectedIndex = curNumPage;
			}
		}
		private void btcBack_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			if (curNumPage > 0)
			{
				loadPaper(--curNumPage);
				cbcPages.ComboBox.SelectedIndex = curNumPage;
			}
		}

		private void loadPaper(int pageNum)
		{
			this.panelInnerScreen.Controls.Clear();
			page p = currentNews.LPages[pageNum];
			for (int i = 0; i < p.LObjects.Count; i++)
			{
				switch (p.LObjects[i].Type)
				{
					case "Image":
						addImage(p.LObjects[i].Xlocation, p.LObjects[i].Ylocation,
							p.LObjects[i].Width, p.LObjects[i].Height, p.LObjects[i].LUris, 1);
						break;
					case "SequenceImages":
						addImage(p.LObjects[i].Xlocation, p.LObjects[i].Ylocation,
							p.LObjects[i].Width, p.LObjects[i].Height, p.LObjects[i].LUris, 0);
						break;
					case "Video":
						addImage(p.LObjects[i].Xlocation, p.LObjects[i].Ylocation,
							p.LObjects[i].Width, p.LObjects[i].Height, p.LObjects[i].LUris, 2);
						break;
				}
			}

		}
		void addImage(int x, int y, int width, int height, List<string> listUris, int type)
		{
			//PictureControl.MyPictureControl newCrl = new MyPictureControl();
			//newCrl.initCtrl();
			//newCrl.MouseDown += new MouseEventHandler(MyPictureControl_MouseDown);
			//newCrl.MouseUp += new MouseEventHandler(MyPictureControl_MouseUp);
			//if (listUris.Count > 1)
			//{
			//    newCrl.ControlType = MyPictureControl.eUIType.PictureList;
			//    int i = listUris.Count;
			//    for (; i > 0; i--)
			//    {
			//        switch (i)
			//        {
			//            case 5:
			//                ((CPictureCollectionAtt)(newCrl.AttList)).setImageURL5(listUris[4]);
			//                break;
			//            case 4:
			//                ((CPictureCollectionAtt)(newCrl.AttList)).setImageURL4(listUris[3]);
			//                break;
			//            case 3:
			//                ((CPictureCollectionAtt)(newCrl.AttList)).setImageURL3(listUris[2]);
			//                break;
			//            case 2:
			//                ((CPictureCollectionAtt)(newCrl.AttList)).setImageURL2(listUris[1]);
			//                break;
			//            case 1:
			//                ((CPictureCollectionAtt)(newCrl.AttList)).setImageURL1(listUris[0]);
			//                break;
			//        }
			//    }
			//}
			//else
			//{
			//    if (type == 1)
			//    {
			//        newCrl.ControlType = MyPictureControl.eUIType.Picture;
			//        ((CPictureAtt)(newCrl.AttList)).setImageURL1(listUris[0]);
			//    }
			//    else
			//    {
			//        newCrl.ControlType = MyPictureControl.eUIType.Video;
			//        ((CVideoAtt)(newCrl.AttList)).setVideoURL(listUris[0]);
			//    }
			//}
			//newCrl.Location = new Point(x, y);
			//newCrl.Height = height;
			//newCrl.Width = width;
			//newCrl.AttList.setXLocation(x);
			//newCrl.AttList.setYLocation(y);
			//newCrl.AttList.setWidth(width);
			//newCrl.AttList.setHeight(height);
			//uiMidPanel.Controls.Add(newCrl);
		}

		private void bcSave_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			if (filename == "")
			{
				saveAs();
			}
			else
			{
				newspaper.writeXML(currentNews, filename);
			}
		}

		private void bcSavePage_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			savePage(curNumPage);
			int a = 3;
		}
		void savePage(int numPage)
		{
			//page p = currentNews.LPages[numPage];
			//foreach(Control ct in uiMidPanel.Controls)
			//{
			//    control c = new control();
			//    MyPictureControl my = (MyPictureControl)ct;
			//    c.Xlocation = my.Location.X;
			//    c.Ylocation = my.Location.Y;
			//    c.Width = my.Width;
			//    c.Height = my.Height;
			//    if (my.ControlType == MyPictureControl.eUIType.Picture)
			//    {
			//        c.Type = "Image";
			//        string str = ((CPictureAtt)(my.AttList)).getImageURL1();
			//        if (str != "(none)")
			//            c.LUris.Add(str);
			//        else
			//        {
			//            MessageBox.Show("Please fill in the path to image file", "Open file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//            return;
			//        }
			//    }
			//    else if (my.ControlType == MyPictureControl.eUIType.Video)
			//    {
			//        c.Type = "Video";
			//        string str = ((CVideoAtt)(my.AttList)).getVideo();
			//        if (str != "(none)") 
			//            c.LUris.Add(str);
			//        else
			//        {
			//            MessageBox.Show("Please fill in the path to video file", "Open file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//            return;
			//        }
			//    }
			//    else
			//    {
			//        c.Type = "SequenceImages";
			//        int i = 0;
			//        string str = ((CPictureCollectionAtt)(my.AttList)).getImageURL1();
			//        if (str != "(none)")
			//        {
			//            c.LUris.Add(str);
			//            i++;
			//        }
			//        string str1 = ((CPictureCollectionAtt)(my.AttList)).getImageURL2();
			//        if (str1 != "(none)")
			//        {
			//            i++;
			//            c.LUris.Add(str1);
			//        }
			//        string str2 = ((CPictureCollectionAtt)(my.AttList)).getImageURL3();
			//        if (str2 != "(none)")
			//        {
			//            i++;
			//            c.LUris.Add(str2);
			//        }
			//        string str3 = ((CPictureCollectionAtt)(my.AttList)).getImageURL4();
			//        if (str3 != "(none)")
			//        {
			//            i++;
			//            c.LUris.Add(str3);
			//        }
			//        string str4 = ((CPictureCollectionAtt)(my.AttList)).getImageURL5();
			//        if (str4 != "(none)")
			//        {
			//            i++;
			//            c.LUris.Add(str4);
			//        }
			//        if(i==0)
			//        {
			//            MessageBox.Show("Please fill in at least one path in sequenceimages control", "Open file error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//            return;
			//        }
			//    }
			//    p.LObjects.Add(c);
			//}
		}

		private void uiPanel0_SizeChanged(object sender, EventArgs e)
		{
			buttonBar1.Height = uiPanel0Container.Height - 10;
			buttonBar1.Width = uiPanel0Container.Width - 4;
		}

		private void panelInnerScreen_SizeChanged(object sender, EventArgs e)
		{
			initNewsPaper();
		}

		private void btnClone_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
		    INewsPaperControl newcontrol = currentNewsPaperControl.clone();
			newcontrol.getCtrl().MouseDown+=new MouseEventHandler(MainProgram_MouseDown);
			newcontrol.getCtrl().MouseUp+=new MouseEventHandler(MainProgram_MouseUp);
			panelInnerScreen.Controls.Add(newcontrol.getCtrl());
			currentNewsPaperControl = newcontrol;
			preState.Add(currentNewsPaperControl.clone());
			currentState.Add(currentNewsPaperControl);
		}

		private void btnUndo_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			if (currentNewsPaperControl != null && panelInnerScreen.Controls.Count>0 && preState.Count>0)
			{
				int index = preState.Count - 2;
				if (index >= 0)
				{
					(currentState[currentState.Count - 2]).copy(preState[index]);
					currentNewsPaperControl = currentState[currentState.Count - 1];
					currentState.RemoveAt(currentState.Count - 1);
					//INewsPaperControl newControl = preState[index].clone();
					//panelInnerScreen.Controls.Add((Control)newControl);
					currentNewsPaperControl.getCtrl().Location = currentNewsPaperControl.getAttList().getLocation();
					//currentNewsPaperControl.getCtrl().MouseDown += new MouseEventHandler(MainProgram_MouseDown);
					//newControl.getCtrl().MouseUp+=new MouseEventHandler(MainProgram_MouseUp);
					nextState.Add(preState[index+1].clone());
					preState.RemoveAt(index+1);
					//currentNewsPaperControl = newControl;
				}
			}
		}

		private void btnRedo_Click(object sender, Janus.Windows.Ribbon.CommandEventArgs e)
		{
			if (currentNewsPaperControl != null && panelInnerScreen.Controls.Count > 0 && nextState.Count>0)
			{
				int index = nextState.Count - 1;
				if (index >= 0)
				{
					currentNewsPaperControl.removeFromPanel();
					INewsPaperControl newControl = nextState[index].clone();
					panelInnerScreen.Controls.Add((Control)newControl);
					preState.Add(nextState[index].clone());
					nextState.RemoveAt(index);
					newControl.getCtrl().Location = newControl.getAttList().getLocation();
					newControl.getCtrl().MouseDown += new MouseEventHandler(MainProgram_MouseDown);
					newControl.getCtrl().MouseUp += new MouseEventHandler(MainProgram_MouseUp);
					currentNewsPaperControl = newControl;
				}
			}
		}

	}
}



