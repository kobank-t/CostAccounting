using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CostAccounting
{
    public partial class Form_Common_SelectYear : Form
    {
        /*************************************************************
         * 
         *************************************************************/
        public Form_Common_SelectYear()
        {
            InitializeComponent();

        }

        /*************************************************************
         * 決定ボタン押下時の処理
         *************************************************************/
        private void btnEnter_Click(object sender, EventArgs e)
        {
            targetYear.BackColor = Color.White;

            if (String.IsNullOrEmpty(targetYear.Text))
            {
                targetYear.BackColor = Color.Red;
                Program.MessageBoxError("対象年度を入力してください。");
                return;
            }

            if (!(Validation.IsNumeric(targetYear.Text) && targetYear.Text.Length == 4))
            {
                targetYear.BackColor = Color.Red;
                Program.MessageBoxError("対象年度は数値4桁で入力してください。");
                return;
            }

            Const.TARGET_YEAR = int.Parse(targetYear.Text);
            Form_Common_Menu form = new Form_Common_Menu();
            form.ShowDialog();
            form.Dispose();
        }
    }
}
