using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CostAccounting
{
    public partial class Form_Print_Product : Form
    {
        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_Print_Product()
        {
            InitializeComponent();
        }

        /*************************************************************
         * リストビューのヘッダ描画はデフォルトのまま
         *************************************************************/
        private void listView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        /*************************************************************
         * リストビューを縞模様に描画
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
        private void Form_Print_Product_Load(object sender, EventArgs e)
        {
            // 出力フォルダのデフォルトはアプリケーションの実行フォルダを指定
            outputDir.Text = Application.StartupPath;
            folderBrowserDialog.SelectedPath = Application.StartupPath;

            // 印刷対象データを表示する
            setTargetData();
        }

        /*************************************************************
         * リストビューに印刷対象データを表示する
         *************************************************************/
        private void setTargetData()
        {
            // 既に表示されているデータをクリアする
            listView.Items.Clear();

            // 出力条件を取得する
            Const.PRODUCT_TYPE productType = Program.judgeProductType(radioProduct, radioBlend);
            Const.CATEGORY_TYPE category = Program.judgeCategory(radioBudget, radioActual);

            // 出力条件に従い、印刷対象データを検索し、リストビューに設定する
            using (var context = new CostAccountingEntities())
            {
                var target = from t in context.ProductSupplier
                             join m_supplier in context.Supplier on new { t.year, code = t.supplier_code } equals new { m_supplier.year, m_supplier.code }
                             join m_product in context.ProductCode on new { t.year, code = t.product_code } equals new { m_product.year, m_product.code }
                             where t.year.Equals(Const.TARGET_YEAR)
                                && t.type.Equals((int)productType)
                                && t.category.Equals((int)category)
                             orderby t.supplier_code, t.product_code
                             select new { t.supplier_code, supplier_name = m_supplier.name, t.product_code, product_name = m_product.name };

                foreach (var data in target.ToList())
                {
                    ListViewItem item = new ListViewItem(data.supplier_code);
                    item.SubItems.Add(data.supplier_name);
                    item.SubItems.Add(data.product_code);
                    item.SubItems.Add(data.product_name);
                    listView.Items.Add(item);
                }

                recordCnt.Text = target.Count().ToString("#,0");
            }
        }

        /*************************************************************
        * ラジオボタンの状態に従い、印刷対象データの再検索を行う。
        *************************************************************/
        private void radio_CheckedChanged(object sender, EventArgs e)
        {
            // チェックされた方のイベントのみ処理を行う
            RadioButton radio = (RadioButton)sender;
            if (!radio.Checked)
                return;

            setTargetData();
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
         * 出力ボタン押下時の処理
         *************************************************************/
        private void btnOutput_Click(object sender, EventArgs e)
        {
            if (Program.MessageBoxBefore("出力条件の内容でExcelファイルを出力しますか？") != DialogResult.Yes)
                return;

            // 出力条件を取得
            Const.PRODUCT_TYPE productType = Program.judgeProductType(radioProduct, radioBlend);
            Const.CATEGORY_TYPE category = Program.judgeCategory(radioBudget, radioActual);

            // テンプレートのファイル
            var template = new FileInfo(string.Concat(System.Configuration.ConfigurationManager.AppSettings["templateFolder"], @"\product.xltx"));
            // 出力ファイル
            var outputFile = new FileInfo(string.Concat(Application.StartupPath, @"\商品帳票_", DateTime.Now.ToString("yyyyMMddHHmmss"), ".xlsx"));


            using (var package = new ExcelPackage(outputFile, template))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["template"];

                // セルを二次元配列で指定する場合1次元目がY方向、2次元目がX方向となります。
                worksheet.Cells[2, 2].Value = "テンプレートのサンプル";
                worksheet.Cells[2, 2].Style.Font.Color.SetColor(Color.Red);

                for (int i = 0; i < 14; i++)
                {
                    worksheet.Cells[4 + i, 2].Value = DateTime.Now.Hour;
                    worksheet.Cells[4 + i, 3].Value = DateTime.Now.Minute;
                    worksheet.Cells[4 + i, 4].Value = DateTime.Now.Second;
                    worksheet.Cells[4 + i, 5].Value = DateTime.Now.Millisecond;
                }

                // Excelファイルのセーブ
                package.Save();
            }

            Logger.Info(Message.INF006, new string[] { this.Text, Message.create(outputDir, recordCnt) + outputFile.Name });

            Program.MessageBoxAfter(
                    string.Concat("以下のExcelファイルを出力しました。出力先のフォルダを開きます。"
                                  , Environment.NewLine
                                  , outputFile.Name));

            System.Diagnostics.Process.Start(outputDir.Text);
        }

    }
}
