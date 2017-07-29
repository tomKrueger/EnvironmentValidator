using EnvironmentValidator.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentValidator
{
    public class ValidationManager
    {
        public async Task Process(string file, string releaseLevel)
        {
            var logMgr = new LogManager();

            var repo = new Repository();
            var manifest = repo.GetManifest(file, releaseLevel);

            foreach(var cmd in manifest.Commands)
            {
                var result = await cmd.ExecuteAsync();
                logMgr.Log(result);    
            }
        }
    }
}
