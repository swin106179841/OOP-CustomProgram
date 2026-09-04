using System.ComponentModel;
using SplashKitSDK;
namespace CustomProgram
{
    public class Texture
    {
        private Bitmap _bmp;
        private String _name;
        public Texture(String name, String path)
        {
            _name = name;
            _bmp = SplashKit.LoadBitmap(name, path);
        }
        public void Draw(double x, double y)
        {
            SplashKit.DrawBitmap(_bmp, x, y);
        }
    }
}