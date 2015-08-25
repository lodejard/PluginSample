using Microsoft.Framework.Runtime;

namespace PluginSample.Engine
{
    public static class Services
    {
        public static IApplicationEnvironment ApplicationEnvironment { get; set; }
        public static IAssemblyLoaderContainer AssemblyLoaderContainer { get; set; }
        public static IAssemblyLoadContextAccessor AssemblyLoadContextAccessor { get; set; }
        public static ILibraryManager LibraryManager { get; set; }

    }
}
