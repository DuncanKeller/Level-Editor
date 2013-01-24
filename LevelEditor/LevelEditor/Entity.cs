using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;

namespace LevelEditor
{
    class Entity
    {
        public enum EditMode
        {
            none,
            drag,
            rotate,
            collision,
            animation
        }

        string name = "unnamed";
        Texture2D texture;
        string textureName;
        List<CollisionList> cVolumes = new List<CollisionList>();
        int currVolume = 0;
        Rectangle rect;
        EditMode mode = EditMode.none;
        List<string> tags = new List<string>();

        float rotationOffset;
        float rotation;
        Point translationOffset;
        bool dynamic = false;

        #region Properties

        public List<string> Tags
        {
            get { return tags; }
            set { tags = value; }
        }

        public int CurrentVolume
        {
            get { return currVolume; }
        }

        public EditMode Mode
        {
            get { return mode; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Rectangle Rect
        {
            get { return rect; }
            //set { rect = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        #endregion

        public Entity(string textureName, int x, int y)
        {
            this.textureName = textureName;
            texture = TextureManager.TexMap[textureName];
            rect = new Rectangle(x, y, texture.Width, texture.Height);
            cVolumes.Add(new CollisionList());
        }

        public Entity(Entity e)
        {
            texture = e.texture;
            textureName = e.textureName;
            rect = e.rect;
            rotation = e.rotation;
            name = "new " + e.Name;
            foreach (CollisionList c in e.cVolumes)
            {
                CollisionList col = new CollisionList();
                col.Clone(c, e);
                cVolumes.Add(col);
            }
            Translate(rect.Width, 0);
        }

        public Entity(string unparsedJson)
        {
            JObject json = JObject.Parse(unparsedJson);
            name = (string)json["name"];
            string textureName = (string)json["texture"];
            this.textureName = textureName;
             texture = MenuSystem.textureBank.Textures[textureName];
            int x, y, w, h;
            x = (int)((float)json["x"]);
            y = (int)((float)json["y"]);
            w = (int)json["width"];
            h = (int)json["height"];
            rect = new Rectangle(x - (w / 2), y - (h / 2), w, h);
            rotation = (float)json["rotation"];
            JArray volumes = (JArray)json["collisionVolumes"];
            for (int index = 0; index < volumes.Count; index++)
            {
                CollisionList cl = new CollisionList();
                JObject collisionJson = (JObject)(volumes[index]);
                JArray points = (JArray)collisionJson["xpoints"];
                List<float> xpoints = new List<float>();
                for (int i = 0; i < points.Count; i++)
                {
                    xpoints.Add((float)points[i] + x);
                }
                points = (JArray)collisionJson["ypoints"];
                List<float> ypoints = new List<float>();
                for (int i = 0; i < points.Count; i++)
                {
                    ypoints.Add((float)points[i] + y);
                }
                for (int i = 0; i < xpoints.Count; i++)
                {
                    cl.Add(xpoints[i], ypoints[i]);
                }
                cl.Physical = (bool)collisionJson["physical"];
                cVolumes.Add(cl);
            }
            dynamic = (bool)json["dynamic"];
            JArray tags = (JArray)json["tags"];
            for (int index = 0; index < tags.Count; index++)
            {
                this.tags.Add((string)tags[index]);
            }

        }

        public void SaveBlueprint()
        {
            //FileStream fs = File.Open(Environment.GetFolderPath(
            //    Environment.SpecialFolder.LocalApplicationData) + name + ".json", FileMode.Create);
            FileStream fs = File.Open("blueprints\\" + name + ".json", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            JsonTextWriter jw = new JsonTextWriter(sw);
            jw.Formatting = Formatting.Indented;

            jw.WriteStartObject();
            jw.WritePropertyName("name");
            jw.WriteValue(name);
            jw.WritePropertyName("rotation");
            jw.WriteValue(rotation);
            jw.WritePropertyName("x");
            Vector2 center;
            if (cVolumes[0] != null)
            {
                center = cVolumes[0].GetCenter();
            }
            else
            {
                center = new Vector2(rect.Center.X, rect.Center.Y);
            }
            jw.WriteValue(center.X);
            jw.WritePropertyName("y");
            jw.WriteValue(center.Y);
            jw.WritePropertyName("width");
            jw.WriteValue(rect.Width);
            jw.WritePropertyName("height");
            jw.WriteValue(rect.Height);
            jw.WritePropertyName("collisionVolumes");
            jw.WriteStartArray();
            foreach (CollisionList cl in cVolumes)
            {
                jw.WriteStartObject();
                jw.WritePropertyName("physical");
                jw.WriteValue(cl.Physical);
                jw.WritePropertyName("xpoints");
                jw.WriteStartArray();
                CollisionList copyList = new CollisionList();
                copyList.Clone(cl, this);
                copyList.Rotate(-rotation * 2, rect.Center.X, rect.Center.Y);
                foreach (CollisionPoint p in copyList.Nodes)
                {
                    jw.WriteValue(p.X - center.X);
                }
                jw.WriteEnd();
                jw.WritePropertyName("ypoints");
                jw.WriteStartArray();
                foreach (CollisionPoint p in copyList.Nodes)
                {
                    jw.WriteValue(p.Y - center.Y);
                }
            }
            jw.WriteEnd();
            jw.WriteEnd();
            jw.WriteEnd();
            jw.WritePropertyName("texture");
            jw.WriteValue(textureName);
            jw.WritePropertyName("dynamic");
            jw.WriteValue(dynamic);
            jw.WritePropertyName("tags");
            jw.WriteStartArray();
            foreach (string s in tags)
            {
                jw.WriteValue(s);
            }
            jw.WriteEnd();

            jw.WriteEnd();
            jw.Close();

            fs = File.Open("blueprints\\" + name + ".json", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string json = sr.ReadToEnd();

            Editor.AddBlueprint(name, json);

            fs.Close();
        }

        public void SaveEntity(ref JsonTextWriter jw)
        {
            jw.WriteStartObject();

            jw.WritePropertyName("name");
            jw.WriteValue(name);
            jw.WritePropertyName("rotation");
            jw.WriteValue(rotation);
            jw.WritePropertyName("x");
            Vector2 center;
            if (cVolumes[0] != null)
            {
                center = cVolumes[0].GetCenter();
            }
            else
            {
                center = new Vector2(rect.Center.X, rect.Center.Y);
            }
            jw.WriteValue(center.X);
            jw.WritePropertyName("y");
            jw.WriteValue(center.Y);
            jw.WritePropertyName("tags");
            jw.WriteStartArray();
            foreach (string s in tags)
            {
                jw.WriteValue(s);
            }
            jw.WriteEnd();

            jw.WriteEnd();
        }

        public void Update()
        {
            if (!Input.LeftHeld() &&
                mode != EditMode.collision)
            {
                if (mode == EditMode.drag ||
                    mode == EditMode.rotate)
                {
                    Input.locked = false;
                }
                mode = EditMode.none;
            }

            if (MenuSystem.Current == null ||
                !Input.Overlapping(MenuSystem.Current.Rect))
            {
                OpenMenu();
                Drag();
                Rotate();

                Collisions();
            }
        }

        public void AddTag(string t)
        {
            tags.Add(t);
        }

        public void SwapCollisionPhys()
        {
            cVolumes[currVolume].Physical = !cVolumes[currVolume].Physical;
        }

        public bool GetCollisionPhys()
        {
            return cVolumes[currVolume].Physical;
        }

        public void AddCollisionVolume()
        {
            if (cVolumes.Count < 5)
            {
                cVolumes.Add(new CollisionList());
            }

            currVolume = cVolumes.Count - 1;
        }

        public void ChangeVolumeLayer()
        {
            currVolume++;

            if (currVolume > cVolumes.Count - 1)
            {
                currVolume = 0;
            }
        }

        public void SetTexture(Texture2D t, string n)
        {
            textureName = n;
            texture = t;
            rect.Width = texture.Width;
            rect.Height = texture.Height;
        }

        public void OpenMenu()
        {
            if (mode == EditMode.none)
            {
                if (Input.LeftClick() &&
                    Input.Overlapping(rect))
                {
                    MenuSystem.OpenEntityMenu(this);
                }
            }
        }

        public void Translate(int x, int y)
        {
            rect.X += x;
            rect.Y += y;
            foreach (CollisionList cl in cVolumes)
            {
                cl.Translate(x, y);
            }
        }

        public void MoveTo(int x, int y)
        {
            int offsetx = x - rect.X;
            int offsety = y - rect.Y;
            rect.X = x;
            rect.Y = y;
            foreach (CollisionList cl in cVolumes)
            {
                cl.Translate(offsetx, offsety);
            }
        }

        public void Collisions()
        {
            if (mode == EditMode.none)
            {
                if (Input.KeyPressed(Keys.LeftControl) &&
                    MenuSystem.IsEntityMenu(this))
                { mode = EditMode.collision; }
            }
            else if (mode == EditMode.collision)
            {
                if (Input.KeyPressed(Keys.LeftControl))
                { mode = EditMode.none; }

                if (Input.LeftClick())
                {
                    cVolumes[currVolume].Add(Input.X, Input.Y);
                }
                else if (Input.RightClick())
                {
                    cVolumes[currVolume].Delete(Input.X, Input.Y);
                }
                else if (Input.KeyPressed(Keys.Back))
                {
                    cVolumes[currVolume].RemoveHead();
                }
            }
        }

        public void Rotate()
        {
            if (mode == EditMode.none)
            {
                if (Input.Overlapping(rect) &&
                    MenuSystem.IsEntityMenu(this))
                {
                    //startdrag
                    if (Input.LeftHeld() &&
                        Input.KeyHeld(Keys.LeftShift) &&
                        !Input.locked)
                    {
                        rotationOffset = -rotation - (float)Math.Atan2(Input.X - rect.Center.X, Input.Y - rect.Center.Y);
                        mode = EditMode.rotate;
                        Input.locked = true;
                    }
                }
            }
            else if (mode == EditMode.rotate)
            {
                float r = -(float)Math.Atan2(Input.X - rect.Center.X, Input.Y - rect.Center.Y) - rotationOffset;
                foreach (CollisionList cl in cVolumes)
                {
                    cl.Rotate(r - rotation, rect.Center.X, rect.Center.Y);
                }
                rotation = r;
            }
        }

        public void Drag()
        {
            if (mode == EditMode.none && !Input.locked)
            {
                if (Input.Overlapping(rect))
                {
                    // start drag
                    if (Input.LeftClick() &&
                        !Input.KeyHeld(Keys.LeftShift))
                    {
                        translationOffset = new Point(Input.X - rect.X, Input.Y - rect.Y);
                        mode = EditMode.drag;
                        Input.locked = true;
                    }
                }
            }
            else if (mode == EditMode.drag)
            {
                int x = Input.X - translationOffset.X;
                int y = Input.Y - translationOffset.Y;
                foreach (CollisionList cl in cVolumes)
                {
                    cl.Translate(x - rect.X, y - rect.Y);
                }
                rect.X = x;
                rect.Y = y;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Color c = Color.White;

            if (mode == EditMode.collision)
            {
                c = new Color(150, 150, 150, 50);
            }

            sb.Draw(texture, new Rectangle(rect.X + texture.Width / 2, rect.Y + texture.Height / 2, rect.Width, rect.Height),
                new Rectangle(0, 0, texture.Width, texture.Height),
                c, rotation, new Vector2(texture.Width / 2, texture.Height / 2),
                SpriteEffects.None, 0);
            if (mode == EditMode.collision)
            {
                for (int i = 0; i < cVolumes.Count; i++)
                {
                    if (i == currVolume)
                    {
                        Color color = Color.White;
                        switch (i)
                        {
                            case 0:
                                color = Color.Red;
                                break;
                            case 1:
                                color = Color.Yellow;
                                break;
                            case 2:
                                color = Color.Blue;
                                break;
                            case 3:
                                color = Color.Green;
                                break;
                            case 4:
                                color = Color.Purple;
                                break;
                        }
                        cVolumes[i].Draw(sb, color);
                    }
                    else
                    {
                        cVolumes[i].Draw(sb, Color.Gray);
                    }
                }
            }
        }
    }
}
