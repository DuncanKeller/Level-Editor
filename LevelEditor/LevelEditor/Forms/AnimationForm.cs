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
        List<Animation> anims = new List<Animation>();
        int move = 50;
        GraphicsDevice graphics;
        Bitmap setimg;

        public AnimationForm()
        {
            InitializeComponent();
        }

        public void Init(GraphicsDevice g)
        {
            graphics = g;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (current != null)
            {
                Texture2D t = LoadImage();
                current.frames.Add(t);
                RefreshItems();
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
            current = anims[listBox1.SelectedIndex];
            move = 0;
            RefreshItems();
        }

        private void RefreshItems()
        {
            pics.Clear();

            for (int i = 0; i < current.frames.Count; i++)
            {
                MemoryStream mem = new MemoryStream();
                PictureBox p = new PictureBox();
                p.Location = new System.Drawing.Point(70 + (i * 100) + (10 * i) + move, 460);
                p.Size = new Size(100, 100);
                Texture2D texutre = current.frames[i];
                texutre.SaveAsJpeg(mem, texutre.Width, texutre.Height);
                Bitmap image = new Bitmap(mem);
                p.Image = image;
                setimg = image;
                p.Click += new EventHandler(FrameClicked);
                Controls.Add(p);
                pics.Add(p);
            }
        }

        private void FrameClicked(object sender, EventArgs e)
        {
            pictureBox1.Image = (sender as PictureBox).Image = setimg;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private Texture2D LoadImage()
        {
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName;
            Stream s = File.Open(path, FileMode.Open);
            Texture2D tex = null;

            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '\\')
                {
                    string n = path.Substring(i + 1, path.Length - i - 1);
                    tex = Texture2D.FromStream(graphics, s);
                    break;
                }
            }

            return tex;
            s.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Animation a = new Animation();
            anims.Add(a);
            listBox1.Items.Add("default");
            current = a;
        }

    }

    public class Animation
    {
        public string name;
        public float speed;
        public int loopback;
        public List<Texture2D> frames = new List<Texture2D>();

        public Animation()
        {
            name = "default";
            speed = 15;
            loopback = 0;
        }
    }
}
