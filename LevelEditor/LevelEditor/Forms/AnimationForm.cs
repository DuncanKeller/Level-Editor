using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace LevelEditor
{
    public partial class AnimationForm : Form
    {
        Animation current;
        List<PictureBox> pics = new List<PictureBox>();
        Dictionary<string, Animation> anims = new Dictionary<string, Animation>();
        int move = 50;

        public AnimationForm()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (current != null)
            {
                //current.frames.Add(new Texture2D());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (current != null)
            {
                foreach (PictureBox p in pics)
                {
                    p.Location = new System.Drawing.Point(p.Location.X - move, p.Location.X);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (current != null)
            {
                foreach (PictureBox p in pics)
                {
                    p.Location = new System.Drawing.Point(p.Location.X + move, p.Location.X);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            current = anims[(string)listBox1.SelectedItem];

        }

        private void Refresh()
        {
            pics.Clear();

            for (int i = 0; i < current.frames.Count; i++)
            {
                MemoryStream mem = new MemoryStream();
                PictureBox p = new PictureBox();
                p.Location = new System.Drawing.Point(50 + (i * 100) + (10 * i), 600);
                p.Size = new Size(100, 100);
                Texture2D texutre = current.frames[i];
                texutre.SaveAsJpeg(mem, texutre.Width, texutre.Height);
                Bitmap image = new Bitmap(mem);
                p.Image = image;
                Controls.Add(p);
                pics.Add(p);
            }
        }

   
   

    }

    public class Animation
    {
        public string name;
        public float speed;
        public int loopback;
        public List<Texture2D> frames = new List<Texture2D>();
    }
}
