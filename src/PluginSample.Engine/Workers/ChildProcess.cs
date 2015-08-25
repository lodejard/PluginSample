using System;
using System.Diagnostics;
using System.Text;

namespace PluginSample.Engine.Workers
{
    public class ChildProcess
    {
        public Tuple<int,string> Execute(string fileName, string arguments)
        {
            var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            var sb = new StringBuilder();
            process.OutputDataReceived += (sender, e) =>
            {
                sb.AppendLine(e.Data);
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                sb.AppendLine(e.Data);
            };
            process.Exited += (sender, e) =>
            {
            };
            process.EnableRaisingEvents = true;
            process.Start();
            process.StandardInput.Dispose();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            return Tuple.Create(process.ExitCode, sb.ToString());
        }
    }
}
