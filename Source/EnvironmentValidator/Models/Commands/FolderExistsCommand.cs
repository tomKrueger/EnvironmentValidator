using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentValidator.Models.Commands
{
    public class FolderExistsCommand : Command
    {
        public FolderExistsCommand()
            :base("FolderExists")
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
                    if (string.IsNullOrWhiteSpace(FolderPath)) { throw new ArgumentException("FolderPath not specified.", "FolderPath"); }

                    if (!Directory.Exists(FolderPath))
                    {
                        throw new Exception($"Folder does not exist. Folder Expected:{FolderPath}");
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

        public string FolderPath { get; set; }

    }
}
