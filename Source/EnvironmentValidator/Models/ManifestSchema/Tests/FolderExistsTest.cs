using EnvironmentValidator.Models.Commands;
using System;
using System.Xml.Serialization;

namespace EnvironmentValidator.Models.ManifestSchema.Tests
{
    public class FolderExistsTest : ManifestTest
    {
        public override Command GetCommand()
        {
            var cmd = new FolderExistsCommand();
            cmd.FolderPath = FolderPath;

            return cmd;
        }

        [XmlAttribute()]
        public string FolderPath { get; set; }
    }
}
