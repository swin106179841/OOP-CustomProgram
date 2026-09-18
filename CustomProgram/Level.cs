using ShapeDrawer;
using SplashKitSDK;
using System.Diagnostics.CodeAnalysis;
using System.IO;
namespace CustomProgram
{
    public class Level
    {
        private int _levelWidth;
        private int _levelHeight;
        private List<int> _scene = [];
        private List<Object> _objects = [];
        private List<int> _floorTiles = [13, 14, 22, 23];
        private TextureManager _texMan = new TextureManager();


        public Level(int w, int h, Bitmap spriteSheet)
        {
            _levelWidth = w;
            _levelHeight = h;
            _texMan.AddTexture(spriteSheet);
            for (int i = 0; i < w * h; i++)
            {
                _scene.Add(22);
            }
        }
        public Level(String levelPath)
        {
            Load(levelPath);
        }
        public void Load(String levelPath)
        {
            // clear level
            Clear();
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
            }
            catch (Exception e)
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
                        case "ObjectPushable":
                            obj = new ObjectPushable();
                            break;
                        default:
                            throw new InvalidDataException();
                    }
                    obj.Load(sr);
                    obj.Texture = _texMan.RequestTexture(obj.TextureIndex);
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
                Console.WriteLine("Error loading tile data from level: " + levelPath);
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
            sr.Close();
        }
        public void Clear()
        {
            _scene.Clear();
            _objects.Clear();
            _texMan.RemoveAllTextures();
            _levelWidth = 0;
            _levelHeight = 0;
        }
        public void Save()
        {
            int sum = (_scene[0] << 3) & _scene[_levelHeight * _levelWidth - 1] ^ _scene[(_levelHeight / 2) * (_levelWidth / 2)];
            String filename = String.Format("./levels/{0}x{1}-{2}.lvl", _levelWidth, _levelHeight, sum);
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("LVLV1"); // magic
            sw.WriteLine(_texMan.RequestTexture(0).Filename); // spritesheet
            sw.WriteLine("32\n32"); // tile w, tile h
            sw.WriteLine(6); // tiles accross
            sw.WriteLine(6); // tiles down
            sw.WriteLine(36); // tiles total
            sw.WriteLine(_texMan.Count - 1); // asset count, most likely 0
            sw.WriteLine(_objects.Count());
            foreach (GameObject obj in _objects)
            {
                sw.WriteLine(obj.GetType());
                obj.Save(sw);
            }
            sw.WriteLine(_levelWidth);
            sw.WriteLine(_levelHeight);
            foreach (int tile in _scene)
            {
                sw.Write(tile + " ");
            }
            sw.WriteLine();
            sw.Close();
            Console.WriteLine("Level {0} successfully saved", filename);

        }
        public void Draw()
        {
            Draw(0, 0);
        }
        public void Draw(int offsetX, int offsetY)
        {
            // draw scene
            for (int y = 0; y < _levelHeight; y++)
            {
                for (int x = 0; x < _levelWidth; x++)
                {
                    if (_scene[y * _levelWidth + x] != -1)
                    {
                        DrawingOptions cellOpts = SplashKit.OptionWithBitmapCell(_scene[y * _levelWidth + x]);
                        cellOpts.ScaleX = Settings.RenderScale;
                        cellOpts.ScaleY = Settings.RenderScale;
                        cellOpts.AnchorOffsetX = 0;
                        cellOpts.AnchorOffsetY = 0;
                        SplashKit.DrawBitmap(_texMan.RequestTexture(0), offsetX + x * 32 * Settings.RenderScale, offsetY + y * 32 * Settings.RenderScale, cellOpts);
                    }
                }
            }
        }
        public void DrawObjects()
        {
            DrawObjects(0, 0);
        }
        public void DrawObjects(int offsetX, int offsetY)
        {
            foreach (GameObject obj in _objects)
            {
                obj.Draw(offsetX, offsetY);
            }
        }
        public void Update()
        {
            foreach (GameObject obj in _objects)
            {
                obj.Update();
            }
        }
        // returns -1 on out of bounds
        public int TileAt(int index)
        {
            if (index < _scene.Count() && index > 0)
            {
                return _scene[index];
            }
            else
            {
                return -1;
            }
        }
        // returns -1 on out of bounds
        public int TileAt(int x, int y)
        {
            int index = y * _levelWidth + x;

            return TileAt(index);
        }
        public int TileAt(float x, float y)
        {
            int index = (int)y * _levelWidth + (int)x;
            return TileAt(index);
        }
        public bool IsFloor(int tile)
        {
            foreach (int t in _floorTiles)
            {
                if (tile == t)
                    return true;
            }
            return false;
        }
        public GameObject? ObjectAt(int x, int y)
        {
            foreach (GameObject obj in _objects)
            {
                if (obj.X == x && obj.Y == y)
                {
                    return obj;
                }
            }
            return null;
        }
        public void RemoveObject(GameObject obj)
        {
            _objects.Remove(obj);
        }
        public void SetTile(int x, int y, int value)
        {
            if (x < 0 || x > _levelWidth || y < 0 || y > _levelHeight)
                return;
            _scene[y * _levelWidth + x] = value;
        }
        public int LevelWidth { get => _levelWidth; set => _levelWidth = value; }
        public int LevelHeight { get => _levelHeight; set => _levelHeight = value; }
    }
}