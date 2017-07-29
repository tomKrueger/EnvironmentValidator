using EnvironmentValidator.Common;
using EnvironmentValidator.Models;
using EnvironmentValidator.Models.ManifestSchema;

namespace EnvironmentValidator.DataAccess
{
    public class Repository
    {
        public Manifest GetManifest(string filePath, string releaseLevel)
        {
            var manifestSchema = XmlSerializerHelper.DeserializeFromFile<ManifestSchema>(filePath);

            var m = new Manifest();

            foreach (var t in manifestSchema.Tests)
            {
                m.Commands.Add(t.GetCommand());
            }

            return m;
        }
    }
}
