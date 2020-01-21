using System;
namespace FileToKindle.Model
{
    public class MagnetLink
    {
        public string Link { get; }
        public string DownloadDirectory { get; }


        public MagnetLink(string link, string downloadDirectory)
        {
            Link = link;
            DownloadDirectory = downloadDirectory;
        }
      
    }
}
