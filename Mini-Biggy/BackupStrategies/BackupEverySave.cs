using System;
using System.Threading.Tasks;

namespace MiniBiggy.BackupStrategies
{
    public class BackupEverySave<T> : IBackupStrategy where T : new()
    {
        public event EventHandler<BackupAttemptedEventArgs> BackupAttempted;

        public BackupEverySave(PersistentList<T> list, string listPath, string pathToSaveTheBackup, int maxNumberOfFilesToKeep = 10)
        {
            var backup = new FileSystemBackup(listPath, pathToSaveTheBackup, maxNumberOfFilesToKeep);

            list.Saved += (sender, args) =>
            {
                Task.Run(() =>
                {
                    try
                    {
                        var backupPath = backup.Backup();
                        BackupAttempted?.Invoke(this, new BackupAttemptedEventArgs(backupPath));
                    }
                    catch (BackupException ex)
                    {
                        BackupAttempted?.Invoke(this, new BackupAttemptedEventArgs(ex));
                    }
                });
            };
        }
    }
}