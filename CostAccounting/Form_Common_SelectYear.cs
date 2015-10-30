using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace CostAccounting
{
    public partial class Form_Common_SelectYear : Form
    {
        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_Common_SelectYear()
        {
            InitializeComponent();
        }

        /*************************************************************
         * 決定ボタン押下時の処理
         *************************************************************/
        private void btnEnter_Click(object sender, EventArgs e)
        {
            targetYear.BackColor = Color.White;

            if (String.IsNullOrEmpty(targetYear.Text))
            {
                targetYear.BackColor = Color.Red;
                Program.MessageBoxError("対象年度を入力してください。");
                return;
            }

            if (!(Validation.IsNumeric(targetYear.Text) && targetYear.Text.Length == 4))
            {
                targetYear.BackColor = Color.Red;
                Program.MessageBoxError("対象年度は数値4桁で入力してください。");
                return;
            }

            Const.TARGET_YEAR = int.Parse(targetYear.Text);
            Form_Common_Menu form = new Form_Common_Menu();
            form.ShowDialog();
            form.Dispose();
        }

        /*************************************************************
         * テキストボックスにて数値のみ入力可能にする
         *************************************************************/
        private void textBox_KeyPress_numeric(object sender, KeyPressEventArgs e)
        {
            Event.textBox_KeyPress_numeric(sender, e);
        }

        /*************************************************************
         * フォーム終了時の処理
         *************************************************************/
        private void Form_Common_SelectYear_FormClosing(object sender, FormClosingEventArgs e)
        {
            // DBファイルのバキューム処理
            string dbPath = System.Configuration.ConfigurationManager.AppSettings["dbPath"];
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "vacuum;";
                    command.ExecuteNonQuery();
                }
            }

            // DBサイズの記録
            using (var context = new CostAccountingEntities())
            {
                // 登録データのオブジェクトを作成
                DateTime dt = DateTime.Now;
                var entity = new DbSize()
                {
                    year = dt.Year,
                    month = dt.Month,
                    daytime = dt.ToString("ddHHmmss"),
                    size = new FileInfo(dbPath).Length / 1024,  // KB単位でDBファイルのサイズを記録
                    update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName),
                    update_date = DateTime.Now
                };

                // データ登録
                context.DbSize.Add(entity);
                context.SaveChanges();
            }

            Logger.Info(Message.INF002);
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_Common_SelectYear_Load(object sender, EventArgs e)
        {
            Logger.Info(Message.INF001);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form_Common_Maintenance form = new Form_Common_Maintenance();
            form.ShowDialog();
            form.Dispose();
        }
    }
}
