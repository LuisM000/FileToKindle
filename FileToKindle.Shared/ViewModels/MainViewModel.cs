using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using FileToKindle.Shared.Models;
using FileToKindle.Shared.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using DependencyService = Xamarin.Forms.DependencyService;

namespace FileToKindle.Shared.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        ISettingsService settingsService = DependencyService.Get<ISettingsService>();

        public ObservableCollection<MagnetLink> MagnetLinks { get; } = new ObservableCollection<MagnetLink>();
        public MagnetLink MagnetLink { get; } = MagnetLink.Empty();

        private Email kindleEmail = Email.Empty();
        public Email KindleEmail
        {
            get => kindleEmail;
            set => SetProperty(ref kindleEmail, value);
        }

        public ICommand AddMagnetLinkCommand { get; }
        public ICommand DeleteMagnetLinkCommand { get; }
        public ICommand ProcessCommand { get; }

        public MainViewModel()
        {
            AddMagnetLinkCommand = new Command<MagnetLink>(AddMagnetLink);
            DeleteMagnetLinkCommand = new Command<MagnetLink>(DeleteMagnetLink);
            ProcessCommand = new AsyncCommand(ProcessAsync);
        }

        public override void Initialize(params object[] navigationData)
        {
            KindleEmail = settingsService.GetKindleEmail();
        }

        private void AddMagnetLink(MagnetLink magnetLink)
        {
            if(magnetLink.IsValid)
            {
                MagnetLinks.Add(new MagnetLink(magnetLink.Link));
                MagnetLink.Clear();
            }            
        }

        private void DeleteMagnetLink(MagnetLink magnetLink)
        {
            MagnetLinks.Remove(magnetLink);
        }

        private async Task ProcessAsync()
        {
            try
            {
                if (MagnetLinks.Any() && KindleEmail.IsValid)
                {
                    await settingsService.SaveKindleAsync(KindleEmail);
                    var senderEmail = await GetSenderEmailAsync();
                    if (senderEmail.IsValid)
                    {
                        await NavigationService.NavigateToAsync<ProcessViewModel>(MagnetLinks, KindleEmail, senderEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }            
        }

        private async Task<FullEmail> GetSenderEmailAsync()
        {
            FullEmail result = FullEmail.Empty();

            try
            {
                var closeEmailPage = new TaskCompletionSource<FullEmail>();
                Action<FullEmail> onFinalize = (email) => closeEmailPage.TrySetResult(email);

                await NavigationService.NavigateToAsync<EmailViewModel>(onFinalize, true);

                result = await closeEmailPage.Task;
            }
            finally
            {
                await NavigationService.NavigateBackAsync();
            }
            return result;
        }

    }
}
