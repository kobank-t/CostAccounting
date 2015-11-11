using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace CostAccounting
{
    public partial class Form_CostMng_ActualReg_child : Form
    {
        private string filePath;
        private decimal targetMonth;
        private Const.STATUS_TYPE type;
        private string clipboardStr;

        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_CostMng_ActualReg_child(string filePath, decimal targetMonth)
        {
            InitializeComponent();
            this.filePath = filePath;
            this.targetMonth = targetMonth;
        }

        /*************************************************************
         * 処理タイプの設定
         *************************************************************/
        public void setType(Const.STATUS_TYPE type)
        {
            this.type = type;

            switch (type)
            {
                case Const.STATUS_TYPE.Default:
                    this.Text = "実績登録（CSVファイル登録内容）";
                    description.Text = string.Concat("以下の内容で"
                        , targetMonth
                        , "月分に反映します。よろしければOKボタンを、キャンセルする場合は右上の「×」ボタンをクリックしてください。");
                    btnDecide.Visible = true;
                    btnClipboard.Visible = false;
                    break;
                case Const.STATUS_TYPE.Error:
                    this.Text = "実績登録（CSVファイル対応エラー内容）";
                    description.Text = "以下のデータが対応しませんでした。内容を確認してください。";
                    btnDecide.Visible = false;
                    btnClipboard.Visible = true;
                    break;
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
         * フォームロード時の処理
         *************************************************************/
        private void Form_CostMng_ActualReg_child_Load(object sender, EventArgs e)
        {
            // 対応しなかったエラー行のみを表示する
            if (type.Equals(Const.STATUS_TYPE.Error))
            {
                foreach (ListViewItem item in listView.Items)
                {
                    if (item.Tag.Equals(Const.FLG_ON))
                    {
                        listView.Items.Remove(item);
                    }
                    else
                    {
                        foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                            clipboardStr += string.Concat(subItem.Text, "\t");
                        clipboardStr = clipboardStr.TrimEnd('\t');
                        clipboardStr += Environment.NewLine;
                    }
                }
                recordCnt.Text = listView.Items.Count.ToString();
                return;
            }

            // 通常処理、指定されたCSVファイルの内容を表示する
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.Default);
            string line;
            try
            {
                // 既に表示されているデータをクリアする
                listView.Items.Clear();

                // ヘッダ行は読み飛ばす
                while ((line = reader.ReadLine()) != null)
                {
                    string[] lineItems = line.Split(',');

                    if (isTargetLine(ref lineItems))
                    {
                        ListViewItem item = new ListViewItem(lineItems[1]);
                        item.SubItems.Add(lineItems[2]);
                        item.SubItems.Add(lineItems[3]);
                        item.SubItems.Add(lineItems[4]);
                        item.SubItems.Add(lineItems[5]);
                        item.SubItems.Add(lineItems[6]);
                        listView.Items.Add(item);
                    }
                }
                recordCnt.Text = listView.Items.Count.ToString("#,0");
            }
            finally
            {
                reader.Close();
            }
        }

        /*************************************************************
         * CSVの対象行がチェックする
         *************************************************************/
        private bool isTargetLine(ref string[] lineItems)
        {
            // 項目が7つではない場合は対象行ではない
            if (lineItems.Length != 7)
                return false;

            // 各項目の2重引用符を除去する
            for (int i = 0; i < lineItems.Length; i++)
                lineItems[i] = lineItems[i].Replace("\"", string.Empty);

            // CSVの先頭項目が空白かつ1項目が数値の場合は対象行
            if (string.IsNullOrEmpty(lineItems[0]) && Validation.IsNumeric(lineItems[1]))
                return true;
            else
                return false;

        }

        /*************************************************************
         * クリップボードにコピーボタン押下時の処理
         *************************************************************/
        private void btnClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(clipboardStr);

            string message = string.Concat("クリップボードにコピーしました。"
                , Environment.NewLine
                , "テキストやExcelに貼り付けの上、内容を確認することもできます。"
                , Environment.NewLine
                , "Excelに貼りつける際は、セルの書式を文字列にしてください。");

            Program.MessageBoxAfter(message);
        }
    }
}
