using System;

namespace FFramework.Serialization
{
    [SerializeProvider(typeof(String))]
    internal class StringProvider : ProviderInstance<StringProvider>
    {
        public override unsafe void Deserialize<K>(void* source, void* address)
        {
            
        }

        public override unsafe void Serialize<K>(void* destination, void* address)
        {
            
        }
    }
}
