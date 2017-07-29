using EnvironmentValidator.Models.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnvironmentValidator.Models
{
    public class Manifest
    {
        public Manifest()
        {
            Commands = new List<Command>();
        }

        public List<Command> Commands { get; set; }
    }
}
