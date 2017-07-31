using EnvironmentValidator.Models.Commands;
using System;
using System.Xml.Serialization;

namespace EnvironmentValidator.Models.ManifestSchema.Tests
{
    public class FileVersionTest : ManifestTest
    {
        public override Command GetCommand()
        {
            var cmd = new FileVersionCommand();
            cmd.FilePath = FilePath;
            cmd.ExpectedVersion = ExpectedVersion;

            return cmd;
        }

        [XmlAttribute()]
        public string FilePath { get; set; }

        [XmlAttribute()]
        public string ExpectedVersion { get; set; }
    }
}
