using SplashKitSDK;

namespace CustomProgram
{
    public class ObjectStatic: GameObject
    {
        
        public ObjectStatic() : this(0, 0, 0, SplashKit.LoadBitmap("fallback", "./assets/fallback.png")) {}
        public ObjectStatic(int x, int y, int textureID, Bitmap texture): base(x, y, textureID, texture) {}
    }
}