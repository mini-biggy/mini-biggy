using MiniBiggy.Util;
using System;
using System.IO;
using System.Linq;

namespace MiniBiggy.BackupStrategies
{
    public class FileSystemBackup
    {
        private readonly string _backupFrom, _filenameWithoutExtension, _backupToDir;
        private readonly int _maxNumberOfFilesToKeep;

        public FileSystemBackup(string backupFrom, string backupToDir, int maxNumberOfFilesToKeep)
        {
            _backupFrom = backupFrom;
            _filenameWithoutExtension = Path.GetFileNameWithoutExtension(backupFrom);
            _backupToDir = backupToDir;
            _maxNumberOfFilesToKeep = maxNumberOfFilesToKeep;
        }

        public string Backup()
        {
            Directory.CreateDirectory(_backupToDir);
            var backupPath = Path.Combine(_backupToDir, $"{_filenameWithoutExtension}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.bkp");
            try
            {
                File.Copy(_backupFrom, backupPath, true);
            }
            catch (Exception ex)
            {
                throw new BackupException(backupPath, ex);
            }
            RemoveOldFiles();
            return backupPath;
        }

        private void RemoveOldFiles()
        {
            var dirInfo = new DirectoryInfo(_backupToDir);
            var files = dirInfo.GetFiles("*.bkp", SearchOption.AllDirectories)
                .OrderByDescending(f => f.CreationTime);
            foreach (var fileInfo in files.Skip(_maxNumberOfFilesToKeep))
            {
                Try.SwallowingExceptions(() => fileInfo.Delete());
            }
        }
    }

    public class BackupException : Exception
    {
        public string Path { get; set; }

        public BackupException(string path, Exception inner) : base("Could not perform backup. See inner exception for more details", inner)
        {
            Path = path;
        }
    }
}