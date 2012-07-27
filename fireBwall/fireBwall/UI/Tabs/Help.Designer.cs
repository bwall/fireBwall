namespace fireBwall.UI.Tabs
{
    partial class Help
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
            this.modBox = new System.Windows.Forms.ListBox();
            this.modData = new System.Windows.Forms.GroupBox();
            this.modAuthorField = new System.Windows.Forms.Label();
            this.modVersionField = new System.Windows.Forms.Label();
            this.modVersionLabel = new System.Windows.Forms.Label();
            this.modDescriptionField = new System.Windows.Forms.Label();
            this.modContactLabel = new System.Windows.Forms.Label();
            this.modAuthorLabel = new System.Windows.Forms.Label();
            this.modContactField = new System.Windows.Forms.Label();
            this.modHelpBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.modData.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // modBox
            // 
            this.modBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.modBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modBox.ForeColor = System.Drawing.Color.White;
            this.modBox.FormattingEnabled = true;
            this.modBox.Location = new System.Drawing.Point(0, 0);
            this.modBox.Name = "modBox";
            this.modBox.Size = new System.Drawing.Size(135, 312);
            this.modBox.TabIndex = 0;
            this.modBox.SelectedIndexChanged += new System.EventHandler(this.modBox_SelectedIndexChanged);
            // 
            // modData
            // 
            this.modData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.modData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.modData.Controls.Add(this.modAuthorField);
            this.modData.Controls.Add(this.modVersionField);
            this.modData.Controls.Add(this.modVersionLabel);
            this.modData.Controls.Add(this.modDescriptionField);
            this.modData.Controls.Add(this.modContactLabel);
            this.modData.Controls.Add(this.modAuthorLabel);
            this.modData.Controls.Add(this.modContactField);
            this.modData.Location = new System.Drawing.Point(3, 3);
            this.modData.MinimumSize = new System.Drawing.Size(0, 122);
            this.modData.Name = "modData";
            this.modData.Size = new System.Drawing.Size(458, 140);
            this.modData.TabIndex = 1;
            this.modData.TabStop = false;
            this.modData.Text = "Module";
            // 
            // modAuthorField
            // 
            this.modAuthorField.AutoSize = true;
            this.modAuthorField.Location = new System.Drawing.Point(53, 100);
            this.modAuthorField.Name = "modAuthorField";
            this.modAuthorField.Size = new System.Drawing.Size(0, 13);
            this.modAuthorField.TabIndex = 5;
            // 
            // modVersionField
            // 
            this.modVersionField.AutoSize = true;
            this.modVersionField.Location = new System.Drawing.Point(53, 78);
            this.modVersionField.Name = "modVersionField";
            this.modVersionField.Size = new System.Drawing.Size(0, 13);
            this.modVersionField.TabIndex = 4;
            // 
            // modVersionLabel
            // 
            this.modVersionLabel.AutoSize = true;
            this.modVersionLabel.Location = new System.Drawing.Point(2, 78);
            this.modVersionLabel.Name = "modVersionLabel";
            this.modVersionLabel.Size = new System.Drawing.Size(45, 13);
            this.modVersionLabel.TabIndex = 3;
            this.modVersionLabel.Text = "Version:";
            // 
            // modDescriptionField
            // 
            this.modDescriptionField.AutoSize = true;
            this.modDescriptionField.Location = new System.Drawing.Point(9, 20);
            this.modDescriptionField.Name = "modDescriptionField";
            this.modDescriptionField.Size = new System.Drawing.Size(0, 13);
            this.modDescriptionField.TabIndex = 2;
            this.modDescriptionField.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // modContactLabel
            // 
            this.modContactLabel.AutoSize = true;
            this.modContactLabel.Location = new System.Drawing.Point(6, 117);
            this.modContactLabel.Name = "modContactLabel";
            this.modContactLabel.Size = new System.Drawing.Size(47, 13);
            this.modContactLabel.TabIndex = 1;
            this.modContactLabel.Text = "Contact:";
            // 
            // modAuthorLabel
            // 
            this.modAuthorLabel.AutoSize = true;
            this.modAuthorLabel.Location = new System.Drawing.Point(6, 100);
            this.modAuthorLabel.Name = "modAuthorLabel";
            this.modAuthorLabel.Size = new System.Drawing.Size(41, 13);
            this.modAuthorLabel.TabIndex = 0;
            this.modAuthorLabel.Text = "Author:";
            // 
            // modContactField
            // 
            this.modContactField.AutoSize = true;
            this.modContactField.Location = new System.Drawing.Point(59, 117);
            this.modContactField.Name = "modContactField";
            this.modContactField.Size = new System.Drawing.Size(0, 13);
            this.modContactField.TabIndex = 6;
            // 
            // modHelpBox
            // 
            this.modHelpBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.modHelpBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modHelpBox.ForeColor = System.Drawing.Color.White;
            this.modHelpBox.Location = new System.Drawing.Point(3, 16);
            this.modHelpBox.Name = "modHelpBox";
            this.modHelpBox.ReadOnly = true;
            this.modHelpBox.Size = new System.Drawing.Size(452, 141);
            this.modHelpBox.TabIndex = 2;
            this.modHelpBox.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.modBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(603, 312);
            this.splitContainer1.SplitterDistance = 135;
            this.splitContainer1.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.modData, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 146F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(464, 312);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.modHelpBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 149);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 160);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Module Information";
            // 
            // Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.splitContainer1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "Help";
            this.Size = new System.Drawing.Size(603, 312);
            this.Load += new System.EventHandler(this.Help_Load);
            this.modData.ResumeLayout(false);
            this.modData.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox modBox;
        private System.Windows.Forms.GroupBox modData;
        private System.Windows.Forms.Label modAuthorLabel;
        private System.Windows.Forms.Label modContactLabel;
        private System.Windows.Forms.Label modContactField;
        private System.Windows.Forms.Label modAuthorField;
        private System.Windows.Forms.Label modVersionField;
        private System.Windows.Forms.Label modVersionLabel;
        private System.Windows.Forms.Label modDescriptionField;
        private System.Windows.Forms.RichTextBox modHelpBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
