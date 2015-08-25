using System.Linq;
using Microsoft.Framework.Runtime;
using System.Diagnostics;
using System;
using PluginSample.Engine;
using PluginSample.Engine.Commands;

namespace PluginSample
{
    public class Program
    {
        public Program(
            IApplicationEnvironment applicationEnvironment,
            IAssemblyLoaderContainer assemblyLoaderContainer,
            IAssemblyLoadContextAccessor assemblyLoadContextAccessor,
            ILibraryManager libraryManager)
        {
            Services.ApplicationEnvironment = applicationEnvironment;
            Services.AssemblyLoaderContainer = assemblyLoaderContainer;
            Services.AssemblyLoadContextAccessor = assemblyLoadContextAccessor;
            Services.LibraryManager = libraryManager;
        }

        public void Main(string[] args)
        {
            var pluginName = args.FirstOrDefault();
            var pluginVersion = args.Skip(1).FirstOrDefault();

            // Install command plugin from nuget feeds
            var pluginInstall = new PluginInstall
            {
                PackageId = pluginName,
                PackageVersion = pluginVersion
            };
            //pluginInstall.Execute();

            var sw = Stopwatch.StartNew();
            // Load plugin from installed assemblies
            var pluginExecute = new PluginExecute
            {
                PackageId = pluginName
            };
            pluginExecute.Execute();
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}ms");
            Console.ReadLine();
        }
    }
}
