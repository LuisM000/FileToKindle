using System;
using CommandLine;

namespace FileToKindle.Console
{
    [Verb("process", HelpText = "Procesa un archivo único")]
    public class ProcessOneLinkOptions
    {      
        [Option('m', "magnetLink", Required = true, HelpText = "Magnet link desde donde se descargará el epub")]
        public string MagnetLink { get; set; }

        [Option('e', "kindleEmail", Required = true, HelpText = "Email asociado a la cuenta kindle donde se enviará el archivo")]
        public string DestinationEmail { get; set; }
    }

    [Verb("clean", HelpText = "Limpia los archivos descargados y generados")]
    public class CleanOptions
    {
         
    }
}
