using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    class Menu
    {
        int x;
        int y;
        protected int w;
        protected int h;

        protected List<MenuItem> items = new List<MenuItem>();

        public Vector2 Position
        {
            get { return new Vector2(x, y); }
        }

        public Menu(float x, float y, float w, float h)
        {
            this.x = (int)x;
            this.y = (int)y;
            this.w = (int)w;
            this.h = (int)h;
        }

        public void AddItem(string text, Texture2D texture, Vector2 pos, Color c, MenuAction a, bool half = false)
        {
            int w = this.w - 6;
            if (half)
            {
                w = MenuItem.defaultHeight;
            }

            items.Add(new MenuItem(text, texture, new Vector2(pos.X + 3, pos.Y + 2),
                w , MenuItem.defaultHeight, true, c, this, a));
        }

        public virtual void Update(float dt)
        {
            foreach (MenuItem i in items)
            {
                i.Update(dt);
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.TexMap["blank"], new Rectangle(x, y, w, h), new Color(0, 0, 150, 150));

            foreach (MenuItem i in items)
            {
                i.Draw(sb);
            }
        }
    }
}
