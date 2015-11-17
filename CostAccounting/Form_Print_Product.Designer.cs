namespace CostAccounting
{
    partial class Form_Print_Product
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
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.category = new System.Windows.Forms.GroupBox();
            this.radioActual = new System.Windows.Forms.RadioButton();
            this.radioBudget = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioBlend = new System.Windows.Forms.RadioButton();
            this.radioProduct = new System.Windows.Forms.RadioButton();
            this.recordCnt = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOutput = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelStatus = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.category.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView.GridLines = true;
            this.listView.Location = new System.Drawing.Point(12, 120);
            this.listView.Name = "listView";
            this.listView.OwnerDraw = true;
            this.listView.Size = new System.Drawing.Size(690, 504);
            this.listView.TabIndex = 6;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView_DrawColumnHeader);
            this.listView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listView_DrawSubItem);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "取引先コード";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "請求先略称";
            this.columnHeader2.Width = 250;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "商品コード";
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "商品名";
            this.columnHeader4.Width = 250;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.category);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 55);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "出力条件";
            // 
            // category
            // 
            this.category.Controls.Add(this.radioActual);
            this.category.Controls.Add(this.radioBudget);
            this.category.Location = new System.Drawing.Point(151, 13);
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
            this.radioActual.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
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
            this.radioBudget.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioBlend);
            this.groupBox2.Controls.Add(this.radioProduct);
            this.groupBox2.Location = new System.Drawing.Point(6, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(139, 36);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // radioBlend
            // 
            this.radioBlend.AutoSize = true;
            this.radioBlend.Location = new System.Drawing.Point(61, 12);
            this.radioBlend.Name = "radioBlend";
            this.radioBlend.Size = new System.Drawing.Size(72, 19);
            this.radioBlend.TabIndex = 1;
            this.radioBlend.Text = "ブレンド品";
            this.radioBlend.UseVisualStyleBackColor = true;
            this.radioBlend.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // radioProduct
            // 
            this.radioProduct.AutoSize = true;
            this.radioProduct.Checked = true;
            this.radioProduct.Location = new System.Drawing.Point(6, 12);
            this.radioProduct.Name = "radioProduct";
            this.radioProduct.Size = new System.Drawing.Size(49, 19);
            this.radioProduct.TabIndex = 0;
            this.radioProduct.TabStop = true;
            this.radioProduct.Text = "商品";
            this.radioProduct.UseVisualStyleBackColor = true;
            this.radioProduct.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // recordCnt
            // 
            this.recordCnt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.recordCnt.Location = new System.Drawing.Point(297, 94);
            this.recordCnt.Name = "recordCnt";
            this.recordCnt.Size = new System.Drawing.Size(71, 18);
            this.recordCnt.TabIndex = 8;
            this.recordCnt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(373, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "件";
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(627, 91);
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
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 634);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(336, 10);
            this.progressBar.TabIndex = 12;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(351, 630);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(0, 15);
            this.labelStatus.TabIndex = 13;
            // 
            // Form_Print_Product
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 653);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.recordCnt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.outputDir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRefOutputDir);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Form_Print_Product";
            this.Text = "帳票出力";
            this.Load += new System.EventHandler(this.Form_Print_Product_Load);
            this.groupBox1.ResumeLayout(false);
            this.category.ResumeLayout(false);
            this.category.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btnRefOutputDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox outputDir;
        internal System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox category;
        private System.Windows.Forms.RadioButton radioActual;
        private System.Windows.Forms.RadioButton radioBudget;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioBlend;
        private System.Windows.Forms.RadioButton radioProduct;
        private System.Windows.Forms.Label recordCnt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelStatus;

    }
}