using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NewsPaperControl
{
    public abstract partial class NewsPaperControlBase : UserControl, INewsPaperControl
    {
        Control ctrl;
		string strName;
		IAtt myAttributs;
		Bitmap imgBackground;
		Bitmap imgIcon;
		string strDllPath;
		string strDisplayName;

		public string StrDisplayName
		{
			get { return strDisplayName; }
			set { strDisplayName = value; }
		}

		public string StrDllPath
		{
			get { return strDllPath; }
			set { strDllPath = value; }
		}
		
		public IAtt Attributs
		{
			get { return myAttributs; }
			set { myAttributs = value; }
		}
		public Bitmap ImgIcon
		{
			get { return imgIcon; }
			set { imgIcon = value; }
		}

		public Bitmap ImgBackground
		{
			get { return imgBackground; }
			set { imgBackground = value; }
		}

		public string StrName
		{
			get { return strName; }
			set { strName = value; }
		}

        public Control Ctrl
        {
            get { return ctrl; }
            set { ctrl = value; }
        }

		public NewsPaperControlBase()
        {
            InitializeComponent();
			setControlName();
			loadBackgroundAndICO();
			initCtrl();
			//Attributs = new CAttBase();
        }

        public abstract void initCtrl();
		public abstract void setControlName();

		#region Implement Methods inhered from INewsPaperControl
		public abstract void updateAttributeValue(string strAttName, object objAttValue);
		public abstract string toXMLString();
		public abstract void loadfromXML(string strXML, string path);
		public abstract INewsPaperControl clone();
		public abstract void copy(INewsPaperControl control);
		public abstract string getDisplayName();

		public void setSize(Size s)
		{
			this.Size = new Size(s.Width,s.Height);
		}

		public string getDllPath()
		{
			return strDllPath;
		}

		public void setDllPath(string DllPath)
		{
			this.strDllPath = DllPath;
		}

		public void updateAttributeValueBase(string strAttName, object objAttValue)
		{
			switch (strAttName)
			{
				case "YLocation":
					Attributs.setLocation(new Point(Attributs.getLocation().X,(int)objAttValue));
					break;
				case "XLocation":
					Attributs.setLocation(new Point((int)objAttValue,Attributs.getLocation().Y));
					break;
				case "Width":
					Attributs.setSize(new Size((int)objAttValue,Attributs.getSize().Height));
					break;
				case "Height":
					Attributs.setSize(new Size(Attributs.getSize().Width,(int)objAttValue));
					ctrl.Invalidate();
					break;
			}
		}
		public IAtt getAttList()
		{
			return myAttributs;
		}

		public Control getCtrl()
		{
			return this;
		}
		public string getName()
		{
			return this.strName;
		}
				
		public Image getICO()
		{
			return imgIcon;
		}

		public Image getBackground()
		{
			return ImgBackground;
		}

		public abstract void loadBackgroundAndICO();

		#endregion

		#region Mouse Move action
		protected void calculateEdges()
        {
            recNW = new Rectangle(this.ctrl.Bounds.Left - iConstDistance, this.ctrl.Bounds.Top - iConstDistance, iConstDistance * 2, iConstDistance * 2);
            recNE = new Rectangle(this.ctrl.Bounds.Right - iConstDistance, this.ctrl.Bounds.Top - iConstDistance, iConstDistance * 2, iConstDistance * 2);
            recSE = new Rectangle(this.ctrl.Bounds.Right - iConstDistance, this.ctrl.Bounds.Bottom - iConstDistance, iConstDistance * 2, iConstDistance * 2);
            recSW = new Rectangle(this.ctrl.Bounds.Left - iConstDistance, this.ctrl.Bounds.Bottom - iConstDistance, iConstDistance * 2, iConstDistance * 2);
            recN = new Rectangle(this.ctrl.Bounds.Left + iConstDistance, this.ctrl.Bounds.Top - iConstDistance, this.ctrl.Bounds.Width - iConstDistance * 2, iConstDistance * 2);
            recE = new Rectangle(this.ctrl.Bounds.Right - iConstDistance, this.ctrl.Bounds.Top + iConstDistance, iConstDistance * 2, this.ctrl.Bounds.Height - iConstDistance * 2);
            recS = new Rectangle(this.ctrl.Bounds.Left + iConstDistance, this.ctrl.Bounds.Bottom - iConstDistance, this.ctrl.Bounds.Width - iConstDistance * 2, iConstDistance * 2);
            recW = new Rectangle(this.ctrl.Bounds.Left - iConstDistance, this.ctrl.Bounds.Top + iConstDistance, iConstDistance * 2, this.ctrl.Bounds.Height - iConstDistance * 2);
        }

		protected int iConstDistance = 6;
        Rectangle recNW, recNE, recSE, recSW, recN, recE, recS, recW;

        public enum ePosition
        {
            North, West, South, East, NorthWest, NorthEast, SouthWest, SouthEast, Inside
        }

        public ePosition isAtEdges(int iX, int iY)
        {
            Point p = new Point(iX, iY);
            if (recNW.Contains(p))
                return ePosition.NorthWest;
            if (recNE.Contains(p))
                return ePosition.NorthEast;
            if (recSE.Contains(p))
                return ePosition.SouthEast;
            if (recSW.Contains(p))
                return ePosition.SouthWest;
            if (recN.Contains(p))
                return ePosition.North;
            if (recS.Contains(p))
                return ePosition.South;
            if (recW.Contains(p))
                return ePosition.West;
            if (recE.Contains(p))
                return ePosition.East;
            return ePosition.Inside;
        }

        bool bIsResizable = false;
		protected ePosition eClickedPosition = ePosition.Inside;

        public ePosition EClickedPosition
        {
            get { return eClickedPosition; }
            set { eClickedPosition = value; }
        }

		protected Point pOriginClick;

		public  void ctrl_MouseDown(object sender, MouseEventArgs e)
        {
            eClickedPosition = isAtEdges(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
                pOriginClick = e.Location;
            bIsResizable = true;

            this.OnMouseDown(e);
        }

		public void ctrl_MouseMove(object sender, MouseEventArgs e)
        {
			//if (e.Button == System.Windows.Forms.MouseButtons.Right)
			//{
			//    ctMenuStrip.Show(ctrl, new Point(e.X, e.Y));
			//    return;
			//}
            if (isAtEdges(e.X, e.Y) == ePosition.NorthWest || isAtEdges(e.X, e.Y) == ePosition.SouthEast)
                this.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            else if (isAtEdges(e.X, e.Y) == ePosition.NorthEast || isAtEdges(e.X, e.Y) == ePosition.SouthWest)
                this.Cursor = System.Windows.Forms.Cursors.SizeNESW;
            else if (isAtEdges(e.X, e.Y) == ePosition.North || isAtEdges(e.X, e.Y) == ePosition.South)
                this.Cursor = System.Windows.Forms.Cursors.SizeNS;
            else if (isAtEdges(e.X, e.Y) == ePosition.West || isAtEdges(e.X, e.Y) == ePosition.East)
                this.Cursor = System.Windows.Forms.Cursors.SizeWE;
            else
                this.Cursor = System.Windows.Forms.Cursors.SizeAll;

            if (bIsResizable)
            {
                if (this.eClickedPosition == ePosition.NorthWest)
                {
                    this.Location = new Point(this.Left + (e.X - pOriginClick.X), this.Top + (e.Y - pOriginClick.Y));
                    this.Width = this.Width + (pOriginClick.X - e.X);
                    this.Height = this.Height + (pOriginClick.Y - e.Y);
                }
                else if (this.eClickedPosition == ePosition.SouthEast)
                {
                    this.Width = e.X;
                    this.Height = e.Y;
                }
                else if (this.eClickedPosition == ePosition.SouthWest)
                {
                    this.Location = new Point(this.Left += (e.X - pOriginClick.X), this.Top);
                    this.Width = this.Width + (pOriginClick.X - e.X);
                    this.Height = e.Y;
                }
                else if (this.eClickedPosition == ePosition.NorthEast)
                {
                    this.Location = new Point(this.Left, this.Top + (e.Y - pOriginClick.Y));
                    this.Width = e.X;
                    this.Height += (pOriginClick.Y - e.Y);
                }
                else if (this.eClickedPosition == ePosition.North)
                {
                    this.Location = new Point(this.Left, this.Top + (e.Y - pOriginClick.Y));
                    this.Height += (pOriginClick.Y - e.Y);
                }
                else if (this.eClickedPosition == ePosition.South)
                {
                    this.Height = e.Y;
                }
                else if (this.eClickedPosition == ePosition.East)
                {
                    this.Width = e.X;
                }
                else if (this.eClickedPosition == ePosition.West)
                {
                    this.Location = new Point(this.Left + (e.X - pOriginClick.X), this.Top);
                    this.Width += (pOriginClick.X - e.X);
                }
                else if (this.eClickedPosition == ePosition.Inside)
                {
                    this.Location = new Point(this.Left + (e.X - pOriginClick.X), this.Top + (e.Y - pOriginClick.Y));
                }
                this.ctrl.Width = this.Width;
                this.ctrl.Height = this.Height;
                calculateEdges();
            }

        }

		public void ctrl_MouseUp(object sender, MouseEventArgs e)
        {
            bIsResizable = false;
            this.OnMouseUp(e);
		}


		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.Panel uiPanel = (System.Windows.Forms.Panel)this.Parent;
			uiPanel.Controls.Remove(this);
		}

		#endregion

		private void NewsPaperControlBase_SizeChanged(object sender, EventArgs e)
		{
			ctrl.Size = this.Size;
		}
		
		private void deleteToolStripMenuItem_Click_2(object sender, EventArgs e)
		{
			removeFromPanel();
		}

		private void cloneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//System.Windows.Forms.Panel uiPanel = (System.Windows.Forms.Panel)this.Parent;
			//INewsPaperControl newControl = this.clone();
			//uiPanel.Controls.Add((Control)newControl);
		}

		public void removeFromPanel()
		{
			try
			{
				System.Windows.Forms.Panel uiPanel = (System.Windows.Forms.Panel)this.Parent;
				uiPanel.Controls.Remove(this);
			}
			catch { };
		}

		public int iZoomDegree = 1;
		public void setZoomCoefficient(int Degree)
		{
			if (Degree > iZoomDegree)
			{
				int iRate = Degree/iZoomDegree;
				Point pLocation = myAttributs.getLocation();
				Size sSize = myAttributs.getSize();
				myAttributs.setLocation(new Point(pLocation.X/iRate,pLocation.Y/iRate));
				myAttributs.setSize(new System.Drawing.Size(sSize.Width/iRate,sSize.Height/iRate));
				//width /= Degree / iZoomDegree;
				//height /= Degree / iZoomDegree;
				//xLocation /= Degree / iZoomDegree;
				//yLocation /= Degree / iZoomDegree;
			}
			else if (Degree < iZoomDegree)
			{
				int iRate = iZoomDegree / Degree;
				Point pLocation = myAttributs.getLocation();
				Size sSize = myAttributs.getSize();
				myAttributs.setLocation(new Point(pLocation.X * iRate, pLocation.Y * iRate));
				myAttributs.setSize(new System.Drawing.Size(sSize.Width * iRate, sSize.Height * iRate));
				//width *= Degree / iZoomDegree;
				//height *= Degree / iZoomDegree;
				//xLocation *= Degree / iZoomDegree;
				//yLocation *= Degree / iZoomDegree;
			}
			else
			{

			}
			iZoomDegree = Degree;

		}
	}
}
