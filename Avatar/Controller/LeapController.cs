using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using Avatar.Listenser;

namespace Avatar.Controller
{
    using Controller = Leap.Controller;
    static class LeapController
    {
        static Controller controller = new Controller();
        static Listener[] listeners= { new WheelListener() , new PilotListener() };
        static int curLisIndex = -1;

        public static void Init(int listenerIndex)
        {
            if(curLisIndex==-1)
            {
                controller.AddListener(listeners[listenerIndex]);
                curLisIndex = listenerIndex;
            }
            else
            {
                Console.WriteLine("Listener already initiated");
            }
        }


        public static void SwitchListener()
        {
            if(curLisIndex!=-1)
            {
                int index = (curLisIndex + 1) % listeners.Length;
                controller.RemoveListener(listeners[curLisIndex]);
                controller.AddListener(listeners[index]);
                Console.WriteLine("Switch from:" + curLisIndex + " to:" + index);
                curLisIndex = index;
            }
            else
            {
                Console.WriteLine("Listener not initated");
            }
        }

        public static void RemoveCurrentLisenter()
        {
            if(curLisIndex!=-1)
            {
                controller.RemoveListener(listeners[curLisIndex]);
            }
        }
    }
}
