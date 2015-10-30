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
using System.IO.Compression;

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
    }
}
