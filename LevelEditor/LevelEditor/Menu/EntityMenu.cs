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
        
        public EntityMenu(Entity e)
            : base(0, 0, 200, Config.screenH)
        {
            this.e = e;
            AddItem("Change Name", null, new Vector2(0, GetMenuItemYPos(1)), Color.White, ChangeName);
            AddItem("", e.Texture, new Vector2(0, GetMenuItemYPos(2)), Color.White, ChangeTexture, true);
            AddItem("Up a Layer", null, new Vector2(0, GetMenuItemYPos(3)), Color.White, MoveUpLayer);
            AddItem("Down a Layer", null, new Vector2(0, GetMenuItemYPos(4)), Color.White, MoveDownLayer);

            AddItem("New Object", null, new Vector2(0, GetMenuItemYPos(5)), Color.White, CreateNewObject);
            AddItem("Scripting", null, new Vector2(0, GetMenuItemYPos(6)), Color.White, OpenScripting);
            AddItem("Animation", null, new Vector2(0, GetMenuItemYPos(7)), Color.White, OpenAnim);
            AddItem("Save Blueprint", null, new Vector2(0, GetMenuItemYPos(8)), Color.White, SaveBlueprint);

            AddItem("Add Volume", null, new Vector2(0, GetMenuItemYPos(10)), Color.White, AddCollision);
            AddItem("Volume Phys=on", null, new Vector2(0, GetMenuItemYPos(11)), Color.White, SetCollisionPhys);
            AddItem("Volume layer=0", null, new Vector2(0, GetMenuItemYPos(12)), Color.White, ChangeVolumeLayer);

            AddItem("add tag", null, new Vector2(0, GetMenuItemYPos(14)), Color.White, AddTag);
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

        public void OpenAnim()
        {
            if (!inUse)
            {
                inUse = true;
                AnimationForm anim = new AnimationForm();
                anim.Init(MenuSystem.graphics, e, delegate() { inUse = false; });
                anim.Show();
            }
        }

        public void AddTag()
        {
            if (!inUse)
            {
                inUse = true;
                TagForm tagForm = new TagForm();
                tagForm.Init(SetTags, e.Tags);
                tagForm.Show();
            }
        }

        public void AddCollision()
        {
            e.AddCollisionVolume();
            RefreshText();
        }

        public void SetCollisionPhys()
        {
            e.SwapCollisionPhys();
            RefreshText();
        }

        private void RefreshText()
        {
            items[9].Text = "Volume Phys=";
            if (e.GetCollisionPhys())
            {
                items[9].Text += "on";
            }
            else
            {
                items[9].Text += "off";
            }
            items[10].Text = "Volume layer=" + e.CurrentVolume;
        }

        public void ChangeVolumeLayer()
        {
            e.ChangeVolumeLayer();
            RefreshText();
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

        public void SetTags(string n)
        {
            string[] tags = n.Split(';');
            List<string> tagList = tags.ToList<string>();
            e.Tags = tagList;
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
