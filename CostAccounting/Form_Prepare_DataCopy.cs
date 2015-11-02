using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CostAccounting
{
    public partial class Form_PrePare_DataCopy : Form
    {
        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_PrePare_DataCopy()
        {
            InitializeComponent();
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_PrePare_DataCopy_Load(object sender, EventArgs e)
        {
            targetYear.Text = Const.TARGET_YEAR.ToString();
        }

        /*************************************************************
         * テキストボックスにて数値のみ入力可能にする
         *************************************************************/
        private void textBox_KeyPress_numeric(object sender, KeyPressEventArgs e)
        {
            Event.textBox_KeyPress_numeric(sender, e);
        }

        /*************************************************************
         * 決定ボタン押下時の処理
         *************************************************************/
        private void btnEnter_Click(object sender, EventArgs e)
        {
            // 入力チェック
            srcYear.BackColor = Color.White;

            if (String.IsNullOrEmpty(srcYear.Text))
            {
                srcYear.BackColor = Color.Red;
                Program.MessageBoxError("コピー元年度を入力してください。");
                return;
            }

            if (!(Validation.IsNumeric(srcYear.Text) && srcYear.Text.Length == 4))
            {
                srcYear.BackColor = Color.Red;
                Program.MessageBoxError("コピー元年度は数値4桁で入力してください。");
                return;
            }

            if (srcYear.Text.Equals(Const.TARGET_YEAR.ToString()))
            {
                srcYear.BackColor = Color.Red;
                Program.MessageBoxError(Const.TARGET_YEAR + "以外の年度を入力してください。");
                return;
            }

            if (Program.MessageBoxBefore(
                   string.Concat(srcYear.Text + "年度の実績データを" + Const.TARGET_YEAR + "年度の予定と実績データにコピーしますか？"
                                 , Environment.NewLine
                                 , "※現在登録されている" + Const.TARGET_YEAR + "年度のデータは削除されます")) == DialogResult.Yes)
            {
                if (Program.MessageBoxBefore("本当に実行してよろしいですか？") == DialogResult.Yes)
                {

                    // 今年度のデータを削除の上、コピー元年度のデータを今年度にコピー
                    using (var context = new CostAccountingEntities())
                    {
                        string deleteLog = allDelete(context);
                        string appendLog = allAppend(context);
                        context.SaveChanges();

                        Logger.Info(Message.INF003, new string[] { this.Text, Message.create(srcYear) + "削除件数{" + deleteLog + " } 登録件数{" + appendLog + "}" });
                    }

                    Program.MessageBoxAfter("データコピーが完了しました。");
                }
            }
        }

        /*************************************************************
         * データ登録処理
         *************************************************************/
        private string allAppend(CostAccountingEntities context)
        {
            string logStr = string.Empty;

            int srcYear = (int)Conversion.Parse(this.srcYear.Text);
            //---------------------------------------------------------
            var target01 = from t in context.CostMngTotal
                           where t.year.Equals(srcYear)
                              && t.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                           select t;
            foreach (var data in target01.ToList())
            {
                //// 固定費データは実績データにはコピーしない
                //foreach (int category in Enum.GetValues(typeof(Const.CATEGORY_TYPE)))
                //{
                var entity = DeepCopyHelper.DeepCopy<CostMngTotal>(data);
                entity.year = Const.TARGET_YEAR;
                entity.category = (int)Const.CATEGORY_TYPE.Budget;
                entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                entity.update_date = DateTime.Now;
                context.CostMngTotal.Add(entity);
                //}
            }
            logStr += string.Concat("CostMngTotal=", target01.Count(), "件 ");

            ////---------------------------------------------------------
            ////乖離幅データはコピーしない
            //var target02 = from t in context.Divergence
            //               where t.year.Equals(srcYear)
            //               select t;
            //foreach (var data in target02.ToList())
            //{
            //    var entity = DeepCopyHelper.DeepCopy<Divergence>(data);
            //    entity.year = Const.TARGET_YEAR;
            //    entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
            //    entity.update_date = DateTime.Now;
            //    context.Divergence.Add(entity);
            //}
            logStr += string.Concat("Divergence=", decimal.Zero, "件 ");

            ////---------------------------------------------------------
            var target03 = from t in context.Fare
                           where t.year.Equals(srcYear)
                           select t;
            foreach (var data in target03.ToList())
            {
                var entity = DeepCopyHelper.DeepCopy<Fare>(data);
                entity.year = Const.TARGET_YEAR;
                entity.price_budget = data.price_actual;
                entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                entity.update_date = DateTime.Now;
                context.Fare.Add(entity);
            }
            logStr += string.Concat("Fare=", target03.Count(), "件 ");

            //---------------------------------------------------------
            var target04 = from t in context.Item
                           where t.year.Equals(srcYear)
                           select t;
            foreach (var data in target04.ToList())
            {
                var entity = DeepCopyHelper.DeepCopy<Item>(data);
                entity.year = Const.TARGET_YEAR;
                entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                entity.update_date = DateTime.Now;
                context.Item.Add(entity);
            }
            logStr += string.Concat("Item=", target04.Count(), "件 ");

            //---------------------------------------------------------
            var target05 = from t in context.Machine
                           where t.year.Equals(srcYear)
                           select t;
            foreach (var data in target05.ToList())
            {
                var entity = DeepCopyHelper.DeepCopy<Machine>(data);
                entity.year = Const.TARGET_YEAR;
                entity.rate_budget = data.rate_actual;
                entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                entity.update_date = DateTime.Now;
                context.Machine.Add(entity);
            }
            logStr += string.Concat("Machine=", target05.Count(), "件 ");

            //---------------------------------------------------------
            var target06 = from t in context.Material
                           where t.year.Equals(srcYear)
                           select t;
            foreach (var data in target06.ToList())
            {
                var entity = DeepCopyHelper.DeepCopy<Material>(data);
                entity.year = Const.TARGET_YEAR;
                entity.price_budget = data.price_actual;
                entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                entity.update_date = DateTime.Now;
                context.Material.Add(entity);
            }
            logStr += string.Concat("Material=", target06.Count(), "件 ");

            //---------------------------------------------------------
            var target07 = from t in context.Other
                           where t.year.Equals(srcYear)
                              && t.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                           select t;
            foreach (var data in target07.ToList())
            {
                foreach (int category in Enum.GetValues(typeof(Const.CATEGORY_TYPE)))
                {
                    var entity = DeepCopyHelper.DeepCopy<Other>(data);
                    entity.year = Const.TARGET_YEAR;
                    entity.category = category;
                    entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                    entity.update_date = DateTime.Now;
                    context.Other.Add(entity);
                }
            }
            logStr += string.Concat("Other=", target07.Count(), "件 ");

            //---------------------------------------------------------
            var target08 = from t in context.Product
                           where t.year.Equals(srcYear)
                              && t.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                           select t;
            foreach (var data in target08.ToList())
            {
                foreach (int category in Enum.GetValues(typeof(Const.CATEGORY_TYPE)))
                {
                    var entity = DeepCopyHelper.DeepCopy<Product>(data);
                    entity.year = Const.TARGET_YEAR;
                    entity.category = category;
                    entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                    entity.update_date = DateTime.Now;
                    context.Product.Add(entity);
                }
            }
            logStr += string.Concat("Product=", target08.Count(), "件 ");

            //---------------------------------------------------------
            var target09 = from t in context.ProductBlend
                           where t.year.Equals(srcYear)
                              && t.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                           select t;
            foreach (var data in target09.ToList())
            {
                foreach (int category in Enum.GetValues(typeof(Const.CATEGORY_TYPE)))
                {
                    var entity = DeepCopyHelper.DeepCopy<ProductBlend>(data);
                    entity.year = Const.TARGET_YEAR;
                    entity.category = category;
                    entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                    entity.update_date = DateTime.Now;
                    context.ProductBlend.Add(entity);
                }
            }
            logStr += string.Concat("ProductBlend=", target09.Count(), "件 ");

            //---------------------------------------------------------
            var target10 = from t in context.ProductCode
                           where t.year.Equals(srcYear)
                           select t;
            foreach (var data in target10.ToList())
            {
                var entity = DeepCopyHelper.DeepCopy<ProductCode>(data);
                entity.year = Const.TARGET_YEAR;
                entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                entity.update_date = DateTime.Now;
                context.ProductCode.Add(entity);
            }
            logStr += string.Concat("ProductCode=", target10.Count(), "件 ");

            //---------------------------------------------------------
            var target11 = from t in context.ProductContractor
                           where t.year.Equals(srcYear)
                              && t.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                           select t;
            foreach (var data in target11.ToList())
            {
                foreach (int category in Enum.GetValues(typeof(Const.CATEGORY_TYPE)))
                {
                    var entity = DeepCopyHelper.DeepCopy<ProductContractor>(data);
                    entity.year = Const.TARGET_YEAR;
                    entity.category = category;
                    entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                    entity.update_date = DateTime.Now;
                    context.ProductContractor.Add(entity);
                }
            }
            logStr += string.Concat("ProductContractor=", target11.Count(), "件 ");

            //---------------------------------------------------------
            var target12 = from t in context.ProductMachine
                           where t.year.Equals(srcYear)
                              && t.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                           select t;
            foreach (var data in target12.ToList())
            {
                foreach (int category in Enum.GetValues(typeof(Const.CATEGORY_TYPE)))
                {
                    var entity = DeepCopyHelper.DeepCopy<ProductMachine>(data);
                    entity.year = Const.TARGET_YEAR;
                    entity.category = category;
                    entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                    entity.update_date = DateTime.Now;
                    context.ProductMachine.Add(entity);
                }
            }
            logStr += string.Concat("ProductMachine=", target12.Count(), "件 ");

            //---------------------------------------------------------
            var target13 = from t in context.ProductMaterial
                           where t.year.Equals(srcYear)
                              && t.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                           select t;
            foreach (var data in target13.ToList())
            {
                foreach (int category in Enum.GetValues(typeof(Const.CATEGORY_TYPE)))
                {
                    var entity = DeepCopyHelper.DeepCopy<ProductMaterial>(data);
                    entity.year = Const.TARGET_YEAR;
                    entity.category = category;
                    entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                    entity.update_date = DateTime.Now;
                    context.ProductMaterial.Add(entity);
                }
            }
            logStr += string.Concat("ProductMaterial=", target13.Count(), "件 ");

            //---------------------------------------------------------
            var target14 = from t in context.ProductMaterialsFare
                           where t.year.Equals(srcYear)
                              && t.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                           select t;
            foreach (var data in target14.ToList())
            {
                foreach (int category in Enum.GetValues(typeof(Const.CATEGORY_TYPE)))
                {
                    var entity = DeepCopyHelper.DeepCopy<ProductMaterialsFare>(data);
                    entity.year = Const.TARGET_YEAR;
                    entity.category = category;
                    entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                    entity.update_date = DateTime.Now;
                    context.ProductMaterialsFare.Add(entity);
                }
            }
            logStr += string.Concat("ProductMaterialsFare=", target14.Count(), "件 ");

            //---------------------------------------------------------
            var targe15 = from t in context.ProductPacking
                          where t.year.Equals(srcYear)
                              && t.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                          select t;
            foreach (var data in targe15.ToList())
            {
                foreach (int category in Enum.GetValues(typeof(Const.CATEGORY_TYPE)))
                {
                    var entity = DeepCopyHelper.DeepCopy<ProductPacking>(data);
                    entity.year = Const.TARGET_YEAR;
                    entity.category = category;
                    entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                    entity.update_date = DateTime.Now;
                    context.ProductPacking.Add(entity);
                }
            }
            logStr += string.Concat("ProductPacking=", targe15.Count(), "件 ");

            //---------------------------------------------------------
            var target16 = from t in context.ProductPackingFare
                           where t.year.Equals(srcYear)
                              && t.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                           select t;
            foreach (var data in target16.ToList())
            {
                foreach (int category in Enum.GetValues(typeof(Const.CATEGORY_TYPE)))
                {
                    var entity = DeepCopyHelper.DeepCopy<ProductPackingFare>(data);
                    entity.year = Const.TARGET_YEAR;
                    entity.category = category;
                    entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                    entity.update_date = DateTime.Now;
                    context.ProductPackingFare.Add(entity);
                }
            }
            logStr += string.Concat("ProductPackingFare=", target16.Count(), "件 ");

            //---------------------------------------------------------
            var target17 = from t in context.ProductSupplier
                           where t.year.Equals(srcYear)
                              && t.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                           select t;
            foreach (var data in target17.ToList())
            {
                foreach (int category in Enum.GetValues(typeof(Const.CATEGORY_TYPE)))
                {
                    var entity = DeepCopyHelper.DeepCopy<ProductSupplier>(data);
                    entity.year = Const.TARGET_YEAR;
                    entity.category = category;
                    entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                    entity.update_date = DateTime.Now;

                    // 実績データの売上にははコピーしない
                    if (category.Equals((int)Const.CATEGORY_TYPE.Actual))
                    {
                        entity.month_01 = decimal.Zero; entity.month_02 = decimal.Zero; entity.month_03 = decimal.Zero;
                        entity.month_04 = decimal.Zero; entity.month_05 = decimal.Zero; entity.month_06 = decimal.Zero;
                        entity.month_07 = decimal.Zero; entity.month_08 = decimal.Zero; entity.month_09 = decimal.Zero;
                        entity.month_10 = decimal.Zero; entity.month_11 = decimal.Zero; entity.month_12 = decimal.Zero;
                    }

                    context.ProductSupplier.Add(entity);
                }
            }
            logStr += string.Concat("ProductSupplier=", target17.Count(), "件 ");

            //---------------------------------------------------------
            var target18 = from t in context.RowMaterial
                           where t.year.Equals(srcYear)
                           select t;
            foreach (var data in target18.ToList())
            {
                var entity = DeepCopyHelper.DeepCopy<RowMaterial>(data);
                entity.year = Const.TARGET_YEAR;
                entity.price_budget = data.price_actual;
                entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                entity.update_date = DateTime.Now;
                context.RowMaterial.Add(entity);
            }
            logStr += string.Concat("RowMaterial=", target18.Count(), "件 ");

            //---------------------------------------------------------
            var target19 = from t in context.Supplier
                           where t.year.Equals(srcYear)
                           select t;
            foreach (var data in target19.ToList())
            {
                var entity = DeepCopyHelper.DeepCopy<Supplier>(data);
                entity.year = Const.TARGET_YEAR;
                entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                entity.update_date = DateTime.Now;
                context.Supplier.Add(entity);
            }
            logStr += string.Concat("Supplier=", target19.Count(), "件 ");

            return logStr.TrimEnd(' ');
        }

        /*************************************************************
         * データ削除処理
         *************************************************************/
        private string allDelete(CostAccountingEntities context)
        {
            string logStr = string.Empty;

            //---------------------------------------------------------
            var target01 = from t in context.CostMngTotal
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.CostMngTotal.RemoveRange(target01);
            logStr += string.Concat("CostMngTotal=", target01.Count(), "件 ");

            //---------------------------------------------------------
            var target02 = from t in context.Divergence
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Divergence.RemoveRange(target02);
            logStr += string.Concat("Divergence=", target02.Count(), "件 ");

            //---------------------------------------------------------
            var target03 = from t in context.Fare
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Fare.RemoveRange(target03);
            logStr += string.Concat("Fare=", target03.Count(), "件 ");

            //---------------------------------------------------------
            var target04 = from t in context.Item
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Item.RemoveRange(target04);
            logStr += string.Concat("Item=", target04.Count(), "件 ");

            //---------------------------------------------------------
            var target05 = from t in context.Machine
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Machine.RemoveRange(target05);
            logStr += string.Concat("Machine=", target05.Count(), "件 ");

            //---------------------------------------------------------
            var target06 = from t in context.Material
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Material.RemoveRange(target06);
            logStr += string.Concat("Material=", target06.Count(), "件 ");

            //---------------------------------------------------------
            var target07 = from t in context.Other
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Other.RemoveRange(target07);
            logStr += string.Concat("Other=", target07.Count(), "件 ");

            //---------------------------------------------------------
            var target08 = from t in context.Product
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Product.RemoveRange(target08);
            logStr += string.Concat("Product=", target08.Count(), "件 ");

            //---------------------------------------------------------
            var target09 = from t in context.ProductBlend
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.ProductBlend.RemoveRange(target09);
            logStr += string.Concat("ProductBlend=", target09.Count(), "件 ");

            //---------------------------------------------------------
            var target10 = from t in context.ProductCode
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.ProductCode.RemoveRange(target10);
            logStr += string.Concat("ProductCode=", target10.Count(), "件 ");

            //---------------------------------------------------------
            var target11 = from t in context.ProductContractor
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.ProductContractor.RemoveRange(target11);
            logStr += string.Concat("ProductContractor=", target11.Count(), "件 ");

            //---------------------------------------------------------
            var target12 = from t in context.ProductMachine
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.ProductMachine.RemoveRange(target12);
            logStr += string.Concat("ProductMachine=", target12.Count(), "件 ");

            //---------------------------------------------------------
            var target13 = from t in context.ProductMaterial
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.ProductMaterial.RemoveRange(target13);
            logStr += string.Concat("ProductMaterial=", target13.Count(), "件 ");

            //---------------------------------------------------------
            var target14 = from t in context.ProductMaterialsFare
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.ProductMaterialsFare.RemoveRange(target14);
            logStr += string.Concat("Divergence=", target01.Count(), "件 ");

            //---------------------------------------------------------
            var targe15 = from t in context.ProductPacking
                          where t.year.Equals(Const.TARGET_YEAR)
                          select t;
            context.ProductPacking.RemoveRange(targe15);
            logStr += string.Concat("ProductPacking=", targe15.Count(), "件 ");

            //---------------------------------------------------------
            var target16 = from t in context.ProductPackingFare
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.ProductPackingFare.RemoveRange(target16);
            logStr += string.Concat("ProductPackingFare=", target16.Count(), "件 ");

            //---------------------------------------------------------
            var target17 = from t in context.ProductSupplier
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.ProductSupplier.RemoveRange(target17);
            logStr += string.Concat("ProductSupplier=", target17.Count(), "件 ");

            //---------------------------------------------------------
            var target18 = from t in context.RowMaterial
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.RowMaterial.RemoveRange(target18);
            logStr += string.Concat("RowMaterial=", target18.Count(), "件 ");

            //---------------------------------------------------------
            var target19 = from t in context.Supplier
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Supplier.RemoveRange(target19);
            logStr += string.Concat("Supplier=", target19.Count(), "件 ");

            return logStr.TrimEnd(' ');
        }
    }
}
