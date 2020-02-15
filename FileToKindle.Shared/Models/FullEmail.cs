using System;
using System.Text.RegularExpressions;
using FileToKindle.Shared.Utils;
using MvvmHelpers;

namespace FileToKindle.Shared.Models
{
    public class FullEmail : ObservableObject
    {
        public static FullEmail Empty() => new FullEmail() { Address = string.Empty, Password = string.Empty };

        private string address;
        public string Address
        {
            get => address;
            set
            {
                if (SetProperty(ref address, value))
                {
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                if (SetProperty(ref password, value))
                {
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        private string host;
        public string Host
        {
            get => host;
            set => SetProperty(ref host, value);
        }
        
        private int port;
        public int Port
        {
            get => port;
            set => SetProperty(ref port, value);
        }


        public bool IsValid
        {
            get => Regex.IsMatch(Address, RegularExpressions.Email, RegexOptions.IgnoreCase) && !string.IsNullOrEmpty(Password);
        }

    }
}
