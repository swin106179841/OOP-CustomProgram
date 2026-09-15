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
        public ObjectInteractable(): this(0, 0, 0) {}
        public ObjectInteractable(int x, int y, int textureID): base(x, y, textureID)
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