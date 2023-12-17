using System;
using System.Drawing;
using System.Threading;
using SharpDX.XInput;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FortniteJammer
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
            stateNew = controller.GetState();
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
            stateOld = stateNew;
        }
    }
}