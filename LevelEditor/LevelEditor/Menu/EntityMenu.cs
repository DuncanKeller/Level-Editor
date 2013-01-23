using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    public delegate void NameAction(string s);
    public delegate void TextureAction(Texture2D t, string n);

    class EntityMenu : Menu
    {
        Entity e;
        bool inUse = false;

        public EntityMenu(Entity e)
            : base(0, 0, 200, Config.screenH)
        {
            this.e = e;
            AddItem("Change Name", null, new Vector2(0, GetMenuItemYPos(1)), Color.White, ChangeName);
            AddItem("", e.Texture, new Vector2(0, GetMenuItemYPos(2)), Color.White, ChangeTexture, true);
            AddItem("Up a Layer", null, new Vector2(0, GetMenuItemYPos(3)), Color.White, MoveUpLayer);
            AddItem("Down a Layer", null, new Vector2(0, GetMenuItemYPos(4)), Color.White, MoveDownLayer);

            AddItem("New Object", null, new Vector2(0, GetMenuItemYPos(5)), Color.White, CreateNewObject);
            AddItem("Script", null, new Vector2(0, GetMenuItemYPos(6)), Color.White, OpenScripting);
            AddItem("Save Blueprint", null, new Vector2(0, GetMenuItemYPos(7)), Color.White, SaveBlueprint);
        }

        public Entity Entity
        {
            get { return e; }
        }

        public void ChangeName()
        {
            if (!inUse)
            {
                inUse = true;
                NameForm nameForm = new NameForm();
                nameForm.Init(SetName, e.Name);
                nameForm.Show();
            }
        }

        public void ChangeTexture()
        {
            if (!inUse)
            {
                inUse = true;
                MenuSystem.textureBank.SetCallback(SetTexture);
                MenuSystem.textureBank.Show();
            }
        }

        public void OpenScripting()
        {
            Scripting scripting = new Scripting();
            scripting.Init(e.Name);
            scripting.Show();
        }

        public void MoveUpLayer()
        {
            Editor.ChangeLayer(e, -1);
        }

        public void MoveDownLayer()
        {
            Editor.ChangeLayer(e, 1);
        }

        public void CreateNewObject()
        {
            Entity newEnt = new Entity(e);
            Editor.AddEntity(newEnt);
        }

        public void SaveBlueprint()
        {
            e.SaveBlueprint();
        }

        public void SetTexture(Texture2D t, string n)
        {
            e.SetTexture(t, n);
            inUse = false;
        }

        public void SetName(string n)
        {
            e.Name = n;
            inUse = false;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            string name = e.Name;
            if (TextureManager.FontMap["menuFont"].MeasureString(name).X > w)
            {
                int loc = (int)(w / (TextureManager.FontMap["menuFont"].MeasureString(name).X / name.Length));
                name = name.Substring(0, loc - 3) + "...";
            }

            sb.DrawString(TextureManager.FontMap["menuFont"], name,
                new Vector2(Position.X + 2, Position.Y + 2), Color.White);
        }

    }
}
