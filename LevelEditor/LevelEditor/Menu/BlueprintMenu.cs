using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    class BlueprintMenu : Menu
    {
        public BlueprintMenu()
            : base(0, 0, 200, Config.screenH)
        {
            UpdateMenuItems();
        }

        void UpdateMenuItems()
        {
            items.Clear();
            int count = 0;
            foreach (KeyValuePair<string, string> entity in Editor.Blueprints)
            {
                AddItem(entity.Key, null, new Vector2(0, GetMenuItemYPos(count)), Color.White, AddObject, entity.Key);
                count++;
            }
        }

        void AddObject(string name)
        {
            if (Editor.Blueprints.ContainsKey(name))
            {
                Entity e = new Entity(Editor.Blueprints[name]);
                e.Rotation = 0;
                e.MoveTo((int)Editor.Cam.Pos.X, (int)Editor.Cam.Pos.Y);
                Editor.AddEntity(e);
            }
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }
    }
}
