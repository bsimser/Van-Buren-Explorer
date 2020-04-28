namespace VanBurenExplorerLib.Views
{
    partial class UserControl1
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.waveViewer1 = new NAudio.Gui.WaveViewer();
            this.volumeMeter1 = new NAudio.Gui.VolumeMeter();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.progressLog1 = new NAudio.Utils.ProgressLog();
            this.waveformPainter1 = new NAudio.Gui.WaveformPainter();
            this.SuspendLayout();
            // 
            // waveViewer1
            // 
            this.waveViewer1.Location = new System.Drawing.Point(23, 153);
            this.waveViewer1.Name = "waveViewer1";
            this.waveViewer1.SamplesPerPixel = 128;
            this.waveViewer1.Size = new System.Drawing.Size(411, 62);
            this.waveViewer1.StartPosition = ((long)(0));
            this.waveViewer1.TabIndex = 0;
            this.waveViewer1.WaveStream = null;
            // 
            // volumeMeter1
            // 
            this.volumeMeter1.Amplitude = 0F;
            this.volumeMeter1.Location = new System.Drawing.Point(23, 16);
            this.volumeMeter1.MaxDb = 18F;
            this.volumeMeter1.MinDb = -60F;
            this.volumeMeter1.Name = "volumeMeter1";
            this.volumeMeter1.Size = new System.Drawing.Size(138, 23);
            this.volumeMeter1.TabIndex = 1;
            this.volumeMeter1.Text = "volumeMeter1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(23, 253);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(167, 253);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(359, 253);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // progressLog1
            // 
            this.progressLog1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.progressLog1.Location = new System.Drawing.Point(167, 16);
            this.progressLog1.Name = "progressLog1";
            this.progressLog1.Padding = new System.Windows.Forms.Padding(1);
            this.progressLog1.Size = new System.Drawing.Size(267, 131);
            this.progressLog1.TabIndex = 5;
            // 
            // waveformPainter1
            // 
            this.waveformPainter1.Location = new System.Drawing.Point(23, 45);
            this.waveformPainter1.Name = "waveformPainter1";
            this.waveformPainter1.Size = new System.Drawing.Size(138, 102);
            this.waveformPainter1.TabIndex = 6;
            this.waveformPainter1.Text = "waveformPainter1";
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.waveformPainter1);
            this.Controls.Add(this.progressLog1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.volumeMeter1);
            this.Controls.Add(this.waveViewer1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(459, 303);
            this.ResumeLayout(false);

        }

        #endregion

        private NAudio.Gui.WaveViewer waveViewer1;
        private NAudio.Gui.VolumeMeter volumeMeter1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private NAudio.Utils.ProgressLog progressLog1;
        private NAudio.Gui.WaveformPainter waveformPainter1;
    }
}
