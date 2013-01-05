using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LevelEditor
{
    public partial class NameForm : Form
    {
        public NameForm()
        {
            InitializeComponent();
        }

        NameAction callback;

        public string NameText
        {
            get { return text.Text; }
        }

        public void Init(NameAction a, string n)
        {
            callback = a;
            text.Text = n;
            text.SelectAll();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void OK_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Save();
            }
        }

        public void Save()
        {
            callback(text.Text);
            Close();
        }
    }
}
