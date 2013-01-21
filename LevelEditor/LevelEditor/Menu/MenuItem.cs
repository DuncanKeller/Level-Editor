using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    public delegate void MenuAction();

    class MenuItem
    {
        public static int defaultHeight = 50;

        protected SpriteFont font;
        protected Vector2 pos;
        Vector2 destination;
        protected Texture2D texture;
        string text;
        int width;
        int height;
        protected Menu m;
        bool selected = false;
        MenuAction action;

        Color c = Color.Black;
        Color backColor = new Color(50, 50, 150, 150);

        public Color Color
        {
            get { return c; }
            set { c = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        public Vector2 Dest
        {
            get { return destination; }
            set { destination = value; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public MenuItem(string text, Texture2D texture, Vector2 pos, int w, int h, bool selectable, Color c, Menu m, MenuAction a)
        {
            this.text = text;
            this.texture = texture;
            this.pos = pos;
            destination = pos;
            font = TextureManager.FontMap["menuFont"];
            this.m = m;
            action = a;
            this.width = w;
            this.height = h;
            this.c = c;
        }

        public virtual void Evoke()
        {
            action();
        }

        public void SetSize(int w, int h)
        {
            width = w;
            height = h;
        }

        public virtual void ChangePosition(Vector2 newPos)
        {
            destination = newPos;
        }

        public virtual void Update(float dt)
        {
            pos = Vector2.Lerp(pos, destination, 0.10f);

            if (Input.Overlapping(
                new Rectangle((int)(pos.X + m.Position.X), (int)(pos.Y + m.Position.Y), 
                    width, height)) &&
                Input.LeftClick())
            {
                Evoke();
            }
        }

        public void Draw(SpriteBatch sb)
        {

            int s = selected ? 100 : 0;

            sb.Draw(TextureManager.TexMap["blank"], new Rectangle(
                (int)(pos.X + m.Position.X), (int)(pos.Y + m.Position.Y), width, height), backColor);

            if (texture != null)
            {
                sb.Draw(texture, new Rectangle((int)(pos.X + m.Position.X) + 2, (int)(pos.Y + m.Position.Y) + 2, width - 4, height - 4), c);
            }
            else
            {
                sb.DrawString(font, text, pos, c);
            }

        }
    }
}
