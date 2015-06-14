using System;
using System.Collections;
using System.Collections.Generic;
using MiniBiggy;
using MiniBiggy.SaveStrategies;

namespace FileSystemSample {
    class Program {
        static void Main(string[] args) {
            var t = new Tweet();
            var list = PersistentList.Create<Tweet>(new BackgroundSave(TimeSpan.FromSeconds(5)));
            list.Saved += (sender, eventArgs) => {
                Console.WriteLine("Saved! Size: " + list.Count);
            };
            for (int i = 0; i < 1000; i++) {
                list.Add(t);                
            }
            Console.WriteLine("Size: " + list.Count);
            Console.ReadLine();
            list.Clear();
            Console.ReadLine();
            //list.Save();
        }

        static void Do(ICollection<Tweet> list) {
            
        }
    }
}
