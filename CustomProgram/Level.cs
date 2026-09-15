using ShapeDrawer;
using SplashKitSDK;
using System.IO;
namespace CustomProgram
{
    class Level
    {
        private int _levelWidth;
        private int _levelHeight;
        private List<int> _scene = [];
        private List<Object> _objects = [];
        private TextureManager _texMan = new TextureManager();
        public Level(String levelPath)
        {
            try
            {

                StreamReader sr = File.OpenText(levelPath);
                // magic number
                if (sr.ReadLine() != "LVLV1")
                {
                    Console.WriteLine("Error. Invalid File Type");
                    return;
                }
                int texCount = Convert.ToInt32(sr.ReadLine());
                for (int i = 0; i < texCount; i++)
                {
                    String name = String.Format("texture-{0}", i);
                    Texture tex = new Texture(name, sr.ReadLine());
                    _texMan.AddTexture(name, tex);
                }
                // keep going
                int objectCount = sr.ReadInteger();
                for (int i = 0; i < objectCount; i++)
                {
                    String objType = sr.ReadLine();
                    GameObject obj;
                    switch (objType) {
                        case "Object":
                            obj = new ObjectStatic();
                            break;
                        case "ObjectInteractable":
                            obj = new ObjectInteractable();
                            break;
                        default:
                            throw new InvalidDataException();
                    }
                    obj.Load(sr);
                }
            } catch (Exception e)
            {
                Console.Write("Error Occured in level: {0}", levelPath);
            }
        }
    }
}