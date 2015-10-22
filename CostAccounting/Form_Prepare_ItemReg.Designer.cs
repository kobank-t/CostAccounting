namespace CostAccounting
{
    partial class Form_Prepare_ItemReg
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.btnAppend = new System.Windows.Forms.Button();
            this.textRegCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textRegName = new System.Windows.Forms.TextBox();
            this.textRegNote = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDefaultDataReg = new System.Windows.Forms.Button();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "CSVファイル|*.csv";
            this.openFileDialog.Title = "CSVファイルの選択";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.btnChange);
            this.groupBox2.Controls.Add(this.btnAppend);
            this.groupBox2.Controls.Add(this.textRegCode);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textRegName);
            this.groupBox2.Controls.Add(this.textRegNote);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.dataGridView);
            this.groupBox2.Location = new System.Drawing.Point(6, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(541, 269);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "個別メンテナンス";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(456, 236);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 29;
            this.btnDelete.Text = "削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(375, 236);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(75, 23);
            this.btnChange.TabIndex = 30;
            this.btnChange.Text = "修正";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // btnAppend
            // 
            this.btnAppend.Location = new System.Drawing.Point(294, 236);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(75, 23);
            this.btnAppend.TabIndex = 31;
            this.btnAppend.Text = "登録";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // textRegCode
            // 
            this.textRegCode.Location = new System.Drawing.Point(76, 207);
            this.textRegCode.Name = "textRegCode";
            this.textRegCode.Size = new System.Drawing.Size(55, 23);
            this.textRegCode.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 210);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 15);
            this.label2.TabIndex = 19;
            this.label2.Text = "品種コード";
            // 
            // textRegName
            // 
            this.textRegName.Location = new System.Drawing.Point(186, 207);
            this.textRegName.Name = "textRegName";
            this.textRegName.Size = new System.Drawing.Size(114, 23);
            this.textRegName.TabIndex = 25;
            // 
            // textRegNote
            // 
            this.textRegNote.Location = new System.Drawing.Point(343, 207);
            this.textRegNote.Name = "textRegNote";
            this.textRegNote.Size = new System.Drawing.Size(188, 23);
            this.textRegNote.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(137, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 15);
            this.label5.TabIndex = 20;
            this.label5.Text = "品種名";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(306, 210);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 15);
            this.label7.TabIndex = 22;
            this.label7.Text = "備考";
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
            this.note});
            this.dataGridView.Location = new System.Drawing.Point(9, 22);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 21;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(522, 178);
            this.dataGridView.TabIndex = 5;
            this.dataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_RowEnter);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnDefaultDataReg);
            this.groupBox1.Location = new System.Drawing.Point(6, 282);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(541, 57);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(124, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(407, 15);
            this.label4.TabIndex = 32;
            this.label4.Text = "※品種コード[001,002,003,004,005,006,099,800,999]のデータを登録します。";
            // 
            // btnDefaultDataReg
            // 
            this.btnDefaultDataReg.Location = new System.Drawing.Point(9, 22);
            this.btnDefaultDataReg.Name = "btnDefaultDataReg";
            this.btnDefaultDataReg.Size = new System.Drawing.Size(109, 23);
            this.btnDefaultDataReg.TabIndex = 21;
            this.btnDefaultDataReg.Text = "初期データ登録";
            this.btnDefaultDataReg.UseVisualStyleBackColor = true;
            this.btnDefaultDataReg.Click += new System.EventHandler(this.btnDefaultDataReg_Click);
            // 
            // code
            // 
            this.code.DataPropertyName = "code";
            this.code.HeaderText = "品種コード";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "品種名";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 200;
            // 
            // note
            // 
            this.note.DataPropertyName = "note";
            this.note.HeaderText = "備考";
            this.note.Name = "note";
            this.note.ReadOnly = true;
            this.note.Width = 200;
            // 
            // Form_Prepare_ItemReg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 347);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Form_Prepare_ItemReg";
            this.Text = "品種コード登録";
            this.Load += new System.EventHandler(this.Form_Prepare_ItemReg_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Button btnAppend;
        private System.Windows.Forms.TextBox textRegCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textRegName;
        private System.Windows.Forms.TextBox textRegNote;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDefaultDataReg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn note;
    }
}