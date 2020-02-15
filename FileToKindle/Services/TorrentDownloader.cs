using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using FileToKindle.Helpers;
using FileToKindle.Model;

namespace FileToKindle.Services
{
    public class TorrentDownloader
    {
        static string aria2cPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dependencies/aria2c");

        public string DownloadMagnetLink(MagnetLink magnetLink)
        {
            return DownloadMagnetLink(magnetLink, CancellationToken.None);
        }

        public string DownloadMagnetLink(MagnetLink magnetLink, CancellationToken cancellationToken)
        {
            var downloadMagnetLinkArguments = string.Concat(aria2cPath, " --dir='", magnetLink.DownloadDirectory, "' '", magnetLink.Link, "' --seed-time=0");
            using (var process = ProcessStartInfoHelper.Create(downloadMagnetLinkArguments, cancellationToken))
            {
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception("No se pudo descargar el archivo");
                }
            }

            return Directory.GetFiles(magnetLink.DownloadDirectory)[0];
        }
    }
}
