using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    public delegate void MenuActionS(string s); 

    class MenuItemBlueprint : MenuItem
    {
        MenuActionS action;
        string bluePrintName;

        public MenuItemBlueprint(string text, Texture2D texture, Vector2 pos, int w, int h, bool selectable, Color c, Menu m, MenuActionS a, string bluePrintName)
            : base(text, texture, pos, w, h, selectable, c, m, delegate() { })
        {
            this.bluePrintName =  bluePrintName;
            action = a;
        }

        public override void Evoke()
        {
            action(bluePrintName);
        }
    }
}
