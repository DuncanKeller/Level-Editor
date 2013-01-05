using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    static class Editor
    {
        static List<Entity> currentLayer;
        static List<List<Entity>> layers = new List<List<Entity>>();

        public static void Init()
        {
            layers.Add(new List<Entity>());
            layers[0].Add(new Entity(TextureManager.TexMap["finger"], 100, 100));
            currentLayer = layers[0];
        }

        public static void Update()
        {
            foreach (List<Entity> layer in layers)
            {
                if (currentLayer == layer)
                {
                    foreach (Entity e in layer)
                    {
                        e.Update();
                    }
                }
            }
        }

        public static void ChangeLayer(Entity e, int index)
        {
            int i = 0;
            foreach (List<Entity> layer in layers)
            {
                if (layer.Contains(e))
                {
                    if (i + index >= 0 &&
                        i + index < layers.Count)
                    {
                        layers[i + index].Add(e);
                        layer.Remove(e);
                        break;
                    }
                }
                i++;
            }
        }

        public static void AddEntity(Entity e)
        {
            currentLayer.Add(e);
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (List<Entity> layer in layers)
            {
                foreach (Entity e in layer)
                {
                    e.Draw(sb);
                }
            }
        }
    }
}
