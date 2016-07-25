using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avatar.Controller;
using Leap;
using System.Threading;

namespace Avatar.Listenser
{
    using Listener = Leap.Listener;
    using Controller = Leap.Controller;

    class PilotListener : Listener
    {
        private Object thisLock = new Object();

        private void SafeOutputInstruction(String line)
        {
            lock (thisLock)
            {
                Console.WriteLine(line);
            }
        }

        public override void OnInit(Controller controller)
        {
            SafeOutputInstruction("PilotListener Initialized");
        }

        public override void OnConnect(Controller controller)
        {
            SafeOutputInstruction("Device Connected");
            controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
            controller.Config.SetFloat("Gesture.Swipe.MinLength", 200.0f);
            controller.Config.Save();
        }

        public override void OnDisconnect(Controller controller)
        {
            //Note: not dispatched when running in a debugger.
            SafeOutputInstruction("Device Disconnected");
        }

        public override void OnExit(Controller controller)
        {
            SafeOutputInstruction("PilotListener Exited");
        }

        public override void OnFrame(Controller controller)
        {
            // Get the most recent frame and report some basic information
            Frame frame = controller.Frame();

            //switch listener when swipe gesture is detected
            GestureList gesturelist = frame.Gestures();
            Gesture gesture = gesturelist[0];
            if (gesture.Type == Gesture.GestureType.TYPESWIPE)
            {
                SwipeGesture swipeGesture = new SwipeGesture(gesture);
                Vector startPoi = swipeGesture.StartPosition;
                Vector curPoi = swipeGesture.Position;
                float distance = Math.Abs(curPoi.x - startPoi.x);
                if (distance > 50)
                {
                    Thread.Sleep(1000);
                    LeapController.SwitchListener();
                    return;
                }
            }

            //todo: autopilot controlling code here
        }
    }
}
