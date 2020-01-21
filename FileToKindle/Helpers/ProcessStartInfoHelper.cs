using System;
using System.Diagnostics;

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
    }
}
