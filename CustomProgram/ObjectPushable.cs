using SplashKitSDK;
namespace CustomProgram
{
    class ObjectPushable: GameObject
    {
        private int _uid;
        private MoveDirection _direction;
        private bool _isMoving;
        private int _frameCount = 0;
        public ObjectPushable(): this(0, 0, 0, SplashKit.LoadBitmap("fallback", "./assets/fallback.png")) {}
        public ObjectPushable(int x, int y, int textureID, Bitmap texture): base(x, y, textureID, texture)
        {
            _uid = ObjectIDs.NewID();
        }
        public virtual void Move(MoveDirection dir)
        {
            _direction = dir;
            _isMoving = true;
            // switch(dir)
            // {
            //     case MoveDirection.Up:
            //         Y -= 1;
            //     break;
            //     case MoveDirection.Down:
            //         Y += 1;
            //     break;
            //     case MoveDirection.Left:
            //         X -= 1;
            //     break;
            //     case MoveDirection.Right:
            //         X += 1;
            //     break;
            // }
        }
        public override void Update()
        {
            base.Update();
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
    }
}