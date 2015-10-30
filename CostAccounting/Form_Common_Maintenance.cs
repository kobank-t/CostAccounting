using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;

namespace CostAccounting
{
    public partial class Form_Common_Maintenance : Form
    {
        private string logPath = Logger.filePath();
        private string dbPath = System.Configuration.ConfigurationManager.AppSettings["dbPath"];

        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_Common_Maintenance()
        {
            InitializeComponent();
        }

        /*************************************************************
         * ログファイル取得ボタン押下時の処理
         *************************************************************/
        private void btnGetLogFile_Click(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(logPath);
            saveFileDialogLog.FileName = "log.zip";

            //ダイアログを表示する
            if (saveFileDialogLog.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo tempDir = Directory.CreateDirectory(file.DirectoryName + @"\temp");

                foreach (FileInfo srcFile in file.Directory.GetFiles())
                    srcFile.CopyTo(tempDir.FullName + @"\" + srcFile.Name, true);


                if (File.Exists(saveFileDialogLog.FileName))
                    File.Delete(saveFileDialogLog.FileName);

                ZipFile.CreateFromDirectory(tempDir.FullName, saveFileDialogLog.FileName);
                tempDir.Delete(true);

                Program.MessageBoxAfter(
                    string.Concat("ログファイルを保存しました。保存先のフォルダを開きます。"
                                  , Environment.NewLine
                                  , saveFileDialogLog.FileName));
                System.Diagnostics.Process.Start(new FileInfo(saveFileDialogLog.FileName).DirectoryName);
            }
        }

        /*************************************************************
         * DBファイル取得ボタン押下時の処理
         *************************************************************/
        private void btnGetDbFile_Click(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(dbPath);
            saveFileDialogDB.FileName = file.Name;

            //ダイアログを表示する
            if (saveFileDialogDB.ShowDialog() == DialogResult.OK)
            {
                file.CopyTo(saveFileDialogDB.FileName, true);
                Program.MessageBoxAfter(
                    string.Concat("DBファイルを保存しました。保存先のフォルダを開きます。"
                                  , Environment.NewLine
                                  , saveFileDialogDB.FileName));
                System.Diagnostics.Process.Start(new FileInfo(saveFileDialogDB.FileName).DirectoryName);
            }
        }


        // 取得データの履歴
        const int MAX_HISTORY = 40;
        Queue<int> countHistory = new Queue<int>();

        // CPU使用率の取得用カウンタ
        PerformanceCounter pc = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);

        private void Form_Common_Maintenance_Load(object sender, EventArgs e)
        {
            // チャートの表示を初期化
            initChart(chart1);

            // 1秒周期でチャートを再描画
            timer1.Interval = 1000;
            timer1.Enabled = true;
        }


        private void initChart(Chart chart)
        {
            // チャート全体の背景色を設定
            //chart.BackColor = Color.Black;
            chart.ChartAreas[0].BackColor = Color.Transparent;

            // チャート表示エリア周囲の余白をカットする
            chart.ChartAreas[0].InnerPlotPosition.Auto = false;
            chart.ChartAreas[0].InnerPlotPosition.Width = 100; // 100%
            chart.ChartAreas[0].InnerPlotPosition.Height = 90;  // 90%(横軸のメモリラベル印字分の余裕を設ける)
            chart.ChartAreas[0].InnerPlotPosition.X = 8;
            chart.ChartAreas[0].InnerPlotPosition.Y = 0;


            // X,Y軸情報のセット関数を定義
            Action<Axis> setAxis = (axisInfo) =>
            {
                // 軸のメモリラベルのフォントサイズ上限値を制限
                axisInfo.LabelAutoFitMaxFontSize = 8;

                // 軸のメモリラベルの文字色をセット
                axisInfo.LabelStyle.ForeColor = Color.White;

                // 軸タイトルの文字色をセット(今回はTitle未使用なので関係ないが...)
                axisInfo.TitleForeColor = Color.White;

                // 軸の色をセット
                axisInfo.MajorGrid.Enabled = true;
                axisInfo.MajorGrid.LineColor = ColorTranslator.FromHtml("#008242");
                axisInfo.MinorGrid.Enabled = false;
                axisInfo.MinorGrid.LineColor = ColorTranslator.FromHtml("#008242");
            };

            // X,Y軸の表示方法を定義
            setAxis(chart.ChartAreas[0].AxisY);
            setAxis(chart.ChartAreas[0].AxisX);
            chart.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart.ChartAreas[0].AxisY.Maximum = 100;    // 縦軸の最大値を100にする

            chart.AntiAliasing = AntiAliasingStyles.None;

            // 折れ線グラフとして表示
            chart.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            // 線の色を指定
            //chart.Series[0].Color = ColorTranslator.FromHtml("#00FF00");

            // 凡例を非表示,各値に数値を表示しない
            chart.Series[0].IsVisibleInLegend = false;
            chart.Series[0].IsValueShownAsLabel = false;

            // チャートに表示させる値の履歴を全て0クリア
            while (countHistory.Count <= MAX_HISTORY)
            {
                countHistory.Enqueue(0);
            }
        }

        //***************************************************************************
        /// <summary> チャートを描画する
        /// </summary>
        /// <param name="chart"></param>
        //***************************************************************************
        private void showChart(Chart chart)
        {

            //-----------------------
            // チャートに値をセット
            //-----------------------
            chart.Series[0].Points.Clear();
            foreach (int value in countHistory)
            {
                // データをチャートに追加
                chart.Series[0].Points.Add(new DataPoint(0, value));
            }
        }


        //***************************************************************************
        /// <summary>   タイマー処理
        ///             CPU負荷を一定周期で取得する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //***************************************************************************
        private void timer1_Tick(object sender, EventArgs e)
        {
            //---------------------------------
            // CPUの使用率を取得し、履歴に登録
            //---------------------------------
            int value = (int)pc.NextValue();
            countHistory.Enqueue(value);

            //------------------------------------------------
            // 履歴の最大数を超えていたら、古いものを削除する
            //------------------------------------------------
            while (countHistory.Count > MAX_HISTORY)
            {
                countHistory.Dequeue();
            }

            //------------------------------------------------
            // グラフを再描画する
            //------------------------------------------------
            showChart(chart1);
        }
    }
}
