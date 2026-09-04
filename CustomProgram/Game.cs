using System.Threading.Tasks.Dataflow;
using SplashKitSDK;
namespace CustomProgram
{
    class Game
    {
        private String _windowName;
        public Game() : this("CustomProgram") { }
        public Game(String windowTitle)
        {
            _windowName = windowTitle;
        }
        public void Run()
        {
            SplashKit.OpenWindow(_windowName, 800, 600);

            // TextureManager tm = new TextureManager();
            Bitmap spriteSheet = SplashKit.LoadBitmap("background", "./assets/mini-tileset.png");
            SplashKit.BitmapSetCellDetails(spriteSheet, 32, 32, 6, 4, 24);

            int[] scene = [
                0, 1, 1, 1, 1, 1, 1, 3,
                6, 8, 7, 7, 8, 8, 7, 9,
                12, 22, 23, 22, 22, 23, 23, 15,
                12, 22, 22, 23, 22, 22, 22, 15,
                12, 23, 22, 22, 22, 22, 23, 15,
                12, 23, 23, 22, 22, 23, 23, 15,
            ];

            Object rock = new Object(1, 2, 0);
            Bitmap rockBmp = SplashKit.LoadBitmap("rock", "./assets/rock.png");

            ObjectInteractable barrel = new ObjectInteractable(2, 2, 0);
            Bitmap barrelBmp = SplashKit.LoadBitmap("barrel", "./assets/barrel.png");
            barrel.HeightOffset = 32;

            while (!SplashKit.WindowCloseRequested(_windowName))
            {
                SplashKit.ProcessEvents();
                // SplashKit.ClearScreen(Color.RGBColor(114, 117, 27));
                SplashKit.ClearScreen(Color.Black);

                // tm.Fallback.Draw(15, 5);
                int index = 0;
                for (int y = 0; y < 6; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        DrawingOptions cellOpts = SplashKit.OptionWithBitmapCell(scene[index++]);
                        cellOpts.ScaleX = Settings.RenderScale;
                        cellOpts.ScaleY = Settings.RenderScale;
                        SplashKit.DrawBitmap(spriteSheet, x * (32 * Settings.RenderScale), y * (32 * Settings.RenderScale), cellOpts);
                    }
                }
                rock.Draw(rockBmp);
                barrel.Draw(barrelBmp);

                // draw scaled buffer
                SplashKit.RefreshScreen(60);
            }
            SplashKit.CloseAllWindows();
        }
    }
}