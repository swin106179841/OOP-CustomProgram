using SplashKitSDK;
using CustomProgram;
namespace ProgramEditor
{

    class Editor
    {
        Window _window;
        // used to edit existing level. requires recompilation
        private String? _currentLevel = null;
        private int _mapOffsetX = 200;
        private int _selectedSprite = 0;
        private Bitmap background;
        private Bitmap spriteSheet;
        private Level myLevel;
        private bool _quitEditor = false;
        private float _prevScale;

        public Editor(Window window)
        {
            _window = window;
            _currentLevel = null;
            background = SplashKit.LoadBitmap("background", "./assets/background.png");

            spriteSheet = SplashKit.LoadBitmap("spriteSheet", "./assets/cus-tileset.png");
            SplashKit.BitmapSetCellDetails(spriteSheet, 32, 32, 6, 6, 36);
            myLevel = new Level(15, 10, spriteSheet);
            // Setup();
            _prevScale = Settings.RenderScale;
        }
        public void Setup()
        {
            Settings.RenderScale = 1;


            if (_currentLevel != null)
            {
                myLevel.Load(_currentLevel);
            } else
            {
                myLevel.Load("./levels/empty.lvl");
            }
        }
        public void Run()
        {
            
            
            // update window width to fit level
            // handle mouse interaction
            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                // selecting tile
                if (SplashKit.MouseX() < 6 * 32 && SplashKit.MouseY() < 6 * 32)
                {
                    int tX = (int)(SplashKit.MouseX() / 32);
                    int tY = (int)(SplashKit.MouseY() / 32);
                    _selectedSprite = tY * 6 + tX;
                }
                // placing tile
                if (SplashKit.MouseX() > 199 && SplashKit.MouseX() < _mapOffsetX + myLevel.LevelWidth * 32 && SplashKit.MouseY() < myLevel.LevelHeight * 32)
                {
                    int tX = (int)(SplashKit.MouseX() - _mapOffsetX) / 32;
                    int tY = (int)(SplashKit.MouseY() / 32);
                    myLevel.SetTile(tX, tY, _selectedSprite);
                }
                // save btn
                if (SplashKit.MouseX() > 63 && SplashKit.MouseX() < 63 + 32 && SplashKit.MouseY() > _window.Height - 32)
                {
                    ConvertTileToObject(myLevel);
                    myLevel.Save();
                }
                // exit btn 
                if (SplashKit.MouseX() > 95 && SplashKit.MouseX() < 95 + 32 && SplashKit.MouseY() > _window.Height - 32)
                {
                    _quitEditor = true;
                }
            }
            if (SplashKit.MouseClicked(MouseButton.RightButton))
            {
                if (SplashKit.MouseX() > 199 && SplashKit.MouseX() < _mapOffsetX + myLevel.LevelWidth * 32 && SplashKit.MouseY() < myLevel.LevelHeight * 32)
                {
                    int tX = (int)(SplashKit.MouseX() - _mapOffsetX) / 32;
                    int tY = (int)(SplashKit.MouseY() / 32);
                    myLevel.SetTile(tX, tY, -1);
                }
            }
            SplashKit.FillRectangle(Color.DarkGray, 0, 0, _window.Width, _window.Height);

            // draw tiles
            spriteSheet.Draw(0, 0);

            // draw selected 
            SplashKit.DrawBitmap(spriteSheet, 0, _window.Height - 32, SplashKit.OptionWithBitmapCell(_selectedSprite));
            // save btn
            SplashKit.FillRectangle(Color.LimeGreen, 64, _window.Height - 32, 32, 32);
            // quit btn
            SplashKit.FillRectangle(Color.Red, 96, _window.Height - 32, 32, 32);

            myLevel.Draw(_mapOffsetX, 0);
            myLevel.DrawObjects(_mapOffsetX, 0);

        }
        void ConvertTileToObject(Level lvl)
        {
            // iterate lvl tiles
            // for each object tile create associated objects
            // replace tile with grass (tile 13 | 22)
            // rock = 30
            // barrel = 31
            // bush = 32
            // stone = 33
            // coin = 20
            for (int y = 0; y < lvl.LevelHeight; y++)
            {
                for (int x = 0; x < lvl.LevelWidth; x++)
                {
                    switch (lvl.TileAt(x, y))
                    {
                        case 20:
                            GameObject coin = new ObjectCollectable(x, y, 20, lvl.Textures.RequestTexture(0));
                            lvl.AddObject(coin);
                            lvl.SetTile(x, y, lvl.FirstFloor);
                            break;
                        case 30:
                            GameObject rock = new ObjectStatic(x, y, 30, lvl.Textures.RequestTexture(0));
                            lvl.AddObject(rock);
                            lvl.SetTile(x, y, lvl.FirstFloor);
                            break;
                        case 31:
                            GameObject barrel = new ObjectPushable(x, y, 31, lvl.Textures.RequestTexture(0));
                            lvl.AddObject(barrel);
                            lvl.SetTile(x, y, lvl.FirstFloor);
                            break;
                        case 32:
                            GameObject bush = new ObjectStatic(x, y, 32, lvl.Textures.RequestTexture(0));
                            lvl.AddObject(bush);
                            lvl.SetTile(x, y, lvl.FirstFloor);
                            break;
                        case 33:
                            GameObject stone = new ObjectPushable(x, y, 33, lvl.Textures.RequestTexture(0));
                            lvl.AddObject(stone);
                            lvl.SetTile(x, y, lvl.FirstFloor);
                            break;
                    }
                }
            }
        }
        public bool QuitEditor {get => _quitEditor; set => _quitEditor = value; }
        public float RestoreScale {get => _prevScale;}
    }
}
