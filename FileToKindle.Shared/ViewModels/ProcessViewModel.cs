using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using FileToKindle.Services;
using FileToKindle.Shared.Models;
using FileToKindle.Shared.Services;
using MimeKit;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Xamarin.Forms;

namespace FileToKindle.Shared.ViewModels
{
    public enum ProcessViewModelStatus
    {
        None, ProcessingFiles, SendingFiles, Error, Completed
    }

    public class ProcessViewModel : ViewModelBase
    {
        readonly TorrentDownloader torrentDownloader = new TorrentDownloader();
        readonly MobiConverter mobiConverter = new MobiConverter();
        readonly IEmailService emailService = DependencyService.Get<IEmailService>();
        

        private Email kindleEmail;
        private FullEmail senderEmail;

        public ObservableRangeCollection<FileProcess> FilesProcess { get; } = new ObservableRangeCollection<FileProcess>();

        private ProcessViewModelStatus status = ProcessViewModelStatus.None;
        public ProcessViewModelStatus Status
        {
            get => status;
            private set => SetProperty(ref status, value);
        }

        private FilesProcessResult filesProcessResult;
        public FilesProcessResult FilesProcessResult
        {
            get => filesProcessResult;
            private set => SetProperty(ref filesProcessResult, value);
        }

        public ICommand BackCommand { get; }
        public ICommand CancelFileProcessCommand { get; }
        public ICommand RetryProcessWithErrorCommand { get; }


        public ProcessViewModel()
        {
            BackCommand = new AsyncCommand(BackAsync);
            CancelFileProcessCommand = new MvvmHelpers.Commands.Command<FileProcess>(CancelFileProcess);
            RetryProcessWithErrorCommand = new MvvmHelpers.Commands.Command(RetryProcessWithError);
        }

        public override void Initialize(params object[] navigationData)
        {
            var magnetLinks = (IList<MagnetLink>)navigationData[0];   
            this.kindleEmail = (Email)navigationData[1];
            this.senderEmail = (FullEmail)navigationData[2];

            FilesProcess.AddRange(magnetLinks.Select(ml => new FileProcess(ml)).ToList());
            Task.Run(Process);         
        }

        private void Process()
        {
            try
            {
                Status = ProcessViewModelStatus.ProcessingFiles;

                Parallel.ForEach(FilesProcess, (fileProcess) =>
                {
                    fileProcess.DownloadAndConvert(torrentDownloader, mobiConverter);
                });

                var completedFiles = FilesProcess.Where(f => f.Status == FileProcessStatus.PendingSend).ToList();
                if (completedFiles.Any())
                {
                    Status = ProcessViewModelStatus.SendingFiles;
                    FileProcess.SendFiles(emailService, completedFiles, senderEmail, kindleEmail);
                }

                Status = ProcessViewModelStatus.Completed;
            }
            catch
            {
                Status = ProcessViewModelStatus.Error;
            }
            finally
            {
                FilesProcessResult = new FilesProcessResult(FilesProcess);
            }
        }
        

        private void CancelFileProcess(FileProcess fileProcess)
        {
            fileProcess?.Cancel();
        }

        private void CancellAllProcess()
        {
            FilesProcess.ToList().ForEach(f => f.Cancel());
        }

        private void RetryProcessWithError()
        {           
            FilesProcess.RemoveRange(FilesProcess.Where(f => f.Status != FileProcessStatus.Error).ToList());
            Task.Run(Process);
        }

        private Task BackAsync()
        {
            CancellAllProcess();
            return NavigationService.NavigateBackAsync();
        }
    }
}
