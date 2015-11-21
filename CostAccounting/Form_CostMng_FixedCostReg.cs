using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace CostAccounting
{
    public partial class Form_CostMng_FixedCostReg : Form
    {
        // 検索タイプ
        private Const.CATEGORY_TYPE category;

        private Dictionary<int, TextBox[]> monthPairText;
        private Dictionary<int, Label> monthPairTotal;

        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_CostMng_FixedCostReg(Const.CATEGORY_TYPE category)
        {
            InitializeComponent();
            this.category = category;

            switch (category)
            {
                case Const.CATEGORY_TYPE.Budget:
                    this.Text = "固定費登録(予定)";
                    break;
                case Const.CATEGORY_TYPE.Actual:
                    this.Text = "固定費登録(実績)";
                    break;
            }

            monthPairText = new Dictionary<int, TextBox[]>() 
            { 
                { 4 , new TextBox[] { manufacturing_personnel_Apr, manufacturing_depreciation_Apr, manufacturing_rent_Apr, manufacturing_repair_Apr, manufacturing_stock_Apr, manufacturing_other_Apr, selling_personnel_Apr, selling_depreciation_Apr, selling_other_Apr, operating_expenses_Apr } }
              , { 5 , new TextBox[] { manufacturing_personnel_May, manufacturing_depreciation_May, manufacturing_rent_May, manufacturing_repair_May, manufacturing_stock_May, manufacturing_other_May, selling_personnel_May, selling_depreciation_May, selling_other_May, operating_expenses_May } }
              , { 6 , new TextBox[] { manufacturing_personnel_Jun, manufacturing_depreciation_Jun, manufacturing_rent_Jun, manufacturing_repair_Jun, manufacturing_stock_Jun, manufacturing_other_Jun, selling_personnel_Jun, selling_depreciation_Jun, selling_other_Jun, operating_expenses_Jun } }
              , { 7 , new TextBox[] { manufacturing_personnel_Jul, manufacturing_depreciation_Jul, manufacturing_rent_Jul, manufacturing_repair_Jul, manufacturing_stock_Jul, manufacturing_other_Jul, selling_personnel_Jul, selling_depreciation_Jul, selling_other_Jul, operating_expenses_Jul } }
              , { 8 , new TextBox[] { manufacturing_personnel_Aug, manufacturing_depreciation_Aug, manufacturing_rent_Aug, manufacturing_repair_Aug, manufacturing_stock_Aug, manufacturing_other_Aug, selling_personnel_Aug, selling_depreciation_Aug, selling_other_Aug, operating_expenses_Aug } }
              , { 9 , new TextBox[] { manufacturing_personnel_Sep, manufacturing_depreciation_Sep, manufacturing_rent_Sep, manufacturing_repair_Sep, manufacturing_stock_Sep, manufacturing_other_Sep, selling_personnel_Sep, selling_depreciation_Sep, selling_other_Sep, operating_expenses_Sep } }
              , { 10, new TextBox[] { manufacturing_personnel_Oct, manufacturing_depreciation_Oct, manufacturing_rent_Oct, manufacturing_repair_Oct, manufacturing_stock_Oct, manufacturing_other_Oct, selling_personnel_Oct, selling_depreciation_Oct, selling_other_Oct, operating_expenses_Oct } }
              , { 11, new TextBox[] { manufacturing_personnel_Nov, manufacturing_depreciation_Nov, manufacturing_rent_Nov, manufacturing_repair_Nov, manufacturing_stock_Nov, manufacturing_other_Nov, selling_personnel_Nov, selling_depreciation_Nov, selling_other_Nov, operating_expenses_Nov } }
              , { 12, new TextBox[] { manufacturing_personnel_Dec, manufacturing_depreciation_Dec, manufacturing_rent_Dec, manufacturing_repair_Dec, manufacturing_stock_Dec, manufacturing_other_Dec, selling_personnel_Dec, selling_depreciation_Dec, selling_other_Dec, operating_expenses_Dec } }
              , { 1 , new TextBox[] { manufacturing_personnel_Jan, manufacturing_depreciation_Jan, manufacturing_rent_Jan, manufacturing_repair_Jan, manufacturing_stock_Jan, manufacturing_other_Jan, selling_personnel_Jan, selling_depreciation_Jan, selling_other_Jan, operating_expenses_Jan } }
              , { 2 , new TextBox[] { manufacturing_personnel_Feb, manufacturing_depreciation_Feb, manufacturing_rent_Feb, manufacturing_repair_Feb, manufacturing_stock_Feb, manufacturing_other_Feb, selling_personnel_Feb, selling_depreciation_Feb, selling_other_Feb, operating_expenses_Feb } }
              , { 3 , new TextBox[] { manufacturing_personnel_Mar, manufacturing_depreciation_Mar, manufacturing_rent_Mar, manufacturing_repair_Mar, manufacturing_stock_Mar, manufacturing_other_Mar, selling_personnel_Mar, selling_depreciation_Mar, selling_other_Mar, operating_expenses_Mar } } 
            };

            monthPairTotal = new Dictionary<int, Label>() 
            { 
                { 4 , total_Apr }
              , { 5 , total_May }
              , { 6 , total_Jun }
              , { 7 , total_Jul }
              , { 8 , total_Aug }
              , { 9 , total_Sep }
              , { 10, total_Oct }
              , { 11, total_Nov }
              , { 12, total_Dec }
              , { 1 , total_Jan }
              , { 2 , total_Feb }
              , { 3 , total_Mar } 
            };
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_CostMng_Divergence_Load(object sender, EventArgs e)
        {
            // 固定費データを設定する
            setData();

            // 計算を行う
            calcAll();
        }

        /*************************************************************
         * テキストボックスにて数値のみ入力可能にする
         *************************************************************/
        private void textBox_KeyPress_numeric(object sender, KeyPressEventArgs e)
        {
            Event.textBox_KeyPress_numeric(sender, e);
        }

        /*************************************************************
         * データグリッドビューのテキストのロストフォーカス時に計算する
         *************************************************************/
        private void textBox_Leave(object sender, EventArgs e)
        {
            Event.textBox_Leave_formatNum(sender, e);
            calcAll();
        }

        /*************************************************************
         * 本画面のすべての計算を行う
         *************************************************************/
        private void calcAll()
        {
            decimal manufacturing_personnel = decimal.Zero;
            decimal manufacturing_depreciation = decimal.Zero;
            decimal manufacturing_rent = decimal.Zero;
            decimal manufacturing_repair = decimal.Zero;
            decimal manufacturing_stock = decimal.Zero;
            decimal manufacturing_other = decimal.Zero;
            decimal selling_personnel = decimal.Zero;
            decimal selling_depreciation = decimal.Zero;
            decimal selling_other = decimal.Zero;
            decimal operating_expenses = decimal.Zero;
            decimal all_total = decimal.Zero;

            foreach (int targetKey in monthPairText.Keys)
            {
                // 各月の合計値を算出
                decimal monthTotal = decimal.Zero;

                foreach (TextBox text in monthPairText[targetKey])
                    monthTotal += Conversion.Parse(text.Text);

                monthPairTotal[targetKey].Text = monthTotal.ToString("#,0");
                all_total += monthTotal;

                // 項目毎の合計値を算出
                manufacturing_personnel += Conversion.Parse((string)monthPairText[targetKey][0].Text);
                manufacturing_depreciation += Conversion.Parse((string)monthPairText[targetKey][1].Text);
                manufacturing_rent += Conversion.Parse((string)monthPairText[targetKey][2].Text);
                manufacturing_repair += Conversion.Parse((string)monthPairText[targetKey][3].Text);
                manufacturing_stock += Conversion.Parse((string)monthPairText[targetKey][4].Text);
                manufacturing_other += Conversion.Parse((string)monthPairText[targetKey][5].Text);
                selling_personnel += Conversion.Parse((string)monthPairText[targetKey][6].Text);
                selling_depreciation += Conversion.Parse((string)monthPairText[targetKey][7].Text);
                selling_other += Conversion.Parse((string)monthPairText[targetKey][8].Text);
                operating_expenses += Conversion.Parse((string)monthPairText[targetKey][9].Text);
            }

            manufacturing_personnel_total.Text = manufacturing_personnel.ToString("#,0");
            manufacturing_depreciation_total.Text = manufacturing_depreciation.ToString("#,0");
            manufacturing_rent_total.Text = manufacturing_rent.ToString("#,0");
            manufacturing_repair_total.Text = manufacturing_repair.ToString("#,0");
            manufacturing_stock_total.Text = manufacturing_stock.ToString("#,0");
            manufacturing_other_total.Text = manufacturing_other.ToString("#,0");
            selling_personnel_total.Text = selling_personnel.ToString("#,0");
            selling_depreciation_total.Text = selling_depreciation.ToString("#,0");
            selling_other_total.Text = selling_other.ToString("#,0");
            operating_expenses_total.Text = operating_expenses.ToString("#,0");
            total.Text = all_total.ToString("#,0");
        }

        /*************************************************************
         * 固定費データを設定する
         *************************************************************/
        private void setData()
        {
            using (var context = new CostAccountingEntities())
            {
                foreach (int month in monthPairText.Keys)
                {
                    var target = from t in context.CostMngTotal
                                 where t.year.Equals(Const.TARGET_YEAR)
                                    && t.month.Equals(month)
                                    && t.category.Equals((int)category)
                                    && t.del_flg.Equals(Const.FLG_OFF)
                                 select t;

                    if (target.Count() == decimal.One)
                    {
                        monthPairText[month][0].Text = target.First().manufacturing_personnel.ToString("#,0");
                        monthPairText[month][1].Text = target.First().manufacturing_depreciation.ToString("#,0");
                        monthPairText[month][2].Text = target.First().manufacturing_rent.ToString("#,0");
                        monthPairText[month][3].Text = target.First().manufacturing_repair.ToString("#,0");
                        monthPairText[month][4].Text = target.First().manufacturing_stock.ToString("#,0");
                        monthPairText[month][5].Text = target.First().manufacturing_other.ToString("#,0");
                        monthPairText[month][6].Text = target.First().selling_personnel.ToString("#,0");
                        monthPairText[month][7].Text = target.First().selling_depreciation.ToString("#,0");
                        monthPairText[month][8].Text = target.First().selling_other.ToString("#,0");
                        monthPairText[month][9].Text = target.First().operating_expenses.ToString("#,0");
                    }
                    else
                    {
                        foreach (TextBox textbox in monthPairText[month])
                            textbox.Text = decimal.Zero.ToString("#,0");
                    }
                }
            }
        }

        /*************************************************************
         * 登録ボタン押下時の処理
         *************************************************************/
        private void btnAppend_Click(object sender, EventArgs e)
        {
            if (Program.MessageBoxBefore("登録しますか？") != DialogResult.Yes)
                return;

            using (var context = new CostAccountingEntities())
            {
                foreach (int month in monthPairText.Keys)
                {
                    var target = from t in context.CostMngTotal
                                 where t.year.Equals(Const.TARGET_YEAR)
                                    && t.month.Equals(month)
                                    && t.category.Equals((int)category)
                                    && t.del_flg.Equals(Const.FLG_OFF)
                                 select t;

                    if (target.Count() == decimal.Zero)
                    {
                        // 登録処理
                        var entity = new CostMngTotal()
                        {
                            year = Const.TARGET_YEAR,
                            month = month,
                            category = (int)category,
                            manufacturing_personnel = Conversion.Parse(monthPairText[month][0].Text),
                            manufacturing_depreciation = Conversion.Parse(monthPairText[month][1].Text),
                            manufacturing_rent = Conversion.Parse(monthPairText[month][2].Text),
                            manufacturing_repair = Conversion.Parse(monthPairText[month][3].Text),
                            manufacturing_stock = Conversion.Parse(monthPairText[month][4].Text),
                            manufacturing_other = Conversion.Parse(monthPairText[month][5].Text),
                            selling_personnel = Conversion.Parse(monthPairText[month][6].Text),
                            selling_depreciation = Conversion.Parse(monthPairText[month][7].Text),
                            selling_other = Conversion.Parse(monthPairText[month][8].Text),
                            operating_expenses = Conversion.Parse(monthPairText[month][9].Text),
                            update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName),
                            update_date = DateTime.Now,
                            del_flg = Const.FLG_OFF
                        };
                        context.CostMngTotal.Add(entity);
                    }
                    else
                    {
                        // 修正処理
                        target.First().manufacturing_personnel = Conversion.Parse(monthPairText[month][0].Text);
                        target.First().manufacturing_depreciation = Conversion.Parse(monthPairText[month][1].Text);
                        target.First().manufacturing_rent = Conversion.Parse(monthPairText[month][2].Text);
                        target.First().manufacturing_repair = Conversion.Parse(monthPairText[month][3].Text);
                        target.First().manufacturing_stock = Conversion.Parse(monthPairText[month][4].Text);
                        target.First().manufacturing_other = Conversion.Parse(monthPairText[month][5].Text);
                        target.First().selling_personnel = Conversion.Parse(monthPairText[month][6].Text);
                        target.First().selling_depreciation = Conversion.Parse(monthPairText[month][7].Text);
                        target.First().selling_other = Conversion.Parse(monthPairText[month][8].Text);
                        target.First().operating_expenses = Conversion.Parse(monthPairText[month][9].Text);
                        target.First().update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                        target.First().update_date = DateTime.Now;
                    }
                }
                context.SaveChanges();
            }

            Logger.Info(Message.INF003, new string[] { this.Text, "-" });
            Program.MessageBoxAfter("登録しました。");
        }
    }
}
