﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LevelEditor
{
    public partial class Scripting : Form
    {
        string name;
        public Scripting()
        {
            InitializeComponent();
        }

        public void Init(string name)
        {
            this.name = name;
            if (File.Exists("scripts\\" + name + ".script"))
            {
                StreamReader sr = new StreamReader("scripts\\" + name + ".script");
                string script = sr.ReadToEnd();
                sr.Close();
                textBox1.Text = script;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string script = textBox1.Text;
            StreamWriter sw = new StreamWriter("scripts\\" + name + ".script");
            sw.Write(script);
            sw.Flush();
            sw.Close();
            this.Close();
        }
    }
}