using EnvironmentValidator.Models.ManifestSchema.Tests;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EnvironmentValidator.Models.ManifestSchema
{
    [XmlRoot(elementName: "Manifests")]
    public class ManifestSchemas : List<ManifestSchema>
    {
    }

    [XmlType("Manifest")]
    public class ManifestSchema
    {
        public ManifestSchema()
        {
        }

        [XmlAttribute()]
        public string ReleaseLevel { get; set; }

        [XmlArrayItem(typeof(HttpGetTest), ElementName ="HttpGet")]
        [XmlArrayItem(typeof(FileExistsTest), ElementName = "FileExists")]
        [XmlArrayItem(typeof(FileVersionTest), ElementName = "FileVersion")]
        [XmlArrayItem(typeof(FolderExistsTest), ElementName = "FolderExists")]        
        public List<ManifestTest> Tests { get; set; }
    }
}
