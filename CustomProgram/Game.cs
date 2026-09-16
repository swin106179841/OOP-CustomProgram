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

            Level myLevel = new Level("./levels/1.lvl");

            while (!SplashKit.WindowCloseRequested(_windowName))
            {
                SplashKit.ProcessEvents();
                SplashKit.ClearScreen(Color.Black);

                myLevel.Draw();
                // draw scaled buffer
                SplashKit.RefreshScreen(60);
            }
            SplashKit.CloseAllWindows();
        }
    }
}