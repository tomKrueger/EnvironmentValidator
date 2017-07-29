using EnvironmentValidator.Models.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnvironmentValidator
{
    class LogManager
    {
        public void Log(CommandResult result)
        {
            var exceptionMsg = "";
            if (result.Exception !=null && result.Exception.Message != null)
            {
                exceptionMsg = result.Exception.Message;
            }

            string line = string.Format($"{DateTime.UtcNow}|{result.Command.CommandType}|{result.Status}|{exceptionMsg}");
            System.Diagnostics.Debug.WriteLine(line);
        }
    }
}
