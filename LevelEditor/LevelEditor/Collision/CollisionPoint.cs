using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    class CollisionPoint
    {
        float x;
        float y;
        static int s = 7;
        CollisionPoint n;
        CollisionPoint p;
        CollisionPoint head;
        Color color = new Color(150, 0, 0, 150);

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public CollisionPoint Next
        {
            get { return n; }
        }

        public CollisionPoint Prev
        {
            get { return p; }
        }

        public CollisionPoint(float x, float y, CollisionPoint h, CollisionPoint p)
        {
            head = h;
            this.p = p;
            this.x = x;
            this.y = y;
        }

        public void Add(float x, float y)
        {
            if (n == null)
            {
                n = new CollisionPoint(x, y, head, this);
            }
            else
            {
                n.Add(x, y);
            }
        }

        /// <summary>
        /// Checks if the specified point is inside the bounds of the node
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if the point is inside the node</returns>
        public bool CheckBounds(int x, int y)
        {
            float dist = Vector2.Distance(new Vector2(x, y), 
                new Vector2(this.x, this.y));
            if (dist < s)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets head node, for this node and ever node after
        /// </summary>
        /// <param name="c">head node</param>
        public void SetHead(CollisionPoint c)
        {
            head = c;
            if (n != null)
            {
                n.SetHead(c);
            }
        }

        /// <summary>
        /// Rotate a point
        /// </summary>
        /// <param name="a">Angle to rotate</param>
        /// <param name="x">x center of rotation</param>
        /// <param name="y">y center of rotation</param>
        public void Rotate(float a, int x, int y)
        {
            float rx = (float)(Math.Cos(a) * (this.x - x) - Math.Sin(a) * (this.y - y) + x);
            float ry = (float)(Math.Sin(a) * (this.x - x) + Math.Cos(a) * (this.y - y) + y);
            this.x = rx;
            this.y = ry;

            if(n != null)
            {
                n.Rotate(a,x,y);
            }
        }

        public void Translate(int x, int y)
        {
            this.x += x;
            this.y += y;

            if (n != null)
            {
                n.Translate(x, y);
            }
        }

        /// <summary>
        /// Deletes current node.
        /// </summary>
        /// <returns>True if deleted, False if current node is head</returns>
        public bool Delete()
        {
            if (p != null && n != null)
            {
                p.n = n;
                n.p = p;
            }
            else if( p == null)
            {
                n.p = null;
                n.SetHead(n);
            }
            else if (n == null)
            {
                p.n = null;
            }
            else
            {
                return false;
            }
            return true;
        }

        public void Draw(SpriteBatch sb, Color c)
        {
            LineBatch.DrawCircle(sb, new Vector2(x, y), s, c);
            if (n != null)
            {
                n.Draw(sb, c);

                LineBatch.DrawLine(sb, c,
                    new Vector2(x, y), new Vector2(n.x, n.y));
            }
            else
            {
                LineBatch.DrawLine(sb, c,
                    new Vector2(x, y), new Vector2(head.x, head.y));
            }
        }
    }
}
