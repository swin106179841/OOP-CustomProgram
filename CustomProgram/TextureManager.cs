using SplashKitSDK;
namespace CustomProgram
{
    public class TextureManager
    {
        private List<Texture> _textures = [];
        private List<String> _names = [];
        private Texture _fallback;
        public TextureManager()
        {
            _fallback = new Texture("fallback", "assets/fallback.png");
        }
        public int QueryName(String name)
        {
            for (int i = 0; i < _names.Count; i++)
            {
                if (name == _names[i])
                {
                    return i;
                }
            }
            return -1;
        }
        public Texture RequestTexture(String name)
        {
            int status = QueryName(name);
            if (status == -1)
            {
                // texture not found in list; add
                Console.WriteLine(String.Format("Error. Texture {0} not found", name));
                return _fallback;
            } else
                return _textures[status];
        }
        public Texture RequestTexture(int index)
        {
            if (index < _textures.Count())
            {
                Console.WriteLine(String.Format("Error. Texture {0} not found", index));
                return _textures[index];
            } else
            {
                return _fallback;
            }
        }
        public void AddTexture(String name, Texture texture)
        {
            _names.Add(name);
            _textures.Add(texture);
        }
        public void RemoveTexure(String name)
        {
            int index = QueryName(name);
            if (index != -1)
            {
                _names.Remove(_names[index]);
                _textures.Remove(_textures[index]);
            }
        }
        public Texture Fallback
        {
            get => _fallback;
        }
        public void LoadTileset(String path)
        {
            
        }
    }
}