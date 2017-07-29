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

            var manifestFilePath = "SampleManifest.xml";
            var releaseLevel = "";

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