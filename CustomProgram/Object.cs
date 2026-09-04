using SplashKitSDK;

namespace CustomProgram
{
    public class Object
    {
        private int _x;
        private int _y;
        private int _textureID;
        private int _heightOffset;
        private DrawingOptions _opts;
        public Object() : this(0, 0, 0)
        {

        }
        public Object(int x, int y, int textureID)
        {
            _x = x;
            _y = y;
            _textureID = textureID;
            _opts = SplashKit.OptionWithBitmapCell(textureID);
            _opts.ScaleX = Settings.RenderScale;
            _opts.ScaleY = Settings.RenderScale;
        }
        public virtual void Draw(Bitmap spriteSheet)
        {
            SplashKit.DrawBitmap(spriteSheet, _x * 32 * Settings.RenderScale, (_y * 32 * Settings.RenderScale) - (_heightOffset * Settings.RenderScale), _opts);
        }
        public int X
        {
            get => _x;
            set => _x = value;
        }
        public int Y
        {
            get => _y;
            set => _y = value;
        }
        public int TextureID
        {
            get => _textureID;
            set => _textureID = value;
        }
        public int HeightOffset
        {
            get => _heightOffset;
            set => _heightOffset = value;
        }
    }
}