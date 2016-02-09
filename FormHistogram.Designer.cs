namespace WindowsFormsApplication1
{
    partial class FormHistogram
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDownB = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownG = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownR = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarB = new System.Windows.Forms.TrackBar();
            this.trackBarG = new System.Windows.Forms.TrackBar();
            this.trackBarR = new System.Windows.Forms.TrackBar();
            this.buttonBrighter = new System.Windows.Forms.Button();
            this.buttonDarker = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarR)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(15, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(502, 267);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(14, 18);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(251, 236);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // buttonApply
            // 
            this.buttonApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonApply.Location = new System.Drawing.Point(128, 303);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(82, 28);
            this.buttonApply.TabIndex = 1;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(415, 303);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(82, 28);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(33, 303);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(82, 28);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDownB);
            this.groupBox2.Controls.Add(this.numericUpDownG);
            this.groupBox2.Controls.Add(this.numericUpDownR);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.trackBarB);
            this.groupBox2.Controls.Add(this.trackBarG);
            this.groupBox2.Controls.Add(this.trackBarR);
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(281, 18);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(205, 235);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RGB Parameters";
            // 
            // numericUpDownB
            // 
            this.numericUpDownB.Location = new System.Drawing.Point(143, 22);
            this.numericUpDownB.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownB.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.numericUpDownB.Name = "numericUpDownB";
            this.numericUpDownB.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownB.TabIndex = 24;
            this.numericUpDownB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numericUpDownB_KeyUp);
            this.numericUpDownB.MouseUp += new System.Windows.Forms.MouseEventHandler(this.numericUpDownB_MouseUp);
            // 
            // numericUpDownG
            // 
            this.numericUpDownG.Location = new System.Drawing.Point(77, 22);
            this.numericUpDownG.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownG.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.numericUpDownG.Name = "numericUpDownG";
            this.numericUpDownG.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownG.TabIndex = 23;
            this.numericUpDownG.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numericUpDownG_KeyUp);
            this.numericUpDownG.MouseUp += new System.Windows.Forms.MouseEventHandler(this.numericUpDownG_MouseUp);
            // 
            // numericUpDownR
            // 
            this.numericUpDownR.Location = new System.Drawing.Point(13, 22);
            this.numericUpDownR.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownR.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.numericUpDownR.Name = "numericUpDownR";
            this.numericUpDownR.Size = new System.Drawing.Size(49, 20);
            this.numericUpDownR.TabIndex = 16;
            this.numericUpDownR.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numericUpDownR_KeyUp);
            this.numericUpDownR.MouseUp += new System.Windows.Forms.MouseEventHandler(this.numericUpDownR_MouseUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(153, 212);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Blue";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(84, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Green";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 212);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Red";
            // 
            // trackBarB
            // 
            this.trackBarB.LargeChange = 10;
            this.trackBarB.Location = new System.Drawing.Point(146, 40);
            this.trackBarB.Maximum = 510;
            this.trackBarB.Name = "trackBarB";
            this.trackBarB.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarB.Size = new System.Drawing.Size(45, 169);
            this.trackBarB.TabIndex = 19;
            this.trackBarB.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarB.Scroll += new System.EventHandler(this.trackBarB_Scroll);
            this.trackBarB.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarB_MouseUp);
            // 
            // trackBarG
            // 
            this.trackBarG.LargeChange = 10;
            this.trackBarG.Location = new System.Drawing.Point(81, 40);
            this.trackBarG.Maximum = 510;
            this.trackBarG.Name = "trackBarG";
            this.trackBarG.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarG.Size = new System.Drawing.Size(45, 169);
            this.trackBarG.TabIndex = 18;
            this.trackBarG.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarG.Scroll += new System.EventHandler(this.trackBarG_Scroll);
            this.trackBarG.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarG_MouseUp);
            // 
            // trackBarR
            // 
            this.trackBarR.LargeChange = 10;
            this.trackBarR.Location = new System.Drawing.Point(17, 40);
            this.trackBarR.Maximum = 510;
            this.trackBarR.Name = "trackBarR";
            this.trackBarR.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarR.Size = new System.Drawing.Size(45, 169);
            this.trackBarR.TabIndex = 17;
            this.trackBarR.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarR.Scroll += new System.EventHandler(this.trackBarR_Scroll);
            this.trackBarR.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarR_MouseUp);
            // 
            // buttonBrighter
            // 
            this.buttonBrighter.Location = new System.Drawing.Point(223, 303);
            this.buttonBrighter.Name = "buttonBrighter";
            this.buttonBrighter.Size = new System.Drawing.Size(82, 28);
            this.buttonBrighter.TabIndex = 4;
            this.buttonBrighter.Text = "Brighter";
            this.buttonBrighter.UseVisualStyleBackColor = true;
            this.buttonBrighter.Click += new System.EventHandler(this.buttonBrighter_Click);
            // 
            // buttonDarker
            // 
            this.buttonDarker.Location = new System.Drawing.Point(319, 303);
            this.buttonDarker.Name = "buttonDarker";
            this.buttonDarker.Size = new System.Drawing.Size(82, 28);
            this.buttonDarker.TabIndex = 5;
            this.buttonDarker.Text = "Darker";
            this.buttonDarker.UseVisualStyleBackColor = true;
            this.buttonDarker.Click += new System.EventHandler(this.buttonDarker_Click);
            // 
            // FormHistogram
            // 
            this.AcceptButton = this.buttonApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(531, 347);
            this.Controls.Add(this.buttonDarker);
            this.Controls.Add(this.buttonBrighter);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "FormHistogram";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Red-Green-Blue Histogram Control";
            this.Load += new System.EventHandler(this.FormHistogram_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarR)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownB;
        private System.Windows.Forms.NumericUpDown numericUpDownG;
        private System.Windows.Forms.NumericUpDown numericUpDownR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarB;
        private System.Windows.Forms.TrackBar trackBarG;
        private System.Windows.Forms.TrackBar trackBarR;
        private System.Windows.Forms.Button buttonBrighter;
        private System.Windows.Forms.Button buttonDarker;

    }
}