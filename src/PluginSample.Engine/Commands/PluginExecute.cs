using PluginSample.Engine.Workers;
using System;
using System.Reflection;

namespace PluginSample.Engine.Commands
{
    public class PluginExecute
    {
        public string PackageId { get; set; }

        public void Execute()
        {
            try
            {
                Services.AssemblyLoaderContainer.AddLoader(new PluginLoader(PackageId));

                var pluginAssembly = Assembly.Load(new AssemblyName(PackageId));

                foreach (var type in pluginAssembly.GetExportedTypes())
                {
                    var x = 5;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
