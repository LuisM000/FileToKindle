using System;
using System.Collections.Generic;
using FileToKindle.Shared.Models;

namespace FileToKindle.Shared.Services
{
    public interface IEmailService
    {
        void Send(FullEmail origin, Email destination, IList<string> attachments);
    }
}
