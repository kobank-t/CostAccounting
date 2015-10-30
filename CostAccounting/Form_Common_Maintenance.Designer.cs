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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnGetLogFile = new System.Windows.Forms.Button();
            this.saveFileDialogDB = new System.Windows.Forms.SaveFileDialog();
            this.btnGetDbFile = new System.Windows.Forms.Button();
            this.saveFileDialogLog = new System.Windows.Forms.SaveFileDialog();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetLogFile
            // 
            this.btnGetLogFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGetLogFile.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnGetLogFile.Location = new System.Drawing.Point(0, 0);
            this.btnGetLogFile.Margin = new System.Windows.Forms.Padding(0);
            this.btnGetLogFile.Name = "btnGetLogFile";
            this.btnGetLogFile.Size = new System.Drawing.Size(431, 74);
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
            this.btnGetDbFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGetDbFile.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnGetDbFile.Location = new System.Drawing.Point(431, 0);
            this.btnGetDbFile.Margin = new System.Windows.Forms.Padding(0);
            this.btnGetDbFile.Name = "btnGetDbFile";
            this.btnGetDbFile.Size = new System.Drawing.Size(431, 74);
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
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.tableLayoutPanel2.SetColumnSpan(this.chart1, 2);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(3, 101);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(856, 394);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "ta";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.chart1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnGetDbFile, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnGetLogFile, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(862, 498);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel2.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(0, 74);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(862, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "DBファイルサイズの推移";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form_Common_Maintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(862, 498);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Form_Common_Maintenance";
            this.Text = "メンテナンス";
            this.Load += new System.EventHandler(this.Form_Common_Maintenance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGetLogFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialogDB;
        private System.Windows.Forms.Button btnGetDbFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialogLog;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;


    }
}