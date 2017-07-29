using EnvironmentValidator.Models.Commands;
using System.Xml.Serialization;

namespace EnvironmentValidator.Models.ManifestSchema.Tests
{
    public class HttpGetTest : ManifestTest
    {
        public override Command GetCommand()
        {
            var cmd = new HttpCommand();
            cmd.Url = Url;
            
            // TODO: pass expectedresponsecode.

            return cmd;
        }

        [XmlAttribute()]
        public string Url { get; set; }

        [XmlAttribute()]
        public string ExpectedResponseCode { get; set; }
    }
}
