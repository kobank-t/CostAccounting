namespace CostAccounting
{
    partial class Form_Common_SelectData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.checkBoxSearchFlg = new System.Windows.Forms.CheckBox();
            this.textSearchName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textSearchCode = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnDecide = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AccessibleRole = System.Windows.Forms.AccessibleRole.Animation;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.code,
            this.name,
            this.note,
            this.Column1});
            this.dataGridView.Location = new System.Drawing.Point(12, 63);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 21;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(607, 316);
            this.dataGridView.TabIndex = 8;
            // 
            // code
            // 
            this.code.DataPropertyName = "code";
            this.code.HeaderText = "コード";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "名称";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 360;
            // 
            // note
            // 
            this.note.DataPropertyName = "note";
            this.note.HeaderText = "備考";
            this.note.Name = "note";
            this.note.ReadOnly = true;
            this.note.Width = 125;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "unit";
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSearch);
            this.groupBox3.Controls.Add(this.checkBoxSearchFlg);
            this.groupBox3.Controls.Add(this.textSearchName);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.textSearchCode);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(607, 45);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "絞り込み";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(526, 16);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "検索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // checkBoxSearchFlg
            // 
            this.checkBoxSearchFlg.AutoSize = true;
            this.checkBoxSearchFlg.Location = new System.Drawing.Point(458, 19);
            this.checkBoxSearchFlg.Name = "checkBoxSearchFlg";
            this.checkBoxSearchFlg.Size = new System.Drawing.Size(62, 19);
            this.checkBoxSearchFlg.TabIndex = 10;
            this.checkBoxSearchFlg.Text = "未登録";
            this.checkBoxSearchFlg.UseVisualStyleBackColor = true;
            // 
            // textSearchName
            // 
            this.textSearchName.Location = new System.Drawing.Point(157, 17);
            this.textSearchName.Name = "textSearchName";
            this.textSearchName.Size = new System.Drawing.Size(288, 23);
            this.textSearchName.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 15);
            this.label8.TabIndex = 4;
            this.label8.Text = "コード";
            // 
            // textSearchCode
            // 
            this.textSearchCode.Location = new System.Drawing.Point(50, 17);
            this.textSearchCode.Name = "textSearchCode";
            this.textSearchCode.Size = new System.Drawing.Size(55, 23);
            this.textSearchCode.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(120, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 15);
            this.label9.TabIndex = 4;
            this.label9.Text = "名称";
            // 
            // btnDecide
            // 
            this.btnDecide.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDecide.Location = new System.Drawing.Point(544, 385);
            this.btnDecide.Name = "btnDecide";
            this.btnDecide.Size = new System.Drawing.Size(75, 23);
            this.btnDecide.TabIndex = 11;
            this.btnDecide.Text = "決定";
            this.btnDecide.UseVisualStyleBackColor = true;
            this.btnDecide.Click += new System.EventHandler(this.btnDecide_Click);
            // 
            // Form_Common_SelectData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 414);
            this.Controls.Add(this.btnDecide);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.dataGridView);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Form_Common_SelectData";
            this.Load += new System.EventHandler(this.Form_Common_SelectData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox textSearchName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textSearchCode;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBoxSearchFlg;
        private System.Windows.Forms.Button btnDecide;
        internal System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn note;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}