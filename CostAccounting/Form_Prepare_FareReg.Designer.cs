namespace CostAccounting
{
    partial class Form_Prepare_FareReg
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
            this.textRegPriceActual = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textRegPriceBudget = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_budget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price_actual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDefaultDataReg = new System.Windows.Forms.Button();
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
            this.groupBox2.Controls.Add(this.textRegPriceActual);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.textRegPriceBudget);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.dataGridView);
            this.groupBox2.Location = new System.Drawing.Point(6, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(625, 298);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "個別メンテナンス";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(541, 265);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(460, 265);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(75, 23);
            this.btnChange.TabIndex = 12;
            this.btnChange.Text = "修正";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // btnAppend
            // 
            this.btnAppend.Location = new System.Drawing.Point(379, 265);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(75, 23);
            this.btnAppend.TabIndex = 11;
            this.btnAppend.Text = "登録";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.btnAppend_Click);
            // 
            // textRegCode
            // 
            this.textRegCode.Location = new System.Drawing.Point(83, 207);
            this.textRegCode.Name = "textRegCode";
            this.textRegCode.Size = new System.Drawing.Size(55, 23);
            this.textRegCode.TabIndex = 2;
            this.textRegCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress_numeric);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 210);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "運送便コード";
            // 
            // textRegName
            // 
            this.textRegName.Location = new System.Drawing.Point(249, 207);
            this.textRegName.Name = "textRegName";
            this.textRegName.Size = new System.Drawing.Size(366, 23);
            this.textRegName.TabIndex = 4;
            // 
            // textRegNote
            // 
            this.textRegNote.Location = new System.Drawing.Point(385, 236);
            this.textRegNote.Name = "textRegNote";
            this.textRegNote.Size = new System.Drawing.Size(230, 23);
            this.textRegNote.TabIndex = 10;
            // 
            // textRegPriceActual
            // 
            this.textRegPriceActual.Location = new System.Drawing.Point(249, 236);
            this.textRegPriceActual.Name = "textRegPriceActual";
            this.textRegPriceActual.Size = new System.Drawing.Size(90, 23);
            this.textRegPriceActual.TabIndex = 8;
            this.textRegPriceActual.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress_numeric);
            this.textRegPriceActual.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(180, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 15);
            this.label5.TabIndex = 3;
            this.label5.Text = "運送便名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(180, 239);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "実績単価";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(349, 239);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 15);
            this.label7.TabIndex = 9;
            this.label7.Text = "備考";
            // 
            // textRegPriceBudget
            // 
            this.textRegPriceBudget.Location = new System.Drawing.Point(83, 236);
            this.textRegPriceBudget.Name = "textRegPriceBudget";
            this.textRegPriceBudget.Size = new System.Drawing.Size(90, 23);
            this.textRegPriceBudget.TabIndex = 6;
            this.textRegPriceBudget.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress_numeric);
            this.textRegPriceBudget.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 239);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 15);
            this.label6.TabIndex = 5;
            this.label6.Text = "予定単価";
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
            this.dataGridView.Location = new System.Drawing.Point(9, 22);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 21;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(607, 178);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_RowEnter);
            // 
            // code
            // 
            this.code.DataPropertyName = "code";
            this.code.HeaderText = "運送便コード";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "運送便名";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 180;
            // 
            // price_budget
            // 
            this.price_budget.DataPropertyName = "price_budget";
            this.price_budget.HeaderText = "予定単価";
            this.price_budget.Name = "price_budget";
            this.price_budget.ReadOnly = true;
            this.price_budget.Width = 90;
            // 
            // price_actual
            // 
            this.price_actual.DataPropertyName = "price_actual";
            this.price_actual.HeaderText = "実績単価";
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnDefaultDataReg);
            this.groupBox1.Location = new System.Drawing.Point(6, 310);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(625, 57);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(124, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(270, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "※運送便コードF0001～F0026のデータを登録します。";
            // 
            // btnDefaultDataReg
            // 
            this.btnDefaultDataReg.Location = new System.Drawing.Point(9, 22);
            this.btnDefaultDataReg.Name = "btnDefaultDataReg";
            this.btnDefaultDataReg.Size = new System.Drawing.Size(109, 23);
            this.btnDefaultDataReg.TabIndex = 0;
            this.btnDefaultDataReg.Text = "初期データ登録";
            this.btnDefaultDataReg.UseVisualStyleBackColor = true;
            this.btnDefaultDataReg.Click += new System.EventHandler(this.btnDefaultDataReg_Click);
            // 
            // Form_Prepare_FareReg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 370);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Form_Prepare_FareReg";
            this.Text = "運賃登録";
            this.Load += new System.EventHandler(this.Form_Prepare_FareReg_Load);
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
        private System.Windows.Forms.TextBox textRegPriceActual;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textRegPriceBudget;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDefaultDataReg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn price_budget;
        private System.Windows.Forms.DataGridViewTextBoxColumn price_actual;
        private System.Windows.Forms.DataGridViewTextBoxColumn note;
    }
}