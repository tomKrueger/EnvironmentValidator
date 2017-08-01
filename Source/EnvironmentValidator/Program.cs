using System;

namespace EnvironmentValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Parse Args
            // Usage: 
            // EnvironmentValidator.exe PathToManifest -r ReleaseLevel
            // EnvironmentValidator.exe PathToManifest -s PathToEnvironmentSettingsFile

            // Rudimentary parsing of args.
            var manifestFilePath = (args.Length > 0) ? args[0] : null;
            var releaseLevel = (args.Length > 1) ? args[1] : null;

#if DEBUG
            // This is just here to make it easier to test while developing or if someone new
            // gets this code and tries to run it they will understand the values needed to be passed.
            if (args.Length  == 0)
            {
                manifestFilePath = "SampleManifest.xml";
                releaseLevel = "Third";
            }
#endif

            Console.WriteLine("*** Environment Validator ***");
            ValidationManager vm = new ValidationManager();
            vm.Process(manifestFilePath, releaseLevel).Wait();

#if DEBUG
            Console.WriteLine("Press any key to end.");
            Console.ReadLine();
#endif
        }
    }
}