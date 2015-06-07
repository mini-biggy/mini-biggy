using System;
using System.Collections;
using System.Collections.Generic;
using MiniBiggy;

namespace FileSystemSample {
    class Program {
        static void Main(string[] args) {
            var t = new Tweet();
            var list = PersistentList.Create<Tweet>();
            list.AutoSave = false;
            for (int i = 0; i < 1000; i++) {
                list.Add(t);                
            }
            Console.WriteLine("Size: " + list.Count);
            list.Clear();
            list.Save();
            Console.ReadLine();
        }

        static void Do(ICollection<Tweet> list) {
            
        }
    }
}
