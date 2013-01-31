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
                foreach (Entity e in entities)
                {
                    e.Update();
                }
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
