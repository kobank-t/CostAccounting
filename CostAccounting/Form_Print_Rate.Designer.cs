namespace CostAccounting
{
    partial class Form_Print_Rate
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
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnRefOutputDir = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.outputDir = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.category = new System.Windows.Forms.GroupBox();
            this.radioActual = new System.Windows.Forms.RadioButton();
            this.radioBudget = new System.Windows.Forms.RadioButton();
            this.btnOutput = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.category.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "出力フォルダを指定してください。";
            // 
            // btnRefOutputDir
            // 
            this.btnRefOutputDir.Location = new System.Drawing.Point(627, 12);
            this.btnRefOutputDir.Name = "btnRefOutputDir";
            this.btnRefOutputDir.Size = new System.Drawing.Size(75, 23);
            this.btnRefOutputDir.TabIndex = 0;
            this.btnRefOutputDir.Text = "変更";
            this.btnRefOutputDir.UseVisualStyleBackColor = true;
            this.btnRefOutputDir.Click += new System.EventHandler(this.btnRefOutputDir_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "出力フォルダ：";
            // 
            // outputDir
            // 
            this.outputDir.Location = new System.Drawing.Point(96, 13);
            this.outputDir.Name = "outputDir";
            this.outputDir.ReadOnly = true;
            this.outputDir.Size = new System.Drawing.Size(525, 23);
            this.outputDir.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.category);
            this.groupBox1.Location = new System.Drawing.Point(12, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(133, 55);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "出力条件";
            // 
            // category
            // 
            this.category.Controls.Add(this.radioActual);
            this.category.Controls.Add(this.radioBudget);
            this.category.Location = new System.Drawing.Point(6, 13);
            this.category.Name = "category";
            this.category.Size = new System.Drawing.Size(118, 36);
            this.category.TabIndex = 1;
            this.category.TabStop = false;
            // 
            // radioActual
            // 
            this.radioActual.AutoSize = true;
            this.radioActual.Checked = true;
            this.radioActual.Location = new System.Drawing.Point(61, 12);
            this.radioActual.Name = "radioActual";
            this.radioActual.Size = new System.Drawing.Size(49, 19);
            this.radioActual.TabIndex = 1;
            this.radioActual.TabStop = true;
            this.radioActual.Text = "実績";
            this.radioActual.UseVisualStyleBackColor = true;
            // 
            // radioBudget
            // 
            this.radioBudget.AutoSize = true;
            this.radioBudget.Location = new System.Drawing.Point(6, 12);
            this.radioBudget.Name = "radioBudget";
            this.radioBudget.Size = new System.Drawing.Size(49, 19);
            this.radioBudget.TabIndex = 0;
            this.radioBudget.Text = "予定";
            this.radioBudget.UseVisualStyleBackColor = true;
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(161, 90);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(75, 23);
            this.btnOutput.TabIndex = 10;
            this.btnOutput.Text = "出力";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Font = new System.Drawing.Font("Meiryo UI", 0.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(12, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(690, 2);
            this.label3.TabIndex = 11;
            // 
            // Form_Print_Rate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 125);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.outputDir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRefOutputDir);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Form_Print_Rate";
            this.Text = "レート表出力";
            this.Load += new System.EventHandler(this.Form_Print_Product_Load);
            this.groupBox1.ResumeLayout(false);
            this.category.ResumeLayout(false);
            this.category.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btnRefOutputDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox outputDir;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox category;
        private System.Windows.Forms.RadioButton radioActual;
        private System.Windows.Forms.RadioButton radioBudget;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.Label label3;

    }
}