using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    class Layer
    {
        static List<Entity> entities = new List<Entity>();
        static List<Entity> toRemove = new List<Entity>();
        static List<Entity> toAdd = new List<Entity>();
        bool draw = true;

        public bool Visibility
        {
            get { return draw; }
            set { draw = value; }
        }

        public List<Entity> Entities
        {
            get { return entities; }
        }

        public void Add(Entity e)
        {
            toAdd.Add(e);
        }

        public void Remove(Entity e)
        {
            toRemove.Remove(e);
        }

        public void Update(float dt)
        {
            foreach (Entity e in toRemove)
            {
                entities.Remove(e);
            }
            foreach (Entity e in toAdd)
            {
                entities.Add(e);
            }

            if (draw)
            {
                entities.Reverse();
                foreach (Entity e in entities)
                {
                    e.Update();
                    if (Input.Overlapping(e.Rect) &&
                        Input.RightClick() &&
                        e.Mode == Entity.EditMode.none)
                    { toRemove.Add(e); MenuSystem.Close(); break; }
                }
                entities.Reverse();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (draw)
            {
                foreach (Entity e in entities)
                {
                    e.Draw(sb);
                }
            }
        }
    }
}
