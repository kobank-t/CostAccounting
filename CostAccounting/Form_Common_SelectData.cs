using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace CostAccounting
{
    public partial class Form_Common_SelectData : Form
    {
        // 検索タイプ
        private Const.SEARCH_TYPE type;

        public Form_Common_SelectData(Const.SEARCH_TYPE type)
        {
            InitializeComponent();
            this.type = type;

            switch (type)
            {
                case Const.SEARCH_TYPE.Product:
                    this.Text = "商品検索";
                    break;
                case Const.SEARCH_TYPE.Supplier:
                    this.Text = "取引先検索";
                    break;
            }
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_Common_SelectData_Load(object sender, EventArgs e)
        {

        }

        /*************************************************************
         * 検索ボタン押下時の処理
         *************************************************************/
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string code = textSearchCode.Text;
            string name = textSearchName.Text;

            using (var context = new CostAccountingEntities())
            {
                switch (type)
                {
                    case Const.SEARCH_TYPE.Product:
                        var productList = from t in context.ProductCode
                                          where t.year.Equals(Const.TARGET_YEAR)
                                             && (string.IsNullOrEmpty(code) || t.code.StartsWith(code))
                                             && (string.IsNullOrEmpty(name) || t.name.Contains(name))
                                             && t.del_flg.Equals(Const.FLG_OFF)
                                          orderby t.code
                                          select new { t.code, t.name, t.note, t.unit };
                        dataGridView.DataSource = productList.ToList();
                        break;
                    case Const.SEARCH_TYPE.Supplier:
                        var supplierList = from t in context.Supplier
                                           where t.year.Equals(Const.TARGET_YEAR)
                                              && (string.IsNullOrEmpty(code) || t.code.StartsWith(code))
                                              && (string.IsNullOrEmpty(name) || t.name.Contains(name))
                                              && t.del_flg.Equals(Const.FLG_OFF)
                                           orderby t.code
                                           select new { t.code, t.name, t.note, t.unit };
                        dataGridView.DataSource = supplierList.ToList();
                        break;
                }
            }
        }

        /*************************************************************
         * 決定ボタン押下時の処理
         *************************************************************/
        private void btnDecide_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                Program.MessageBoxError("データが選択されていません。");
                DialogResult = DialogResult.None;
                return;
            }
            DialogResult = DialogResult.OK;
        }

        /*************************************************************
         * テキストボックスにて数値のみ入力可能にする
         *************************************************************/
        private void textBox_KeyPress_numeric(object sender, KeyPressEventArgs e)
        {
            Event.textBox_KeyPress_numeric(sender, e);
        }

    }
}
