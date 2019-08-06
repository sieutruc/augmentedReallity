using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace NewsPaperControl
{
	public abstract class CAttBase:IAtt
	{
		public CAttBase()
		{
			xLocation = 0;
			yLocation = 0;
			height = 100;
			width = 100;
		}
		//public int iZoomDegree = 1;
		
		private int xLocation;
		[Description("X Location")]
		[Category("Location")]
		[DisplayName("X")]
		public int XLocation
		{
			get { return xLocation; }
			set { xLocation = value; }
		}

		private int yLocation;
		[Description("Y Location")]
		[Category("Location")]
		[DisplayName("Y")]
		public int YLocation
		{
			get { return yLocation; }
			set { yLocation = value; }
		}

		private int width;
		[Description("Width")]
		[Category("Size")]
		[DisplayName("Width")]
		public int Width
		{
			get { return width; }
			set { width = value; }
		}
		
		private int height;
		[Description("Height")]
		[Category("Size")]
		[DisplayName("Height")]
		public int Height
		{
			get { return height; }
			set { height = value; }
		}
		
		string strMarkerName;
		[Description("Reference to Marker")]
		[Category("Data")]
		[DisplayName("Marker Name")]
		[TypeConverter(typeof(MarkerDropdownList))]
		public string StrMarkerName
		{
			get { return strMarkerName; }
			set { strMarkerName = value; }
		}

		string strInstanceName;
		[Description("Instance's Object name")]
		[Category("Data")]
		[DisplayName("Instance Name")]
		public string StrInstanceName
		{
			get { return strInstanceName; }
			set { strInstanceName = value; }
		}

		public string getInstanceName()
		{
			return strInstanceName;
		}

		public void setInstanceName(string name)
		{
			strInstanceName = name;
		}

		public Point getLocation()
		{
			return new Point(xLocation, yLocation);
		}

		public void setLocation(Point pLocation)
		{
			xLocation = pLocation.X;
			yLocation = pLocation.Y;
		}

		public Size getSize()
		{
			return new Size(width,height);
		}

		public void setSize(Size pSize)
		{
			width = pSize.Width;
			height = pSize.Height;
		}

		public void setZoomCoefficient(int Degree)
		{
		}

		public string getMarkerName()
		{
			return strMarkerName;
		}

		public void setMarkerName(string name)
		{
			this.strMarkerName = name;
		}
	}
}
