using System.Threading.Tasks.Dataflow;
using SplashKitSDK;
namespace CustomProgram
{
    class Game
    {
        private String _windowName;
        private String _currentLevel = "./levels/1.lvl";
        public Game() : this("CustomProgram") { }
        public Game(String windowTitle)
        {
            _windowName = windowTitle;
        }
        public void Run()
        {
            SplashKit.OpenWindow(_windowName, 800, 600);

            Level myLevel = new Level(_currentLevel);
            Player p1 = new Player(myLevel);
            p1.X = 3;
            p1.Y = 3;

            while (!SplashKit.WindowCloseRequested(_windowName))
            {
                SplashKit.ProcessEvents();
                // player input
                if (SplashKit.KeyDown(KeyCode.WKey))
                {
                    p1.Move(PlayerMoveDirection.Up);
                }
                else if (SplashKit.KeyDown(KeyCode.SKey))
                {
                    p1.Move(PlayerMoveDirection.Down);
                }
                else if (SplashKit.KeyDown(KeyCode.AKey))
                {
                    p1.Move(PlayerMoveDirection.Left);
                }
                else if (SplashKit.KeyDown(KeyCode.DKey))
                {
                    p1.Move(PlayerMoveDirection.Right);
                }
                // debug keybinds
                if (SplashKit.KeyTyped(KeyCode.RKey))
                {
                    myLevel.Load(_currentLevel);
                }
                p1.Update();
                SplashKit.ClearScreen(Color.Black);

                myLevel.Draw();
                p1.Draw();
                // draw scaled buffer
                SplashKit.RefreshScreen(60);
            }
            SplashKit.CloseAllWindows();
        }
    }
}