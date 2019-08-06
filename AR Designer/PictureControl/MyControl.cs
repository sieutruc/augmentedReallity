using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PictureControl
{
    public enum ePosition
    {
        North, West, South, East, NorthWest, NorthEast, SouthWest, SouthEast, Inside
    }
    public partial class MyPictureControl : UserControl
    {
        public MyPictureControl()
        {
            InitializeComponent();
            ctrl = new PictureBox();
			ctrl.SizeMode = PictureBoxSizeMode.CenterImage;
        }
        
        public PictureBox ctrl;
		Stream s;
		Bitmap bmp;
		private ICAtt attList;
		public ICAtt AttList
		{
			get { return attList; }
			set { attList = value; }
		}

		public enum eUIType
		{
			Picture, PictureList, Video
		}

		private eUIType controlType;

		public eUIType ControlType
		{
			get { return controlType; }
			set
			{
				controlType = value;
				switch (controlType)
				{
					case eUIType.Picture:
						this.ctrl.Text = "Define your Image"; 
						attList = new CPictureAtt();
						s = this.GetType().Assembly.GetManifestResourceStream("PictureControl.photo.png");
						bmp = new Bitmap(s);
						ctrl.Image = bmp;						
						ctrl.Height = 80;
						ctrl.Width = 100;
						this.Height = 80;
						this.Width = 100;   
						break;
					case eUIType.PictureList:
						this.ctrl.Text = "Define your Image List";
						attList = new CPictureCollectionAtt();
						s = this.GetType().Assembly.GetManifestResourceStream("PictureControl.CollectionImg.png");
						bmp = new Bitmap(s);
						ctrl.Image = bmp;
						ctrl.Image = bmp;						
						ctrl.Height = 80;
						ctrl.Width = 100;
						this.Height = 80;
						this.Width = 100;   
						break;
					case eUIType.Video:
						this.ctrl.Text = "Define your Video";
						attList = new CVideoAtt();
						ctrl.Height = 80;
						ctrl.Width = 100;
						this.Height = 80;
						this.Width = 100;
						s = this.GetType().Assembly.GetManifestResourceStream("PictureControl.video.jpg");
						bmp = new Bitmap(s);
						ctrl.Image = bmp;
						break;
					default:
						break;
				}
			}
		}

        public void calculateEdges()
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

        public void initCtrl()
        {
            this.ctrl.Name = "Control";
            //this.ctrl.Text = "Control";
            //this.ctrl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctrl_MouseMove);
            this.ctrl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrl_MouseDown);
            this.ctrl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctrl_MouseUp);
            this.ctrl.MouseMove+=new MouseEventHandler(this.ctrl_MouseMove);
            this.Controls.Add(this.ctrl);
            this.ctrl.Bounds = this.Bounds;
            calculateEdges();
        }
        private int iConstDistance = 6;
        Rectangle recNW, recNE, recSE, recSW, recN, recE, recS, recW;

        
     
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

        public bool BIsResizable
        {
            get { return bIsResizable; }
            set { bIsResizable = value; }
        }
        public ePosition eClickedPosition = ePosition.Inside;

        public ePosition EClickedPosition
        {
            get { return eClickedPosition; }
            set { eClickedPosition = value; }
        }

        public Point pOriginClick;

        private void ctrl_MouseDown(object sender, MouseEventArgs e)
        {
            eClickedPosition = isAtEdges(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
                pOriginClick = e.Location;
            bIsResizable = true;

            this.OnMouseDown(e);
        }

        private void ctrl_MouseMove(object sender, MouseEventArgs e)
        {
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				ctMenuStrip.Show(ctrl, new Point(e.X, e.Y));
				return;
			}

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

        private void ctrl_MouseUp(object sender, MouseEventArgs e)
        {
            bIsResizable = false;
            this.OnMouseUp(e);
        }

		private void MyPictureControl_Resize(object sender, EventArgs e)
		{
			ctrl.Width = this.Width;
			ctrl.Height = this.Height;
			
			//ctrl.Image = bmp;
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.Panel uiPanel = (System.Windows.Forms.Panel)this.Parent;
			uiPanel.Controls.Remove(this);
		}
       
    }
}
