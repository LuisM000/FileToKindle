using System;
using System.Collections.Generic;
using System.Linq;

namespace FileToKindle.Shared.Models
{
    public class FilesProcessResult
    {
        public int CompletedCount => Completed.Count;
        public int NotCompletedCount => NotCompleted.Count;

        public IList<FileProcess> Completed { get; }
        public IList<FileProcess> NotCompleted { get; }

        public FilesProcessResult(IList<FileProcess> filesProcess)
        {
            Completed = filesProcess.Where(c => c.Status == FileProcessStatus.Completed).ToList();
            NotCompleted = filesProcess.Where(c => c.Status != FileProcessStatus.Completed).ToList();
        }
    }
}
