using System;
using System.Collections.Generic;

namespace FileToKindle.Model
{
    public class Email
    {
        public string To { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public IEnumerable<string> AttachmentPaths { get; set; }
    }
}
