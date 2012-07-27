namespace ScanDetector
{
    partial class Report
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
            this.label1 = new System.Windows.Forms.Label();
            this.addressField = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.accessField = new System.Windows.Forms.Label();
            this.portBox = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.averageField = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.portsField = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP Address:";
            // 
            // addressField
            // 
            this.addressField.AutoSize = true;
            this.addressField.Location = new System.Drawing.Point(164, 13);
            this.addressField.Name = "addressField";
            this.addressField.Size = new System.Drawing.Size(35, 13);
            this.addressField.TabIndex = 1;
            this.addressField.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Last Access:";
            // 
            // accessField
            // 
            this.accessField.AutoSize = true;
            this.accessField.Location = new System.Drawing.Point(164, 42);
            this.accessField.Name = "accessField";
            this.accessField.Size = new System.Drawing.Size(35, 13);
            this.accessField.TabIndex = 3;
            this.accessField.Text = "label4";
            // 
            // portBox
            // 
            this.portBox.FormattingEnabled = true;
            this.portBox.Location = new System.Drawing.Point(12, 166);
            this.portBox.Name = "portBox";
            this.portBox.Size = new System.Drawing.Size(268, 95);
            this.portBox.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Touched Ports:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Average Packet Rate:";
            // 
            // averageField
            // 
            this.averageField.AutoSize = true;
            this.averageField.Location = new System.Drawing.Point(164, 72);
            this.averageField.Name = "averageField";
            this.averageField.Size = new System.Drawing.Size(35, 13);
            this.averageField.TabIndex = 7;
            this.averageField.Text = "label7";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Ports Touched:";
            // 
            // portsField
            // 
            this.portsField.AutoSize = true;
            this.portsField.Location = new System.Drawing.Point(164, 97);
            this.portsField.Name = "portsField";
            this.portsField.Size = new System.Drawing.Size(35, 13);
            this.portsField.TabIndex = 9;
            this.portsField.Text = "label4";
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.portsField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.averageField);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.portBox);
            this.Controls.Add(this.accessField);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.addressField);
            this.Controls.Add(this.label1);
            this.Name = "Report";
            this.Text = "Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label addressField;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label accessField;
        private System.Windows.Forms.ListBox portBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label averageField;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label portsField;
    }
}