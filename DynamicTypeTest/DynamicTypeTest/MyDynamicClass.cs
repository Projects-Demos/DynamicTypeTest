using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DynamicTypeTest
{

    public class MyDynamicClass : ICustomTypeDescriptor
    {
        // 假设我们有一些已知的属性
        public string Name { get; set; }
        public int Age { get; set; }

        private DynamicTypeDescriptor _typeDescriptor = new DynamicTypeDescriptor();

        public void SetDynamicProperty(string propertyName, object value, string category, string displayName)
        {
            // 这里我们假设属性类型不会改变，所以如果已经存在，则只更新值
            var prop = _typeDescriptor.GetProperties().Find(propertyName, false);
            if (prop != null)
            {
                ((DynamicPropertyDescriptor)prop).SetValue(this, value);
            }
            else
            {
                _typeDescriptor.AddProperty(propertyName, value.GetType(), value, category, displayName);
            }
        }

        public void LoadPropertiesFromXml(string xml)
        {
            var document = XDocument.Parse(xml);
            foreach (var prop in document.Root.Elements("Property"))
            {
                string name = prop.Attribute("Name").Value;
                string typeAsString = prop.Attribute("Type").Value;
                string valueAsString = prop.Attribute("Value").Value;
                string category = prop.Attribute("Category").Value;
                string displayName = prop.Attribute("DisplayName").Value;

                // 使用Type.GetType来获取Type实例
                Type type = Type.GetType(typeAsString);

                // 将字符串值转换为正确的类型
                object typedValue = Convert.ChangeType(valueAsString, type);

                SetDynamicProperty(name, typedValue, category, displayName);
            }
        }
        public void LoadPropertiesFromXmlFile(string xmlFilePath)
        {
            XDocument doc = XDocument.Load(xmlFilePath);
            foreach (var prop in doc.Root.Elements("Property"))
            {
                string name = prop.Attribute("Name").Value;
                string typeAsString = prop.Attribute("Type").Value;
                string valueAsString = prop.Attribute("Value").Value;
                string category = prop.Attribute("Category").Value;
                string displayName = prop.Attribute("DisplayName").Value;

                // 使用Type.GetType来获取Type实例
                Type type = Type.GetType(typeAsString);

                // 将字符串值转换为正确的类型
                object typedValue = Convert.ChangeType(valueAsString, type);

                SetDynamicProperty(name, typedValue, category, displayName);
            }
        }



        // ICustomTypeDescriptor 接口实现，委托给内部的 DynamicTypeDescriptor 实例
        public AttributeCollection GetAttributes() => _typeDescriptor.GetAttributes();
        public string GetClassName() => _typeDescriptor.GetClassName();
        public string GetComponentName() => _typeDescriptor.GetComponentName();
        public TypeConverter GetConverter() => _typeDescriptor.GetConverter();
        public EventDescriptor GetDefaultEvent() => _typeDescriptor.GetDefaultEvent();
        public PropertyDescriptor GetDefaultProperty() => _typeDescriptor.GetDefaultProperty();
        public object GetEditor(Type editorBaseType) => _typeDescriptor.GetEditor(editorBaseType);
        public EventDescriptorCollection GetEvents(Attribute[] attributes) => _typeDescriptor.GetEvents(attributes);
        public EventDescriptorCollection GetEvents() => _typeDescriptor.GetEvents();
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes) => _typeDescriptor.GetProperties(attributes);
        public PropertyDescriptorCollection GetProperties() => _typeDescriptor.GetProperties();
        public object GetPropertyOwner(PropertyDescriptor pd) => _typeDescriptor.GetPropertyOwner(pd);
    }
}
