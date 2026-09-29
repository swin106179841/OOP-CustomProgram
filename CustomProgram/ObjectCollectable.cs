using SplashKitSDK;

namespace CustomProgram
{
    public class ObjectCollectable: GameObject
    {
        
        public ObjectCollectable() : this(0, 0, 0, SplashKit.LoadBitmap("fallback", "./assets/fallback.png")) {}
        public ObjectCollectable(int x, int y, int spriteIndex, Bitmap texture): base(x, y, spriteIndex, texture) {}
    }
}