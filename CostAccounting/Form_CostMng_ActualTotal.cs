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
            base.setCategory(Program.judgeCategory(radioBudget, radioActual));
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

            // 出力フォルダのデフォルトはアプリケーションの実行フォルダを指定
            outputDir.Text = Application.StartupPath;
            folderBrowserDialog.SelectedPath = Application.StartupPath;
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

            // 品種別または取り引き先別の集計データを設定する
            Const.CATEGORY_TYPE category = Program.judgeCategory(radioBudget, radioActual);

            // 固定費データを設定する
            setDataFixedCost();
            
            // 品種または取引先での集計データを設定する
            if (radioItem.Checked)
            {
                setData_Item();
                dataGridViewHeader.Columns[1].Visible = true;
                dataGridView.Columns[1].Visible = true;
                dataGridViewTotal.Columns[1].Visible = true;
                dataGridViewHeader.Columns[2].Visible = false;
                dataGridView.Columns[2].Visible = false;
                dataGridViewTotal.Columns[2].Visible = false;
            }
            else if (radioSuppliers.Checked)
            {
                setData_Supplier();
                dataGridViewHeader.Columns[1].Visible = false;
                dataGridView.Columns[1].Visible = false;
                dataGridViewTotal.Columns[1].Visible = false;
                dataGridViewHeader.Columns[2].Visible = true;
                dataGridView.Columns[2].Visible = true;
                dataGridViewTotal.Columns[2].Visible = true;
            }

            // 設定内容で計算する
            int[] columnIndex = { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 };

            foreach (int index in columnIndex)
                base.calcColumn(index);

            base.calcProceeds();
            base.calcOnlyTotal();
            base.calcManagementProfit();
            base.calcProfit();

            int[] columnIndexTotal = { 40, 41, 42, 43, 44, 45, 46, 47, 48, 49 };
            foreach (int index in columnIndexTotal)
            {
                base.calcTotal(index);
                base.calcManagementProfit();
                base.calcProfit();
            }

            //スクロールバーの状態をリセットする
            base.resetScrollBars();
        }

        /*************************************************************
         * データを画面に設定（品種でのグルーピング）
         *************************************************************/
        private void setData_Item()
        {
            Const.CATEGORY_TYPE category = Program.judgeCategory(radioBudget, radioActual);

            using (var context = new CostAccountingEntities())
            {
                var target = from t_supplier in context.ProductSupplier
                             join m_product in context.ProductCode
                                  on new { t_supplier.year, code = t_supplier.product_code } equals new { m_product.year, m_product.code }
                             join m_supplier in context.Supplier
                                  on new { t_supplier.year, code = t_supplier.supplier_code } equals new { m_supplier.year, m_supplier.code }
                             join t_product in context.Product
                                  on new { t_supplier.year, code = t_supplier.product_code, t_supplier.category, t_supplier.type }
                                       equals
                                     new { t_product.year, t_product.code, t_product.category, t_product.type }
                             join m_item in context.Item
                                  on new { t_product.year, code = t_product.item_code } equals new { m_item.year, m_item.code }

                             where t_supplier.year.Equals(Const.TARGET_YEAR)
                                && t_supplier.category.Equals((int)category)
                             group new { t_product, t_supplier } by m_item.name;

                var dataList = target.ToList();
                dataGridView.RowCount = dataList.Count;

                int[] saveColIndex = { 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38 };

                for (int i = 0; i < dataList.Count; i++)
                {
                    dataGridView.Rows[i].Cells[0].Value = (i + 1).ToString();
                    dataGridView.Rows[i].Cells[1].Value = dataList[i].Key;

                    decimal rateExpend = decimal.Divide(Parameters.getInstance(category).rateExpend, 100);
                    decimal[] saveVal = new decimal[saveColIndex.Length];

                    foreach (var data in dataList[i])
                    {
                        dataGridView.Rows[i].Cells[4].Value = data.t_supplier.unit_price.ToString("#,0");
                        dataGridView.Rows[i].Cells[5].Value = data.t_product.material_cost.ToString("#,0");
                        dataGridView.Rows[i].Cells[6].Value = data.t_product.labor_cost_direct.ToString("#,0");
                        dataGridView.Rows[i].Cells[7].Value = data.t_product.contractors_cost.ToString("#,0");
                        dataGridView.Rows[i].Cells[8].Value = data.t_product.materials_fare.ToString("#,0");
                        dataGridView.Rows[i].Cells[9].Value = data.t_product.packing_cost.ToString("#,0");
                        dataGridView.Rows[i].Cells[10].Value = data.t_product.utilities_cost.ToString("#,0");
                        dataGridView.Rows[i].Cells[11].Value = decimal.Multiply(data.t_product.other_cost, rateExpend).ToString("#,0");
                        dataGridView.Rows[i].Cells[12].Value = data.t_product.packing_fare.ToString("#,0");

                        dataGridView.Rows[i].Cells[13].Value = (data.t_product.material_cost
                                                                   + data.t_product.labor_cost_direct
                                                                   + data.t_product.contractors_cost
                                                                   + data.t_product.materials_fare
                                                                   + data.t_product.packing_cost
                                                                   + data.t_product.utilities_cost
                                                                   + decimal.Multiply(data.t_product.other_cost, rateExpend)
                                                                   + data.t_product.packing_fare).ToString("#,0");

                        dataGridView.Rows[i].Cells[14].Value = decimal.Subtract(Conversion.Parse((string)dataGridView.Rows[i].Cells[4].Value)
                                                                                , Conversion.Parse((string)dataGridView.Rows[i].Cells[13].Value)).ToString("#,0");

                        dataGridView.Rows[i].Cells[16].Value = checkBoxApr.Checked ? data.t_supplier.month_04.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[17].Value = checkBoxMay.Checked ? data.t_supplier.month_05.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[18].Value = checkBoxJun.Checked ? data.t_supplier.month_06.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[19].Value = checkBoxJul.Checked ? data.t_supplier.month_07.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[20].Value = checkBoxAug.Checked ? data.t_supplier.month_08.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[21].Value = checkBoxSep.Checked ? data.t_supplier.month_09.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[22].Value = checkBoxOct.Checked ? data.t_supplier.month_10.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[23].Value = checkBoxNov.Checked ? data.t_supplier.month_11.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[24].Value = checkBoxDec.Checked ? data.t_supplier.month_12.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[25].Value = checkBoxJan.Checked ? data.t_supplier.month_01.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[26].Value = checkBoxFeb.Checked ? data.t_supplier.month_02.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[27].Value = checkBoxMar.Checked ? data.t_supplier.month_03.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells["num04"].Value = data.t_supplier.num04.ToString("N");
                        dataGridView.Rows[i].Cells["num05"].Value = data.t_supplier.num05.ToString("N");
                        dataGridView.Rows[i].Cells["num06"].Value = data.t_supplier.num06.ToString("N");
                        dataGridView.Rows[i].Cells["num07"].Value = data.t_supplier.num07.ToString("N");
                        dataGridView.Rows[i].Cells["num08"].Value = data.t_supplier.num08.ToString("N");
                        dataGridView.Rows[i].Cells["num09"].Value = data.t_supplier.num09.ToString("N");
                        dataGridView.Rows[i].Cells["num10"].Value = data.t_supplier.num10.ToString("N");
                        dataGridView.Rows[i].Cells["num11"].Value = data.t_supplier.num11.ToString("N");
                        dataGridView.Rows[i].Cells["num12"].Value = data.t_supplier.num12.ToString("N");
                        dataGridView.Rows[i].Cells["num01"].Value = data.t_supplier.num01.ToString("N");
                        dataGridView.Rows[i].Cells["num02"].Value = data.t_supplier.num02.ToString("N");
                        dataGridView.Rows[i].Cells["num03"].Value = data.t_supplier.num03.ToString("N");
                        base.calcRow(i);

                        for (int j = 0; j < saveColIndex.Length; j++)
                        {
                            saveVal[j] += Conversion.Parse((string)dataGridView.Rows[i].Cells[saveColIndex[j]].Value);
                        }
                    }

                    for (int j = 0; j < saveColIndex.Length; j++)
                    {
                        dataGridView.Rows[i].Cells[saveColIndex[j]].Value = saveVal[j].ToString("#,0");
                    }
                }
            }
        }

        /*************************************************************
         * データを画面に設定（取引先でのグルーピング）
         *************************************************************/
        private void setData_Supplier()
        {
            Const.CATEGORY_TYPE category = Program.judgeCategory(radioBudget, radioActual);

            using (var context = new CostAccountingEntities())
            {
                var target = from t_supplier in context.ProductSupplier
                             join m_product in context.ProductCode
                                  on new { t_supplier.year, code = t_supplier.product_code } equals new { m_product.year, m_product.code }
                             join m_supplier in context.Supplier
                                  on new { t_supplier.year, code = t_supplier.supplier_code } equals new { m_supplier.year, m_supplier.code }
                             join t_product in context.Product
                                  on new { t_supplier.year, code = t_supplier.product_code, t_supplier.category, t_supplier.type }
                                       equals
                                     new { t_product.year, t_product.code, t_product.category, t_product.type }
                             join m_item in context.Item
                                  on new { t_product.year, code = t_product.item_code } equals new { m_item.year, m_item.code }

                             where t_supplier.year.Equals(Const.TARGET_YEAR)
                                && t_supplier.category.Equals((int)category)
                             orderby t_supplier.supplier_code, t_supplier.product_code
                             group new { t_product, t_supplier } by m_supplier.name;

                var dataList = target.ToList();
                dataGridView.RowCount = dataList.Count;

                int[] saveColIndex = { 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38 };

                for (int i = 0; i < dataList.Count; i++)
                {
                    dataGridView.Rows[i].Cells[0].Value = (i + 1).ToString();
                    dataGridView.Rows[i].Cells[2].Value = dataList[i].Key;

                    decimal rateExpend = decimal.Divide(Parameters.getInstance(category).rateExpend, 100);
                    decimal[] saveVal = new decimal[saveColIndex.Length];

                    foreach (var data in dataList[i])
                    {
                        dataGridView.Rows[i].Cells[4].Value = data.t_supplier.unit_price.ToString("#,0");
                        dataGridView.Rows[i].Cells[5].Value = data.t_product.material_cost.ToString("#,0");
                        dataGridView.Rows[i].Cells[6].Value = data.t_product.labor_cost_direct.ToString("#,0");
                        dataGridView.Rows[i].Cells[7].Value = data.t_product.contractors_cost.ToString("#,0");
                        dataGridView.Rows[i].Cells[8].Value = data.t_product.materials_fare.ToString("#,0");
                        dataGridView.Rows[i].Cells[9].Value = data.t_product.packing_cost.ToString("#,0");
                        dataGridView.Rows[i].Cells[10].Value = data.t_product.utilities_cost.ToString("#,0");
                        dataGridView.Rows[i].Cells[11].Value = decimal.Multiply(data.t_product.other_cost, rateExpend).ToString("#,0");
                        dataGridView.Rows[i].Cells[12].Value = data.t_product.packing_fare.ToString("#,0");

                        dataGridView.Rows[i].Cells[13].Value = (data.t_product.material_cost
                                                                   + data.t_product.labor_cost_direct
                                                                   + data.t_product.contractors_cost
                                                                   + data.t_product.materials_fare
                                                                   + data.t_product.packing_cost
                                                                   + data.t_product.utilities_cost
                                                                   + decimal.Multiply(data.t_product.other_cost, rateExpend)
                                                                   + data.t_product.packing_fare).ToString("#,0");

                        dataGridView.Rows[i].Cells[14].Value = decimal.Subtract(Conversion.Parse((string)dataGridView.Rows[i].Cells[4].Value)
                                                                                , Conversion.Parse((string)dataGridView.Rows[i].Cells[13].Value)).ToString("#,0");

                        dataGridView.Rows[i].Cells[16].Value = checkBoxApr.Checked ? data.t_supplier.month_04.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[17].Value = checkBoxMay.Checked ? data.t_supplier.month_05.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[18].Value = checkBoxJun.Checked ? data.t_supplier.month_06.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[19].Value = checkBoxJul.Checked ? data.t_supplier.month_07.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[20].Value = checkBoxAug.Checked ? data.t_supplier.month_08.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[21].Value = checkBoxSep.Checked ? data.t_supplier.month_09.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[22].Value = checkBoxOct.Checked ? data.t_supplier.month_10.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[23].Value = checkBoxNov.Checked ? data.t_supplier.month_11.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[24].Value = checkBoxDec.Checked ? data.t_supplier.month_12.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[25].Value = checkBoxJan.Checked ? data.t_supplier.month_01.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[26].Value = checkBoxFeb.Checked ? data.t_supplier.month_02.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells[27].Value = checkBoxMar.Checked ? data.t_supplier.month_03.ToString("#,0") : decimal.Zero.ToString();
                        dataGridView.Rows[i].Cells["num04"].Value = data.t_supplier.num04.ToString("N");
                        dataGridView.Rows[i].Cells["num05"].Value = data.t_supplier.num05.ToString("N");
                        dataGridView.Rows[i].Cells["num06"].Value = data.t_supplier.num06.ToString("N");
                        dataGridView.Rows[i].Cells["num07"].Value = data.t_supplier.num07.ToString("N");
                        dataGridView.Rows[i].Cells["num08"].Value = data.t_supplier.num08.ToString("N");
                        dataGridView.Rows[i].Cells["num09"].Value = data.t_supplier.num09.ToString("N");
                        dataGridView.Rows[i].Cells["num10"].Value = data.t_supplier.num10.ToString("N");
                        dataGridView.Rows[i].Cells["num11"].Value = data.t_supplier.num11.ToString("N");
                        dataGridView.Rows[i].Cells["num12"].Value = data.t_supplier.num12.ToString("N");
                        dataGridView.Rows[i].Cells["num01"].Value = data.t_supplier.num01.ToString("N");
                        dataGridView.Rows[i].Cells["num02"].Value = data.t_supplier.num02.ToString("N");
                        dataGridView.Rows[i].Cells["num03"].Value = data.t_supplier.num03.ToString("N");
                        base.calcRow(i);

                        for (int j = 0; j < saveColIndex.Length; j++)
                        {
                            saveVal[j] += Conversion.Parse((string)dataGridView.Rows[i].Cells[saveColIndex[j]].Value);
                        }
                    }

                    for (int j = 0; j < saveColIndex.Length; j++)
                    {
                        dataGridView.Rows[i].Cells[saveColIndex[j]].Value = saveVal[j].ToString("#,0");
                    }
                }
            }
        }

        /*************************************************************
         * 固定費データを画面に設定
         *************************************************************/
        private new void setDataFixedCost()
        {
            base.setCategory(Program.judgeCategory(radioBudget, radioActual));
            base.setDataFixedCost();
        }

        /*************************************************************
         * 集計対象月のチェックボックスを全てONにする
         *************************************************************/
        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            changeCheckBoxState(true);
            showTotalizationData();
        }

        /*************************************************************
         * 集計対象月のチェックボックスを全てOFFにする
         *************************************************************/
        private void btnAllClear_Click(object sender, EventArgs e)
        {
            changeCheckBoxState(false);
            showTotalizationData();
        }

        /*************************************************************
         * チェックボックスのON/OFFに従い、表示月の制御と計算を行う
         *************************************************************/
        private new void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox target = (CheckBox)sender;
            int index = monthDic.First(x => x.Value.Equals(target.Text)).Key;
            bool state = target.Checked;

            dataGridViewHeader.Columns[index].Visible = state;
            dataGridView.Columns[index].Visible = state;
            dataGridViewTotal.Columns[index].Visible = state;

            resetScrollBars();
            showTotalizationData();
        }

        /*************************************************************
         * チェックボックスの状態を変更する
         *************************************************************/
        private void changeCheckBoxState(bool state)
        {
            foreach (var control in groupMonth.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox target = (CheckBox)control;

                    target.CheckedChanged -= new EventHandler(checkBox_CheckedChanged);
                    target.Checked = state;
                    target.CheckedChanged += new EventHandler(checkBox_CheckedChanged);

                    int index = monthDic.First(x => x.Value.Equals(target.Text)).Key;
                    dataGridViewHeader.Columns[index].Visible = state;
                    dataGridView.Columns[index].Visible = state;
                    dataGridViewTotal.Columns[index].Visible = state;
                }
            }
            resetScrollBars();
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

        /*************************************************************
         * 出力フォルダの変更ボタン押下時の処理
         *************************************************************/
        private void btnRefOutputDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                outputDir.Text = folderBrowserDialog.SelectedPath;
            }
        }

        /*************************************************************
         * Excel出力ボタン押下時の処理
         *************************************************************/
        private void btnOutput_Click(object sender, EventArgs e)
        {
            string fileName = "集計表_";
            fileName += radioItem.Checked ? "品種別" : "取引先別";
            fileName += radioBudget.Checked ? "【予算】" : "【実績】";
            string template = radioItem.Checked ? Properties.Resources.template_total_item : Properties.Resources.template_total_supplier;
            base.btnOutput_Click(outputDir.Text, fileName, template);
        }
    }
}
