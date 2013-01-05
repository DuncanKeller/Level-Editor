using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LevelEditor
{
    class Entity
    {
        enum EditMode
        {
            none,
            drag,
            rotate,
            collision,
            animation
        }

        string name = "unnamed";
        Texture2D texture;
        CollisionList collision = new CollisionList();
        Rectangle rect;
        EditMode mode = EditMode.none;

        float rotationOffset;
        float rotation;
        Point translationOffset;

        #region Properties

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Rectangle Rect
        {
            get { return rect; }
            //set { rect = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                rect.Width = texture.Width;
                rect.Height = texture.Height;
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        #endregion

        public Entity(Texture2D t, int x, int y)
        {
            texture = t;
            rect = new Rectangle(x, y, t.Width, t.Height);
        }

        public Entity(Entity e)
        {
            texture = e.texture;
            rect = e.rect;
            rotation = e.rotation;
            name = "new " + e.Name;
            collision.Clone(e.collision, e);

            Translate(rect.Width, 0);
        }

        public void Update()
        {
            if (!Input.LeftHeld() &&
                mode != EditMode.collision)
            {
                mode = EditMode.none;
            }

            OpenMenu();
            Drag();
            Rotate();

            Collisions();
           
        }

        public void OpenMenu()
        {
            if (mode == EditMode.none)
            {
                if (Input.LeftClick() &&
                    Input.Overlapping(rect))
                {
                    MenuSystem.OpenEntityMenu(this);
                }
            }
        }

        public void Translate(int x, int y)
        {
            rect.X += x;
            rect.Y += y;
            collision.Translate(x, y);
        }

        public void Collisions()
        {
            if (mode == EditMode.none)
            {
                if (Input.KeyPressed(Keys.LeftControl) &&
                    MenuSystem.IsEntityMenu(this))
                { mode = EditMode.collision; }
            }
            else if (mode == EditMode.collision)
            {
                if (Input.KeyPressed(Keys.LeftControl))
                { mode = EditMode.none; }

                if (Input.LeftClick())
                {
                    collision.Add(Input.X, Input.Y);
                }
                else if (Input.RightClick())
                {
                    collision.Delete(Input.X, Input.Y);
                }
                else if (Input.KeyPressed(Keys.Back))
                {
                    collision.RemoveHead();
                }
            }
        }

        public void Rotate()
        {
            if (mode == EditMode.none)
            {
                if (Input.Overlapping(rect) &&
                    MenuSystem.IsEntityMenu(this))
                {
                    //startdrag
                    if (Input.LeftHeld() &&
                        Input.KeyHeld(Keys.LeftShift))
                    {
                        rotationOffset = -rotation - (float)Math.Atan2(Input.X - rect.Center.X, Input.Y - rect.Center.Y);
                        mode = EditMode.rotate;
                    }
                }
            }
            else if (mode == EditMode.rotate)
            {
                float r = -(float)Math.Atan2(Input.X - rect.Center.X, Input.Y - rect.Center.Y) - rotationOffset;

                collision.Rotate(r - rotation, rect.Center.X, rect.Center.Y);
                rotation = r;
            }
        }

        public void Drag()
        {
            if (mode == EditMode.none)
            {
                if (Input.Overlapping(rect))
                {
                    // start drag
                    if (Input.LeftClick() &&
                        !Input.KeyHeld(Keys.LeftShift))
                    {
                        translationOffset = new Point(Input.X - rect.X, Input.Y - rect.Y);
                        mode = EditMode.drag;
                    }
                }
            }
            else if (mode == EditMode.drag)
            {
                int x = Input.X - translationOffset.X;
                int y = Input.Y - translationOffset.Y;

                collision.Translate(x - rect.X, y - rect.Y);
                rect.X = x;
                rect.Y = y;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Color c = Color.White;

            if (mode == EditMode.collision)
            {
                c = new Color(150, 150, 150, 50);
            }

            sb.Draw(texture, new Rectangle(rect.X + texture.Width / 2, rect.Y + texture.Height / 2, rect.Width, rect.Height),
                new Rectangle(0, 0, texture.Width, texture.Height),
                c, rotation, new Vector2(texture.Width / 2, texture.Height / 2),
                SpriteEffects.None, 0);
            if (mode == EditMode.collision)
            {
                collision.Draw(sb);
            }
        }
    }
}
