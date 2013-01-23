using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace LevelEditor
{
    class BlueprintMenu : Menu
    {
        static GraphicsDevice graphics;
        public BlueprintMenu()
            : base(0, 0, 200, Config.screenH)
        {
            UpdateMenuItems();
        }

        public void Init(GraphicsDevice g)
        {
            graphics = g;
            LoadBlueprints();
        }

        public void LoadBlueprints()
        {
            foreach (string f in Directory.EnumerateFiles("blueprints"))
            {
                string n = Path.GetFileName(f);
                string e = Path.GetExtension(n);
                n = n.Substring(0, n.Length - e.Length);
                StreamReader sr = new StreamReader(f);
                string json = sr.ReadToEnd();
                Editor.AddBlueprint(n, json);
                sr.Close();
            }
            foreach (string f in Directory.EnumerateFiles("images"))
            {
                string n = Path.GetFileName(f);
                string e = Path.GetExtension(n);
                n = n.Substring(0, n.Length - e.Length);
                Stream tr = File.OpenRead("images\\" + n + ".png");
                Texture2D texture = Texture2D.FromStream(graphics, tr);
                MenuSystem.textureBank.AddTexture(texture, n + ".png");
                tr.Close();
            }
        }

        public void UpdateMenuItems()
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