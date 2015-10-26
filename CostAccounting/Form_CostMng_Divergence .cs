using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace CostAccounting
{
    public partial class Form_CostMng_Divergence : Form
    {
        private Dictionary<TextBox, Label[]> textboxDic;
        private Dictionary<int, TextBox[]> xxxx;

        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_CostMng_Divergence()
        {
            InitializeComponent();
            textboxDic = new Dictionary<TextBox, Label[]>()
            { 
                { materialCost, new Label[] { materialCost_db, materialCost_diff, materialCost_rate } } 
                , { laborCost, new Label[] {laborCost_db,laborCost_diff,laborCost_rate } }
                , { contractorsCost, new Label[] { contractorsCost_db, contractorsCost_diff, contractorsCost_rate } }
                , { materialsFare, new Label[] { materialsFare_db, materialsFare_diff, materialsFare_rate } }
                , { packingCost, new Label[] { packingCost_db, packingCost_diff, packingCost_rate } }
                , { utilitiesCost, new Label[] { utilitiesCost_db, utilitiesCost_diff, utilitiesCost_rate } }
                , { otherCost, new Label[] { otherCost_db, otherCost_diff, otherCost_rate } }
                , { packingFare, new Label[] { packingFare_db, packingFare_diff, packingFare_rate } }
            };

            xxxx = new Dictionary<int, TextBox[]>() 
            { 
                { 4 , new TextBox[] { materialCost_Apr, laborCost_Apr, contractorsCost_Apr, materialsFare_Apr, packingCost_Apr, utilitiesCost_Apr, otherCost_Apr, packingFare_Apr } }
              , { 5 , new TextBox[] { materialCost_May, laborCost_May, contractorsCost_May, materialsFare_May, packingCost_May, utilitiesCost_May, otherCost_May, packingFare_May } }
              , { 6 , new TextBox[] { materialCost_Jun, laborCost_Jun, contractorsCost_Jun, materialsFare_Jun, packingCost_Jun, utilitiesCost_Jun, otherCost_Jun, packingFare_Jun } }
              , { 7 , new TextBox[] { materialCost_Jul, laborCost_Jul, contractorsCost_Jul, materialsFare_Jul, packingCost_Jul, utilitiesCost_Jul, otherCost_Jul, packingFare_Jul } }
              , { 8 , new TextBox[] { materialCost_Aug, laborCost_Aug, contractorsCost_Aug, materialsFare_Aug, packingCost_Aug, utilitiesCost_Aug, otherCost_Aug, packingFare_Aug } }
              , { 9 , new TextBox[] { materialCost_Sep, laborCost_Sep, contractorsCost_Sep, materialsFare_Sep, packingCost_Sep, utilitiesCost_Sep, otherCost_Sep, packingFare_Sep } }
              , { 10, new TextBox[] { materialCost_Oct, laborCost_Oct, contractorsCost_Oct, materialsFare_Oct, packingCost_Oct, utilitiesCost_Oct, otherCost_Oct, packingFare_Oct } }
              , { 11, new TextBox[] { materialCost_Nov, laborCost_Nov, contractorsCost_Nov, materialsFare_Nov, packingCost_Nov, utilitiesCost_Nov, otherCost_Nov, packingFare_Nov } }
              , { 12, new TextBox[] { materialCost_Dec, laborCost_Dec, contractorsCost_Dec, materialsFare_Dec, packingCost_Dec, utilitiesCost_Dec, otherCost_Dec, packingFare_Dec } }
              , { 1 , new TextBox[] { materialCost_Jan, laborCost_Jan, contractorsCost_Jan, materialsFare_Jan, packingCost_Jan, utilitiesCost_Jan, otherCost_Jan, packingFare_Jan } }
              , { 2 , new TextBox[] { materialCost_Feb, laborCost_Feb, contractorsCost_Feb, materialsFare_Feb, packingCost_Feb, utilitiesCost_Feb, otherCost_Feb, packingFare_Feb } }
              , { 3 , new TextBox[] { materialCost_Mar, laborCost_Mar, contractorsCost_Mar, materialsFare_Mar, packingCost_Mar, utilitiesCost_Mar, otherCost_Mar, packingFare_Mar } } 
            };
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_CostMng_Divergence_Load(object sender, EventArgs e)
        {
            // 決算書実績データを設定する
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
            TextBox target = (TextBox)sender;
            decimal targetVal = Conversion.Parse(target.Text);

            if (textboxDic.ContainsKey(target))
            {
                decimal db = Conversion.Parse(textboxDic[target][0].Text);
                decimal diff = decimal.Subtract(targetVal, db);
                decimal rate = db != decimal.Zero ? decimal.Divide(diff, db) : decimal.Zero;

                textboxDic[target][1].Text = diff.ToString("N");
                textboxDic[target][2].Text = rate.ToString("P");
            }
            target.Text = targetVal.ToString("N");
            calcTotal();
        }

        /*************************************************************
         * 合計値の計算を行う
         *************************************************************/
        private void calcTotal()
        {
            decimal total = decimal.Zero;
            foreach (TextBox textbox in textboxDic.Keys)
            {
                total += Conversion.Parse(textbox.Text);
            }

            decimal total_db = Conversion.Parse(this.total_db.Text);
            decimal total_diff = decimal.Subtract(total, total_db);
            decimal total_rate = total_db != decimal.Zero ? decimal.Divide(total_diff, total_db) : decimal.Zero;

            this.total.Text = total.ToString("N");
            this.total_diff.Text = total_diff.ToString("N");
            this.total_rate.Text = total_rate.ToString("P");
        }

        /*************************************************************
         * 本画面のすべての計算を行う
         *************************************************************/
        private void calcAll()
        {
            foreach (TextBox textbox in textboxDic.Keys)
            {
                textBox_Leave(textbox, null);
            }
            calcTotal();
        }

        /*************************************************************
         * 決算書実績データを設定する
         *************************************************************/
        private void setData()
        {
            using (var context = new CostAccountingEntities())
            {
                foreach (int month in xxxx.Keys)
                {
                    var target = from t in context.Divergence
                                 where t.year.Equals(Const.TARGET_YEAR)
                                    && t.month.Equals(month)
                                    && t.del_flg.Equals(Const.FLG_OFF)
                                 select t;

                    if (target.Count() == decimal.One)
                    {
                        xxxx[month][0].Text = target.First().materialCost.ToString("N");
                        xxxx[month][1].Text = target.First().laborCost.ToString("N");
                        xxxx[month][2].Text = target.First().contractorsCost.ToString("N");
                        xxxx[month][3].Text = target.First().materialsFare.ToString("N");
                        xxxx[month][4].Text = target.First().packingCost.ToString("N");
                        xxxx[month][5].Text = target.First().utilitiesCost.ToString("N");
                        xxxx[month][6].Text = target.First().otherCost.ToString("N");
                        xxxx[month][7].Text = target.First().packingFare.ToString("N");
                    }
                    else
                    {
                        foreach (TextBox textbox in xxxx[month])
                            textbox.Text = decimal.Zero.ToString("N");
                    }
                    //materialCost_db.Text = target.First().materialCost_costing.ToString("N");
                    //laborCost_db.Text = target.First().laborCost_costing.ToString("N");
                    //contractorsCost_db.Text = target.First().contractorsCost_costing.ToString("N");
                    //materialsFare_db.Text = target.First().materialsFare_costing.ToString("N");
                    //packingCost_db.Text = target.First().packingCost_costing.ToString("N");
                    //utilitiesCost_db.Text = target.First().utilitiesCost_costing.ToString("N");
                    //otherCost_db.Text = target.First().otherCost_costing.ToString("N");
                    //packingFare_db.Text = target.First().packingFare_costing.ToString("N");
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

            // データ登録処理（削除→登録をおこなう）
            using (var context = new CostAccountingEntities())
            {
                foreach (int month in xxxx.Keys)
                {
                    var target = from t in context.Divergence
                                 where t.year.Equals(Const.TARGET_YEAR)
                                    && t.month.Equals(month)
                                    && t.del_flg.Equals(Const.FLG_OFF)
                                 select t;

                    if (target.Count() == decimal.Zero)
                    {
                        // 登録処理
                        var entity = new Divergence()
                        {
                            year = Const.TARGET_YEAR,
                            month = month,
                            materialCost = Conversion.Parse(xxxx[month][0].Text),
                            laborCost = Conversion.Parse(xxxx[month][1].Text),
                            contractorsCost = Conversion.Parse(xxxx[month][2].Text),
                            materialsFare = Conversion.Parse(xxxx[month][3].Text),
                            packingCost = Conversion.Parse(xxxx[month][4].Text),
                            utilitiesCost = Conversion.Parse(xxxx[month][5].Text),
                            otherCost = Conversion.Parse(xxxx[month][6].Text),
                            packingFare = Conversion.Parse(xxxx[month][7].Text),
                            update_user = SystemInformation.UserName,
                            update_date = DateTime.Now,
                            del_flg = Const.FLG_OFF
                        };
                        context.Divergence.Add(entity);
                    }
                    else
                    {
                        // 修正処理
                        target.First().materialCost = Conversion.Parse(xxxx[month][0].Text);
                        target.First().laborCost = Conversion.Parse(xxxx[month][1].Text);
                        target.First().contractorsCost = Conversion.Parse(xxxx[month][2].Text);
                        target.First().materialsFare = Conversion.Parse(xxxx[month][3].Text);
                        target.First().packingCost = Conversion.Parse(xxxx[month][4].Text);
                        target.First().utilitiesCost = Conversion.Parse(xxxx[month][5].Text);
                        target.First().otherCost = Conversion.Parse(xxxx[month][6].Text);
                        target.First().packingFare = Conversion.Parse(xxxx[month][7].Text);
                        target.First().update_user = SystemInformation.UserName;
                        target.First().update_date = DateTime.Now;
                    }
                }
                context.SaveChanges();
            }
            Program.MessageBoxAfter("登録しました。");
        }
    }
}
