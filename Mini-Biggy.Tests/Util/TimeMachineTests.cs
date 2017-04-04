using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MiniBiggy.Util;
using Xunit;

namespace MiniBiggy.Tests.Util {
    public class TimeMachineTests {
        [Fact]
        public void Shoul_replace_now() {
            var fixedDate = new DateTime(2015, 1, 1);
            TimeMachine.OverrideNowWith(() => fixedDate);
            Assert.Equal(fixedDate, TimeMachine.Now);
        }

        [Fact]
        public async Task Should_override_delay() {
            TimeMachine.OverrideDelayWith(orig => TimeSpan.Zero);
            var sw = new Stopwatch();
            sw.Start();
            await TimeMachine.Delay(10000000);
            Assert.True(sw.Elapsed < TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task Should_unblock_delay() {
            var task = Task.Run(async () => {
                while (TimeMachine.UnblockAllDelays() == 0) {
                    await Task.Delay(10);
                }
            });
            await TimeMachine.Delay(10000000);
        }
    }
}