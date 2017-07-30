using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentValidator.Models.Commands
{
    public class FileExistsCommand : Command
    {
        public FileExistsCommand()
            :base("FileExists")
        {
        }

        public override Task<CommandResult> ExecuteAsync()
        {
            // Use Task.Run so that a Task is returned since
            // the code to check for existence is not Aysnc.
            return Task.Run<CommandResult>(() =>
            {
                var result = new CommandResult(this);

                try
                {
                    if (string.IsNullOrWhiteSpace(FilePath)) { throw new ArgumentException("FilePath not specified.", "FilePath"); }

                    if (!File.Exists(FilePath))
                    {
                        throw new Exception($"File does not exist. File Expected:{FilePath}");
                    }

                    result.Status = ResultStatus.Success;
                }
                catch (Exception ex)
                {
                    result.Status = ResultStatus.Error;
                    result.Exception = ex;
                }

                return result;
            });

        }

        public string FilePath { get; set; }

    }
}
