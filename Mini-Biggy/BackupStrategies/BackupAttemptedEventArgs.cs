using System;

namespace MiniBiggy.BackupStrategies
{
    public class BackupAttemptedEventArgs : EventArgs
    {
        public string BackupPath { get; }
        public bool Success { get; }
        public Exception Exception { get; }

        public BackupAttemptedEventArgs(string backupPath)
        {
            BackupPath = backupPath;
            Success = true;
        }

        public BackupAttemptedEventArgs(BackupException ex)
        {
            BackupPath = ex.Path;
            Exception = ex.InnerException;
        }
    }
}