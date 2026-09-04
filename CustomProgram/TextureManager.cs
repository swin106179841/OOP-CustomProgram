using SplashKitSDK;
namespace CustomProgram
{
    public class TextureManager
    {
        private List<Texture> _textures = [];
        public TextureManager()
        {
        }
    }
    public class Texture
    {
        private Bitmap _bmp;
        private String _name;
        private int _width;
        private int _height;
        public Texture(String name, String path, int width, int height)
        {
            _name = name;
            _bmp = SplashKit.CreateBitmap(path, width, height);
            _width = width;
            _height = height;
        }
    }
}