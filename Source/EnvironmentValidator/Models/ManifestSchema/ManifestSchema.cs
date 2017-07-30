using EnvironmentValidator.Models.ManifestSchema.Tests;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EnvironmentValidator.Models.ManifestSchema
{
    [XmlRoot(elementName:"Manifest")]
    public class ManifestSchema
    {
        public ManifestSchema()
        {
        }

        [XmlArray()]
        [XmlArrayItem(typeof(HttpGetTest), ElementName ="HttpGet")]
        [XmlArrayItem(typeof(FileExistsTest), ElementName = "FileExists")]
        public List<ManifestTest> Tests { get; set; }
    }
}
