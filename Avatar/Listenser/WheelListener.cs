using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using Avatar.Controller;
using System.Threading;

namespace Avatar.Listenser
{
    using Listener = Leap.Listener;
    using Controller = Leap.Controller;

    class WheelListener : Listener
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
            SafeOutputInstruction("WheelListener Initialized");
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
            SafeOutputInstruction("WheelListener Exited");
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
                Console.WriteLine("dis:"+distance);
                if (distance > 50)
                {
                    Thread.Sleep(1000);
                    LeapController.SwitchListener();
                    return;
                }
            }

            // Decide if the car goes forward, backward or stands still depending on the extended condition of left and right thumb
            Hand leftHand = frame.Hands[0];
            Hand rightHand = frame.Hands[1];

            FingerList extendedLeftFingerList = leftHand.Fingers.Extended();
            FingerList extendedRightFingerList = rightHand.Fingers.Extended();
            FingerList leftThumbList = extendedLeftFingerList.FingerType(Finger.FingerType.TYPE_THUMB);
            FingerList rightThumbList = extendedRightFingerList.FingerType(Finger.FingerType.TYPE_THUMB);
            Finger leftThumb = leftThumbList.Count == 0 ? null : leftThumbList[0];
            Finger rightThumb = rightThumbList.Count == 0 ? null : rightThumbList[0];

            if (leftThumb == null && rightThumb == null || leftThumb != null && rightThumb != null)
            {
                SafeOutputInstruction("st");
            }
            else if (leftThumb != null && rightThumb == null)
            {
                SafeOutputInstruction("fd");
            }
            else
            {
                SafeOutputInstruction("bk");
            }


            // Set the direction
            Vector leftVec = leftHand.PalmPosition;
            Vector rightVec = rightHand.PalmPosition;

            float slope = (leftVec.z - rightVec.z) / (leftVec.x - rightVec.x);
            double angle = Math.Atan(slope) * 180 / Math.PI;
            angle = angle < 90 ? angle : angle - 180;


            //only if the angle between two lines is above 10 degree, do the following
            if (Math.Abs(angle) >= 10)
            {
				SafeOutputInstruction(Math.Round(angle,0).ToString());
				/*
                //control the direction
                if (angle < 0 )
                {
                    SafeOutputInstruction("rt"+);
                }
                else
                {
                    SafeOutputInstruction("lt");
                }
				*/
            }
        }
    }
}
