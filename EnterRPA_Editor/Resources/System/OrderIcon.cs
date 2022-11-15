using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace New_RPA_Editor.Resources.System
{
    public class OrderIcon : PictureBox
    {
        public string order;
        WRPA WParent;
        public OrderIcon(string pOrder, WRPA pParent)
        {
            WParent = pParent;
            Refresh(pOrder);

            this.Click += Order_Click;
            this.DoubleClick += Order_DoubleClick;
        }

        public void Refresh(string pOrder)
        {
            order = pOrder;
            string filePath;

            try
            {
                filePath = pOrder.Split(":::")[0];
            }
            catch
            {
                filePath = pOrder;
            }

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
            this.BackColor = Color.DimGray;
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        private void Order_Click(object? sender, EventArgs e)
        {
            if (order.CompareTo("") != 0 && ((MouseEventArgs)e).Button == MouseButtons.Right)
            {
                DefaultBox dbx = new DefaultBox(order, true);
                if (DialogResult.OK == dbx.ShowDialog())
                {
                    order = dbx.GetValue();
                }
            }

            if (((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                WParent.SetFocus(this);
            }
        }

        private void Order_DoubleClick(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Clear This box", "W RPA", MessageBoxButtons.OKCancel) == DialogResult.OK)
                this.Clear();
        }

        private void Clear()
        {
            this.Image = null;
            this.order = "";
        }

        public override string ToString()
        {
            return this.order;
        }
    }
}
