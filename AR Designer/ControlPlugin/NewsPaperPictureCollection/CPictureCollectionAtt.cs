using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel;
using NewsPaperControl;
using MyCollectionEditor;

namespace NewsPaperPictureCollectionControl
{
	public class CPictureCollectionAtt:CAttBase
	{
		public CPictureCollectionAtt()
		{
			pictureURLList = new List<PictureList>(10);
			currentIndex = 0;
		}

		private List<PictureList> pictureURLList;
		//[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
		//[System.ComponentModel.TypeConverter(typeof(System.ComponentModel.ExpandableObjectConverter))]
		[Editor(typeof(MyCollectionEditor.MyCollectionEditor), typeof(UITypeEditor))]
		[Description("Select images' URL")] 
		[Category("Data")]
		[DisplayName("Pictures")]
		public List<PictureList> PictureURLList
		{
			get { return pictureURLList; }
			set { pictureURLList = value; }
		}

		int currentIndex;
		[Description("Piture's index")]
		[Category("Data")]
		[DisplayName("Picture index")]
		public int CurrentIndex
		{
			get { return currentIndex; }
			set { currentIndex = value; }
		}
	}
}
