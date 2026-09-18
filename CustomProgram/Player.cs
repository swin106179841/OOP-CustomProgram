using SplashKitSDK;

namespace CustomProgram
{
    public class Player : IDrawable, IAnimated
    {
        private float _x;
        private float _y;
        private MoveDirection _direction;
        private Level _level;
        private Bitmap _texture;
        public DrawingOptions DOpts;
        private bool _isMoving = false;
        private int _frameCount = 0;
        private int _animationFrame = 0;
        private int _spriteIndex = 0;
        public Player(Level level)
        {
            _level = level;
            _texture = SplashKit.LoadBitmap("player", "./assets/player.png");
            SplashKit.BitmapSetCellDetails(_texture, 32, 32, 6, 2, 10);
            DOpts = SplashKit.OptionWithBitmapCell(_spriteIndex);
            DOpts.ScaleX = Settings.RenderScale;
            DOpts.ScaleY = Settings.RenderScale;
            _direction = MoveDirection.Down;
        }
        public void Draw()
        {
            SplashKit.DrawBitmap(_texture, X * 32 * Settings.RenderScale, Y * 32 * Settings.RenderScale, DOpts);
        }
        public void Move(MoveDirection dir)
        {
            if (!_isMoving)
            {
                _direction = dir;
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
                        _isMoving = true;
                    }
                    else if (obj is ObjectPushable pushable && _level.IsFloor(followingTile) && followingObj == null)
                    {
                        _isMoving = true;
                        pushable.Move(dir);
                    }
                }
            }
        }
        public void Update()
        {
            Animate();
            // update position according to animation
            if (_isMoving)
            {
                DOpts = SplashKit.OptionWithBitmapCell((int)_direction + 6, DOpts);
                // 60fps so move for 30 frames
                // 1 / 30 = 0.0333
                _frameCount++;
                switch (_direction)
                {
                    case MoveDirection.Up:
                        Y -= Settings.MoveSpeed;
                        break;
                    case MoveDirection.Down:
                        Y += Settings.MoveSpeed;
                        break;
                    case MoveDirection.Left:
                        X -= Settings.MoveSpeed;
                        break;
                    case MoveDirection.Right:
                        X += Settings.MoveSpeed;
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
        public void Animate()
        {
            // if (!_isMoving)
            {
                AnimationFrame++;
                if (AnimationFrame >= Settings.AnimationTimePerFrame)
                {
                    _spriteIndex = (_spriteIndex + 1) % 6;
                    DOpts = SplashKit.OptionWithBitmapCell(_spriteIndex, DOpts);
                    AnimationFrame = 0;
                }
            }
        }

        public float X { get => _x; set => _x = value; }
        public float Y { get => _y; set => _y = value; }
        public MoveDirection Direction { get => _direction; set => _direction = value; }
        public Bitmap Texture
        {
            get => _texture;
            set => _texture = value;
        }
        public int AnimationFrame { get => _animationFrame; set => _animationFrame = value; }
    }
}