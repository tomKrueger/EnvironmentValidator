using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentValidator.Models.Commands
{
    public abstract class Command
    {
        public Command(string commandType)
        {
            CommandType = commandType;
        }

        public abstract Task<CommandResult> ExecuteAsync();

        public string CommandType { get; private set; }

    }
}
