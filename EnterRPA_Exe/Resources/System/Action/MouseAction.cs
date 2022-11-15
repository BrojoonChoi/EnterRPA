using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
 
namespace MouseActionNameSpace
{
    public class MouseAction
    { 
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        private const uint LBDOWN = 0x0002; // 왼쪽 마우스 버튼 눌림
        private const uint LBUP = 0x0004; // 왼쪽 마우스 버튼 떼어짐
        private const uint RBDOWN = 0x00000008; // 오른쪽 마우스 버튼 눌림
        private const uint RBUP = 0x000000010; // 오른쪽 마우스 버튼 떼어짐
        private const uint MBDOWN = 0x00000020; // 휠 버튼 눌림
        private const uint MBUP = 0x000000040; // 휠 버튼 떼어짐
        private const uint WHEEL = 0x00000800; //휠 스크롤
        private const uint ABSOLUTEMOVE = 0x8000;

        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);
        [DllImport("user32")]
        static extern bool GetCursorPos(ref Point point);
        
        public void LeftClick()
        {
            Thread.Sleep(50);
            mouse_event(LBDOWN, 0, 0, 0, 0);
            Thread.Sleep(5);
            mouse_event(LBUP, 0, 0, 0, 0);
            Thread.Sleep(50);
        }
        
        public void LeftDown()
        {
            Thread.Sleep(50);
            mouse_event(LBDOWN, 0, 0, 0, 0);
            Thread.Sleep(50);
        }
        
        public void LeftUp()
        {
            Thread.Sleep(50);
            mouse_event(LBUP, 0, 0, 0, 0);
            Thread.Sleep(50);
        }
        
        public void RightClick()
        {
            Thread.Sleep(50);
            mouse_event(RBDOWN, 0, 0, 0, 0);
            Thread.Sleep(5);
            mouse_event(RBUP, 0, 0, 0, 0);
            Thread.Sleep(50);
        }

        public void RightDown()
        {
            Thread.Sleep(50);
            mouse_event(RBDOWN, 0, 0, 0, 0);
            Thread.Sleep(50);
        }

        public void RightUp()
        {
            Thread.Sleep(50);
            mouse_event(RBUP, 0, 0, 0, 0);
            Thread.Sleep(50);
        }

        public void LeftDrag(Point pStart, Point pEnd)
        {
            Thread.Sleep(50);
            SetPoint(pStart);
            mouse_event(LBDOWN, 0, 0, 0, 0);
            SetPoint(pEnd);
            mouse_event(LBUP, 0, 0, 0, 0);
            Thread.Sleep(50);
        }

        public void RightDrag(Point pStart, Point pEnd)
        {
            Thread.Sleep(50);
            SetPoint(pStart);
            mouse_event(RBDOWN, 0, 0, 0, 0);
            SetPoint(pEnd);
            mouse_event(RBUP, 0, 0, 0, 0);
            Thread.Sleep(50);
        }

        public string GetPointString ()
        {
            Point pt = new Point();
            GetCursorPos(ref pt);
            
            return pt.ToString();
        }
                
        public Point GetPoint ()
        {
            Point pt = new Point();
            GetCursorPos(ref pt);
            return pt;
        }


        public void RecordPoint ()
        {
            IO_NameSpace.IO.Instance().Record(GetPointString());
        }

        public bool SetPoint (int pX, int pY)
        {
            Thread.Sleep(50);
            try
            {
                SetCursorPos(pX, pY);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetPointRef (int pX, int pY)
        {
            Thread.Sleep(50);
            try
            {
                Point cPos = GetPoint();
                SetCursorPos(cPos.X + pX, cPos.Y + pY);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public bool SetPoint (Point pPoint)
        {
            Thread.Sleep(50);
            try
            {
                SetCursorPos(pPoint.X, pPoint.Y);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}