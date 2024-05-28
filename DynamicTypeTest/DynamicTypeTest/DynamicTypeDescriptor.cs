
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DynamicTypeTest
{
	public class DynamicTypeDescriptor : ICustomTypeDescriptor
	{
		private PropertyDescriptorCollection _properties;

		public DynamicTypeDescriptor()
		{
			_properties = new PropertyDescriptorCollection(null);
		}

		public void AddProperty(string name, Type type, object value, string category, string displayName)
		{
			var property = new DynamicPropertyDescriptor(name, type, value, category, displayName);
			_properties.Add(property);
		}

		// Implementation of ICustomTypeDescriptor:
		public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(this, true);
		public string GetClassName() => TypeDescriptor.GetClassName(this, true);
		public string GetComponentName() => TypeDescriptor.GetComponentName(this, true);
		public TypeConverter GetConverter() => TypeDescriptor.GetConverter(this, true);
		public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(this, true);
		public PropertyDescriptor GetDefaultProperty() => TypeDescriptor.GetDefaultProperty(this, true);
		public object GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor(this, editorBaseType, true);
		public EventDescriptorCollection GetEvents(Attribute[] attributes) => TypeDescriptor.GetEvents(this, attributes, true);
		public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents(this, true);
		public PropertyDescriptorCollection GetProperties(Attribute[] attributes) => _properties;
		public PropertyDescriptorCollection GetProperties() => _properties;
		public object GetPropertyOwner(PropertyDescriptor pd) => this;
	}

	public class DynamicPropertyDescriptor : PropertyDescriptor
	{
		private readonly Type _type;
		private object _value;

		public DynamicPropertyDescriptor(string name, Type type, object value, string category, string displayName)
			: base(name, new Attribute[] { new CategoryAttribute(category), new DisplayNameAttribute(displayName) })
		{
			_type = type;
			_value = value;
		}

		public override bool CanResetValue(object component) => false;
		public override Type ComponentType => _type;
		public override object GetValue(object component) => _value;
		public override bool IsReadOnly => false;
		public override Type PropertyType => _type;
		public override void ResetValue(object component) { }
		public override void SetValue(object component, object value) => _value = value;
		public override bool ShouldSerializeValue(object component) => false;
	}
}