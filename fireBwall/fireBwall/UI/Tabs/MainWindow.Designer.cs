namespace fireBwall.UI.Tabs
{
		partial class MainWindow: fireBwall.UI.DynamicForm 
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

			/// <summary>
			/// Required method for Designer support - do not modify
			/// the contents of this method with the code editor.
			/// </summary>
			private void InitializeComponent() 
            {
                this.splitContainer1 = new System.Windows.Forms.SplitContainer();
                this.tabPage4 = new System.Windows.Forms.Button();
                this.tabPage3 = new System.Windows.Forms.Button();
                this.tabPage2 = new System.Windows.Forms.Button();
                this.tabPage1 = new System.Windows.Forms.Button();
                ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
                this.splitContainer1.Panel1.SuspendLayout();
                this.splitContainer1.SuspendLayout();
                this.SuspendLayout();
                // 
                // splitContainer1
                // 
                this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
                this.splitContainer1.Location = new System.Drawing.Point(0, 0);
                this.splitContainer1.Name = "splitContainer1";
                this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
                // 
                // splitContainer1.Panel1
                // 
                this.splitContainer1.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                this.splitContainer1.Panel1.Controls.Add(this.tabPage4);
                this.splitContainer1.Panel1.Controls.Add(this.tabPage3);
                this.splitContainer1.Panel1.Controls.Add(this.tabPage2);
                this.splitContainer1.Panel1.Controls.Add(this.tabPage1);
                this.splitContainer1.Size = new System.Drawing.Size(794, 572);
                this.splitContainer1.SplitterDistance = 97;
                this.splitContainer1.TabIndex = 4;
                this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
                // 
                // tabPage4
                // 
                this.tabPage4.AutoSize = true;
                this.tabPage4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.tabPage4.FlatAppearance.BorderSize = 0;
                this.tabPage4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.tabPage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.tabPage4.Location = new System.Drawing.Point(703, 35);
                this.tabPage4.Name = "tabPage4";
                this.tabPage4.Size = new System.Drawing.Size(56, 30);
                this.tabPage4.TabIndex = 4;
                this.tabPage4.Text = "Help";
                this.tabPage4.UseVisualStyleBackColor = true;
                this.tabPage4.Click += new System.EventHandler(this.tabPage4_Click);
                // 
                // tabPage3
                // 
                this.tabPage3.AutoSize = true;
                this.tabPage3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.tabPage3.FlatAppearance.BorderSize = 0;
                this.tabPage3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.tabPage3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.tabPage3.Location = new System.Drawing.Point(494, 35);
                this.tabPage3.Name = "tabPage3";
                this.tabPage3.Size = new System.Drawing.Size(96, 30);
                this.tabPage3.TabIndex = 3;
                this.tabPage3.Text = "tabPage1";
                this.tabPage3.UseVisualStyleBackColor = true;
                this.tabPage3.Click += new System.EventHandler(this.tabPage3_Click);
                // 
                // tabPage2
                // 
                this.tabPage2.AutoSize = true;
                this.tabPage2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.tabPage2.FlatAppearance.BorderSize = 0;
                this.tabPage2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.tabPage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.tabPage2.Location = new System.Drawing.Point(176, 35);
                this.tabPage2.Name = "tabPage2";
                this.tabPage2.Size = new System.Drawing.Size(96, 30);
                this.tabPage2.TabIndex = 2;
                this.tabPage2.Text = "tabPage1";
                this.tabPage2.UseVisualStyleBackColor = true;
                this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
                // 
                // tabPage1
                // 
                this.tabPage1.AutoSize = true;
                this.tabPage1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                this.tabPage1.FlatAppearance.BorderSize = 0;
                this.tabPage1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.tabPage1.Location = new System.Drawing.Point(26, 35);
                this.tabPage1.Name = "tabPage1";
                this.tabPage1.Size = new System.Drawing.Size(96, 30);
                this.tabPage1.TabIndex = 1;
                this.tabPage1.Text = "tabPage1";
                this.tabPage1.UseVisualStyleBackColor = true;
                this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
                // 
                // MainWindow
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.BackColor = System.Drawing.Color.Black;
                this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
                this.ClientSize = new System.Drawing.Size(794, 572);
                this.Controls.Add(this.splitContainer1);
                this.DoubleBuffered = true;
                this.ForeColor = System.Drawing.Color.White;
                this.Name = "MainWindow";
                this.Text = "fireBwall v0.3.12.1";
                this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
                this.Load += new System.EventHandler(this.MainWindow_Load);
                this.Resize += new System.EventHandler(this.MainWindow_Resize);
                this.splitContainer1.Panel1.ResumeLayout(false);
                this.splitContainer1.Panel1.PerformLayout();
                ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
                this.splitContainer1.ResumeLayout(false);
                this.ResumeLayout(false);

            }
            private System.Windows.Forms.SplitContainer splitContainer1;
            private System.Windows.Forms.Button tabPage4;
            private System.Windows.Forms.Button tabPage3;
            private System.Windows.Forms.Button tabPage2;
            private System.Windows.Forms.Button tabPage1;
		}
    
}

