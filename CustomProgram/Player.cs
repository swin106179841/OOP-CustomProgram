using SplashKitSDK;

namespace CustomProgram
{
    public class Player : IDrawable
    {
        private float _x;
        private float _y;
        private MoveDirection _direction;
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
        public void Move(MoveDirection dir)
        {
            if (!_isMoving)
            {
                // check collision for objects
                GameObject? obj = null;
                GameObject? followingObj = null;
                // check ground
                int nextTile = -1;
                int followingTile = -1;
                switch (dir)
                {
                    case MoveDirection.Up:
                        nextTile = _level.TileAt(X, Y - 1);
                        followingTile = _level.TileAt(X, Y - 2);
                        obj = _level.ObjectAt((int)X, (int)Y - 1);
                        followingObj = _level.ObjectAt((int)X, (int)Y - 2);
                        break;
                    case MoveDirection.Down:
                        nextTile = _level.TileAt(X, Y + 1);
                        followingTile = _level.TileAt(X, Y + 2);
                        obj = _level.ObjectAt((int)X, (int)Y + 1);
                        followingObj = _level.ObjectAt((int)X, (int)Y + 2);
                        break;
                    case MoveDirection.Left:
                        nextTile = _level.TileAt(X - 1, Y);
                        followingTile = _level.TileAt(X - 2, Y);
                        obj = _level.ObjectAt((int)X - 1, (int)Y);
                        followingObj = _level.ObjectAt((int)X - 2, (int)Y);
                        break;
                    case MoveDirection.Right:
                        nextTile = _level.TileAt(X + 1, Y);
                        followingTile = _level.TileAt(X + 2, Y);
                        obj = _level.ObjectAt((int)X + 1, (int)Y);
                        followingObj = _level.ObjectAt((int)X + 2, (int)Y);
                        break;
                }
                if (_level.IsFloor(nextTile))
                {
                    if (obj == null)
                    {
                        _direction = dir;
                        _isMoving = true;
                    } else if (obj is ObjectPushable pushable && _level.IsFloor(followingTile) && followingObj == null)
                    {
                        _direction = dir;
                        _isMoving = true;
                        pushable.Move(dir);
                    }
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
                    case MoveDirection.Up:
                        Y -= Settings.PlayerMoveSpeed;
                        break;
                    case MoveDirection.Down:
                        Y += Settings.PlayerMoveSpeed;
                        break;
                    case MoveDirection.Left:
                        X -= Settings.PlayerMoveSpeed;
                        break;
                    case MoveDirection.Right:
                        X += Settings.PlayerMoveSpeed;
                        break;
                }

                if (_frameCount >= Settings.MoveFrames)
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
        public MoveDirection Direction { get => _direction; set => _direction = value; }
        public Bitmap Texture
        {
            get => _texture;
            set => _texture = value;
        }
    }
}