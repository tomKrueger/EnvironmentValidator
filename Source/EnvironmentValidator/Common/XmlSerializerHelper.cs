using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace EnvironmentValidator.Common
{
    public class XmlSerializerHelper
    {
        public static T DeserializeFromFile<T>(string filePath)
        {
            using (var s = File.OpenRead(filePath))
            {
                return Deserialize<T>(s);
            }
        }

        public static T Deserialize<T>(Stream input)
        {
            var serializer = new XmlSerializer(typeof(T));
            if (serializer == null) { throw new Exception("XmlSerializer is null"); }

            return (T)serializer.Deserialize(input);
        }

        public static T Deserialize<T>(string input, Encoding encoding)
        {
            using (var ms = new MemoryStream(encoding.GetBytes(input)))
            {
                return Deserialize<T>(ms);
            }
        }

        public static T Deserialize<T>(string input)
        {
            return Deserialize<T>(input, Encoding.UTF8);
        }

        public static void Serialize<T>(Stream output, T objectGraph, XmlAttributeOverrides aor = null)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T), aor);
            ser.Serialize(output, objectGraph);

            //var serializer = new XmlSerializer(typeof(T));
            //serializer.Serialize(output, objectGraph);
        }

        public static string Serialize<T>(T objectGraph, XmlAttributeOverrides aor = null)
        {
            using (var memoryStream = new MemoryStream())
            {
                Serialize<T>(memoryStream, objectGraph, aor);
                memoryStream.Position = 0;

                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
