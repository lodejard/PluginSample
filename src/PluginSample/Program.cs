using PluginSample.Commands;
using System.Linq;
using Microsoft.Framework.Runtime;
using PluginSample.Workers;
using System.Reflection;

namespace PluginSample
{
    public class Program
    {
        public static IApplicationEnvironment ApplicationEnvironment { get; private set; }
        public static IAssemblyLoaderContainer AssemblyLoaderContainer { get; private set; }
        public static IAssemblyLoadContextAccessor AssemblyLoadContextAccessor { get; private set; }
        public static ILibraryManager LibraryManager { get; private set; }

        public Program(
            IApplicationEnvironment applicationEnvironment,
            IAssemblyLoaderContainer assemblyLoaderContainer,
            IAssemblyLoadContextAccessor assemblyLoadContextAccessor,
            ILibraryManager libraryManager)
        {
            ApplicationEnvironment = applicationEnvironment;
            AssemblyLoaderContainer = assemblyLoaderContainer;
            AssemblyLoadContextAccessor = assemblyLoadContextAccessor;
            LibraryManager = libraryManager;
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
            pluginInstall.Execute();

            // Load plugin from installed assemblies
            var pluginExecute = new PluginExecute
            {
                PackageId = pluginName
            };
            pluginExecute.Execute();

        }
    }
}
