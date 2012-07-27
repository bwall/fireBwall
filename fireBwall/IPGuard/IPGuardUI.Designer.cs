namespace PassThru
{
    partial class IPGuardUI
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
            this.availableBox = new System.Windows.Forms.ListBox();
            this.loadedBox = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.addList = new System.Windows.Forms.Button();
            this.logBox = new System.Windows.Forms.CheckBox();
            this.removeButton = new System.Windows.Forms.Button();
            this.incomingSelection = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // availableBox
            // 
            this.availableBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.availableBox.FormattingEnabled = true;
            this.availableBox.Location = new System.Drawing.Point(324, 3);
            this.availableBox.Name = "availableBox";
            this.availableBox.Size = new System.Drawing.Size(315, 408);
            this.availableBox.TabIndex = 0;
            // 
            // loadedBox
            // 
            this.loadedBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadedBox.FormattingEnabled = true;
            this.loadedBox.Location = new System.Drawing.Point(3, 3);
            this.loadedBox.Name = "loadedBox";
            this.loadedBox.Size = new System.Drawing.Size(315, 408);
            this.loadedBox.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.addList, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.loadedBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.logBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.incomingSelection, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.removeButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.availableBox, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.19728F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.802721F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(642, 469);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // addList
            // 
            this.addList.Location = new System.Drawing.Point(324, 417);
            this.addList.Name = "addList";
            this.addList.Size = new System.Drawing.Size(75, 23);
            this.addList.TabIndex = 5;
            this.addList.Text = "Add List";
            this.addList.UseVisualStyleBackColor = true;
            this.addList.Click += new System.EventHandler(this.addList_Click);
            // 
            // logBox
            // 
            this.logBox.AutoSize = true;
            this.logBox.Location = new System.Drawing.Point(3, 447);
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(148, 17);
            this.logBox.TabIndex = 4;
            this.logBox.Text = "Log Blocked Connections";
            this.logBox.UseVisualStyleBackColor = true;
            this.logBox.CheckedChanged += new System.EventHandler(this.logBox_CheckedChanged);
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(3, 417);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 6;
            this.removeButton.Text = "Remove List";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // incomingSelection
            // 
            this.incomingSelection.AutoSize = true;
            this.incomingSelection.Location = new System.Drawing.Point(324, 447);
            this.incomingSelection.Name = "incomingSelection";
            this.incomingSelection.Size = new System.Drawing.Size(147, 17);
            this.incomingSelection.TabIndex = 7;
            this.incomingSelection.Text = "Block Incoming Requests";
            this.incomingSelection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.incomingSelection.UseVisualStyleBackColor = true;
            this.incomingSelection.CheckedChanged += new System.EventHandler(this.incomingSelection_CheckedChanged);
            // 
            // IPGuardUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "IPGuardUI";
            this.Size = new System.Drawing.Size(642, 469);
            this.Load += new System.EventHandler(this.IPGuard_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox availableBox;
        private System.Windows.Forms.ListBox loadedBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox logBox;
        private System.Windows.Forms.Button addList;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.CheckBox incomingSelection;
    }
}
