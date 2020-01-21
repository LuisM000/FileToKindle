using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CommandLine;
using FileToKindle.Model;
using FileToKindle.Services;

namespace FileToKindle.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ProcessOneLinkOptions, CleanOptions>(args)
                .WithParsed<CleanOptions>(Process)
                .WithParsed<ProcessOneLinkOptions>(Process);       
        }


        private static void Process(CleanOptions options)
        {
            var temporalDirectories = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory, "_temp_*");
            foreach (var temporalDirectory in temporalDirectories)
            {
                Directory.Delete(temporalDirectory, true);
            }
        }

        private static void Process(ProcessOneLinkOptions options)
        {
            var torrentDownloader = new TorrentDownloader();
            var mobiConverter = new MobiConverter();
            var emailSender = new EmailSender();
            var downloadDirectory = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_temp_" + Guid.NewGuid().ToString()));

            try
            {
                var magnetLink = new MagnetLink(options.MagnetLink, downloadDirectory.FullName);
                var filePath = torrentDownloader.DownloadMagnetLink(magnetLink);
                var mobiPath = mobiConverter.ConvertToMobi(filePath);
                var email = CreateEmail(mobiPath, options.DestinationEmail);

                emailSender.Send(email);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        private static Email CreateEmail(string mobiPath, string kindleEmail)
        {
            var jsonString = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "email.json"));
            var email = JsonSerializer.Deserialize<Email>(jsonString);
            email.To = kindleEmail;
            email.AttachmentPaths = new List<string>()
            {
                mobiPath
            };
            return email;
        }
    }
}
