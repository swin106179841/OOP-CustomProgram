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

            while (!SplashKit.WindowCloseRequested(_windowName))
            {
                SplashKit.ProcessEvents();
                // player input
                if (SplashKit.KeyTyped(KeyCode.WKey))
                {
                    p1.Move(PlayerMoveDirection.Up);
                }
                else if (SplashKit.KeyTyped(KeyCode.SKey))
                {
                    p1.Move(PlayerMoveDirection.Down);
                }
                else if (SplashKit.KeyTyped(KeyCode.AKey))
                {
                    p1.Move(PlayerMoveDirection.Left);
                }
                else if (SplashKit.KeyTyped(KeyCode.DKey))
                {
                    p1.Move(PlayerMoveDirection.Right);
                }
                // debug keybinds
                if (SplashKit.KeyTyped(KeyCode.RKey))
                {
                    myLevel.Load(_currentLevel);
                }
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