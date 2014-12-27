using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace JoystickBall
{
    public partial class Program
    {
        Joystick.Position joystickPosition = new Joystick.Position();
        private Joystick.Position currentPosition = new Joystick.Position();
        private GT.Timer joystickTimer;
        private int displayHeight, displayWidth = 0;
        int inversion = 1;


        void ProgramStarted()
        {
            Debug.Print("Program Started");
            displayHeight = displayT43.Height;
            displayWidth = displayT43.Width;
            currentPosition.X = displayWidth / 2.0;
            currentPosition.Y = displayHeight / 2.0;
            joystickPosition = joystick.GetPosition();
            joystickTimer = new GT.Timer(100);
            joystickTimer.Tick += JoystickTimerOnTick;
            button.ButtonPressed += button_ButtonPressed;
            joystick.Calibrate();
            joystickTimer.Start();
            multicolorLED.TurnRed();
        }

        private void JoystickTimerOnTick(GT.Timer timer)
        {
            MoveCursor();
        }


        private void button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            inversion *= -1;
            if (inversion == 1) multicolorLED.TurnRed();
            else multicolorLED.TurnBlue();
            displayT43.SimpleGraphics.Clear();
            currentPosition.X = displayWidth / 2.0;
            currentPosition.Y = displayHeight / 2.0;
        }

        private void MoveCursor()
        {
            double realX = 0, realY = 0;
            Joystick.Position newJoystickPosition = joystick.GetPosition();
            double newX = joystickPosition.X;
            double newY = joystickPosition.Y;
            joystickPosition = newJoystickPosition;

            // did we actually move...
            if (System.Math.Abs(newX) >= 0.03) { realX = newX; }
            if (System.Math.Abs(newY) >= 0.03) { realY = newY; }
            if (realX == 0.0 && realY == 0.0) return;

            Debug.Print(realX + " " + realY);
            currentPosition.X += realX * 5;
            currentPosition.Y += (realY * 5) * inversion;

            bool justchangedY = false;
            if (realX + currentPosition.X >= displayWidth) currentPosition.X = 0;
            if (realX + currentPosition.X <= 0) currentPosition.X = displayWidth;
            if (realY + currentPosition.Y >= displayHeight)
            {
                currentPosition.Y = 0;
                justchangedY = true;
            }
            if (realY + currentPosition.Y <= 0 && !justchangedY)
            {
                currentPosition.Y = displayHeight;
            }
            displayT43.SimpleGraphics.DisplayEllipse(Gadgeteer.Color.Orange, 1,Gadgeteer.Color.White, (int)currentPosition.X, (int)currentPosition.Y, 2, 2);
        }
    }
}
