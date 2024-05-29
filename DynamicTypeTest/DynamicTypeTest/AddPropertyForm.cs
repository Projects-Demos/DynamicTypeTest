using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamicTypeTest
{
    public partial class AddPropertyForm : Form
    {
        private TextBox txtPropertyName;
        private TextBox txtPropertyValue;
        private Button btnAdd;
        private Button btnCancel;

        public string PropertyName { get; private set; }
        public string PropertyValue { get; private set; }
        public AddPropertyForm()
        {
            InitializeComponent();

            txtPropertyName = new TextBox { Dock = DockStyle.Top, Text = "Property Name" };
            txtPropertyValue = new TextBox { Dock = DockStyle.Top, Text = "Property Value" };
            btnAdd = new Button { Text = "Add", Dock = DockStyle.Top };
            btnCancel = new Button { Text = "Cancel", Dock = DockStyle.Top };

            btnAdd.Click += (sender, e) =>
            {
                PropertyName = txtPropertyName.Text;
                PropertyValue = txtPropertyValue.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            btnCancel.Click += (sender, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.Add(txtPropertyValue);
            this.Controls.Add(txtPropertyName);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnCancel);

            this.Text = "Add Property";
            this.Height = 160;
            this.Width = 200;
            this.StartPosition = FormStartPosition.CenterParent;

        }
    }
}
