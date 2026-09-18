using System.Threading.Tasks.Dataflow;
using SplashKitSDK;
namespace CustomProgram
{
    class Game
    {
        private String _windowName;
        private Window _window;
        private String _currentLevel = "./levels/3.lvl";
        public Game() : this("CustomProgram") { }
        public Game(String windowTitle)
        {
            _windowName = windowTitle;
            _window = SplashKit.OpenWindow(_windowName, 800, 600);
        }
        public void Run()
        {
            Bitmap background = SplashKit.LoadBitmap("background", "./assets/background.png");

            Level myLevel = new Level(_currentLevel);
            Player p1 = new Player(myLevel);
            p1.X = 3;
            p1.Y = 4;

            _window.Resize((int)(myLevel.LevelWidth * 32 * Settings.RenderScale), (int)(myLevel.LevelHeight * 32 * Settings.RenderScale));
            while (!SplashKit.WindowCloseRequested(_windowName))
            {
                SplashKit.ProcessEvents();
                // player input
                if (SplashKit.KeyDown(KeyCode.WKey))
                {
                    p1.Move(MoveDirection.Up);
                }
                else if (SplashKit.KeyDown(KeyCode.SKey))
                {
                    p1.Move(MoveDirection.Down);
                }
                else if (SplashKit.KeyDown(KeyCode.AKey))
                {
                    p1.Move(MoveDirection.Left);
                }
                else if (SplashKit.KeyDown(KeyCode.DKey))
                {
                    p1.Move(MoveDirection.Right);
                }
                // debug keybinds
                if (SplashKit.KeyTyped(KeyCode.RKey))
                {
                    myLevel.Load(_currentLevel);
                }
                p1.Update();
                myLevel.Update();
                // SplashKit.ClearScreen(Color.Black);
                background.Draw(0, 0, SplashKit.OptionScaleBmp(Settings.RenderScale, Settings.RenderScale));
                myLevel.Draw();
                p1.Draw();
                myLevel.DrawObjects();
                // draw scaled buffer
                SplashKit.RefreshScreen(60);
            }
            SplashKit.CloseAllWindows();
        }
    }
}