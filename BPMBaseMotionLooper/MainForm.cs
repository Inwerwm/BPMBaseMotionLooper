using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using VmdIO;

namespace BPMBaseMotionLooper
{
    public partial class MainForm : Form
    {
        VocaloidMotionData vmd;
        string filePath = null;

        public MainForm()
        {
            InitializeComponent();
            vmd = new VocaloidMotionData();
            CalculateFPB();
            CaluculateLoopTimes();
        }

        private void OpenFile(string path)
        {
            vmd.Clear();
            try
            {
                var reader = new BinaryReader(File.OpenRead(path));
                vmd.Read(reader);
                reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("非対応のファイル形式です。");
                return;
            }
            filePath = path;
            textBoxOpenURL.Text = filePath;
            textBoxDisplay.Text = "入力キーフレーム数：" + (vmd.MotionFrames.Count + vmd.MorphFrames.Count).ToString() + Environment.NewLine;
        }

        private decimal CalculateFPB()
        {
            var fpb = numericUpDownBPM.Value == 0 ? -1 : (30 / (numericUpDownBPM.Value / 60));
            textBoxFPB.Text = fpb == -1 ? "-------------------------------" : fpb.ToString();
            return fpb;
        }

        private void CaluculateLoopTimes()
        {
            var times = numericUpDownMetre.Value * numericUpDownTimes.Value / numericUpDownPeriod.Value;
            if (checkBoxPlusOne.Checked)
                times++;
            textBoxLength.Text = times.ToString();
        }

        private List<T> GetLoopFrame<T>(List<T> frames) where T : IVmdFrameData
        {
            var fpb = CalculateFPB();
            if (fpb == -1)
                throw new DivideByZeroException();

            var loopFrame = new List<T>();
            for (decimal i = 0; i < numericUpDownMetre.Value * numericUpDownTimes.Value; i += numericUpDownPeriod.Value)
            {
                foreach (var frame in frames)
                {
                    var element = frame;
                    element.FrameTime += (uint)decimal.Round(fpb * i);
                    loopFrame.Add(element);
                }
            }

            if (checkBoxPlusOne.Checked)
            {
                foreach (var frame in frames)
                {
                    var element = frame;
                    element.FrameTime += (uint)decimal.Round(fpb * numericUpDownMetre.Value * numericUpDownTimes.Value);
                    loopFrame.Add(element);
                }
            }

            return loopFrame;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            OpenFile(files[0]);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ButtonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "vmdファイル(*.vmd)|*.vmd|全てのファイル(*.*)|*.*";
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
                OpenFile(ofd.FileName);
        }

        private void NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CalculateFPB();
                CaluculateLoopTimes();
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void ButtonExcute_Click(object sender, EventArgs e)
        {
            try
            {
                if (filePath == null)
                    throw new InvalidOperationException();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("vmdファイルを読み込んでください。");
                return;
            }

            var vmdMotionBuffer = vmd.MotionFrames;
            try
            {
                vmd.MotionFrames = GetLoopFrame(vmd.MotionFrames);
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("BPMが0です。");
                return;
            }


            var vmdMorphBuffer = vmd.MorphFrames;
            try
            {
                vmd.MorphFrames = GetLoopFrame(vmd.MorphFrames);
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("BPMが0です。");
                return;
            }

            if (checkBoxReload.Checked)
            {
                OpenFile(filePath);
            }

            string writePath;
            writePath = Path.GetDirectoryName(filePath) + @"\" + Path.GetFileNameWithoutExtension(filePath) + "_loop.vmd";
            textBoxDisplay.AppendText("出力データパス：" + writePath + Environment.NewLine);
            try
            {
                var writer = new BinaryWriter(File.OpenWrite(writePath));
                vmd.Write(writer);
                writer.Close();

            }
            catch (Exception)
            {
                MessageBox.Show("データ出力に失敗しました。");
            }

            textBoxDisplay.AppendText("出力キーフレーム数：" + (vmd.MotionFrames.Count + vmd.MorphFrames.Count).ToString() + Environment.NewLine);
            vmd.MotionFrames = vmdMotionBuffer;
            vmd.MorphFrames = vmdMorphBuffer;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.saveBPM = numericUpDownBPM.Value;
            Properties.Settings.Default.saveReload = checkBoxReload.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
