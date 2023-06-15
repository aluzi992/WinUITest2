// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIWndProc
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private IntPtr _oldWndProc;
        private AppWindow? ThisWindow;
        private IntPtr m_hwnd;

        private const int GWLP_WNDPROC = -4;
        public delegate IntPtr WndProcDelegate(IntPtr hwnd, uint message, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")] //32-bit
        public static extern IntPtr SetWindowLong32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")] // 64-bit
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        private Point mousePoint = new Point();
        private const int WM_COPYDATA = 0x004A;
        private const int WM_NCCALCSIZE = 0x0083;
        private const int WM_NCPAINT = 133;
        private const int WM_NCACTIVATE = 0x0086;
        private const int WM_ACTIVATE = 0x0006;
        private const int WM_NCHITTEST = 0x0084;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int WS_MAXIMIZEBOX = 0x10000;
        private const int WS_EX_LAYERED = 0x00080000;
        private const int WS_CAPTION = 0xC00000;
        private const int WS_SYSMENU = 0x80000;
        private const int GWL_STYLE = -16;
        private const int GWL_EXSTYLE = -20;


        [StructLayout(LayoutKind.Sequential)]
        public struct CWPSTRUCT
        {
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            internal RECT(int X, int Y, int Width, int Height)
            {
                this.Left = X;
                this.Top = Y;
                this.Right = Width;
                this.Bottom = Height;
            }
            internal int Left;
            internal int Top;
            internal int Right;
            internal int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NCCALCSIZE_PARAMS
        {
            internal RECT rect0, rect1, rect2;
            internal IntPtr lppos;
        }

        public enum HitTest : int
        {
            HTERROR = -2,
            HTTRANSPARENT = -1,
            HTNOWHERE = 0,
            HTCLIENT = 1,
            HTCAPTION = 2,
            HTSYSMENU = 3,
            HTGROWBOX = 4,
            HTSIZE = HTGROWBOX,
            HTMENU = 5,
            HTHSCROLL = 6,
            HTVSCROLL = 7,
            HTMINBUTTON = 8,
            HTMAXBUTTON = 9,
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17,
            HTBORDER = 18,
            HTREDUCE = HTMINBUTTON,
            HTZOOM = HTMAXBUTTON,
            HTSIZEFIRST = HTLEFT,
            HTSIZELAST = HTBOTTOMRIGHT,
            HTOBJECT = 19,
            HTCLOSE = 20,
            HTHELP = 21,
        }

        public MainWindow()
        {
            this.InitializeComponent();
            m_hwnd = WindowNative.GetWindowHandle(this);
            SetWindowLong(m_hwnd, GWL_STYLE, (uint)(GetWindowLong(m_hwnd, GWL_STYLE) & ~WS_CAPTION & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX & ~WS_SYSMENU));
            //SetWindowLong(m_hwnd, GWL_EXSTYLE, WS_EX_LAYERED);

            WindowId id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(m_hwnd);
            ThisWindow = AppWindow.GetFromWindowId(id);
            MainWindow.WndProc._coreWindowHwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            _oldWndProc = MainWindow.WndProc.SetWndProc(WindowProcess);
            ThisWindow.Resize(new Windows.Graphics.SizeInt32(550, 300));
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";
        }

        private IntPtr WindowProcess(IntPtr hwnd, uint message, IntPtr wParam, IntPtr lParam)
        {
            //Debug.WriteLine(hwnd);
            //if (hwnd != m_hwnd)
            //    return CallWindowProc(_oldWndProc, hwnd, message, wParam, lParam);

            switch (message)
            {
                case WM_NCPAINT:
                    Debug.WriteLine("NCPAINT skiped....");
                    return IntPtr.Zero;
                //case WM_NCACTIVATE:
                //    Debug.WriteLine("WM_NCACTIVATE running.......");
                //    return (IntPtr)1;
                //case WM_ACTIVATE:
                //    InvalidateRect(hwnd, IntPtr.Zero, true);
                //    break;
                case WM_NCHITTEST:
                    this.mousePoint.X = (lParam.ToInt32() & 0xFFFF);
                    this.mousePoint.Y = (lParam.ToInt32() >> 16);
                    Debug.WriteLine($"Received message NCHITTEST:{this.mousePoint.X} {this.mousePoint.Y} **Window Position:{ThisWindow!.Position.X},{ThisWindow!.Position.Y} Size:{ThisWindow!.Size.Width}X{ThisWindow!.Size.Height}");
                    if (this.mousePoint.X <= ThisWindow!.Position.X + ThisWindow.Size.Width && this.mousePoint.Y < ThisWindow.Position.Y + 48)
                    {
                        Debug.WriteLine("Return HTCAPTION");
                        return new IntPtr((int)HitTest.HTCAPTION);
                    }
                    else
                        return new IntPtr((int)HitTest.HTCLIENT);
                case WM_NCCALCSIZE:
                    if (wParam != IntPtr.Zero)
                    {
                        NCCALCSIZE_PARAMS nc = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(lParam, typeof(NCCALCSIZE_PARAMS));
                        Debug.WriteLine($"---------------------{nc.rect0.Left} {nc.rect0.Top} {nc.rect0.Right} {nc.rect0.Bottom}");
                        //Debug.WriteLine($"---------------------{nc.rect1.Left} {nc.rect1.Top} {nc.rect1.Right} {nc.rect1.Bottom}");
                        //Debug.WriteLine($"---------------------{nc.rect2.Left} {nc.rect2.Top} {nc.rect2.Right} {nc.rect2.Bottom}");
                        //nc.rect0.Top -= 20;
                        //var border = nc.rect2.Left - nc.rect0.Left;
                        //nc.rect0.Top += 1;
                        nc.rect0.Bottom -= 1;
                        //nc.rect0.Left += border;
                        //nc.rect0.Right -= border;
                        nc.rect1 = nc.rect0;
                        Marshal.StructureToPtr(nc, lParam, false);
                        return (IntPtr)0x0400;
                    }
                    else
                        return IntPtr.Zero;
            }
            return MainWindow.Interop.CallWindowProc(_oldWndProc, hwnd, message, wParam, lParam);
        }

        [ComImport, Guid("45D64A29-A63E-4CB6-B498-5781D298CB4F")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface ICoreWindowInterop
        {
            IntPtr WindowHandle { get; }
            bool MessageHandled { get; }
        }
        public class Interop
        {
            [DllImport("user32.dll", EntryPoint = "SetWindowLong")] //32-bit
            public static extern IntPtr SetWindowLong32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
            [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")] // 64-bit
            public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);
        }
        public static class WndProc
        {
            public delegate IntPtr WndProcDelegate(IntPtr hwnd, uint message, IntPtr wParam, IntPtr lParam);
            private const int GWLP_WNDPROC = -4;
            public static IntPtr _coreWindowHwnd;
            private static WndProcDelegate _currDelegate = null;
            public static IntPtr SetWndProc(WndProcDelegate newProc)
            {
                _currDelegate = newProc;
                IntPtr newWndProcPtr = Marshal.GetFunctionPointerForDelegate(newProc);
                if (IntPtr.Size == 8)
                {
                    return Interop.SetWindowLongPtr64(_coreWindowHwnd, GWLP_WNDPROC, newWndProcPtr);
                }
                else
                {
                    return Interop.SetWindowLong32(_coreWindowHwnd, GWLP_WNDPROC, newWndProcPtr);
                }
            }
        }
    }
}
