using MiniBiggy;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sample.DotNetCoreCmd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            var storagePath = @"db\tweets.json";
            var list = Create.ListOf<Tweet>().SavingAt(storagePath)
                                             .UsingPrettyJsonSerializer()
                                             .BackgroundSavingEveryTwoSeconds();

            Console.WriteLine("Loaded: " + sw.Elapsed.TotalMilliseconds);

            list.Saved += (sender, arg) =>
            {
                Console.WriteLine($"-- Save: Serialize: {arg.TimeToSerialize} Save: {arg.TimeToSave} Size: {arg.SizeInBytes} Success: {arg.Success} Error: {arg.Exception?.ToString() ?? "None"}");
            };

            var times = 1000000;
            var result = Parallel.For(0, times, async (i) =>
            {
                list.Add(new Tweet
                {
                    Id = i,
                    Message = "" + i
                }
                );
                //await list.SaveAsync();
            });
            //await list.SaveAsync();

            Console.WriteLine("End");

            //var listPath = @"db\tweets.json";
            //var bkpDir = @"db\bkp";

            //var db = Create.ListOf<Tweet>()
            //    .SavingAt(listPath)
            //    .UsingPrettyJsonSerializer()
            //    .SavingWhenRequested();

            //var backup = ConfigureBackup.CopyListFrom(listPath)
            //    .ToDirectory(bkpDir)
            //    .KeepNewest(10)
            //    .BackupEverySave(db);

            //backup.BackupAttempted += (sender, eventArgs) => {
            //    Console.WriteLine($"Backup: {eventArgs.BackupPath} - {eventArgs.Success} : {eventArgs.Exception}");
            //};

            //Console.WriteLine("Hello, hit enter to create and save a tweet");
            //while (true) {
            //    //var line = Console.ReadLine();
            //    //if (line == "exit") {
            //    //    break;
            //    //}
            //    db.Add(new Tweet {Message = "foo"});
            //    db.Save();
            //}
            //Console.WriteLine("End");

            Console.ReadLine();
        }
    }

    public class Tweet
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
    }
}