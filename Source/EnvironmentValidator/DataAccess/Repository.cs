using EnvironmentValidator.Common;
using EnvironmentValidator.Models;
using EnvironmentValidator.Models.ManifestSchema;
using System.Collections.Generic;
using System.Linq;

namespace EnvironmentValidator.DataAccess
{
    public class Repository
    {
        public Manifest GetManifest(string filePath, string releaseLevel)
        {
            var manifestSchemas = XmlSerializerHelper.DeserializeFromFile<ManifestSchemas>(filePath);

            var manifestSchemasForRelease = FindManifests(manifestSchemas, releaseLevel);
            
            var m = new Manifest();

            foreach (var ms in manifestSchemasForRelease)
            {
                foreach (var t in ms.Tests)
                {
                    m.Commands.Add(t.GetCommand());
                }
            }

            return m;
        }
        
        private List<ManifestSchema> FindManifests(List<ManifestSchema> manifestsToSearch, string releaseLevel)
        {
            var manifest = manifestsToSearch.Where(x => x.ReleaseLevel == releaseLevel).FirstOrDefault();

            var manifestsToReturn = new List<ManifestSchema>();

            if (manifest != null)
            {
                // Add BasedOn first, to manifestsToReturn, so that the BasedOn tests run first.
                if (manifest.BasedOn != null)
                {
                    var basedOnManifests = FindManifests(manifestsToSearch, manifest.BasedOn);
                    manifestsToReturn.AddRange(basedOnManifests);
                }

                manifestsToReturn.Add(manifest);
            }

            return manifestsToReturn;
        }
    }
}
