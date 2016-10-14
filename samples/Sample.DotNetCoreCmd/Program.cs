using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MiniBiggy;

namespace Sample.DotNetCoreCmd {
    public class Program {
        public static void Main(string[] args) {

            var storagePath = @"C:\dev\way2labs\SxDashboard\src\SxDashboard\dashboardfiles\dashboardheaders.data";
            var list  = CreateList<Tweet>.SavingAt(storagePath)
                                                          .UsingPrettyJsonSerializer()
                                                          .SavingWhenRequested();

            list.Saved += (sender, eventArgs) => {
                Console.WriteLine("saved");
            };

            list.Add(new Tweet());

            list.SaveAsync().Wait();



            var listPath = @"db\tweets.json";
            var bkpDir = @"db\bkp";

            var db = CreateList<Tweet>
                .SavingAt(listPath)
                .UsingPrettyJsonSerializer()
                .SavingWhenRequested();

            var backup = ConfigureBackup.CopyListFrom(listPath)
                .ToDirectory(bkpDir)
                .KeepNewest(10)
                .BackupEverySave(db);

            backup.BackupAttempted += (sender, eventArgs) => {
                Console.WriteLine($"Backup: {eventArgs.BackupPath} - {eventArgs.Success} : {eventArgs.Exception}");
            };

            Console.WriteLine("Hello, hit enter to create and save a tweet");
            while (true) {
                var line = Console.ReadLine();
                if (line == "exit") {
                    break;
                }
                db.Add(new Tweet {Message = line});
                db.Save();
            }
            Console.WriteLine("End");
        }
    }

    public class Tweet {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
    }
}
