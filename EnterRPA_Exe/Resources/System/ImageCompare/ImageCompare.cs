using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ImageCompareNameSpace
{
    public class ImageCompare
    {
        Bitmap? CurrentScreen;
        Bitmap? TargetScreen;
        const int GAP = 3;

        /*
        ~ImageCompare()
        {
            CurrentScreen.Dispose();
        }
        */

        private Bitmap GetCurrenScreen()
        {
            CopyScreenNameSpace.CopyScreen csn = new CopyScreenNameSpace.CopyScreen();
            return csn.Copy();
        }

        public bool GetImagePoint(Bitmap pTarget, ref int pX, ref int pY)
        {
            CurrentScreen = GetCurrenScreen();
            TargetScreen = pTarget;

            Color tPixelStart = TargetScreen.GetPixel(0, 0);
            Color tPixelEnd = TargetScreen.GetPixel(TargetScreen.Width - 1, TargetScreen.Height - 1);
            Color cPixelStart;
            Color cPixelEnd;
            for (int x = 0; x < CurrentScreen.Width - TargetScreen.Width; x++)
            {
                for (int y = 0; y < CurrentScreen.Height - TargetScreen.Height; y++)
                {
                    cPixelStart = CurrentScreen.GetPixel(x, y);
                    cPixelEnd = CurrentScreen.GetPixel(x + TargetScreen.Width - 1, y + TargetScreen.Height - 1);
                    
                    if (PixelCompare(tPixelStart, cPixelStart))
                    if (PixelCompare(tPixelEnd, cPixelEnd))
                    {
                        if (DeepCompare(x, y))
                        {
                            pX = x;
                            pY = y;
                            Console.WriteLine("Success");
                            return true;
                        }
                    }
                }
            }

            CurrentScreen.Dispose();
            return false;
        }

        private bool DeepCompare(int pX, int pY) 
        {
            float max = TargetScreen.Width * TargetScreen.Height;
            float failure = 0;
            for (int x = 0; x < TargetScreen.Width; x++)
            {
                for (int y = 0; y < TargetScreen.Height; y++)
                {
                    try
                    {
                        Color tPixel = TargetScreen.GetPixel(x, y);
                        Color cPixel = CurrentScreen.GetPixel(pX + x, pY + y);

                        if (!PixelCompare(tPixel, cPixel, true))
                        {
                            failure++;
                            if ((failure / max) > 0.025f)
                                return false;
                            continue;
                        }

                        if (PixelCompare(tPixel, cPixel, true))
                        {
                            continue;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("ERROR");
                        Console.WriteLine("pX, pY = (" + pX + ", " + pY + ")");
                        Console.WriteLine("x, y = (" + x + ", " + y + ")");
                    }
                }
            }

            return true;
        }
        
        private bool PixelCompare(Color tPixel, Color cPixel, bool isDeep = false)
        {
            
            if (tPixel.A < 5)
            {
                return true;
            }

            if (Math.Abs(tPixel.R - cPixel.R) > GAP)
            {
                //Console.WriteLine("R Gap");
                return false;
            }
                    
            if (Math.Abs(tPixel.G - cPixel.G) > GAP)
            {
                //Console.WriteLine("G Gap");
                return false;
            }
                
            if (Math.Abs(tPixel.B - cPixel.B) > GAP)
            {
                //Console.WriteLine("B Gap");
                return false;
            }
                
            if (Math.Abs(tPixel.A - cPixel.A) > GAP)
            {
                return false;
            }
            /*
            
            if (tPixel == cPixel)
                return true;
            */
            return true;
        }
    }
}