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
        private List<Label[]> labelDic;
        private Dictionary<int, TextBox[]> monthPairText;
        private Dictionary<CheckBox, TextBox[]> checkboxPairText;
        private Dictionary<CheckBox, int> checkboxPairMonth;

        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_CostMng_Divergence()
        {
            InitializeComponent();

            labelDic = new List<Label[]>()
            { 
                new Label[]{ materialCost, materialCost_db, materialCost_diff, materialCost_rate } 
              , new Label[]{ laborCost, laborCost_db,laborCost_diff,laborCost_rate }
              , new Label[]{ contractorsCost, contractorsCost_db, contractorsCost_diff, contractorsCost_rate }
              , new Label[]{ materialsFare, materialsFare_db, materialsFare_diff, materialsFare_rate }
              , new Label[]{ packingCost, packingCost_db, packingCost_diff, packingCost_rate }
              , new Label[]{ utilitiesCost, utilitiesCost_db, utilitiesCost_diff, utilitiesCost_rate }
              , new Label[]{ otherCost, otherCost_db, otherCost_diff, otherCost_rate }
              , new Label[]{ packingFare, packingFare_db, packingFare_diff, packingFare_rate }
            };


            monthPairText = new Dictionary<int, TextBox[]>() 
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

            checkboxPairText = new Dictionary<CheckBox, TextBox[]>() 
            { 
                { checkBoxApr, new TextBox[] { materialCost_Apr, laborCost_Apr, contractorsCost_Apr, materialsFare_Apr, packingCost_Apr, utilitiesCost_Apr, otherCost_Apr, packingFare_Apr } }
              , { checkBoxMay, new TextBox[] { materialCost_May, laborCost_May, contractorsCost_May, materialsFare_May, packingCost_May, utilitiesCost_May, otherCost_May, packingFare_May } }
              , { checkBoxJun, new TextBox[] { materialCost_Jun, laborCost_Jun, contractorsCost_Jun, materialsFare_Jun, packingCost_Jun, utilitiesCost_Jun, otherCost_Jun, packingFare_Jun } }
              , { checkBoxJul, new TextBox[] { materialCost_Jul, laborCost_Jul, contractorsCost_Jul, materialsFare_Jul, packingCost_Jul, utilitiesCost_Jul, otherCost_Jul, packingFare_Jul } }
              , { checkBoxAug, new TextBox[] { materialCost_Aug, laborCost_Aug, contractorsCost_Aug, materialsFare_Aug, packingCost_Aug, utilitiesCost_Aug, otherCost_Aug, packingFare_Aug } }
              , { checkBoxSep, new TextBox[] { materialCost_Sep, laborCost_Sep, contractorsCost_Sep, materialsFare_Sep, packingCost_Sep, utilitiesCost_Sep, otherCost_Sep, packingFare_Sep } }
              , { checkBoxOct, new TextBox[] { materialCost_Oct, laborCost_Oct, contractorsCost_Oct, materialsFare_Oct, packingCost_Oct, utilitiesCost_Oct, otherCost_Oct, packingFare_Oct } }
              , { checkBoxNov, new TextBox[] { materialCost_Nov, laborCost_Nov, contractorsCost_Nov, materialsFare_Nov, packingCost_Nov, utilitiesCost_Nov, otherCost_Nov, packingFare_Nov } }
              , { checkBoxDec, new TextBox[] { materialCost_Dec, laborCost_Dec, contractorsCost_Dec, materialsFare_Dec, packingCost_Dec, utilitiesCost_Dec, otherCost_Dec, packingFare_Dec } }
              , { checkBoxJan, new TextBox[] { materialCost_Jan, laborCost_Jan, contractorsCost_Jan, materialsFare_Jan, packingCost_Jan, utilitiesCost_Jan, otherCost_Jan, packingFare_Jan } }
              , { checkBoxFeb, new TextBox[] { materialCost_Feb, laborCost_Feb, contractorsCost_Feb, materialsFare_Feb, packingCost_Feb, utilitiesCost_Feb, otherCost_Feb, packingFare_Feb } }
              , { checkBoxMar, new TextBox[] { materialCost_Mar, laborCost_Mar, contractorsCost_Mar, materialsFare_Mar, packingCost_Mar, utilitiesCost_Mar, otherCost_Mar, packingFare_Mar } } 
            };

            checkboxPairMonth = new Dictionary<CheckBox, int>() 
            { 
                { checkBoxApr, 4  }
              , { checkBoxMay, 5  }
              , { checkBoxJun, 6  }
              , { checkBoxJul, 7  }
              , { checkBoxAug, 8  }
              , { checkBoxSep, 9  }
              , { checkBoxOct, 10 }
              , { checkBoxNov, 11 }
              , { checkBoxDec, 12 }
              , { checkBoxJan, 1  }
              , { checkBoxFeb, 2  }
              , { checkBoxMar, 3  } 
            };
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_CostMng_Divergence_Load(object sender, EventArgs e)
        {
            // 決算書実績データを設定する
            set決算書実績データ();

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
            Event.textBox_Leave_format(sender, e);
            calc決算書実績();
            calc乖離幅();
        }

        /*************************************************************
         * チェックボックスの状態から、決算書実績の計上対象月の値を集計する
         *************************************************************/
        private void calc決算書実績()
        {
            foreach (Label[] target in labelDic)
                target[0].Text = decimal.Zero.ToString("N");

            foreach (CheckBox target in checkboxPairText.Keys)
            {
                if (target.Checked)
                {
                    materialCost.Text = decimal.Add(Conversion.Parse(materialCost.Text), Conversion.Parse(checkboxPairText[target][0].Text)).ToString("N");
                    laborCost.Text = decimal.Add(Conversion.Parse(laborCost.Text), Conversion.Parse(checkboxPairText[target][1].Text)).ToString("N");
                    contractorsCost.Text = decimal.Add(Conversion.Parse(contractorsCost.Text), Conversion.Parse(checkboxPairText[target][2].Text)).ToString("N");
                    materialsFare.Text = decimal.Add(Conversion.Parse(materialsFare.Text), Conversion.Parse(checkboxPairText[target][3].Text)).ToString("N");
                    packingCost.Text = decimal.Add(Conversion.Parse(packingCost.Text), Conversion.Parse(checkboxPairText[target][4].Text)).ToString("N");
                    utilitiesCost.Text = decimal.Add(Conversion.Parse(utilitiesCost.Text), Conversion.Parse(checkboxPairText[target][5].Text)).ToString("N");
                    otherCost.Text = decimal.Add(Conversion.Parse(otherCost.Text), Conversion.Parse(checkboxPairText[target][6].Text)).ToString("N");
                    packingFare.Text = decimal.Add(Conversion.Parse(packingFare.Text), Conversion.Parse(checkboxPairText[target][7].Text)).ToString("N");
                }
            }
        }

        /*************************************************************
         * チェックボックスの状態から、原価計算実績の値を設定する
         *************************************************************/
        private void calc原価計算表実績()
        {
            foreach (Label[] target in labelDic)
                target[1].Text = decimal.Zero.ToString("N");

            using (var context = new CostAccountingEntities())
            {
                string inStr = string.Empty;
                foreach (CheckBox target in checkboxPairText.Keys)
                {
                    if (target.Checked)
                        inStr += string.Concat(checkboxPairMonth[target], ",");
                }

                inStr = inStr.TrimEnd(',');
                var targetData = from t in context.Divergence
                                 where t.year.Equals(Const.TARGET_YEAR)
                                    && inStr.Contains(t.month.ToString())
                                    && t.del_flg.Equals(Const.FLG_OFF)
                                 select t;

                foreach (var data in targetData.ToList())
                {
                    materialCost_db.Text = decimal.Add(Conversion.Parse(materialCost_db.Text), data.materialCost_costing).ToString("N");
                    laborCost_db.Text = decimal.Add(Conversion.Parse(laborCost_db.Text), data.laborCost_costing).ToString("N");
                    contractorsCost_db.Text = decimal.Add(Conversion.Parse(contractorsCost_db.Text), data.contractorsCost_costing).ToString("N");
                    materialsFare_db.Text = decimal.Add(Conversion.Parse(materialsFare_db.Text), data.materialsFare_costing).ToString("N");
                    packingCost_db.Text = decimal.Add(Conversion.Parse(packingCost_db.Text), data.packingCost_costing).ToString("N");
                    utilitiesCost_db.Text = decimal.Add(Conversion.Parse(utilitiesCost_db.Text), data.utilitiesCost_costing).ToString("N");
                    otherCost_db.Text = decimal.Add(Conversion.Parse(otherCost_db.Text), data.otherCost_costing).ToString("N");
                    packingFare_db.Text = decimal.Add(Conversion.Parse(packingFare_db.Text), data.packingFare_costing).ToString("N");
                }
            }
        }

        /*************************************************************
         * 乖離幅を計算する
         *************************************************************/
        private void calc乖離幅()
        {
            decimal total = decimal.Zero;
            decimal total_db = decimal.Zero;
            foreach (Label[] target in labelDic)
            {
                decimal value = Conversion.Parse(target[0].Text);
                decimal db = Conversion.Parse(target[1].Text);
                decimal diff = decimal.Subtract(value, db);
                decimal rate = db != decimal.Zero ? decimal.Divide(diff, db) : decimal.Zero;

                target[2].Text = diff.ToString("N");
                target[3].Text = rate.ToString("P");

                total += value;
                total_db += db;
            }

            decimal total_diff = decimal.Subtract(total, total_db);
            decimal total_rate = total_db != decimal.Zero ? decimal.Divide(total_diff, total_db) : decimal.Zero;

            this.total.Text = total.ToString("N");
            this.total_db.Text = total_db.ToString("N");
            this.total_diff.Text = total_diff.ToString("N");
            this.total_rate.Text = total_rate.ToString("P");
        }

        /*************************************************************
         * 本画面のすべての計算を行う
         *************************************************************/
        private void calcAll()
        {
            // チェックボックスの状態から、決算書実績の計上対象月の値を集計する
            calc決算書実績();

            // チェックボックスの状態から、原価計算実績の値を設定する
            calc原価計算表実績();

            // 乖離幅を計算する
            calc乖離幅();
        }

        /*************************************************************
         * 決算書実績データを設定する
         *************************************************************/
        private void set決算書実績データ()
        {
            using (var context = new CostAccountingEntities())
            {
                foreach (int month in monthPairText.Keys)
                {
                    var target = from t in context.Divergence
                                 where t.year.Equals(Const.TARGET_YEAR)
                                    && t.month.Equals(month)
                                    && t.del_flg.Equals(Const.FLG_OFF)
                                 select t;

                    if (target.Count() == decimal.One)
                    {
                        monthPairText[month][0].Text = target.First().materialCost.ToString("N");
                        monthPairText[month][1].Text = target.First().laborCost.ToString("N");
                        monthPairText[month][2].Text = target.First().contractorsCost.ToString("N");
                        monthPairText[month][3].Text = target.First().materialsFare.ToString("N");
                        monthPairText[month][4].Text = target.First().packingCost.ToString("N");
                        monthPairText[month][5].Text = target.First().utilitiesCost.ToString("N");
                        monthPairText[month][6].Text = target.First().otherCost.ToString("N");
                        monthPairText[month][7].Text = target.First().packingFare.ToString("N");
                    }
                    else
                    {
                        foreach (TextBox textbox in monthPairText[month])
                            textbox.Text = decimal.Zero.ToString("N");
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

            // データ登録処理（削除→登録をおこなう）
            using (var context = new CostAccountingEntities())
            {
                foreach (int month in monthPairText.Keys)
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
                            materialCost = Conversion.Parse(monthPairText[month][0].Text),
                            laborCost = Conversion.Parse(monthPairText[month][1].Text),
                            contractorsCost = Conversion.Parse(monthPairText[month][2].Text),
                            materialsFare = Conversion.Parse(monthPairText[month][3].Text),
                            packingCost = Conversion.Parse(monthPairText[month][4].Text),
                            utilitiesCost = Conversion.Parse(monthPairText[month][5].Text),
                            otherCost = Conversion.Parse(monthPairText[month][6].Text),
                            packingFare = Conversion.Parse(monthPairText[month][7].Text),
                            update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName),
                            update_date = DateTime.Now,
                            del_flg = Const.FLG_OFF
                        };
                        context.Divergence.Add(entity);
                    }
                    else
                    {
                        // 修正処理
                        target.First().materialCost = Conversion.Parse(monthPairText[month][0].Text);
                        target.First().laborCost = Conversion.Parse(monthPairText[month][1].Text);
                        target.First().contractorsCost = Conversion.Parse(monthPairText[month][2].Text);
                        target.First().materialsFare = Conversion.Parse(monthPairText[month][3].Text);
                        target.First().packingCost = Conversion.Parse(monthPairText[month][4].Text);
                        target.First().utilitiesCost = Conversion.Parse(monthPairText[month][5].Text);
                        target.First().otherCost = Conversion.Parse(monthPairText[month][6].Text);
                        target.First().packingFare = Conversion.Parse(monthPairText[month][7].Text);
                        target.First().update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                        target.First().update_date = DateTime.Now;
                    }
                }
                context.SaveChanges();
            }

            Logger.Info(Message.INF003, new string[] { this.Text, "-" });
            Program.MessageBoxAfter("登録しました。");
        }

        /*************************************************************
         * チェックボックスのON/OFFに従い、計算を行う
         *************************************************************/
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            calcAll();
        }

        /*************************************************************
         * 月のチェックボックスを全てONにする
         *************************************************************/
        private void btnAllCheck_Click(object sender, EventArgs e)
        {
            changeCheckBoxState(groupMonth.Controls, true);
        }

        /*************************************************************
         * 月のチェックボックスを全てOFFにする
         *************************************************************/
        private void btnAllClear_Click(object sender, EventArgs e)
        {
            changeCheckBoxState(groupMonth.Controls, false);
        }

        /*************************************************************
         * チェックボックスの状態を変更する
         *************************************************************/
        private void changeCheckBoxState(Control.ControlCollection controls, bool state)
        {
            foreach (var control in controls)
            {
                if (control is CheckBox)
                {
                    CheckBox target = (CheckBox)control;
                    target.CheckedChanged -= new EventHandler(checkBox_CheckedChanged);
                    target.Checked = state;
                    target.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
                }
            }
            calcAll();
        }
    }
}
