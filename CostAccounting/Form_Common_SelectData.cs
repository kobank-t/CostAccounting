using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace CostAccounting
{
    public partial class Form_Common_SelectData : Form
    {
        // 検索タイプ
        private Const.SEARCH_TYPE type;

        /*************************************************************
         * コンストラクタ
         *************************************************************/
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
                case Const.SEARCH_TYPE.Material:
                    this.Text = "原材料検索";
                    break;
                case Const.SEARCH_TYPE.Packing:
                    this.Text = "資材検索";
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
            using (var context = new CostAccountingEntities())
            {
                switch (type)
                {
                    case Const.SEARCH_TYPE.Product:
                        searchProduct();
                        break;
                    case Const.SEARCH_TYPE.Supplier:
                        searchSupplier();
                        break;
                    case Const.SEARCH_TYPE.Material:
                        searchMaterial();
                        break;
                    case Const.SEARCH_TYPE.Packing:
                        searchPacking();
                        break;
                }
            }
        }

        /*************************************************************
         * 商品検索を行う
         *************************************************************/
        private void searchProduct()
        {
            string code = textSearchCode.Text;
            string name = textSearchName.Text;

            using (var context = new CostAccountingEntities())
            {
                var productList = from t in context.ProductCode
                                  where t.year.Equals(Const.TARGET_YEAR)
                                     && (string.IsNullOrEmpty(code) || t.code.StartsWith(code))
                                     && t.del_flg.Equals(Const.FLG_OFF)
                                  orderby t.code
                                  select new { t.code, t.name, t.note, t.unit };

                var ret = productList.ToList();
                if (!string.IsNullOrEmpty(name))
                {
                    foreach (var data in productList.ToList())
                        if (!data.name.Contains(name))
                            ret.Remove(data);
                }
                dataGridView.DataSource = ret;
            }
        }

        /*************************************************************
         * 取引先検索を行う
         *************************************************************/
        private void searchSupplier()
        {
            string code = textSearchCode.Text;
            string name = textSearchName.Text;

            using (var context = new CostAccountingEntities())
            {
                var supplierList = from t in context.Supplier
                                   where t.year.Equals(Const.TARGET_YEAR)
                                      && (string.IsNullOrEmpty(code) || t.code.StartsWith(code))
                                      && t.del_flg.Equals(Const.FLG_OFF)
                                   orderby t.code
                                   select new { t.code, t.name, t.note, unit = "" };

                var ret = supplierList.ToList();
                if (!string.IsNullOrEmpty(name))
                {
                    foreach (var data in supplierList.ToList())
                        if (!data.name.Contains(name))
                            ret.Remove(data);
                }
                dataGridView.DataSource = ret;
            }
        }

        /*************************************************************
         * 原材料検索を行う
         *************************************************************/
        private void searchMaterial()
        {
            string code = textSearchCode.Text;
            string name = textSearchName.Text;

            using (var context = new CostAccountingEntities())
            {
                var target = from t in context.RowMaterial
                             where t.year.Equals(Const.TARGET_YEAR)
                                && (string.IsNullOrEmpty(code) || t.code.StartsWith(code))
                                && t.del_flg.Equals(Const.FLG_OFF)
                             orderby t.code
                             select new { t.code, t.name, t.note, unit = "" };

                var ret = target.ToList();
                if (!string.IsNullOrEmpty(name))
                {
                    foreach (var data in target.ToList())
                        if (!data.name.Contains(name))
                            ret.Remove(data);
                }
                dataGridView.DataSource = ret;
            }
        }

        /*************************************************************
         * 資材検索を行う
         *************************************************************/
        private void searchPacking()
        {
            string code = textSearchCode.Text;
            string name = textSearchName.Text;

            using (var context = new CostAccountingEntities())
            {
                var target = from t in context.Material
                             where t.year.Equals(Const.TARGET_YEAR)
                                && (string.IsNullOrEmpty(code) || t.code.StartsWith(code))
                                && t.del_flg.Equals(Const.FLG_OFF)
                             orderby t.code
                             select new { t.code, t.name, t.note, unit = "" };

                var ret = target.ToList();
                if (!string.IsNullOrEmpty(name))
                {
                    foreach (var data in target.ToList())
                        if (!data.name.Contains(name))
                            ret.Remove(data);
                }
                dataGridView.DataSource = ret;
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

        /*************************************************************
         * データグリッドビューの行をダブルクリックで決定とする
         *************************************************************/
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnDecide.PerformClick();
        }
    }
}
