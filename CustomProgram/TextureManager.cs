using SplashKitSDK;
namespace CustomProgram
{
    public class TextureManager
    {
        private List<Bitmap> _textures = [];
        private Bitmap _fallback;
        public TextureManager()
        {
            _fallback = SplashKit.LoadBitmap("fallback", "./assets/fallback.png");
        }
        public Bitmap RequestTexture(int index)
        {
            if (index < _textures.Count() && index >= 0)
            {
                return _textures[index];
            }
            else
            {
                return Fallback;
            }
        }
        public void AddTexture(Bitmap texture)
        {
            _textures.Add(texture);
        }
        public Bitmap Fallback
        {
            get => _fallback;
        }
        public void RemoveAllTextures()
        {
            _textures.Clear();
        }
        public int Count => _textures.Count();
    }
}