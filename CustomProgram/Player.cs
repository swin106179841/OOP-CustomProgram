using SplashKitSDK;

namespace CustomProgram
{
    public enum PlayerMoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    public class Player : IDrawable
    {
        private float _x;
        private float _y;
        private PlayerMoveDirection _direction;
        private Level _level;
        private Bitmap _texture;
        public DrawingOptions DOpts;
        public Player(Level level)
        {
            _level = level;
            _texture = SplashKit.LoadBitmap("fallback", "./assets/fallback.png");
            DOpts = SplashKit.OptionDefaults();
            DOpts.ScaleX = Settings.RenderScale;
            DOpts.ScaleY = Settings.RenderScale;
        }
        public void Draw()
        {
            SplashKit.DrawBitmap(_texture, X * 32 * Settings.RenderScale, Y * 32 * Settings.RenderScale, DOpts);
        }
        public void Move(PlayerMoveDirection dir)
        {
            switch (dir)
            {
                case PlayerMoveDirection.Up:
                    // check collision then move
                    Y -= 1;
                    break;
                case PlayerMoveDirection.Down:
                    Y += 1;
                    break;
                case PlayerMoveDirection.Left:
                    X -= 1;
                    break;
                case PlayerMoveDirection.Right:
                    X += 1;
                    break;
                default:
                    break;
            }
        }
        public void SetLevel(Level level) { _level = level; }
        public float X { get => _x; set => _x = value; }
        public float Y { get => _y; set => _y = value; }
        public PlayerMoveDirection Direction { get => _direction; set => _direction = value; }
        public Bitmap Texture
        {
            get => _texture;
            set => _texture = value;
        }
    }
}