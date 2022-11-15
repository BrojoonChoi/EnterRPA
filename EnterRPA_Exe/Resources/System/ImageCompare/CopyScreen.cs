using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CopyScreenNameSpace
{
    class CopyScreen
    {
        public Bitmap Copy ()
        {            
            // 주화면의 크기 정보 읽기
            Rectangle rect = Screen.PrimaryScreen.Bounds;            
            // 2nd screen = Screen.AllScreens[1]

            // 픽셀 포맷 정보 얻기 (Optional), 메모리 문제로 24비트 고정.
            /*
            int bitsPerPixel = Screen.PrimaryScreen.BitsPerPixel;
            PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
            if (bitsPerPixel <= 16)
            {
                pixelFormat = PixelFormat.Format16bppRgb565;
            }
            if (bitsPerPixel == 24)
            {
                pixelFormat = PixelFormat.Format24bppRgb;
            }
            */
            // 화면 크기만큼의 Bitmap 생성
            //Bitmap bmp = new Bitmap(rect.Width, rect.Height, pixelFormat);
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);

            // Bitmap 이미지 변경을 위해 Graphics 객체 생성
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                // 화면을 그대로 카피해서 Bitmap 메모리에 저장
                gr.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);
            }

            //Normalize(bmp);

            // Bitmap 데이타를 파일로 저장
            //bmp.Save("Test2.bmp");
            // Ref로 받아와서 나중에 메모리 비워줘야 겠다. wjchoi
            
            return bmp;
            //bmp.Dispose();
        }
        public void Copy (int pStartX, int pStartY, int pEndX, int pEndY)
        {            
            Rectangle rect = new Rectangle(pStartX, pStartY, pEndX, pEndY);
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);
            
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);
            }
            
            bmp = new Bitmap(bmp, new Size(rect.Width * 2, rect.Height * 2));

            Normalize(bmp);
            
            bmp.Save("TempImage.bmp");
            
            bmp.Dispose();
        }
        public void Normalize(Bitmap bmp)
        {
            for(int i = 0; i < bmp.Width; i++)
            {
                for(int j = 0; j < bmp.Height; j++)
                {
                    Color c = bmp.GetPixel(i, j);
                    int binary = (c.R + c.G + c.B) / 3;

                    if (binary < 190)
                        bmp.SetPixel(i, j, Color.White);
                    else
                        bmp.SetPixel(i, j, Color.Black);
                }
            }
        }    
    }
}
