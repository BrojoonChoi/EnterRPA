using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Threading;
using System.Data;

namespace IO_NameSpace
{
    public class IO
    {
        public WebHelper webHelper;
        OfficeHelper.OfficeHelper officeHelper;
        string mDirectory = "";
        string mPythonPath = "";
        
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private Queue<string> actionQueue = new Queue<string>();
        private Stack<Queue<string>> subActionQueue = new Stack<Queue<string>>();
        private Queue<string> recordQueue = new Queue<string>();
        private Dictionary<string, string> variables = new Dictionary<string, string>();
        private string lastResult;
        private static IO? master;
        public static IO Instance()
        {
            if (master == null)
            {
                master = new IO();
            }
            return master;
        }

        public void init(string pPython)
        {
            webHelper = new WebHelper();
            officeHelper = new OfficeHelper.OfficeHelper();
            mPythonPath = pPython;
            LoadAction();
            Action();
        }
        
        public void init(string pPython, string pPath)
        {
            webHelper = new WebHelper();
            officeHelper = new OfficeHelper.OfficeHelper();
            mPythonPath = pPython;
            LoadDefaultAction(pPath);
            Action();
        }

        public void Record(string pString)
        {
            StringBuilder stb = new StringBuilder();
            pString = isVariable(pString);
            stb.Append(DateTime.Now.ToString());
            stb.Append(" : ");
            stb.Append(pString);
            stb.Append("\n");
            try
            {
                if(!File.Exists(mDirectory + "log.txt"))
                    File.Create(mDirectory + "log.txt");
                File.AppendAllText(mDirectory + "log.txt", stb.ToString());
                Console.WriteLine(stb.ToString());
            }
            catch
            {

            }
        }

        private void LoadAction()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Data File (*.dat)|*.dat|Text File (*.txt)|*.txt|All File (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] tempString = File.ReadAllLines(ofd.FileName);
                Queue<string> tempQ = new Queue<string>();
                for (int i = 0; i < tempString.Length; i++)
                {
                    tempQ.Enqueue(tempString[i].TrimStart());
                }
                subActionQueue.Push(tempQ);
            }
            mDirectory = ofd.FileName.Substring(0, ofd.FileName.LastIndexOf("\\") + 1);
        }
        
        private void LoadDefaultAction(string pPath)
        {
            string[] tempString = File.ReadAllLines(pPath);
            Queue<string> tempQ = new Queue<string>();
            for (int i = 0; i < tempString.Length; i++)
            {
                tempQ.Enqueue(tempString[i].TrimStart());
            }
            subActionQueue.Push(tempQ);
            mDirectory = pPath.Substring(0, pPath.LastIndexOf("\\") + 1);
        }

        /*
        private void Action()
        {
            string action;
            string[] order;
            while(actionQueue.Count != 0)
            {
                action = actionQueue.Dequeue();
                order = action.Split(":");

                Decode(order);

                Wait(50);
            }
        }
        */

        private void Action()
        {
            string action;
            string[] order;
            while(subActionQueue.Count > 0)
            {
                actionQueue = subActionQueue.Pop();
                while(actionQueue.Count > 0)
                {
                    action = actionQueue.Dequeue();
                    order = action.Split(":::");

                    Decode(order);

                    Wait(50);
                }
            }
        }

        private void Action(string pDir)
        {
            string action;
            string[] order;
            while(subActionQueue.Count > 0)
            {
                actionQueue = subActionQueue.Pop();
                while(actionQueue.Count > 0)
                {
                    action = actionQueue.Dequeue();
                    order = action.Split(":::");

                    Decode(order);

                    Wait(50);
                }
                mDirectory = pDir;
            }
        }

        private void Decode(string[] order)
        {
            string syntax = "";
            int count = 1;

            switch (order[0])
                {
                    case ("Print") :
                        Record(order[1]);
                        return;
                    case ("Wait") :
                        Wait(order[1]);
                        return;
                    case ("WaitImage") :
                        WaitImage(order[1], order[2]);
                        return;
                    case ("Set") :
                        if(order.Length == 2)
                        {
                            SetVariable(order[1]);
                            return;
                        }
                        SetVariable(order[1], order[2]);
                        return;
                    case ("Get") :
                        GetVariable(order[1]);
                        return;
                    case ("Mouse") :
                        if (order.Length == 4)
                            Mouse(order[1], order[2], order[3]);
                        Mouse(order[1]);
                        return;
                    case ("Keyboard") :
                        Keyboard(order[1]);                        
                        return;
                    case ("If") :
                        List<string> tempIF = new List<string>();
                        Queue<string> tempSubIF = new Queue<string>();

                        if(If(order[1],order[2],order[3],order[4]))
                        {
                            Record(syntax);
                            count = 1;
                            while (count > 0)
                            {
                                syntax = actionQueue.Peek();
                                if (syntax.Contains("If:::"))
                                    count++;
                                if (syntax.CompareTo("TRUE_END") == 0)
                                    count--;
                                tempIF.Add(actionQueue.Dequeue());
                            }

                            for (int j = 0; j < tempIF.Count; j++)
                            {
                                tempSubIF.Enqueue(tempIF[j]);
                            }

                            subActionQueue.Push(actionQueue);
                            subActionQueue.Push(tempSubIF);

                            syntax = actionQueue.Peek();
                            while (syntax.CompareTo("FALSE_END") != 0)
                            {
                                actionQueue.Dequeue();
                                syntax = actionQueue.Peek();
                            }

                            Action();
                        }
                        else
                        {
                            count = 2;
                            while (count > 1)
                            {
                                syntax = actionQueue.Peek();
                                if (syntax.Contains("If:::"))
                                    count++;
                                if (syntax.CompareTo("TRUE_END") == 0)
                                    count--;

                                actionQueue.Dequeue();
                            }

                            count = 1;
                            while (count > 0)
                            {
                                syntax = actionQueue.Peek();
                                if (syntax.Contains("If:::"))
                                    count++;
                                if (syntax.CompareTo("FALSE_END") == 0)
                                    count--;
                                tempIF.Add(actionQueue.Dequeue());
                            }

                            for (int j = 0; j < tempIF.Count; j++)
                            {
                                tempSubIF.Enqueue(tempIF[j]);
                            }

                            subActionQueue.Push(actionQueue);
                            subActionQueue.Push(tempSubIF);

                            Action();
                        }
                        return;
                    case ("For") :
                        int length = int.Parse(order[1]);
                        List<string> temp = new List<string>();
                        Queue<string> tempSub = new Queue<string>();

                        count = 1;
                        while (count > 0)
                        {
                            syntax = actionQueue.Peek();
                            if (syntax.Contains("For:::"))
                                count++;
                            if (syntax.CompareTo("FOR_END") == 0)
                                count--;
                            temp.Add(actionQueue.Dequeue());
                        }

                        for (int i = 0; i < length; i++)
                        {
                            for (int j = 0; j < temp.Count; j++)
                            {
                                tempSub.Enqueue(temp[j]);
                            }
                        }

                        subActionQueue.Push(actionQueue);
                        subActionQueue.Push(tempSub);

                        Action();
                        return;
                    case ("ReadOCR") :
                        ReadOCR(int.Parse(order[1]), int.Parse(order[2]), int.Parse(order[3]), int.Parse(order[4]), order[5]);
                        return;
                    case ("Math") :
                        MathDecode(order[1], order[2], order[3] ,order[4]);
                        break;
                    case ("Calendar") :
                        MathDecode(order[1], order[2], order[3] ,order[4]);
                        break;
                    case ("GPortalLogin") :
                        GPortalLogin(order[1], order[2]);
                        break;
                    case ("GDWFind") :
                        GDWFind(order[1]);
                        break;
                    case ("GDWQuery") :
                        GDWQuery();
                        break;
                    case ("GDWDownload") :
                        GDWDownload(order[1]);
                        break;
                    case ("GDWQuit") :
                        GDWQuit();
                        break;
                    case ("WaitImageIf") :
                        WaitImageIf(order[1]);
                        break;
                    case ("GDWQueryWait") :
                        WaitImageIf("GDW_Wait.png");
                        break;
                    case ("PythonExe") :
                        PythonExe(order[1]);
                        break;
                    case ("ERPLogin") :
                        ERPLogin(order[1]);
                        break;
                    case ("ERPSearch") :
                        ERPSearch(order[1]);
                        break;
                    case ("SendEmail") :
                        SendEmail(order[1], order[2], order[3], order[4], order[5]);
                        break;
                    case ("ExcelFileOpen") :
                        ExcelFileOpen(order[1]);
                        break;
                    case ("ExcelReadCell") :
                        ExcelReadCell(order[1], order[2], order[3], order[4]);
                        break;
                    case ("ExcelWriteCell") :
                        ExcelWriteCell(order[1], order[2], order[3], order[4]);
                        break;
                    case ("DAVINCILogin") :
                        DAVINCILogin(order[1], order[2], order[3]);
                        break;
                    case ("DAVINCIDownload") :
                        DAVINCIDownload(order[1]);
                        break;
                    case ("WebMove") :
                        webHelper.WebMove(order[1]);
                        break;
                    case ("WebClick") :
                        webHelper.WebClick(order[1], order[2], order[3]);
                        break;
                }
        }

        private string isVariable (string pString)
        {
            if (pString.Contains("Get/"))
            {
                pString = GetVariable(pString.Substring(4));
            }
            return pString;
        }

        private void Wait(string pTime)
        {
            Thread.Sleep(int.Parse(pTime));
        }
        
        private void Wait(int pTime)
        {
            Thread.Sleep(pTime);
        }
        
        private bool SetVariable(string pVarName)
        {
            try
            {
                if (variables.ContainsKey(pVarName))
                {
                    variables.Add(pVarName, lastResult);
                }
                else
                {
                    variables[pVarName] = lastResult;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool SetVariable(string pVarName, string pVarValue)
        {
            if (variables.ContainsKey(pVarName))
            {
                variables.Add(pVarName, pVarValue);
            }
            else
            {
                variables[pVarName] = pVarValue;
            }
            return true;
        }

        private string GetVariable(string pVarName)
        {
            try
            {
                return variables[pVarName];
            }
            catch
            {
                Record("Getting Error");
                return "";
            }
        }

        private void WaitImage(string pPath, string pTry)
        {
            ImageCompareNameSpace.ImageCompare imgc = new ImageCompareNameSpace.ImageCompare();
            MouseActionNameSpace.MouseAction mouse = new MouseActionNameSpace.MouseAction();

            Bitmap target;

            int x = 0, y = 0;

            target = (Bitmap)Image.FromFile(mDirectory + pPath);

            int Timeout;
            if (pTry.CompareTo("") == 0)
                Timeout = 0;
            else
                Timeout = int.Parse(pTry);
            
            
            if (Timeout > 0)
            {
                for (int i = 0; i < Timeout; i++)
                {
                    if (imgc.GetImagePoint(target, ref x, ref y))
                    {
                        mouse.SetPoint(x, y);
                        return;
                    }
                    Record("Failed to Find : " + pPath);
                    Wait(500);
                }
            }
            else
            {
                while(true)
                {
                    if (imgc.GetImagePoint(target, ref x, ref y))
                    {
                        mouse.SetPoint(x, y);
                        return;
                    }
                    Record("Failed to Find : " + pPath);
                    Wait(500);
                }
            }

            target.Dispose();
        }

        private void Mouse(string pAction)
        {
            MouseActionNameSpace.MouseAction mouse = new MouseActionNameSpace.MouseAction();

            switch (pAction)
            {
                case ("LClick") :
                    mouse.LeftClick();
                    break;
                case ("LDown") :
                    mouse.LeftDown();
                    break;
                case ("LUp") :
                    mouse.LeftUp();
                    break;
                case ("RClick") :
                    mouse.RightClick();
                    break;
                case ("RDown") :
                    mouse.RightDown();
                    break;
                case ("RUp") :
                    mouse.RightUp();
                    break;
            }
        }
        
        private void Mouse(string pAction, string pX, string pY)
        {
            MouseActionNameSpace.MouseAction mouse = new MouseActionNameSpace.MouseAction();
            if (pX.CompareTo("") ==0)
            {
                pX = mouse.GetPoint().X.ToString();
            }

            if (pY.CompareTo("") ==0)
            {
                pY = mouse.GetPoint().Y.ToString();
            }

            switch (pAction)
            {
                case ("LClick") :
                    mouse.SetPoint(int.Parse(pX), int.Parse(pY));
                    mouse.LeftClick();
                    break;
                case ("LDown") :
                    mouse.SetPoint(int.Parse(pX), int.Parse(pY));
                    mouse.LeftDown();
                    break;
                case ("LUp") :
                    mouse.SetPoint(int.Parse(pX), int.Parse(pY));
                    mouse.LeftUp();
                    break;
                case ("RClick") :
                    mouse.SetPoint(int.Parse(pX), int.Parse(pY));
                    mouse.RightClick();
                    break;
                case ("RDown") :
                    mouse.SetPoint(int.Parse(pX), int.Parse(pY));
                    mouse.RightDown();
                    break;
                case ("RUp") :
                    mouse.SetPoint(int.Parse(pX), int.Parse(pY));
                    mouse.RightUp();
                    break;
                case ("Move") :
                    mouse.SetPoint(int.Parse(pX), int.Parse(pY));
                    break;
                case ("MoveRef") :
                    mouse.SetPointRef(int.Parse(pX), int.Parse(pY));
                    break;
            }
        }

        private void Keyboard(string pString)
        {
            pString = isVariable(pString);
            KeyboardActionNameSpace.KeyboardAction keyboard = new KeyboardActionNameSpace.KeyboardAction();
            keyboard.Type(pString);
        }

        private bool If(string pType, string pParam, string pString, string pTarget)
        {
            pString = isVariable(pString);
            pTarget = isVariable(pTarget);

            switch (pType)
            {
                case "Number" :
                    return IfNumber(pParam, pString, pTarget);
                case "Text" :
                    return IfString(pParam, pString, pTarget);
                case "Date" :
                    return IfDate(pParam, pString, pTarget);
            }

            return false;
        }

        bool IfString(string pParam, string pString, string pTarget)
        {
            StringBuilder stb = new StringBuilder();

            int failure = 0;
            
            try
            {
                for (int i = 0; i < pTarget.Length; i++)
                {
                    if (pString[i] != pTarget[i])
                        failure++;
                    if (((float)failure / pTarget.Length) >= 0.1f)
                        return false;
                }
                return true;
            }
            catch
            {

            }
            
            try
            {
                for (int i = 0; i < pString.Length; i++)
                {
                    if (pString[i] != pTarget[i])
                        failure++;
                    if (((float)failure / pString.Length) >= 0.1f)
                        return false;
                }
                return true;
            }
            catch
            {
                
            }

            return false;
        }

        bool IfDate(string pParam, string pString, string pTarget)
        {
            DateTime dt1 = DateTime.Parse(pString);
            DateTime dt2 = DateTime.Parse(pTarget);

            switch(pParam)
            {
                case "==" :
                    return (dt1 == dt2);
                case ">=" :
                    return (dt1 >= dt2);
                case "<=" :
                    return (dt1 <= dt2);
                case ">" :
                    return (dt1 > dt2);
                case "<" :
                    return (dt1 < dt2);
            }

            return false;
        }

        #region IfNumber
        bool IfNumber(string pParam, string pString, string pTarget)
        {
            StringBuilder stb = new StringBuilder();
            
            if (pParam.CompareTo("==") == 0)
            {
                for (int i = 0; i < pString.Length; i++)
                {
                    if ((int)pString[i] >= 48 && (int)pString[i] <= 57)
                    {
                        stb.Append(pString[i]);
                    }
                }

                if (int.Parse(stb.ToString()) == int.Parse(pTarget))
                {
                    return true;
                }
                
                if (Math.Abs(float.Parse(stb.ToString()) - float.Parse(pTarget)) < 0.01f)
                {
                    return true;
                }

                return false;
            }

            if (pParam.CompareTo(">") == 0)
            {
                for (int i = 0; i < pString.Length; i++)
                {
                    if ((int)pString[i] >= 48 && (int)pString[i] <= 57)
                    {
                        stb.Append(pString[i]);
                    }
                }

                if (int.Parse(stb.ToString()) > int.Parse(pTarget))
                {
                    return true;
                }
                
                if (float.Parse(stb.ToString()) > float.Parse(pTarget))
                {
                    return true;
                }

                return false;
            }

            if (pParam.CompareTo(">=") == 0)
            {
                for (int i = 0; i < pString.Length; i++)
                {
                    if ((int)pString[i] >= 48 && (int)pString[i] <= 57)
                    {
                        stb.Append(pString[i]);
                    }
                }

                if (int.Parse(stb.ToString()) >= int.Parse(pTarget))
                {
                    return true;
                }
                
                if (float.Parse(stb.ToString()) >= float.Parse(pTarget))
                {
                    return true;
                }

                return false;
            }

            if (pParam.CompareTo("<") == 0)
            {
                for (int i = 0; i < pString.Length; i++)
                {
                    if ((int)pString[i] >= 48 && (int)pString[i] <= 57)
                    {
                        stb.Append(pString[i]);
                    }
                }

                if (int.Parse(stb.ToString()) < int.Parse(pTarget))
                {
                    return true;
                }
                
                if (float.Parse(stb.ToString()) < float.Parse(pTarget))
                {
                    return true;
                }

                return false;
            }

            if (pParam.CompareTo("<=") == 0)
            {
                for (int i = 0; i < pString.Length; i++)
                {
                    if ((int)pString[i] >= 48 && (int)pString[i] <= 57)
                    {
                        stb.Append(pString[i]);
                    }
                }

                if (int.Parse(stb.ToString()) <= int.Parse(pTarget))
                {
                    return true;
                }
                
                if (float.Parse(stb.ToString()) <= float.Parse(pTarget))
                {
                    return true;
                }

                return false;
            }

            return false;
        }
        #endregion

        private string ReadOCR(int pStartX, int pStartY, int pEndX, int pEndY)
        {
            TesseractNameSpace.OCR ocr = new TesseractNameSpace.OCR();
            try
            {
                lastResult = ocr.read(pStartX, pStartY, pEndX, pEndY);
            }
            catch
            {
                lastResult = "";
            }
            return lastResult;
        }
        
        private void ReadOCR(int pStartX, int pStartY, int pEndX, int pEndY, string pTarget)
        {
            TesseractNameSpace.OCR ocr = new TesseractNameSpace.OCR();
            string Result = ocr.read(pStartX, pStartY, pEndX, pEndY);
            
            SetVariable(pTarget, Result);

            lastResult = Result;
        }

        private void MathDecode(string pOperator, string pOperandOne, string pOperandTwo, string pTarget)
        {
            pOperandOne = isVariable(pOperandOne);
            pOperandTwo = isVariable(pOperandTwo);

            DataTable dt = new DataTable();
            
            dt.Columns.Add("pOperandOne", typeof(int));
            dt.Columns.Add("pOperandTwo", typeof(int));
            dt.Columns.Add("Result", typeof(int));
            dt.Columns["Result"].Expression = pOperandOne + pOperator + pOperandTwo;
    
            DataRow row = dt.Rows.Add();
            row["pOperandOne"] = int.Parse(pOperandOne);
            row["pOperandTwo"] = int.Parse(pOperandTwo);

            dt.BeginLoadData();
            dt.EndLoadData();
 
            int a = (int)row["Result"];
            
            SetVariable(pTarget, a.ToString());

            lastResult = a.ToString();
        }
        
        private void CalendarDecode(string pOperator, string pOperandOne, string pOperandTwo, string pTarget)
        {
            pOperandOne = isVariable(pOperandOne);
            pOperandTwo = isVariable(pOperandTwo);
            
            DateTime dateOne = DateTime.Parse(pOperandOne);

            if (pOperator.CompareTo("Begin") == 0)
            {
                dateOne = new DateTime(dateOne.Year, dateOne.Month, 1);
            }
            if (pOperator.CompareTo("End") == 0)
            {
                dateOne = dateOne.AddMonths(1);
                dateOne = new DateTime(dateOne.Year, dateOne.Month, 1);
                dateOne = dateOne.AddDays(-1);
            }
            if (pOperator.CompareTo("Year") == 0)
            {
                dateOne = dateOne.AddYears(int.Parse(pOperandTwo));
            }
            if (pOperator.CompareTo("Month") == 0)
            {
                dateOne = dateOne.AddMonths(int.Parse(pOperandTwo));
            }
            if (pOperator.CompareTo("Day") == 0)
            {
                dateOne = dateOne.AddDays(int.Parse(pOperandTwo));
            }

            SetVariable(pTarget, dateOne.ToString());

            lastResult = dateOne.ToString();
        }
        private void GPortalLogin(string pID, string pPW)
        {
            pID = isVariable(pID);
            pPW = isVariable(pPW);
            webHelper.GPortalLogin(pID, pPW);
        }

        private void GDWFind(string pReportName)
        {
            pReportName = isVariable(pReportName);
            webHelper.GDWFind(pReportName);
            
            string tempDir = mDirectory;
            mDirectory = Application.StartupPath + "GDW\\";

            Queue<string> tempQ = new Queue<string>();
            tempQ.Enqueue("WaitImage:::GDW_Filter.png:::0");
            subActionQueue.Push(actionQueue);
            subActionQueue.Push(tempQ);
            Action(tempDir);
        }
        
        private void GDWQuery()
        {
            string tempDir = mDirectory;
            mDirectory = Application.StartupPath + "GDW\\";

            Queue<string> tempQ = new Queue<string>();
            tempQ.Enqueue("WaitImage:::GDW_Query.png:::0");
            tempQ.Enqueue("Wait:::250");
            tempQ.Enqueue("Mouse:::LClick");
            tempQ.Enqueue("GDWQueryWait:::");
            subActionQueue.Push(actionQueue);
            subActionQueue.Push(tempQ);
            Action(tempDir);
        }
        
        private void GDWDownload(string pReportName)
        {
            string tempDir = mDirectory;
            mDirectory = Application.StartupPath + "GDW\\";

            Queue<string> tempQ = new Queue<string>();
            tempQ.Enqueue("WaitImage:::GDW_Download.png:::0");
            tempQ.Enqueue("Mouse:::LClick");
            tempQ.Enqueue("WaitImage:::GDW_CSV.png:::0");
            tempQ.Enqueue("Mouse:::LClick");
            tempQ.Enqueue("WaitImage:::GDW_OK.png:::0");
            tempQ.Enqueue("Mouse:::LClick");
            tempQ.Enqueue("WaitImage:::GDW_Save.png:::0");
            tempQ.Enqueue("Mouse:::LClick");
            tempQ.Enqueue("WaitImage:::GDW_FileSave.png:::0");
            string fileName = "Keyboard:::" + pReportName;
            tempQ.Enqueue(fileName);
            tempQ.Enqueue("Mouse:::LClick");
            tempQ.Enqueue("WaitImage:::GDW_Download_Confirm.png:::0");
            tempQ.Enqueue("Mouse:::LClick");
            tempQ.Enqueue("Wait:::250");
            subActionQueue.Push(actionQueue);
            subActionQueue.Push(tempQ);
            Action(tempDir);
        }
        
        private void GDWQuit()
        {
            Queue<string> tempQ = new Queue<string>();
            tempQ.Enqueue("Mouse:::Move:1900:::15");
            tempQ.Enqueue("Mouse:::LClick");
            tempQ.Enqueue("Keyboard:::Enter");
            subActionQueue.Push(actionQueue);
            subActionQueue.Push(tempQ);
            Action();
        }
        
        private void ERPLogin(string pIDNumber)
        {
            pIDNumber = isVariable(pIDNumber);
            webHelper.ERPLogin(pIDNumber);
        }

        private void ERPSearch(string pTCode)
        {
            pTCode = isVariable(pTCode);
            string tempDir = mDirectory;
            mDirectory = Application.StartupPath + "GERP\\";

            Queue<string> tempQ = new Queue<string>();
            tempQ.Enqueue("WaitImage:::GERP_Search.png:::0");
            string fileName = "Keyboard:::" + pTCode;
            tempQ.Enqueue(fileName);
            tempQ.Enqueue("Keyboard:::Enter");
            subActionQueue.Push(actionQueue);
            subActionQueue.Push(tempQ);
            Action(tempDir);
        }


        private void WaitImageIf(string pPath)
        {
            pPath = isVariable(pPath);
            Console.WriteLine("I am waiting");
            ImageCompareNameSpace.ImageCompare imgc = new ImageCompareNameSpace.ImageCompare();
            MouseActionNameSpace.MouseAction mouse = new MouseActionNameSpace.MouseAction();

            Bitmap target;
            int x = 0, y = 0;
            int timeout = 2;
            target = (Bitmap)Image.FromFile(mDirectory + pPath);            
            
            while (timeout > 0)
            {
                if (imgc.GetImagePoint(target, ref x, ref y))
                {
                    Record("Image is still there : " + pPath);
                    mouse.SetPoint(x, y);
                    timeout++;
                }
                timeout--;
                Wait(750);
            }

            target.Dispose();
        }

        private void PythonExe(string pPath)
        {
            pPath = isVariable(pPath);
            System.Diagnostics.Process.Start(mPythonPath, mDirectory + "\\" + pPath);
        }

        private void SendEmail(string pTo, string pCC, string pSubject, string pBody, string pAttachment)
        {
            pTo = isVariable(pTo);
            pCC = isVariable(pCC);
            pSubject = isVariable(pSubject);
            pBody = isVariable(pBody);
            pAttachment = isVariable(pAttachment);
            officeHelper.EmailSend(pTo, pCC, pSubject, pBody, pAttachment);
        }

        private void ExcelFileOpen(string pPath)
        {
            pPath = isVariable(pPath);
            officeHelper.ExcelFileOpen(pPath);
        }
        
        private void ExcelReadCell(string pSheet, string pColumn, string pRow, string pTarget)
        {
            pSheet = isVariable(pSheet);
            pColumn = isVariable(pColumn);
            pRow = isVariable(pRow);
            lastResult = officeHelper.ExcelReadCell(int.Parse(pSheet), int.Parse(pColumn), int.Parse(pRow));
            SetVariable(pTarget);
        }
        
        private void ExcelWriteCell(string pSheet, string pColumn, string pRow, string pValue)
        {
            pSheet = isVariable(pSheet);
            pColumn = isVariable(pColumn);
            pRow = isVariable(pRow);
            officeHelper.ExcelWriteCell(int.Parse(pSheet), int.Parse(pColumn), int.Parse(pRow), pValue);
        }
        
        private void DAVINCILogin(string pLink, string pID, string pPW)
        {
            pLink = isVariable(pLink);
            pID = isVariable(pID);
            pPW = isVariable(pPW);
            webHelper.DAVINCILogin(pLink, pID, pPW);
        }
        
        private void DAVINCIDownload(string pReportID)
        {
            pReportID = isVariable(pReportID);
            webHelper.DAVINCIDownload(pReportID);
        }
    }
}