namespace New_RPA_Editor
{
    partial class CaptureBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelDrag = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panelDrag.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDrag
            // 
            this.panelDrag.Controls.Add(this.button1);
            this.panelDrag.Location = new System.Drawing.Point(9, 9);
            this.panelDrag.Margin = new System.Windows.Forms.Padding(20);
            this.panelDrag.Name = "panelDrag";
            this.panelDrag.Padding = new System.Windows.Forms.Padding(20);
            this.panelDrag.Size = new System.Drawing.Size(780, 430);
            this.panelDrag.TabIndex = 0;
            this.panelDrag.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelDrag_MouseDown);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // CaptureBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelDrag);
            this.Name = "CaptureBox";
            this.Text = "CaptureBox";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CaptureBox_FormClosed);
            this.Load += new System.EventHandler(this.CaptureBox_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CaptureBox_KeyPress);
            this.panelDrag.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panelDrag;
        private Button button1;
    }
}