namespace CostAccounting
{
    partial class Form_PrePare_DataCopy
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
            this.targetYear = new System.Windows.Forms.Label();
            this.btnEnter = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.srcYear = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.targetYear);
            this.groupBox1.Controls.Add(this.btnEnter);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.srcYear);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(561, 77);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "コピー元年度　※西暦で入力してください※";
            // 
            // targetYear
            // 
            this.targetYear.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.targetYear.Font = new System.Drawing.Font("Meiryo UI", 18F);
            this.targetYear.Location = new System.Drawing.Point(217, 27);
            this.targetYear.Name = "targetYear";
            this.targetYear.Size = new System.Drawing.Size(84, 38);
            this.targetYear.TabIndex = 2;
            this.targetYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnEnter
            // 
            this.btnEnter.Location = new System.Drawing.Point(473, 42);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new System.Drawing.Size(75, 23);
            this.btnEnter.TabIndex = 4;
            this.btnEnter.Text = "決定";
            this.btnEnter.UseVisualStyleBackColor = true;
            this.btnEnter.Click += new System.EventHandler(this.btnEnter_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(307, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "年度の予定と実績にコピーする。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(109, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "年度の実績データを";
            // 
            // srcYear
            // 
            this.srcYear.Font = new System.Drawing.Font("Meiryo UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.srcYear.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.srcYear.Location = new System.Drawing.Point(19, 27);
            this.srcYear.MaxLength = 4;
            this.srcYear.Name = "srcYear";
            this.srcYear.Size = new System.Drawing.Size(84, 38);
            this.srcYear.TabIndex = 0;
            this.srcYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.srcYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress_numeric);
            // 
            // Form_PrePare_DataCopy
            // 
            this.AcceptButton = this.btnEnter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(586, 103);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Form_PrePare_DataCopy";
            this.Text = "データコピー";
            this.Load += new System.EventHandler(this.Form_PrePare_DataCopy_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnEnter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox srcYear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label targetYear;

    }
}