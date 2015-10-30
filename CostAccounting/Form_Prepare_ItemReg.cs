using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace CostAccounting
{
    public partial class Form_Prepare_ItemReg : Form
    {
        public Form_Prepare_ItemReg()
        {
            InitializeComponent();
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_Prepare_ItemReg_Load(object sender, EventArgs e)
        {
            using (var context = new CostAccountingEntities())
            {
                var list = from t in context.Item
                           where t.year.Equals(Const.TARGET_YEAR)
                              && t.del_flg.Equals(Const.FLG_OFF)
                           select new { t.code, t.name, t.note };

                dataGridView.DataSource = list.ToList();
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
                textRegNote.Text = (string)dataGridView.SelectedRows[0].Cells[2].Value;
            }
        }

        /*************************************************************
         * 登録ボタン押下時の処理
         *************************************************************/
        private void btnAppend_Click(object sender, EventArgs e)
        {
            if (Program.MessageBoxBefore("登録しますか？") == DialogResult.Yes)
            {
                // 登録データのオブジェクトを作成
                var entity = new Item()
                {
                    code = textRegCode.Text,
                    year = Const.TARGET_YEAR,
                    name = textRegName.Text,
                    note = textRegNote.Text,
                    update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName),
                    update_date = DateTime.Now,
                    del_flg = Const.FLG_OFF
                };

                // データ登録処理（既に同じコードが存在する場合は登録しない）
                using (var context = new CostAccountingEntities())
                {
                    var target = from t in context.Item
                                 where t.code.Equals(entity.code) && t.year.Equals(entity.year)
                                 select t;
                    if (target.Count() == 0)
                    {
                        context.Item.Add(entity);
                        context.SaveChanges();

                        Logger.Info(Message.INF003, new string[] { this.Text, Message.create(textRegCode, textRegName) });
                        Program.MessageBoxAfter("登録しました。");
                        Form_Prepare_ItemReg_Load(sender, e);
                    }
                    else
                    {
                        Program.MessageBoxError("既に同じコードの種目が登録されています。");
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
                    var target = from t in context.Item
                                 where t.year.Equals(Const.TARGET_YEAR)
                                    && t.code.Equals(textRegCode.Text)
                                 select t;

                    if (target.Count() > 0)
                    {
                        target.First().code = textRegCode.Text;
                        target.First().name = textRegName.Text;
                        target.First().note = textRegNote.Text;
                        target.First().update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                        target.First().update_date = DateTime.Now;
                    }
                    context.SaveChanges();
                }

                Logger.Info(Message.INF004, new string[] { this.Text, Message.create(textRegCode, textRegName) });
                Program.MessageBoxAfter("更新しました。");
                Form_Prepare_ItemReg_Load(sender, e);
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
                    var target = from t in context.Item
                                 where t.year.Equals(Const.TARGET_YEAR)
                                    && t.code.Equals(textRegCode.Text)
                                 select t;

                    if (target.Count() > 0)
                    {
                        //target.First().del_flg = Const.FLG_ON;
                        context.Item.Remove(target.First());
                    }
                    context.SaveChanges();
                }

                Logger.Info(Message.INF005, new string[] { this.Text, Message.create(textRegCode, textRegName) });
                Program.MessageBoxAfter("削除しました。");
                Form_Prepare_ItemReg_Load(sender, e);
            }
        }

        /*************************************************************
         * 初期データ登録ボタン押下時の処理
         *************************************************************/
        private void btnDefaultDataReg_Click(object sender, EventArgs e)
        {
            if (Program.MessageBoxBefore("初期データを登録しますか？" + Environment.NewLine + "※現在登録されているデータは削除されます※") == DialogResult.Yes)
            {
                using (var context = new CostAccountingEntities())
                {
                    // データ削除処理
                    var target = from t in context.Item
                                 where t.year.Equals(Const.TARGET_YEAR)
                                 select t;

                    context.Item.RemoveRange(target);

                    string itemData = Properties.Resources.defaultItemData;
                    string[] items = itemData.Split(',');

                    // データ登録処理
                    foreach (string item in items)
                    {
                        string[] data = item.Split('#');
                        var entity = new Item()
                        {
                            code = data[0],
                            year = Const.TARGET_YEAR,
                            name = data[1],
                            note = string.Empty,
                            update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName),
                            update_date = DateTime.Now,
                            del_flg = Const.FLG_OFF
                        };
                        context.Item.Add(entity);
                    }
                    context.SaveChanges();
                }
                Program.MessageBoxAfter("登録しました。");
                Form_Prepare_ItemReg_Load(sender, e);
            }
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
