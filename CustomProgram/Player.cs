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
        public void Draw()
        {
            throw new NotImplementedException();
        }
        public void Move()
        {
            switch (Direction)
            {
                case PlayerMoveDirection.Up:
                    break;
                case PlayerMoveDirection.Down:
                    break;
                case PlayerMoveDirection.Left:
                    break;
                case PlayerMoveDirection.Right:
                    break;
                default:
                    break;
            }
        }
        public void SetLevel(Level level) { _level = level; }
        public float X { get => _x; set => _x = value; }
        public float Y { get => _y; set => _y = value; }
        public PlayerMoveDirection Direction { get => _direction; set => _direction = value; }
    }
}