using System.Runtime.InteropServices;
using ShapeDrawer;
using SplashKitSDK;
namespace CustomProgram
{
    public abstract class GameObject: IDrawable
    {
        private float _x;
        private float _y;
        // what texture in spritesheet
        private int _spriteIndex;
        // what texture from texture manager
        private int _textureIndex;
        private Bitmap _texture;
        private int _heightOffset;
        public DrawingOptions DOpts;
        public GameObject(): this(0, 0, 0, SplashKit.LoadBitmap("fallback", "./assets/fallback.png")) {}
        public GameObject(float x, float y, int spriteIndex, Bitmap texture)
        {
            X = x;
            Y = y;
            SpriteIndex = spriteIndex;
            DOpts = SplashKit.OptionWithBitmapCell(spriteIndex);
            DOpts.ScaleX = Settings.RenderScale;
            DOpts.ScaleY = Settings.RenderScale;
            _texture = texture;
        }
        public virtual void Draw()
        {
            SplashKit.DrawBitmap(_texture, X * 32 * Settings.RenderScale, (Y * 32 * Settings.RenderScale) - (HeightOffset * Settings.RenderScale), DOpts);
        }
        public virtual void Update()
        {
            
        }
        public virtual void Load(StreamReader sr)
        {
            X = sr.ReadInteger();
            Y = sr.ReadInteger();
            SpriteIndex = sr.ReadInteger();
            TextureIndex = sr.ReadInteger();
            HeightOffset = sr.ReadInteger();
            DOpts = SplashKit.OptionWithBitmapCell(SpriteIndex, DOpts);
        }
        public virtual bool Pushable()
        {
            return false;
        }
        public float X
        {
            get => _x;
            set => _x = value;
        }
        public float Y
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
        public Bitmap Texture
        {
            get => _texture;
            set => _texture = value;
        }
    }
}