using System;
using System.Threading;
using System.Diagnostics;
using Outlook = Microsoft.Office.Interop.Outlook;
using Excel = Microsoft.Office.Interop.Excel;


namespace OfficeHelper
{
    public class OfficeHelper
    {
        Excel.Workbook mWorkBook;
        Excel.Application Excel;

        ~OfficeHelper()
        {
            if (mWorkBook != null)
            {
                mWorkBook.Close(true);
                Excel.Quit();
            }
        }
        public void EmailSend (string pTo, string pCC, string pSubject, string pBody, string pAttachment)
        {
            string OutlookFilepath = @"C:\Program Files\Microsoft Office\root\Office16\OUTLOOK.EXE";
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(OutlookFilepath);
            process.Start();

            Outlook._Application app = new Outlook.Application();
            Outlook.NameSpace nameSpace = app.GetNamespace("MAPI");
            nameSpace.Logon("", "", false, false);
            
            Outlook.MailItem mail = (Outlook.MailItem)app.CreateItem(Outlook.OlItemType.olMailItem);
            
            mail.To = pTo;
            mail.CC = pCC;
            mail.Subject = pSubject;
            mail.Body = pBody;

            if (pAttachment.CompareTo("") != 0)
            {
                string attachment = pAttachment;
                mail.Attachments.Add(attachment, Microsoft.Office.Interop.Outlook.OlAttachmentType.olByValue);
                Thread.Sleep(1000);
            }

            mail.Importance = Outlook.OlImportance.olImportanceLow;
            mail.Display(false);

            //Outlook.Account account = GetAccountForEmailAddress(app); 
            //mail.SendUsingAccount = account;

            ((Outlook._MailItem)mail).Send();
            IO_NameSpace.IO.Instance().Record("Email Sent");

            Thread.Sleep(2500);
            
            if (pAttachment.CompareTo("") != 0)
                Thread.Sleep(2500);
            process.Kill();
            app.Quit();
            Thread.Sleep(2000);
        }
        public void ExcelFileOpen(string pPath)
        {
            Excel = new Excel.Application();
            mWorkBook = Excel.Workbooks.Open(pPath);
        }

        public string ExcelReadCell(int pSheet, int pColumn, int pRow)
        {
            if (mWorkBook == null)
            {
                IO_NameSpace.IO.Instance().Record("Open Excel File First.");
                throw new FileLoadException("Open Excel File First.");
            }

            Excel.Worksheet Ws = mWorkBook.Worksheets[pSheet] as Excel.Worksheet;
            
            // 시트 선택
            Ws.Select();
            Excel.Range result = Ws.Cells[pRow, pColumn] as Excel.Range;
            return result.Value2.ToString();
            // 시트에서 데이터가 있는 범위 자동 선택
            //Excel.Range range = Ws.UsedRange;
            // 해당 범위에서 "{GROUP_CD}" 를 => "aa" 로 수정
            //range.Replace("{GROUP_CD}", "aa");
        }
        
        public void ExcelWriteCell(int pSheet, int pColumn, int pRow, string pValue)
        {
            if (mWorkBook == null)
            {
                IO_NameSpace.IO.Instance().Record("Open Excel File First.");
                throw new FileLoadException("Open Excel File First.");
            }

            Excel.Worksheet Ws = mWorkBook.Worksheets[pSheet] as Excel.Worksheet;
            
            // 시트 선택
            Ws.Select();
            Excel.Range result = Ws.Cells[pRow, pColumn] as Excel.Range;
            result.Value2 = pValue;
            // 시트에서 데이터가 있는 범위 자동 선택
            //Excel.Range range = Ws.UsedRange;
            // 해당 범위에서 "{GROUP_CD}" 를 => "aa" 로 수정
            //range.Replace("{GROUP_CD}", "aa");
        }
    }
}