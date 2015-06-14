using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MiniBiggy.Util;
using NUnit.Framework;

namespace MiniBiggy.Tests.Util {
    public class TimeMachineTests {
        [Test]
        public void Shoul_replace_now() {
            var fixedDate = new DateTime(2015, 1, 1);
            TimeMachine.OverrideNowWith(() => fixedDate);
            Assert.AreEqual(fixedDate, TimeMachine.Now);
        }

        [Test]
        public async void Should_override_delay() {
            TimeMachine.OverrideDelayWith(orig => TimeSpan.Zero);
            var sw = new Stopwatch();
            sw.Start();
            await TimeMachine.Delay(10000000);
            Assert.IsTrue(sw.Elapsed < TimeSpan.FromSeconds(1));
        }

        [Test]
        public async void Should_unblock_delay() {
            var task = Task.Run(async () => {
                while (TimeMachine.UnblockAllDelays() == 0) {
                    await Task.Delay(10);
                }
            });
            await TimeMachine.Delay(10000000);
        }

        [Test]
        public async void Should_unblock_one_or_more_delays() {
            var task = Task.Run(() => {
                TimeMachine.UnblockOneOrMoreDelays();
            });
            await TimeMachine.Delay(10000000);
        }
    }
}