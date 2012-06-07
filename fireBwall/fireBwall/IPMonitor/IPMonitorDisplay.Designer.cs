namespace IPMonitor
{
    partial class IPMonitorDisplay
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
            this.ipDisplay = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tcpDisplay = new System.Windows.Forms.DataGridView();
            this.tcpTotal = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.udpTotal = new System.Windows.Forms.Label();
            this.udpDisplay = new System.Windows.Forms.DataGridView();
            this.statistics = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.incDataErrField = new System.Windows.Forms.Label();
            this.incDataDiscField = new System.Windows.Forms.Label();
            this.dataSentField = new System.Windows.Forms.Label();
            this.dataReceivedField = new System.Windows.Forms.Label();
            this.incDataWErrLabel = new System.Windows.Forms.Label();
            this.incDataDiscLabel = new System.Windows.Forms.Label();
            this.dataSentLabel = new System.Windows.Forms.Label();
            this.dataRecLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.segsSentField = new System.Windows.Forms.Label();
            this.segsResentField = new System.Windows.Forms.Label();
            this.segsReceivedField = new System.Windows.Forms.Label();
            this.resetsSentField = new System.Windows.Forms.Label();
            this.maxConnField = new System.Windows.Forms.Label();
            this.failedConAttField = new System.Windows.Forms.Label();
            this.errorsReceivedField = new System.Windows.Forms.Label();
            this.cumulativeConnectionsField = new System.Windows.Forms.Label();
            this.connInitiatedField = new System.Windows.Forms.Label();
            this.resetConnField = new System.Windows.Forms.Label();
            this.connAcceptField = new System.Windows.Forms.Label();
            this.segSentLabel = new System.Windows.Forms.Label();
            this.segResentLabel = new System.Windows.Forms.Label();
            this.segRecLabel = new System.Windows.Forms.Label();
            this.resetsSentLabel = new System.Windows.Forms.Label();
            this.resetConLabel = new System.Windows.Forms.Label();
            this.maxConLabel = new System.Windows.Forms.Label();
            this.failedConnAttLabel = new System.Windows.Forms.Label();
            this.errRecLabel = new System.Windows.Forms.Label();
            this.cumulativeConnLabel = new System.Windows.Forms.Label();
            this.connInitLabel = new System.Windows.Forms.Label();
            this.connAcceptLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ipDisplay.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcpDisplay)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udpDisplay)).BeginInit();
            this.statistics.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ipDisplay
            // 
            this.ipDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ipDisplay.Controls.Add(this.tabPage1);
            this.ipDisplay.Controls.Add(this.tabPage2);
            this.ipDisplay.Controls.Add(this.statistics);
            this.ipDisplay.Location = new System.Drawing.Point(3, 3);
            this.ipDisplay.Name = "ipDisplay";
            this.ipDisplay.SelectedIndex = 0;
            this.ipDisplay.Size = new System.Drawing.Size(547, 444);
            this.ipDisplay.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tcpDisplay);
            this.tabPage1.Controls.Add(this.tcpTotal);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(539, 418);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "TCP Connections";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tcpDisplay
            // 
            this.tcpDisplay.AllowUserToResizeRows = false;
            this.tcpDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcpDisplay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tcpDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tcpDisplay.Location = new System.Drawing.Point(0, 0);
            this.tcpDisplay.Name = "tcpDisplay";
            this.tcpDisplay.ReadOnly = true;
            this.tcpDisplay.RowHeadersVisible = false;
            this.tcpDisplay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tcpDisplay.Size = new System.Drawing.Size(543, 395);
            this.tcpDisplay.TabIndex = 0;
            // 
            // tcpTotal
            // 
            this.tcpTotal.AutoSize = true;
            this.tcpTotal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tcpTotal.Location = new System.Drawing.Point(3, 402);
            this.tcpTotal.Name = "tcpTotal";
            this.tcpTotal.Size = new System.Drawing.Size(78, 13);
            this.tcpTotal.TabIndex = 1;
            this.tcpTotal.Text = "Connections: 0";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.udpTotal);
            this.tabPage2.Controls.Add(this.udpDisplay);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(539, 418);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "UDP Listeners";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // udpTotal
            // 
            this.udpTotal.AutoSize = true;
            this.udpTotal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.udpTotal.Location = new System.Drawing.Point(3, 402);
            this.udpTotal.Name = "udpTotal";
            this.udpTotal.Size = new System.Drawing.Size(61, 13);
            this.udpTotal.TabIndex = 1;
            this.udpTotal.Text = "Listeners: 0";
            // 
            // udpDisplay
            // 
            this.udpDisplay.AllowUserToAddRows = false;
            this.udpDisplay.AllowUserToDeleteRows = false;
            this.udpDisplay.AllowUserToResizeRows = false;
            this.udpDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.udpDisplay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.udpDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.udpDisplay.Location = new System.Drawing.Point(0, 0);
            this.udpDisplay.Name = "udpDisplay";
            this.udpDisplay.ReadOnly = true;
            this.udpDisplay.RowHeadersVisible = false;
            this.udpDisplay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.udpDisplay.Size = new System.Drawing.Size(543, 393);
            this.udpDisplay.TabIndex = 0;
            // 
            // statistics
            // 
            this.statistics.Controls.Add(this.groupBox2);
            this.statistics.Controls.Add(this.groupBox1);
            this.statistics.Location = new System.Drawing.Point(4, 22);
            this.statistics.Name = "statistics";
            this.statistics.Padding = new System.Windows.Forms.Padding(3);
            this.statistics.Size = new System.Drawing.Size(539, 418);
            this.statistics.TabIndex = 2;
            this.statistics.Text = "Statistics";
            this.statistics.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.incDataErrField);
            this.groupBox2.Controls.Add(this.incDataDiscField);
            this.groupBox2.Controls.Add(this.dataSentField);
            this.groupBox2.Controls.Add(this.dataReceivedField);
            this.groupBox2.Controls.Add(this.incDataWErrLabel);
            this.groupBox2.Controls.Add(this.incDataDiscLabel);
            this.groupBox2.Controls.Add(this.dataSentLabel);
            this.groupBox2.Controls.Add(this.dataRecLabel);
            this.groupBox2.Location = new System.Drawing.Point(6, 241);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(514, 119);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "UDP";
            // 
            // incDataErrField
            // 
            this.incDataErrField.AutoSize = true;
            this.incDataErrField.Location = new System.Drawing.Point(410, 44);
            this.incDataErrField.Name = "incDataErrField";
            this.incDataErrField.Size = new System.Drawing.Size(41, 13);
            this.incDataErrField.TabIndex = 7;
            this.incDataErrField.Text = "label30";
            // 
            // incDataDiscField
            // 
            this.incDataDiscField.AutoSize = true;
            this.incDataDiscField.Location = new System.Drawing.Point(410, 20);
            this.incDataDiscField.Name = "incDataDiscField";
            this.incDataDiscField.Size = new System.Drawing.Size(41, 13);
            this.incDataDiscField.TabIndex = 6;
            this.incDataDiscField.Text = "label29";
            // 
            // dataSentField
            // 
            this.dataSentField.AutoSize = true;
            this.dataSentField.Location = new System.Drawing.Point(142, 44);
            this.dataSentField.Name = "dataSentField";
            this.dataSentField.Size = new System.Drawing.Size(41, 13);
            this.dataSentField.TabIndex = 5;
            this.dataSentField.Text = "label28";
            // 
            // dataReceivedField
            // 
            this.dataReceivedField.AutoSize = true;
            this.dataReceivedField.Location = new System.Drawing.Point(142, 20);
            this.dataReceivedField.Name = "dataReceivedField";
            this.dataReceivedField.Size = new System.Drawing.Size(41, 13);
            this.dataReceivedField.TabIndex = 4;
            this.dataReceivedField.Text = "label27";
            // 
            // incDataWErrLabel
            // 
            this.incDataWErrLabel.AutoSize = true;
            this.incDataWErrLabel.Location = new System.Drawing.Point(241, 44);
            this.incDataWErrLabel.Name = "incDataWErrLabel";
            this.incDataWErrLabel.Size = new System.Drawing.Size(162, 13);
            this.incDataWErrLabel.TabIndex = 3;
            this.incDataWErrLabel.Text = "Incoming Datagrams With Errors:";
            // 
            // incDataDiscLabel
            // 
            this.incDataDiscLabel.AutoSize = true;
            this.incDataDiscLabel.Location = new System.Drawing.Point(245, 20);
            this.incDataDiscLabel.Name = "incDataDiscLabel";
            this.incDataDiscLabel.Size = new System.Drawing.Size(158, 13);
            this.incDataDiscLabel.TabIndex = 2;
            this.incDataDiscLabel.Text = "Incoming Datagrams Discarded:";
            // 
            // dataSentLabel
            // 
            this.dataSentLabel.AutoSize = true;
            this.dataSentLabel.Location = new System.Drawing.Point(49, 44);
            this.dataSentLabel.Name = "dataSentLabel";
            this.dataSentLabel.Size = new System.Drawing.Size(86, 13);
            this.dataSentLabel.TabIndex = 1;
            this.dataSentLabel.Text = "Datagrams Sent:";
            // 
            // dataRecLabel
            // 
            this.dataRecLabel.AutoSize = true;
            this.dataRecLabel.Location = new System.Drawing.Point(25, 20);
            this.dataRecLabel.Name = "dataRecLabel";
            this.dataRecLabel.Size = new System.Drawing.Size(110, 13);
            this.dataRecLabel.TabIndex = 0;
            this.dataRecLabel.Text = "Datagrams Received:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.segsSentField);
            this.groupBox1.Controls.Add(this.segsResentField);
            this.groupBox1.Controls.Add(this.segsReceivedField);
            this.groupBox1.Controls.Add(this.resetsSentField);
            this.groupBox1.Controls.Add(this.maxConnField);
            this.groupBox1.Controls.Add(this.failedConAttField);
            this.groupBox1.Controls.Add(this.errorsReceivedField);
            this.groupBox1.Controls.Add(this.cumulativeConnectionsField);
            this.groupBox1.Controls.Add(this.connInitiatedField);
            this.groupBox1.Controls.Add(this.resetConnField);
            this.groupBox1.Controls.Add(this.connAcceptField);
            this.groupBox1.Controls.Add(this.segSentLabel);
            this.groupBox1.Controls.Add(this.segResentLabel);
            this.groupBox1.Controls.Add(this.segRecLabel);
            this.groupBox1.Controls.Add(this.resetsSentLabel);
            this.groupBox1.Controls.Add(this.resetConLabel);
            this.groupBox1.Controls.Add(this.maxConLabel);
            this.groupBox1.Controls.Add(this.failedConnAttLabel);
            this.groupBox1.Controls.Add(this.errRecLabel);
            this.groupBox1.Controls.Add(this.cumulativeConnLabel);
            this.groupBox1.Controls.Add(this.connInitLabel);
            this.groupBox1.Controls.Add(this.connAcceptLabel);
            this.groupBox1.Location = new System.Drawing.Point(6, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(514, 200);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TCP";
            // 
            // segsSentField
            // 
            this.segsSentField.AutoSize = true;
            this.segsSentField.Location = new System.Drawing.Point(384, 136);
            this.segsSentField.Name = "segsSentField";
            this.segsSentField.Size = new System.Drawing.Size(41, 13);
            this.segsSentField.TabIndex = 21;
            this.segsSentField.Text = "label26";
            // 
            // segsResentField
            // 
            this.segsResentField.AutoSize = true;
            this.segsResentField.Location = new System.Drawing.Point(384, 107);
            this.segsResentField.Name = "segsResentField";
            this.segsResentField.Size = new System.Drawing.Size(41, 13);
            this.segsResentField.TabIndex = 20;
            this.segsResentField.Text = "label25";
            // 
            // segsReceivedField
            // 
            this.segsReceivedField.AutoSize = true;
            this.segsReceivedField.Location = new System.Drawing.Point(384, 79);
            this.segsReceivedField.Name = "segsReceivedField";
            this.segsReceivedField.Size = new System.Drawing.Size(41, 13);
            this.segsReceivedField.TabIndex = 19;
            this.segsReceivedField.Text = "label24";
            // 
            // resetsSentField
            // 
            this.resetsSentField.AutoSize = true;
            this.resetsSentField.Location = new System.Drawing.Point(384, 45);
            this.resetsSentField.Name = "resetsSentField";
            this.resetsSentField.Size = new System.Drawing.Size(41, 13);
            this.resetsSentField.TabIndex = 18;
            this.resetsSentField.Text = "label23";
            // 
            // maxConnField
            // 
            this.maxConnField.AutoSize = true;
            this.maxConnField.Location = new System.Drawing.Point(153, 162);
            this.maxConnField.Name = "maxConnField";
            this.maxConnField.Size = new System.Drawing.Size(41, 13);
            this.maxConnField.TabIndex = 17;
            this.maxConnField.Text = "label22";
            // 
            // failedConAttField
            // 
            this.failedConAttField.AutoSize = true;
            this.failedConAttField.Location = new System.Drawing.Point(153, 137);
            this.failedConAttField.Name = "failedConAttField";
            this.failedConAttField.Size = new System.Drawing.Size(41, 13);
            this.failedConAttField.TabIndex = 16;
            this.failedConAttField.Text = "label21";
            // 
            // errorsReceivedField
            // 
            this.errorsReceivedField.AutoSize = true;
            this.errorsReceivedField.Location = new System.Drawing.Point(153, 107);
            this.errorsReceivedField.Name = "errorsReceivedField";
            this.errorsReceivedField.Size = new System.Drawing.Size(41, 13);
            this.errorsReceivedField.TabIndex = 15;
            this.errorsReceivedField.Text = "label20";
            // 
            // cumulativeConnectionsField
            // 
            this.cumulativeConnectionsField.AutoSize = true;
            this.cumulativeConnectionsField.Location = new System.Drawing.Point(153, 79);
            this.cumulativeConnectionsField.Name = "cumulativeConnectionsField";
            this.cumulativeConnectionsField.Size = new System.Drawing.Size(41, 13);
            this.cumulativeConnectionsField.TabIndex = 14;
            this.cumulativeConnectionsField.Text = "label19";
            // 
            // connInitiatedField
            // 
            this.connInitiatedField.AutoSize = true;
            this.connInitiatedField.Location = new System.Drawing.Point(152, 46);
            this.connInitiatedField.Name = "connInitiatedField";
            this.connInitiatedField.Size = new System.Drawing.Size(41, 13);
            this.connInitiatedField.TabIndex = 13;
            this.connInitiatedField.Text = "label18";
            // 
            // resetConnField
            // 
            this.resetConnField.AutoSize = true;
            this.resetConnField.Location = new System.Drawing.Point(383, 16);
            this.resetConnField.Name = "resetConnField";
            this.resetConnField.Size = new System.Drawing.Size(41, 13);
            this.resetConnField.TabIndex = 12;
            this.resetConnField.Text = "label17";
            // 
            // connAcceptField
            // 
            this.connAcceptField.AutoSize = true;
            this.connAcceptField.Location = new System.Drawing.Point(152, 16);
            this.connAcceptField.Name = "connAcceptField";
            this.connAcceptField.Size = new System.Drawing.Size(41, 13);
            this.connAcceptField.TabIndex = 11;
            this.connAcceptField.Text = "label16";
            // 
            // segSentLabel
            // 
            this.segSentLabel.AutoSize = true;
            this.segSentLabel.Location = new System.Drawing.Point(295, 137);
            this.segSentLabel.Name = "segSentLabel";
            this.segSentLabel.Size = new System.Drawing.Size(82, 13);
            this.segSentLabel.TabIndex = 10;
            this.segSentLabel.Text = "Segments Sent:";
            // 
            // segResentLabel
            // 
            this.segResentLabel.AutoSize = true;
            this.segResentLabel.Location = new System.Drawing.Point(283, 107);
            this.segResentLabel.Name = "segResentLabel";
            this.segResentLabel.Size = new System.Drawing.Size(94, 13);
            this.segResentLabel.TabIndex = 9;
            this.segResentLabel.Text = "Segments Resent:";
            // 
            // segRecLabel
            // 
            this.segRecLabel.AutoSize = true;
            this.segRecLabel.Location = new System.Drawing.Point(271, 79);
            this.segRecLabel.Name = "segRecLabel";
            this.segRecLabel.Size = new System.Drawing.Size(106, 13);
            this.segRecLabel.TabIndex = 8;
            this.segRecLabel.Text = "Segments Received:";
            // 
            // resetsSentLabel
            // 
            this.resetsSentLabel.AutoSize = true;
            this.resetsSentLabel.Location = new System.Drawing.Point(309, 46);
            this.resetsSentLabel.Name = "resetsSentLabel";
            this.resetsSentLabel.Size = new System.Drawing.Size(68, 13);
            this.resetsSentLabel.TabIndex = 7;
            this.resetsSentLabel.Text = "Resets Sent:";
            // 
            // resetConLabel
            // 
            this.resetConLabel.AutoSize = true;
            this.resetConLabel.Location = new System.Drawing.Point(277, 16);
            this.resetConLabel.Name = "resetConLabel";
            this.resetConLabel.Size = new System.Drawing.Size(100, 13);
            this.resetConLabel.TabIndex = 6;
            this.resetConLabel.Text = "Reset Connections:";
            // 
            // maxConLabel
            // 
            this.maxConLabel.AutoSize = true;
            this.maxConLabel.Location = new System.Drawing.Point(30, 162);
            this.maxConLabel.Name = "maxConLabel";
            this.maxConLabel.Size = new System.Drawing.Size(116, 13);
            this.maxConLabel.TabIndex = 5;
            this.maxConLabel.Text = "Maximum Connections:";
            // 
            // failedConnAttLabel
            // 
            this.failedConnAttLabel.AutoSize = true;
            this.failedConnAttLabel.Location = new System.Drawing.Point(7, 137);
            this.failedConnAttLabel.Name = "failedConnAttLabel";
            this.failedConnAttLabel.Size = new System.Drawing.Size(139, 13);
            this.failedConnAttLabel.TabIndex = 4;
            this.failedConnAttLabel.Text = "Failed Connection Attempts:";
            // 
            // errRecLabel
            // 
            this.errRecLabel.AutoSize = true;
            this.errRecLabel.Location = new System.Drawing.Point(60, 107);
            this.errRecLabel.Name = "errRecLabel";
            this.errRecLabel.Size = new System.Drawing.Size(86, 13);
            this.errRecLabel.TabIndex = 3;
            this.errRecLabel.Text = "Errors Received:";
            // 
            // cumulativeConnLabel
            // 
            this.cumulativeConnLabel.AutoSize = true;
            this.cumulativeConnLabel.Location = new System.Drawing.Point(22, 79);
            this.cumulativeConnLabel.Name = "cumulativeConnLabel";
            this.cumulativeConnLabel.Size = new System.Drawing.Size(124, 13);
            this.cumulativeConnLabel.TabIndex = 2;
            this.cumulativeConnLabel.Text = "Cumulative Connections:";
            // 
            // connInitLabel
            // 
            this.connInitLabel.AutoSize = true;
            this.connInitLabel.Location = new System.Drawing.Point(37, 46);
            this.connInitLabel.Name = "connInitLabel";
            this.connInitLabel.Size = new System.Drawing.Size(109, 13);
            this.connInitLabel.TabIndex = 1;
            this.connInitLabel.Text = "Connections Initiated:";
            // 
            // connAcceptLabel
            // 
            this.connAcceptLabel.AutoSize = true;
            this.connAcceptLabel.Location = new System.Drawing.Point(26, 16);
            this.connAcceptLabel.Name = "connAcceptLabel";
            this.connAcceptLabel.Size = new System.Drawing.Size(118, 13);
            this.connAcceptLabel.TabIndex = 0;
            this.connAcceptLabel.Text = "Connections Accepted:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ipDisplay, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(553, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // IPMonitorDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "IPMonitorDisplay";
            this.Size = new System.Drawing.Size(553, 450);
            this.ipDisplay.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcpDisplay)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udpDisplay)).EndInit();
            this.statistics.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl ipDisplay;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView tcpDisplay;
        private System.Windows.Forms.Label tcpTotal;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label udpTotal;
        private System.Windows.Forms.DataGridView udpDisplay;
        private System.Windows.Forms.TabPage statistics;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label incDataErrField;
        private System.Windows.Forms.Label incDataDiscField;
        private System.Windows.Forms.Label dataSentField;
        private System.Windows.Forms.Label dataReceivedField;
        private System.Windows.Forms.Label incDataWErrLabel;
        private System.Windows.Forms.Label incDataDiscLabel;
        private System.Windows.Forms.Label dataSentLabel;
        private System.Windows.Forms.Label dataRecLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label segsSentField;
        private System.Windows.Forms.Label segsResentField;
        private System.Windows.Forms.Label segsReceivedField;
        private System.Windows.Forms.Label resetsSentField;
        private System.Windows.Forms.Label maxConnField;
        private System.Windows.Forms.Label failedConAttField;
        private System.Windows.Forms.Label errorsReceivedField;
        private System.Windows.Forms.Label cumulativeConnectionsField;
        private System.Windows.Forms.Label connInitiatedField;
        private System.Windows.Forms.Label resetConnField;
        private System.Windows.Forms.Label connAcceptField;
        private System.Windows.Forms.Label segSentLabel;
        private System.Windows.Forms.Label segResentLabel;
        private System.Windows.Forms.Label segRecLabel;
        private System.Windows.Forms.Label resetsSentLabel;
        private System.Windows.Forms.Label resetConLabel;
        private System.Windows.Forms.Label maxConLabel;
        private System.Windows.Forms.Label failedConnAttLabel;
        private System.Windows.Forms.Label errRecLabel;
        private System.Windows.Forms.Label cumulativeConnLabel;
        private System.Windows.Forms.Label connInitLabel;
        private System.Windows.Forms.Label connAcceptLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;


    }
}
