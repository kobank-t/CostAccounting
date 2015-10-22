using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CostAccounting
{
    public class BaseCostMng : Form
    {
        // データグリッドビューのテキストボックスのコントロール
        private DataGridViewTextBoxEditingControl dgvTextBox = null;

        // データグリッドビューの変数定義
        private DataGridView dataGridView;
        private DataGridView dataGridViewTotal;
        private DataGridView dataGridViewHeader;

        protected DataGridView DataGridView
        {
            set { this.dataGridView = value; }
        }

        protected DataGridView DataGridViewTotal
        {
            set { this.dataGridViewTotal = value; }
        }

        protected DataGridView DataGridViewHeader
        {
            set { this.dataGridViewHeader = value; }
        }

        // チェックボックスのコントロール定義
        private Dictionary<string, CheckBox> checkBoxDic = new Dictionary<string, CheckBox>();
        private Dictionary<int, string> monthDic = new Dictionary<int, string> {
                { 16, "4月" }, { 17, "5月" }, { 18, "6月" }, { 19, "7月" },
                { 20, "8月" }, { 21, "9月" }, { 22, "10月" }, { 23, "11月" },
                { 24 , "12月" }, { 25, "1月" }, { 26, "2月" }, { 27, "3月" }
            };

        protected Control.ControlCollection ChecBoxControls
        {
            set
            {
                foreach (var control in value)
                {
                    if (control is CheckBox)
                    {
                        CheckBox checkbox = (CheckBox)control;
                        checkBoxDic.Add(checkbox.Text, checkbox);
                    }
                }
            }
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        protected void Form_Load(object sender, EventArgs e, Const.CATEGORY_TYPE category)
        {
            // ヘッダ行の初期化
            initDgvHeaderRow();

            // 合計行の初期化
            initDgvTotalRow();

            // データを設定する
            setData(category);

            // 設定内容で計算する
            calcAll();

            //スクロールバーの状態をリセットする
            resetScrollBars();
        }

        /*************************************************************
         * データグリッドビュー合計行の初期設定
         *************************************************************/
        protected void initDgvTotalRow()
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
        protected void initDgvHeaderRow()
        {
            dataGridViewHeader.RowCount = 2;
            dataGridViewHeader.Rows[0].Cells[0].Value = "No";
            dataGridViewHeader.Rows[0].Cells[1].Value = "品種";
            dataGridViewHeader.Rows[0].Cells[2].Value = "取引先";
            dataGridViewHeader.Rows[0].Cells[3].Value = "製品名";
            dataGridViewHeader.Rows[0].Cells[4].Value = "標準売価";
            dataGridViewHeader.Rows[1].Cells[4].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[5].Value = "原材料費";
            dataGridViewHeader.Rows[1].Cells[5].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[6].Value = "製造人件費";
            dataGridViewHeader.Rows[1].Cells[6].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[7].Value = "外注費";
            dataGridViewHeader.Rows[1].Cells[7].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[8].Value = "原料運賃";
            dataGridViewHeader.Rows[1].Cells[8].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[9].Value = "包装資材費";
            dataGridViewHeader.Rows[1].Cells[9].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[10].Value = "光熱費";
            dataGridViewHeader.Rows[1].Cells[10].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[11].Value = "消耗品費";
            dataGridViewHeader.Rows[0].Cells[12].Value = "荷造運賃";
            dataGridViewHeader.Rows[1].Cells[12].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[13].Value = "変動費+直接人件費(A)計";
            dataGridViewHeader.Rows[1].Cells[13].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[14].Value = "利益(売上-A)";
            dataGridViewHeader.Rows[1].Cells[14].Value = "(円/kg)";
            dataGridViewHeader.Rows[1].Cells[15].Value = "売り上げ(円)";
            dataGridViewHeader.Rows[1].Cells[16].Value = "4月";
            dataGridViewHeader.Rows[1].Cells[17].Value = "5月";
            dataGridViewHeader.Rows[1].Cells[18].Value = "6月";
            dataGridViewHeader.Rows[1].Cells[19].Value = "7月";
            dataGridViewHeader.Rows[1].Cells[20].Value = "8月";
            dataGridViewHeader.Rows[1].Cells[21].Value = "9月";
            dataGridViewHeader.Rows[1].Cells[22].Value = "10月";
            dataGridViewHeader.Rows[1].Cells[23].Value = "11月";
            dataGridViewHeader.Rows[1].Cells[24].Value = "12月";
            dataGridViewHeader.Rows[1].Cells[25].Value = "1月";
            dataGridViewHeader.Rows[1].Cells[26].Value = "2月";
            dataGridViewHeader.Rows[1].Cells[27].Value = "3月";
            dataGridViewHeader.Rows[1].Cells[28].Value = "数量";
            dataGridViewHeader.Rows[1].Cells[29].Value = "原材料費(円)";
            dataGridViewHeader.Rows[1].Cells[30].Value = "製造人件費(円)";
            dataGridViewHeader.Rows[1].Cells[31].Value = "外注費(円)";
            dataGridViewHeader.Rows[0].Cells[31].Value = "製造原価";
            dataGridViewHeader.Rows[1].Cells[32].Value = "原料運賃(円)";
            dataGridViewHeader.Rows[1].Cells[33].Value = "包装資材費(円)";
            dataGridViewHeader.Rows[1].Cells[34].Value = "水道光熱費(円)";
            dataGridViewHeader.Rows[1].Cells[35].Value = "消耗品費(円)";
            dataGridViewHeader.Rows[0].Cells[36].Value = "販管費";
            dataGridViewHeader.Rows[1].Cells[36].Value = "荷造運賃(円)";
            dataGridViewHeader.Rows[0].Cells[37].Value = "変動費+直接人件費(A)計";
            dataGridViewHeader.Rows[1].Cells[37].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[38].Value = "利益(売上-A)";
            dataGridViewHeader.Rows[1].Cells[38].Value = "(円/kg)";
            dataGridViewHeader.Rows[0].Cells[39].Value = "固定費配賦率";
            dataGridViewHeader.Rows[1].Cells[39].Value = "(転売品除く売上比率)";
            dataGridViewHeader.Rows[1].Cells[40].Value = "人件費";
            dataGridViewHeader.Rows[1].Cells[41].Value = "減価償却費";
            dataGridViewHeader.Rows[0].Cells[42].Value = "製造原価";
            dataGridViewHeader.Rows[1].Cells[42].Value = "賃借料";
            dataGridViewHeader.Rows[1].Cells[43].Value = "修繕費";
            dataGridViewHeader.Rows[1].Cells[44].Value = "在庫増減";
            dataGridViewHeader.Rows[1].Cells[45].Value = "その他経費";
            dataGridViewHeader.Rows[1].Cells[46].Value = "人件費";
            dataGridViewHeader.Rows[0].Cells[47].Value = "販売管理費";
            dataGridViewHeader.Rows[1].Cells[47].Value = "減価償却費";
            dataGridViewHeader.Rows[1].Cells[48].Value = "その他販管費";
            dataGridViewHeader.Rows[0].Cells[49].Value = "営業外費用/収益";
            dataGridViewHeader.Rows[0].Cells[50].Value = "固定費合計";
            dataGridViewHeader.Rows[1].Cells[50].Value = "(外含む)";
            dataGridViewHeader.Rows[0].Cells[51].Value = "経常利益(円)";
            dataGridViewHeader.Rows[0].Cells[52].Value = "製造原価";
            dataGridViewHeader.Rows[0].Cells[53].Value = "粗利益";
            dataGridViewHeader.Rows[0].Cells[54].Value = "粗利益率";
            dataGridViewHeader.Rows[0].Cells[55].Value = "原単価";
            dataGridViewHeader.Rows[0].Cells[56].Value = "仕掛品原価";

            for (int i = 0; i < 2; i++)
            {
                dataGridViewHeader.Rows[i].ReadOnly = true;
                dataGridViewHeader.Rows[i].DefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.Control);
                dataGridViewHeader.Rows[i].DefaultCellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Control);
                dataGridViewHeader.Rows[i].DefaultCellStyle.SelectionForeColor = Color.Black;
            }

            // 年間予定列の背景色を変更
            for (int i = 0; i < 2; i++)
            {
                for (int j = 15; j <= 28; j++)
                {
                    dataGridViewHeader.Rows[i].Cells[j].Style.BackColor = Color.LightBlue;
                    dataGridViewHeader.Rows[i].Cells[j].Style.SelectionBackColor = Color.LightBlue;
                }
            }

            // 製造原価列と販管費列の背景色を変更
            for (int i = 0; i < 2; i++)
            {
                for (int j = 29; j <= 38; j++)
                {
                    dataGridViewHeader.Rows[i].Cells[j].Style.BackColor = Color.LightGreen;
                    dataGridViewHeader.Rows[i].Cells[j].Style.SelectionBackColor = Color.LightGreen;
                }
            }

            // 固定費配賦率の背景色を変更
            for (int i = 0; i < 2; i++)
            {
                dataGridViewHeader.Rows[i].Cells[39].Style.BackColor = Color.LightBlue;
                dataGridViewHeader.Rows[i].Cells[39].Style.SelectionBackColor = Color.LightBlue;
            }

            // 製造原価列と販売管理費列の背景色を変更
            for (int i = 0; i < 2; i++)
            {
                for (int j = 40; j <= 51; j++)
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
        protected void dataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            dataGridViewHeader.FirstDisplayedScrollingColumnIndex = dataGridView.FirstDisplayedScrollingColumnIndex;
            dataGridViewHeader.HorizontalScrollingOffset = dataGridView.HorizontalScrollingOffset;
            dataGridViewTotal.FirstDisplayedScrollingColumnIndex = dataGridView.FirstDisplayedScrollingColumnIndex;
            dataGridViewTotal.HorizontalScrollingOffset = dataGridView.HorizontalScrollingOffset;
        }

        /*************************************************************
         * データグリッドビュー（ヘッダー）のセル描画処理
         *************************************************************/
        protected void dataGridViewHeader_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //下の境界線を消す
            if (e.RowIndex == 0 &
                    (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2 ||
                     e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 ||
                     e.ColumnIndex == 6 || e.ColumnIndex == 7 || e.ColumnIndex == 8 ||
                     e.ColumnIndex == 9 || e.ColumnIndex == 10 || e.ColumnIndex == 11 ||
                     e.ColumnIndex == 12 || e.ColumnIndex == 13 || e.ColumnIndex == 14 ||
                     e.ColumnIndex == 15 || e.ColumnIndex == 28 || e.ColumnIndex == 37 ||
                     e.ColumnIndex == 38 || e.ColumnIndex == 39 || e.ColumnIndex == 49 ||
                     e.ColumnIndex == 50 || e.ColumnIndex == 51 || e.ColumnIndex == 52 ||
                     e.ColumnIndex == 53 || e.ColumnIndex == 54 || e.ColumnIndex == 55 ||
                     e.ColumnIndex == 56
                    )
                )
            {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            }

            //右の境界線を消す
            if (e.RowIndex == 0 &
                    (e.ColumnIndex == 15 || e.ColumnIndex == 16 || e.ColumnIndex == 17 ||
                     e.ColumnIndex == 18 || e.ColumnIndex == 19 || e.ColumnIndex == 20 ||
                     e.ColumnIndex == 21 || e.ColumnIndex == 22 || e.ColumnIndex == 23 ||
                     e.ColumnIndex == 24 || e.ColumnIndex == 25 || e.ColumnIndex == 26 ||
                     e.ColumnIndex == 29 || e.ColumnIndex == 30 || e.ColumnIndex == 31 ||
                     e.ColumnIndex == 32 || e.ColumnIndex == 33 || e.ColumnIndex == 34 ||
                     e.ColumnIndex == 40 || e.ColumnIndex == 41 || e.ColumnIndex == 42 ||
                     e.ColumnIndex == 43 || e.ColumnIndex == 44 || e.ColumnIndex == 46 ||
                     e.ColumnIndex == 47
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
        protected void dataGridViewTotal_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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
        protected void dataGridViewHeader_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (dataGridView.Columns.Count > 0)
            {
                dataGridView.Columns[e.Column.Index].Width = dataGridViewHeader.Columns[e.Column.Index].Width;

                if (dataGridViewHeader.FirstDisplayedScrollingColumnIndex != -1)
                {
                    dataGridViewHeader.FirstDisplayedScrollingColumnIndex = dataGridView.FirstDisplayedScrollingColumnIndex;
                    dataGridViewHeader.HorizontalScrollingOffset = dataGridView.HorizontalScrollingOffset;
                }
            }

            if (dataGridViewTotal.Columns.Count > 0)
            {
                dataGridViewTotal.Columns[e.Column.Index].Width = dataGridViewHeader.Columns[e.Column.Index].Width;

                if (dataGridViewTotal.FirstDisplayedScrollingColumnIndex != -1)
                {
                    dataGridViewTotal.FirstDisplayedScrollingColumnIndex = dataGridView.FirstDisplayedScrollingColumnIndex;
                    dataGridViewTotal.HorizontalScrollingOffset = dataGridView.HorizontalScrollingOffset;
                }
            }
        }

        /*************************************************************
         * データグリッドビューの編集中のイベント追加
         *************************************************************/
        protected void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            // テキストボックスのイベント追加
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                dgvTextBox = (DataGridViewTextBoxEditingControl)e.Control;
                dgvTextBox.KeyPress += new KeyPressEventHandler(Event.textBox_KeyPress_numeric);

                if (dgv.Name.Equals(dataGridView.Name))
                    dgvTextBox.Leave += new EventHandler(dataGridView_Leave);
                else if (dgv.Name.Equals(dataGridViewTotal.Name))
                    dgvTextBox.Leave += new EventHandler(dataGridViewTotal_Leave);
            }
        }

        /*************************************************************
         * データグリッドビューの編集後のイベント削除
         *************************************************************/
        protected void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            // テキストボックスのイベント削除
            if (dgvTextBox != null)
            {
                dgvTextBox.KeyPress -= new KeyPressEventHandler(Event.textBox_KeyPress_numeric);

                if (dgv.Name.Equals(dataGridView.Name))
                    dgvTextBox.Leave -= new EventHandler(dataGridView_Leave);
                else if (dgv.Name.Equals(dataGridViewTotal.Name))
                    dgvTextBox.Leave -= new EventHandler(dataGridViewTotal_Leave);

                dgvTextBox = null;
            }
        }

        /*************************************************************
         * データグリッドビューのコンボボックスをワンクリックで編集可能とする
         *************************************************************/
        protected void dataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex > decimal.MinusOne)
            {
                if (dgv.Columns[e.ColumnIndex] is DataGridViewTextBoxColumn)
                {
                    dgv.BeginEdit(true);
                }
            }
        }

        /*************************************************************
         * データグリッドビューのテキストをワンクリックで編集可能とする
         *************************************************************/
        protected void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex > decimal.MinusOne)
            {
                if (dgv.Columns[e.ColumnIndex] is DataGridViewTextBoxColumn)
                {
                    SendKeys.Send("{F2}");
                }
            }
        }

        /*************************************************************
         * 本画面の入力値に対して全て計算する
         *************************************************************/
        protected void calcAll()
        {
            int[] columnIndex = { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 };

            foreach (DataGridViewRow row in dataGridView.Rows)
                calcRow(row.Index);

            foreach (int index in columnIndex)
                calcColumn(index);

            calcProceeds();
            calcOnlyTotal();
            calcManagementProfit();
            calcProfit();

            int[] columnIndexTotal = { 40, 41, 42, 43, 44, 45, 46, 47, 48, 49 };
            foreach (int index in columnIndexTotal)
            {
                dataGridViewTotal.Columns[index].Selected = true;
                dataGridViewTotal_Leave(null, null);
            }
        }

        /*************************************************************
         * スクロールバーの状態をリセットする
         *************************************************************/
        protected void resetScrollBars()
        {
            if (dataGridView.Rows.Count > 0)
            {
                int fdsr = dataGridView.FirstDisplayedScrollingRowIndex;
                dataGridView.ScrollBars = ScrollBars.None;
                dataGridView.ScrollBars = ScrollBars.Both;
                dataGridView.FirstDisplayedScrollingRowIndex = fdsr;
                dataGridView.Rows[0].Cells[0].Selected = true;
            }
        }

        /*************************************************************
         * データグリッドビューのテキストのロストフォーカス時に計算する
         *************************************************************/
        protected void dataGridView_Leave(object sender, EventArgs e)
        {
            int rowIndex = dataGridView.SelectedCells[0].RowIndex;
            int columnIndex = dataGridView.SelectedCells[0].ColumnIndex;

            calcRow(rowIndex);
            calcColumn(columnIndex);
            calcProceeds();
            calcOnlyTotal();
            calcManagementProfit();
            calcProfit();

            dataGridView.Rows[rowIndex].Cells[columnIndex].Value = Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[columnIndex].Value).ToString("N");
        }

        /*************************************************************
         * データグリッドビュー（合計）のテキストのロストフォーカス時に計算する
         *************************************************************/
        protected void dataGridViewTotal_Leave(object sender, EventArgs e)
        {
            int columnIndex = dataGridViewTotal.SelectedColumns[0].Index;

            calcTotal(columnIndex);
            calcManagementProfit();
            calcProfit();

            dataGridViewTotal.Rows[0].Cells[columnIndex].Value = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[columnIndex].Value).ToString("N");
        }

        /*************************************************************
         * データを画面に設定
         *************************************************************/
        private void setData(Const.CATEGORY_TYPE category)
        {
            using (var context = new CostAccountingEntities())
            {
                //------------------------------------------------------------------------------ 各商品の入力値を設定
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
                             select new { t_product, t_supplier, m_product, m_supplier, m_item };

                var dataList = target.ToList();
                dataGridView.RowCount = dataList.Count;

                for (int i = 0; i < dataList.Count; i++)
                {
                    dataGridView.Rows[i].Cells[0].Value = (i + 1).ToString();
                    dataGridView.Rows[i].Cells[1].Value = dataList[i].m_item.name;
                    dataGridView.Rows[i].Cells[2].Value = dataList[i].m_supplier.name;
                    dataGridView.Rows[i].Cells[3].Value = dataList[i].m_product.name;
                    dataGridView.Rows[i].Cells[4].Value = dataList[i].t_supplier.unit_price.ToString("N");

                    dataGridView.Rows[i].Cells[5].Value = dataList[i].t_product.material_cost.ToString("N");
                    dataGridView.Rows[i].Cells[6].Value = dataList[i].t_product.labor_cost_direct.ToString("N");
                    dataGridView.Rows[i].Cells[7].Value = dataList[i].t_product.contractors_cost.ToString("N");
                    dataGridView.Rows[i].Cells[8].Value = dataList[i].t_product.materials_fare.ToString("N");
                    dataGridView.Rows[i].Cells[9].Value = dataList[i].t_product.packing_cost.ToString("N");
                    dataGridView.Rows[i].Cells[10].Value = dataList[i].t_product.utilities_cost.ToString("N");
                    dataGridView.Rows[i].Cells[11].Value = decimal.Multiply(dataList[i].t_product.other_cost, (decimal)0.15948).ToString("N");
                    dataGridView.Rows[i].Cells[12].Value = dataList[i].t_product.packing_fare.ToString("N");

                    dataGridView.Rows[i].Cells[13].Value = (dataList[i].t_product.material_cost
                                                            + dataList[i].t_product.labor_cost_direct
                                                            + dataList[i].t_product.contractors_cost
                                                            + dataList[i].t_product.materials_fare
                                                            + dataList[i].t_product.packing_cost
                                                            + dataList[i].t_product.utilities_cost
                                                            + decimal.Multiply(dataList[i].t_product.other_cost, (decimal)0.15948)
                                                            + dataList[i].t_product.packing_fare).ToString("N");

                    dataGridView.Rows[i].Cells[14].Value = decimal.Subtract(Conversion.Parse((string)dataGridView.Rows[i].Cells[4].Value)
                                                                            , Conversion.Parse((string)dataGridView.Rows[i].Cells[13].Value)).ToString("N");

                    dataGridView.Rows[i].Cells[16].Value = dataList[i].t_supplier.month_04.ToString("N");
                    dataGridView.Rows[i].Cells[17].Value = dataList[i].t_supplier.month_05.ToString("N");
                    dataGridView.Rows[i].Cells[18].Value = dataList[i].t_supplier.month_06.ToString("N");
                    dataGridView.Rows[i].Cells[19].Value = dataList[i].t_supplier.month_07.ToString("N");
                    dataGridView.Rows[i].Cells[20].Value = dataList[i].t_supplier.month_08.ToString("N");
                    dataGridView.Rows[i].Cells[21].Value = dataList[i].t_supplier.month_09.ToString("N");
                    dataGridView.Rows[i].Cells[22].Value = dataList[i].t_supplier.month_10.ToString("N");
                    dataGridView.Rows[i].Cells[23].Value = dataList[i].t_supplier.month_11.ToString("N");
                    dataGridView.Rows[i].Cells[24].Value = dataList[i].t_supplier.month_12.ToString("N");
                    dataGridView.Rows[i].Cells[25].Value = dataList[i].t_supplier.month_01.ToString("N");
                    dataGridView.Rows[i].Cells[26].Value = dataList[i].t_supplier.month_02.ToString("N");
                    dataGridView.Rows[i].Cells[27].Value = dataList[i].t_supplier.month_03.ToString("N");
                    dataGridView.Rows[i].Cells["product_code"].Value = dataList[i].t_supplier.product_code;
                    dataGridView.Rows[i].Cells["supplier_code"].Value = dataList[i].t_supplier.supplier_code;
                }

                //------------------------------------------------------------------------------ 合計行の入力値を設定
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
         * 指定列の合計を計算する
         *************************************************************/
        protected void calcColumn(int columnIndex)
        {
            decimal total = decimal.Zero;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                total += Conversion.Parse((string)row.Cells[columnIndex].Value);
            }

            dataGridViewTotal.Rows[0].Cells[columnIndex].Value = total;
        }

        /*************************************************************
         * 計算対象の月に含まれるか判断する
         *************************************************************/
        protected decimal containCalc(string value, int index)
        {
            decimal ret;
            if (checkBoxDic.ContainsKey(monthDic[index]) && checkBoxDic[monthDic[index]].Checked)
                ret = Conversion.Parse(value);
            else
                ret = decimal.Zero;

            return ret;
        }

        /*************************************************************
         * 売り上げ列を計算する
         *************************************************************/
        private void calcProceeds()
        {
            // 売り上げの計算
            decimal total = decimal.Zero;
            decimal excludeResaleTotal = decimal.Zero;

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (!"転売品".Equals(row.Cells[1].Value))
                    excludeResaleTotal += Conversion.Parse((string)row.Cells[15].Value);
                total += Conversion.Parse((string)row.Cells[15].Value);
            }
            dataGridViewTotal.Rows[0].Cells[15].Value = total.ToString("N");
            dataGridViewTotal.Rows[1].Cells[15].Value = excludeResaleTotal.ToString("N");

            // 固定費配賦率の計算
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if ("転売品".Equals(row.Cells[1].Value))
                    row.Cells[39].Value = decimal.Zero.ToString("P");
                else
                    row.Cells[39].Value = excludeResaleTotal == decimal.Zero ?
                        decimal.Zero.ToString("P") :
                        decimal.Divide(Conversion.Parse((string)row.Cells[15].Value), excludeResaleTotal).ToString("P");
            }
        }

        /*************************************************************
         * 合計行の計算を行う
         *************************************************************/
        private void calcTotal(int columnIndex)
        {
            decimal total = decimal.Zero;
            decimal targetCost = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[columnIndex].Value);
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                string rateStr = (string)row.Cells[39].Value;
                if (!string.IsNullOrEmpty(rateStr))
                    rateStr = rateStr.Replace("%", "");
                decimal rate = decimal.Divide(Conversion.Parse(rateStr), 100);
                row.Cells[columnIndex].Value = decimal.Multiply(targetCost, rate).ToString("N");

                row.Cells[50].Value = (Conversion.Parse((string)row.Cells[40].Value)
                                       + Conversion.Parse((string)row.Cells[41].Value)
                                       + Conversion.Parse((string)row.Cells[42].Value)
                                       + Conversion.Parse((string)row.Cells[43].Value)
                                       + Conversion.Parse((string)row.Cells[44].Value)
                                       + Conversion.Parse((string)row.Cells[45].Value)
                                       + Conversion.Parse((string)row.Cells[46].Value)
                                       + Conversion.Parse((string)row.Cells[47].Value)
                                       + Conversion.Parse((string)row.Cells[48].Value)
                                       + Conversion.Parse((string)row.Cells[49].Value)).ToString("N");
                total += Conversion.Parse((string)row.Cells[50].Value);
            }
            dataGridViewTotal.Rows[0].Cells[50].Value = total.ToString("N");
        }

        /*************************************************************
         * 経営利益の計算を行う
         *************************************************************/
        private void calcManagementProfit()
        {
            decimal total = decimal.Zero;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                row.Cells[51].Value = decimal.Subtract(Conversion.Parse((string)row.Cells[38].Value)
                                                       , Conversion.Parse((string)row.Cells[50].Value)).ToString("N");
                total += Conversion.Parse((string)row.Cells[51].Value);
            }
            dataGridViewTotal.Rows[0].Cells[51].Value = total.ToString("N");
        }

        /*************************************************************
         * 製造原価・粗利益・粗利益率の計算を行う
         *************************************************************/
        private void calcProfit()
        {
            decimal totalCost = decimal.Zero;
            decimal totalProfit = decimal.Zero;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                row.Cells[52].Value = (Conversion.Parse((string)row.Cells[29].Value)
                                       + Conversion.Parse((string)row.Cells[30].Value)
                                       + Conversion.Parse((string)row.Cells[31].Value)
                                       + Conversion.Parse((string)row.Cells[32].Value)
                                       + Conversion.Parse((string)row.Cells[33].Value)
                                       + Conversion.Parse((string)row.Cells[34].Value)
                                       + Conversion.Parse((string)row.Cells[35].Value)
                                       + Conversion.Parse((string)row.Cells[40].Value)
                                       + Conversion.Parse((string)row.Cells[41].Value)
                                       + Conversion.Parse((string)row.Cells[42].Value)
                                       + Conversion.Parse((string)row.Cells[43].Value)
                                       + Conversion.Parse((string)row.Cells[44].Value)
                                       + Conversion.Parse((string)row.Cells[45].Value)).ToString("N");
                row.Cells[53].Value = decimal.Subtract(Conversion.Parse((string)row.Cells[15].Value)
                                                       , Conversion.Parse((string)row.Cells[52].Value)).ToString("N");
                row.Cells[54].Value = Conversion.Parse((string)row.Cells[15].Value) == decimal.Zero ?
                    decimal.Zero.ToString("P") :
                    decimal.Divide(Conversion.Parse((string)row.Cells[53].Value)
                                   , Conversion.Parse((string)row.Cells[15].Value)).ToString("P");

                row.Cells[55].Value = Conversion.Parse((string)row.Cells[28].Value) == decimal.Zero ?
                    decimal.Zero.ToString("P") :
                    decimal.Divide(Conversion.Parse((string)row.Cells[52].Value)
                                   , Conversion.Parse((string)row.Cells[28].Value)).ToString("P");

                row.Cells[56].Value = Conversion.Parse((string)row.Cells[28].Value) == decimal.Zero ?
                    decimal.Zero.ToString("P") :
                    decimal.Divide(Conversion.Parse((string)row.Cells[29].Value)
                                   + decimal.Divide(Conversion.Parse((string)row.Cells[30].Value), 2)
                                   + Conversion.Parse((string)row.Cells[32].Value)
                                   + Conversion.Parse((string)row.Cells[34].Value)
                                   + Conversion.Parse((string)row.Cells[35].Value)
                                   + decimal.Divide(Conversion.Parse((string)row.Cells[40].Value)
                                                    + Conversion.Parse((string)row.Cells[41].Value)
                                                    + Conversion.Parse((string)row.Cells[42].Value)
                                                    + Conversion.Parse((string)row.Cells[43].Value)
                                                    + Conversion.Parse((string)row.Cells[44].Value)
                                                    + Conversion.Parse((string)row.Cells[45].Value), 2)
                                   , Conversion.Parse((string)row.Cells[28].Value)).ToString("P");

                totalCost += Conversion.Parse((string)row.Cells[52].Value);
                totalProfit += Conversion.Parse((string)row.Cells[53].Value);
            }
            dataGridViewTotal.Rows[0].Cells[52].Value = totalCost.ToString("N");
            dataGridViewTotal.Rows[0].Cells[53].Value = totalProfit.ToString("N");
            dataGridViewTotal.Rows[0].Cells[54].Value =
                Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[15].Value) == decimal.Zero ?
                decimal.Zero.ToString("P") :
                decimal.Divide(Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[53].Value)
                               , Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[15].Value)).ToString("P");
        }

        /*************************************************************
         * 指定行の計算を行う
         *************************************************************/
        private void calcRow(int rowIndex)
        {
            dataGridView.Rows[rowIndex].Cells[15].Value =
                (containCalc((string)dataGridView.Rows[rowIndex].Cells[16].Value, 16)
                 + containCalc((string)dataGridView.Rows[rowIndex].Cells[17].Value, 17)
                 + containCalc((string)dataGridView.Rows[rowIndex].Cells[18].Value, 18)
                 + containCalc((string)dataGridView.Rows[rowIndex].Cells[19].Value, 19)
                 + containCalc((string)dataGridView.Rows[rowIndex].Cells[20].Value, 20)
                 + containCalc((string)dataGridView.Rows[rowIndex].Cells[21].Value, 21)
                 + containCalc((string)dataGridView.Rows[rowIndex].Cells[22].Value, 22)
                 + containCalc((string)dataGridView.Rows[rowIndex].Cells[23].Value, 23)
                 + containCalc((string)dataGridView.Rows[rowIndex].Cells[24].Value, 24)
                 + containCalc((string)dataGridView.Rows[rowIndex].Cells[25].Value, 25)
                 + containCalc((string)dataGridView.Rows[rowIndex].Cells[26].Value, 26)
                 + containCalc((string)dataGridView.Rows[rowIndex].Cells[27].Value, 27)).ToString("N");

            dataGridView.Rows[rowIndex].Cells[28].Value =
                        Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[4].Value) == decimal.Zero ?
                        decimal.Zero.ToString("N") :
                        decimal.Divide(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[15].Value)
                                       , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[4].Value)).ToString("N");

            dataGridView.Rows[rowIndex].Cells[29].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[28].Value)
                                                                           , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[5].Value)).ToString("N");
            dataGridView.Rows[rowIndex].Cells[30].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[28].Value)
                                                                           , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[6].Value)).ToString("N");
            dataGridView.Rows[rowIndex].Cells[31].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[28].Value)
                                                                           , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[7].Value)).ToString("N");
            dataGridView.Rows[rowIndex].Cells[32].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[28].Value)
                                                                           , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[8].Value)).ToString("N");
            dataGridView.Rows[rowIndex].Cells[33].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[28].Value)
                                                                           , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[9].Value)).ToString("N");
            dataGridView.Rows[rowIndex].Cells[34].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[28].Value)
                                                                           , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[10].Value)).ToString("N");
            dataGridView.Rows[rowIndex].Cells[35].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[28].Value)
                                                                           , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[11].Value)).ToString("N");
            dataGridView.Rows[rowIndex].Cells[36].Value = decimal.Multiply(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[28].Value)
                                                                           , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[12].Value)).ToString("N");
            dataGridView.Rows[rowIndex].Cells[37].Value = (Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[29].Value)
                                                           + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[30].Value)
                                                           + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[31].Value)
                                                           + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[32].Value)
                                                           + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[33].Value)
                                                           + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[34].Value)
                                                           + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[35].Value)
                                                           + Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[36].Value)).ToString("N");
            dataGridView.Rows[rowIndex].Cells[38].Value = decimal.Subtract(Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[15].Value)
                                                                           , Conversion.Parse((string)dataGridView.Rows[rowIndex].Cells[37].Value)).ToString("N");
        }

        /*************************************************************
         * 合計値の算出のみ必要な列の計算を行う
         *************************************************************/
        private void calcOnlyTotal()
        {
            int[] columnIndex = { 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38 };
            decimal[] total = new decimal[columnIndex.Count()];

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                for (int i = 0; i < columnIndex.Count(); i++)
                    total[i] += Conversion.Parse((string)row.Cells[columnIndex[i]].Value);
            }

            for (int i = 0; i < columnIndex.Count(); i++)
                dataGridViewTotal.Rows[0].Cells[columnIndex[i]].Value = total[i].ToString("N");
        }

        /*************************************************************
         * 登録ボタン押下時の処理
         *************************************************************/
        protected void btnAppend_Click(object sender, EventArgs e, Const.CATEGORY_TYPE category)
        {
            if (Program.MessageBoxBefore("登録しますか？") != DialogResult.Yes)
            {
                return;
            }

            // 入力チェックを行う
            if (!checkInputData())
            {
                return;
            }

            // 登録処理を行う
            using (var context = new CostAccountingEntities())
            {
                //----------------------------------------- 商品と取引先ごとの各月の入力内容を登録
                foreach (DataGridViewRow row in dataGridView.Rows)
                {

                    string productCode = (string)row.Cells["product_code"].Value;
                    string supplierCode = (string)row.Cells["supplier_code"].Value;

                    var target = from t in context.ProductSupplier
                                 where t.year.Equals(Const.TARGET_YEAR)
                                    && t.product_code.Equals(productCode)
                                    && t.supplier_code.Equals(supplierCode)
                                    && t.category.Equals((int)category)
                                 select t;

                    if (target.Count() > 0)
                    {
                        target.First().month_04 = Conversion.Parse((string)row.Cells[16].Value);
                        target.First().month_05 = Conversion.Parse((string)row.Cells[17].Value);
                        target.First().month_06 = Conversion.Parse((string)row.Cells[18].Value);
                        target.First().month_07 = Conversion.Parse((string)row.Cells[19].Value);
                        target.First().month_08 = Conversion.Parse((string)row.Cells[20].Value);
                        target.First().month_09 = Conversion.Parse((string)row.Cells[21].Value);
                        target.First().month_10 = Conversion.Parse((string)row.Cells[22].Value);
                        target.First().month_11 = Conversion.Parse((string)row.Cells[23].Value);
                        target.First().month_12 = Conversion.Parse((string)row.Cells[24].Value);
                        target.First().month_01 = Conversion.Parse((string)row.Cells[25].Value);
                        target.First().month_02 = Conversion.Parse((string)row.Cells[26].Value);
                        target.First().month_03 = Conversion.Parse((string)row.Cells[27].Value);
                        target.First().update_user = SystemInformation.UserName;
                        target.First().update_date = DateTime.Now;
                    }
                }

                //----------------------------------------- 合計値の入力内容を登録
                var budget = from t in context.CostMngTotal
                             where t.year.Equals(Const.TARGET_YEAR)
                                && t.category.Equals((int)category)
                             select t;

                if (budget.Count() > 0)
                {
                    // 更新
                    budget.First().manufacturing_personnel = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[40].Value);
                    budget.First().manufacturing_depreciation = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[41].Value);
                    budget.First().manufacturing_rent = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[42].Value);
                    budget.First().manufacturing_repair = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[43].Value);
                    budget.First().manufacturing_stock = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[44].Value);
                    budget.First().manufacturing_other = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[45].Value);
                    budget.First().selling_personnel = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[46].Value);
                    budget.First().selling_depreciation = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[47].Value);
                    budget.First().selling_other = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[48].Value);
                    budget.First().operating_expenses = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[49].Value);
                    budget.First().update_user = SystemInformation.UserName;
                    budget.First().update_date = DateTime.Now;
                }
                else
                {
                    // 新規登録
                    var entity = new CostMngTotal()
                    {
                        year = Const.TARGET_YEAR,
                        category = (int)category,
                        manufacturing_personnel = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[40].Value),
                        manufacturing_depreciation = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[41].Value),
                        manufacturing_rent = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[42].Value),
                        manufacturing_repair = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[43].Value),
                        manufacturing_stock = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[44].Value),
                        manufacturing_other = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[45].Value),
                        selling_personnel = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[46].Value),
                        selling_depreciation = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[47].Value),
                        selling_other = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[48].Value),
                        operating_expenses = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[49].Value),
                        update_user = SystemInformation.UserName,
                        update_date = DateTime.Now,
                        del_flg = Const.FLG_OFF
                    };
                    context.CostMngTotal.Add(entity);
                }

                //----------------------------------------- 実績登録の場合は、乖離幅測定データも登録
                if (Const.CATEGORY_TYPE.Actual.Equals(category))
                {
                    var divergence = from t in context.Divergence
                                     where t.year.Equals(Const.TARGET_YEAR)
                                        && t.del_flg.Equals(Const.FLG_OFF)
                                     select t;

                    if (divergence.Count() == 0)
                    {
                        // 登録処理
                        var entity = new Divergence()
                        {
                            year = Const.TARGET_YEAR,
                            materialCost_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[29].Value),
                            laborCost_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[30].Value),
                            contractorsCost_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[31].Value),
                            materialsFare_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[32].Value),
                            packingCost_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[33].Value),
                            utilitiesCost_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[34].Value),
                            otherCost_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[35].Value),
                            packingFare_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[36].Value),
                            update_user = SystemInformation.UserName,
                            update_date = DateTime.Now,
                            del_flg = Const.FLG_OFF
                        };
                        context.Divergence.Add(entity);
                    }
                    else
                    {
                        // 更新処理
                        divergence.First().materialCost_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[29].Value);
                        divergence.First().laborCost_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[30].Value);
                        divergence.First().contractorsCost_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[31].Value);
                        divergence.First().materialsFare_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[32].Value);
                        divergence.First().packingCost_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[33].Value);
                        divergence.First().utilitiesCost_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[34].Value);
                        divergence.First().otherCost_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[35].Value);
                        divergence.First().packingFare_costing = Conversion.Parse((string)dataGridViewTotal.Rows[0].Cells[36].Value);
                        divergence.First().update_user = SystemInformation.UserName;
                        divergence.First().update_date = DateTime.Now;
                    }

                }

                context.SaveChanges();
            }

            Program.MessageBoxAfter("登録しました。");
        }

        /*************************************************************
         * 入力チェックを行う
         *************************************************************/
        private bool checkInputData()
        {
            bool ret = true;


            return ret;
        }

        /*************************************************************
         * チェックボックスの状態を変更する
         *************************************************************/
        protected void changeCheckBoxState(Control.ControlCollection controls, bool state)
        {
            foreach (var control in controls)
            {
                if (control is CheckBox)
                    ((CheckBox)control).Checked = state;
            }
            calcAll();
            resetScrollBars();
        }

        /*************************************************************
         * チェックボックスのON/OFFに従い、表示月の制御と計算を行う
         *************************************************************/
        protected void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox target = (CheckBox)sender;

            if (monthDic.ContainsValue(target.Text))
            {
                int index = monthDic.First(x => x.Value.Equals(target.Text)).Key;
                bool state = target.Checked;

                dataGridViewHeader.Columns[index].Visible = state;
                dataGridView.Columns[index].Visible = state;
                dataGridViewTotal.Columns[index].Visible = state;

                calcAll();
                resetScrollBars();
            }
        }
    }
}
