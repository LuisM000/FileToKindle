using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using FileToKindle.Helpers;

namespace FileToKindle.Services
{
    public class MobiConverter
    {
        static string kindlegenPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dependencies/kindlegen");

        public string ConvertToMobi(string filePath)
        {
            return ConvertToMobi(filePath, CancellationToken.None);
        }


        public string ConvertToMobi(string filePath, CancellationToken cancellationToken)
        {
            var generateMobiArguments = string.Concat(kindlegenPath, " '", filePath, "'");
            using (var process = ProcessStartInfoHelper.Create(generateMobiArguments, cancellationToken))
            {
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0 && process.ExitCode != 1)
                {
                    throw new Exception("No se pudo generar el archivo .mobi");
                }
            }

            return Directory.GetFiles(Path.GetDirectoryName(filePath)).First(f => Path.GetExtension(f) == ".mobi");
        }
    }
}
