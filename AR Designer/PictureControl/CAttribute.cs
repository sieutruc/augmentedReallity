using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Windows.Forms; 

namespace PictureControl
{
    /// <summary>
    /// Mảng các Attribute dùng ở PropertyGrid
    /// </summary>
    public class CAttributesList : CollectionBase,ICustomTypeDescriptor
	{
		/// <summary>
		/// Add 1 CAttribute vào list
		/// </summary>
		/// <param name="Value"></param>
		public void Add(CAttribute Value)
		{
			base.List.Add(Value);
		}

		/// <summary>
		/// Remove attribute có StrName = Name
		/// </summary>
		/// <param name="Name"></param>
		public void Remove(string Name)
		{
			foreach(CAttribute prop in base.List)
			{
				if(prop.StrName == Name)
				{
					base.List.Remove(prop);
					return;
				}
			}
		}

		/// <summary>
		/// Lấy phần tử thứ index
		/// </summary>
		public CAttribute this[int index] 
		{
			get 
			{
				return (CAttribute)base.List[index];
			}
			set
			{
				base.List[index] = (CAttribute)value;
			}
		}

        //Hiện thực các phương thức của ICustomTypeDescriptor
		#region "TypeDescriptor Implementation"
		/// <summary>
		/// Get Class Name
		/// </summary>
		/// <returns>String</returns>
		public String GetClassName()
		{
			return TypeDescriptor.GetClassName(this,true);
		}

		/// <summary>
		/// GetAttributes
		/// </summary>
		/// <returns>AttributeCollection</returns>
		public AttributeCollection GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this,true);
		}

		/// <summary>
		/// GetComponentName
		/// </summary>
		/// <returns>String</returns>
		public String GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this, true);
		}

		/// <summary>
		/// GetConverter
		/// </summary>
		/// <returns>TypeConverter</returns>
		public TypeConverter GetConverter()
		{
			return TypeDescriptor.GetConverter(this, true);
		}

		/// <summary>
		/// GetDefaultEvent
		/// </summary>
		/// <returns>EventDescriptor</returns>
		public EventDescriptor GetDefaultEvent() 
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		/// <summary>
		/// GetDefaultProperty
		/// </summary>
		/// <returns>PropertyDescriptor</returns>
		public PropertyDescriptor GetDefaultProperty() 
		{
			return TypeDescriptor.GetDefaultProperty(this, true);
		}

		/// <summary>
		/// GetEditor
		/// </summary>
		/// <param name="editorBaseType">editorBaseType</param>
		/// <returns>object</returns>
		public object GetEditor(Type editorBaseType) 
		{
			return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}

		public EventDescriptorCollection GetEvents(Attribute[] attributes) 
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		public EventDescriptorCollection GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);
		}

		public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			PropertyDescriptor[] newProps = new PropertyDescriptor[this.Count];
			for (int i = 0; i < this.Count; i++)
			{

				CAttribute  prop = (CAttribute) this[i];
				newProps[i] = new CustomPropertyDescriptor(ref prop, attributes);
			}

			return new PropertyDescriptorCollection(newProps);
		}

		public PropertyDescriptorCollection GetProperties()
		{
			
			return TypeDescriptor.GetProperties(this, true);
			
		}

		public object GetPropertyOwner(PropertyDescriptor pd) 
		{
			return this;
		}
		#endregion
	
	}

    public enum eType
    {
        String, Int, Double, Boolean, XML, Char, ItemsCollection, Decimal
    }

    public enum eFontStyle
    {
        Bold, Italic, Underline, Plain
    }

    public enum eFontSize
    {
        Large, Medium, Small
    }
	
    public enum eBool
    {
        True, False
    }

    /// <summary>
    /// Class mô tả 1 attribute
    /// </summary>
    public class CAttribute
    {
        string strName;

        public string StrName
        {
            get { return strName; }
            set { strName = value; }
        }
        eType valueType;

        public eType ValueType
        {
            get { return valueType; }
            set { valueType = value;
                switch (valueType)
                {
                    case eType.String:
                        break;
                    case eType.Int:
                        break;
                    case eType.Double:
                        break;
                    case eType.Boolean:
                        objValue = new eBool();
                        break;
                    case eType.XML:
                        break;
                    case eType.Char:
                        break;
                    case eType.ItemsCollection:
                        break;
                    case eType.Decimal:
                        break;
                    default:
                        break;
                }
            }
        }

        private object objValue = null;

        public object ObjValue
        {
            get { return objValue; }
            set { objValue = value; }
        }

        public CAttribute(string sName, object value)
        {
            this.strName = sName;
            this.objValue = value;
        }
    }

    /// <summary>
	/// Custom PropertyDescriptor
	/// </summary>
    public class CustomPropertyDescriptor : PropertyDescriptor
    {
        CAttribute m_Property;
        public CustomPropertyDescriptor(ref CAttribute myProperty, Attribute[] attrs)
            : base(myProperty.StrName, attrs)
        {
            m_Property = myProperty;
        }

        #region PropertyDescriptor specific

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get
            {
                return null;
            }
        }

        public override object GetValue(object component)
        {
            return m_Property.ObjValue;
        }

        public override string Description
        {
            get
            {
                return m_Property.StrName;
            }
        }

        public override string Category
        {
            get
            {
                return string.Empty;
            }
        }

        public override string DisplayName
        {
            get
            {
                return m_Property.StrName;
            }

        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override void ResetValue(object component)
        {
            //Have to implement
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override void SetValue(object component, object value)
        {
            m_Property.ObjValue = value;
        }

        public override Type PropertyType
        {
            get { return m_Property.ObjValue.GetType(); }
        }
		
        #endregion
    }
}
