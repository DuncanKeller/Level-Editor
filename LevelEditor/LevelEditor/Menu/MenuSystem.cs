using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    static class MenuSystem
    {
        static Menu current;
        public static TextureBank textureBank;

        public static Menu Current
        {
            get { return current; }
        }

        public static void Init(GraphicsDevice g)
        {
            textureBank = new TextureBank();
            textureBank.Init(g);
        }

        public static void OpenEntityMenu(Entity e)
        {
            current = new EntityMenu(e);
        }

        public static void OpenBlueprintMenu()
        {
            current = new BlueprintMenu();
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
