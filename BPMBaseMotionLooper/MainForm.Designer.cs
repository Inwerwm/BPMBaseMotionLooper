namespace BPMBaseMotionLooper
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxOpenURL = new System.Windows.Forms.TextBox();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.numericUpDownBPM = new System.Windows.Forms.NumericUpDown();
            this.labelBPM = new System.Windows.Forms.Label();
            this.numericUpDownPeriod = new System.Windows.Forms.NumericUpDown();
            this.labelPeriod = new System.Windows.Forms.Label();
            this.numericUpDownMetre = new System.Windows.Forms.NumericUpDown();
            this.labelMetre = new System.Windows.Forms.Label();
            this.numericUpDownTimes = new System.Windows.Forms.NumericUpDown();
            this.labelTimes = new System.Windows.Forms.Label();
            this.labelPeriodRemark = new System.Windows.Forms.Label();
            this.labelTimesRemark = new System.Windows.Forms.Label();
            this.buttonExcute = new System.Windows.Forms.Button();
            this.textBoxDisplay = new System.Windows.Forms.TextBox();
            this.textBoxFPB = new System.Windows.Forms.TextBox();
            this.labelFPB = new System.Windows.Forms.Label();
            this.textBoxLength = new System.Windows.Forms.TextBox();
            this.labelLength = new System.Windows.Forms.Label();
            this.checkBoxPlusOne = new System.Windows.Forms.CheckBox();
            this.checkBoxReload = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMetre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimes)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxOpenURL
            // 
            this.textBoxOpenURL.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.textBoxOpenURL.Location = new System.Drawing.Point(12, 12);
            this.textBoxOpenURL.Name = "textBoxOpenURL";
            this.textBoxOpenURL.ReadOnly = true;
            this.textBoxOpenURL.Size = new System.Drawing.Size(641, 29);
            this.textBoxOpenURL.TabIndex = 0;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.buttonOpen.Location = new System.Drawing.Point(659, 12);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(129, 29);
            this.buttonOpen.TabIndex = 1;
            this.buttonOpen.Text = "ファイルを開く";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.ButtonOpen_Click);
            // 
            // numericUpDownBPM
            // 
            this.numericUpDownBPM.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.numericUpDownBPM.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.numericUpDownBPM.Location = new System.Drawing.Point(12, 68);
            this.numericUpDownBPM.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownBPM.Name = "numericUpDownBPM";
            this.numericUpDownBPM.Size = new System.Drawing.Size(120, 29);
            this.numericUpDownBPM.TabIndex = 2;
            this.numericUpDownBPM.Value = global::BPMBaseMotionLooper.Properties.Settings.Default.saveBPM;
            this.numericUpDownBPM.ValueChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // labelBPM
            // 
            this.labelBPM.AutoSize = true;
            this.labelBPM.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelBPM.Location = new System.Drawing.Point(8, 44);
            this.labelBPM.Name = "labelBPM";
            this.labelBPM.Size = new System.Drawing.Size(42, 21);
            this.labelBPM.TabIndex = 3;
            this.labelBPM.Text = "BPM";
            // 
            // numericUpDownPeriod
            // 
            this.numericUpDownPeriod.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.numericUpDownPeriod.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.numericUpDownPeriod.Location = new System.Drawing.Point(138, 68);
            this.numericUpDownPeriod.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownPeriod.Name = "numericUpDownPeriod";
            this.numericUpDownPeriod.Size = new System.Drawing.Size(120, 29);
            this.numericUpDownPeriod.TabIndex = 2;
            this.numericUpDownPeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPeriod.ValueChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // labelPeriod
            // 
            this.labelPeriod.AutoSize = true;
            this.labelPeriod.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelPeriod.Location = new System.Drawing.Point(134, 44);
            this.labelPeriod.Name = "labelPeriod";
            this.labelPeriod.Size = new System.Drawing.Size(42, 21);
            this.labelPeriod.TabIndex = 3;
            this.labelPeriod.Text = "周期";
            // 
            // numericUpDownMetre
            // 
            this.numericUpDownMetre.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.numericUpDownMetre.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.numericUpDownMetre.Location = new System.Drawing.Point(264, 68);
            this.numericUpDownMetre.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownMetre.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMetre.Name = "numericUpDownMetre";
            this.numericUpDownMetre.Size = new System.Drawing.Size(120, 29);
            this.numericUpDownMetre.TabIndex = 2;
            this.numericUpDownMetre.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownMetre.ValueChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // labelMetre
            // 
            this.labelMetre.AutoSize = true;
            this.labelMetre.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelMetre.Location = new System.Drawing.Point(260, 44);
            this.labelMetre.Name = "labelMetre";
            this.labelMetre.Size = new System.Drawing.Size(42, 21);
            this.labelMetre.TabIndex = 3;
            this.labelMetre.Text = "拍子";
            // 
            // numericUpDownTimes
            // 
            this.numericUpDownTimes.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.numericUpDownTimes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.numericUpDownTimes.Location = new System.Drawing.Point(390, 68);
            this.numericUpDownTimes.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownTimes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTimes.Name = "numericUpDownTimes";
            this.numericUpDownTimes.Size = new System.Drawing.Size(120, 29);
            this.numericUpDownTimes.TabIndex = 2;
            this.numericUpDownTimes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTimes.ValueChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // labelTimes
            // 
            this.labelTimes.AutoSize = true;
            this.labelTimes.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelTimes.Location = new System.Drawing.Point(386, 44);
            this.labelTimes.Name = "labelTimes";
            this.labelTimes.Size = new System.Drawing.Size(42, 21);
            this.labelTimes.TabIndex = 3;
            this.labelTimes.Text = "拍数";
            // 
            // labelPeriodRemark
            // 
            this.labelPeriodRemark.AutoSize = true;
            this.labelPeriodRemark.Font = new System.Drawing.Font("Yu Gothic UI", 11F);
            this.labelPeriodRemark.Location = new System.Drawing.Point(134, 100);
            this.labelPeriodRemark.Name = "labelPeriodRemark";
            this.labelPeriodRemark.Size = new System.Drawing.Size(89, 40);
            this.labelPeriodRemark.TabIndex = 5;
            this.labelPeriodRemark.Text = "※何拍に1回\r\n　設置するか";
            // 
            // labelTimesRemark
            // 
            this.labelTimesRemark.AutoSize = true;
            this.labelTimesRemark.Font = new System.Drawing.Font("Yu Gothic UI", 11F);
            this.labelTimesRemark.Location = new System.Drawing.Point(386, 100);
            this.labelTimesRemark.Name = "labelTimesRemark";
            this.labelTimesRemark.Size = new System.Drawing.Size(122, 40);
            this.labelTimesRemark.TabIndex = 5;
            this.labelTimesRemark.Text = "※拍子×泊数回が\r\n　範囲になる";
            // 
            // buttonExcute
            // 
            this.buttonExcute.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.buttonExcute.Location = new System.Drawing.Point(659, 164);
            this.buttonExcute.Name = "buttonExcute";
            this.buttonExcute.Size = new System.Drawing.Size(129, 29);
            this.buttonExcute.TabIndex = 6;
            this.buttonExcute.Text = "実行";
            this.buttonExcute.UseVisualStyleBackColor = true;
            this.buttonExcute.Click += new System.EventHandler(this.ButtonExcute_Click);
            // 
            // textBoxDisplay
            // 
            this.textBoxDisplay.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.textBoxDisplay.Location = new System.Drawing.Point(12, 199);
            this.textBoxDisplay.Multiline = true;
            this.textBoxDisplay.Name = "textBoxDisplay";
            this.textBoxDisplay.ReadOnly = true;
            this.textBoxDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDisplay.Size = new System.Drawing.Size(641, 239);
            this.textBoxDisplay.TabIndex = 4;
            // 
            // textBoxFPB
            // 
            this.textBoxFPB.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.textBoxFPB.Location = new System.Drawing.Point(12, 164);
            this.textBoxFPB.Name = "textBoxFPB";
            this.textBoxFPB.ReadOnly = true;
            this.textBoxFPB.Size = new System.Drawing.Size(372, 29);
            this.textBoxFPB.TabIndex = 0;
            // 
            // labelFPB
            // 
            this.labelFPB.AutoSize = true;
            this.labelFPB.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelFPB.Location = new System.Drawing.Point(12, 140);
            this.labelFPB.Name = "labelFPB";
            this.labelFPB.Size = new System.Drawing.Size(241, 21);
            this.labelFPB.TabIndex = 3;
            this.labelFPB.Text = "Frame Per Beat (Where FPS = 30)";
            // 
            // textBoxLength
            // 
            this.textBoxLength.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.textBoxLength.Location = new System.Drawing.Point(390, 164);
            this.textBoxLength.Name = "textBoxLength";
            this.textBoxLength.ReadOnly = true;
            this.textBoxLength.Size = new System.Drawing.Size(263, 29);
            this.textBoxLength.TabIndex = 0;
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelLength.Location = new System.Drawing.Point(390, 140);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(58, 21);
            this.labelLength.TabIndex = 3;
            this.labelLength.Text = "設置数";
            // 
            // checkBoxPlusOne
            // 
            this.checkBoxPlusOne.AutoSize = true;
            this.checkBoxPlusOne.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.checkBoxPlusOne.Location = new System.Drawing.Point(498, 139);
            this.checkBoxPlusOne.Name = "checkBoxPlusOne";
            this.checkBoxPlusOne.Size = new System.Drawing.Size(155, 25);
            this.checkBoxPlusOne.TabIndex = 7;
            this.checkBoxPlusOne.Text = "最後に1回追加する";
            this.checkBoxPlusOne.UseVisualStyleBackColor = true;
            this.checkBoxPlusOne.CheckedChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // checkBoxReload
            // 
            this.checkBoxReload.AutoSize = true;
            this.checkBoxReload.Checked = global::BPMBaseMotionLooper.Properties.Settings.Default.saveReload;
            //this.checkBoxReload.CheckState = global::BPMBaseMotionLooper.Properties.Settings.Default.saveReload ? System.Windows.Forms.CheckState.Checked : System.Windows.Forms.CheckState.Unchecked;
            this.checkBoxReload.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.checkBoxReload.Location = new System.Drawing.Point(659, 139);
            this.checkBoxReload.Name = "checkBoxReload";
            this.checkBoxReload.Size = new System.Drawing.Size(137, 25);
            this.checkBoxReload.TabIndex = 7;
            this.checkBoxReload.Text = "実行時に再読込";
            this.checkBoxReload.UseVisualStyleBackColor = true;
            this.checkBoxReload.CheckedChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.checkBoxReload);
            this.Controls.Add(this.checkBoxPlusOne);
            this.Controls.Add(this.buttonExcute);
            this.Controls.Add(this.labelTimesRemark);
            this.Controls.Add(this.labelPeriodRemark);
            this.Controls.Add(this.textBoxDisplay);
            this.Controls.Add(this.labelTimes);
            this.Controls.Add(this.labelPeriod);
            this.Controls.Add(this.labelMetre);
            this.Controls.Add(this.labelLength);
            this.Controls.Add(this.labelFPB);
            this.Controls.Add(this.labelBPM);
            this.Controls.Add(this.numericUpDownTimes);
            this.Controls.Add(this.numericUpDownMetre);
            this.Controls.Add(this.numericUpDownPeriod);
            this.Controls.Add(this.numericUpDownBPM);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.textBoxLength);
            this.Controls.Add(this.textBoxFPB);
            this.Controls.Add(this.textBoxOpenURL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BPM Base Motion Looper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMetre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTimes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxOpenURL;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.NumericUpDown numericUpDownBPM;
        private System.Windows.Forms.Label labelBPM;
        private System.Windows.Forms.NumericUpDown numericUpDownPeriod;
        private System.Windows.Forms.Label labelPeriod;
        private System.Windows.Forms.NumericUpDown numericUpDownMetre;
        private System.Windows.Forms.Label labelMetre;
        private System.Windows.Forms.NumericUpDown numericUpDownTimes;
        private System.Windows.Forms.Label labelTimes;
        private System.Windows.Forms.Label labelPeriodRemark;
        private System.Windows.Forms.Label labelTimesRemark;
        private System.Windows.Forms.Button buttonExcute;
        private System.Windows.Forms.TextBox textBoxDisplay;
        private System.Windows.Forms.TextBox textBoxFPB;
        private System.Windows.Forms.Label labelFPB;
        private System.Windows.Forms.TextBox textBoxLength;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.CheckBox checkBoxPlusOne;
        private System.Windows.Forms.CheckBox checkBoxReload;
    }
}

