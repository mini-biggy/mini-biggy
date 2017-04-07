using System;
using System.Threading.Tasks;
using MiniBiggy.Util;

namespace MiniBiggy.BackupStrategies {
    public class DailyBackup : IBackupStrategy {
        private readonly int _hourBase;
        private FileSystemBackup _backup;

        public event EventHandler<BackupAttemptedEventArgs> BackupAttempted;

        public DailyBackup(int hourBase, string listPath, string pathToSaveTheBackup, int maxNumberOfFilesToKeep = 10) {
            _hourBase = hourBase;
            _backup = new FileSystemBackup(listPath, pathToSaveTheBackup, maxNumberOfFilesToKeep);
        }

        public void Start() {
            Task.Factory.StartNew(Loop, TaskCreationOptions.LongRunning);
        }

        private async Task Loop() {
            while (true) {
                await TimeMachine.Delay(TimeSpan.FromMinutes(1));
                var currentHour = TimeMachine.Now.Hour;
                if (currentHour != _hourBase) {
                    return;
                }
                try {
                    var backupPath = _backup.Backup();
                    BackupAttempted?.Invoke(this, new BackupAttemptedEventArgs(backupPath));
                }
                catch (BackupException ex) {
                    BackupAttempted?.Invoke(this, new BackupAttemptedEventArgs(ex));
                }
            }
        }
    }
}