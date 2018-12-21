using System;

namespace MiniBiggy.BackupStrategies
{
    public interface IBackupStrategy
    {
        event EventHandler<BackupAttemptedEventArgs> BackupAttempted;
    }
}