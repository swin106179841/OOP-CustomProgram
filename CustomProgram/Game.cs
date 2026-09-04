using SplashKitSDK;
namespace CustomProgram
{
    class Game
    {
        private String _windowName;
        public Game(): this("CustomProgram") {}
        public Game(String windowTitle)
        {
            _windowName = windowTitle;
        }
        public void Run()
        {
            SplashKit.OpenWindow(_windowName, 800, 600);
            while (!SplashKit.WindowCloseRequested(_windowName))
            {
                SplashKit.ProcessEvents();
                SplashKit.ClearScreen(Color.Blue);


                SplashKit.RefreshScreen(60);
            }
            SplashKit.CloseAllWindows();
        }
    }
}