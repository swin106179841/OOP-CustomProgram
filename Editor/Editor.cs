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
    }
}
