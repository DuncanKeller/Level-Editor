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
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;

namespace LevelEditor
{
    public partial class AnimationForm : Form
    {
        Entity entity;
        Animation current;
        List<PictureBox> pics = new List<PictureBox>();
        List<Animation> anims = new List<Animation>();
        int move = 50;
        GraphicsDevice graphics;

        int currFrame = 0;
        int colIndex = 0;

        MenuAction callback;

        public AnimationForm()
        {
            InitializeComponent();
        }

        public void Init(GraphicsDevice g, Entity e, MenuAction c)
        {
            entity = e;
            graphics = g;
            callback = c;

            if (File.Exists("blueprints\\anim\\" + e.Name + ".json"))
            {
                StreamReader sr = new StreamReader("blueprints\\anim\\" + e.Name + ".json");
                string json = sr.ReadToEnd();
                sr.Close();
                LoadAnim(json);
            }
            foreach (Animation a in anims)
            {
                listBox1.Items.Add(a.name);
            }
        }

        public void LoadAnim(string unparsedJson)
        {
            JObject json = JObject.Parse(unparsedJson);
            JArray animations = (JArray)json["animations"];
            for (int i = 0; i < animations.Count; i++)
            {
                anims.Add(new Animation());
                JObject animation = (JObject)(animations[i]);
                anims[i].name = (string)animation["name"];
                anims[i].speed = (float)animation["speed"];
                anims[i].loopback = (int)animation["loop"];
                anims[i].nFrames = (int)animation["frames"];

                JArray collisionArray = (JArray)animation["collisionVolumes"];
                for (int ca = 0; ca < collisionArray.Count; ca++)
                {
                    JObject volumeContainer = (JObject)(collisionArray[ca]);
                    JArray volumes = (JArray)volumeContainer["volumes"];
                    anims[i].collision.Add(new List<CollisionList>());

                    for (int v = 0; v < volumes.Count; v++)
                    {
                        JObject volume = (JObject)(volumes[v]);

                        float centerX = (float)volume["centerX"];
                        float centerY = (float)volume["centerY"];
                        bool physical = (bool)volume["physical"];
                        CollisionList cl = new CollisionList();
                        JArray points = (JArray)volume["xpoints"];
                        List<float> xpoints = new List<float>();
                        for (int p = 0; p < points.Count; p++)
                        {
                            xpoints.Add((float)points[p] + centerX);
                        }
                        points = (JArray)volume["ypoints"];
                        List<float> ypoints = new List<float>();
                        for (int p = 0; p< points.Count; p++)
                        {
                            ypoints.Add((float)points[p] + centerY);
                        }
                        for (int p = 0; p < xpoints.Count; p++)
                        {
                            cl.Add(xpoints[p], ypoints[p]);
                        }

                        anims[i].collision[ca].Add(cl);
                    }
                }
            }
            foreach (Animation anim in anims)
            {
                if (Directory.Exists("images\\anim\\" + entity.Name + "\\" + anim.name))
                {
                    for (int i = 0; i < anim.nFrames; i++)
                    {
                        Stream s = File.OpenRead("images\\anim\\" + entity.Name + "\\" + anim.name + "\\" + i + ".png");
                        Texture2D texture = Texture2D.FromStream(graphics, s);
                        anim.frames.Add(texture);
                    }
                }
            }
        }

        public void SaveAnim(string name)
        {
            if (!Directory.Exists("blueprints\\anim"))
            { Directory.CreateDirectory("blueprints\\anim"); }

            FileStream fs = File.Open("blueprints\\anim\\" + name + ".json", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            JsonTextWriter jw = new JsonTextWriter(sw);
            jw.Formatting = Formatting.Indented;
            
            jw.WriteStartObject();
            //anim
            jw.WritePropertyName("animations");
            jw.WriteStartArray();
            foreach (Animation a in anims)
            {
                jw.WriteStartObject();

                jw.WritePropertyName("name");
                jw.WriteValue(a.name);
                jw.WritePropertyName("speed");
                jw.WriteValue(a.speed);
                jw.WritePropertyName("loop");
                jw.WriteValue(a.loopback);
                jw.WritePropertyName("frames");
                jw.WriteValue(a.frames.Count);

                //collision
                jw.WritePropertyName("collisionVolumes");
                jw.WriteStartArray();
                for (int i = 0; i < a.frames.Count; i++)
                {
                    if (a.collision[i].Count > 0)
                    {
                        jw.WriteStartObject();
                        jw.WritePropertyName("volumes");
                        jw.WriteStartArray();
                        foreach (CollisionList cl in a.collision[i])
                        {
                            jw.WriteStartObject();
                            jw.WritePropertyName("physical");
                            jw.WriteValue(cl.Physical);
                            jw.WritePropertyName("centerX");
                            jw.WriteValue(a.frames[i].Width / 2);
                            jw.WritePropertyName("centerY");
                            jw.WriteValue(a.frames[i].Height / 2);
                            jw.WritePropertyName("xpoints");
                            jw.WriteStartArray();
                            foreach (CollisionPoint p in cl.Nodes)
                            {
                                jw.WriteValue(p.X - (a.frames[i].Width / 2));
                            }
                            jw.WriteEnd();
                            jw.WritePropertyName("ypoints");
                            jw.WriteStartArray();
                            foreach (CollisionPoint p in cl.Nodes)
                            {
                                jw.WriteValue(p.Y - (a.frames[i].Height / 2));
                            }
                            jw.WriteEnd();
                            jw.WriteEnd();
                        }
                        jw.WriteEnd();
                        jw.WriteEnd();
                    }
                    else
                    {
                        jw.WriteWhitespace("");
                    }
                }
                jw.WriteEnd();
                // end anim
                jw.WriteEnd();
            }
            jw.WriteEnd();

            jw.WriteEnd();
            jw.Close();

            if (!Directory.Exists("images\\anim"))
            { Directory.CreateDirectory("images\\anim"); }

            if (!Directory.Exists("images\\anim\\" + name))
            { Directory.CreateDirectory("images\\anim\\" + name); }
            foreach (Animation a in anims)
            {
                if (!Directory.Exists("images\\anim\\" + name + "\\" + a.name))
                { Directory.CreateDirectory("images\\anim\\" + name + "\\" + a.name); }
                int frameNum = 0;
                foreach (Texture2D t in a.frames)
                {
                    Stream s = File.OpenWrite("images\\anim\\" + name + "\\" + a.name + "\\" + frameNum + ".png");
                    t.SaveAsPng(s, t.Width, t.Height);
                    s.Close();
                    frameNum++;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (current != null)
            {
                Texture2D t = LoadImage();
                current.frames.Add(t);
                current.collision.Add(new List<CollisionList>());
                current.collision[current.frames.Count - 1].Add(new CollisionList());
                RefreshItems();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (current != null)
            {
                foreach (PictureBox p in pics)
                {
                    p.Location = new System.Drawing.Point(p.Location.X - move, p.Location.Y);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (current != null)
            {
                foreach (PictureBox p in pics)
                {
                    p.Location = new System.Drawing.Point(p.Location.X + move, p.Location.Y);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                current = anims[listBox1.SelectedIndex];
                frame.SetList(null);
                move = 0;
                RefreshItems();
                RefreshCol();
                currFrame = 0;
                if (current.frames.Count == 0)
                {
                    frame.SetImage(null);
                }
                else
                {
                    frame.SetImage(current.frames[0]);
                }
            }
        }

        private void RefreshItems()
        {
            foreach (PictureBox p in pics)
            {
                Controls.Remove(p);
            }

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
                p.Click += new EventHandler(FrameClicked);
                Controls.Add(p);
                pics.Add(p);
            }

            textBox1.Text = current.name;
        }

        private void FrameClicked(object sender, EventArgs e)
        {
            //pictureBox1.Image = (sender as PictureBox).Image = setimg;
            int i = 0;
            foreach (PictureBox p in pics)
            {
                int mx = Cursor.Position.X;
                int my = Cursor.Position.Y;
                if (sender == p)
                {
                    Texture2D image = current.frames[i];
                    frame.SetImage(image);
                    currFrame = i;
                    frame.SetList(current.collision[i]);
                    RefreshCol();
                    break;
                }
                i++;
            }
        }

        private void RefreshCol()
        {
            if (current.frames.Count > 0)
            {
                if (colIndex >= current.collision[currFrame].Count)
                { colIndex = current.collision[currFrame].Count - 1; }
                if (colIndex < 0)
                { colIndex = 0; }
                textBox5.Text = current.collision[currFrame][colIndex].Physical.ToString();
                textBox4.Text = (colIndex + 1).ToString();
                textBox6.Text = current.collision[currFrame].Count.ToString();
                frame.SetIndex(colIndex);
                //frame.SetImage(current.frames[0]);
                //currFrame = 0;
            }
            else
            {
                textBox5.Text = "";
                textBox4.Text = "";
                textBox6.Text = "";
                frame.SetIndex(0);
                frame.SetImage(null);
                //currFrame = 0;
            }

            textBox1.Text = current.name;
            textBox2.Text = current.speed.ToString();
            textBox3.Text = current.loopback.ToString();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //if (current != null)
            //{
            //    Texture2D t = LoadImage();
            //    current.frames.Add(t);
            //    RefreshItems();
            //}
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

            s.Close();
            return tex;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveAnim(entity.Name);
            callback();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Animation a = new Animation();
            a.collision.Add( new List<CollisionList>());
            anims.Add(a);
            listBox1.Items.Add("default");
            current = a;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (current != null)
            {
                current.name = textBox1.Text;
                listBox1.Items[listBox1.SelectedIndex] = textBox1.Text;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (current != null)
            {
                current.speed = float.Parse(textBox2.Text);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (current != null)
            {
                current.loopback = int.Parse(textBox3.Text);
            }
        }

        private void frame_Click(object sender, EventArgs e)
        {
            if (current != null)
            {
                System.Drawing.Point p = PointToClient(Cursor.Position);
                current.collision[currFrame][colIndex].Add(
                    p.X - frame.Location.X,
                    p.Y - frame.Location.Y);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            colIndex++;
            if (colIndex >= current.collision[currFrame].Count)
            { colIndex = 0; }
            textBox4.Text = (colIndex + 1).ToString();
            frame.SetIndex(colIndex);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            frame.ToggelVis();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            current.collision[currFrame][colIndex].Physical = !current.collision[currFrame][colIndex].Physical;
            textBox5.Text = current.collision[currFrame][colIndex].Physical.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (current.collision[currFrame].Count < 4)
            {
                current.collision[currFrame].Add(new CollisionList());
            }
            textBox6.Text = current.collision[currFrame].Count.ToString();

            button8_Click(this, new EventArgs());
        }

        private void AnimationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            callback();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (currFrame < current.frames.Count - 1)
            {
                if (current.collision[currFrame + 1].Count < colIndex)
                { current.collision[currFrame + 1].Add(null); }

                current.collision[currFrame + 1][colIndex] = current.collision[currFrame][colIndex];
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (currFrame < current.frames.Count - 1)
            {
                if (current.collision[currFrame + 1].Count < colIndex)
                { current.collision[currFrame + 1].Add(null); }

                CollisionList cl = new CollisionList();
                cl.Clone(current.collision[currFrame][colIndex]);
                current.collision[currFrame + 1][colIndex] = cl;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (current.collision[currFrame].Count > 1)
            {
                current.collision[currFrame].RemoveAt(colIndex);
                colIndex--;
                if (colIndex < 0)
                { colIndex = 0; }
                RefreshCol();
            }
            else
            {
                current.collision[currFrame][colIndex].RemoveHead();
            }
        }
    }

    public class Animation
    {
        public string name;
        public float speed;
        public int loopback;
        public List<Texture2D> frames = new List<Texture2D>();
        public List<List<CollisionList>> collision = new List<List<CollisionList>>();
        public int nFrames;

        public Animation()
        {
            name = "default";
            speed = 15;
            loopback = 0;
        }
    }
}
