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
    public partial class TagForm : Form
    {
        public TagForm()
        {
            InitializeComponent();
        }

        NameAction callback;

        public void Init(NameAction a, List<string> t)
        {
            callback = a;

            foreach (string s in t)
            {
                tagBox.Items.Add(s);
            }
            
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
                tagBox.Items.Add(text.Text);
            }
            text.Clear();
        }

        public void Save()
        {
            string tags = "";
            foreach (object tag in tagBox.Items)
            {
                tags += tag + ";";
            }
            callback(tags);
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tagBox.Items.Add(text.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tagBox.Items.Remove(tagBox.SelectedItem);
        }
    }
}
