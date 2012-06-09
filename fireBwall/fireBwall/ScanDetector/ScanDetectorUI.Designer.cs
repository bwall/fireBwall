namespace ScanDetector
{
    partial class ScanDetectorUI
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.potentialIPBox = new System.Windows.Forms.ListBox();
            this.blockedIPList = new System.Windows.Forms.DataGridView();
            this.IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.skipPotential = new System.Windows.Forms.CheckBox();
            this.blockButton = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.infoButton = new System.Windows.Forms.Button();
            this.removeBlockedButton = new System.Windows.Forms.Button();
            this.cloakedMode = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blockedIPList)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.1129F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.8871F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.blockedIPList, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.2018F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.79821F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(555, 446);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.potentialIPBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(358, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 373);
            this.panel1.TabIndex = 0;
            // 
            // potentialIPBox
            // 
            this.potentialIPBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.potentialIPBox.FormattingEnabled = true;
            this.potentialIPBox.Location = new System.Drawing.Point(0, 0);
            this.potentialIPBox.Name = "potentialIPBox";
            this.potentialIPBox.Size = new System.Drawing.Size(194, 373);
            this.potentialIPBox.TabIndex = 0;
            this.potentialIPBox.Click += new System.EventHandler(this.potentialIPBox_Click);
            // 
            // blockedIPList
            // 
            this.blockedIPList.AllowUserToAddRows = false;
            this.blockedIPList.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.blockedIPList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.blockedIPList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.blockedIPList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IP});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.blockedIPList.DefaultCellStyle = dataGridViewCellStyle5;
            this.blockedIPList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blockedIPList.Location = new System.Drawing.Point(3, 3);
            this.blockedIPList.Name = "blockedIPList";
            this.blockedIPList.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.blockedIPList.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.blockedIPList.RowHeadersVisible = false;
            this.blockedIPList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.blockedIPList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.blockedIPList.Size = new System.Drawing.Size(349, 373);
            this.blockedIPList.TabIndex = 1;
            this.blockedIPList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.blockedIPList_CellClick);
            // 
            // IP
            // 
            this.IP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.IP.HeaderText = "IP";
            this.IP.Name = "IP";
            this.IP.ReadOnly = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.skipPotential);
            this.panel2.Controls.Add(this.blockButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(358, 382);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(194, 61);
            this.panel2.TabIndex = 2;
            // 
            // skipPotential
            // 
            this.skipPotential.AutoSize = true;
            this.skipPotential.Location = new System.Drawing.Point(4, 33);
            this.skipPotential.Name = "skipPotential";
            this.skipPotential.Size = new System.Drawing.Size(111, 17);
            this.skipPotential.TabIndex = 2;
            this.skipPotential.Text = "Block Immediately";
            this.skipPotential.UseVisualStyleBackColor = true;
            this.skipPotential.CheckedChanged += new System.EventHandler(this.skipPotential_CheckedChanged);
            // 
            // blockButton
            // 
            this.blockButton.Location = new System.Drawing.Point(3, 3);
            this.blockButton.Name = "blockButton";
            this.blockButton.Size = new System.Drawing.Size(75, 23);
            this.blockButton.TabIndex = 0;
            this.blockButton.Text = "Block IP";
            this.blockButton.UseVisualStyleBackColor = true;
            this.blockButton.Click += new System.EventHandler(this.blockButton_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cloakedMode);
            this.panel3.Controls.Add(this.infoButton);
            this.panel3.Controls.Add(this.removeBlockedButton);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 382);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(349, 61);
            this.panel3.TabIndex = 3;
            // 
            // infoButton
            // 
            this.infoButton.Location = new System.Drawing.Point(4, 32);
            this.infoButton.Name = "infoButton";
            this.infoButton.Size = new System.Drawing.Size(75, 23);
            this.infoButton.TabIndex = 1;
            this.infoButton.Text = "Report";
            this.infoButton.UseVisualStyleBackColor = true;
            this.infoButton.Click += new System.EventHandler(this.infoButton_Click);
            // 
            // removeBlockedButton
            // 
            this.removeBlockedButton.Location = new System.Drawing.Point(4, 2);
            this.removeBlockedButton.Name = "removeBlockedButton";
            this.removeBlockedButton.Size = new System.Drawing.Size(75, 23);
            this.removeBlockedButton.TabIndex = 0;
            this.removeBlockedButton.Text = "Remove";
            this.removeBlockedButton.UseVisualStyleBackColor = true;
            this.removeBlockedButton.Click += new System.EventHandler(this.removeBlockedButton_Click);
            // 
            // cloakedMode
            // 
            this.cloakedMode.AutoSize = true;
            this.cloakedMode.Location = new System.Drawing.Point(251, 36);
            this.cloakedMode.Name = "cloakedMode";
            this.cloakedMode.Size = new System.Drawing.Size(95, 17);
            this.cloakedMode.TabIndex = 2;
            this.cloakedMode.Text = "Cloaked Mode";
            this.cloakedMode.UseVisualStyleBackColor = true;
            this.cloakedMode.CheckedChanged += new System.EventHandler(this.cloakedMode_CheckedChanged);
            // 
            // ScanDetectorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ScanDetectorUI";
            this.Size = new System.Drawing.Size(555, 446);
            this.Load += new System.EventHandler(this.ScanDetectorUI_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.blockedIPList)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox potentialIPBox;
        private System.Windows.Forms.DataGridView blockedIPList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button infoButton;
        private System.Windows.Forms.Button removeBlockedButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP;
        private System.Windows.Forms.CheckBox skipPotential;
        private System.Windows.Forms.Button blockButton;
        private System.Windows.Forms.CheckBox cloakedMode;
    }
}
