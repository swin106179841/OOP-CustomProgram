using SplashKitSDK;
using CustomProgram;
namespace ProgramEditor
{

    class Editor
    {
        private String _windowName;
        private Window _window;
        private String _currentLevel = "./levels/2.lvl";
        private int _mapOffsetX = 200;
        private int _selectedSprite = 0;
        public Editor() : this("CustomProgram Editor") { }
        public Editor(String windowTitle)
        {
            _windowName = windowTitle;
            _window = SplashKit.OpenWindow(_windowName, 1000, 600);

            Settings.RenderScale = 1;

        }
        public void Run()
        {
            Bitmap background = SplashKit.LoadBitmap("background", "./assets/background.png");

            Bitmap spriteSheet = SplashKit.LoadBitmap("spriteSheet", "./assets/med-tileset.png");
            SplashKit.BitmapSetCellDetails(spriteSheet, 32, 32, 6, 6, 36);

            Level myLevel = new Level(15, 10, spriteSheet);

            // update window width to fit level
            SplashKit.ProcessEvents();
            _window.Resize(_mapOffsetX + (myLevel.LevelWidth * 32), myLevel.LevelHeight * 32);
            while (!SplashKit.WindowCloseRequested(_windowName))
            {
                SplashKit.ProcessEvents();

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

                myLevel.Draw(_mapOffsetX, 0);
                myLevel.DrawObjects(_mapOffsetX, 0);

                SplashKit.RefreshScreen(60);
            }
            SplashKit.CloseAllWindows();
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
            for (int y = 0; y < lvl.LevelHeight; y++)
            {
                for (int x = 0; x < lvl.LevelWidth; x++)
                {
                    switch (lvl.TileAt(x, y))
                    {
                        case 30:
                            GameObject rock = new ObjectStatic(x, y, 30, lvl.Textures.RequestTexture(0));
                            lvl.AddObject(rock);
                            lvl.SetTile(x, y, 13);
                            break;
                        case 31:
                            GameObject barrel = new ObjectPushable(x, y, 31, lvl.Textures.RequestTexture(0));
                            lvl.AddObject(barrel);
                            lvl.SetTile(x, y, 13);
                            break;
                        case 32:
                            GameObject bush = new ObjectStatic(x, y, 32, lvl.Textures.RequestTexture(0));
                            lvl.AddObject(bush);
                            lvl.SetTile(x, y, 13);
                            break;
                        case 33:
                            GameObject stone = new ObjectPushable(x, y, 33, lvl.Textures.RequestTexture(0));
                            lvl.AddObject(stone);
                            lvl.SetTile(x, y, 13);
                            break;
                    }
                }
            }
        }
    }
}
