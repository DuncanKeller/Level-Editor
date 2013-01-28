#region Using Statements
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
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
        BasicEffect effect;
        Stopwatch timer;
        Texture2D texture;
        ContentManager content;
        SpriteBatch sb;

        Texture2D test;

        /// <summary>
        /// Initializes the control.
        /// </summary>
        protected override void Initialize()
        {
            // Start the animation timer.
            content = new ContentManager(Services, "Content");

            sb = new SpriteBatch(GraphicsDevice);

            test = content.Load<Texture2D>("blank");

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        public void SetImage(Texture2D t)
        {
            texture = t;
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

            // Set renderstates.
            //GraphicsDevice.RasterizerState = RasterizerState.CullNone;
           
            sb.Begin();

            if (texture != null)
            {
                //GraphicsDevice.Clear(Color.Green);
                Rectangle r = new Rectangle(0, 0, 50, 50);
                sb.Draw(texture, r, Color.White);
            }
            else
            {
                //GraphicsDevice.Clear(Color.Red);
            }

          

            sb.End();
        }
    }
}