using SplashKitSDK;
namespace CustomProgram
{
    public static class Settings
    {
        public static float RenderScale = 2.0f;
        public static int TileSize = 32;
        public static int MoveFrames = 20;
        // formula: 1 / MoveFrames
        public static float MoveSpeed => 1.0f / (float)MoveFrames;
        public static int AnimationTimePerFrame = 8;
        public static bool ShadowsEnabled = true;
    }
}