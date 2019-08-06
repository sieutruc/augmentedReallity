using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PictureControl
{
	public interface ICAtt
	{
		void setXLocation(int value);
		void setYLocation(int value);
		void setHeight(int value);
		void setWidth(int value);
		void setImageURL1(string value);
		void setImageURL2(string value);
		void setImageURL3(string value);
		void setImageURL4(string value);
		void setImageURL5(string value);
		void setVideoURL(string value);

		int getXLocation();
		int getYLocation();
		int getHeight();
		int getWidth();
		string getImageURL1();
		string getImageURL2();
		string getImageURL3();
		string getImageURL4();
		string getImageURL5();
		string getVideoURL();
	}
}
