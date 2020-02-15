using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FileToKindle.Services;
using FileToKindle.Shared.Services;
using MvvmHelpers;

namespace FileToKindle.Shared.Models
{
    public enum FileProcessStatus
    {
        None, Downloading, Converting, PendingSend, Sending, Error, Completed
    }

    public class FileProcess : ObservableObject
    {
        private CancellationTokenSource cancellationTokenSource;

        public MagnetLink MagnetLink { get; }
        public string OutputFilePath { get; private set; }

        private FileProcessStatus status = FileProcessStatus.None;
        public FileProcessStatus Status
        {
            get => status;
            private set
            {
                if (value == FileProcessStatus.Error)
                {
                    StatusError = status;
                }
                SetProperty(ref status, value);
            }
        }

        private FileProcessStatus statusError = FileProcessStatus.None;
        public FileProcessStatus StatusError
        {
            get => statusError;
            private set => SetProperty(ref statusError, value);
        }


        public FileProcess(MagnetLink magnetLink)
        {
            MagnetLink = magnetLink;
        }

        public void DownloadAndConvert(TorrentDownloader downloader, MobiConverter converter)
        {
            try
            {
                cancellationTokenSource = new CancellationTokenSource();
                Status = FileProcessStatus.Downloading;
                var downloadDirectory = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_temp_" + Guid.NewGuid().ToString()));
                string downloadedFilePath = downloader.DownloadMagnetLink(new Model.MagnetLink(MagnetLink.Link, downloadDirectory.FullName), cancellationTokenSource.Token);

                Status = FileProcessStatus.Converting;
                string mobiFilePath = converter.ConvertToMobi(downloadedFilePath, cancellationTokenSource.Token);

                OutputFilePath = mobiFilePath;
                Status = FileProcessStatus.PendingSend;
            }
            catch
            {
                Status = FileProcessStatus.Error;
            }
        }

        public void Cancel()
        {
            cancellationTokenSource?.Cancel();
        }

        public static void SendFiles(IEmailService emailService, List<FileProcess> filesProcess, FullEmail origin, Email destination)
        {
            try
            {
                filesProcess.ForEach(f => f.Status = FileProcessStatus.Sending);
                emailService.Send(origin, destination, filesProcess.Select(f => f.OutputFilePath).ToList());
                filesProcess.ForEach(f => f.Status = FileProcessStatus.Completed);
            }
            catch
            {
                filesProcess.ForEach(f => f.Status = FileProcessStatus.Error);
            }
        }
    }
}
