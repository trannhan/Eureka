namespace WindowsFormsApplication1
{
    partial class FormNewRankInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewRankInput));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBoxOldRank = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownNewRank = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxBestRank = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNewRank)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 97);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "New Rank";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(104, 140);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(84, 33);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxOldRank
            // 
            this.textBoxOldRank.Location = new System.Drawing.Point(140, 20);
            this.textBoxOldRank.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxOldRank.Name = "textBoxOldRank";
            this.textBoxOldRank.ReadOnly = true;
            this.textBoxOldRank.Size = new System.Drawing.Size(132, 22);
            this.textBoxOldRank.TabIndex = 9;
            this.textBoxOldRank.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 25);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Current Rank";
            // 
            // numericUpDownNewRank
            // 
            this.numericUpDownNewRank.Location = new System.Drawing.Point(140, 95);
            this.numericUpDownNewRank.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownNewRank.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownNewRank.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownNewRank.Name = "numericUpDownNewRank";
            this.numericUpDownNewRank.Size = new System.Drawing.Size(133, 22);
            this.numericUpDownNewRank.TabIndex = 33;
            this.numericUpDownNewRank.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 63);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 17);
            this.label3.TabIndex = 35;
            this.label3.Text = "Best Rank";
            // 
            // textBoxBestRank
            // 
            this.textBoxBestRank.Location = new System.Drawing.Point(140, 58);
            this.textBoxBestRank.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxBestRank.Name = "textBoxBestRank";
            this.textBoxBestRank.ReadOnly = true;
            this.textBoxBestRank.Size = new System.Drawing.Size(132, 22);
            this.textBoxBestRank.TabIndex = 34;
            this.textBoxBestRank.Text = "0";
            // 
            // FormNewRankInput
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 186);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxBestRank);
            this.Controls.Add(this.numericUpDownNewRank);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxOldRank);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimizeBox = false;
            this.Name = "FormNewRankInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Rank";
            this.Shown += new System.EventHandler(this.FormNewRankInput_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNewRank)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox textBoxOldRank;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownNewRank;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxBestRank;

    }
}