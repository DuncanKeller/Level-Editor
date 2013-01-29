#region Using Statements
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
#endregion

namespace LevelEditor
{
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, which allows it to
    /// render using a GraphicsDevice. This control shows how to draw animating
    /// 3D graphics inside a WinForms application. It hooks the Application.Idle
    /// event, using this to invalidate the control, which will cause the animation
    /// to constantly redraw.
    /// </summary>
    partial class FrameDisplayComponent : GraphicsDeviceControl
    {
        Texture2D texture;
        ContentManager content;
        SpriteBatch sb;
        
        List<CollisionList> list;
        bool trans = false;
        int index = 0;

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            // Start the animation timer.
            content = new ContentManager(Services, "Content");

            sb = new SpriteBatch(GraphicsDevice);

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        public void SetImage(Texture2D t)
        {
            if (t != null)
            {
                texture = new Texture2D(GraphicsDevice, t.Width, t.Height);
                int[] data = new int[t.Width * t.Height];
                t.GetData<int>(data);
                texture.SetData<int>(data);
            }
            else
            {
                texture = null;
            }
        }

        public void SetList(List<CollisionList> l)
        {
            list = l;
            index = 0;
        }

        public void SetIndex(int i)
        {
            index = i;
        }

        public void ToggelVis()
        {
            trans = !trans;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                content.Unload();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            sb.Begin();

            if (texture != null)
            {
                Rectangle r = new Rectangle(0, 0, texture.Width, texture.Height);
                if (trans)
                {
                    sb.Draw(texture, r, new Color(100, 100, 100, 100));
                }
                else
                {
                    sb.Draw(texture, r, Color.White);
                }
            }

            if (list != null)
            {
                int count = 0;
                foreach (CollisionList l in list)
                {
                    if (count == index)
                    {
                        l.Draw(sb, Color.Red);
                    }
                    else
                    {
                        l.Draw(sb, new Color(50,50,50,50));
                    }
                    count++;
                }
            }

            sb.End();
        }
    }
}