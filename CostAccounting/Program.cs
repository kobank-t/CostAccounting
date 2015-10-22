﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form_Common_SelectYear());
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
        public static DataTableSupport budget = new DataTableSupport(Const.CATEGORY_TYPE.Budget);
        public static DataTableSupport actual = new DataTableSupport(Const.CATEGORY_TYPE.Actual);

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

        ///// -----------------------------------------------------------------------------
        ///// <summary>
        /////     包装資材のデータテーブルの指定キーの価格を返却します。</summary>
        ///// <param name="materialTable">
        /////     検査対象と包装資材データテーブル。<param>
        ///// <param name="searchKey">
        /////     検索キー。<param>
        ///// <returns>
        /////     キーに該当する価格。</returns>
        ///// -----------------------------------------------------------------------------
        //public static decimal getPackingPrice(DataTable packingTable, string searchKey)
        //{
        //    decimal ret = decimal.Zero;

        //    foreach (DataRow data in packingTable.Rows)
        //    {
        //        if (searchKey.Equals(data["code"].ToString()))
        //        {
        //            ret = (decimal)data["price"];
        //        }
        //    }
        //    return ret;
        //}

        ///// -----------------------------------------------------------------------------
        ///// <summary>
        /////     設備のデータテーブルの指定キーのレートを返却します。</summary>
        ///// <param name="machineTable">
        /////     検査対象と設備データテーブル。<param>
        ///// <param name="searchKey">
        /////     検索キー。<param>
        ///// <returns>
        /////     キーに該当するレート。</returns>
        ///// -----------------------------------------------------------------------------
        //public static decimal getMachineRate(DataTable machineTable, string searchKey)
        //{
        //    decimal ret = decimal.Zero;

        //    foreach (DataRow data in machineTable.Rows)
        //    {
        //        if (searchKey.Equals(data["code"].ToString()))
        //        {
        //            ret = (decimal)data["price"];
        //        }
        //    }
        //    return ret;
        //}

        ///// -----------------------------------------------------------------------------
        ///// <summary>
        /////     荷造運賃のデータテーブルの指定キーの価格を返却します。</summary>
        ///// <param name="machineTable">
        /////     検査対象と荷造運賃データテーブル。<param>
        ///// <param name="searchKey">
        /////     検索キー。<param>
        ///// <returns>
        /////     キーに該当する価格。</returns>
        ///// -----------------------------------------------------------------------------
        //public static decimal getPackingFarePrice(DataTable packingFareTable, string searchKey)
        //{
        //    decimal ret = decimal.Zero;

        //    foreach (DataRow data in packingFareTable.Rows)
        //    {
        //        if (searchKey.Equals(data["code"].ToString()))
        //        {
        //            ret = (decimal)data["price"];
        //        }
        //    }
        //    return ret;
        //}
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
        public enum SEARCH_TYPE { Product, Supplier }

        // ステータスのタイプ
        public enum STATUS_TYPE { Default, Error }

        // 処理のタイプ
        public enum CATEGORY_TYPE { Budget, Actual }
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

        public static Parameters plan = new Parameters(Const.CATEGORY_TYPE.Budget);
        public static Parameters actual = new Parameters(Const.CATEGORY_TYPE.Actual);


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
    }
}
