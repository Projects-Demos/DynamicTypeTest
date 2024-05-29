using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
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
        MyDynamicClass myDynamicClass = new MyDynamicClass();
        string xmlPath = "MyDynamicClassProperties.xml";
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

            this.Load += new System.EventHandler(this.MainForm_Load);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                myDynamicClass.LoadPropertiesFromXmlFile(xmlPath);
                propertyGrid.SelectedObject = myDynamicClass;

                //var descriptor = LoadPropertiesFromXml("properties.xml");
                //propertyGrid.SelectedObject = descriptor;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading XML: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 按钮点击事件用于保存属性
        private void btnSaveProperties_Click(object sender, EventArgs e)
        {
            var descriptor = (MyDynamicClass)propertyGrid.SelectedObject;
            SavePropertiesToXml(xmlPath, descriptor);
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
            var descriptor = (MyDynamicClass)propertyGrid.SelectedObject;
            descriptor.SetDynamicProperty(name, value, category, displayName);
            // 刷新PropertyGrid
            propertyGrid.Refresh();

        }

        #region ExpandoObject
        private ExpandoObject LoadExpandoFromXml(string filePath)
        {
            var expando = new ExpandoObject();
            var expandoDict = (IDictionary<string, object>)expando;

            // 加载XML文档
            var document = XDocument.Load(filePath);

            // 解析Properties元素并添加到ExpandoObject
            foreach (var propertyElement in document.Descendants("Property"))
            {
                string name = propertyElement.Attribute("Name").Value;
                string value = propertyElement.Attribute("Value").Value;
                expandoDict[name] = value;
            }

            return expando;
        }

        private void SaveExpandoToXml(string filePath, ExpandoObject expando)
        {
            var propertiesElement = new XElement("Properties");

            foreach (var kvp in (IDictionary<string, object>)expando)
            {
                propertiesElement.Add(new XElement("Property",
                    new XAttribute("Name", kvp.Key),
                    new XAttribute("Value", kvp.Value)));
            }

            var document = new XDocument(new XElement("Root", propertiesElement));
            document.Save(filePath);
        }
        #endregion


        #region DynamicTypeDescriptor
        // 保存属性到XML文件
        private void SavePropertiesToXml(string xmlFilePath, MyDynamicClass descriptor)
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
        #endregion

    }
}
