namespace WindowsFormsApplication1
{
    partial class FormSystemOfEquationsInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSystemOfEquationsInput));
            this.dataGridViewMatrix = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonRandom = new System.Windows.Forms.RadioButton();
            this.radioButtonSymetric = new System.Windows.Forms.RadioButton();
            this.radioButtonIdentity = new System.Windows.Forms.RadioButton();
            this.radioButtonDiagonal = new System.Windows.Forms.RadioButton();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownCol = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRow = new System.Windows.Forms.NumericUpDown();
            this.buttonGenerateMatrix = new System.Windows.Forms.Button();
            this.buttonClearAll = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownMax = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMin = new System.Windows.Forms.NumericUpDown();
            this.dataGridViewVectorb = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatrix)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRow)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVectorb)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewMatrix
            // 
            this.dataGridViewMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMatrix.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMatrix.Name = "dataGridViewMatrix";
            this.dataGridViewMatrix.Size = new System.Drawing.Size(599, 395);
            this.dataGridViewMatrix.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.radioButtonRandom);
            this.groupBox1.Controls.Add(this.radioButtonSymetric);
            this.groupBox1.Controls.Add(this.radioButtonIdentity);
            this.groupBox1.Controls.Add(this.radioButtonDiagonal);
            this.groupBox1.Location = new System.Drawing.Point(90, 413);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(615, 58);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Matrix Type";
            // 
            // radioButtonRandom
            // 
            this.radioButtonRandom.AutoSize = true;
            this.radioButtonRandom.Checked = true;
            this.radioButtonRandom.Location = new System.Drawing.Point(26, 24);
            this.radioButtonRandom.Name = "radioButtonRandom";
            this.radioButtonRandom.Size = new System.Drawing.Size(96, 17);
            this.radioButtonRandom.TabIndex = 8;
            this.radioButtonRandom.TabStop = true;
            this.radioButtonRandom.Text = "Random Matrix";
            this.radioButtonRandom.UseVisualStyleBackColor = true;
            // 
            // radioButtonSymetric
            // 
            this.radioButtonSymetric.AutoSize = true;
            this.radioButtonSymetric.Location = new System.Drawing.Point(496, 24);
            this.radioButtonSymetric.Name = "radioButtonSymetric";
            this.radioButtonSymetric.Size = new System.Drawing.Size(96, 17);
            this.radioButtonSymetric.TabIndex = 7;
            this.radioButtonSymetric.Text = "Symetric Matrix";
            this.radioButtonSymetric.UseVisualStyleBackColor = true;
            // 
            // radioButtonIdentity
            // 
            this.radioButtonIdentity.AutoSize = true;
            this.radioButtonIdentity.Location = new System.Drawing.Point(185, 24);
            this.radioButtonIdentity.Name = "radioButtonIdentity";
            this.radioButtonIdentity.Size = new System.Drawing.Size(90, 17);
            this.radioButtonIdentity.TabIndex = 6;
            this.radioButtonIdentity.Text = "Identity Matrix";
            this.radioButtonIdentity.UseVisualStyleBackColor = true;
            // 
            // radioButtonDiagonal
            // 
            this.radioButtonDiagonal.AutoSize = true;
            this.radioButtonDiagonal.Location = new System.Drawing.Point(338, 24);
            this.radioButtonDiagonal.Name = "radioButtonDiagonal";
            this.radioButtonDiagonal.Size = new System.Drawing.Size(98, 17);
            this.radioButtonDiagonal.TabIndex = 5;
            this.radioButtonDiagonal.Text = "Diagonal Matrix";
            this.radioButtonDiagonal.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(528, 558);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(97, 29);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numericUpDownCol);
            this.groupBox2.Controls.Add(this.numericUpDownRow);
            this.groupBox2.Location = new System.Drawing.Point(90, 486);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(286, 58);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Matrix Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(152, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Columns";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Rows";
            // 
            // numericUpDownCol
            // 
            this.numericUpDownCol.Location = new System.Drawing.Point(205, 21);
            this.numericUpDownCol.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDownCol.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCol.Name = "numericUpDownCol";
            this.numericUpDownCol.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownCol.TabIndex = 1;
            this.numericUpDownCol.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownCol.ValueChanged += new System.EventHandler(this.numericUpDownColValueChange);
            // 
            // numericUpDownRow
            // 
            this.numericUpDownRow.Location = new System.Drawing.Point(64, 21);
            this.numericUpDownRow.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDownRow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownRow.Name = "numericUpDownRow";
            this.numericUpDownRow.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownRow.TabIndex = 0;
            this.numericUpDownRow.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownRow.ValueChanged += new System.EventHandler(this.numericUpDownRowValueChange);
            // 
            // buttonGenerateMatrix
            // 
            this.buttonGenerateMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGenerateMatrix.Location = new System.Drawing.Point(349, 558);
            this.buttonGenerateMatrix.Name = "buttonGenerateMatrix";
            this.buttonGenerateMatrix.Size = new System.Drawing.Size(97, 29);
            this.buttonGenerateMatrix.TabIndex = 10;
            this.buttonGenerateMatrix.Text = "Generate Matrix";
            this.buttonGenerateMatrix.UseVisualStyleBackColor = true;
            this.buttonGenerateMatrix.Click += new System.EventHandler(this.buttonGenerateMatrix_Click);
            // 
            // buttonClearAll
            // 
            this.buttonClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearAll.Location = new System.Drawing.Point(174, 558);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Size = new System.Drawing.Size(97, 29);
            this.buttonClearAll.TabIndex = 11;
            this.buttonClearAll.Text = "Clear All";
            this.buttonClearAll.UseVisualStyleBackColor = true;
            this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.numericUpDownMax);
            this.groupBox3.Controls.Add(this.numericUpDownMin);
            this.groupBox3.Location = new System.Drawing.Point(419, 486);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(286, 58);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Matrix Value";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(146, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Max Value";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Min Value";
            // 
            // numericUpDownMax
            // 
            this.numericUpDownMax.Location = new System.Drawing.Point(209, 21);
            this.numericUpDownMax.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownMax.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.numericUpDownMax.Name = "numericUpDownMax";
            this.numericUpDownMax.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownMax.TabIndex = 1;
            this.numericUpDownMax.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // numericUpDownMin
            // 
            this.numericUpDownMin.Location = new System.Drawing.Point(80, 21);
            this.numericUpDownMin.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericUpDownMin.Minimum = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.numericUpDownMin.Name = "numericUpDownMin";
            this.numericUpDownMin.Size = new System.Drawing.Size(54, 20);
            this.numericUpDownMin.TabIndex = 0;
            // 
            // dataGridViewVectorb
            // 
            this.dataGridViewVectorb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewVectorb.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewVectorb.Location = new System.Drawing.Point(707, 0);
            this.dataGridViewVectorb.Name = "dataGridViewVectorb";
            this.dataGridViewVectorb.Size = new System.Drawing.Size(95, 395);
            this.dataGridViewVectorb.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(599, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 46);
            this.label5.TabIndex = 13;
            this.label5.Text = "* X =";
            // 
            // FormSystemOfEquationsInput
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 601);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataGridViewVectorb);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.buttonClearAll);
            this.Controls.Add(this.buttonGenerateMatrix);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dataGridViewMatrix);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormSystemOfEquationsInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System of Equations Input: Ax = b";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatrix)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRow)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVectorb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewMatrix;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.RadioButton radioButtonDiagonal;
        private System.Windows.Forms.RadioButton radioButtonRandom;
        private System.Windows.Forms.RadioButton radioButtonSymetric;
        private System.Windows.Forms.RadioButton radioButtonIdentity;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownCol;
        private System.Windows.Forms.NumericUpDown numericUpDownRow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonGenerateMatrix;
        private System.Windows.Forms.Button buttonClearAll;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownMax;
        private System.Windows.Forms.NumericUpDown numericUpDownMin;
        private System.Windows.Forms.DataGridView dataGridViewVectorb;
        private System.Windows.Forms.Label label5;
    }
}