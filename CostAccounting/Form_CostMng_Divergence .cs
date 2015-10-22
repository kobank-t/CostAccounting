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
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_CostMng_Divergence_Load(object sender, EventArgs e)
        {
            // 原価計算実績データを設定する
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
         * 原価計算実績データを設定する
         *************************************************************/
        private void setData()
        {
            using (var context = new CostAccountingEntities())
            {
                var target = from t in context.Divergence
                             where t.year.Equals(Const.TARGET_YEAR)
                                && t.del_flg.Equals(Const.FLG_OFF)
                             select t;

                if (target.Count() == decimal.One)
                {
                    materialCost.Text = target.First().materialCost.ToString("N");
                    materialCost_db.Text = target.First().materialCost_costing.ToString("N");
                    laborCost.Text = target.First().laborCost.ToString("N");
                    laborCost_db.Text = target.First().laborCost_costing.ToString("N");
                    contractorsCost.Text = target.First().contractorsCost.ToString("N");
                    contractorsCost_db.Text = target.First().contractorsCost_costing.ToString("N");
                    materialsFare.Text = target.First().materialsFare.ToString("N");
                    materialsFare_db.Text = target.First().materialsFare_costing.ToString("N");
                    packingCost.Text = target.First().packingCost.ToString("N");
                    packingCost_db.Text = target.First().packingCost_costing.ToString("N");
                    utilitiesCost.Text = target.First().utilitiesCost.ToString("N");
                    utilitiesCost_db.Text = target.First().utilitiesCost_costing.ToString("N");
                    otherCost.Text = target.First().otherCost.ToString("N");
                    otherCost_db.Text = target.First().otherCost_costing.ToString("N");
                    packingFare.Text = target.First().packingFare.ToString("N");
                    packingFare_db.Text = target.First().packingFare_costing.ToString("N");
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

            // データ登録処理（既に同年のデータ存在する場合は更新）
            using (var context = new CostAccountingEntities())
            {
                var target = from t in context.Divergence
                             where t.year.Equals(Const.TARGET_YEAR)
                                && t.del_flg.Equals(Const.FLG_OFF)
                             select t;

                if (target.Count() == 0)
                {
                    // 登録処理
                    var entity = new Divergence()
                    {
                        year = Const.TARGET_YEAR,
                        materialCost = Conversion.Parse(materialCost.Text),
                        laborCost = Conversion.Parse(laborCost.Text),
                        contractorsCost = Conversion.Parse(contractorsCost.Text),
                        materialsFare = Conversion.Parse(materialsFare.Text),
                        packingCost = Conversion.Parse(packingCost.Text),
                        utilitiesCost = Conversion.Parse(utilitiesCost.Text),
                        otherCost = Conversion.Parse(otherCost.Text),
                        packingFare = Conversion.Parse(packingFare.Text),
                        update_user = SystemInformation.UserName,
                        update_date = DateTime.Now,
                        del_flg = Const.FLG_OFF
                    };
                    context.Divergence.Add(entity);
                }
                else
                {
                    // 更新処理
                    target.First().materialCost = Conversion.Parse(materialCost.Text);
                    target.First().laborCost = Conversion.Parse(laborCost.Text);
                    target.First().contractorsCost = Conversion.Parse(contractorsCost.Text);
                    target.First().materialsFare = Conversion.Parse(materialsFare.Text);
                    target.First().packingCost = Conversion.Parse(packingCost.Text);
                    target.First().utilitiesCost = Conversion.Parse(utilitiesCost.Text);
                    target.First().otherCost = Conversion.Parse(otherCost.Text);
                    target.First().packingFare = Conversion.Parse(packingFare.Text);
                    target.First().update_user = SystemInformation.UserName;
                    target.First().update_date = DateTime.Now;
                }
                context.SaveChanges();
            }
            Program.MessageBoxAfter("登録しました。");
        }
    }
}
