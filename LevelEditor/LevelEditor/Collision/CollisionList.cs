using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    class CollisionList
    {
        CollisionPoint head;

        public void Add(float x, float y)
        {
            if (head == null)
            {
                head = new CollisionPoint(x, y, null, null);
                head.SetHead(head);
            }
            else
            {
                head.Add(x, y);
            }
        }

        /// <summary>
        /// deletes node overlapping with (x,y)
        /// </summary>
        /// <returns>True if node is deleted</returns>
        public bool Delete(int x, int y)
        {
            if (head != null && 
                head.Next == null)
            {
                RemoveHead();
                return true;
            }
            else if (head != null)
            {
                CollisionPoint c = head;

                while (c != null)
                {
                    if (c.CheckBounds(x, y))
                    {
                        if (!c.Delete())
                        {
                            head = null;
                            return true;
                        }
                        return true;
                    }
                    c = c.Next;
                }
            }

            return false;
        }

        public void RemoveHead()
        {
            head = null;
        }

        /// <summary>
        /// Rotate around an arbirary point
        /// </summary>
        /// <param name="a">angle</param>
        /// <param name="x">x point</param>
        /// <param name="y">y point</param>
        public void Rotate(float a, int x, int y)
        {
            if (head != null)
            {
                head.Rotate(a, x, y);
            }
        }

        public void Translate(int x, int y)
        {
            if (head != null)
            {
                head.Translate(x, y);
            }
        }

        /// <summary>
        /// Create new nodes to match another collision list
        /// </summary>
        /// <param name="c">The list to clone</param>
        public void Clone(CollisionList list, Entity e)
        {
            if (list.head != null)
            {
                CollisionPoint c = list.head;

                while (c != null)
                {
                    Add(c.X, c.Y);
                    c = c.Next;
                }
            }

            Rotate(e.Rotation, e.Rect.Center.X, e.Rect.Center.Y);
        }

        public void Draw(SpriteBatch sb)
        {
            if (head != null)
            {
                head.Draw(sb);
            }
        }
    }
}