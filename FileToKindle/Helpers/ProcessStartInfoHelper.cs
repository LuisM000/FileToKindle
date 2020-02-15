using System;
using System.Diagnostics;
using System.Threading;

namespace FileToKindle.Helpers
{
    public static class ProcessStartInfoHelper
    {
        public static ProcessStartInfo CreateWithArguments(string arguments)
        {
            return new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \" " + arguments + " \"",
                UseShellExecute = false,
                CreateNoWindow = true,
            };
        }

        public static Process Create(string arguments, CancellationToken cancellationToken)
        {
            var process = new Process()
            {
                StartInfo = CreateWithArguments(arguments)
            };
            cancellationToken.Register(() =>
            {
                try
                {
                    if (!process.HasExited)
                        process.Kill();
                }
                catch { }
            });
            return process;
        }
    }
}
