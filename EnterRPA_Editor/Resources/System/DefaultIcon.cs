using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace New_RPA_Editor.Resources.System
{
    public class DefaultIcon : PictureBox
    {
        public string order;
        WRPA WParent;
        public DefaultIcon(string pOrder, WRPA pParent)
        {
            order = pOrder;
            WParent = pParent;

            string filePath;

            filePath = pOrder.Split(":::")[1];

            if (pOrder.CompareTo("") != 0)
            {
                try
                {
                    this.Image = Image.FromFile(@"Icon/" + filePath + ".png");
                }
                catch
                {
                    this.Image = (Image)Properties.Resources.ResourceManager.GetObject("img_" + filePath);
                }
            }
            this.Size = new Size(64, 64);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Click += Order_Click;
            this.DoubleClick += Order_DoubleClick;
            this.BackColor = Color.DimGray;
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        private void Order_Click(object? sender, EventArgs e)
        {
        }

        private void Order_DoubleClick(object? sender, EventArgs e)
        {
            AddOrder();
        }

        private void AddOrder()
        {
            DefaultBox mbw = new DefaultBox(order);
            if (DialogResult.OK == mbw.ShowDialog())
            {
                WParent.SetIcon(mbw.GetValue());
            }
        }


        public override string ToString()
        {
            return this.order;
        }
    }
}
