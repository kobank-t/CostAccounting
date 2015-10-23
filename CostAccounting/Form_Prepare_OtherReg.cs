using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace CostAccounting
{
    public partial class Form_Prepare_OtherReg : Form
    {

        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_Prepare_OtherReg()
        {
            InitializeComponent();
        }

        /*************************************************************
         * テキストボックスにて数値のみ入力可能にする
         *************************************************************/
        private void textBox_KeyPress_numeric(object sender, KeyPressEventArgs e)
        {
            Event.textBox_KeyPress_numeric(sender, e);
        }

        /*************************************************************
         * テキストのロストフォーカス時に入力値をフォーマットする
         *************************************************************/
        private void textBox_Leave(object sender, EventArgs e)
        {
            TextBox target = (TextBox)sender;
            target.Text = Conversion.Parse(target.Text).ToString("N");
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_Prepare_OtherReg_Load(object sender, EventArgs e)
        {
            foreach (var control in tableLayoutPanel.Controls)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = decimal.Zero.ToString("N");
            }

            // データの設定
            using (var context = new CostAccountingEntities())
            {
                // 予定データの設定
                var plan = from t in context.Other
                           where t.year.Equals(Const.TARGET_YEAR)
                              && t.category.Equals((int)Const.CATEGORY_TYPE.Budget)
                           select t;

                if (plan.Count() == decimal.One)
                {
                    wageM_plan.Text = plan.First().wage_m.ToString("N");
                    wageF_plan.Text = plan.First().wage_f.ToString("N");
                    wageIndirect_plan.Text = plan.First().wage_indirect.ToString("N");
                    utilitiesFD_plan.Text = plan.First().utilities_fd.ToString("N");
                    utilitiesAD_plan.Text = plan.First().utilities_ad.ToString("N");
                    allocationFD_plan.Text = plan.First().allocation_fd.ToString("N");
                    allocationAD_plan.Text = plan.First().allocation_ad.ToString("N");
                    allocationLabor_plan.Text = plan.First().allocation_labor.ToString("N");
                    allocationSale_plan.Text = plan.First().allocation_sale.ToString("N");
                    allocationMng_plan.Text = plan.First().allocation_mng.ToString("N");
                    allocationExt_plan.Text = plan.First().allocation_ext.ToString("N");
                    rateExpend_plan.Text = plan.First().rate_expend.ToString("N");
                    rateLoss_plan.Text = plan.First().rate_loss.ToString("N");
                    trayNum_plan.Text = plan.First().tray_num.ToString("N");
                }

                // 実績データの設定
                var actual = from t in context.Other
                             where t.year.Equals(Const.TARGET_YEAR)
                                && t.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                             select t;

                if (actual.Count() == decimal.One)
                {
                    wageM_actual.Text = actual.First().wage_m.ToString("N");
                    wageF_actual.Text = actual.First().wage_f.ToString("N");
                    wageIndirect_actual.Text = actual.First().wage_indirect.ToString("N");
                    utilitiesFD_actual.Text = actual.First().utilities_fd.ToString("N");
                    utilitiesAD_actual.Text = actual.First().utilities_ad.ToString("N");
                    allocationFD_actual.Text = actual.First().allocation_fd.ToString("N");
                    allocationAD_actual.Text = actual.First().allocation_ad.ToString("N");
                    allocationLabor_actual.Text = actual.First().allocation_labor.ToString("N");
                    allocationSale_actual.Text = actual.First().allocation_sale.ToString("N");
                    allocationMng_actual.Text = actual.First().allocation_mng.ToString("N");
                    allocationExt_actual.Text = actual.First().allocation_ext.ToString("N");
                    rateExpend_actual.Text = actual.First().rate_expend.ToString("N");
                    rateLoss_actual.Text = actual.First().rate_loss.ToString("N");
                    trayNum_actual.Text = actual.First().tray_num.ToString("N");
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

            // データ登録処理（削除→登録を行う）
            using (var context = new CostAccountingEntities())
            {
                // データ削除
                var target = from t in context.Other
                             where t.year.Equals(Const.TARGET_YEAR)
                             select t;
                context.Other.RemoveRange(target);

                // データ登録
                var plan = new Other()
                {
                    year = Const.TARGET_YEAR,
                    category = (int)Const.CATEGORY_TYPE.Budget,
                    wage_m = Conversion.Parse(wageM_plan.Text),
                    wage_f = Conversion.Parse(wageF_plan.Text),
                    wage_indirect = Conversion.Parse(wageIndirect_plan.Text),
                    utilities_fd = Conversion.Parse(utilitiesFD_plan.Text),
                    utilities_ad = Conversion.Parse(utilitiesAD_plan.Text),
                    allocation_fd = Conversion.Parse(allocationFD_plan.Text),
                    allocation_ad = Conversion.Parse(allocationAD_plan.Text),
                    allocation_labor = Conversion.Parse(allocationLabor_plan.Text),
                    allocation_sale = Conversion.Parse(allocationSale_plan.Text),
                    allocation_mng = Conversion.Parse(allocationMng_plan.Text),
                    allocation_ext = Conversion.Parse(allocationExt_plan.Text),
                    rate_expend = Conversion.Parse(rateExpend_plan.Text),
                    rate_loss = Conversion.Parse(rateLoss_plan.Text),
                    tray_num = Conversion.Parse(trayNum_plan.Text),
                    update_user = SystemInformation.UserName,
                    update_date = DateTime.Now,
                    del_flg = Const.FLG_OFF
                };

                var actual = new Other()
                {
                    year = Const.TARGET_YEAR,
                    category = (int)Const.CATEGORY_TYPE.Actual,
                    wage_m = Conversion.Parse(wageM_actual.Text),
                    wage_f = Conversion.Parse(wageF_actual.Text),
                    wage_indirect = Conversion.Parse(wageIndirect_actual.Text),
                    utilities_fd = Conversion.Parse(utilitiesFD_actual.Text),
                    utilities_ad = Conversion.Parse(utilitiesAD_actual.Text),
                    allocation_fd = Conversion.Parse(allocationFD_actual.Text),
                    allocation_ad = Conversion.Parse(allocationAD_actual.Text),
                    allocation_labor = Conversion.Parse(allocationLabor_actual.Text),
                    allocation_sale = Conversion.Parse(allocationSale_actual.Text),
                    allocation_mng = Conversion.Parse(allocationMng_actual.Text),
                    allocation_ext = Conversion.Parse(allocationExt_actual.Text),
                    rate_expend = Conversion.Parse(rateExpend_actual.Text),
                    rate_loss = Conversion.Parse(rateLoss_actual.Text),
                    tray_num = Conversion.Parse(trayNum_actual.Text),
                    update_user = SystemInformation.UserName,
                    update_date = DateTime.Now,
                    del_flg = Const.FLG_OFF
                };

                context.Other.Add(plan);
                context.Other.Add(actual);
                context.SaveChanges();
            }

            Program.MessageBoxAfter("登録しました。");
        }

    }
}
