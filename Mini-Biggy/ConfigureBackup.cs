using MiniBiggy.BackupStrategies;

namespace MiniBiggy
{
    public class ConfigureBackup : IChooseTargetDirectory, IChooseNumberOfFilesToKeep, IChooseBackupKind
    {
        private string _sourceDir, _targetDir;
        private int _filesToKeep;

        private ConfigureBackup()
        {
        }

        public static IChooseTargetDirectory CopyListFrom(string sourceDir)
        {
            var config = new ConfigureBackup();
            config._sourceDir = sourceDir;
            return config;
        }

        public IChooseNumberOfFilesToKeep ToDirectory(string dirPath)
        {
            _targetDir = dirPath;
            return this;
        }

        public IChooseBackupKind KeepNewest(int numberOfFiles)
        {
            _filesToKeep = numberOfFiles;
            return this;
        }

        public IBackupStrategy BackupEveryDayAtHour(int hourOfDay)
        {
            return new DailyBackup(hourOfDay, _sourceDir, _targetDir, _filesToKeep);
        }

        public IBackupStrategy BackupEverySave<T>(PersistentList<T> list) where T : new()
        {
            return new BackupEverySave<T>(list, _sourceDir, _targetDir, _filesToKeep);
        }
    }

    public interface IChooseTargetDirectory
    {
        IChooseNumberOfFilesToKeep ToDirectory(string dirPath);
    }

    public interface IChooseNumberOfFilesToKeep
    {
        IChooseBackupKind KeepNewest(int numberOfFiles);
    }

    public interface IChooseBackupKind
    {
        IBackupStrategy BackupEveryDayAtHour(int hourOfDay);

        IBackupStrategy BackupEverySave<T>(PersistentList<T> list) where T : new();
    }
}