using System.Diagnostics;
using flrig.net.lib;

namespace flrig.yaesu.ft891
{
    public class YaesuFt891 : IRigs
    {
        public string Test { get; set; }

        public string Name { get; set; } = "hello";

        public string Description { get; set; } = "Hello message";
        
        public int Execute()
        {
            Debug.WriteLine("Hello !!!");
            return 0;
        }
    }
}
