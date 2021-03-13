using System;
using System.Diagnostics;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "D://"
                }
            };
            process.Start();


        }


    }
    public class BaseInfo
    {
        public int MyProperty { get; set; }
    }
    public class UserInfo: BaseInfo
    {
        public int Age { get; set; }
    }
}
