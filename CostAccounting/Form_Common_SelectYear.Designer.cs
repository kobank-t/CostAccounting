namespace CostAccounting
{
    partial class Form_Common_SelectYear
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
            this.label5 = new System.Windows.Forms.Label();
            this.targetYear = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEnter = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Meiryo UI", 16F);
            this.label5.Location = new System.Drawing.Point(3, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(406, 28);
            this.label5.TabIndex = 2;
            this.label5.Text = "浦幌フリーズドライ株式会社　原価計算ソフト";
            // 
            // targetYear
            // 
            this.targetYear.Font = new System.Drawing.Font("Meiryo UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.targetYear.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.targetYear.Location = new System.Drawing.Point(19, 27);
            this.targetYear.MaxLength = 4;
            this.targetYear.Name = "targetYear";
            this.targetYear.Size = new System.Drawing.Size(84, 38);
            this.targetYear.TabIndex = 3;
            this.targetYear.Text = "2017";
            this.targetYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(109, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "年度";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEnter);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.targetYear);
            this.groupBox1.Location = new System.Drawing.Point(39, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(322, 77);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "対象年度　※西暦で入力してください※";
            // 
            // btnEnter
            // 
            this.btnEnter.Location = new System.Drawing.Point(241, 42);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(75, 23);
            this.btnEnter.TabIndex = 6;
            this.btnEnter.Text = "決定";
            this.btnEnter.UseVisualStyleBackColor = true;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // Form_Common_SelectYear
            // 
            this.AcceptButton = this.btnEnter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(414, 138);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Form_Common_SelectYear";
            this.Text = "年度入力";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox targetYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnEnter;

    }
}