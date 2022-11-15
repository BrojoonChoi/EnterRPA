using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
 
namespace KeyboardActionNameSpace
{
    public class KeyboardAction : Form
    {
        [DllImport("user32.dll")]
        static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        const uint KEYDOWN = 0x1;
        const uint KEYUP = 0x2;

        public bool Type (string pString)
        {
            Thread.Sleep(50);
            SendKeys.SendWait("");
            switch (pString)
            {
                case "Enter" :
                    SendKeys.SendWait("{ENTER}");
                    break;
                case "Tab" :
                    SendKeys.SendWait("{TAB}");
                    break;
                case "F1" :
                    SendKeys.SendWait("{F1}");
                    break;
                case "F2" :
                    SendKeys.SendWait("{F2}");
                    break;
                case "F3" :
                    SendKeys.SendWait("{F3}");
                    break;
                case "F4" :
                    SendKeys.SendWait("{F4}");
                    break;
                case "F5" :
                    SendKeys.SendWait("{F5}");
                    break;
                case "F6" :
                    SendKeys.SendWait("{F6}");
                    break;
                case "F7" :
                    SendKeys.SendWait("{F7}");
                    break;
                case "F8" :
                    SendKeys.SendWait("{F8}");
                    break;
                case "F9" :
                    SendKeys.SendWait("{F9}");
                    break;
                case "F10" :
                    SendKeys.SendWait("{F10}");
                    break;
                case "F11" :
                    SendKeys.SendWait("{F11}");
                    break;
                case "F12" :
                    SendKeys.SendWait("{F12}");
                    break;
                case "Down" :
                    SendKeys.SendWait("{Down}");
                    break;
                case "Up" :
                    SendKeys.SendWait("{Up}");
                    break;
                case "Left" :
                    SendKeys.SendWait("{Left}");
                    break;
                case "Right" :
                    SendKeys.SendWait("{Right}");
                    break;
                case "^+F9" :
                    SendKeys.SendWait("^+{F9}");
                    break;
                default :
                    for (int i = 0; i < pString.Length; i++)
                    {
                        SendKeys.SendWait(pString[i].ToString());
                        Thread.Sleep(1);
                    }
                    break;
            }

            return true;
        }
        /*
        public void Test (string pString)
        {
            for (int i = 0; i < pString.Length; i++)
            {
                keybd_event(, 0x45, 0x2, UIntPtr.Zero);
                Thread.Sleep(10);
                keybd_event((byte)pString[i], 0x45, 0x2, UIntPtr.Zero);
            }
        }
        */
    }
}