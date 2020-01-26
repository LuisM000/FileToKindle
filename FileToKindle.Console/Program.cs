using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CommandLine;
using FileToKindle.Model;
using FileToKindle.Services;
using FileToKindle.Console.Options;
using System.Threading.Tasks;

namespace FileToKindle.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<ProcessMagnetLinksOptions, CleanOptions>(args).
                MapResult(
                    (CleanOptions opts) => Process(opts),
                    (ProcessMagnetLinksOptions opts) => Process(opts),
                    errs => 1);
        }


        private static int Process(CleanOptions options)
        {
            var temporalDirectories = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory, "_temp_*");
            foreach (var temporalDirectory in temporalDirectories)
            {
                Directory.Delete(temporalDirectory, true);
            }
            return 0;
        }

        private static int Process(ProcessMagnetLinksOptions options)
        {
            try
            {
                List<string> mobiPaths = new List<string>();
                Parallel.ForEach(options.MagnetLinks, (o) =>
                {
                    mobiPaths.Add(GetMobiFile(o));
                });

                var emailSender = new EmailSender();
                var email = CreateEmail(mobiPaths, options.DestinationEmail);

                emailSender.Send(email);

                return 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return 1;
            }
        }

        private static string GetMobiFile(string magnetUrl)
        {
            var torrentDownloader = new TorrentDownloader();
            var mobiConverter = new MobiConverter();

            var downloadDirectory = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_temp_" + Guid.NewGuid().ToString()));
            var magnetLink = new MagnetLink(magnetUrl, downloadDirectory.FullName);
            var filePath = torrentDownloader.DownloadMagnetLink(magnetLink);
            var mobiPath = mobiConverter.ConvertToMobi(filePath);

            return mobiPath;
        }

        private static Email CreateEmail(IEnumerable<string> mobiPaths, string kindleEmail)
        {
            var jsonString = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "email.json"));
            var email = JsonSerializer.Deserialize<Email>(jsonString);
            email.To = kindleEmail;
            email.AttachmentPaths = mobiPaths;
            return email;
        }
    }
}
