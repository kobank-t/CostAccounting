namespace CostAccounting
{
    partial class Form_Prepare_RawMaterialsReg
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
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnFileOpen = new System.Windows.Forms.Button();
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.recordCnt = new System.Windows.Forms.Label();
            this.labelFilePath = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFileReg = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_budget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_actual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.textRegCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textRegName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textRegPriceBudget = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textRegNote = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.btnAppend = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.textSearchName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textSearchCode = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textRegPriceActual = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSVファイル|*.csv";
            this.openFileDialog.Title = "CSVファイルの選択";
            // 
            // btnFileOpen
            // 
            this.btnFileOpen.Location = new System.Drawing.Point(9, 22);
            this.btnFileOpen.Name = "btnFileOpen";
            this.btnFileOpen.Size = new System.Drawing.Size(109, 23);
            this.btnFileOpen.TabIndex = 0;
            this.btnFileOpen.Text = "CSVファイル参照";
            this.btnFileOpen.UseVisualStyleBackColor = true;
            this.btnFileOpen.Click += new System.EventHandler(this.btnFileOpen_Click);
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView.GridLines = true;
            this.listView.Location = new System.Drawing.Point(9, 50);
            this.listView.Name = "listView";
            this.listView.OwnerDraw = true;
            this.listView.Size = new System.Drawing.Size(607, 139);
            this.listView.TabIndex = 1;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView_DrawColumnHeader);
            this.listView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listView_DrawSubItem);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "商品コード";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "商品名";
            this.columnHeader2.Width = 360;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "原価(税抜)";
            this.columnHeader3.Width = 100;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.recordCnt);
            this.groupBox1.Controls.Add(this.labelFilePath);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnFileReg);
            this.groupBox1.Controls.Add(this.btnFileOpen);
            this.groupBox1.Controls.Add(this.listView);
            this.groupBox1.Location = new System.Drawing.Point(6, 365);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(625, 225);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "一括登録";
            // 
            // recordCnt
            // 
            this.recordCnt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.recordCnt.Location = new System.Drawing.Point(9, 198);
            this.recordCnt.Name = "recordCnt";
            this.recordCnt.Size = new System.Drawing.Size(68, 18);
            this.recordCnt.TabIndex = 4;
            // 
            // labelFilePath
            // 
            this.labelFilePath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFilePath.Location = new System.Drawing.Point(215, 25);
            this.labelFilePath.Name = "labelFilePath";
            this.labelFilePath.Size = new System.Drawing.Size(401, 18);
            this.labelFilePath.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(82, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "件";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(124, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "選択したファイル：";
            // 
            // btnFileReg
            // 
            this.btnFileReg.Location = new System.Drawing.Point(541, 195);
            this.btnFileReg.Name = "btnFileReg";
            this.btnFileReg.Size = new System.Drawing.Size(75, 23);
            this.btnFileReg.TabIndex = 0;
            this.btnFileReg.Text = "登録";
            this.btnFileReg.UseVisualStyleBackColor = true;
            this.btnFileReg.Click += new System.EventHandler(this.btnFileReg_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AccessibleRole = System.Windows.Forms.AccessibleRole.Animation;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.code,
            this.name,
            this.price_budget,
            this.price_actual,
            this.note});
            this.dataGridView.Location = new System.Drawing.Point(15, 85);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 21;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(607, 178);
            this.dataGridView.TabIndex = 3;
            this.dataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_RowEnter);
            // 
            // code
            // 
            this.code.DataPropertyName = "code";
            this.code.HeaderText = "商品コード";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            this.code.Width = 80;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "商品名";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 200;
            // 
            // price_budget
            // 
            this.price_budget.DataPropertyName = "price_budget";
            this.price_budget.HeaderText = "予算(税抜)";
            this.price_budget.Name = "price_budget";
            this.price_budget.ReadOnly = true;
            this.price_budget.Width = 90;
            // 
            // price_actual
            // 
            this.price_actual.DataPropertyName = "price_actual";
            this.price_actual.HeaderText = "実績(税抜)";
            this.price_actual.Name = "price_actual";
            this.price_actual.ReadOnly = true;
            this.price_actual.Width = 90;
            // 
            // note
            // 
            this.note.DataPropertyName = "note";
            this.note.HeaderText = "備考";
            this.note.Name = "note";
            this.note.ReadOnly = true;
            this.note.Width = 125;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 272);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "商品コード";
            // 
            // textRegCode
            // 
            this.textRegCode.Location = new System.Drawing.Point(90, 269);
            this.textRegCode.Name = "textRegCode";
            this.textRegCode.Size = new System.Drawing.Size(55, 23);
            this.textRegCode.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(181, 260);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "商品名";
            // 
            // textRegName
            // 
            this.textRegName.Location = new System.Drawing.Point(250, 257);
            this.textRegName.Name = "textRegName";
            this.textRegName.Size = new System.Drawing.Size(366, 23);
            this.textRegName.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 301);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 15);
            this.label6.TabIndex = 4;
            this.label6.Text = "予算(税抜)";
            // 
            // textRegPriceBudget
            // 
            this.textRegPriceBudget.Location = new System.Drawing.Point(90, 298);
            this.textRegPriceBudget.Name = "textRegPriceBudget";
            this.textRegPriceBudget.Size = new System.Drawing.Size(90, 23);
            this.textRegPriceBudget.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(350, 289);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 15);
            this.label7.TabIndex = 4;
            this.label7.Text = "備考";
            // 
            // textRegNote
            // 
            this.textRegNote.Location = new System.Drawing.Point(386, 286);
            this.textRegNote.Name = "textRegNote";
            this.textRegNote.Size = new System.Drawing.Size(230, 23);
            this.textRegNote.TabIndex = 5;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(540, 315);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(459, 315);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(75, 23);
            this.btnChange.TabIndex = 0;
            this.btnChange.Text = "修正";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // btnAppend
            // 
            this.btnAppend.Location = new System.Drawing.Point(378, 315);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(75, 23);
            this.btnAppend.TabIndex = 0;
            this.btnAppend.Text = "登録";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.textRegName);
            this.groupBox2.Controls.Add(this.textRegNote);
            this.groupBox2.Controls.Add(this.textRegPriceActual);
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.btnChange);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnAppend);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(6, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(625, 347);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "個別メンテナンス";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSearch);
            this.groupBox3.Controls.Add(this.textSearchName);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.textSearchCode);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(9, 22);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(607, 45);
            this.groupBox3.TabIndex = 7;
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
            // textSearchName
            // 
            this.textSearchName.Location = new System.Drawing.Point(183, 17);
            this.textSearchName.Name = "textSearchName";
            this.textSearchName.Size = new System.Drawing.Size(337, 23);
            this.textSearchName.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 15);
            this.label8.TabIndex = 4;
            this.label8.Text = "商品コード";
            // 
            // textSearchCode
            // 
            this.textSearchCode.Location = new System.Drawing.Point(72, 17);
            this.textSearchCode.Name = "textSearchCode";
            this.textSearchCode.Size = new System.Drawing.Size(55, 23);
            this.textSearchCode.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(136, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 15);
            this.label9.TabIndex = 4;
            this.label9.Text = "商品名";
            // 
            // textRegPriceActual
            // 
            this.textRegPriceActual.Location = new System.Drawing.Point(250, 286);
            this.textRegPriceActual.Name = "textRegPriceActual";
            this.textRegPriceActual.Size = new System.Drawing.Size(90, 23);
            this.textRegPriceActual.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(181, 289);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "実績(税抜)";
            // 
            // Form_Prepare_RawMaterialsReg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 602);
            this.Controls.Add(this.textRegPriceBudget);
            this.Controls.Add(this.textRegCode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Form_Prepare_RawMaterialsReg";
            this.Text = "原材料登録";
            this.Load += new System.EventHandler(this.Form_Prepare_RawMaterialsReg_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnFileOpen;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFileReg;
        private System.Windows.Forms.Label recordCnt;
        private System.Windows.Forms.Label labelFilePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textRegCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textRegName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textRegPriceBudget;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textRegNote;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Button btnAppend;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn price_budget;
        private System.Windows.Forms.DataGridViewTextBoxColumn price_actual;
        private System.Windows.Forms.DataGridViewTextBoxColumn note;
        private System.Windows.Forms.TextBox textRegPriceActual;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox textSearchName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textSearchCode;
        private System.Windows.Forms.Label label9;
    }
}