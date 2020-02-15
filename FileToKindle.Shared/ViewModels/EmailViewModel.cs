using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FileToKindle.Shared.Models;
using FileToKindle.Shared.Services;
using MvvmHelpers.Commands;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace FileToKindle.Shared.ViewModels
{
    public class EmailViewModel : ViewModelBase
    {
        ISettingsService settingsService = DependencyService.Get<ISettingsService>();
        Action<FullEmail> onFinalize;

        private FullEmail email = FullEmail.Empty();
        public FullEmail Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public ICommand AcceptCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }

        public EmailViewModel()
        {   
            CancelCommand = new Command(Cancel);
            AcceptCommand = new Command(Accept);
            SaveCommand = new AsyncCommand(SaveCommandAsync);
        }

        public override void Initialize(params object[] navigationData)
        {
            this.onFinalize = (Action<FullEmail>)navigationData[0];
            bool loadEmailFromSettings = (bool)navigationData[1];
            if(loadEmailFromSettings)
            {
                Email = settingsService.GetSenderEmail();
            }
        }

        private void Accept()
        {
            if(Email.IsValid)
            {
                onFinalize?.Invoke(Email);
            }
        }

        private void Cancel()
        {
            onFinalize?.Invoke(FullEmail.Empty());
        }

        private async Task SaveCommandAsync()
        {
            await settingsService.SaveSenderEmailAsync(Email);
        }
    }
}
