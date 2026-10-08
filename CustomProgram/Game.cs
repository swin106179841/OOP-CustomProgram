using System.Threading.Tasks.Dataflow;
using SplashKitSDK;
namespace CustomProgram
{
    class Game
    {
        private String _windowName;
        private Window _window;
        private String _currentLevel = "./levels/6.lvl";
        private GameState _gameState;
        private Bitmap _winScr;
        private int _winScrFrameCount;
        public Game() : this("Temple of Fortune") { }
        public Game(String windowTitle)
        {
            _windowName = windowTitle;
            _window = SplashKit.OpenWindow(_windowName, 800, 600);
            _winScr = SplashKit.LoadBitmap("win", "./assets/win.png");
        }
        public void Run()
        {
            _gameState = GameState.Playing;
            Bitmap background = SplashKit.LoadBitmap("background", "./assets/background.png");

            Level myLevel = new Level(_currentLevel);
            Player p1 = new Player(myLevel);
            p1.X = 5;
            p1.Y = 6;

            _window.Resize((int)(myLevel.LevelWidth * 32 * Settings.RenderScale), (int)(myLevel.LevelHeight * 32 * Settings.RenderScale));
            while (!SplashKit.WindowCloseRequested(_windowName))
            {
                SplashKit.ProcessEvents();
                // clear screen with background
                background.Draw(0, 0, SplashKit.OptionScaleBmp(Settings.RenderScale, Settings.RenderScale));
                // While Playing 
                // ---------------------------------------------------------------
                if (_gameState == GameState.Playing)
                {
                    // check objects
                    myLevel.CheckObjects(p1);
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
                    myLevel.Draw();
                    p1.Draw();
                    myLevel.DrawObjects();
                    // check game win to update game state
                    _gameState = myLevel.CheckWin(p1);
                } else if (_gameState == GameState.Win)
                {
                    _winScr.Draw(_window.Width / 2 - (_winScr.Width / 2), _window.Height / 2 - (_winScr.Height / 2));
                    _winScrFrameCount++;

                    // wait for 3 seconds (3sec * 60frames = 180frames)
                    if (_winScrFrameCount == 180)
                        _gameState = GameState.Menu;

                }
                SplashKit.RefreshScreen(60);
            }
            SplashKit.CloseAllWindows();
        }
    }
}