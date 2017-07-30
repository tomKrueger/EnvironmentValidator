using EnvironmentValidator.Models.Commands;
using System;
using System.Xml.Serialization;

namespace EnvironmentValidator.Models.ManifestSchema.Tests
{
    public class HttpGetTest : ManifestTest
    {
        public override Command GetCommand()
        {
            var cmd = new HttpCommand();
            cmd.Url = Url;

            if (ExpectedResponseCode != null)
            {
                int expectedResponseCode;
                if (!int.TryParse(ExpectedResponseCode, out expectedResponseCode))
                {
                    throw new Exception($"Invalid 'ExpectedResponseCode' in manifest.  Expecting integer.  Actual: {ExpectedResponseCode}");
                }

                cmd.ExpectedResponseCode = expectedResponseCode;
            }



            return cmd;
        }

        [XmlAttribute()]
        public string Url { get; set; }

        [XmlAttribute()]
        public string ExpectedResponseCode { get; set; }
    }
}
