﻿using EnvironmentValidator.Common;
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
            manifestSchemasForRelease.Reverse();
            
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
                manifestsToReturn.Add(manifest);
            }

            return manifestsToReturn;
        }
    }
}
