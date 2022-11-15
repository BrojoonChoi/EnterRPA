namespace New_RPA_Editor
{
    partial class DefaultBox
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.btn_brows = new System.Windows.Forms.Button();
            this.lb_mouse_pos = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(296, 246);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(377, 246);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Fix Data";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button3
            // 
            this.button3.Image = global::New_RPA_Editor.Properties.Resources.img_filter;
            this.button3.Location = new System.Drawing.Point(330, 64);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(32, 32);
            this.button3.TabIndex = 3;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Image = global::New_RPA_Editor.Properties.Resources.img_filter;
            this.button4.Location = new System.Drawing.Point(330, 64);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(32, 32);
            this.button4.TabIndex = 4;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btn_brows
            // 
            this.btn_brows.Enabled = false;
            this.btn_brows.Location = new System.Drawing.Point(330, 64);
            this.btn_brows.Name = "btn_brows";
            this.btn_brows.Size = new System.Drawing.Size(32, 32);
            this.btn_brows.TabIndex = 5;
            this.btn_brows.UseVisualStyleBackColor = true;
            this.btn_brows.Visible = false;
            this.btn_brows.Click += new System.EventHandler(this.btn_brows_Click);
            // 
            // lb_mouse_pos
            // 
            this.lb_mouse_pos.AutoSize = true;
            this.lb_mouse_pos.Location = new System.Drawing.Point(220, 250);
            this.lb_mouse_pos.Name = "lb_mouse_pos";
            this.lb_mouse_pos.Size = new System.Drawing.Size(58, 15);
            this.lb_mouse_pos.TabIndex = 6;
            this.lb_mouse_pos.Text = "9999,9999";
            this.lb_mouse_pos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb_mouse_pos.Visible = false;
            // 
            // DefaultBox
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(464, 281);
            this.Controls.Add(this.lb_mouse_pos);
            this.Controls.Add(this.btn_brows);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "DefaultBox";
            this.ShowIcon = false;
            this.Text = "Data Add";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DefaultBox_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        private Button button2;
        private Label label1;
        private Button button3;
        private Button button4;
        private Button btn_brows;
        private Label lb_mouse_pos;
    }
}