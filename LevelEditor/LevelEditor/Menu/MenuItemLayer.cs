using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    delegate void LayerAction(int i);

    class MenuItemLayer : MenuItem
    {
        LayerAction action;
        int index;

        public MenuItemLayer(string text, Texture2D texture, Vector2 pos, int w, int h, bool selectable, Color c, Menu m, LayerAction a, int i)
            : base(text, texture, pos, w, h, selectable, c, m, delegate() { })
        {
            this.index = i;
            action = a;
        }

        public override void Evoke()
        {
            action(index);
        }
    }
}
