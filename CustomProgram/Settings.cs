using SplashKitSDK;
namespace CustomProgram
{
    public static class Settings
    {
        public const float RenderScale = 2.0f;
        public const int TileSize = 32;
        public static int MoveFrames = 20;
        // formula: 1 / MoveFrames
        public static float PlayerMoveSpeed = 1.0f / (float)MoveFrames;
    }
}