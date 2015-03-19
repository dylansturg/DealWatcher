using System;
using System.CodeDom;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DealWatcher.ConfigurationManagement
{
    static class ConfigurationValueFactory
    {


        internal static Object ConstructValue(byte[] valueBytes)
        {
            var deserialized = DeserializeValue(valueBytes);
            return deserialized;

        }

        internal static Object ConstructValue(byte[] valueBytes, String valueType)
        {
            return default(Object);
        }

        internal static byte[] SerializeConfigBytes<T>(T value)
        {
            var formatter = new BinaryFormatter();
            using (var bytes = new MemoryStream())
            {
                formatter.Serialize(bytes, value);
                return bytes.ToArray();
            }
        }

        private static Object DeserializeValue(byte[] valueBytes)
        {
            var binaryFormatter = new BinaryFormatter();
            Object result = null;
            using (var bytes = new MemoryStream(valueBytes))
            {
                result = binaryFormatter.Deserialize(bytes);
            }
            return result;
        }
    }
}