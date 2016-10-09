using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using OfficeOpenXml;

namespace CostAccounting
{
    public partial class Form_CostMng_Comparison : BaseCostMng
    {
        // チェックボックスのコントロール定義
        private Dictionary<string, CheckBox> checkBoxDic = new Dictionary<string, CheckBox>();
        private new Dictionary<int, string> monthDic = new Dictionary<int, string> {
                { 28, "4月" }, { 30, "5月" }, { 32, "6月" }, { 34, "7月" },
                { 36, "8月" }, { 38, "9月" }, { 40, "10月" }, { 42, "11月" },
                { 44, "12月" }, { 46, "1月" }, { 48, "2月" }, { 50, "3月" }
            };
        private Dictionary<CheckBox, int> checkBoxMonthDic = new Dictionary<CheckBox, int>();
        private Dictionary<CheckBox, string> checkBoxNumDic = new Dictionary<CheckBox, string>();

        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_CostMng_Comparison()
        {
            InitializeComponent();
            base.ChecBoxControls = this.groupMonth.Controls;

            foreach (var control in groupMonth.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox checkbox = (CheckBox)control;
                    checkBoxDic.Add(checkbox.Text, checkbox);
                    checkBoxMonthDic.Add(checkbox, int.Parse(checkbox.Text.Replace("月", "")));
                    checkBoxNumDic.Add(checkbox, "num" + String.Format("{0:00}", int.Parse(checkbox.Text.Replace("月", ""))));
                }
            }
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_CostMng_Comparison_Load(object sender, EventArgs e)
        {
            // ヘッダ行の初期化
            initDgvHeaderRow();

            // 合計行の初期化
            initDgvTotalRow();

            // データを設定する
            setData();
            setDataFixedCost();

            // 設定内容で計算する
            calcAll();

            // 表示列の初期設定
            checkedListBox.SetItemChecked(18, true);
            checkedListBox.SetItemChecked(19, true);
            checkedListBox.SetItemChecked(33, true);
            checkedListBox.SetItemChecked(34, true);
            checkedListBox_SelectedIndexChanged(sender, e);

            //スクロールバーの状態をリセットする
            resetScrollBars();

            // 出力フォルダのデフォルトはアプリケーションの実行フォルダを指定
            outputDir.Text = Application.StartupPath;
            folderBrowserDialog.SelectedPath = Application.StartupPath;
        }

        /*************************************************************
         * データグリッドビュー合計行の初期設定
         *************************************************************/
        private new void initDgvTotalRow()
        {
            dataGridViewTotal.RowCount = 2;
            dataGridViewTotal.Rows[0].Cells[0].Value = "総計";
            dataGridViewTotal.Rows[0].Frozen = true;
            dataGridViewTotal.Rows[1].ReadOnly = true;
            dataGridViewTotal.ColumnHeadersVisible = false;
        }

        /*************************************************************
         * datagridviewのヘッダをカスタマイズする
         *************************************************************/
        private new void initDgvHeaderRow()
        {
            dataGridViewHeader.RowCount = 3;

            for (int i = 4; i < dataGridViewHeader.ColumnCount; i += 2)
            {
                dataGridViewHeader.Rows[2].Cells[i].Value = "予定";
                dataGridViewHeader.Rows[2].Cells[i + 1].Value = "実績";
            }

            dataGridViewHeader.Rows[0].Cells[0].Value = "No";
            dataGridViewHeader.Rows[0].Cells[1].Value = "品種";
            dataGridViewHeader.Rows[0].Cells[2].Value = "取引先";
            dataGridViewHeader.Rows[0].Cells[3].Value = "製品名";
            dataGridViewHeader.Rows[0].Cells[4].Value = "標準売価";
            dataGridViewHeader.Rows[1].Cells[4].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[6].Value = "原材料費";
            dataGridViewHeader.Rows[1].Cells[6].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[8].Value = "製造人件費";
            dataGridViewHeader.Rows[1].Cells[8].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[10].Value = "外注費";
            dataGridViewHeader.Rows[1].Cells[10].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[12].Value = "原料運賃";
            dataGridViewHeader.Rows[1].Cells[12].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[14].Value = "包装資材費";
            dataGridViewHeader.Rows[1].Cells[14].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[16].Value = "光熱費";
            dataGridViewHeader.Rows[1].Cells[16].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[18].Value = "消耗品費";
            dataGridViewHeader.Rows[0].Cells[20].Value = "荷造運賃";
            dataGridViewHeader.Rows[1].Cells[20].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[22].Value = "変動費+直接人件費(A)計";
            dataGridViewHeader.Rows[1].Cells[22].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[24].Value = "利益(売上-A)";
            dataGridViewHeader.Rows[1].Cells[24].Value = "(円/kg)";
            dataGridViewHeader.Rows[1].Cells[26].Value = "売り上げ(円)";
            dataGridViewHeader.Rows[1].Cells[28].Value = "4月";
            dataGridViewHeader.Rows[1].Cells[30].Value = "5月";
            dataGridViewHeader.Rows[1].Cells[32].Value = "6月";
            dataGridViewHeader.Rows[1].Cells[34].Value = "7月";
            dataGridViewHeader.Rows[1].Cells[36].Value = "8月";
            dataGridViewHeader.Rows[1].Cells[38].Value = "9月";
            dataGridViewHeader.Rows[1].Cells[40].Value = "10月";
            dataGridViewHeader.Rows[1].Cells[42].Value = "11月";
            dataGridViewHeader.Rows[1].Cells[44].Value = "12月";
            dataGridViewHeader.Rows[1].Cells[46].Value = "1月";
            dataGridViewHeader.Rows[1].Cells[48].Value = "2月";
            dataGridViewHeader.Rows[1].Cells[50].Value = "3月";
            dataGridViewHeader.Rows[1].Cells[52].Value = "数量";
            dataGridViewHeader.Rows[1].Cells[54].Value = "原材料費(円)";
            dataGridViewHeader.Rows[1].Cells[56].Value = "製造人件費(円)";
            dataGridViewHeader.Rows[1].Cells[58].Value = "外注費(円)";
            dataGridViewHeader.Rows[0].Cells[58].Value = "製造原価";
            dataGridViewHeader.Rows[1].Cells[60].Value = "原料運賃(円)";
            dataGridViewHeader.Rows[1].Cells[62].Value = "包装資材費(円)";
            dataGridViewHeader.Rows[1].Cells[64].Value = "水道光熱費(円)";
            dataGridViewHeader.Rows[1].Cells[66].Value = "消耗品費(円)";
            dataGridViewHeader.Rows[0].Cells[68].Value = "販管費";
            dataGridViewHeader.Rows[1].Cells[68].Value = "荷造運賃(円)";
            dataGridViewHeader.Rows[0].Cells[70].Value = "変動費+直接人件費(A)計";
            dataGridViewHeader.Rows[1].Cells[70].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[72].Value = "利益(売上-A)";
            dataGridViewHeader.Rows[1].Cells[72].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[74].Value = "固定費配賦率";
            dataGridViewHeader.Rows[1].Cells[74].Value = "(転売品除く売上比率)";
            dataGridViewHeader.Rows[1].Cells[76].Value = "人件費";
            dataGridViewHeader.Rows[1].Cells[78].Value = "減価償却費";
            dataGridViewHeader.Rows[0].Cells[80].Value = "製造原価";
            dataGridViewHeader.Rows[1].Cells[80].Value = "賃借料";
            dataGridViewHeader.Rows[1].Cells[82].Value = "修繕費";
            dataGridViewHeader.Rows[1].Cells[84].Value = "在庫増減";
            dataGridViewHeader.Rows[1].Cells[86].Value = "その他経費";
            dataGridViewHeader.Rows[1].Cells[88].Value = "人件費";
            dataGridViewHeader.Rows[0].Cells[90].Value = "販売管理費";
            dataGridViewHeader.Rows[1].Cells[90].Value = "減価償却費";
            dataGridViewHeader.Rows[1].Cells[92].Value = "その他販管費";
            dataGridViewHeader.Rows[0].Cells[94].Value = "営業外費用/収益";
            dataGridViewHeader.Rows[0].Cells[96].Value = "固定費合計";
            dataGridViewHeader.Rows[1].Cells[96].Value = "(外含む)";
            dataGridViewHeader.Rows[0].Cells[98].Value = "経常利益(円)";
            dataGridViewHeader.Rows[0].Cells[100].Value = "製造原価";
            dataGridViewHeader.Rows[0].Cells[102].Value = "粗利益";
            dataGridViewHeader.Rows[0].Cells[104].Value = "粗利益率";
            dataGridViewHeader.Rows[0].Cells[106].Value = "原単価";
            dataGridViewHeader.Rows[0].Cells[108].Value = "仕掛品原価";

            for (int i = 0; i < 3; i++)
            {
                dataGridViewHeader.Rows[i].ReadOnly = true;
                dataGridViewHeader.Rows[i].DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.Control);
                dataGridViewHeader.Rows[i].DefaultCellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Control);
                dataGridViewHeader.Rows[i].DefaultCellStyle.SelectionForeColor = Color.Black;
            }

            // 年間予定列の背景色を変更
            for (int i = 0; i < 3; i++)
            {
                for (int j = 26; j <= 53; j++)
                {
                    dataGridViewHeader.Rows[i].Cells[j].Style.BackColor = Color.LightBlue;
                    dataGridViewHeader.Rows[i].Cells[j].Style.SelectionBackColor = Color.LightBlue;
                }
            }

            // 製造原価列と販管費列の背景色を変更
            for (int i = 0; i < 3; i++)
            {
                for (int j = 54; j <= 73; j++)
                {
                    dataGridViewHeader.Rows[i].Cells[j].Style.BackColor = Color.LightGreen;
                    dataGridViewHeader.Rows[i].Cells[j].Style.SelectionBackColor = Color.LightGreen;
                }
            }

            // 固定費配賦率の背景色を変更
            for (int i = 0; i < 3; i++)
            {
                for (int j = 74; j <= 75; j++)
                {
                    dataGridViewHeader.Rows[i].Cells[j].Style.BackColor = Color.LightBlue;
                    dataGridViewHeader.Rows[i].Cells[j].Style.SelectionBackColor = Color.LightBlue;
                }
            }

            // 製造原価列と販売管理費列の背景色を変更
            for (int i = 0; i < 3; i++)
            {
                for (int j = 76; j <= 99; j++)
                {
                    dataGridViewHeader.Rows[i].Cells[j].Style.BackColor = Color.Gold;
                    dataGridViewHeader.Rows[i].Cells[j].Style.SelectionBackColor = Color.Gold;
                }
            }

            // 不要な行が表示されないよう行を固定する
            dataGridViewHeader.Rows[1].Frozen = true;

            // 本来のヘッダを隠す
            dataGridViewHeader.ColumnHeadersVisible = false;

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
            //下の境界線を消す
            if ((e.RowIndex == 0 || e.RowIndex == 1) &
                    (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2 ||
                     e.ColumnIndex == 3
                    )
                )
            {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            }

            if (e.RowIndex == 0 &
                    (e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6 ||
                     e.ColumnIndex == 7 || e.ColumnIndex == 8 || e.ColumnIndex == 9 ||
                     e.ColumnIndex == 10 || e.ColumnIndex == 11 || e.ColumnIndex == 12 ||
                     e.ColumnIndex == 13 || e.ColumnIndex == 14 || e.ColumnIndex == 15 ||
                     e.ColumnIndex == 16 || e.ColumnIndex == 17 || e.ColumnIndex == 18 ||
                     e.ColumnIndex == 19 || e.ColumnIndex == 20 || e.ColumnIndex == 21 ||
                     e.ColumnIndex == 22 || e.ColumnIndex == 23 || e.ColumnIndex == 24 ||
                     e.ColumnIndex == 25 || e.ColumnIndex == 26 || e.ColumnIndex == 27 ||
                     e.ColumnIndex == 52 || e.ColumnIndex == 53 || e.ColumnIndex == 70 ||
                     e.ColumnIndex == 71 || e.ColumnIndex == 72 || e.ColumnIndex == 73 ||
                     e.ColumnIndex == 74 || e.ColumnIndex == 75 || e.ColumnIndex == 94 ||
                     e.ColumnIndex == 95 || e.ColumnIndex == 96 || e.ColumnIndex == 97 ||
                     e.ColumnIndex == 98 || e.ColumnIndex == 99 || e.ColumnIndex == 100 ||
                     e.ColumnIndex == 101 || e.ColumnIndex == 102 || e.ColumnIndex == 103 ||
                     e.ColumnIndex == 104 || e.ColumnIndex == 105 || e.ColumnIndex == 106 ||
                     e.ColumnIndex == 107 || e.ColumnIndex == 108 || e.ColumnIndex == 109
                    )
                )
            {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            }

            //右の境界線を消す
            if ((e.RowIndex == 0 || e.RowIndex == 1) &
                    (e.ColumnIndex == 4 || e.ColumnIndex == 6 || e.ColumnIndex == 8 ||
                     e.ColumnIndex == 10 || e.ColumnIndex == 12 || e.ColumnIndex == 14 ||
                     e.ColumnIndex == 16 || e.ColumnIndex == 18 || e.ColumnIndex == 20 ||
                     e.ColumnIndex == 22 || e.ColumnIndex == 24 || e.ColumnIndex == 26 ||
                     e.ColumnIndex == 28 || e.ColumnIndex == 30 || e.ColumnIndex == 32 ||
                     e.ColumnIndex == 34 || e.ColumnIndex == 36 || e.ColumnIndex == 38 ||
                     e.ColumnIndex == 40 || e.ColumnIndex == 42 || e.ColumnIndex == 44 ||
                     e.ColumnIndex == 46 || e.ColumnIndex == 48 || e.ColumnIndex == 50 ||
                     e.ColumnIndex == 52 || e.ColumnIndex == 54 || e.ColumnIndex == 56 ||
                     e.ColumnIndex == 58 || e.ColumnIndex == 60 || e.ColumnIndex == 62 ||
                     e.ColumnIndex == 64 || e.ColumnIndex == 66 || e.ColumnIndex == 68 ||
                     e.ColumnIndex == 70 || e.ColumnIndex == 72 || e.ColumnIndex == 74 ||
                     e.ColumnIndex == 76 || e.ColumnIndex == 78 || e.ColumnIndex == 80 ||
                     e.ColumnIndex == 82 || e.ColumnIndex == 84 || e.ColumnIndex == 86 ||
                     e.ColumnIndex == 88 || e.ColumnIndex == 90 || e.ColumnIndex == 92 ||
                     e.ColumnIndex == 94 || e.ColumnIndex == 96 || e.ColumnIndex == 98 ||
                     e.ColumnIndex == 100 || e.ColumnIndex == 102 || e.ColumnIndex == 104 ||
                     e.ColumnIndex == 106 || e.ColumnIndex == 108
                    )
                )
            {
                e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            }

            if (e.RowIndex == 0 &
                    (e.ColumnIndex == 27 || e.ColumnIndex == 29 || e.ColumnIndex == 31 ||
                     e.ColumnIndex == 33 || e.ColumnIndex == 35 || e.ColumnIndex == 37 ||
                     e.ColumnIndex == 39 || e.ColumnIndex == 41 || e.ColumnIndex == 43 ||
                     e.ColumnIndex == 45 || e.ColumnIndex == 47 || e.ColumnIndex == 49 ||
                     e.ColumnIndex == 55 || e.ColumnIndex == 57 || e.ColumnIndex == 59 ||
                     e.ColumnIndex == 61 || e.ColumnIndex == 63 || e.ColumnIndex == 65 ||
                     e.ColumnIndex == 77 || e.ColumnIndex == 79 || e.ColumnIndex == 81 ||
                     e.ColumnIndex == 83 || e.ColumnIndex == 85 || e.ColumnIndex == 89 ||
                     e.ColumnIndex == 91
                    )
                )
            {
                e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            }

            //セルの値表示を中央寄せにする
            e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        /*************************************************************
         * データグリッドビュー（合計行）のセル描画処理
         *************************************************************/
        private new void dataGridViewTotal_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //下の境界線を消す
            if (e.RowIndex == 0)
            {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            }

            //下の境界線を描画する
            if (e.RowIndex == 0 && e.ColumnIndex == 15)
            {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            }
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
         * 集計対象月のチェックボックスを全てONにする
         *************************************************************/
        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            changeCheckBoxState(groupMonth.Controls, true);
        }

        /*************************************************************
         * 集計対象月のチェックボックスを全てOFFにする
         *************************************************************/
        private void btnAllClear_Click(object sender, EventArgs e)
        {
            changeCheckBoxState(groupMonth.Controls, false);
        }

        /*************************************************************
         * チェックボックスの状態を変更する
         *************************************************************/
        private new void changeCheckBoxState(Control.ControlCollection controls, bool state)
        {
            foreach (var control in controls)
            {
                if (control is CheckBox)
                {
                    CheckBox target = (CheckBox)control;

                    target.CheckedChanged -= new EventHandler(checkBox_CheckedChanged);
                    target.Checked = state;
                    target.CheckedChanged += new EventHandler(checkBox_CheckedChanged);

                    int index = monthDic.First(x => x.Value.Equals(target.Text)).Key;
                    for (int offset = 0; offset < 2; offset++)
                    {
                        dataGridViewHeader.Columns[index + offset].Visible = state;
                        dataGridView.Columns[index + offset].Visible = state;
                        dataGridViewTotal.Columns[index + offset].Visible = state;
                    }
                }
            }

            setDataFixedCost();
            calcAll();
            resetScrollBars();
        }

        /*************************************************************
         * チェックボックスのON/OFFに従い、表示月の制御と計算を行う
         *************************************************************/
        private new void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox target = (CheckBox)sender;

            if (monthDic.ContainsValue(target.Text))
            {
                int index = monthDic.First(x => x.Value.Equals(target.Text)).Key;
                bool state = target.Checked;

                for (int offset = 0; offset < 2; offset++)
                {
                    dataGridViewHeader.Columns[index + offset].Visible = state;
                    dataGridView.Columns[index + offset].Visible = state;
                    dataGridViewTotal.Columns[index + offset].Visible = state;
                }

                setDataFixedCost();
                calcAll();
                resetScrollBars();
            }
        }

        /*************************************************************
         * データを画面に設定
         *************************************************************/
        private void setData()
        {
            using (var context = new CostAccountingEntities())
            {
                //------------------------------------------------------------------------------ 各商品の入力値を設定
                var target = from t_actual in
                                 (
                                   from t_product in context.Product
                                   join t_supplier in context.ProductSupplier
                                   on new { t_product.year, t_product.code, t_product.category, t_product.type }
                                        equals
                                      new { t_supplier.year, code = t_supplier.product_code, t_supplier.category, t_supplier.type }
                                   where t_product.year.Equals(Const.TARGET_YEAR)
                                      && t_product.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                                   select new { t_product, t_supplier }
                                 )
                             join t_budget_temp in
                                 (
                                   from t_product in context.Product
                                   join t_supplier in context.ProductSupplier
                                   on new { t_product.year, t_product.code, t_product.category, t_product.type }
                                        equals
                                      new { t_supplier.year, code = t_supplier.product_code, t_supplier.category, t_supplier.type }
                                   where t_product.year.Equals(Const.TARGET_YEAR)
                                      && t_product.category.Equals((int)Const.CATEGORY_TYPE.Budget)
                                   select new { t_product, t_supplier }
                                 )
                             on new { t_actual.t_product.year, t_actual.t_product.code, t_actual.t_product.type, t_actual.t_supplier.supplier_code }
                                  equals
                                new { t_budget_temp.t_product.year, t_budget_temp.t_product.code, t_budget_temp.t_product.type, t_budget_temp.t_supplier.supplier_code } into t_budget_sub
                             from t_budget in t_budget_sub.DefaultIfEmpty()

                             join m_product in context.ProductCode
                                  on new { t_actual.t_product.year, t_actual.t_product.code } equals new { m_product.year, m_product.code }
                             join m_supplier in context.Supplier
                                  on new { t_actual.t_supplier.year, code = t_actual.t_supplier.supplier_code } equals new { m_supplier.year, m_supplier.code }
                             join m_item in context.Item
                                  on new { t_actual.t_product.year, code = t_actual.t_product.item_code } equals new { m_item.year, m_item.code }

                             orderby t_actual.t_supplier.supplier_code, t_actual.t_supplier.product_code
                             select new { t_actual, t_budget, m_product, m_supplier, m_item };


                var dataList = target.ToList();
                dataGridView.RowCount = dataList.Count;
                decimal rateExpend_budget = decimal.Divide(Parameters.getInstance(Const.CATEGORY_TYPE.Budget).rateExpend, 100);
                decimal rateExpend_actual = decimal.Divide(Parameters.getInstance(Const.CATEGORY_TYPE.Actual).rateExpend, 100);
                for (int i = 0; i < dataList.Count; i++)
                {
                    dataGridView.Rows[i].Cells[0].Value = (i + 1).ToString();
                    dataGridView.Rows[i].Cells[1].Value = dataList[i].m_item.name;
                    dataGridView.Rows[i].Cells[2].Value = dataList[i].m_supplier.name;
                    dataGridView.Rows[i].Cells[3].Value = dataList[i].m_product.name;
                    dataGridView.Rows[i].Cells[5].Value = dataList[i].t_actual.t_supplier.unit_price.ToString("#,0");
                    dataGridView.Rows[i].Cells[7].Value = dataList[i].t_actual.t_product.material_cost.ToString("#,0");
                    dataGridView.Rows[i].Cells[9].Value = dataList[i].t_actual.t_product.labor_cost_direct.ToString("#,0");
                    dataGridView.Rows[i].Cells[11].Value = dataList[i].t_actual.t_product.contractors_cost.ToString("#,0");
                    dataGridView.Rows[i].Cells[13].Value = dataList[i].t_actual.t_product.materials_fare.ToString("#,0");
                    dataGridView.Rows[i].Cells[15].Value = dataList[i].t_actual.t_product.packing_cost.ToString("#,0");
                    dataGridView.Rows[i].Cells[17].Value = dataList[i].t_actual.t_product.utilities_cost.ToString("#,0");
                    dataGridView.Rows[i].Cells[19].Value = decimal.Multiply(dataList[i].t_actual.t_product.other_cost, rateExpend_actual).ToString("#,0");

                    //----------------------------------------------------------------------------------------------
                    // 荷造運賃は、取引先単位に計算した金額を設定するよう修正（2016/10/09）
                    // ↓ここから
                    //----------------------------------------------------------------------------------------------
                    // dataGridView.Rows[i].Cells[21].Value = dataList[i].t_actual.t_product.packing_fare.ToString("#,0");
                    dataGridView.Rows[i].Cells[21].Value = getPackingFare(context, Const.CATEGORY_TYPE.Actual, dataList[i].m_product.code, dataList[i].m_supplier.code);
                    //----------------------------------------------------------------------------------------------
                    // ↑ここまで
                    //----------------------------------------------------------------------------------------------

                    dataGridView.Rows[i].Cells[23].Value = (dataList[i].t_actual.t_product.material_cost
                                                            + dataList[i].t_actual.t_product.labor_cost_direct
                                                            + dataList[i].t_actual.t_product.contractors_cost
                                                            + dataList[i].t_actual.t_product.materials_fare
                                                            + dataList[i].t_actual.t_product.packing_cost
                                                            + dataList[i].t_actual.t_product.utilities_cost
                                                            + decimal.Multiply(dataList[i].t_actual.t_product.other_cost, rateExpend_actual)
                                                            + dataList[i].t_actual.t_product.packing_fare).ToString("#,0");

                    dataGridView.Rows[i].Cells[25].Value = decimal.Subtract(Conversion.Parse((string)dataGridView.Rows[i].Cells[5].Value)
                                                                            , Conversion.Parse((string)dataGridView.Rows[i].Cells[23].Value)).ToString("#,0");

                    dataGridView.Rows[i].Cells[29].Value = dataList[i].t_actual.t_supplier.month_04.ToString("#,0");
                    dataGridView.Rows[i].Cells[31].Value = dataList[i].t_actual.t_supplier.month_05.ToString("#,0");
                    dataGridView.Rows[i].Cells[33].Value = dataList[i].t_actual.t_supplier.month_06.ToString("#,0");
                    dataGridView.Rows[i].Cells[35].Value = dataList[i].t_actual.t_supplier.month_07.ToString("#,0");
                    dataGridView.Rows[i].Cells[37].Value = dataList[i].t_actual.t_supplier.month_08.ToString("#,0");
                    dataGridView.Rows[i].Cells[39].Value = dataList[i].t_actual.t_supplier.month_09.ToString("#,0");
                    dataGridView.Rows[i].Cells[41].Value = dataList[i].t_actual.t_supplier.month_10.ToString("#,0");
                    dataGridView.Rows[i].Cells[43].Value = dataList[i].t_actual.t_supplier.month_11.ToString("#,0");
                    dataGridView.Rows[i].Cells[45].Value = dataList[i].t_actual.t_supplier.month_12.ToString("#,0");
                    dataGridView.Rows[i].Cells[47].Value = dataList[i].t_actual.t_supplier.month_01.ToString("#,0");
                    dataGridView.Rows[i].Cells[49].Value = dataList[i].t_actual.t_supplier.month_02.ToString("#,0");
                    dataGridView.Rows[i].Cells[51].Value = dataList[i].t_actual.t_supplier.month_03.ToString("#,0");
                    dataGridView.Rows[i].Cells["product_code"].Value = dataList[i].t_actual.t_supplier.product_code;
                    dataGridView.Rows[i].Cells["supplier_code"].Value = dataList[i].t_actual.t_supplier.supplier_code;
                    dataGridView.Rows[i].Cells["num04"].Value = dataList[i].t_actual.t_supplier.num04.ToString("N");
                    dataGridView.Rows[i].Cells["num05"].Value = dataList[i].t_actual.t_supplier.num05.ToString("N");
                    dataGridView.Rows[i].Cells["num06"].Value = dataList[i].t_actual.t_supplier.num06.ToString("N");
                    dataGridView.Rows[i].Cells["num07"].Value = dataList[i].t_actual.t_supplier.num07.ToString("N");
                    dataGridView.Rows[i].Cells["num08"].Value = dataList[i].t_actual.t_supplier.num08.ToString("N");
                    dataGridView.Rows[i].Cells["num09"].Value = dataList[i].t_actual.t_supplier.num09.ToString("N");
                    dataGridView.Rows[i].Cells["num10"].Value = dataList[i].t_actual.t_supplier.num10.ToString("N");
                    dataGridView.Rows[i].Cells["num11"].Value = dataList[i].t_actual.t_supplier.num11.ToString("N");
                    dataGridView.Rows[i].Cells["num12"].Value = dataList[i].t_actual.t_supplier.num12.ToString("N");
                    dataGridView.Rows[i].Cells["num01"].Value = dataList[i].t_actual.t_supplier.num01.ToString("N");
                    dataGridView.Rows[i].Cells["num02"].Value = dataList[i].t_actual.t_supplier.num02.ToString("N");
                    dataGridView.Rows[i].Cells["num03"].Value = dataList[i].t_actual.t_supplier.num03.ToString("N");

                    if (dataList[i].t_budget != null)
                    {
                        dataGridView.Rows[i].Cells[4].Value = dataList[i].t_budget.t_supplier.unit_price.ToString("#,0");
                        dataGridView.Rows[i].Cells[6].Value = dataList[i].t_budget.t_product.material_cost.ToString("#,0");
                        dataGridView.Rows[i].Cells[8].Value = dataList[i].t_budget.t_product.labor_cost_direct.ToString("#,0");
                        dataGridView.Rows[i].Cells[10].Value = dataList[i].t_budget.t_product.contractors_cost.ToString("#,0");
                        dataGridView.Rows[i].Cells[12].Value = dataList[i].t_budget.t_product.materials_fare.ToString("#,0");
                        dataGridView.Rows[i].Cells[14].Value = dataList[i].t_budget.t_product.packing_cost.ToString("#,0");
                        dataGridView.Rows[i].Cells[16].Value = dataList[i].t_budget.t_product.utilities_cost.ToString("#,0");
                        dataGridView.Rows[i].Cells[18].Value = decimal.Multiply(dataList[i].t_budget.t_product.other_cost, rateExpend_budget).ToString("#,0");

                        //----------------------------------------------------------------------------------------------
                        // 荷造運賃は、取引先単位に計算した金額を設定するよう修正（2016/10/09）
                        // ↓ここから
                        //----------------------------------------------------------------------------------------------
                        // dataGridView.Rows[i].Cells[20].Value = dataList[i].t_budget.t_product.packing_fare.ToString("#,0");
                        dataGridView.Rows[i].Cells[20].Value = getPackingFare(context, Const.CATEGORY_TYPE.Budget, dataList[i].m_product.code, dataList[i].m_supplier.code);
                        //----------------------------------------------------------------------------------------------
                        // ↑ここまで
                        //----------------------------------------------------------------------------------------------

                        dataGridView.Rows[i].Cells[22].Value = (dataList[i].t_budget.t_product.material_cost
                                                                + dataList[i].t_budget.t_product.labor_cost_direct
                                                                + dataList[i].t_budget.t_product.contractors_cost
                                                                + dataList[i].t_budget.t_product.materials_fare
                                                                + dataList[i].t_budget.t_product.packing_cost
                                                                + dataList[i].t_budget.t_product.utilities_cost
                                                                + decimal.Multiply(dataList[i].t_budget.t_product.other_cost, rateExpend_budget)
                                                                + dataList[i].t_budget.t_product.packing_fare).ToString("#,0");

                        dataGridView.Rows[i].Cells[24].Value = decimal.Subtract(Conversion.Parse((string)dataGridView.Rows[i].Cells[4].Value)
                                                                                , Conversion.Parse((string)dataGridView.Rows[i].Cells[22].Value)).ToString("#,0");

                        dataGridView.Rows[i].Cells[28].Value = dataList[i].t_budget.t_supplier.month_04.ToString("#,0");
                        dataGridView.Rows[i].Cells[30].Value = dataList[i].t_budget.t_supplier.month_05.ToString("#,0");
                        dataGridView.Rows[i].Cells[32].Value = dataList[i].t_budget.t_supplier.month_06.ToString("#,0");
                        dataGridView.Rows[i].Cells[34].Value = dataList[i].t_budget.t_supplier.month_07.ToString("#,0");
                        dataGridView.Rows[i].Cells[36].Value = dataList[i].t_budget.t_supplier.month_08.ToString("#,0");
                        dataGridView.Rows[i].Cells[38].Value = dataList[i].t_budget.t_supplier.month_09.ToString("#,0");
                        dataGridView.Rows[i].Cells[40].Value = dataList[i].t_budget.t_supplier.month_10.ToString("#,0");
                        dataGridView.Rows[i].Cells[42].Value = dataList[i].t_budget.t_supplier.month_11.ToString("#,0");
                        dataGridView.Rows[i].Cells[44].Value = dataList[i].t_budget.t_supplier.month_12.ToString("#,0");
                        dataGridView.Rows[i].Cells[46].Value = dataList[i].t_budget.t_supplier.month_01.ToString("#,0");
                        dataGridView.Rows[i].Cells[48].Value = dataList[i].t_budget.t_supplier.month_02.ToString("#,0");
                        dataGridView.Rows[i].Cells[50].Value = dataList[i].t_budget.t_supplier.month_03.ToString("#,0");
                    }
                    else
                    {
                        int[] index = { 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50 };
                        foreach (int j in index)
                            dataGridView.Rows[i].Cells[j].Value = "-";
                    }
                }
            }
        }

        /*************************************************************
         * 指定した取引先の荷造運賃を返却する
         *************************************************************/
        private string getPackingFare(CostAccountingEntities context, Const.CATEGORY_TYPE category, string productCode, string supplierCode)
        {
            string product_code = productCode;
            string supplier_code = supplierCode;
            var packingFare = from t in context.ProductPackingFare
                              where t.year.Equals(Const.TARGET_YEAR)
                                 && t.product_code.Equals(product_code)
                                 && t.supplier_code.Equals(supplier_code)
                                 && t.category.Equals((int)category)
                              select t;

            decimal sumAmount = 0;
            DataTable fare = DataTableSupport.getInstance(category).fare;
            foreach (var data in packingFare)
            {
                decimal amount = decimal.Multiply(data.quantity, DataTableSupport.getPrice(fare, data.code));
                sumAmount += amount;
            }
            return sumAmount.ToString("#,0");
        }

        /*************************************************************
         * 固定費データを画面に設定
         *************************************************************/
        private new void setDataFixedCost()
        {
            for (int columnIdx = 76; columnIdx <= 95; columnIdx++)
                dataGridViewTotal.Rows[0].Cells[columnIdx].Value = decimal.Zero.ToString("#,0");

            using (var context = new CostAccountingEntities())
            {
                string inStr = string.Empty;
                foreach (CheckBox target in checkBoxMonthDic.Keys)
                {
                    if (target.Checked)
                        inStr += string.Concat(checkBoxMonthDic[target], ",");
                }

                inStr = inStr.TrimEnd(',');

                var targetData = from t in context.CostMngTotal
                                 where t.year.Equals(Const.TARGET_YEAR)
                                    && inStr.Contains(t.month.ToString())
                                    && t.del_flg.Equals(Const.FLG_OFF)
                                 select t;

                foreach (var data in targetData.ToList())
                {
                    if (data.category.Equals((int)Const.CATEGORY_TYPE.Budget))
                    {
                        dataGridViewTotal.Rows[0].Cells[76].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[76].Value), data.manufacturing_personnel).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[78].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[78].Value), data.manufacturing_depreciation).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[80].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[80].Value), data.manufacturing_rent).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[82].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[82].Value), data.manufacturing_repair).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[84].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[84].Value), data.manufacturing_stock).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[86].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[86].Value), data.manufacturing_other).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[88].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[88].Value), data.selling_personnel).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[90].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[90].Value), data.selling_depreciation).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[92].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[92].Value), data.selling_other).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[94].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[94].Value), data.operating_expenses).ToString("#,0");
                    }
                    else if (data.category.Equals((int)Const.CATEGORY_TYPE.Actual))
                    {
                        dataGridViewTotal.Rows[0].Cells[77].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[77].Value), data.manufacturing_personnel).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[79].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[79].Value), data.manufacturing_depreciation).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[81].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[81].Value), data.manufacturing_rent).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[83].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[83].Value), data.manufacturing_repair).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[85].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[85].Value), data.manufacturing_stock).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[87].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[87].Value), data.manufacturing_other).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[89].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[89].Value), data.selling_personnel).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[91].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[91].Value), data.selling_depreciation).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[93].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[93].Value), data.selling_other).ToString("#,0");
                        dataGridViewTotal.Rows[0].Cells[95].Value = decimal.Add(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[95].Value), data.operating_expenses).ToString("#,0");
                    }
                    else
                    {
                        MessageBox.Show("予算でも実績でもないデータが存在しています。category：" + data.category);
                    }
                }
            }
        }

        /*************************************************************
         * 本画面の入力値に対して全て計算する
         *************************************************************/
        private new void calcAll()
        {
            int[] columnIndex = { 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50 };

            foreach (DataGridViewRow row in dataGridView.Rows)
                calcRow(row.Index);

            for (int offset = 0; offset < 2; offset++)
                foreach (int index in columnIndex)
                    calcColumn(index + offset);

            calcProceeds();
            calcOnlyTotal();

            int[] columnIndexTotal = { 76, 78, 80, 82, 84, 86, 88, 90, 92, 94 };

            for (int offset = 0; offset < 2; offset++)
                foreach (int index in columnIndexTotal)
                    calcTotal(index, offset);

            calcManagementProfit();
            calcProfit();
        }

        /*************************************************************
         * 計算対象の月に含まれるか判断する
         *************************************************************/
        private new decimal containCalc(string value, int index)
        {
            decimal ret;
            if (checkBoxDic.ContainsKey(monthDic[index]) && checkBoxDic[monthDic[index]].Checked)
                ret = Conversion.Parse(value);
            else
                ret = decimal.Zero;

            return ret;
        }

        /*************************************************************
         * 指定行の計算を行う
         *************************************************************/
        private new void calcRow(int rowIndex)
        {
            for (int offset = 0; offset < 2; offset++)
            {
                dataGridView.Rows[rowIndex].Cells[26 + offset].Value =
                    (containCalc((string)dataGridView.Rows[rowIndex].Cells[28 + offset].Value, 28)
                     + containCalc((string)dataGridView.Rows[rowIndex].Cells[30 + offset].Value, 30)
                     + containCalc((string)dataGridView.Rows[rowIndex].Cells[32 + offset].Value, 32)
                     + containCalc((string)dataGridView.Rows[rowIndex].Cells[34 + offset].Value, 34)
                     + containCalc((string)dataGridView.Rows[rowIndex].Cells[36 + offset].Value, 36)
                     + containCalc((string)dataGridView.Rows[rowIndex].Cells[38 + offset].Value, 38)
                     + containCalc((string)dataGridView.Rows[rowIndex].Cells[40 + offset].Value, 40)
                     + containCalc((string)dataGridView.Rows[rowIndex].Cells[42 + offset].Value, 42)
                     + containCalc((string)dataGridView.Rows[rowIndex].Cells[44 + offset].Value, 44)
                     + containCalc((string)dataGridView.Rows[rowIndex].Cells[46 + offset].Value, 46)
                     + containCalc((string)dataGridView.Rows[rowIndex].Cells[48 + offset].Value, 48)
                     + containCalc((string)dataGridView.Rows[rowIndex].Cells[50 + offset].Value, 50)).ToString("#,0");


                // 予算の場合の数量は、売り上げから割返し
                if (offset == 0) {
                    dataGridView.Rows[rowIndex].Cells[52].Value =
                            Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[4 + offset].Value) == decimal.Zero ?
                            decimal.Zero.ToString("N") :
                            decimal.Divide(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[26 + offset].Value)
                                           , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[4 + offset].Value)).ToString("N");
                }
                // 実績の場合の数量は、各月の数量を集計
                else
                {
                    decimal numTotal = decimal.Zero;
                    foreach (CheckBox checkbox in checkBoxNumDic.Keys)
                    {
                        if (checkbox.Checked)
                        {
                            numTotal += Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[checkBoxNumDic[checkbox]].Value);
                        }
                    }
                    dataGridView.Rows[rowIndex].Cells[53].Value = numTotal.ToString("N");
                }

                dataGridView.Rows[rowIndex].Cells[54 + offset].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[52 + offset].Value)
                                                                               , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[6 + offset].Value)).ToString("#,0");
                dataGridView.Rows[rowIndex].Cells[56 + offset].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[52 + offset].Value)
                                                                               , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[8 + offset].Value)).ToString("#,0");
                dataGridView.Rows[rowIndex].Cells[58 + offset].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[52 + offset].Value)
                                                                               , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[10 + offset].Value)).ToString("#,0");
                dataGridView.Rows[rowIndex].Cells[60 + offset].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[52 + offset].Value)
                                                                               , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[12 + offset].Value)).ToString("#,0");
                dataGridView.Rows[rowIndex].Cells[62 + offset].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[52 + offset].Value)
                                                                               , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[14 + offset].Value)).ToString("#,0");
                dataGridView.Rows[rowIndex].Cells[64 + offset].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[52 + offset].Value)
                                                                               , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[16 + offset].Value)).ToString("#,0");
                dataGridView.Rows[rowIndex].Cells[66 + offset].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[52 + offset].Value)
                                                                               , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[18 + offset].Value)).ToString("#,0");
                dataGridView.Rows[rowIndex].Cells[68 + offset].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[52 + offset].Value)
                                                                               , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[20 + offset].Value)).ToString("#,0");
                dataGridView.Rows[rowIndex].Cells[70 + offset].Value = (Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[54 + offset].Value)
                                                               + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[56 + offset].Value)
                                                               + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[58 + offset].Value)
                                                               + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[60 + offset].Value)
                                                               + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[62 + offset].Value)
                                                               + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[64 + offset].Value)
                                                               + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[66 + offset].Value)
                                                               + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[68 + offset].Value)).ToString("#,0");
                dataGridView.Rows[rowIndex].Cells[72 + offset].Value = decimal.Subtract(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[26 + offset].Value)
                                                                               , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[70 + offset].Value)).ToString("#,0");

            }
        }

        /*************************************************************
         * 売り上げ列を計算する
         *************************************************************/
        private new void calcProceeds()
        {
            for (int offset = 0; offset < 2; offset++)
            {
                // 売り上げの計算
                decimal total = decimal.Zero;
                decimal excludeResaleTotal = decimal.Zero;

                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!"転売品".Equals(row.Cells[1].Value))
                        excludeResaleTotal += Conversion.Parse((string)row.Cells[26 + offset].Value);
                    total += Conversion.Parse((string)row.Cells[26 + offset].Value);
                }
                dataGridViewTotal.Rows[0].Cells[26 + offset].Value = total.ToString("#,0");
                dataGridViewTotal.Rows[1].Cells[26 + offset].Value = excludeResaleTotal.ToString("#,0");

                // 固定費配賦率の計算
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if ("転売品".Equals(row.Cells[1].Value))
                        row.Cells[74 + offset].Value = decimal.Zero.ToString("P6");
                    else
                        row.Cells[74 + offset].Value = excludeResaleTotal == decimal.Zero ?
                            decimal.Zero.ToString("P6") :
                            decimal.Divide(Conversion.Parse((string)row.Cells[26 + offset].Value), excludeResaleTotal).ToString("P6");
                }
            }
        }

        /*************************************************************
         * 合計値の算出のみ必要な列の計算を行う
         *************************************************************/
        private new void calcOnlyTotal()
        {
            int[] columnIndex = { 52, 54, 56, 58, 60, 62, 64, 66, 68, 70, 72,
                                  53, 55, 57, 59, 61, 63, 65, 67, 69, 71, 73 };
            decimal[] total = new decimal[columnIndex.Count()];

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                for (int i = 0; i < columnIndex.Count(); i++)
                    total[i] += Conversion.Parse((string)row.Cells[columnIndex[i]].Value);
            }

            for (int i = 0; i < columnIndex.Count(); i++)
                dataGridViewTotal.Rows[0].Cells[columnIndex[i]].Value = total[i].ToString("#,0");
        }

        /*************************************************************
         * 合計行の計算を行う
         *************************************************************/
        private void calcTotal(int columnIndex, int offset)
        {
            decimal total = decimal.Zero;
            decimal targetCost = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[columnIndex + offset].Value);
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                string rateStr = (string)row.Cells[74 + offset].Value;
                if (!string.IsNullOrEmpty(rateStr))
                    rateStr = rateStr.Replace("%", "");
                decimal rate = decimal.Divide(Conversion.Parse(rateStr), 100);
                row.Cells[columnIndex + offset].Value = decimal.Multiply(targetCost, rate).ToString("#,0");

                row.Cells[96 + offset].Value = (Conversion.Parse((string)row.Cells[76 + offset].Value)
                                                + Conversion.Parse((string)row.Cells[78 + offset].Value)
                                                + Conversion.Parse((string)row.Cells[80 + offset].Value)
                                                + Conversion.Parse((string)row.Cells[82 + offset].Value)
                                                + Conversion.Parse((string)row.Cells[84 + offset].Value)
                                                + Conversion.Parse((string)row.Cells[86 + offset].Value)
                                                + Conversion.Parse((string)row.Cells[88 + offset].Value)
                                                + Conversion.Parse((string)row.Cells[90 + offset].Value)
                                                + Conversion.Parse((string)row.Cells[92 + offset].Value)
                                                + Conversion.Parse((string)row.Cells[94 + offset].Value)).ToString("#,0");
                total += Conversion.Parse((string)row.Cells[96 + offset].Value);
            }
            dataGridViewTotal.Rows[0].Cells[96 + offset].Value = total.ToString("#,0");
        }

        /*************************************************************
         * 経営利益の計算を行う
         *************************************************************/
        private new void calcManagementProfit()
        {
            for (int offset = 0; offset < 2; offset++)
            {
                decimal total = decimal.Zero;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    row.Cells[98 + offset].Value = decimal.Subtract(Conversion.Parse((string)row.Cells[72 + offset].Value)
                                                           , Conversion.Parse((string)row.Cells[96 + offset].Value)).ToString("#,0");
                    total += Conversion.Parse((string)row.Cells[51 + offset].Value);
                }
                dataGridViewTotal.Rows[0].Cells[98 + offset].Value = total.ToString("#,0");
            }
        }

        /*************************************************************
         * 製造原価・粗利益・粗利益率の計算を行う
         *************************************************************/
        private new void calcProfit()
        {
            for (int offset = 0; offset < 2; offset++)
            {
                decimal totalCost = decimal.Zero;
                decimal totalProfit = decimal.Zero;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    row.Cells[100 + offset].Value = (Conversion.Parse((string)row.Cells[54 + offset].Value)
                                            + Conversion.Parse((string)row.Cells[56 + offset].Value)
                                            + Conversion.Parse((string)row.Cells[58 + offset].Value)
                                            + Conversion.Parse((string)row.Cells[60 + offset].Value)
                                            + Conversion.Parse((string)row.Cells[62 + offset].Value)
                                            + Conversion.Parse((string)row.Cells[64 + offset].Value)
                                            + Conversion.Parse((string)row.Cells[66 + offset].Value)
                                            + Conversion.Parse((string)row.Cells[76 + offset].Value)
                                            + Conversion.Parse((string)row.Cells[78 + offset].Value)
                                            + Conversion.Parse((string)row.Cells[80 + offset].Value)
                                            + Conversion.Parse((string)row.Cells[82 + offset].Value)
                                            + Conversion.Parse((string)row.Cells[84 + offset].Value)
                                            + Conversion.Parse((string)row.Cells[86 + offset].Value)).ToString("#,0");
                    row.Cells[102 + offset].Value = decimal.Subtract(Conversion.Parse((string)row.Cells[26 + offset].Value)
                                                                     , Conversion.Parse((string)row.Cells[100 + offset].Value)).ToString("#,0");
                    row.Cells[104 + offset].Value = Conversion.Parse((string)row.Cells[26 + offset].Value) == decimal.Zero ?
                        decimal.Zero.ToString("P") :
                        decimal.Divide(Conversion.Parse((string)row.Cells[102 + offset].Value)
                                       , Conversion.Parse((string)row.Cells[26 + offset].Value)).ToString("P");

                    row.Cells[106 + offset].Value = Conversion.Parse((string)row.Cells[52 + offset].Value) == decimal.Zero ?
                        decimal.Zero.ToString("#,0") :
                        decimal.Divide(Conversion.Parse((string)row.Cells[100 + offset].Value)
                                       , Conversion.Parse((string)row.Cells[52 + offset].Value)).ToString("#,0");

                    row.Cells[108 + offset].Value = Conversion.Parse((string)row.Cells[52 + offset].Value) == decimal.Zero ?
                        decimal.Zero.ToString("#,0") :
                        decimal.Divide(Conversion.Parse((string)row.Cells[54 + offset].Value)
                                       + decimal.Divide(Conversion.Parse((string)row.Cells[56 + offset].Value), 2)
                                       + Conversion.Parse((string)row.Cells[60 + offset].Value)
                                       + Conversion.Parse((string)row.Cells[64 + offset].Value)
                                       + Conversion.Parse((string)row.Cells[66 + offset].Value)
                                       + decimal.Divide(Conversion.Parse((string)row.Cells[76 + offset].Value)
                                                        + Conversion.Parse((string)row.Cells[78 + offset].Value)
                                                        + Conversion.Parse((string)row.Cells[80 + offset].Value)
                                                        + Conversion.Parse((string)row.Cells[82 + offset].Value)
                                                        + Conversion.Parse((string)row.Cells[84 + offset].Value)
                                                        + Conversion.Parse((string)row.Cells[86 + offset].Value), 2)
                                       , Conversion.Parse((string)row.Cells[52 + offset].Value)).ToString("#,0");

                    totalCost += Conversion.Parse((string)row.Cells[100 + offset].Value);
                    totalProfit += Conversion.Parse((string)row.Cells[102 + offset].Value);
                }
                dataGridViewTotal.Rows[0].Cells[100 + offset].Value = totalCost.ToString("#,0");
                dataGridViewTotal.Rows[0].Cells[102 + offset].Value = totalProfit.ToString("#,0");
                dataGridViewTotal.Rows[0].Cells[104 + offset].Value =
                    Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[26 + offset].Value) == decimal.Zero ?
                    decimal.Zero.ToString("P") :
                    decimal.Divide(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[102 + offset].Value)
                                   , Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[26 + offset].Value)).ToString("P");
            }
        }


        /*************************************************************
         * チェックボックスのON/OFFに従い、表示列の制御を行う
         *************************************************************/
        private int[] checkedColumnIndex = { 6, 8, 10, 12, 14, 16, 18, 20, 22, 24
                                             , 54, 56, 58, 60, 62, 64, 66, 68, 70
                                             , 72, 74, 76, 78, 80, 82, 84, 86, 88
                                             , 90, 92, 94, 96, 98, 100, 102, 104, 106, 108 };

        private void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                bool state = checkedListBox.GetItemChecked(i);
                int index = checkedColumnIndex[i];

                for (int offset = 0; offset < 2; offset++)
                {
                    dataGridViewHeader.Columns[index + offset].Visible = state;
                    dataGridView.Columns[index + offset].Visible = state;
                    dataGridViewTotal.Columns[index + offset].Visible = state;
                }
            }
            resetScrollBars();
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
            base.btnOutput_Click(outputDir.Text, "予算実績対比表", Properties.Resources.template_compare);
        }
    }
}
