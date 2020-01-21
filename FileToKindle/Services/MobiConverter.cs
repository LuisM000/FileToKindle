using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FileToKindle.Helpers;

namespace FileToKindle.Services
{
    public class MobiConverter
    {
        static string kindlegenPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dependencies/kindlegen");

        public string ConvertToMobi(string filePath)
        {
            var generateMobiArguments = string.Concat(kindlegenPath, " '", filePath, "'");
            using (var process = Process.Start(ProcessStartInfoHelper.CreateWithArguments(generateMobiArguments)))
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception("No se pudo generar el archivo .mobi");
                }
            }
            
            return Directory.GetFiles(Path.GetDirectoryName(filePath)).First(f => Path.GetExtension(f) == ".mobi");
        }
    }
}
