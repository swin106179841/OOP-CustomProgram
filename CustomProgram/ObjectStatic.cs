using SplashKitSDK;

namespace CustomProgram
{
    public class ObjectStatic: GameObject
    {
        
        public ObjectStatic() : this(0, 0, 0) {}
        public ObjectStatic(int x, int y, int textureID): base(x, y, textureID) {}
    }
}