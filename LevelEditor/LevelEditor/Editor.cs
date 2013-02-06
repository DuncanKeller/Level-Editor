using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.IO;

namespace LevelEditor
{
    static class Editor
    {
        static Camera cam;
        static int currentLayer;
        static Dictionary<string, string> blueprints = new Dictionary<string, string>();
        static List<Layer> layers = new List<Layer>();
        
        public static Dictionary<string, string> Blueprints
        {
            get { return blueprints; }
        }

        public static List<Layer> Layers
        {
            get { return layers; }
            set { layers = value; }
        }

        public static int CurrentLayer
        {
            get { return currentLayer; }
            set { currentLayer = value; }
        }

        public static Camera Cam
        {
            get { return cam; }
        }

        public static void Init()
        {
            cam = new Camera();
            cam._pos.X += Config.screenW / 2;
            cam._pos.Y += Config.screenH / 2;
            layers.Add(new Layer());
            currentLayer = 0;

            layers[0].Add(new Entity("finger", 100, 100));
        }

        public static void Update()
        {
            foreach (Layer layer in layers)
            {
                layer.Update(1);
            }

            if (Input.KeyPressed(Keys.Tab))
            {
                if (MenuSystem.Current != null)
                {
                    MenuSystem.Close();
                }
                else
                {
                    MenuSystem.OpenBlueprintMenu();
                }
            }
            else if (Input.KeyPressed(Keys.L))
            {
                if (MenuSystem.Current is LayerMenu)
                {
                    MenuSystem.Close();
                }
                else
                {
                    MenuSystem.OpenLayerMenu();
                }
            }
            else if (Input.KeyPressed(Keys.S))
            {
                Save();
            }

            MoveCam();
        }

        private static void MoveCam()
        {
            int speed = 10;
            if (Input.KeyHeld(Keys.LeftShift))
            { speed = 100; }
            else if (Input.KeyHeld(Keys.LeftControl))
            { speed = 1; }
            if (Input.KeyHeld(Keys.A))
            {
                cam._pos.X -= speed;
            }
            if (Input.KeyHeld(Keys.D))
            {
                cam._pos.X += speed;
            }
            if (Input.KeyHeld(Keys.W))
            {
                cam._pos.Y -= speed;
            }
            if (Input.KeyHeld(Keys.S))
            {
                cam._pos.Y += speed;
            }
        }

        public static void ChangeLayer(Entity e, int index)
        {
            int i = 0;
            foreach (Layer layer in layers)
            {
                if (layer.Entities.Contains(e))
                {
                    if (i + index >= 0 &&
                        i + index < layers.Count)
                    {
                        layers[i + index].Add(e);
                        layer.Remove(e);
                        break;
                    }
                }
                i++;
            }
        }

        public static void AddEntity(Entity e)
        {
            layers[currentLayer].Add(e);
        }

        public static void AddBlueprint(string name, string json)
        {
            if (!blueprints.ContainsKey(name))
            {
                blueprints.Add(name, json);
            }
            else
            {
                blueprints[name] = json;
            }
        }

        public static void Save()
        {
            string levelName = "testlevel";
            FileStream fs = File.Open(levelName + ".json", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            JsonTextWriter jw = new JsonTextWriter(sw);
            jw.Formatting = Formatting.Indented;

            jw.WriteStartObject();
            jw.WritePropertyName("entities");
            jw.WriteStartArray();
            foreach (Layer layer in layers)
            {
                foreach (Entity e in layer.Entities)
                {
                    e.SaveEntity(ref jw);
                }
            }
            jw.WriteEnd();
            jw.WriteEnd();
            jw.Close();
        }

        public static void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp,
                DepthStencilState.Default, RasterizerState.CullNone, null, cam.get_transformation(g));
            foreach (Layer layer in layers)
            {
                layer.Draw(sb);
            }
            sb.End();
        }
    }
}
