using System;
using System.Collections.Generic;
using System.Threading;
using Tesseract;

namespace TesseractNameSpace
{
    public class OCR
    {
        public string read (string pFile) 
        {
            var ocr = new TesseractEngine(@"./Resources/tessdata", "kor+eng", EngineMode.LstmOnly);
            Pix pix = Pix.LoadFromFile(pFile);
            var result = ocr.Process(pix);
            return result.GetText().Trim();
        }

        public string read (string pFile, string pLang) 
        {
            var ocr = new TesseractEngine(@"./Resources/tessdata", pLang, EngineMode.LstmOnly);
            Pix pix = Pix.LoadFromFile(pFile);
            var result = ocr.Process(pix);
            return result.GetText().Trim();
        }

        public string read (int pStartX, int pStartY, int pEndX, int pEndY) 
        {
            var ocr = new TesseractEngine(@"./Resources/tessdata", "kor+eng", EngineMode.LstmOnly);

            CopyScreenNameSpace.CopyScreen copyScreen = new CopyScreenNameSpace.CopyScreen();
            copyScreen.Copy(pStartX, pStartY, pEndX, pEndY);
            Pix pix = Pix.LoadFromFile("TempImage.bmp");

            var result = ocr.Process(pix);
            return result.GetText().Trim();
        }
        public string read (int pStartX, int pStartY, int pEndX, int pEndY, string pLang) 
        {
            var ocr = new TesseractEngine(@"./Resources/tessdata", pLang, EngineMode.LstmOnly);

            CopyScreenNameSpace.CopyScreen copyScreen = new CopyScreenNameSpace.CopyScreen();
            copyScreen.Copy(pStartX, pStartY, pEndX, pEndY);
            Pix pix = Pix.LoadFromFile("TempImage.bmp");

            var result = ocr.Process(pix);
            
            return result.GetText().Trim();
        }
    }
}