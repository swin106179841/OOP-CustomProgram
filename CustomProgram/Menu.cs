using System.Dynamic;
using System.Runtime.CompilerServices;
using SplashKitSDK;
namespace CustomProgram
{
    public abstract class Menu
    {
        public abstract void Update();
        public abstract void Draw();

    }
    public interface IMenuItem
    {
        public string Text {get;set;}
        public int X {get;set;}
        public int Y {get;set;}
        public int W {get;set;}
        public int H {get;set;}
        public void Draw();

    }
    public interface IMenuClickable : IMenuItem
    {
        public void Click();
    }
    public class LevelButton : IMenuClickable
    {
        private string _text;
        private int _x;
        private int _y;
        public int _w;
        public int _h;
        public LevelButton(string text, int x, int y, int w, int h)
        {
            _text = text;
            _x = x;
            _y = y;    
            _w = w;
            _h = h;
        }
        public void Draw() {}
        public void Click() {}
        public string Text {get => _text; set => _text = value;}
        public int X {get => _x; set => _x = value;}
        public int Y {get => _y; set => _y = value;}
        public int W {get => _w; set => _w = value;}
        public int H {get => _h; set => _h = value;}
    }

    // main menu used for:
    // - listing levels
    // - selecting level to play
    public class MainMenu : Menu
    {
        List<IMenuItem> _widgets;
        IMenuClickable? _selected;
        string? _chosenLevel;
        public MainMenu()
        {
            _widgets = new List<IMenuItem>{};
            // load levels and create button for each
        }
        public override void Update()
        {
            if (_selected != null)
                _selected.Click();

            if (SplashKit.MouseClicked(MouseButton.LeftButton))
            {
                SelectItem((int)SplashKit.MouseX(), (int)SplashKit.MouseY());
            }
        }
        public override void Draw()
        {
            foreach (IMenuItem itm in _widgets)
            {
                itm.Draw();
            }
        }
        private void SelectItem(int x, int y)
        {
            foreach (IMenuItem itm in _widgets)
            {
                if (x > itm.X && x <= itm.W + itm.X && y > itm.Y && y < itm.Y + itm.H)
                {
                    _selected = itm as IMenuClickable;
                }
            }
        }
        public string? ChosenLevel {get=>_chosenLevel;}
    }
}