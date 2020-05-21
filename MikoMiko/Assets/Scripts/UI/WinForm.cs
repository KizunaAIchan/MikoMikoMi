using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
using System;
using System.Runtime.InteropServices;

public class WinForm : Singleton<WinForm>
{
    #region Win函数常量
    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left; //Positon Left
        public int Top; //Positon Top
        public int Right; //Positon Right
        public int Bottom; //Positon Bottom
    }
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

    [DllImport("user32.dll")]
    static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, int bAlpha, int dwFlags);

    [DllImport("Dwmapi.dll")]
    static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

    private const int WS_POPUP = 0x800000;
    private const int GWL_EXSTYLE = -20;
    private const int GWL_STYLE = -16;
    private const int WS_EX_LAYERED = 0x00080000;
    private const int WS_BORDER = 0x00800000;
    private const int WS_CAPTION = 0x00C00000;
    private const int SWP_SHOWWINDOW = 0x0040;
    private const int LWA_COLORKEY = 0x00000001;
    private const int LWA_ALPHA = 0x00000002;
    private const int WS_EX_TRANSPARENT = 0x20;

    private const int ULW_COLORKEY = 0x00000001;
    private const int ULW_ALPHA = 0x00000002;
    private const int ULW_OPAQUE = 0x00000004;
    private const int ULW_EX_NORESIZE = 0x00000008;
    #endregion



    public void Show()
    {
        Form form = new Form();

        form.Show();
    }

    public void InitForm()
    {

    }

    public override void Init()
    {
    }
}
