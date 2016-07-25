using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Avatar.Configuration;
using Avatar.Controller;

namespace Avatar
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting LeapMotion...");
            Thread.Sleep(1000);
            Console.WriteLine("Ready in 3");
            Thread.Sleep(1000);
            Console.WriteLine("Ready in 2");
            Thread.Sleep(1000);
            Console.WriteLine("Ready in 1");
            Thread.Sleep(1000);

            // Keep this process running until Enter is pressed
            Console.WriteLine("Press any key to quit");
            Thread.Sleep(1000);
            LeapController.Init(DeviceSetting.WHEEL);

            Console.ReadLine();

            // Remove the listener when done
            LeapController.RemoveCurrentLisenter();

        }
    }
}
