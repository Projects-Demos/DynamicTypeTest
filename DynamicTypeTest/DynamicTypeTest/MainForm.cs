using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DynamicTypeTest
{
    public partial class MainForm : Form
    {
        private PropertyGrid propertyGrid;
        private Button btnAddProperty;
        private Button btnSaveProperties;
        public MainForm()
        {
            InitializeComponent();

            this.Text = "Dynamic PropertyGrid Example";
            this.Width = 800;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            // 初始化 PropertyGrid
            propertyGrid = new PropertyGrid();
            propertyGrid.Dock = DockStyle.Fill;

            // 初始化添加属性的按钮
            btnAddProperty = new Button();
            btnAddProperty.Text = "Add Property";
            btnAddProperty.Dock = DockStyle.Top;
            btnAddProperty.Click += btnAddProperty_Click; // 为按钮添加事件处理

            // 初始化保存属性到XML的按钮
            btnSaveProperties = new Button();
            btnSaveProperties.Text = "Save Properties";
            btnSaveProperties.Dock = DockStyle.Top;
            btnSaveProperties.Click += btnSaveProperties_Click; // 为按钮添加事件处理

            // 将控件添加到窗体
            this.Controls.Add(propertyGrid);
            this.Controls.Add(btnSaveProperties);
            this.Controls.Add(btnAddProperty);



            LoadPropertiesFromXml("properties.xml");
        }

        private void LoadPropertiesFromXml(string xmlFilePath)
        {
            var descriptor = new DynamicTypeDescriptor();
            XDocument doc = XDocument.Load(xmlFilePath);

            foreach (XElement propertyElement in doc.Descendants("Property"))
            {
                string name = propertyElement.Element("Name").Value;
                Type type = Type.GetType(propertyElement.Element("Type").Value);
                object value = Convert.ChangeType(propertyElement.Element("Value").Value, type);
                string category = propertyElement.Element("Category").Value;
                string displayName = propertyElement.Element("DisplayName").Value;

                descriptor.AddProperty(name, type, value, category, displayName);
            }

            propertyGrid.SelectedObject = descriptor;
        }
        // 保存属性到XML文件
        private void SavePropertiesToXml(string xmlFilePath, DynamicTypeDescriptor descriptor)
        {
            var properties = new XElement("Properties");

            foreach (DynamicPropertyDescriptor prop in descriptor.GetProperties())
            {
                var property = new XElement("Property",
                    new XElement("Name", prop.Name),
                    new XElement("Type", prop.PropertyType.AssemblyQualifiedName),
                    new XElement("Value", prop.GetValue(descriptor)?.ToString() ?? ""),
                    new XElement("Category", prop.Category),
                    new XElement("DisplayName", prop.DisplayName)
                );
                properties.Add(property);
            }

            var doc = new XDocument(properties);
            doc.Save(xmlFilePath);
        }

        // 按钮点击事件用于保存属性
        private void btnSaveProperties_Click(object sender, EventArgs e)
        {
            var descriptor = (DynamicTypeDescriptor)propertyGrid.SelectedObject;
            SavePropertiesToXml("properties.xml", descriptor);
        }

        // 按钮点击事件用于添加属性
        private void btnAddProperty_Click(object sender, EventArgs e)
        {
            // 弹出对话框，收集属性信息
            // 这里简化为直接添加一个属性
            string name = "NewProperty";
            Type type = typeof(string);
            object value = "DefaultValue";
            string category = "Dynamic";
            string displayName = "New Property";

            // 更新动态类型描述器
            var descriptor = (DynamicTypeDescriptor)propertyGrid.SelectedObject;
            descriptor.AddProperty(name, type, value, category, displayName);

            // 刷新PropertyGrid
            propertyGrid.Refresh();
        }
    }
}
