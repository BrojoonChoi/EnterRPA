using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using New_RPA_Editor.Resources.System;

namespace New_RPA_Editor
{
    public partial class DefaultBox : Form
    {
        private List<TextBox> tbList = new List<TextBox>();
        private string lbOrder;
        private ComboBox cbOrder;
        private ComboBox cbOrderChild;
        public DefaultBox(string pOrder, bool isOrder = false)
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            string[] temp = pOrder.Split(":::");

            if (isOrder)
            {
                // If it's already existing one
                temp = IO.Instance().GetDefaultList(temp[0]);
                BuildDefault(temp);
                BuildOrder(pOrder);
            }
            else
            {
                //If it's new one
                BuildDefault(temp);
            }
        }
        private void BuildOrder(string pOrder)
        {
            string[] temp = pOrder.Split(":::");

            for (int i = 0; i < temp.Length - 1; i++)
            {
                // i + 1 is to skip the name of order.
                tbList[i].Text = temp[i + 1];
            }

            if (cbOrder != null)
                cbOrder.Text = temp[1];

            if (cbOrderChild != null)
                cbOrderChild.Text = temp[2];
        }

        private void BuildDefault(string[] temp)
        {
            this.label1.Text = temp[0];

            Label lb;
            TextBox tb;

            lbOrder = temp[1];

            InitButton(temp);

            if (temp.Length > 3)
            {
                lb = new Label();
                lb.Location = new Point(32, 64);
                lb.AutoSize = true;
                lb.Text = temp[1];
                this.Controls.Add(lb);

                for (int i = 2; i < temp.Length - 1; i++)
                {
                    lb = new Label();
                    lb.Location = new Point(32, 64 + (i - 1) * 32);
                    lb.AutoSize = true;
                    lb.Text = temp[i];
                    this.Controls.Add(lb);

                    tb = new TextBox();
                    tb.Location = new Point(192, 64 + (i - 1) * 32);
                    tb.Text = "";
                    this.Controls.Add(tb);
                    tbList.Add(tb);
                }

                AddComboBox(temp[1]);

                return;
            }

            for (int i = 1; i < temp.Length - 1; i++)
            {
                lb = new Label();
                lb.Location = new Point(32, 64 + (i - 1) * 32);
                lb.AutoSize = true;
                lb.Text = temp[i];
                this.Controls.Add(lb);

                tb = new TextBox();
                tb.Location = new Point(192, 64 + (i - 1) * 32);
                tb.Text = "";
                this.Controls.Add(tb);
                tbList.Add(tb);
            }
        }

        private void AddComboBox(string pStr)
        {
            ComboBox cb = new ComboBox();
            cb.Location = new Point(192, 96);
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cb);
            cbOrder = cb;

            string[] comboBoxList = IO.Instance().GetComboBoxList(pStr);

            if (comboBoxList[0].CompareTo("Empty") == 0)
            {
                cb.Hide();
            }
            else
            {
                cb.SelectedIndexChanged += ComboboxChange1;
                cb.Items.AddRange(comboBoxList);
                cb.Text = cb.Items[0].ToString();
                tbList[0].Hide();
            }

            //If there is sub options.
            comboBoxList = IO.Instance().GetComboBoxList(cb.Text);
            if (comboBoxList[0].CompareTo("Empty") != 0)
            {
                cb = new ComboBox();
                cb.Location = new Point(192, 128);
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                this.Controls.Add(cb);
                cbOrderChild = cb;

                cb.Items.AddRange(comboBoxList);
                tbList[1].Hide();
                cb.SelectedIndexChanged += ComboboxChange2;
            }
        }

        #region Event
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void MessageBoxW_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void InitButton(string[] temp)
        {
            if (temp.Length == 2)
            {
                return;
            }

            if (temp[2].CompareTo("Path") == 0)
            {
                btn_brows.Enabled = true;
                btn_brows.Visible = true;
            }

            if (temp[1].Contains("Image"))
            {
                button3.Visible = true;
                btn_brows.Enabled = false;
                btn_brows.Visible = false;
            }

            if (temp[1].CompareTo("ReadOCR") == 0)
            {
                button4.Visible = true;
                btn_brows.Enabled = false;
                btn_brows.Visible = false;
            }

            if (temp[1].CompareTo("Mouse") == 0)
            {
                lb_mouse_pos.Visible = true;
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Start();
                timer.Interval = 100;
                timer.Tick += MousePosition;
                timer.Enabled = true;
            }
        }

        private void DefaultBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            button1_Click(sender, e);
        }

        public string GetValue ()
        {
            StringBuilder stb = new StringBuilder();

            stb.Append(lbOrder);
            for (int i = 0; i < tbList.Count; i++)
            {
                stb.Append(":::");
                stb.Append(tbList[i].Text);
            }

            return stb.ToString();
        }

        private void ComboboxChange1(object sender, EventArgs e)
        {
            tbList[0].Text = ((ComboBox)sender).Text;

            if (cbOrderChild != null)
            {
                string[] comboBoxList = IO.Instance().GetComboBoxList(tbList[0].Text);

                if (comboBoxList[0].CompareTo("Empty") == 0)
                {
                    tbList[1].Clear();
                    cbOrderChild.Items.Clear();
                }
                else
                {
                    tbList[1].Clear();
                    cbOrderChild.Items.Clear();
                    cbOrderChild.Items.AddRange(comboBoxList);
                }
            }
        }

        private void ComboboxChange2(object sender, EventArgs e)
        {
            tbList[1].Text = ((ComboBox)sender).Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Image capture
            CaptureBox captureBox = new CaptureBox(tbList, true);
            captureBox.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Just to get rect angle position
            CaptureBox captureBox = new CaptureBox(tbList, false);
            captureBox.Show();
        }

        private void btn_brows_Click(object sender, EventArgs e)
        {
            //file browsing
            OpenFileDialog dialog = new OpenFileDialog();
            if (DialogResult.OK == dialog.ShowDialog())
            {
                tbList[0].Text = dialog.FileName;
            }
        }

        private void MousePosition(object sender, EventArgs e)
        {
            lb_mouse_pos.Text = String.Format("{0}, {1}", Cursor.Position.X.ToString(), Cursor.Position.Y.ToString());
        }
    }
}
