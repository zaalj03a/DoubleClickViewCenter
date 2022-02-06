using ABB.Robotics.Math;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Environment;
using ABB.Robotics.RobotStudio.Stations;
using ABB.Robotics.RobotStudio.Stations.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;

namespace DoubleClickViewCenter
{
    public class DoubleClickViewCenter
    {
        private static DateTime _previousClickTimestamp = DateTime.MinValue;
        private static Vector3 _previousClickLocation = new Vector3();

        public static void AddinMain()
        {
            GraphicPicker.GraphicPick += OnGraphicPick;
        }

        private static void OnGraphicPick(object sender, GraphicPickEventArgs e)
        {
            // Determine if this click is a double click
            if ((DateTime.Now - _previousClickTimestamp).TotalMilliseconds <= 500 && _previousClickLocation.AlmostEquals(e.PickedPosition))
            {
                // If user holds SHIFT key while performing a double click then whatever object is picked should be examined instead
                if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) && e.PickedObject != null)
                {
                    // Examine object if possible
                    if (GraphicControl.ActiveGraphicControl.CanExamineObject(e.PickedObject))
                    {
                        GraphicControl.ActiveGraphicControl.ExamineObject(e.PickedObject, 0.3f);
                        return;
                    }
                }

                // View center around cursor if SHIFT key is not pressed  
                GraphicControl.ActiveGraphicControl.ViewCenter(e.PickedPosition, 0.3f);
            }

            // Save current pick position and time for next click event
            _previousClickLocation = e.PickedPosition;
            _previousClickTimestamp = DateTime.Now;
        }

    }
}