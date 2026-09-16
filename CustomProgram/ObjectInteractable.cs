using SplashKitSDK;
namespace CustomProgram
{
    public enum ObjectMoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    class ObjectInteractable: GameObject
    {
        private int _uid;
        public ObjectInteractable(): this(0, 0, 0, SplashKit.LoadBitmap("fallback", "./assets/fallback.png")) {}
        public ObjectInteractable(int x, int y, int textureID, Bitmap texture): base(x, y, textureID, texture)
        {
            _uid = ObjectIDs.NewID();
        }
        public virtual void Move(ObjectMoveDirection dir)
        {
            switch(dir)
            {
                case ObjectMoveDirection.Up:
                    Y -= 1;
                break;
                case ObjectMoveDirection.Down:
                    Y += 1;
                break;
                case ObjectMoveDirection.Left:
                    X -= 1;
                break;
                case ObjectMoveDirection.Right:
                    X += 1;
                break;
            }
        }
    }
}