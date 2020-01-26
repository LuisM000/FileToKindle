using System;
using System.Collections.Generic;
using CommandLine;

namespace FileToKindle.Console.Options
{
    [Verb("process", HelpText = "Procesa magnet links")]
    public class ProcessMagnetLinksOptions
    {      
        [Option('m', "magnetLinks", Required = true, HelpText = "Magnet link desde donde se descargará el epub")]
        public IList<string> MagnetLinks { get; set; }

        [Option('e', "kindleEmail", Required = true, HelpText = "Email asociado a la cuenta kindle donde se enviará el archivo")]
        public string DestinationEmail { get; set; }
    }
}
