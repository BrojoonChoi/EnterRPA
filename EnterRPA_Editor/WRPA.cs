using New_RPA_Editor.Resources.System;
using System.Text;
using System.IO;
using System.Threading;

namespace New_RPA_Editor
{
    public partial class WRPA : Form
    {
        List<OrderIcon> orderList = new List<OrderIcon>();
        OrderIcon currentIcon;

        private bool isDragging = false;
        private PictureBox draggingBox;

        public WRPA()
        {
            InitializeComponent();
            IO.Instance().init();

            AddTabs();

            this.DoubleBuffered = true;

            this.MouseMove += pictureBox_MouseMove;

            AddOrder("");
            SortOrder();
        }
        #region Event
        private void PanelResize()
        {
            //splitContainer1.Panel1.Controls.
        }
        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            
        }
        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btn_plus_Click_1(object sender, EventArgs e)
        {
            int index = orderList.IndexOf(currentIcon);
            AddOrder("", index);
            SortOrder();
            SetFocus(orderList[index + 1]);
        }

        private void btn_minus_Click_1(object sender, EventArgs e)
        {
            int index = orderList.IndexOf(currentIcon);
            OrderIcon order = orderList[index];
            if (index == 0)
                return;
            orderList.RemoveAt(index);
            order.Dispose();
            SortOrder();
            SetFocus(orderList[index - 1]);
        }

        #endregion

        #region Save & Load
        private void toolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            foreach (OrderIcon order in orderList)
            {
                order.Dispose();
            }
            orderList.Clear();
            AddOrder("");
            SortOrder();
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            foreach (OrderIcon order in orderList)
            {
                order.Dispose();
            }
            orderList.Clear();
            string[] result = IO.Instance().OpenFile();
            if (result != null)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    AddOrder(result[i]);
                }
            }

            SortOrder();
        }

        private void saveFileToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string[] pOrder = new string[orderList.Count];
            for (int i = 0; i < orderList.Count; i++)
            {
                pOrder[i] = orderList[i].ToString();
            }
            IO.Instance().SaveFile(pOrder);
        }
        #endregion

        #region Order Adding
       
        private void AddOrder(string pOrder)
        {
            OrderIcon order = new OrderIcon(pOrder, this);
            splitContainer2.Panel2.Controls.Add(order);
            orderList.Add(order);

            if (pOrder.CompareTo("") == 0)
                SetFocus(order);
        }
        private void AddOrder(string pOrder, int pIndex)
        {
            OrderIcon order = new OrderIcon(pOrder, this);
            splitContainer2.Panel2.Controls.Add(order);
            orderList.Insert(pIndex, order);

            if (pOrder.CompareTo("") == 0)
                SetFocus(order);
        }

        private void SortOrder()
        {
            splitContainer2.Panel2.AutoScroll = false;
            splitContainer2.Panel2.AutoScroll = true;
            for (int i = 0; i < orderList.Count; i++)
            {
                OrderIcon order = (OrderIcon)orderList[i];
                order.Location = new Point(32 + 96 * i, (splitContainer2.Panel2.Height / 2));
            }
        }
        #endregion

        private void AddTabs()
        {
            tabControl1.TabPages.Clear();
            string[] tabs = IO.Instance().OpenTabInfo();
            Dictionary<string, int> tabOrderCount = new Dictionary<string, int>();
            foreach (string tab in tabs)
            {
                TabPage page = new TabPage(tab);
                page.Name = tab;
                page.Text = tab;
                page.AutoScroll = true;
                tabControl1.TabPages.Add(page);
                tabOrderCount.Add(tab, 0);
            }
            AddDefault(tabOrderCount);
        }

        private void AddDefault(Dictionary<string, int> pTabOrderCount)
        {
            string[] result = IO.Instance().OpenDefault();
            string minorResult;
            
            DefaultIcon order;

            if (result != null)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    minorResult = result[i].Substring(0, result[i].IndexOf(":::"));

                    order = new DefaultIcon(result[i].Substring(result[i].IndexOf(":::") + 3), this);

                    tabControl1.SelectTab(minorResult);
                    tabControl1.SelectedTab.Controls.Add(order);

                    order.Location = new Point(16 + 96 * pTabOrderCount[minorResult], (splitContainer1.Panel2.Height / 2) - 48);

                    pTabOrderCount[minorResult] += 1;
                }
            }
            tabControl1.SelectTab(0);
        }

        public void BeginDrag(PictureBox pObj)
        {
            this.draggingBox = pObj;
            this.Controls.Add(pObj);
            isDragging = true;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isDragging)
            {
                this.draggingBox.Location = new Point(e.X, e.Y);

                this.draggingBox.Invalidate();
            }
        }

        public void SetFocus(OrderIcon pIcon)
        {
            if (currentIcon != null)
                currentIcon.BackColor = Color.DimGray;

            currentIcon = pIcon;
            currentIcon.BackColor = Color.GhostWhite;
        }
        public void SetIcon(string pOrder)
        {
            currentIcon.Refresh(pOrder);
            if (currentIcon == orderList[orderList.Count - 1])
            {
                AddOrder("");
                SortOrder();
            }
        }
    }
}