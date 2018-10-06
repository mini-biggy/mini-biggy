using System;
using System.Threading;

namespace MiniBiggy.BackupStrategies
{
    public class DailyBackup : IBackupStrategy
    {
        private readonly int _hourBase;
        private FileSystemBackup _backup;
        private Timer _timer;

        public event EventHandler<BackupAttemptedEventArgs> BackupAttempted;

        public DailyBackup(int hourBase, string listPath, string pathToSaveTheBackup, int maxNumberOfFilesToKeep = 10)
        {
            _hourBase = hourBase;
            _backup = new FileSystemBackup(listPath, pathToSaveTheBackup, maxNumberOfFilesToKeep);
        }

        public void Start()
        {
            var startAt = TimeSpan.FromHours(_hourBase) - DateTime.Now.TimeOfDay;
            if (startAt.Ticks < 0)
                startAt += TimeSpan.FromDays(1);

            _timer = new Timer(state => Loop(), null, startAt, TimeSpan.FromDays(1));
        }

        private void Loop()
        {
            try
            {
                var backupPath = _backup.Backup();
                BackupAttempted?.Invoke(this, new BackupAttemptedEventArgs(backupPath));
            }
            catch (BackupException ex)
            {
                BackupAttempted?.Invoke(this, new BackupAttemptedEventArgs(ex));
            }
        }
    }
}