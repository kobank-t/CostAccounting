namespace CostAccounting
{
    partial class Form_Common_Maintenance
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
            this.btnGetLogFile = new System.Windows.Forms.Button();
            this.saveFileDialogDB = new System.Windows.Forms.SaveFileDialog();
            this.btnGetDbFile = new System.Windows.Forms.Button();
            this.saveFileDialogLog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // btnGetLogFile
            // 
            this.btnGetLogFile.Location = new System.Drawing.Point(12, 12);
            this.btnGetLogFile.Name = "btnGetLogFile";
            this.btnGetLogFile.Size = new System.Drawing.Size(134, 23);
            this.btnGetLogFile.TabIndex = 0;
            this.btnGetLogFile.Text = "ログファイル取得";
            this.btnGetLogFile.UseVisualStyleBackColor = true;
            this.btnGetLogFile.Click += new System.EventHandler(this.btnGetLogFile_Click);
            // 
            // saveFileDialogDB
            // 
            this.saveFileDialogDB.Filter = "dbファイル(*.db)|*.db";
            this.saveFileDialogDB.Title = "DBファイルhの保存先を選択してください";
            // 
            // btnGetDbFile
            // 
            this.btnGetDbFile.Location = new System.Drawing.Point(12, 41);
            this.btnGetDbFile.Name = "btnGetDbFile";
            this.btnGetDbFile.Size = new System.Drawing.Size(134, 23);
            this.btnGetDbFile.TabIndex = 0;
            this.btnGetDbFile.Text = "DBファイル取得";
            this.btnGetDbFile.UseVisualStyleBackColor = true;
            this.btnGetDbFile.Click += new System.EventHandler(this.btnGetDbFile_Click);
            // 
            // saveFileDialogLog
            // 
            this.saveFileDialogLog.Filter = "ログファイル(*.zip)|*.zip";
            this.saveFileDialogLog.Title = "DBファイルhの保存先を選択してください";
            // 
            // Form_Common_Maintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(414, 138);
            this.Controls.Add(this.btnGetDbFile);
            this.Controls.Add(this.btnGetLogFile);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Form_Common_Maintenance";
            this.Text = "メンテナンス";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGetLogFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialogDB;
        private System.Windows.Forms.Button btnGetDbFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialogLog;


    }
}