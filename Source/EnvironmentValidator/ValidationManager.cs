using EnvironmentValidator.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            var sw = new Stopwatch();

            foreach (var cmd in manifest.Commands)
            {
                var startTime = DateTime.UtcNow;
                sw.Restart();

                var result = await cmd.ExecuteAsync();

                sw.Stop();
                var endTime = DateTime.UtcNow;

                result.StartTime = startTime;
                result.EndTime = endTime;
                result.ElapsedMilliseconds = sw.ElapsedMilliseconds;

                logMgr.Log(result);    
            }
        }
    }
}
