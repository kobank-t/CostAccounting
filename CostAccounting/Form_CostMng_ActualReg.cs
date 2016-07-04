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
    public partial class Form_CostMng_ActualReg : BaseCostMng
    {
        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_CostMng_ActualReg()
        {
            InitializeComponent();
            base.ChecBoxControls = this.groupMonth.Controls;
            base.setCategory(Const.CATEGORY_TYPE.Actual);
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_CostMng_ActualReg_Load(object sender, EventArgs e)
        {
            base.Form_Load(sender, e);

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
         * データグリッドビューの編集中のイベント追加
         *************************************************************/
        private new void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            base.dataGridView_EditingControlShowing(sender, e);
        }

        /*************************************************************
         * データグリッドビューの編集後のイベント削除
         *************************************************************/
        private new void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            base.dataGridView_CellEndEdit(sender, e);
        }

        /*************************************************************
         * データグリッドビューのコンボボックスをワンクリックで編集可能とする
         *************************************************************/
        private new void dataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            base.dataGridView_CellEnter(sender, e);
        }

        /*************************************************************
         * データグリッドビューのテキストをワンクリックで編集可能とする
         *************************************************************/
        private new void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            base.dataGridView_CellClick(sender, e);
        }

        /*************************************************************
         * データグリッドビューのテキストのロストフォーカス時に計算する
         *************************************************************/
        private new void dataGridView_Leave(object sender, EventArgs e)
        {
            base.dataGridView_Leave(sender, e);
        }

        /*************************************************************
         * 登録ボタン押下時の処理
         *************************************************************/
        private new void btnAppend_Click(object sender, EventArgs e)
        {
            base.btnAppend_Click(sender, e);
            Logger.Info(Message.INF003, new string[] { this.Text, Message.create(targetMonth, labelFilePath) });
        }

        /*************************************************************
         * CSVファイル参照ボタン押下時の処理
         *************************************************************/
        private void btnFileOpen_Click(object sender, EventArgs e)
        {
            string filePath;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                labelFilePath.Text = filePath;
            }
            else
            {
                return;
            }

            Form_CostMng_ActualReg_child form = new Form_CostMng_ActualReg_child(filePath, targetMonth.Value);
            form.setType(Const.STATUS_TYPE.Default);
            form.ShowDialog();

            if (form.DialogResult == DialogResult.OK)
            {
                Dictionary<decimal, int> targetIndex = new Dictionary<decimal, int> 
                {
                    { 4, 16 }, { 5, 17 }, { 6, 18 }, { 7, 19 }, { 8, 20 }, { 9, 21 },
                    { 10, 22 }, { 11, 23 }, { 12, 24 }, { 1, 25 }, { 2, 26 }, { 3, 27 }
                };

                Dictionary<decimal, string> numIndex = new Dictionary<decimal, string>
                {
                    { 4, "num04" }, { 5, "num05" }, { 6, "num06" },
                    { 7, "num07" }, { 8, "num08" }, { 9, "num09" },
                    { 10, "num10" }, { 11, "num11" }, { 12, "num12" },
                    { 1, "num01" }, { 2, "num02" }, { 3, "num03" }
                };


                // CSVデータを反映する前に、登録済みデータを削除
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    row.Cells[targetIndex[targetMonth.Value]].Value = decimal.Zero.ToString("#,0");
                    row.Cells[numIndex[targetMonth.Value]].Value = decimal.Zero.ToString("N");
                    base.calcRow(row.Index);
                }

                // CSVデータの反映
                bool allok = true;
                foreach (ListViewItem item in form.listView.Items)
                {
                    string supplier_code = item.SubItems[0].Text;
                    string product_code = item.SubItems[2].Text;

                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (supplier_code.Equals(row.Cells["supplier_code"].Value)
                            && product_code.Equals(row.Cells["product_code"].Value))
                        {
                            row.Cells[targetIndex[targetMonth.Value]].Value = Conversion.Parse(item.SubItems[4].Text).ToString("#,0");
                            row.Cells[numIndex[targetMonth.Value]].Value = Conversion.Parse(item.SubItems[5].Text).ToString("N");
                            base.calcRow(row.Index);
                            item.Tag = Const.FLG_ON;
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty((string)item.Tag))
                    {
                        item.Tag = Const.FLG_OFF;
                        allok = false;
                    }
                }
                base.calcColumn(targetIndex[targetMonth.Value]);

                if (!allok)
                {
                    form.setType(Const.STATUS_TYPE.Error);
                    form.ShowDialog(this);
                }

            }
            form.Dispose();
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
         * 固定費登録ボタン押下時の処理
         *************************************************************/
        private void btnFixedCost_Click(object sender, EventArgs e)
        {
            base.btnFixedCost_Click();
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
            base.btnOutput_Click(outputDir.Text, "実績表", Properties.Resources.template_list);
        }
    }
}
