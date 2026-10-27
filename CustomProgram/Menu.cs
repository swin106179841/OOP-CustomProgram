using System.Dynamic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ShapeDrawer;
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
        public string Text { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public void Draw();
        public void Load(StreamReader sr);

    }
    public interface IMenuButton
    {
        public void Click();
        public bool Clicked {get; set;}
    }
    public class LevelButton : MenuLabel, IMenuButton
    {
        private string _lvlPath;
        private bool _clicked;

        public LevelButton(string text, int x, int y, int w, int h) : base(text, x, y)
        {
            W = w;
            H = h;
            _lvlPath = "";
            _clicked = false;
        }
        public LevelButton() : this("", 0, 0, 0, 0) { }
        public override void Draw()
        {
            SplashKit.FillRectangle(Color.RGBColor(30, 30, 30), X-20, Y-5, W, H);
            SplashKit.DrawText(Text, Color.White, X, Y);
        }
        public override void Load(StreamReader sr)
        {
            base.Load(sr);
            _lvlPath = sr.ReadLine()!;
        }
        public void Click()
        {
            _clicked = true;
        }
        public string LvlPath => _lvlPath;
        public bool Clicked { get => _clicked; set => _clicked = value; }
    }
    public class EditorButton : MenuLabel, IMenuButton
    {
        private bool _clicked;
        public EditorButton() : base("Editor", 0, 0)
        {
            _clicked = false;
        }
        public override void Draw()
        {
            SplashKit.FillRectangle(Color.RGBColor(30, 30, 30), X-20, Y-5, W, H);
            SplashKit.DrawText(Text, Color.White, X, Y);
        }
        public bool Clicked { get => _clicked; set => _clicked = value; }

        public void Click()
        {
            _clicked = true;
        }
    }
    public class MenuLabel : IMenuItem
    {
        private string _text;
        private int _x;
        private int _y;
        private int _w;
        private int _h;
        public MenuLabel(string text, int x, int y)
        {
            _text = text;
            _x = x;
            _y = y;
            _w = 0;
            _h = 0;
        }
        public MenuLabel() : this("", 0, 0) {}
        public virtual void Draw()
        {
            SplashKit.DrawText(_text, Color.White, _x, _y);
        }
        public virtual void Load(StreamReader sr)
        {
            Text = sr.ReadLine()!;
            X = sr.ReadInteger()!;
            Y = sr.ReadInteger()!;
            W = sr.ReadInteger()!;
            H = sr.ReadInteger()!;
        }
        public string Text { get => _text; set => _text = value; }
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        public int W { get => _w; set => _w = value; }
        public int H { get => _h; set => _h = value; }

    }
    public class MenuImage: MenuLabel
    {
        private Bitmap _img;
        public MenuImage(): base("fallback", 0, 0)
        {
            _img = SplashKit.LoadBitmap("fallback", "./assets/fallback.png");
        }
        public MenuImage(string path, int x, int y): base(path, x, y)
        {
            _img = SplashKit.LoadBitmap($"menuimg-{path}", path);
        }
        public override void Draw()
        {
            _img.Draw(X, Y);
        }
        public override void Load(StreamReader sr)
        {
            base.Load(sr);
            _img = SplashKit.LoadBitmap($"menuimg-{Text}", Text);
        }
    }

    // main menu used for:
    // - listing ui elems to select level
    public class MainMenu : Menu
    {
        List<IMenuItem> _widgets;
        IMenuButton? _selected;
        string? _chosenLevel;
        bool _editorRequested = false;
        public MainMenu(string path)
        {
            _widgets = new List<IMenuItem> { };
            // load levels and create button for each
            StreamReader sr = new StreamReader("./menus/main.menu");

            int widgetCount = sr.ReadInteger();
            for (int i = 0; i < widgetCount; i++)
            {
                IMenuItem widget;
                string type = sr.ReadLine()!;
                switch (type)
                {
                    case "LevelButton":
                        widget = new LevelButton();
                        break;
                    case "MenuLabel":
                        widget = new MenuLabel();
                        break;
                    case "MenuImage":
                        widget = new MenuImage();
                        break;
                    case "EditorButton":
                        widget = new EditorButton();
                        break;
                    default:
                        widget = new MenuLabel();
                        break;
                }
                widget.Load(sr);
                _widgets.Add(widget);
            }
        }
        public override void Update()
        {
            if (_selected != null)
            {
                _selected.Click();
                if (_selected.Clicked)
                {
                    _selected.Clicked = false;
                    // handle type 
                    if (_selected as LevelButton != null)
                    {
                        _chosenLevel = (_selected as LevelButton)!.LvlPath;
                    } else if (_selected as EditorButton != null)
                    {
                        _editorRequested = true;
                    }
                }
            }

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
                    _selected = itm as IMenuButton;

                }
            }
        }
        public void Reset()
        {
            _selected = null;
            _chosenLevel = null;
            _editorRequested = false;
        }
        public string? ChosenLevel { get => _chosenLevel; }
        public bool EditorRequested { get => _editorRequested; }
    }
}