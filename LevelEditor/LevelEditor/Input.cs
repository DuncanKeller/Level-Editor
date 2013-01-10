using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LevelEditor
{
    static class Input
    {
        static MouseState m;
        static MouseState pm;

        static KeyboardState k;
        static KeyboardState pk;

        public static int X
        {
            get { return m.X; }
        }

        public static int Y
        {
            get { return m.Y; }
        }

        public static float Scroll
        {
            get
            {
                return m.ScrollWheelValue - pm.ScrollWheelValue;
            }
        }

        public static bool LeftClick()
        {
            return m.LeftButton == ButtonState.Pressed &&
                pm.LeftButton == ButtonState.Released;
        }

        public static bool RightClick()
        {
            return m.RightButton == ButtonState.Pressed &&
                pm.RightButton == ButtonState.Released;
        }

        public static bool LeftHeld()
        {
            return m.LeftButton == ButtonState.Pressed;
        }

        public static bool RightHeld()
        {
            return m.RightButton == ButtonState.Pressed;
        }

        public static bool KeyHeld(Keys key)
        {
            if (k.IsKeyDown(key))
            { return true; }
            return false;
        }

        public static bool KeyPressed(Keys key)
        {
            if (k.IsKeyDown(key) &&
                pk.IsKeyUp(key))
            { return true; }
            return false;
        }

        public static bool Overlapping(Rectangle r)
        {
            if (new Rectangle(X, Y, 1, 1).Intersects(r))
            { return true; }
            return false;
        }

        public static void Update()
        {
            m = Mouse.GetState();
            k = Keyboard.GetState();
        }

        public static void LateUpdate()
        {
            pm = m;
            pk = k;
        }
    }
}
