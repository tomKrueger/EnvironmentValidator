using System;
using System.Collections.Generic;
using System.Text;

namespace EnvironmentValidator.Models.Commands
{
    public class CommandResult
    {
        public CommandResult(Command command)
        {
            Command = command;
        }

        public Command Command { get; set; }

        public ResultStatus Status { get; set; }    

        public Exception Exception { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public long ElapsedMilliseconds { get; set; }
    }

    public enum ResultStatus
    {
        Unknown,
        Success,
        Error
    }
}
