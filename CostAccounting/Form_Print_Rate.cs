using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CostAccounting
{
    public partial class Form_Print_Rate : Form
    {
        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_Print_Rate()
        {
            InitializeComponent();
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_Print_Product_Load(object sender, EventArgs e)
        {
            // 出力フォルダのデフォルトはアプリケーションの実行フォルダを指定
            outputDir.Text = Application.StartupPath;
            folderBrowserDialog.SelectedPath = Application.StartupPath;
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

            // テンプレートのファイル
            var template = @"\" + Properties.Resources.template_rate;
            var templateFile = new FileInfo(string.Concat(System.Configuration.ConfigurationManager.AppSettings["templateFolder"], template));

            // 出力ファイル
            var outputFile = new FileInfo(string.Concat(Application.StartupPath
                                                        , @"\レート表"
                                                        , radioBudget.Checked ? "【予定】_" : "【実績】_"
                                                        , DateTime.Now.ToString("yyyyMMddHHmmss")
                                                        , ".xlsx"));

            using (var package = new ExcelPackage(outputFile, templateFile))
            {
                using (var context = new CostAccountingEntities())
                {
                    // 原料費の設定------------------------------------------------------------
                    ExcelWorksheet rowMaterialsSheet = package.Workbook.Worksheets["原材料"];

                    var rowMaterials = (from t in context.RowMaterial
                                        where t.year.Equals(Const.TARGET_YEAR)
                                        orderby t.code
                                        select t).ToList();

                    rowMaterialsSheet.InsertRow(2, rowMaterials.Count());
                    for (int i = 0; i < rowMaterials.Count(); i++)
                    {
                        int row = i + 2;
                        rowMaterialsSheet.Cells[row, 1].Value = (i + 1);
                        rowMaterialsSheet.Cells[row, 2].Value = rowMaterials[i].code;
                        rowMaterialsSheet.Cells[row, 3].Value = rowMaterials[i].name;
                        rowMaterialsSheet.Cells[row, 4].Value = radioBudget.Checked ? rowMaterials[i].price_budget : rowMaterials[i].price_actual;
                    }

                    // 資材の設定--------------------------------------------------------------
                    ExcelWorksheet materialsSheet = package.Workbook.Worksheets["資材"];

                    var materials = (from t in context.Material
                                     where t.year.Equals(Const.TARGET_YEAR)
                                     orderby t.code
                                     select t).ToList();

                    materialsSheet.InsertRow(2, materials.Count());
                    for (int i = 0; i < materials.Count(); i++)
                    {
                        int row = i + 2;
                        materialsSheet.Cells[row, 1].Value = (i + 1);
                        materialsSheet.Cells[row, 2].Value = materials[i].code;
                        materialsSheet.Cells[row, 3].Value = materials[i].name;
                        materialsSheet.Cells[row, 4].Value = radioBudget.Checked ? materials[i].price_budget : materials[i].price_actual;
                    }

                    // マシンの設定------------------------------------------------------------
                    ExcelWorksheet machineSheet = package.Workbook.Worksheets["マシン"];

                    var machine = (from t in context.Machine
                                   where t.year.Equals(Const.TARGET_YEAR)
                                   orderby t.code
                                   select t).ToList();

                    machineSheet.InsertRow(2, machine.Count());
                    for (int i = 0; i < machine.Count(); i++)
                    {
                        int row = i + 2;
                        machineSheet.Cells[row, 1].Value = (i + 1);
                        machineSheet.Cells[row, 2].Value = machine[i].code;
                        machineSheet.Cells[row, 3].Value = machine[i].name;
                        machineSheet.Cells[row, 4].Value = radioBudget.Checked ? machine[i].rate_budget : machine[i].rate_actual;
                    }

                    // 運賃の設定--------------------------------------------------------------
                    ExcelWorksheet fareSheet = package.Workbook.Worksheets["運賃"];

                    var fare = (from t in context.Fare
                                where t.year.Equals(Const.TARGET_YEAR)
                                orderby t.code
                                select t).ToList();

                    fareSheet.InsertRow(2, fare.Count());
                    for (int i = 0; i < fare.Count(); i++)
                    {
                        int row = i + 2;
                        fareSheet.Cells[row, 1].Value = (i + 1);
                        fareSheet.Cells[row, 2].Value = fare[i].code;
                        fareSheet.Cells[row, 3].Value = fare[i].name;
                        fareSheet.Cells[row, 4].Value = radioBudget.Checked ? fare[i].price_budget : fare[i].price_actual;
                    }

                    // その他の設定------------------------------------------------------------
                    ExcelWorksheet otherSheet = package.Workbook.Worksheets["その他"];

                    Const.CATEGORY_TYPE category = Program.judgeCategory(radioBudget, radioActual);
                    otherSheet.Cells["C2"].Value = Parameters.getInstance(category).wageM;
                    otherSheet.Cells["C3"].Value = Parameters.getInstance(category).wageF;
                    otherSheet.Cells["C4"].Value = Parameters.getInstance(category).wageIndirect;
                    otherSheet.Cells["C5"].Value = Parameters.getInstance(category).utilitiesFD;
                    otherSheet.Cells["C6"].Value = Parameters.getInstance(category).utilitiesAD;
                    otherSheet.Cells["C7"].Value = Parameters.getInstance(category).allocationFD;
                    otherSheet.Cells["C8"].Value = Parameters.getInstance(category).allocationAD;
                    otherSheet.Cells["C9"].Value = Parameters.getInstance(category).allocationLabor;
                    otherSheet.Cells["C10"].Value = Parameters.getInstance(category).trayNum;
                    otherSheet.Cells["C11"].Value = Parameters.getInstance(category).allocationSale;
                    otherSheet.Cells["C12"].Value = Parameters.getInstance(category).allocationMng;
                    otherSheet.Cells["C13"].Value = Parameters.getInstance(category).allocationExt;
                    otherSheet.Cells["C14"].Value = Parameters.getInstance(category).rateExpend;
                    otherSheet.Cells["C15"].Value = Parameters.getInstance(category).rateLoss;

                }
                // Excelファイルを保存する
                package.Workbook.Worksheets.First().Select();
                package.Save();
            }

            Logger.Info(Message.INF006, new string[] { this.Text, Message.create(outputDir) + outputFile.Name });

            Program.MessageBoxAfter(
                    string.Concat("以下のExcelファイルを出力しました。出力先のフォルダを開きます。"
                                  , Environment.NewLine
                                  , outputFile.Name));

            System.Diagnostics.Process.Start(outputDir.Text);
        }
    }
}
