using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    public partial class TextureBank : Form
    {
        public TextureBank()
        {
            InitializeComponent();
        }

        GraphicsDevice graphics;
        Dictionary<string, Texture2D> textures = new Dictionary<string,Texture2D>();
        TextureAction callback;

        public Dictionary<string, Texture2D> Textures
        {
            get { return textures; }
            set { textures = value; }
        }

        private void TextureBank_Load(object sender, EventArgs e)
        {
            textures = new Dictionary<string, Texture2D>();
        }

        public void Init(GraphicsDevice g)
        {
            graphics = g;
        }

        public void SetCallback(TextureAction a)
        {
            callback = a;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MemoryStream mem = new MemoryStream();

            string img = textureList.SelectedItem.ToString();
            Texture2D texutre = textures[img];
            texutre.SaveAsJpeg(mem, texutre.Width, texutre.Height);
            Bitmap image = new Bitmap(mem);

            pictureBox1.Image = image;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            foreach (string path in openFileDialog1.FileNames)
            {
                Stream s = File.Open(path, FileMode.Open);
                
                for (int i = path.Length - 1; i >= 0; i--)
                {
                    if (path[i] == '\\')
                    {
                        string n = path.Substring(i + 1, path.Length - i - 1);
                        textures.Add(n, Texture2D.FromStream(graphics, s));
                        textureList.Items.Add(n);
                        break;
                    }
                }
                s.Close();
            }
        }

        private void selectButton_Click(object sender, EventArgs e)
        {
            if (textureList.SelectedItem != null)
            {
                string img = textureList.SelectedItem.ToString();
                Texture2D texutre = textures[img];
                callback(texutre, img);
            }
            Hide();
        }

        public void SaveTextures()
        {
            foreach (KeyValuePair<string, Texture2D> texture in textures)
            {
                Stream sw = File.OpenWrite("images\\" + texture.Key);
                texture.Value.SaveAsPng(sw, texture.Value.Width, texture.Value.Height);
            }
        }
    }
}
