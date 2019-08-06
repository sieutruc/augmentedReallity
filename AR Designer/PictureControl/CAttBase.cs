using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PictureControl
{
	public class CAttBase:ICAtt
	{
		public CAttBase()
		{
			xLocation = 0;
			yLocation = 0;
			width = 200;
			height = 100;
		}
        public void setVideoURL(string value)
        {
        }
		private int xLocation;
		[ReadOnly(true)]
		public int XLocation
		{
			get { return xLocation; }
			set { xLocation = value; }
		}
		private int yLocation;
		[ReadOnly(true)]
		public int YLocation
		{
			get { return yLocation; }
			set { yLocation = value; }
		}
		private int width;
		[ReadOnly(true)]
		public int Width
		{
			get { return width; }
			set { width = value; }
		}
		private int height;
		[ReadOnly(true)]
		public int Height
		{
			get { return height; }
			set { height = value; }
		}

		public void setHeight(int value)
		{
			height = value;
		}

		public void setWidth(int value)
		{
			width = value;
		}

		public void setXLocation(int value)
		{
			xLocation = value;
		}

		public void setYLocation(int value)
		{
			yLocation = value;
		}

		public void setImageURL1(string value)
		{			
		}

		public void setImageURL2(string value)
		{
		}

		public void setImageURL3(string value)
		{
		}

		public void setImageURL4(string value)
		{
		}

		public void setImageURL5(string value)
		{
		}

		public int getXLocation()
		{
			return xLocation;
		}

		public int getYLocation()
		{
			return yLocation;
		}

		public int getHeight()
		{
			return height;
		}

		public int getWidth()
		{
			return width;
		}

		public string getImageURL1()
		{
			return "";
		}
		public string getImageURL2()
		{
			return "";
		}
		public string getImageURL3()
		{
			return "";
		}

		public string getImageURL4()
		{
			return "";
		}
		public string getImageURL5()
		{
			return "";
		}
		public string getVideoURL()
		{
			return "";
		}
	}
}
