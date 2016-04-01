using NUnitLite;

namespace MiniBiggy.Tests
{
    public class Program {
        public static int Main(string[] args) {
            args = new[] {
                "--workers","1"
            };
            return new AutoRun().Execute(args);
        }
    }
}
