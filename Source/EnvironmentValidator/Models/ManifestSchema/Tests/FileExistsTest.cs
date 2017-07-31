using EnvironmentValidator.Models.Commands;
using System;
using System.Xml.Serialization;

namespace EnvironmentValidator.Models.ManifestSchema.Tests
{
    public class FileExistsTest : ManifestTest
    {
        public override Command GetCommand()
        {
            var cmd = new FileExistsCommand();
            cmd.FilePath = FilePath;

            return cmd;
        }

        [XmlAttribute()]
        public string FilePath { get; set; }
    }
}
