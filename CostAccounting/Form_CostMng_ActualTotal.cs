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
            // 不要列を非表示
            int[] columunIndex = { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            foreach (int index in columunIndex)
            {
                dataGridViewHeader.Columns[index].Visible = false;
                dataGridView.Columns[index].Visible = false;
                dataGridViewTotal.Columns[index].Visible = false;
            }

            // 集計データを表示する
            showTotalizationData(sender, e);

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
        private void showTotalizationData(object sender, EventArgs e)
        {
            // 前回集計時の表示内容をクリア
            dataGridView.Rows.Clear();

            // カテゴリの設定
            Const.CATEGORY_TYPE category = Program.judgeCategory(radioBudget, radioActual);
            base.setCategory(category);

            // 集計前のデータ設定・計算
            base.Form_Load(sender, e);

            // 品種または取引先での集計データを設定する
            if (radioItem.Checked)
            {
                setData(1);
                dataGridViewHeader.Columns[1].Visible = true;
                dataGridView.Columns[1].Visible = true;
                dataGridViewTotal.Columns[1].Visible = true;
                dataGridViewHeader.Columns[2].Visible = false;
                dataGridView.Columns[2].Visible = false;
                dataGridViewTotal.Columns[2].Visible = false;
            }
            else if (radioSuppliers.Checked)
            {
                setData(2);
                dataGridViewHeader.Columns[1].Visible = false;
                dataGridView.Columns[1].Visible = false;
                dataGridViewTotal.Columns[1].Visible = false;
                dataGridViewHeader.Columns[2].Visible = true;
                dataGridView.Columns[2].Visible = true;
                dataGridViewTotal.Columns[2].Visible = true;
            }

            // 設定内容で計算する
            base.calcOnlyTotal();
            base.calcManagementProfit();
            base.calcProfit();

            //スクロールバーの状態をリセットする
            base.resetScrollBars();
        }

        /*************************************************************
         * データを画面に設定（品種でのグルーピング）
         *************************************************************/
        private void setData(int keyIndex)
        {

            Dictionary<String, decimal[]> aggregate = new Dictionary<string, decimal[]>();
            int[] columnlIndex = { 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27
                                     , 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39
                                     , 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 };

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                string key = (string)row.Cells[keyIndex].Value;
                decimal[] aggregateRow = aggregate.ContainsKey(key) ? aggregate[key] : new decimal[columnlIndex.Length];

                for (int i = 0; i < columnlIndex.Length; i++)
                {
                    if (columnlIndex[i] != 39)
                    {
                        aggregateRow[i] += Conversion.Parse((string)row.Cells[columnlIndex[i]].Value);
                    }
                    else
                    {
                        // 固定配賦率はパーセントを考慮した集計
                        string rateStr = (string)row.Cells[columnlIndex[i]].Value;
                        if (!string.IsNullOrEmpty(rateStr))
                            rateStr = rateStr.Replace("%", "");
                        decimal rate = decimal.Divide(Conversion.Parse(rateStr), 100);
                        aggregateRow[i] += rate;
                    }
                }

                // 値の置き換え
                if (aggregate.ContainsKey(key))
                {
                    aggregate[key] = aggregateRow;
                }
                else
                {
                    aggregate.Add(key, aggregateRow);
                }

            }

            // 集計データの設定
            dataGridView.Rows.Clear();
            dataGridView.RowCount = aggregate.Count;

            int index = 0;
            foreach (string key in aggregate.Keys)
            {
                decimal[] aggregateRow = aggregate[key];
                dataGridView.Rows[index].Cells[0].Value = (index + 1).ToString();
                dataGridView.Rows[index].Cells[keyIndex].Value = key;

                for (int i = 0; i < columnlIndex.Length; i++)
                {
                    if (!(28 <= columnlIndex[i] && columnlIndex[i] <= 39))
                    {
                        dataGridView.Rows[index].Cells[columnlIndex[i]].Value = aggregateRow[i].ToString("#,0");
                    }
                    else
                    {
                        // 数量は小数ありのフォーマットで表示
                        if (28 <= columnlIndex[i] && columnlIndex[i] <= 38)
                          dataGridView.Rows[index].Cells[columnlIndex[i]].Value = aggregateRow[i].ToString("N");

                        // 固定配賦率はパーセントのフォーマットで表示
                        if (columnlIndex[i] == 39)
                            dataGridView.Rows[index].Cells[columnlIndex[i]].Value = aggregateRow[i].ToString("P6");
                    }
                }
                ++index;
            }
        }

        /*************************************************************
         * 集計対象月のチェックボックスを全てONにする
         *************************************************************/
        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            base.changeCheckBoxState(groupMonth.Controls, true);
            showTotalizationData(sender, e);
        }

        /*************************************************************
         * 集計対象月のチェックボックスを全てOFFにする
         *************************************************************/
        private void btnAllClear_Click(object sender, EventArgs e)
        {
            base.changeCheckBoxState(groupMonth.Controls, false);
            showTotalizationData(sender, e);
        }

        /*************************************************************
         * チェックボックスのON/OFFに従い、表示月の制御と計算を行う
         *************************************************************/
        private new void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            base.checkBox_CheckedChanged(sender, e);
            showTotalizationData(sender, e);
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

            showTotalizationData(sender, e);
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
