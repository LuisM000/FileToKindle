using System;
using System.Threading.Tasks;
using FileToKindle.Shared.Models;

namespace FileToKindle.Shared.Services
{
    public interface ISettingsService
    {
        FullEmail GetSenderEmail();

        Task SaveSenderEmailAsync(FullEmail senderEmail);

        Email GetKindleEmail();

        Task SaveKindleAsync(Email kindleEmail);

    }
}
