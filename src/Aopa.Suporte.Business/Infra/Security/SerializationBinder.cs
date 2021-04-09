using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Aopa.Suporte.Business.Infra.Security
{
    public class SerializationBinder : ISerializationBinder
    {
        private readonly Dictionary<string, Type> _mappedTypes = new Dictionary<string, Type>();

        public SerializationBinder(Type typeForSerialization)
        {
            if (!_mappedTypes.ContainsKey(typeForSerialization.Name))
                _mappedTypes.Add(typeForSerialization.Name, typeForSerialization);
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            if (_mappedTypes.TryGetValue(typeName, out Type result))
                return result;

            return null;
        }
    }
}
