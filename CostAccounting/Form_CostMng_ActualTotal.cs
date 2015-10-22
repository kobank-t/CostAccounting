using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq.Expressions;

namespace CostAccounting
{
    public partial class Form_CostMng_ActualTotal : BaseCostMng
    {
        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_CostMng_ActualTotal()
        {
            InitializeComponent();
            base.ChecBoxControls = this.groupMonth.Controls;
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_CostMng_ActualTotal_Load(object sender, EventArgs e)
        {
            // ヘッダ行の初期化
            base.initDgvHeaderRow();

            // 合計行の初期化
            base.initDgvTotalRow();

            // 不要列を非表示
            int[] columunIndex = { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            foreach (int index in columunIndex)
            {
                dataGridViewHeader.Columns[index].Visible = false;
                dataGridView.Columns[index].Visible = false;
                dataGridViewTotal.Columns[index].Visible = false;
            }

            // 集計データを表示する
            showTotalizationData();
        }

        /*************************************************************
         * データグリッドビューのスクロール処理
         *************************************************************/
        private new void dataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            base.dataGridView_Scroll(sender, e);
        }

        /*************************************************************
         * データグリッドビュー（ヘッダー）のセル描画処理
         *************************************************************/
        private new void dataGridViewHeader_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            base.dataGridViewHeader_CellPainting(sender, e);
        }

        /*************************************************************
         * データグリッドビュー（合計行）のセル描画処理
         *************************************************************/
        private new void dataGridViewTotal_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            base.dataGridViewTotal_CellPainting(sender, e);
        }

        /*************************************************************
         * データグリッドビュー（ヘッダー）の列幅変更に伴う処理
         *************************************************************/
        private new void dataGridViewHeader_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            base.DataGridView = this.dataGridView;
            base.DataGridViewTotal = this.dataGridViewTotal;
            base.DataGridViewHeader = this.dataGridViewHeader;
            base.dataGridViewHeader_ColumnWidthChanged(sender, e);
        }

        /*************************************************************
         * 集計条件に応じて集計を行う
         *************************************************************/
        private void showTotalizationData()
        {
            // 前回集計時の表示内容をクリア
            dataGridView.Rows.Clear();

            // データの設定
            Const.CATEGORY_TYPE category = Program.judgeCategory(radioBudget, radioActual);
            if (radioItem.Checked)
            {
                setData_Item(category);
                dataGridViewHeader.Columns[1].Visible = true;
                dataGridView.Columns[1].Visible = true;
                dataGridViewTotal.Columns[1].Visible = true;
                dataGridViewHeader.Columns[2].Visible = false;
                dataGridView.Columns[2].Visible = false;
                dataGridViewTotal.Columns[2].Visible = false;
            }
            else if (radioSuppliers.Checked)
            {
                setData_Supplier(category);
                dataGridViewHeader.Columns[1].Visible = false;
                dataGridView.Columns[1].Visible = false;
                dataGridViewTotal.Columns[1].Visible = false;
                dataGridViewHeader.Columns[2].Visible = true;
                dataGridView.Columns[2].Visible = true;
                dataGridViewTotal.Columns[2].Visible = true;
            }
            setData_TotalRow(category);

            // 設定内容で計算する
            base.calcAll();

            //スクロールバーの状態をリセットする
            base.resetScrollBars();
        }

        /*************************************************************
         * データを画面に設定（品種でのグルーピング）
         *************************************************************/
        private void setData_Item(Const.CATEGORY_TYPE category)
        {
            using (var context = new CostAccountingEntities())
            {
                var target = from t_supplier in context.ProductSupplier
                             join m_product in context.ProductCode
                                  on new { t_supplier.year, code = t_supplier.product_code } equals new { m_product.year, m_product.code }
                             join m_supplier in context.Supplier
                                  on new { t_supplier.year, code = t_supplier.supplier_code } equals new { m_supplier.year, m_supplier.code }
                             join t_product in context.Product
                                  on new { t_supplier.year, code = t_supplier.product_code, t_supplier.category } equals new { t_product.year, t_product.code, t_product.category }
                             join m_item in context.Item
                                  on t_product.item_code equals m_item.code
                             where t_supplier.year.Equals(Const.TARGET_YEAR)
                                && t_supplier.category.Equals((int)category)
                             group new { t_product, t_supplier } by m_item.name;

                var dataList = target.ToList();
                dataGridView.RowCount = dataList.Count;

                for (int i = 0; i < dataList.Count; i++)
                {
                    dataGridView.Rows[i].Cells[0].Value = (i + 1).ToString();
                    dataGridView.Rows[i].Cells[1].Value = dataList[i].Key;

                    foreach (var data in dataList[i])
                    {
                        dataGridView.Rows[i].Cells[4].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[4].Value)
                                                               + data.t_supplier.unit_price).ToString("N");
                        dataGridView.Rows[i].Cells[5].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[5].Value)
                                                               + data.t_product.material_cost).ToString("N");
                        dataGridView.Rows[i].Cells[6].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[6].Value)
                                                               + data.t_product.labor_cost_direct).ToString("N");
                        dataGridView.Rows[i].Cells[7].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[7].Value)
                                                               + data.t_product.contractors_cost).ToString("N");
                        dataGridView.Rows[i].Cells[8].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[8].Value)
                                                               + data.t_product.materials_fare).ToString("N");
                        dataGridView.Rows[i].Cells[9].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[9].Value)
                                                               + data.t_product.packing_cost).ToString("N");
                        dataGridView.Rows[i].Cells[10].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[10].Value)
                                                                + data.t_product.utilities_cost).ToString("N");
                        dataGridView.Rows[i].Cells[11].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[11].Value)
                                                                + decimal.Multiply(data.t_product.other_cost, (decimal)0.15948)).ToString("N");
                        dataGridView.Rows[i].Cells[12].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[12].Value)
                                                                + data.t_product.packing_fare).ToString("N");

                        dataGridView.Rows[i].Cells[13].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[13].Value)
                                                                + (data.t_product.material_cost
                                                                   + data.t_product.labor_cost_direct
                                                                   + data.t_product.contractors_cost
                                                                   + data.t_product.materials_fare
                                                                   + data.t_product.packing_cost
                                                                   + data.t_product.utilities_cost
                                                                   + decimal.Multiply(data.t_product.other_cost, (decimal)0.15948)
                                                                   + data.t_product.packing_fare)
                                                                ).ToString("N");

                        dataGridView.Rows[i].Cells[14].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[14].Value)
                                                                + decimal.Subtract(Conversion.Parse((string)dataGridView.Rows[i].Cells[4].Value)
                                                                                 , Conversion.Parse((string)dataGridView.Rows[i].Cells[13].Value))
                                                                ).ToString("N");
                        dataGridView.Rows[i].Cells[16].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[16].Value)
                                                                + data.t_supplier.month_04).ToString("N");
                        dataGridView.Rows[i].Cells[17].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[17].Value)
                                                                + data.t_supplier.month_05).ToString("N");
                        dataGridView.Rows[i].Cells[18].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[18].Value)
                                                                + data.t_supplier.month_06).ToString("N");
                        dataGridView.Rows[i].Cells[19].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[19].Value)
                                                                + data.t_supplier.month_07).ToString("N");
                        dataGridView.Rows[i].Cells[20].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[20].Value)
                                                                + data.t_supplier.month_08).ToString("N");
                        dataGridView.Rows[i].Cells[21].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[21].Value)
                                                                + data.t_supplier.month_09).ToString("N");
                        dataGridView.Rows[i].Cells[22].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[22].Value)
                                                                + data.t_supplier.month_10).ToString("N");
                        dataGridView.Rows[i].Cells[23].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[23].Value)
                                                                + data.t_supplier.month_11).ToString("N");
                        dataGridView.Rows[i].Cells[24].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[24].Value)
                                                                + data.t_supplier.month_12).ToString("N");
                        dataGridView.Rows[i].Cells[25].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[25].Value)
                                                                + data.t_supplier.month_01).ToString("N");
                        dataGridView.Rows[i].Cells[26].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[26].Value)
                                                                + data.t_supplier.month_02).ToString("N");
                        dataGridView.Rows[i].Cells[27].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[27].Value)
                                                                + data.t_supplier.month_03).ToString("N");
                    }
                }
            }
        }

        /*************************************************************
         * データを画面に設定（取引先でのグルーピング）
         *************************************************************/
        private void setData_Supplier(Const.CATEGORY_TYPE category)
        {
            using (var context = new CostAccountingEntities())
            {
                var target = from t_supplier in context.ProductSupplier
                             join m_product in context.ProductCode
                                  on new { t_supplier.year, code = t_supplier.product_code } equals new { m_product.year, m_product.code }
                             join m_supplier in context.Supplier
                                  on new { t_supplier.year, code = t_supplier.supplier_code } equals new { m_supplier.year, m_supplier.code }
                             join t_product in context.Product
                                  on new { t_supplier.year, code = t_supplier.product_code, t_supplier.category } equals new { t_product.year, t_product.code, t_product.category }
                             join m_item in context.Item
                                  on t_product.item_code equals m_item.code
                             where t_supplier.year.Equals(Const.TARGET_YEAR)
                                && t_supplier.category.Equals((int)category)
                             orderby t_supplier.supplier_code, t_supplier.product_code
                             group new { t_product, t_supplier } by m_supplier.name;

                var dataList = target.ToList();
                dataGridView.RowCount = dataList.Count;

                for (int i = 0; i < dataList.Count; i++)
                {
                    dataGridView.Rows[i].Cells[0].Value = (i + 1).ToString();
                    dataGridView.Rows[i].Cells[2].Value = dataList[i].Key;

                    foreach (var data in dataList[i])
                    {
                        dataGridView.Rows[i].Cells[4].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[4].Value)
                                                               + data.t_supplier.unit_price).ToString("N");
                        dataGridView.Rows[i].Cells[5].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[5].Value)
                                                               + data.t_product.material_cost).ToString("N");
                        dataGridView.Rows[i].Cells[6].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[6].Value)
                                                               + data.t_product.labor_cost_direct).ToString("N");
                        dataGridView.Rows[i].Cells[7].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[7].Value)
                                                               + data.t_product.contractors_cost).ToString("N");
                        dataGridView.Rows[i].Cells[8].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[8].Value)
                                                               + data.t_product.materials_fare).ToString("N");
                        dataGridView.Rows[i].Cells[9].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[9].Value)
                                                               + data.t_product.packing_cost).ToString("N");
                        dataGridView.Rows[i].Cells[10].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[10].Value)
                                                                + data.t_product.utilities_cost).ToString("N");
                        dataGridView.Rows[i].Cells[11].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[11].Value)
                                                                + decimal.Multiply(data.t_product.other_cost, (decimal)0.15948)).ToString("N");
                        dataGridView.Rows[i].Cells[12].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[12].Value)
                                                                + data.t_product.packing_fare).ToString("N");

                        dataGridView.Rows[i].Cells[13].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[13].Value)
                                                                + (data.t_product.material_cost
                                                                   + data.t_product.labor_cost_direct
                                                                   + data.t_product.contractors_cost
                                                                   + data.t_product.materials_fare
                                                                   + data.t_product.packing_cost
                                                                   + data.t_product.utilities_cost
                                                                   + decimal.Multiply(data.t_product.other_cost, (decimal)0.15948)
                                                                   + data.t_product.packing_fare)
                                                                ).ToString("N");

                        dataGridView.Rows[i].Cells[14].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[14].Value)
                                                                + decimal.Subtract(Conversion.Parse((string)dataGridView.Rows[i].Cells[4].Value)
                                                                                 , Conversion.Parse((string)dataGridView.Rows[i].Cells[13].Value))
                                                                ).ToString("N");
                        dataGridView.Rows[i].Cells[16].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[16].Value)
                                                                + data.t_supplier.month_04).ToString("N");
                        dataGridView.Rows[i].Cells[17].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[17].Value)
                                                                + data.t_supplier.month_05).ToString("N");
                        dataGridView.Rows[i].Cells[18].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[18].Value)
                                                                + data.t_supplier.month_06).ToString("N");
                        dataGridView.Rows[i].Cells[19].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[19].Value)
                                                                + data.t_supplier.month_07).ToString("N");
                        dataGridView.Rows[i].Cells[20].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[20].Value)
                                                                + data.t_supplier.month_08).ToString("N");
                        dataGridView.Rows[i].Cells[21].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[21].Value)
                                                                + data.t_supplier.month_09).ToString("N");
                        dataGridView.Rows[i].Cells[22].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[22].Value)
                                                                + data.t_supplier.month_10).ToString("N");
                        dataGridView.Rows[i].Cells[23].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[23].Value)
                                                                + data.t_supplier.month_11).ToString("N");
                        dataGridView.Rows[i].Cells[24].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[24].Value)
                                                                + data.t_supplier.month_12).ToString("N");
                        dataGridView.Rows[i].Cells[25].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[25].Value)
                                                                + data.t_supplier.month_01).ToString("N");
                        dataGridView.Rows[i].Cells[26].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[26].Value)
                                                                + data.t_supplier.month_02).ToString("N");
                        dataGridView.Rows[i].Cells[27].Value = (Conversion.Parse((string)dataGridView.Rows[i].Cells[27].Value)
                                                                + data.t_supplier.month_03).ToString("N");
                    }
                }
            }
        }

        /*************************************************************
         * 合計行のデータ値を画面に設定
         *************************************************************/
        public void setData_TotalRow(Const.CATEGORY_TYPE category)
        {

            using (var context = new CostAccountingEntities())
            {

                var total = from t in context.CostMngTotal
                            where t.year.Equals(Const.TARGET_YEAR)
                               && t.category.Equals((int)category)
                            select t;
                if (total.Count() > decimal.Zero)
                {
                    dataGridViewTotal.Rows[0].Cells[40].Value = total.First().manufacturing_personnel.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[41].Value = total.First().manufacturing_depreciation.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[42].Value = total.First().manufacturing_rent.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[43].Value = total.First().manufacturing_repair.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[44].Value = total.First().manufacturing_stock.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[45].Value = total.First().manufacturing_other.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[46].Value = total.First().selling_personnel.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[47].Value = total.First().selling_depreciation.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[48].Value = total.First().selling_other.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[49].Value = total.First().operating_expenses.ToString("N");
                }
                else
                {
                    dataGridViewTotal.Rows[0].Cells[40].Value = decimal.Zero.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[41].Value = decimal.Zero.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[42].Value = decimal.Zero.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[43].Value = decimal.Zero.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[44].Value = decimal.Zero.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[45].Value = decimal.Zero.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[46].Value = decimal.Zero.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[47].Value = decimal.Zero.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[48].Value = decimal.Zero.ToString("N");
                    dataGridViewTotal.Rows[0].Cells[49].Value = decimal.Zero.ToString("N");
                }
            }
        }

        /*************************************************************
         * 集計対象月のチェックボックスを全てONにする
         *************************************************************/
        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            base.changeCheckBoxState(groupMonth.Controls, true);
        }

        /*************************************************************
         * 集計対象月のチェックボックスを全てOFFにする
         *************************************************************/
        private void btnAllClear_Click(object sender, EventArgs e)
        {
            base.changeCheckBoxState(groupMonth.Controls, false);
        }

        /*************************************************************
         * チェックボックスのON/OFFに従い、表示月の制御と計算を行う
         *************************************************************/
        private new void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            base.checkBox_CheckedChanged(sender, e);
        }

        /*************************************************************
         * ラジオボタンの状態に従い、再集計を行う。
         *************************************************************/
        private void radio_CheckedChanged(object sender, EventArgs e)
        {
            // チェックされた方のイベントのみ処理を行う。
            RadioButton radio = (RadioButton)sender;
            if (!radio.Checked)
                return;

            showTotalizationData();
        }
    }
}
