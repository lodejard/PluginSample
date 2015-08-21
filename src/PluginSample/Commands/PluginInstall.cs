using Newtonsoft.Json.Linq;
using PluginSample.Workers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSample.Commands
{
    public class PluginInstall
    {
        public string PackageId { get; set; }

        public string PackageVersion { get; set; }

        public void Execute()
        {
            var dnxPath = Process.GetCurrentProcess().Modules[0].FileName;
            var dnxFolder = Path.GetDirectoryName(dnxPath);
            var toolingPath = Path.Combine(
                dnxFolder,
                "lib",
                "Microsoft.Dnx.Tooling",
                "Microsoft.Dnx.Tooling.dll");

            if (!File.Exists(toolingPath))
            {
                toolingPath = Path.Combine(
                    dnxFolder,
                    "lib",
                    "Microsoft.Framework.PackageManager",
                    "Microsoft.Framework.PackageManager.dll");
            }

            var childProcess = new ChildProcess();
            var results = childProcess.Execute(dnxPath, $@"""{toolingPath}"" commands install {PackageId} {PackageVersion}");

            const string writingLockFile = "Writing lock file ";
            var lockFiles = results.Item2
                .Split("\r\n".ToArray())
                .Where(x => x.StartsWith(writingLockFile))
                .Select(x => x.Substring(writingLockFile.Length))
                .Where(File.Exists);

            var runtimeAssemblies = new List<string>();
            foreach (var lockFile in lockFiles)
            {
                var root = JToken.Parse(File.ReadAllText(lockFile));
                var targets = root["targets"];
                var target = targets[Program.ApplicationEnvironment.RuntimeFramework.ToString()];
                foreach (var package in target.OfType<JProperty>())
                {
                    var packageRuntime = package.Value["runtime"];
                    if (packageRuntime != null)
                    {
                        foreach (var assembly in packageRuntime.OfType<JProperty>())
                        {
                            runtimeAssemblies.Add(Path.Combine(package.Name, assembly.Name));
                        }
                    }
                }
            }
            File.WriteAllLines(
                $"Command.{PackageId}.txt",
                runtimeAssemblies.Select(x => $"{Path.GetFileNameWithoutExtension(x)}={x}"));
        }
    }
}
