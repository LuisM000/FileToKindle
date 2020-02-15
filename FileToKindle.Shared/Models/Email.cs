using System;
using System.Text.RegularExpressions;
using FileToKindle.Shared.Utils;
using MvvmHelpers;

namespace FileToKindle.Shared.Models
{
    public class Email : ObservableObject
    {       
        public static Email Empty() => new Email() { Address = string.Empty };

        private string address;
        public string Address
        {
            get => address;
            set
            {
                if (SetProperty(ref address, value))
                {
                    IsValid = Regex.IsMatch(value, RegularExpressions.Email, RegexOptions.IgnoreCase);
                }
            }
        }

        private bool isValid;
        public bool IsValid
        {
            get => isValid;
            private set => SetProperty(ref isValid, value);
        }

    }
}
