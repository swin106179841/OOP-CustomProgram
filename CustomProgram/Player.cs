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
        private bool _isMoving = false;
        private int _frameCount = 0;
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
            if (!_isMoving)
            {
                // check collision
                // check ground
                int nextTile = -1;
                switch (dir)
                {
                    case PlayerMoveDirection.Up:
                        nextTile = _level.TileAt(X, Y - 1);
                        break;
                    case PlayerMoveDirection.Down:
                        nextTile = _level.TileAt(X, Y + 1);
                        break;
                    case PlayerMoveDirection.Left:
                        nextTile = _level.TileAt(X - 1, Y);
                        break;
                    case PlayerMoveDirection.Right:
                        nextTile = _level.TileAt(X + 1, Y);
                        break;
                }
                Console.WriteLine("next tile int dir {0} is {1}", dir, nextTile);
                if (_level.IsFloor(nextTile))
                {
                    _direction = dir;
                    _isMoving = true;
                }
            }
        }
        public void Update()
        {
            // update position according to animation
            if (_isMoving)
            {
                // 60fps so move for 30 frames
                // 1 / 30 = 0.0333
                _frameCount++;
                switch (_direction)
                {
                    case PlayerMoveDirection.Up:
                        Y -= Settings.PlayerMoveSpeed;
                        break;
                    case PlayerMoveDirection.Down:
                        Y += Settings.PlayerMoveSpeed;
                        break;
                    case PlayerMoveDirection.Left:
                        X -= Settings.PlayerMoveSpeed;
                        break;
                    case PlayerMoveDirection.Right:
                        X += Settings.PlayerMoveSpeed;
                        break;
                }

                if (_frameCount >= 30)
                {
                    _isMoving = false;
                    _frameCount = 0;
                    X = (float)Math.Round(X);
                    Y = (float)Math.Round(Y);
                }
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