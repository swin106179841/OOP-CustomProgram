using System.Runtime.InteropServices;
using ShapeDrawer;
using SplashKitSDK;
namespace CustomProgram
{
    public abstract class GameObject
    {
        private int _x;
        private int _y;
        // what texture in spritesheet
        private int _spriteIndex;
        // what texture from texture manager
        private int _textureIndex;
        private int _heightOffset;
        public DrawingOptions DOpts;
        public GameObject(): this(0, 0, 0) {}
        public GameObject(int x, int y, int spriteIndex)
        {
            X = x;
            Y = y;
            SpriteIndex = spriteIndex;
            DOpts = SplashKit.OptionWithBitmapCell(spriteIndex);
            DOpts.ScaleX = Settings.RenderScale;
            DOpts.ScaleY = Settings.RenderScale;
        }

        public virtual void Draw(Bitmap texture)
        {
            SplashKit.DrawBitmap(texture, X * 32 * Settings.RenderScale, (Y * 32 * Settings.RenderScale) - (HeightOffset * Settings.RenderScale), DOpts);
        }
        public virtual void Load(StreamReader sr)
        {
            X = sr.ReadInteger();
            Y = sr.ReadInteger();
            SpriteIndex = sr.ReadInteger();
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
        public int SpriteIndex
        {
            get => _spriteIndex;
            set => _spriteIndex = value;
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