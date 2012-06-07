namespace ICMPFilter
{
    partial class ICMPFilterDisplay
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
            this.label1 = new System.Windows.Forms.Label();
            this.typeField = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.codeField = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.deleteButton = new System.Windows.Forms.Button();
            this.allBox = new System.Windows.Forms.CheckBox();
            this.viewICMP = new System.Windows.Forms.Button();
            this.tableDisplay = new System.Windows.Forms.DataGridView();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.allButNDP = new System.Windows.Forms.CheckBox();
            this.blockIPv6Box = new System.Windows.Forms.CheckBox();
            this.ipv6Box = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.logBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.tableDisplay)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Filter Type";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // typeField
            // 
            this.typeField.Location = new System.Drawing.Point(64, 3);
            this.typeField.Name = "typeField";
            this.typeField.Size = new System.Drawing.Size(61, 20);
            this.typeField.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(136, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Code";
            // 
            // codeField
            // 
            this.codeField.Location = new System.Drawing.Point(174, 3);
            this.codeField.Name = "codeField";
            this.codeField.Size = new System.Drawing.Size(188, 20);
            this.codeField.TabIndex = 4;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(368, 3);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(97, 23);
            this.addButton.TabIndex = 5;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Codes may be comma separated.";
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteButton.Location = new System.Drawing.Point(471, 3);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(116, 23);
            this.deleteButton.TabIndex = 9;
            this.deleteButton.Text = "Delete Selected";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // allBox
            // 
            this.allBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.allBox.AutoSize = true;
            this.allBox.Location = new System.Drawing.Point(458, 3);
            this.allBox.Name = "allBox";
            this.allBox.Size = new System.Drawing.Size(92, 17);
            this.allBox.TabIndex = 10;
            this.allBox.Text = "Block All IPv4";
            this.allBox.UseVisualStyleBackColor = true;
            this.allBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // viewICMP
            // 
            this.viewICMP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewICMP.Location = new System.Drawing.Point(471, 28);
            this.viewICMP.Name = "viewICMP";
            this.viewICMP.Size = new System.Drawing.Size(116, 23);
            this.viewICMP.TabIndex = 11;
            this.viewICMP.Text = "View ICMP";
            this.viewICMP.UseVisualStyleBackColor = true;
            this.viewICMP.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // tableDisplay
            // 
            this.tableDisplay.AllowUserToAddRows = false;
            this.tableDisplay.AllowUserToDeleteRows = false;
            this.tableDisplay.AllowUserToResizeRows = false;
            this.tableDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tableDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableDisplay.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.Code,
            this.Description,
            this.Version});
            this.tableDisplay.Location = new System.Drawing.Point(0, 3);
            this.tableDisplay.MultiSelect = false;
            this.tableDisplay.Name = "tableDisplay";
            this.tableDisplay.ReadOnly = true;
            this.tableDisplay.RowHeadersVisible = false;
            this.tableDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tableDisplay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tableDisplay.Size = new System.Drawing.Size(445, 307);
            this.tableDisplay.TabIndex = 7;
            // 
            // Type
            // 
            this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Width = 56;
            // 
            // Code
            // 
            this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Width = 57;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // Version
            // 
            this.Version.HeaderText = "Version";
            this.Version.Name = "Version";
            this.Version.ReadOnly = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.36658F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.63342F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(601, 374);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ipv6Box);
            this.panel1.Controls.Add(this.typeField);
            this.panel1.Controls.Add(this.viewICMP);
            this.panel1.Controls.Add(this.deleteButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.codeField);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.addButton);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 318);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(595, 53);
            this.panel1.TabIndex = 8;
            // 
            // allButNDP
            // 
            this.allButNDP.AutoSize = true;
            this.allButNDP.Location = new System.Drawing.Point(458, 49);
            this.allButNDP.Name = "allButNDP";
            this.allButNDP.Size = new System.Drawing.Size(140, 17);
            this.allButNDP.TabIndex = 14;
            this.allButNDP.Text = "All ICMPv6 Except NDP";
            this.allButNDP.UseVisualStyleBackColor = true;
            this.allButNDP.CheckedChanged += new System.EventHandler(this.allButNDP_CheckedChanged);
            // 
            // blockIPv6Box
            // 
            this.blockIPv6Box.AutoSize = true;
            this.blockIPv6Box.Location = new System.Drawing.Point(458, 26);
            this.blockIPv6Box.Name = "blockIPv6Box";
            this.blockIPv6Box.Size = new System.Drawing.Size(92, 17);
            this.blockIPv6Box.TabIndex = 13;
            this.blockIPv6Box.Text = "Block All IPv6";
            this.blockIPv6Box.UseVisualStyleBackColor = true;
            this.blockIPv6Box.CheckedChanged += new System.EventHandler(this.blockIPv6Box_CheckedChanged);
            // 
            // ipv6Box
            // 
            this.ipv6Box.AutoSize = true;
            this.ipv6Box.Location = new System.Drawing.Point(5, 32);
            this.ipv6Box.Name = "ipv6Box";
            this.ipv6Box.Size = new System.Drawing.Size(48, 17);
            this.ipv6Box.TabIndex = 12;
            this.ipv6Box.Text = "IPv6";
            this.ipv6Box.UseVisualStyleBackColor = true;
            this.ipv6Box.CheckedChanged += new System.EventHandler(this.ipv6Box_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.logBox);
            this.panel2.Controls.Add(this.blockIPv6Box);
            this.panel2.Controls.Add(this.allButNDP);
            this.panel2.Controls.Add(this.tableDisplay);
            this.panel2.Controls.Add(this.allBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(595, 309);
            this.panel2.TabIndex = 9;
            // 
            // logBox
            // 
            this.logBox.AutoSize = true;
            this.logBox.Location = new System.Drawing.Point(458, 73);
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(58, 17);
            this.logBox.TabIndex = 15;
            this.logBox.Text = "Log All";
            this.logBox.UseVisualStyleBackColor = true;
            this.logBox.CheckedChanged += new System.EventHandler(this.logBox_CheckedChanged);
            // 
            // ICMPFilterDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ICMPFilterDisplay";
            this.Size = new System.Drawing.Size(601, 374);
            this.Load += new System.EventHandler(this.ICMPFilterDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tableDisplay)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox typeField;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox codeField;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.CheckBox allBox;
        private System.Windows.Forms.Button viewICMP;
        private System.Windows.Forms.DataGridView tableDisplay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Version;
        private System.Windows.Forms.CheckBox ipv6Box;
        private System.Windows.Forms.CheckBox blockIPv6Box;
        private System.Windows.Forms.CheckBox allButNDP;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox logBox;

    }
}
