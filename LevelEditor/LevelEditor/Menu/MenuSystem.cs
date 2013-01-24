using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LevelEditor
{
    static class MenuSystem
    {
        static Menu current;
        public static TextureBank textureBank;
        static bool doonce = true;
        static BlueprintMenu blueprints;
        public static GraphicsDevice graphics;

        public static Menu Current
        {
            get { return current; }
        }

        public static void Init(GraphicsDevice g)
        {
            graphics = g;
            textureBank = new TextureBank();
            textureBank.Init(g);
            blueprints = new BlueprintMenu();
            blueprints.Init(g);
        }

        public static void OpenEntityMenu(Entity e)
        {
            current = new EntityMenu(e);
        }

        public static void OpenBlueprintMenu()
        {
            current = blueprints;
            blueprints.UpdateMenuItems();
        }

        public static void OpenLayerMenu()
        {
            current = new LayerMenu();
        }

        public static bool IsEntityMenu(Entity e)
        {
            if (current is EntityMenu)
            {
                if ((current as EntityMenu).Entity == e)
                {
                    return true;
                }
            }
            return false;
        }

        public static void Close()
        {
            current = null;
        }

        public static void Update(float dt)
        {
            if (current != null)
            {
                current.Update(dt);
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            if (current != null)
            {
                current.Draw(sb);
            }
        }
    }
}
