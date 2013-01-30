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
        protected bool inUse = false;


        protected List<MenuItem> items = new List<MenuItem>();

        public bool InUse
        {
            get { return inUse; }
        }

        public Vector2 Position
        {
            get { return new Vector2(x, y); }
        }

        public Rectangle Rect
        {
            get { return new Rectangle(x, y, w, h); }
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
                w, MenuItem.defaultHeight, true, c, this, a));
        }

        public void AddItem(string text, Texture2D texture, Vector2 pos, Color c, MenuActionS a, string bpn)
        {
            int w = this.w - 6;

            items.Add(new MenuItemBlueprint(text, texture, new Vector2(pos.X + 3, pos.Y + 2),
                w, MenuItem.defaultHeight, true, c, this, a, bpn));
        }

        public void AddItem(string text, Texture2D texture, Vector2 pos, Color c, LayerAction a, int index)
        {
            int w = this.w - 6;

            items.Add(new MenuItemLayer(text, texture, new Vector2(pos.X + 3, pos.Y + 2),
                w, MenuItem.defaultHeight, true, c, this, a, index));
        }

        protected int GetMenuItemYPos(int i)
        {
            return (MenuItem.defaultHeight * i) + (4 * i);
        }

        public virtual void Update(float dt)
        {
            foreach (MenuItem i in items)
            {
                i.Update(dt);
            }
            Scroll();
        }

        public void Scroll()
        {
            float scrollAmnt = Input.Scroll;

            if (items.Count > 0)
            {
                MoveItems(scrollAmnt);
            }
        }


        void MoveItems(float amount)
        {
            if (items.Count > 0)
            {
                if (amount > 0 &&
                    amount + items[0].Dest.Y > y + h - 10)
                {
                    //amount = (y + 10) - items[0].Dest.Y;
                }
                else if (amount < 0 &&
                    amount + items[items.Count - 1].Dest.Y < y + 10)
                {
                   // amount = (y + h - 10) - items[items.Count - 1].Dest.Y;
                }
            }

            foreach (MenuItem i in items)
            {
                i.ChangePosition(new Vector2(
                    i.Position.X,
                    i.Position.Y + amount));
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
