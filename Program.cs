using System;
using System.Drawing;
using System.Threading;
using SharpDX.XInput;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FortniteMapper
{
    internal class Program
    {
        //[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public static Controller controller;
        public static State stateNew;
        public static State stateOld;
        //private int x, y;
        public static System.Timers.Timer LoopTimer = new System.Timers.Timer();
        const uint WM_KEYUP = 0x101;
        const uint WM_KEYDOWN = 0x100;
        public static IntPtr FNhwnd = IntPtr.Zero;

        public const int VK_DOWN = 0x28;
        public const int VK_LEFT = 0x25;
        public const int VK_UP = 0x26;
        public const int VK_RIGHT = 0x27;
        public const int D_KEY = 0x44;
        public const int F_KEY = 0x46;
        public const int J_KEY = 0x4A;
        public const int K_KEY = 0x4B;
        public static bool A_HELD = false;
        public static bool B_HELD = false;
        public static bool X_HELD = false;
        public static bool Y_HELD = false;


        static async Task Main()
        {
            controller = new Controller(UserIndex.Two);
            if (!controller.IsConnected)
            {
                Console.Write("Error", "It seems like there is no XBOX 360 Controller connected via USB!");
                Console.ReadKey();
                Environment.Exit(0);
            }

            TryFindGameWindow();

            LoopTimer.Interval = 10;
            LoopTimer.Elapsed += loop_Tick;
            LoopTimer.Start();

            await Task.Delay(-1);
        }

        static public void loop_Tick(object sender, EventArgs e)
        {
            //if (!enabledcb.Checked)
            //{
            //    return;
            //}
            //
            stateNew = controller.GetState();
            ///////////////////////////////////
            //x = stateNew.Gamepad.RightThumbX;
            //y = stateNew.Gamepad.RightThumbY;

            //x /= 1000;
            //y /= 1000;
            ////////////////////////////////
            //int movementX = x;
            //int movementY = y;

            //movementX *= 1; //(int)sensnud.Value;
            //movementY *= 1; //(int)sensnud.Value;

            //movementX /= 2;
            //movementY /= 2;

            //movementY *= -1;
            ////////////////////////
            //if (x > buffernud.Value || x < -buffernud.Value)
            //{
            //    Cursor.Position = new Point(Cursor.Position.X + movementX, Cursor.Position.Y);
            //}
            //if (y > buffernud.Value || y < -buffernud.Value)
            //{
            //    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + movementY);
            //}
            //buttons
            CheckButtons();
        } 

        public static bool TryFindGameWindow()
        {
            FNhwnd = FindWindowEx(IntPtr.Zero, FNhwnd, null, "Fortnite  ");
            if (FNhwnd == IntPtr.Zero)
            {
                Console.WriteLine("Couldn't find fortnite??");
                Console.ReadKey();
                Environment.Exit(0);
            }
            return FNhwnd != IntPtr.Zero;
        }

        public static void SendKeycode(int keycode)
        {
            SendMessage(FNhwnd, WM_KEYUP, (IntPtr)keycode, (IntPtr)0);
            Thread.Sleep(100);
            SendMessage(FNhwnd, WM_KEYDOWN, (IntPtr)keycode, (IntPtr)0);
        }

        public static void SendKeyUp(int keycode)
        {
            SendMessage(FNhwnd, WM_KEYUP, (IntPtr)keycode, (IntPtr)0);
        }
        public static void SendKeyDown(int keycode)
        {
            SendMessage(FNhwnd, WM_KEYDOWN, (IntPtr)keycode, (IntPtr)0);
        }

        public static void CheckButtons()
        {
            //A -> D
            if (/*!(stateOld.Gamepad.Buttons == GamepadButtonFlags.A) && */stateNew.Gamepad.Buttons == GamepadButtonFlags.A && A_HELD == false)
            {
                A_HELD = true;
                Console.WriteLine("A button (D key)");
                SendKeyDown(D_KEY);
                do
                {

                } while (stateNew.Gamepad.Buttons == GamepadButtonFlags.A);
                A_HELD = false;
                SendKeyUp(D_KEY);
            }
            // B -> F
            if (/*!(stateOld.Gamepad.Buttons == GamepadButtonFlags.A) && */stateNew.Gamepad.Buttons == GamepadButtonFlags.B && B_HELD == false)
            {
                B_HELD = true;
                Console.WriteLine("B button (F key)");
                SendKeyDown(F_KEY);
                do
                {

                } while (stateNew.Gamepad.Buttons == GamepadButtonFlags.B);
                B_HELD = false;
                SendKeyUp(F_KEY);
            }
            //X -> K
            if (/*!(stateOld.Gamepad.Buttons == GamepadButtonFlags.A) && */stateNew.Gamepad.Buttons == GamepadButtonFlags.X && X_HELD == false)
            {
                X_HELD = true;
                Console.WriteLine("X button (K key)");
                SendKeyDown(K_KEY);
                do
                {

                } while (stateNew.Gamepad.Buttons == GamepadButtonFlags.X);
                X_HELD = false;
                SendKeyUp(K_KEY);
            }
            //Y -> J
            if (/*!(stateOld.Gamepad.Buttons == GamepadButtonFlags.A) && */stateNew.Gamepad.Buttons == GamepadButtonFlags.Y && Y_HELD == false)
            {
                Y_HELD = true;
                Console.WriteLine("Y button (J key)");
                SendKeyDown(J_KEY);
                do
                {

                } while (stateNew.Gamepad.Buttons == GamepadButtonFlags.Y);
                Y_HELD = false;
                SendKeyUp(J_KEY);
            }
            if (!(stateOld.Gamepad.Buttons == GamepadButtonFlags.DPadDown) && stateNew.Gamepad.Buttons == GamepadButtonFlags.DPadDown)
            {
                Console.WriteLine("DPadDown!");
                SendKeycode(VK_DOWN);
            }
            if (!(stateOld.Gamepad.Buttons == GamepadButtonFlags.DPadLeft) && stateNew.Gamepad.Buttons == GamepadButtonFlags.DPadLeft)
            {
                //Console.WriteLine("DPadLeft!");
                SendKeycode(VK_LEFT);
            }
            if (!(stateOld.Gamepad.Buttons == GamepadButtonFlags.DPadUp) && stateNew.Gamepad.Buttons == GamepadButtonFlags.DPadUp)
            {
                Console.WriteLine("DPadUp!");
                SendKeycode(VK_UP);
            }
            if (!(stateOld.Gamepad.Buttons == GamepadButtonFlags.DPadRight) && stateNew.Gamepad.Buttons == GamepadButtonFlags.DPadRight)
            {
                //Console.WriteLine("DPadRight!");
                SendKeycode(VK_RIGHT);
            }

            ////a left click
            //if (stateOld.Gamepad.Buttons == GamepadButtonFlags.LeftShoulder && stateNew.Gamepad.Buttons == GamepadButtonFlags.LeftShoulder)
            //{
            //    LeftClick();
            //}

            ////b right click
            //if (stateOld.Gamepad.Buttons == GamepadButtonFlags.RightShoulder && stateNew.Gamepad.Buttons == GamepadButtonFlags.RightShoulder)
            //{
            //    RightClick();
            //}

            ////dpad up button sens up
            //if (stateOld.Gamepad.Buttons == GamepadButtonFlags.DPadUp && stateNew.Gamepad.Buttons == GamepadButtonFlags.DPadUp)
            //{
            //    sensnud.UpButton();
            //}

            ////dpad down button sens down
            //if (stateOld.Gamepad.Buttons == GamepadButtonFlags.DPadDown && stateNew.Gamepad.Buttons == GamepadButtonFlags.DPadDown)
            //{
            //    sensnud.DownButton();
            //}
            stateOld = stateNew;
        }

        //private void LeftClick()
        //{
        //    int X = Cursor.Position.X;
        //    int Y = Cursor.Position.Y;
        //    mouse_event(0x02 | 0x04, (uint)X, (uint)Y, 0, 0);
        //    Thread.Sleep(100);
        //}

        //private void RightClick()
        //{
        //    int X = Cursor.Position.X;
        //    int Y = Cursor.Position.Y;
        //    mouse_event(0x08 | 0x10, (uint)X, (uint)Y, 0, 0);
        //    Thread.Sleep(100);
        //}

        //private void updateLoop_Tick(object sender, EventArgs e)
        //{
        //    xlbl.Text = "Offset X: " + x;
        //    ylbl.Text = "Offset Y: " + y;
        //}

        //private void applybtn_Click(object sender, EventArgs e)
        //{
        //    loop.Interval = (int)updatenud.Value;
        //}
    }
}