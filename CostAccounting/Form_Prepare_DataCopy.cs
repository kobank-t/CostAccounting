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
                        allDelete(context);
                        allAppend(context);
                        context.SaveChanges();
                    }

                    Program.MessageBoxAfter("データコピーが完了しました。");
                }
            }
        }

        /*************************************************************
         * データ登録処理
         *************************************************************/
        private void allAppend(CostAccountingEntities context)
        {
            int srcYear = (int)Conversion.Parse(this.srcYear.Text);
            //---------------------------------------------------------
            var target01 = from t in context.CostMngTotal
                           where t.year.Equals(srcYear)
                              && t.category.Equals((int)Const.CATEGORY_TYPE.Actual)
                           select t;
            foreach (var data in target01.ToList())
            {
                foreach (int category in Enum.GetValues(typeof(Const.CATEGORY_TYPE)))
                {
                    var entity = DeepCopyHelper.DeepCopy<CostMngTotal>(data);
                    entity.year = Const.TARGET_YEAR;
                    entity.category = category;
                    entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                    entity.update_date = DateTime.Now;
                    context.CostMngTotal.Add(entity);
                }
            }

            ////---------------------------------------------------------
            var target02 = from t in context.Divergence
                           where t.year.Equals(srcYear)
                           select t;
            foreach (var data in target02.ToList())
            {
                var entity = DeepCopyHelper.DeepCopy<Divergence>(data);
                entity.year = Const.TARGET_YEAR;
                entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                entity.update_date = DateTime.Now;
                context.Divergence.Add(entity);
            }

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

            //---------------------------------------------------------
            var target09 = from t in context.ProductBlend
                           where t.id.StartsWith(srcYear.ToString())
                              && t.id.EndsWith(((int)Const.CATEGORY_TYPE.Actual).ToString())
                           select t;
            foreach (var data in target09.ToList())
            {
                foreach (int category in Enum.GetValues(typeof(Const.CATEGORY_TYPE)))
                {
                    var entity = DeepCopyHelper.DeepCopy<ProductBlend>(data);
                    entity.id = "";
                    entity.update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                    entity.update_date = DateTime.Now;
                    context.ProductBlend.Add(entity);
                }
            }

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

            ////---------------------------------------------------------
            //var target11 = from t in context.ProductContractor
            //               where t.id.StartsWith(srcYear.ToString())
            //               select t;
            //context.ProductContractor.AddRange(target11);

            ////---------------------------------------------------------
            //var target12 = from t in context.ProductMachine
            //               where t.id.StartsWith(srcYear.ToString())
            //               select t;
            //context.ProductMachine.AddRange(target12);

            ////---------------------------------------------------------
            //var target13 = from t in context.ProductMaterial
            //               where t.id.StartsWith(srcYear.ToString())
            //               select t;
            //context.ProductMaterial.AddRange(target13);

            ////---------------------------------------------------------
            //var target14 = from t in context.ProductMaterialsFare
            //               where t.id.StartsWith(srcYear.ToString())
            //               select t;
            //context.ProductMaterialsFare.AddRange(target14);

            ////---------------------------------------------------------
            //var targe15 = from t in context.ProductPacking
            //              where t.id.StartsWith(srcYear.ToString())
            //              select t;
            //context.ProductPacking.AddRange(targe15);

            ////---------------------------------------------------------
            //var target16 = from t in context.ProductPackingFare
            //               where t.id.StartsWith(srcYear.ToString())
            //               select t;
            //context.ProductPackingFare.AddRange(target16);

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
                    context.ProductSupplier.Add(entity);
                }
            }

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
        }

        /*************************************************************
         * データ削除処理
         *************************************************************/
        private void allDelete(CostAccountingEntities context)
        {
            //---------------------------------------------------------
            var target01 = from t in context.CostMngTotal
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.CostMngTotal.RemoveRange(target01);

            //---------------------------------------------------------
            var target02 = from t in context.Divergence
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Divergence.RemoveRange(target02);

            //---------------------------------------------------------
            var target03 = from t in context.Fare
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Fare.RemoveRange(target03);

            //---------------------------------------------------------
            var target04 = from t in context.Item
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Item.RemoveRange(target04);

            //---------------------------------------------------------
            var target05 = from t in context.Machine
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Machine.RemoveRange(target05);

            //---------------------------------------------------------
            var target06 = from t in context.Material
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Material.RemoveRange(target06);

            //---------------------------------------------------------
            var target07 = from t in context.Other
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Other.RemoveRange(target07);

            //---------------------------------------------------------
            var target08 = from t in context.Product
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Product.RemoveRange(target08);

            //---------------------------------------------------------
            var target09 = from t in context.ProductBlend
                           where t.id.StartsWith(Const.TARGET_YEAR.ToString())
                           select t;
            context.ProductBlend.RemoveRange(target09);

            //---------------------------------------------------------
            var target10 = from t in context.ProductCode
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.ProductCode.RemoveRange(target10);

            //---------------------------------------------------------
            var target11 = from t in context.ProductContractor
                           where t.id.StartsWith(Const.TARGET_YEAR.ToString())
                           select t;
            context.ProductContractor.RemoveRange(target11);

            //---------------------------------------------------------
            var target12 = from t in context.ProductMachine
                           where t.id.StartsWith(Const.TARGET_YEAR.ToString())
                           select t;
            context.ProductMachine.RemoveRange(target12);

            //---------------------------------------------------------
            var target13 = from t in context.ProductMaterial
                           where t.id.StartsWith(Const.TARGET_YEAR.ToString())
                           select t;
            context.ProductMaterial.RemoveRange(target13);

            //---------------------------------------------------------
            var target14 = from t in context.ProductMaterialsFare
                           where t.id.StartsWith(Const.TARGET_YEAR.ToString())
                           select t;
            context.ProductMaterialsFare.RemoveRange(target14);

            //---------------------------------------------------------
            var targe15 = from t in context.ProductPacking
                          where t.id.StartsWith(Const.TARGET_YEAR.ToString())
                          select t;
            context.ProductPacking.RemoveRange(targe15);

            //---------------------------------------------------------
            var target16 = from t in context.ProductPackingFare
                           where t.id.StartsWith(Const.TARGET_YEAR.ToString())
                           select t;
            context.ProductPackingFare.RemoveRange(target16);

            //---------------------------------------------------------
            var target17 = from t in context.ProductSupplier
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.ProductSupplier.RemoveRange(target17);

            //---------------------------------------------------------
            var target18 = from t in context.RowMaterial
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.RowMaterial.RemoveRange(target18);

            //---------------------------------------------------------
            var target19 = from t in context.Supplier
                           where t.year.Equals(Const.TARGET_YEAR)
                           select t;
            context.Supplier.RemoveRange(target19);
        }
    }
}
