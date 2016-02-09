namespace WindowsFormsApplication1
{
    partial class FormGaussianSmooth
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDownRow = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownCol = new System.Windows.Forms.NumericUpDown();
            this.textBoxTheta = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxSigma2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxSigma1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonRunGaussian = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(532, 256);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDownRow);
            this.groupBox2.Controls.Add(this.numericUpDownCol);
            this.groupBox2.Controls.Add(this.textBoxTheta);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.textBoxSigma2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxSigma1);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(288, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(228, 213);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Gaussian Parameters";
            // 
            // numericUpDownRow
            // 
            this.numericUpDownRow.Location = new System.Drawing.Point(106, 28);
            this.numericUpDownRow.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownRow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownRow.Name = "numericUpDownRow";
            this.numericUpDownRow.Size = new System.Drawing.Size(100, 20);
            this.numericUpDownRow.TabIndex = 32;
            this.numericUpDownRow.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // numericUpDownCol
            // 
            this.numericUpDownCol.Location = new System.Drawing.Point(106, 65);
            this.numericUpDownCol.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownCol.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCol.Name = "numericUpDownCol";
            this.numericUpDownCol.Size = new System.Drawing.Size(100, 20);
            this.numericUpDownCol.TabIndex = 31;
            this.numericUpDownCol.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // textBoxTheta
            // 
            this.textBoxTheta.Location = new System.Drawing.Point(106, 172);
            this.textBoxTheta.Name = "textBoxTheta";
            this.textBoxTheta.Size = new System.Drawing.Size(100, 20);
            this.textBoxTheta.TabIndex = 30;
            this.textBoxTheta.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Theta";
            // 
            // textBoxSigma2
            // 
            this.textBoxSigma2.Location = new System.Drawing.Point(106, 137);
            this.textBoxSigma2.Name = "textBoxSigma2";
            this.textBoxSigma2.Size = new System.Drawing.Size(100, 20);
            this.textBoxSigma2.TabIndex = 28;
            this.textBoxSigma2.Text = "0.4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Sigma 2";
            // 
            // textBoxSigma1
            // 
            this.textBoxSigma1.Location = new System.Drawing.Point(106, 101);
            this.textBoxSigma1.Name = "textBoxSigma1";
            this.textBoxSigma1.Size = new System.Drawing.Size(100, 20);
            this.textBoxSigma1.TabIndex = 26;
            this.textBoxSigma1.Text = "0.4";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "Sigma 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Kernel Columns";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Kernel Rows";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(14, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(258, 213);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // buttonRunGaussian
            // 
            this.buttonRunGaussian.Location = new System.Drawing.Point(296, 286);
            this.buttonRunGaussian.Name = "buttonRunGaussian";
            this.buttonRunGaussian.Size = new System.Drawing.Size(82, 28);
            this.buttonRunGaussian.TabIndex = 6;
            this.buttonRunGaussian.Text = "Blur";
            this.buttonRunGaussian.UseVisualStyleBackColor = true;
            this.buttonRunGaussian.Click += new System.EventHandler(this.buttonRunGaussian_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonApply.Location = new System.Drawing.Point(175, 286);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(82, 28);
            this.buttonApply.TabIndex = 5;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(55, 286);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(82, 28);
            this.buttonReset.TabIndex = 7;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(417, 286);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(82, 28);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // FormGaussianSmooth
            // 
            this.AcceptButton = this.buttonApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(556, 331);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonRunGaussian);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonReset);
            this.MaximizeBox = false;
            this.Name = "FormGaussianSmooth";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gaussian Smoothing - Noise Reduction";
            this.Load += new System.EventHandler(this.FormGaussianSmooth_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonRunGaussian;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxTheta;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSigma2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxSigma1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownRow;
        private System.Windows.Forms.NumericUpDown numericUpDownCol;
    }
}