namespace NewspaperDesigner
{
    partial class frmNewNewspaper
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
			this.ribbon1 = new Janus.Windows.Ribbon.Ribbon();
			this.cbNewspaper = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.dtpDate = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.btCreate = new System.Windows.Forms.Button();
			this.nudNumber = new System.Windows.Forms.NumericUpDown();
			this.nudPages = new System.Windows.Forms.NumericUpDown();
			this.nudW = new System.Windows.Forms.NumericUpDown();
			this.nudH = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.ribbon1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudNumber)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPages)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudW)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudH)).BeginInit();
			this.SuspendLayout();
			// 
			// ribbon1
			// 
			this.ribbon1.Location = new System.Drawing.Point(0, 0);
			this.ribbon1.Minimized = true;
			this.ribbon1.Name = "ribbon1";
			this.ribbon1.Size = new System.Drawing.Size(339, 56);
			// 
			// 
			// 
			this.ribbon1.SuperTipComponent.AutoPopDelay = 2000;
			this.ribbon1.SuperTipComponent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ribbon1.SuperTipComponent.ImageList = null;
			this.ribbon1.TabIndex = 0;
			// 
			// cbNewspaper
			// 
			this.cbNewspaper.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbNewspaper.FormattingEnabled = true;
			this.cbNewspaper.Items.AddRange(new object[] {
            "Tuổi trẻ",
            "Thanh niên"});
			this.cbNewspaper.Location = new System.Drawing.Point(169, 75);
			this.cbNewspaper.Name = "cbNewspaper";
			this.cbNewspaper.Size = new System.Drawing.Size(121, 24);
			this.cbNewspaper.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(35, 78);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Newspaper:";
			// 
			// dtpDate
			// 
			this.dtpDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpDate.Location = new System.Drawing.Point(169, 156);
			this.dtpDate.Name = "dtpDate";
			this.dtpDate.Size = new System.Drawing.Size(100, 22);
			this.dtpDate.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(35, 117);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 16);
			this.label2.TabIndex = 5;
			this.label2.Text = "Number:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(35, 162);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "Date:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(35, 199);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(115, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "Number of pages:";
			// 
			// btCreate
			// 
			this.btCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btCreate.Location = new System.Drawing.Point(119, 328);
			this.btCreate.Name = "btCreate";
			this.btCreate.Size = new System.Drawing.Size(83, 30);
			this.btCreate.TabIndex = 9;
			this.btCreate.Text = "Create";
			this.btCreate.UseVisualStyleBackColor = true;
			this.btCreate.Click += new System.EventHandler(this.btCreate_Click);
			// 
			// nudNumber
			// 
			this.nudNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nudNumber.Location = new System.Drawing.Point(170, 115);
			this.nudNumber.Name = "nudNumber";
			this.nudNumber.Size = new System.Drawing.Size(49, 22);
			this.nudNumber.TabIndex = 10;
			// 
			// nudPages
			// 
			this.nudPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nudPages.Location = new System.Drawing.Point(170, 197);
			this.nudPages.Name = "nudPages";
			this.nudPages.Size = new System.Drawing.Size(46, 22);
			this.nudPages.TabIndex = 11;
			// 
			// nudW
			// 
			this.nudW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nudW.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudW.Location = new System.Drawing.Point(170, 235);
			this.nudW.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
			this.nudW.Name = "nudW";
			this.nudW.Size = new System.Drawing.Size(43, 22);
			this.nudW.TabIndex = 14;
			this.nudW.Value = new decimal(new int[] {
            276,
            0,
            0,
            0});
			// 
			// nudH
			// 
			this.nudH.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nudH.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudH.Location = new System.Drawing.Point(169, 277);
			this.nudH.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
			this.nudH.Name = "nudH";
			this.nudH.Size = new System.Drawing.Size(42, 22);
			this.nudH.TabIndex = 15;
			this.nudH.Value = new decimal(new int[] {
            405,
            0,
            0,
            0});
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(35, 237);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(86, 16);
			this.label5.TabIndex = 16;
			this.label5.Text = "Default width:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(35, 279);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(92, 16);
			this.label6.TabIndex = 17;
			this.label6.Text = "Default height:";
			// 
			// frmNewNewspaper
			// 
			this.AcceptButton = this.btCreate;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(339, 370);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.nudH);
			this.Controls.Add(this.nudW);
			this.Controls.Add(this.nudPages);
			this.Controls.Add(this.nudNumber);
			this.Controls.Add(this.btCreate);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.dtpDate);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbNewspaper);
			this.Controls.Add(this.ribbon1);
			this.Name = "frmNewNewspaper";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Newspaper";
			this.Load += new System.EventHandler(this.New_Load);
			((System.ComponentModel.ISupportInitialize)(this.ribbon1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudNumber)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPages)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudW)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudH)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private Janus.Windows.Ribbon.Ribbon ribbon1;
        private System.Windows.Forms.ComboBox cbNewspaper;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btCreate;
        private System.Windows.Forms.NumericUpDown nudNumber;
        private System.Windows.Forms.NumericUpDown nudPages;
        private System.Windows.Forms.NumericUpDown nudW;
        private System.Windows.Forms.NumericUpDown nudH;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}