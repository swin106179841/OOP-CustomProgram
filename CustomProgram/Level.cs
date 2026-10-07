using ShapeDrawer;
using SplashKitSDK;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
namespace CustomProgram
{
    public class Level
    {
        private int _levelWidth;
        private int _levelHeight;
        private List<int> _scene = [];
        private List<GameObject> _objects = [];
        private List<int> _floorTiles = [7, 10, 35, 19, 25];
        private List<int> _specialTiles = [34];
        private List<int> _doorTiles = [18, 24];
        private List<int> _exitTiles = [19, 25];
        private int _goalsCollected = 0;
        private int _goalsNeeded = 0;
        private TextureManager _texMan = new TextureManager();


        public Level(int w, int h, Bitmap spriteSheet)
        {
            _levelWidth = w;
            _levelHeight = h;
            _texMan.AddTexture(spriteSheet);
            for (int i = 0; i < w * h; i++)
            {
                _scene.Add(_floorTiles[0]);
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
            if (sr.ReadLine() != "LVLV2")
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
            // load floor and special tiles
            try
            {
                int floorTileCount = sr.ReadInteger();
                for (int i = 0; i < floorTileCount; i++)
                {
                    _floorTiles.Add(sr.ReadInteger());
                }
                int specialTileCount = sr.ReadInteger();
                for (int i = 0; i < specialTileCount; i++)
                {
                    _specialTiles.Add(sr.ReadInteger());
                }
                _goalsNeeded = sr.ReadInteger();
                int doorTileCount = sr.ReadInteger();
                for (int i = 0; i < doorTileCount; i++)
                {
                    _doorTiles.Add(sr.ReadInteger());
                }
                int exitTileCount = sr.ReadInteger();
                for (int i = 0; i < exitTileCount; i++)
                {
                    _exitTiles.Add(sr.ReadInteger());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading floor/special tile info from level: " + levelPath);
                Console.WriteLine("Error: " + e.Message);
                errorCount++;
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
                        case "CustomProgram.ObjectStatic":
                            obj = new ObjectStatic();
                            break;
                        case "CustomProgram.ObjectPushable":
                            obj = new ObjectPushable();
                            break;
                        case "CustomProgram.ObjectCollectable":
                            obj = new ObjectCollectable();
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
            _floorTiles.Clear();
            _specialTiles.Clear();
            _goalsCollected = 0;
            _doorTiles.Clear();
            _exitTiles.Clear();
        }
        public void Save()
        {
            int sum = (_scene[0] << 3) & _scene[_levelHeight * _levelWidth - 1] ^ _scene[(_levelHeight / 2) * (_levelWidth / 2)];
            String filename = String.Format("./levels/{0}x{1}-{2}.lvl", _levelWidth, _levelHeight, sum);
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("LVLV2"); // magic
            sw.WriteLine(_texMan.RequestTexture(0).Filename); // spritesheet
            sw.WriteLine("32\n32"); // tile w, tile h
            sw.WriteLine(6); // tiles accross
            sw.WriteLine(6); // tiles down
            sw.WriteLine(36); // tiles total
            sw.WriteLine(_floorTiles.Count); // how many floor tiles (walkable)
            foreach (int floorTile in _floorTiles)
            {
                sw.WriteLine(floorTile); // walkable tile value
            }
            sw.WriteLine(_specialTiles.Count); // special tile count
            foreach (int specTile in _specialTiles)
            {
                sw.WriteLine(specTile);
            }
            sw.WriteLine(_goalsNeeded); // required collectables
            sw.WriteLine(_doorTiles.Count); // door tiles
            foreach (int doorTile in _doorTiles)
            {
                sw.WriteLine(doorTile);
            }
            sw.WriteLine(_exitTiles.Count); // exit tiles
            foreach (int exitTile in _exitTiles)
            {
                sw.WriteLine(exitTile);
            }
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
        public bool IsSpecial(int tile)
        {
            foreach (int t in _specialTiles)
            {
                if (tile == t)
                    return true;
            }
            return false;
        }
        // use for collisions
        public GameObject? ObjectAt(int x, int y)
        {
            foreach (GameObject obj in _objects)
            {
                if (obj.X == x && obj.Y == y)
                {
                    if ((obj as ObjectCollectable) != null)
                    {
                        return null; // dont collide collectables
                    }
                    else
                    {
                        return obj;
                    }
                }
            }
            return null;
        }
        // use for data
        public GameObject? AnyObjectAt(int x, int y)
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
        public void AddObject(GameObject obj)
        {
            _objects.Add(obj);
        }
        public void SetTile(int x, int y, int value)
        {
            if (x < 0 || x > _levelWidth || y < 0 || y > _levelHeight)
                return;
            _scene[y * _levelWidth + x] = value;
        }
        // check objects for special interactions
        public void CheckObjects(Player player)
        {
            // iterate backwards to avoid shifting order before loop finishes
            for (int i = _objects.Count()-1; i >= 0; i--)
            {
                GameObject obj = _objects[i];
                if ((obj as ObjectPushable)?.Type == PushableTypes.Stone && (obj as ObjectPushable)?.IsMoving == false)
                {
                    if (TileAt(obj.X, obj.Y) == 34)
                    {
                        // push stone into hole
                        // change spriteindex
                        SetTile((int)obj.X, (int)obj.Y, 35);
                        // delete obj
                        _objects.Remove(obj);
                        return;
                    }
                }
                else if ((obj as ObjectCollectable)?.X == player.X && (obj as ObjectCollectable)?.Y == player.Y)
                {
                    // collect coin (goal)
                    GameObject? goal = AnyObjectAt((int)player.X, (int)player.Y);
                    if (goal != null)
                    {
                        _objects.Remove(obj);
                        _goalsCollected++;
                    }
                }
            }
        }
        public int LevelWidth { get => _levelWidth; set => _levelWidth = value; }
        public int LevelHeight { get => _levelHeight; set => _levelHeight = value; }
        public TextureManager Textures => _texMan;
        public int FirstFloor => _floorTiles[0];
    }
}