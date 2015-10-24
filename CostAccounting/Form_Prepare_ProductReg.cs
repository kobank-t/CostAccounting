using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace CostAccounting
{
    public partial class Form_Prepare_ProductReg : Form
    {
        public Form_Prepare_ProductReg()
        {
            InitializeComponent();
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_Prepare_ProductReg_Load(object sender, EventArgs e)
        {
            using (var context = new CostAccountingEntities())
            {
                var list = from t in context.ProductCode
                           where t.year.Equals(Const.TARGET_YEAR)
                              && t.del_flg.Equals(Const.FLG_OFF)
                           orderby t.code
                           select new { t.code, t.name, t.unit, t.kbn, t.note };

                dataGridView.DataSource = list.ToList();
            }

            SelectedRows();
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
                var list = from t in context.ProductCode
                           where t.year.Equals(Const.TARGET_YEAR)
                              && (string.IsNullOrEmpty(code) || t.code.StartsWith(code))
                              && t.del_flg.Equals(Const.FLG_OFF)
                           orderby t.code
                           select new { t.code, t.name, t.unit, t.kbn, t.note };

                var ret = list.ToList();
                if (!string.IsNullOrEmpty(name))
                {
                    foreach (var data in list.ToList())
                        if (!data.name.Contains(name))
                            ret.Remove(data);
                }
                dataGridView.DataSource = ret;
            }

            SelectedRows();
        }

        /*************************************************************
         * dataGridView任意行を選択時の処理
         *************************************************************/
        private void dataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            SelectedRows();
        }

        /*************************************************************
         * 選択行の情報を表示
         *************************************************************/
        private void SelectedRows()
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                textRegCode.Text = dataGridView.SelectedRows[0].Cells[0].Value.ToString();
                textRegName.Text = dataGridView.SelectedRows[0].Cells[1].Value.ToString();
                textRegUnit.Text = dataGridView.SelectedRows[0].Cells[2].Value.ToString();
                checkedProductKbn(dataGridView.SelectedRows[0].Cells[3].Value.ToString());
                textRegNote.Text = (string)dataGridView.SelectedRows[0].Cells[4].Value;
            }
        }


        private void checkedProductKbn(string target)
        {
            foreach (RadioButton button in groupKbn1.Controls)
            {
                if (button.Text.Equals(target))
                {
                    button.Checked = true;
                }
                else
                {
                    button.Checked = false;
                }

            }
        }

        private String getProductKbn(GroupBox target)
        {
            string ret = String.Empty;
            foreach (RadioButton button in target.Controls)
            {
                if (button.Checked)
                {
                    ret = button.Text;
                    break;
                }
            }
            return ret;
        }

        /*************************************************************
         * 登録ボタン押下時の処理
         *************************************************************/
        private void btnAppend_Click(object sender, EventArgs e)
        {
            if (Program.MessageBoxBefore("登録しますか？") == DialogResult.Yes)
            {
                // 登録データのオブジェクトを作成
                var entity = new ProductCode()
                {
                    code = textRegCode.Text,
                    year = Const.TARGET_YEAR,
                    name = textRegName.Text,
                    unit = textRegUnit.Text,
                    kbn = getProductKbn(groupKbn1),
                    note = textRegNote.Text,
                    update_user = SystemInformation.UserName,
                    update_date = DateTime.Now,
                    del_flg = Const.FLG_OFF
                };

                // データ登録処理（既に同じコードが存在する場合は登録しない）
                using (var context = new CostAccountingEntities())
                {
                    var target = from t in context.ProductCode
                                 where t.code.Equals(entity.code) && t.year.Equals(entity.year)
                                 select t;
                    if (target.Count() == 0)
                    {
                        context.ProductCode.Add(entity);
                        context.SaveChanges();
                        Program.MessageBoxAfter("登録し、再検索を行いました。");
                        btnSearch_Click(sender, e);
                    }
                    else
                    {
                        Program.MessageBoxError("既に同じコードの商品が登録されています。");
                    }

                }
            }
        }

        /*************************************************************
         * 修正ボタン押下時の処理
         *************************************************************/
        private void btnChange_Click(object sender, EventArgs e)
        {
            if (Program.MessageBoxBefore("更新しますか？") == DialogResult.Yes)
            {
                using (var context = new CostAccountingEntities())
                {
                    var target = from t in context.ProductCode
                                 where t.year.Equals(Const.TARGET_YEAR)
                                    && t.code.Equals(textRegCode.Text)
                                 select t;

                    if (target.Count() > 0)
                    {
                        target.First().code = textRegCode.Text;
                        target.First().name = textRegName.Text;
                        target.First().unit = textRegUnit.Text;
                        target.First().kbn = getProductKbn(groupKbn1);
                        target.First().note = textRegNote.Text;
                        target.First().update_user = SystemInformation.UserName;
                        target.First().update_date = DateTime.Now;
                    }
                    context.SaveChanges();
                }

                Program.MessageBoxAfter("更新し、再検索を行いました。");
                btnSearch_Click(sender, e);
            }
        }

        /*************************************************************
         * 削除ボタン押下時の処理
         *************************************************************/
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Program.MessageBoxBefore("削除しますか？") == DialogResult.Yes)
            {
                using (var context = new CostAccountingEntities())
                {
                    var target = from t in context.ProductCode
                                 where t.year.Equals(Const.TARGET_YEAR)
                                    && t.code.Equals(textRegCode.Text)
                                 select t;

                    if (target.Count() > 0)
                    {
                        //target.First().del_flg = Const.FLG_ON;
                        context.ProductCode.Remove(target.First());
                    }
                    context.SaveChanges();
                }

                Program.MessageBoxAfter("削除し、再検索を行いました。");
                btnSearch_Click(sender, e);
            }
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
                // キャンセルの場合は処理を抜ける
                return;
            }

            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.Default);
            string line;
            try
            {
                // 既に表示されているデータをクリアする
                listView.Items.Clear();

                if ((line = reader.ReadLine()) != null)
                {
                    // ヘッダ行は読み飛ばす
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] lineItems = line.Split(',');
                        if (lineItems.Length >= 3)
                        {
                            ListViewItem item = new ListViewItem(lineItems[0].Replace("\"", string.Empty));
                            item.SubItems.Add(lineItems[1].Replace("\"", string.Empty));
                            item.SubItems.Add(lineItems[2].Replace("\"", string.Empty));
                            listView.Items.Add(item);
                        }
                    }
                    recordCnt.Text = listView.Items.Count.ToString();
                }
            }
            finally
            {
                reader.Close();
            }
        }

        /*************************************************************
         * CSVファイル登録ボタン押下時の処理
         *************************************************************/
        private void btnFileReg_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(labelFilePath.Text))
            {
                Program.MessageBoxError("CSVファイルを選択してください。");
                return;

            }

            string productKbn = getProductKbn(groupKbn2);
            if (string.IsNullOrEmpty(productKbn))
            {
                Program.MessageBoxError("商品区分を選択してください。");
                return;
            }

            if (Program.MessageBoxBefore(productKbn + "として登録しますか？" + Environment.NewLine
                + "※現在登録されている" + productKbn + "データは削除されます※") == DialogResult.Yes)
            {
                using (var context = new CostAccountingEntities())
                {
                    // データ削除処理
                    var target = from t in context.ProductCode
                                 where t.year.Equals(Const.TARGET_YEAR)
                                    && t.kbn.Equals(productKbn)
                                 select t;

                    context.ProductCode.RemoveRange(target);

                    // データ登録処理
                    foreach (ListViewItem items in listView.Items)
                    {
                        // 登録データのオブジェクトを作成
                        var entity = new ProductCode()
                        {
                            code = items.SubItems[0].Text,
                            year = Const.TARGET_YEAR,
                            name = items.SubItems[1].Text,
                            unit = items.SubItems[2].Text,
                            kbn = productKbn,
                            note = string.Empty,
                            update_user = SystemInformation.UserName,
                            update_date = DateTime.Now,
                            del_flg = Const.FLG_OFF
                        };
                        context.ProductCode.Add(entity);
                    }
                    context.SaveChanges();
                }
                Program.MessageBoxAfter("登録し、再検索を行いました。");
                btnSearch_Click(sender, e);
            }
        }

        /*************************************************************
         * CSVリストビューのヘッダ描画はデフォルトのまま
         *************************************************************/
        private void listView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        /*************************************************************
         * CSVリストビューを縞模様に描画
         *************************************************************/
        private void listView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // 奇数行の場合は背景色を変更し、縞々に見えるようにする
            if (e.ItemIndex % 2 > 0)
            {
                e.Graphics.FillRectangle(Brushes.Azure, e.Bounds);
            }
            // テキストを忘れずに描画する
            e.DrawText();
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
