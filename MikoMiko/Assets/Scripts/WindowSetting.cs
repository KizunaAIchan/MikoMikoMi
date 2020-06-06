using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WindowSetting : MonoBehaviour
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
    public static extern int GetWindowText(IntPtr hWnd,ref char[] lpString, int nMaxCount);

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
    private const uint WS_VISIBLE = 0x10000000;
    private const int ULW_COLORKEY = 0x00000001;
    private const int ULW_ALPHA = 0x00000002;
    private const int ULW_OPAQUE = 0x00000004;
    private const int ULW_EX_NORESIZE = 0x00000008;
    private const uint WS_EX_TOPMOST = 0x00000008;
    #endregion


    public static WindowSetting instance = null;

    public int winWidth;
    public int winHeight;

    //For test in Editor
    public bool isDebug = false;


    private IntPtr _hwnd;
    private bool _canDrag = false;
    private bool _StartDrag = false;
    private bool readyToDrag = false;
    public bool canLook = false;
    private Vector3 _lastMousePositon = Vector3.zero;
    private Vector3 _lastMousePositonV2 = Vector3.zero;
    private float lastpressTime = -1f;

    public enum WindowTopType{
        HWND_TOPMOST = -1,
        HWND_NOTOPMOST = -2,
    }

    public WindowTopType currentTopType = WindowTopType.HWND_TOPMOST;


    public enum SwipeType
    {
        Left,
        Right,
        None,
    }

    private SwipeType currentSwipeType = SwipeType.None;

    void Awake()
    {
        instance = this;

        _hwnd = FindWindow(null, UnityEngine.Application.productName);
        //var _windowHandle = FindWindow(null, UnityEngine.Application.productName);
        //Debug.Log(_hwnd.ToString() + "    " + _windowHandle.ToString() + "    " + UnityEngine.Application.productName);

      //  Debug.Log();
        if (UnityEngine.Application.isEditor)
            return;

      
        UnityEngine.Screen.fullScreen = false;

        SetWindowLong(_hwnd, GWL_EXSTYLE, WS_EX_LAYERED);
        int intExTemp = GetWindowLong(_hwnd, GWL_EXSTYLE);

        SetWindowLong(_hwnd, GWL_EXSTYLE, intExTemp | WS_EX_TRANSPARENT | WS_EX_LAYERED);

        SetWindowLong(_hwnd, GWL_STYLE, GetWindowLong(_hwnd, GWL_STYLE) & ~WS_BORDER & ~WS_CAPTION);

        int currentX = UnityEngine.Screen.currentResolution.width / 2 - winWidth / 2;
        int currentY = UnityEngine.Screen.currentResolution.height / 2 - winHeight / 2;

        SetWindowPos(_hwnd, (int)currentTopType, currentX, currentY, winWidth, winHeight, SWP_SHOWWINDOW);

        var margins = new MARGINS() { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(_hwnd, ref margins);






    }

    public void Update()
    {
        //if (Input.touchCount > 0)
        //{
        //    Debug.Log(Input.GetTouch(0).phase);
        //}
        RaycastHit hitinfo;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo, 10000f, LayerMask.GetMask("TouchArea"))
         && !IsPointOnUI(Input.mousePosition))
        {
            CheckSwipeType();

        }
        else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo, 10000f, LayerMask.GetMask("ClickArea"))
         && !IsPointOnUI(Input.mousePosition))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!MikoChi.instance.animationComponent.isPlaying)
                    MikoChi.instance.PlayAnimator("Shy");
            }
           
        }


        

        RECT rect = new RECT();
        GetWindowRect(_hwnd, ref rect);

        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
           
        //    var x = rect.Left;
        //    var y = rect.Top;

        //}

        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //  //  RECT rect = new RECT();
        //    GetWindowRect(_hwnd, ref rect);
        //    var x = rect.Left;
        //    var y = rect.Top;
        //    SetWindowPos(_hwnd, -1, --x, y, winWidth, winHeight, SWP_SHOWWINDOW);

        //}
    }

    public void LateUpdate()
    {
  
        if (Input.GetMouseButtonUp(1))
        {
            if (!UIManager.instance.IsAlive(UINames.configPage)
                && !UIManager.instance.IsAlive(UINames.AvgEditor))
            {
                var menu = UIManager.instance.ShowUI<UI_RightClickMenu>(UINames.rightClickMenu);
                menu.Init();
                menu.transform.localPosition = new Vector3(0, 25, 0);
            }

            lastpressTime = -1f;
        }

    
        //if (Input.GetMouseButtonUp(0))
        //{
        //    UIManager.instance.CloseUIByName(UINames.rightClickMenu);
        //}

        RaycastHit hitinfo1;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo1, 10000f, LayerMask.GetMask("LookMouse"))
           && !IsPointOnUI(Input.mousePosition))
        {
            canLook = true;
        }
        else
        {
            canLook = false;
        }
        CheckDrag();


        if (UnityEngine.Application.isEditor)
            return;
        CheckPosition();
        if (_canDrag)
            CheckDrag();
    }

    public void CheckPosition()
    {
        var pos = GetMousePos();
        var screenPos = new Vector3(pos.x, pos.y, 0);
        if (Vector3.Distance(screenPos, _lastMousePositon) < 1)
        {

            return;
        }

        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPos), out hitInfo, 10000f, LayerMask.GetMask("WindowRect"))
            || IsPointOnUI(screenPos))
        {
            if (!_canDrag)
            {
                var s = GetWindowLong(_hwnd, GWL_EXSTYLE);
                SetWindowLong(_hwnd, GWL_EXSTYLE, (uint)(s & ~WS_EX_TRANSPARENT));
                _canDrag = true;
            }
        }
        else
        {
            if (_canDrag)
            {
                var s = GetWindowLong(_hwnd, GWL_EXSTYLE);
                SetWindowLong(_hwnd, GWL_EXSTYLE, (uint)(s | WS_EX_TRANSPARENT));
                _canDrag = false;
            }
        }

        _lastMousePositon = screenPos;


    }

    //Window Screen Coordinate To Unity Screen Coordinate
    //Window Screen  Top Left   Unity Screen Bottom Left
    public Vector2 GetMousePos()
    {
        RECT rect = new RECT();
        GetWindowRect(_hwnd, ref rect);
        var pos = System.Windows.Forms.Cursor.Position;
        Vector2Int leftBottom = new Vector2Int(rect.Left, rect.Bottom);
        var mousePos = new Vector2Int(pos.X, pos.Y);
        var screenHeight = SystemInformation.PrimaryMonitorSize.Height;
        leftBottom.y = screenHeight - leftBottom.y;
        mousePos.y = screenHeight - mousePos.y;
        return mousePos - leftBottom;
    }


    //This function will throw an error but nothing happend

    //issue
    //An abnormal situation has occurred : 
    //the PlayerLoop internal function has been called recursively.
        
    //BaseThreadInitThunk
    //rtlGetAppContainerNamedObjectPath
    public void DragWindow()
    {
        ReleaseCapture();
        if (_hwnd == null)
            return;
        SendMessage(_hwnd, 0xA1, 0x02, 0);
        SendMessage(_hwnd, 0x0202, 0, 0);
    }



    public static bool IsPointOnUI(Vector3 pos)
    {

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = pos;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        return raycastResults.Count > 0;

    }

    public void DoMoveWindow(int x, int y)
    {
        SetWindowPos(_hwnd, -1, x, y, winWidth, winHeight, SWP_SHOWWINDOW);

    }

    public RECT GetCurrentWindowPos()
    {
        RECT rect = new RECT();
        GetWindowRect(_hwnd, ref rect);
        return rect;
    }

    public void SetWindowTop(bool top)
    {
        if (UnityEngine.Application.isEditor)
            return;
        RECT rect = new RECT();
        GetWindowRect(_hwnd, ref rect);
        var x = rect.Left;
        var y = rect.Top;
        if (top)
            SetWindowPos(_hwnd, -1, x, y, winWidth, winHeight, SWP_SHOWWINDOW);
        else
            SetWindowPos(_hwnd, -2, x, y, winWidth, winHeight, SWP_SHOWWINDOW);

        currentTopType = top ? WindowTopType.HWND_TOPMOST : WindowTopType.HWND_NOTOPMOST;

    }

    public TheWorld theworld;
    private List<Vector3> pointsList = new List<Vector3>();
    public void CheckSwipeType()
    {
        if (Input.GetMouseButtonUp(0))
        {
            pointsList.Clear();

            currentSwipeType = SwipeType.None;
        }

        if (Input.GetMouseButton(0))
        {
            pointsList.Add(Input.mousePosition);

            if (pointsList.Count >= 5)
            {
                int a = 0;
                int g = 0;
                for (int i = 1; i < pointsList.Count; ++i)
                {
                    if (Mathf.Abs(pointsList[i].y - pointsList[i - 1].y) >= 10)
                    {
                        currentSwipeType = SwipeType.None;

                        pointsList.Clear();
                        break;
                    }

                    if (pointsList[i].x < pointsList[i - 1].x)
                    {
                        a++;
                    }

                    if (pointsList[i].x > pointsList[i - 1].x)
                    {
                        a--;
                    }

                    if (pointsList[i].x == pointsList[i - 1].x)
                        g++;
                }

                if (Mathf.Abs(a) + g + 1 == pointsList.Count
                    && a != 0)
                {
                    var p = a > 0 ? SwipeType.Left : SwipeType.Right;
                    if ((p == SwipeType.Right && currentSwipeType == SwipeType.Left)
                        || (p == SwipeType.Left && currentSwipeType == SwipeType.Right))
                    {
                        Debug.Log("Swipe");
                        p = SwipeType.None;
                        if (!MikoChi.instance.animationComponent.isPlaying)
                        {
                            MikoChi.instance.AddLove(1, AddLoveType.Touch);
                            MikoChi.instance.PlayAnimator("Shyv2");
                        }
                    }
                    currentSwipeType = p;

                }

                pointsList.Clear();
            }
        }
    }

    public void CheckDrag()
    {

        float t = Time.realtimeSinceStartup;

        if (Input.GetMouseButtonDown(0))
        {
            _lastMousePositonV2 = Input.mousePosition;
            lastpressTime = t;
            readyToDrag = false;
            _StartDrag = false;

        }
        if (Input.GetMouseButtonUp(0))
        {
           // if (readyToDrag)
                theworld.theWorld(false);
            _StartDrag = false;
            readyToDrag = false;

            lastpressTime = -1;
        }

        if (Input.GetMouseButton(0))
        {

            if (readyToDrag && Vector3.Distance(Input.mousePosition, _lastMousePositonV2) > 1)
            {
                _StartDrag = true;
                DragWindow(); 
            }


            if (Vector3.Distance(Input.mousePosition, _lastMousePositonV2) > 1 && !_StartDrag)
            {
                lastpressTime = t;
                readyToDrag = false;

                _lastMousePositonV2 = Input.mousePosition;

            }
            else if (t - lastpressTime > 1 && lastpressTime > 0 && !IsPointOnUI(Input.mousePosition))
            {
                if (!readyToDrag)
                    theworld.theWorld(true);
                readyToDrag = true;
              

            }
        }
    }

    //public Material m_Material;
    //void OnRenderImage(RenderTexture from, RenderTexture to)
    //{
    //    Graphics.Blit(from, to, m_Material);
    //}
}