using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{

    class LayerMenu : Menu
    {
        bool reInit = false;
        public LayerMenu()
            : base(0, 0, 200, Config.screenH)
        {
            Initlayers();
        }

        public void Initlayers()
        {
            items.Clear();

            AddItem("Add Layer", null, new Vector2(0, GetMenuItemYPos(0)), Color.Black, AddLayer);
            AddItem("Delete Layer", null, new Vector2(0, GetMenuItemYPos(1)), Color.Black, DeleteLayer);

            int count = 0;
            foreach (Layer layer in Editor.Layers)
            {
                AddItem((count + 1).ToString(), null, new Vector2(0, GetMenuItemYPos(count + 2)), Color.White, ChangeLayer, count);
                count++;
            }
        }

        public void ChangeLayer(int i)
        {
            Editor.CurrentLayer = i;
        }

        public void AddLayer()
        {
            Editor.Layers.Add(new Layer());
            reInit = true;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if (reInit)
            {
                reInit = false;
                Initlayers();
            }
        }

        public void DeleteLayer()
        {
            if (Editor.Layers.Count > 1)
            {
                Editor.Layers.Remove(Editor.Layers[Editor.CurrentLayer]);
                if (Editor.CurrentLayer != 0)
                {
                    Editor.CurrentLayer--;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            int count = 0;
            foreach (MenuItem i in items)
            {
                if (count > 1)
                {
                    if (count - 2 == Editor.CurrentLayer)
                    {
                        i.Color = Color.HotPink;
                    }
                    else
                    {
                        i.Color = Color.White;
                    }
                }
                count++;
            }

            base.Draw(sb);
        }
    }
}
