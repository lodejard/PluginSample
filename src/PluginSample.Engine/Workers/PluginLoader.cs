using Microsoft.Framework.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System;

namespace PluginSample.Engine.Workers
{
    public class PluginLoader : IAssemblyLoader
    {
        private readonly Dictionary<string, string> _pluginFiles;

        public PluginLoader(string packageId)
        {
            var pluginSamplePath = Services.LibraryManager.GetLibraryInformation("PluginSample").Path;
            var packagesPath = Path.GetDirectoryName(Path.GetDirectoryName(pluginSamplePath));

            packagesPath = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), ".dnx", "bin", "packages");

            _pluginFiles = File.ReadAllLines($"Command.{packageId}.txt")
                .Select(x => x.Split("=".ToArray(), 2))
                .Select(parts => new { Name = parts[0], Path = Path.Combine(packagesPath, parts[1]) })
                .ToDictionary(kv => kv.Name, kv => kv.Path);
        }

        public Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath;
            if (!_pluginFiles.TryGetValue(assemblyName.Name, out assemblyPath))
            {
                return null;
            }

            return Services.AssemblyLoadContextAccessor.Default.LoadFile(assemblyPath);
        }
    }
}
