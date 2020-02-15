using System;
using System.Threading.Tasks;
using FileToKindle.Shared.Models;
using FileToKindle.Shared.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Dependency(typeof(SettingsService))]
namespace FileToKindle.Shared.Services
{
    public class SettingsService : ISettingsService
    {
        const string senderEmailKey = "senderEmailKey";
        const string kindleEmailKey = "kindleEmailKey";

        public FullEmail GetSenderEmail()
        {
            return Get(senderEmailKey, FullEmail.Empty());
        }

        public Task SaveSenderEmailAsync(FullEmail senderEmail)
        {
            return SaveAsync(senderEmailKey, senderEmail);
        }

        public Email GetKindleEmail()
        {
            return Get(kindleEmailKey, Email.Empty());
        }

        public Task SaveKindleAsync(Email kindleEmail)
        {
            return SaveAsync(kindleEmailKey, kindleEmail);
        }


        private T Get<T>(string key, T defaultValue)
        {
            if (Application.Current.Properties.TryGetValue(key, out object value))
            {
                return JsonConvert.DeserializeObject<T>(value?.ToString()) ?? defaultValue;
            }
            return defaultValue;
        }

        private Task SaveAsync(string key, object value)
        {
            Application.Current.Properties[key] = JsonConvert.SerializeObject(value);
            return Application.Current.SavePropertiesAsync();
        }
    }
}
