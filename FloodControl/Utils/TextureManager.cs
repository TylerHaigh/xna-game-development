using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodControl.Utils
{
    class TextureManager
    {

        public ContentManager Content { get; set; }
        public TextureManager(ContentManager content) { this.Content = content; }

        private Dictionary<string, object> _loadedContent = new Dictionary<string, object>();


        public T OptionalLoadContent<T>(string path)
        {
            if (!HasLoadedContentWithKey(path))
                return LoadContent<T>(path);

            return Get<T>(path);
        }

        public T OptionalLoadContent<T>(string key, string path)
        {
            if (!HasLoadedContentWithKey(key))
                return LoadContent<T>(key, path);

            return Get<T>(key);
        }


        public T LoadContent<T>(string path)
        {
            return LoadContent<T>(path, path);
        }

        public T LoadContent<T>(string key, string path)
        {
            if (_loadedContent.ContainsKey(key))
                throw new ArgumentException("key '" + key + "' already registered with loaded content", nameof(key));

            T cont = Content.Load<T>(path);
            _loadedContent[key] = cont;
            return cont;
        }

        public T Get<T>(string key)
        {
            if (!_loadedContent.ContainsKey(key))
                throw new ArgumentException("key '"+ key +"' not registered", nameof(key));

            object cont = _loadedContent[key];

            return (T)cont;
        }

        public bool HasLoadedContentWithKey(string key) { return _loadedContent.ContainsKey(key); }

    }
}
