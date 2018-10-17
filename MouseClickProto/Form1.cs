using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MouseClickProto
{
    public partial class Form1 : Form
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        [DllImport("user32.dll",
           CharSet = CharSet.Auto,
           CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(long dwFlags,
                                              long dx,
                                              long dy,
                                              long cButtons,
                                              long dwExtraInfo);

        [DllImport("user32.dll",
             CharSet = CharSet.Auto,
           CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            //bool success = User32.GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }


        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        public void Click(Point pt)
        {
            Cursor.Position = pt;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, pt.X, pt.Y, 0, 0);
        }

        Point pt;
        private Timer timer1;
        private long ClickCount;
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = MouseClickProto.Properties.Settings.Default.ClickDelayinSec * 1000; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ClickCount++;
            Click( pt);

            MouseClickProto.Properties.Settings.Default.GlobalClickCount = ClickCount.ToString();
            Update();
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            pt = GetCursorPosition();
            InitTimer();
        }
    }
}
