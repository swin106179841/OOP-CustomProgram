using ShapeDrawer;
using SplashKitSDK;
using System.IO;
namespace CustomProgram
{
    class Level
    {
        private int _levelWidth;
        private int _levelHeight;
        private List<int> _scene = [];
        private List<Object> _objects = [];
        private TextureManager _texMan = new TextureManager();
        public Level(String levelPath)
        {
            int errorCount = 0;
            StreamReader sr = File.OpenText(levelPath);
            // magic number
            if (sr.ReadLine() != "LVLV1")
            {
                Console.WriteLine("Error. Invalid File Type");
                return;
            }
            // load sprite sheet
            try
            {
                Bitmap spriteSheet = SplashKit.LoadBitmap("spritesheet", sr.ReadLine()!);
                int tileWidth = sr.ReadInteger();
                int tileHeight = sr.ReadInteger();
                int tilesX = sr.ReadInteger();
                int tilesY = sr.ReadInteger();
                int tilesCount = sr.ReadInteger();
                SplashKit.BitmapSetCellDetails(spriteSheet, tileWidth, tileHeight, tilesX, tilesY, tilesCount);
                _texMan.AddTexture(spriteSheet);
            } catch (Exception e)
            {
                Console.WriteLine("Error loading spritesheet from level: " + levelPath);
                Console.WriteLine("Error: " + e.Message);
            }
            // load assets
            try
            {
                int texCount = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < texCount; i++)
                {
                    String name = String.Format("texture-{0}", i);
                    Bitmap tex = SplashKit.LoadBitmap(name, sr.ReadLine()!);
                    _texMan.AddTexture(tex);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading assets from level: " + levelPath);
                Console.WriteLine("Error: " + e.Message);
                errorCount++;
            }
            // load objects
            try
            {
                int objectCount = sr.ReadInteger();
                for (int i = 0; i < objectCount; i++)
                {
                    String objType = sr.ReadLine()!;
                    GameObject obj;
                    switch (objType)
                    {
                        case "ObjectStatic":
                            obj = new ObjectStatic();
                            break;
                        case "ObjectInteractable":
                            obj = new ObjectInteractable();
                            break;
                        default:
                            throw new InvalidDataException();
                    }
                    obj.Load(sr);
                    _objects.Add(obj);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading objects from level: " + levelPath);
                Console.WriteLine("Error: " + e.Message);
                errorCount++;
            }
            // load map
            try
            {
                _levelWidth = sr.ReadInteger();
                _levelHeight = sr.ReadInteger();
                string mapData = sr.ReadLine()!;
                string[] values = mapData.Split(' ');
                foreach (string tile in values)
                {
                    _scene.Add(Convert.ToInt32(tile));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading assets from level: " + levelPath);
                Console.WriteLine("Error: " + e.Message);
                errorCount++;
            }
            if (errorCount > 0)
            {
                _levelWidth = 1;
                _levelHeight = 1;
                _scene.Clear();
                _scene.Add(0);
                _objects.Clear();
                _texMan.RemoveAllTextures();
            }
        }
        public void Draw()
        {
            // draw scene
            for (int y = 0; y < _levelHeight; y++)
            {
                for (int x = 0; x < _levelWidth; x++)
                {
                    DrawingOptions cellOpts = SplashKit.OptionWithBitmapCell(_scene[y * _levelWidth + x]);
                    cellOpts.ScaleX = Settings.RenderScale;
                    cellOpts.ScaleY = Settings.RenderScale;
                    SplashKit.DrawBitmap(_texMan.RequestTexture(0), x * (32 * Settings.RenderScale), y * (32 * Settings.RenderScale), cellOpts);
                }
            }
            // draw objects
            foreach (GameObject obj in _objects)
            {
                obj.Draw(_texMan.RequestTexture(obj.TextureIndex));
            }
        }
    }
}