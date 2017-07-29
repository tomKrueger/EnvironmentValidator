using EnvironmentValidator.Models;
using EnvironmentValidator.Models.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnvironmentValidator.DataAccess
{
    public class Repository
    {
        public Manifest GetManifest(string file, string releaseLevel)
        {
            // TODO: Read manifest from file and replace hard coded commands.

            var m = new Manifest();

            m.Commands.Add(new HttpCommand() { Url="http://www.google.com"});

            return m;
        }
    }
}
