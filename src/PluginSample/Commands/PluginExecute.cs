using PluginSample.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PluginSample.Commands
{
    public class PluginExecute
    {
        public string PackageId { get; internal set; }

        public void Execute()
        {
            Program.AssemblyLoaderContainer.AddLoader(new PluginLoader(PackageId));

            var pluginAssembly = Assembly.Load(new AssemblyName(PackageId));

            foreach (var type in pluginAssembly.GetExportedTypes())
            {
                var x = 5;
            }
        }
    }
}
