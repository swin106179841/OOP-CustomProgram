using System.Runtime.InteropServices;
using ShapeDrawer;
using SplashKitSDK;
namespace CustomProgram
{
    public abstract class GameObject
    {
        private int _x;
        private int _y;
        private int _textureID;
        private int _textureIndex;
        private int _heightOffset;
        public DrawingOptions DOpts;
        public GameObject(): this(0, 0, 0) {}
        public GameObject(int x, int y, int textureID)
        {
            X = x;
            Y = y;
            TextureID = textureID;
            DOpts = SplashKit.OptionWithBitmapCell(textureID);
            DOpts.ScaleX = Settings.RenderScale;
            DOpts.ScaleY = Settings.RenderScale;
        }

        public virtual void Draw(Bitmap spriteSheet)
        {
            SplashKit.DrawBitmap(spriteSheet, X * 32 * Settings.RenderScale, (Y * 32 * Settings.RenderScale) - (HeightOffset * Settings.RenderScale), DOpts);
        }
        public virtual void Load(StreamReader sr)
        {
            X = sr.ReadInteger();
            Y = sr.ReadInteger();
            TextureID = sr.ReadInteger();
            TextureIndex = sr.ReadInteger();
            HeightOffset = sr.ReadInteger();
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
        public int TextureIndex
        {
            get => _textureIndex;
            set => _textureIndex = value;
        }
        public int HeightOffset
        {
            get => _heightOffset;
            set => _heightOffset = value;
        }
    }
}