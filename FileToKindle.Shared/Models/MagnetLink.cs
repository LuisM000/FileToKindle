using System;
using System.Text.RegularExpressions;
using System.Web;
using FileToKindle.Shared.Utils;
using MvvmHelpers;

namespace FileToKindle.Shared.Models
{
    public class MagnetLink : ObservableObject
    {
        public static MagnetLink Empty() => new MagnetLink(string.Empty);
        
        public MagnetLink(string link)
        {
            this.Link = link;
        }
        
        private string link;
        public string Link
        {
            get => link;
            set
            {
                if(SetProperty(ref link, value))
                {
                    IsValid = Regex.IsMatch(value, RegularExpressions.MagnetLink, RegexOptions.IgnoreCase);
                    DisplayNameOrLink = HttpUtility.ParseQueryString(link).Get("dn") ?? link;
                }
            }
        }

        private bool isValid;
        public bool IsValid
        {
            get => isValid;
            private set => SetProperty(ref isValid, value);
        }

        private string displayNameOrLink;
        public string DisplayNameOrLink
        {
            get => displayNameOrLink;
            private set => SetProperty(ref displayNameOrLink, value);
        }

        public void Clear()
        {
            Link = string.Empty;
        }

    }
}
