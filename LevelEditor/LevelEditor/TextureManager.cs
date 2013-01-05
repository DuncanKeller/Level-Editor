using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    static class TextureManager
    {
        static ContentManager c;
        static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public static Dictionary<string, Texture2D> TexMap
        {
            get
            {
                return textures;
            }
        }

        public static Dictionary<string, SpriteFont> FontMap
        {
            get
            {
                return fonts;
            }
        }

        public static void Init(ContentManager content)
        {
            c = content;
        }

        public static void Load()
        {
            LoadImage("circle");
            LoadImage("blank");
            LoadImage("finger");

            LoadFont("menuFont");
        }

        public static void LoadImage(string s)
        {
            textures.Add(s, c.Load<Texture2D>(s));
        }

        public static void LoadFont(string s)
        {
            fonts.Add(s, c.Load<SpriteFont>("fonts\\" + s));
        }
    }
}
