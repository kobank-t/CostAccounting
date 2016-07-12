using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using System.Threading;
using System.Data.Entity.Validation;
using System.Text.RegularExpressions;

namespace CostAccounting
{
    static class Program
    {

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ThreadExceptionイベント・ハンドラを登録する
            Application.ThreadException += new
              ThreadExceptionEventHandler(Application_ThreadException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form_Common_SelectYear());
        }

        // 未処理例外をキャッチするイベント・ハンドラ
        public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {

            Program.MessageBoxError(string.Concat("予期しないエラーが発生しました。"
                                               , Environment.NewLine
                                               , "お手数ですが、ログファイルの送付をお願いします。"
                                               , Environment.NewLine
                                               , "_(._.)_"));


            if (e.Exception is DbEntityValidationException)
            {
                DbEntityValidationException error = (DbEntityValidationException)e.Exception;
                foreach (var validationErrors in error.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Info(
                              "Class: {0}, Property: {1}, Error: {2}",
                              validationErrors.Entry.Entity.GetType().FullName,
                              validationError.PropertyName,
                              validationError.ErrorMessage);
                    }
                }
            }
            Logger.Error(Message.ERR001, e.Exception);
            Environment.Exit(-1);
        }

        /*************************************************************
         * 処理前に表示するメッセージボックス
         *************************************************************/
        public static DialogResult MessageBoxBefore(string text)
        {
            return MessageBox.Show(text, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /*************************************************************
         * 処理後に表示するメッセージボックス
         *************************************************************/
        public static void MessageBoxAfter(string text)
        {
            MessageBox.Show(text, "結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /*************************************************************
         * エラー時にに表示するメッセージボックス
         *************************************************************/
        public static void MessageBoxError(string text)
        {
            MessageBox.Show(text, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /*************************************************************
         * ラジオボタンのチェック状態が予算か実績かを判定する
         *************************************************************/
        public static Const.CATEGORY_TYPE judgeCategory(RadioButton budget, RadioButton actual)
        {
            if (budget.Checked)
                return Const.CATEGORY_TYPE.Budget;
            else if (actual.Checked)
                return Const.CATEGORY_TYPE.Actual;
            else
                throw new ArgumentException("予算または実績のラジオボタンはチェックしましょう。");
        }

        /*************************************************************
         * ラジオボタンのチェック状態が商品かブレンド品かを判定する
         *************************************************************/
        public static Const.PRODUCT_TYPE judgeProductType(RadioButton product, RadioButton blend)
        {
            if (product.Checked)
                return Const.PRODUCT_TYPE.Normal;
            else if (blend.Checked)
                return Const.PRODUCT_TYPE.Blend;
            else
                throw new ArgumentException("商品またはブレンドのラジオボタンはチェックしましょう。");
        }

        /*************************************************************
         * 指定されたファイルをエクセルで開きます
         *************************************************************/
        public static void openExcel(string filePath)
        {
            Program.MessageBoxAfter(
                    string.Concat("出力したExcelファイルを開きます。"
                                  , Environment.NewLine
                                  , filePath));

            //Processオブジェクトを作成する
            System.Diagnostics.Process p = new System.Diagnostics.Process();

            //起動する実行ファイルのパスを設定する
            p.StartInfo.FileName = "excel.exe";

            //コマンドライン引数を指定する
            p.StartInfo.Arguments = @"""" + filePath + @"""";

            //起動する。プロセスが起動した時はTrueを返す。
            //bool result = p.Start();
            p.Start();
        }
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     データテーブルの作成をサポートしたクラスです。
    /// </summary>
    /// -----------------------------------------------------------------------------
    public sealed class DataTableSupport
    {
        public static DataTable item = createItem();
        public DataTable rowMaterial;
        public DataTable material;
        public DataTable machine;
        public DataTable fare;
        private static DataTableSupport budget = new DataTableSupport(Const.CATEGORY_TYPE.Budget);
        private static DataTableSupport actual = new DataTableSupport(Const.CATEGORY_TYPE.Actual);

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     コンストラクタ</summary>
        /// <param name="category">
        ///     予算か実績の区分<param>
        /// <returns>
        ///     </returns>
        /// -----------------------------------------------------------------------------
        private DataTableSupport(Const.CATEGORY_TYPE category)
        {
            rowMaterial = createRowMaterial(category);
            material = createMaterial(category);
            machine = createMachine(category);
            fare = createFare(category);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     予算か実績のインスタンスを返却する</summary>
        /// <param name="oTarget">
        ///     予算か実績の区分<param>
        /// <returns>
        ///     インスタンス</returns>
        /// -----------------------------------------------------------------------------
        public static DataTableSupport getInstance(Const.CATEGORY_TYPE category)
        {
            DataTableSupport ret = null;

            switch (category)
            {
                case Const.CATEGORY_TYPE.Budget:
                    ret = budget;
                    break;
                case Const.CATEGORY_TYPE.Actual:
                    ret = actual;
                    break;
            }
            return ret;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     インスタンスをリフレッシュする</summary>
        /// <returns>
        ///     </returns>
        /// -----------------------------------------------------------------------------
        public static void refresh()
        {
            budget = new DataTableSupport(Const.CATEGORY_TYPE.Budget);
            actual = new DataTableSupport(Const.CATEGORY_TYPE.Actual);
            item = createItem();
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     種目コードのデータテーブルを返却します。</summary>
        /// <returns>
        ///     種目コードのデータテーブル</returns>
        /// -----------------------------------------------------------------------------
        private static DataTable createItem()
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("code", typeof(string)));
            table.Columns.Add(new DataColumn("name", typeof(string)));

            using (var context = new CostAccountingEntities())
            {
                var list = from t in context.Item
                           where t.year.Equals(Const.TARGET_YEAR)
                           orderby t.code
                           select new { t.code, t.name };

                foreach (var record in list)
                {
                    DataRow row = table.NewRow();
                    row["code"] = record.code;
                    row["name"] = record.name;
                    table.Rows.Add(row);
                }
                return table;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     原材料のデータテーブルを返却します。</summary>
        /// <returns>
        ///     原材料のデータテーブル</returns>
        /// -----------------------------------------------------------------------------
        private DataTable createRowMaterial(Const.CATEGORY_TYPE category)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("code", typeof(string)));
            table.Columns.Add(new DataColumn("name", typeof(string)));
            table.Columns.Add(new DataColumn("price", typeof(decimal)));

            using (var context = new CostAccountingEntities())
            {
                var list = from t in context.RowMaterial
                           where t.year.Equals(Const.TARGET_YEAR)
                           orderby t.code
                           select new { t.code, t.name, t.price_budget, t.price_actual };

                foreach (var record in list)
                {
                    DataRow row = table.NewRow();
                    row["code"] = record.code;
                    row["name"] = record.name;
                    row["price"] = Const.CATEGORY_TYPE.Budget.Equals(category) ? record.price_budget : record.price_actual;

                    table.Rows.Add(row);
                }
                return table;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     資材のデータテーブルを返却します。</summary>
        /// <returns>
        ///     資材のデータテーブル</returns>
        /// -----------------------------------------------------------------------------
        private DataTable createMaterial(Const.CATEGORY_TYPE category)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("code", typeof(string)));
            table.Columns.Add(new DataColumn("name", typeof(string)));
            table.Columns.Add(new DataColumn("price", typeof(decimal)));

            using (var context = new CostAccountingEntities())
            {
                var list = from t in context.Material
                           where t.year.Equals(Const.TARGET_YEAR)
                           orderby t.code
                           select new { t.code, t.name, t.price_budget, t.price_actual };

                foreach (var record in list)
                {
                    DataRow row = table.NewRow();
                    row["code"] = record.code;
                    row["name"] = record.name;
                    row["price"] = Const.CATEGORY_TYPE.Budget.Equals(category) ? record.price_budget : record.price_actual;

                    table.Rows.Add(row);
                }
                return table;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     設備のデータテーブルを返却します。</summary>
        /// <returns>
        ///     設備のデータテーブル</returns>
        /// -----------------------------------------------------------------------------
        private DataTable createMachine(Const.CATEGORY_TYPE category)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("code", typeof(string)));
            table.Columns.Add(new DataColumn("name", typeof(string)));
            table.Columns.Add(new DataColumn("price", typeof(decimal)));

            using (var context = new CostAccountingEntities())
            {
                var list = from t in context.Machine
                           where t.year.Equals(Const.TARGET_YEAR)
                           orderby t.code
                           select new { t.code, t.name, t.rate_budget, t.rate_actual };

                foreach (var record in list)
                {
                    DataRow row = table.NewRow();
                    row["code"] = record.code;
                    row["name"] = string.Concat(record.code, "　", record.name);
                    row["price"] = Const.CATEGORY_TYPE.Budget.Equals(category) ? record.rate_budget : record.rate_actual;

                    table.Rows.Add(row);
                }
                return table;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     配送会社のデータテーブルを返却します。</summary>
        /// <returns>
        ///     配送会社のデータテーブル</returns>
        /// -----------------------------------------------------------------------------
        private DataTable createFare(Const.CATEGORY_TYPE category)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("code", typeof(string)));
            table.Columns.Add(new DataColumn("name", typeof(string)));
            table.Columns.Add(new DataColumn("price", typeof(decimal)));

            using (var context = new CostAccountingEntities())
            {
                var list = from t in context.Fare
                           where t.year.Equals(Const.TARGET_YEAR)
                           orderby t.code
                           select new { t.code, t.name, t.price_budget, t.price_actual };

                foreach (var record in list)
                {
                    DataRow row = table.NewRow();
                    row["code"] = record.code;
                    row["name"] = record.name;
                    row["price"] = Const.CATEGORY_TYPE.Budget.Equals(category) ? record.price_budget : record.price_actual;

                    table.Rows.Add(row);
                }
                return table;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     データテーブルにキーが含まれるか判定します。</summary>
        /// <param name="targetTable">
        ///     検査対象とデータテーブル。<param>
        /// <param name="searchKey">
        ///     検索キー。<param>
        /// <returns>
        ///     判定結果</returns>
        /// -----------------------------------------------------------------------------
        public static bool containsKey(DataTable targetTable, params string[] searchKey)
        {
            bool ret = false;

            foreach (string key in searchKey)
            {
                ret = false;
                foreach (DataRow data in targetTable.Rows)
                {
                    if (key.Equals(data["code"].ToString()))
                    {
                        ret = true;
                        break;
                    }
                }

                if (!ret)
                {
                    break;
                }
            }

            return ret;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     データテーブルの指定キーの価格を返却します。</summary>
        /// <param name="materialTable">
        ///     検査対象とデータテーブル。<param>
        /// <param name="searchKey">
        ///     検索キー。<param>
        /// <returns>
        ///     キーに該当する価格。</returns>
        /// -----------------------------------------------------------------------------
        public static decimal getPrice(DataTable table, string searchKey)
        {
            decimal ret = decimal.Zero;

            foreach (DataRow data in table.Rows)
            {
                if (searchKey.Equals(data["code"].ToString()))
                {
                    ret = (decimal)data["price"];
                }
            }
            return ret;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     データテーブルの指定キーの名称を返却します。</summary>
        /// <param name="materialTable">
        ///     検査対象とデータテーブル。<param>
        /// <param name="searchKey">
        ///     検索キー。<param>
        /// <returns>
        ///     キーに該当する名称。</returns>
        /// -----------------------------------------------------------------------------
        public static string getName(DataTable table, string searchKey)
        {
            string ret = string.Empty;

            foreach (DataRow data in table.Rows)
            {
                if (searchKey.Equals(data["code"].ToString()))
                {
                    ret = (string)data["name"];
                }
            }
            return ret;
        }
    }


    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     共通変数を定義したクラスです。
    /// </summary>
    /// -----------------------------------------------------------------------------
    public sealed class Const
    {
        // 対象年度（年度入力画面で値を設定）
        public static int TARGET_YEAR = 9999;

        // フラグ＝ON
        public const string FLG_ON = "1";

        // フラグ＝OFF
        public const string FLG_OFF = "0";

        // データ検索のタイプ
        public enum SEARCH_TYPE { Product, Supplier, Material, Packing }

        // ステータスのタイプ
        public enum STATUS_TYPE { Default, Error }

        // 処理のタイプ
        public enum CATEGORY_TYPE { Budget, Actual }

        // 商品のタイプ
        public enum PRODUCT_TYPE { Normal, Blend }
    }


    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     型変換をサポートした静的クラスです。
    /// </summary>
    /// -----------------------------------------------------------------------------
    public sealed class Conversion
    {
        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     文字列を数値に変換します。</summary>
        /// <param name="stTarget">
        ///     検査対象となる文字列。<param>
        /// <returns>
        ///     変換できる場合は変換後の数値を返却し、変換できない場合は0を返却。</returns>
        /// -----------------------------------------------------------------------------
        public static decimal Parse(string stTarget)
        {
            decimal ret;

            if (Validation.IsNumeric(stTarget))
            {
                ret = decimal.Parse(stTarget);
            }
            else
            {
                ret = decimal.Zero;
            }

            return ret;
        }
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     イベント処理をサポートした静的クラスです。
    /// </summary>
    /// -----------------------------------------------------------------------------
    public sealed class Event
    {
        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     テキストボックスでのEnterキー押下を無効化します。</summary>
        /// <param name="sender">
        ///     イベント発生元のオブジェクト。<param>
        /// <param name="e">
        ///     キーイベント引数。<param>
        /// -----------------------------------------------------------------------------
        public static void textBox_KeyDown_noEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     テキストボックスにて数値のみ入力可能にします。</summary>
        /// <param name="sender">
        ///     イベント発生元のオブジェクト。<param>
        /// <param name="e">
        ///     キーイベント引数。<param>
        /// -----------------------------------------------------------------------------
        public static void textBox_KeyPress_numeric(object sender, KeyPressEventArgs e)
        {
            // 制御文字は入力可
            if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
                return;
            }

            // 数字(0-9)は入力可
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
                return;
            }

            // 小数点は１つだけ入力可
            if (e.KeyChar == '.')
            {
                TextBox target = sender as TextBox;
                if (target.Text.IndexOf('.') < 0)
                {
                    // 複数のピリオド入力はNG
                    e.Handled = false;
                    return;
                }
            }

            // 上記以外は入力不可
            e.Handled = true;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     テキストボックスにて数値とアルファベットのみ入力可能にします。</summary>
        /// <param name="sender">
        ///     イベント発生元のオブジェクト。<param>
        /// <param name="e">
        ///     キーイベント引数。<param>
        /// -----------------------------------------------------------------------------
        public static void textBox_KeyPress_alphanum(object sender, KeyPressEventArgs e)
        {
            // 制御文字は入力可
            if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
                return;
            }

            // 半角英数は入力可能
            if ((Regex.Match(e.KeyChar.ToString(), "^[a-zA-Z0-9]+$")).Success)
            {
                e.Handled = false;
                return;
            }

            // 上記以外は入力不可
            e.Handled = true;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     テキストボックスのロストフォーカス時にフォーマットします。</summary>
        /// <param name="sender">
        ///     イベント発生元のオブジェクト。<param>
        /// <param name="e">
        ///     イベント引数。<param>
        /// -----------------------------------------------------------------------------
        public static void textBox_Leave_format(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox target = (TextBox)sender;
                target.Text = Conversion.Parse(target.Text).ToString("N");
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     テキストボックスのロストフォーカス時にフォーマットします。</summary>
        /// <param name="sender">
        ///     イベント発生元のオブジェクト。<param>
        /// <param name="e">
        ///     イベント引数。<param>
        /// -----------------------------------------------------------------------------
        public static void textBox_Leave_formatNum(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox target = (TextBox)sender;
                target.Text = Conversion.Parse(target.Text).ToString("#,0");
            }
        }
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     検証・エラーチェックをサポートした静的クラスです。
    /// </summary>
    /// -----------------------------------------------------------------------------
    public sealed class Validation
    {

        #region　IsNumeric メソッド (+1)

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     文字列が数値であるかどうかを返します。</summary>
        /// <param name="stTarget">
        ///     検査対象となる文字列。<param>
        /// <returns>
        ///     指定した文字列が数値であれば true。それ以外は false。</returns>
        /// -----------------------------------------------------------------------------
        public static bool IsNumeric(string stTarget)
        {
            double dNullable;

            return double.TryParse(
                stTarget,
                System.Globalization.NumberStyles.Any,
                null,
                out dNullable
            );
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     オブジェクトが数値であるかどうかを返します。</summary>
        /// <param name="oTarget">
        ///     検査対象となるオブジェクト。<param>
        /// <returns>
        ///     指定したオブジェクトが数値であれば true。それ以外は false。</returns>
        /// -----------------------------------------------------------------------------
        public static bool IsNumeric(object oTarget)
        {
            return IsNumeric(oTarget.ToString());
        }

        #endregion

    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     事前準備－その他登録で登録した各種パラメータの取得をサポートした静的クラスです。
    /// </summary>
    /// -----------------------------------------------------------------------------
    public sealed class Parameters
    {
        public decimal wageM = decimal.Zero;
        public decimal wageF = decimal.Zero;
        public decimal wageIndirect = decimal.Zero;
        public decimal utilitiesFD = decimal.Zero;
        public decimal utilitiesAD = decimal.Zero;
        public decimal allocationFD = decimal.Zero;
        public decimal allocationAD = decimal.Zero;
        public decimal allocationLabor = decimal.Zero;
        public decimal allocationSale = decimal.Zero;
        public decimal allocationMng = decimal.Zero;
        public decimal allocationExt = decimal.Zero;
        public decimal rateExpend = decimal.Zero;
        public decimal rateLoss = decimal.Zero;
        public decimal trayNum = decimal.Zero;

        private static Parameters plan = new Parameters(Const.CATEGORY_TYPE.Budget);
        private static Parameters actual = new Parameters(Const.CATEGORY_TYPE.Actual);


        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     コンストラクタ</summary>
        /// <param name="category">
        ///     予算か実績の区分<param>
        /// <returns>
        ///     </returns>
        /// -----------------------------------------------------------------------------
        private Parameters(Const.CATEGORY_TYPE category)
        {
            // データの設定
            using (var context = new CostAccountingEntities())
            {
                var target = from t in context.Other
                             where t.year.Equals(Const.TARGET_YEAR)
                                && t.category.Equals((int)category)
                             select t;

                if (target.Count() == decimal.One)
                {
                    wageM = target.First().wage_m;
                    wageF = target.First().wage_f;
                    wageIndirect = target.First().wage_indirect;
                    utilitiesFD = target.First().utilities_fd;
                    utilitiesAD = target.First().utilities_ad;
                    allocationFD = target.First().allocation_fd;
                    allocationAD = target.First().allocation_ad;
                    allocationLabor = target.First().allocation_labor;
                    allocationSale = target.First().allocation_sale;
                    allocationMng = target.First().allocation_mng;
                    allocationExt = target.First().allocation_ext;
                    rateExpend = target.First().rate_expend;
                    rateLoss = target.First().rate_loss;
                    trayNum = target.First().tray_num;
                }
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     予算か実績のインスタンスを返却する</summary>
        /// <param name="oTarget">
        ///     予算か実績の区分<param>
        /// <returns>
        ///     インスタンス</returns>
        /// -----------------------------------------------------------------------------
        public static Parameters getInstance(Const.CATEGORY_TYPE category)
        {
            Parameters ret = null;

            switch (category)
            {
                case Const.CATEGORY_TYPE.Budget:
                    ret = plan;
                    break;
                case Const.CATEGORY_TYPE.Actual:
                    ret = actual;
                    break;
            }
            return ret;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     インスタンスをリフレッシュする</summary>
        /// <returns>
        ///     </returns>
        /// -----------------------------------------------------------------------------
        public static void refresh()
        {
            plan = new Parameters(Const.CATEGORY_TYPE.Budget);
            actual = new Parameters(Const.CATEGORY_TYPE.Actual);
        }
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///     オブジェクトコピーをサポートした静的クラスです。
    /// </summary>
    /// -----------------------------------------------------------------------------
    public static class DeepCopyHelper
    {
        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     指定されたオブジェクトのディープコピーを返却します。</summary>
        /// <param name="target">
        ///     コピー元オブジェクト<param>
        /// <returns>
        ///     コピーオブジェクト</returns>
        /// -----------------------------------------------------------------------------
        public static T DeepCopy<T>(T target)
        {
            T result;
            BinaryFormatter b = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();

            try
            {
                b.Serialize(mem, target);
                mem.Position = 0;
                result = (T)b.Deserialize(mem);
            }
            finally
            {
                mem.Close();
            }
            return result;
        }
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///    ログ出力をサポートした静的クラスです。
    /// </summary>
    /// -----------------------------------------------------------------------------
    /// 

    public static class Logger
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     ログファイルのパスを返却します。</summary>
        /// <returns>ログファイルのパス</returns>
        /// -----------------------------------------------------------------------------
        public static string filePath()
        {
            var rootLogger = ((Hierarchy)logger.Logger.Repository).Root;
            var appender = rootLogger.GetAppender("RollingFileAppender_Size") as FileAppender;

            return appender.File;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     インフォレベルのログを出力します。</summary>
        /// <param name="message">
        ///     ログメッセージ<param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        public static void Info(string message)
        {
            logger.Info(message);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     インフォレベルのログを出力します。</summary>
        /// <param name="message">
        ///     ログメッセージ<param>
        /// <param name="param">
        ///     置換パラメータ<param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        public static void Info(string message, params string[] param)
        {
            logger.InfoFormat(message, param);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     エラーレベルのログを出力します。</summary>
        /// <param name="message">
        ///     ログメッセージ<param>
        /// <param name="exception">
        ///     エラークラス<param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        public static void Error(string message, Exception exception)
        {
            logger.Error(message, exception);
        }
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///    メッセージの定数クラスです。
    /// </summary>
    /// -----------------------------------------------------------------------------
    public static class Message
    {
        public static string INF001 = "INF001：原価計算ソフトが起動しました。";
        public static string INF002 = "INF002：原価計算ソフトが終了しました。";
        public static string INF003 = "INF003：登録しました。画面名=[{0}]　内容=[{1}]";
        public static string INF004 = "INF004：修正しました。画面名=[{0}]　内容=[{1}]";
        public static string INF005 = "INF005：削除しました。画面名=[{0}]　内容=[{1}]";
        public static string INF006 = "INF006：出力しました。画面名=[{0}]　内容=[{1}]";
        public static string INF007 = "INF007：再計算しました。画面名=[{0}]　内容=[{1}]";

        public static string ERR001 = "ERR001：予期しないエラーが発生しました。";

        public static string create(params Control[] controls)
        {
            string ret = string.Empty;

            ret = string.Concat("targetYear=", Const.TARGET_YEAR, " ");
            foreach (Control control in controls)
                ret += string.Concat(control.Name, "=", control.Text, " ");

            return ret;
        }
    }
}
