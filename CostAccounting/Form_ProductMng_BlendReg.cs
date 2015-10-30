using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CostAccounting
{
    public partial class Form_ProductMng_BlendReg : Form
    {
        // データグリッドビューのテキストボックスのコントロール
        private DataGridViewTextBoxEditingControl dgvTextBox = null;

        public Form_ProductMng_BlendReg()
        {
            InitializeComponent();
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_ProductMng_BlendReg_Load(object sender, EventArgs e)
        {
            Const.CATEGORY_TYPE category = Program.judgeCategory(radioBudget, radioActual);
            // 種目コードのコンボボックス生成
            DataTable itemTable = DataTableSupport.item;
            itemCode.DataSource = itemTable;
            itemCode.ValueMember = itemTable.Columns[0].Caption;
            itemCode.DisplayMember = itemTable.Columns[1].Caption;

            // 処理区分の設定
            setOperationKbn();

            // 初期設定における計算
            calcAll();
        }

        /*************************************************************
         * ラジオボタンの状態に従い、値の再設定と再計算を行う
         *************************************************************/
        private void radio_CheckedChanged(object sender, EventArgs e)
        {
            // チェックされた方のイベントのみ処理を行う。
            RadioButton radio = (RadioButton)sender;
            if (!radio.Checked)
                return;

            // ブレンド対象の商品データを再設定
            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                string code = (string)row.Cells["dgvProductCode"].Value;
                if (!string.IsNullOrEmpty(code))
                    setProductData(code, row.Index);
            }

            // 商品データと取引先データの再設定
            if (!string.IsNullOrEmpty(productCode.Text)
                && Program.MessageBoxBefore(string.Concat(productName.Text + "の登録済み" + radio.Text + "情報に切り替えますか？"
                                                          , Environment.NewLine
                                                          , "※" + radio.Text + "情報が未登録の場合は、全て初期値を設定します。")) == DialogResult.Yes)
            {
                setProductData();
                setSupplierData();
            }
            setOperationKbn();

            // 設定内容で再計算を行う
            calcAll();
        }

        /*************************************************************
         * 商品検索ボタン押下時の処理
         *************************************************************/
        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            setSelectData(Const.SEARCH_TYPE.Product);
            setProductData();
            setOperationKbn();
            calcAll();
        }

        /*************************************************************
         * 取引先検索ボタン押下時の処理
         *************************************************************/
        private void btnSearchSupplier_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(productCode.Text))
            {
                Program.MessageBoxError("商品を選択してください。");
                return;
            }

            setSelectData(Const.SEARCH_TYPE.Supplier);
            setSupplierData();
            setOperationKbn();
            calcAll();
        }

        /*************************************************************
         * 登録ボタン押下時の処理
         *************************************************************/
        private void btnAppend_Click(object sender, EventArgs e)
        {
            string radioText = radioBudget.Checked ? "【予定】" : "【実績】";

            string msg = radioText + "情報を登録しますか？";
            msg = radioBudget.Checked ? string.Concat(msg, Environment.NewLine, "※実績情報にもコピーします。") : msg;

            if (Program.MessageBoxBefore(msg) != DialogResult.Yes)
            {
                return;
            }

            // 念のため各種計算を行う
            calcAll();

            // 入力チェックを行う
            if (!checkInputData())
            {
                return;
            }

            // 登録処理を行う
            using (var context = new CostAccountingEntities())
            {
                executeDelete(context, true);
                executeAppend(context);
                executeAppendSupplier(context);

                // 予定の場合は実績も同データで登録する
                if (radioBudget.Checked)
                {
                    radioActual.CheckedChanged -= new EventHandler(radio_CheckedChanged);
                    radioBudget.CheckedChanged -= new EventHandler(radio_CheckedChanged);

                    radioActual.Checked = true;
                    executeDelete(context, true);
                    executeAppend(context);
                    executeDeleteSupplier(context);
                    executeAppendSupplier(context);
                    radioBudget.Checked = true;

                    radioActual.CheckedChanged += new EventHandler(radio_CheckedChanged);
                    radioBudget.CheckedChanged += new EventHandler(radio_CheckedChanged);
                }
                context.SaveChanges();
            }

            setSupplierData();
            setOperationKbn();

            Logger.Info(Message.INF003, new string[] { this.Text, Message.create(productCode, suppllierCode) + radioText });
            Program.MessageBoxAfter("登録しました。");
        }

        /*************************************************************
         * 修正ボタン押下時の処理
         *************************************************************/
        private void btnChange_Click(object sender, EventArgs e)
        {
            string radioText = radioBudget.Checked ? "【予定】" : "【実績】";

            if (Program.MessageBoxBefore(radioText + "情報を修正しますか？") != DialogResult.Yes)
            {
                return;
            }

            // 念のため各種計算を行う
            calcAll();

            // 入力チェックを行う
            if (!checkInputData())
            {
                return;
            }

            // 削除→登録を行うことで、修正とする
            using (var context = new CostAccountingEntities())
            {
                executeDelete(context, true);
                executeAppend(context);
                executeChangeSupplier(context);
                context.SaveChanges();
            }

            setSupplierData();
            setOperationKbn();

            Logger.Info(Message.INF004, new string[] { this.Text, Message.create(productCode, suppllierCode) + radioText });
            Program.MessageBoxAfter("修正しました。");
        }

        /*************************************************************
         * 削除ボタン押下時の処理
         *************************************************************/
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string radioText = radioBudget.Checked ? "【予定】" : "【実績】";

            if (Program.MessageBoxBefore(radioText + "情報を削除しますか？") != DialogResult.Yes)
            {
                return;
            }

            // 削除処理を行う
            using (var context = new CostAccountingEntities())
            {
                executeDelete(context, false);
                executeDeleteSupplier(context);
                context.SaveChanges();
            }

            setProductData();
            setSupplierData();
            setOperationKbn();

            Logger.Info(Message.INF005, new string[] { this.Text, Message.create(productCode, suppllierCode) + radioText });
            Program.MessageBoxAfter("削除しました。");
        }

        /*************************************************************
         * 取引先の登録処理を行う
         *************************************************************/
        private void executeAppendSupplier(CostAccountingEntities context)
        {
            int category = (int)Program.judgeCategory(radioBudget, radioActual);

            // 取引先データの登録
            var entitySupplier = new ProductSupplier()
            {
                year = Const.TARGET_YEAR,
                product_code = productCode.Text,
                supplier_code = suppllierCode.Text,
                category = category,
                type = (int)Const.PRODUCT_TYPE.Blend,
                unit_price = Conversion.Parse(unitPrice.Text),
                update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName),
                update_date = DateTime.Now,
                del_flg = Const.FLG_OFF
            };
            context.ProductSupplier.Add(entitySupplier);
        }

        /*************************************************************
         * 取引先の修正処理を行う
         *************************************************************/
        private void executeChangeSupplier(CostAccountingEntities context)
        {
            int category = (int)Program.judgeCategory(radioBudget, radioActual);

            var target = from t in context.ProductSupplier
                         where t.year.Equals(Const.TARGET_YEAR)
                            && t.product_code.Equals(productCode.Text)
                            && t.supplier_code.Equals(suppllierCode.Text)
                            && t.category.Equals(category)
                            && t.type.Equals((int)Const.PRODUCT_TYPE.Blend)
                         select t;

            if (target.Count() > decimal.Zero)
            {
                target.First().unit_price = Conversion.Parse(unitPrice.Text);
                target.First().update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName);
                target.First().update_date = DateTime.Now;
            }
        }

        /*************************************************************
         * 取引先の削除処理を行う
         *************************************************************/
        private void executeDeleteSupplier(CostAccountingEntities context)
        {
            int category = (int)Program.judgeCategory(radioBudget, radioActual);

            var target = from t in context.ProductSupplier
                         where t.year.Equals(Const.TARGET_YEAR)
                            && t.product_code.Equals(productCode.Text)
                            && t.supplier_code.Equals(suppllierCode.Text)
                            && t.category.Equals(category)
                            && t.type.Equals((int)Const.PRODUCT_TYPE.Blend)
                         select t;

            context.ProductSupplier.RemoveRange(target);
        }

        /*************************************************************
         * 登録処理を行う
         *************************************************************/
        private void executeAppend(CostAccountingEntities context)
        {
            int category = (int)Program.judgeCategory(radioBudget, radioActual);

            // 商品データの登録
            var entityProduct = new Product()
            {
                year = Const.TARGET_YEAR,
                code = productCode.Text,
                category = category,
                type = (int)Const.PRODUCT_TYPE.Blend,
                item_code = itemCode.SelectedValue.ToString(),
                packing = packing.Text,
                note = note.Text,
                material_cost = Conversion.Parse(materialCost.Text),
                labor_cost = Conversion.Parse(laborCost.Text),
                labor_cost_direct = Conversion.Parse(laborCostDirect.Text),
                labor_cost_indirect = Conversion.Parse(laborCostIndirect.Text),
                contractors_cost = Conversion.Parse(contractorsCost.Text),
                manufacturing_cost = Conversion.Parse(manufacturingCost.Text),
                materials_fare = Conversion.Parse(materialsFare.Text),
                packing_cost = Conversion.Parse(packingCost.Text),
                machine_cost = Conversion.Parse(machineCost.Text),
                utilities_cost = Conversion.Parse(utilitiesCost.Text),
                other_cost = Conversion.Parse(otherCost.Text),
                product_cost = Conversion.Parse(productCost.Text),
                packing_fare = Conversion.Parse(packingFare.Text),
                selling_cost = Conversion.Parse(sellingCost.Text),
                management_cost = Conversion.Parse(managementCost.Text),
                overall_cost = Conversion.Parse(overallCost.Text),
                update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName),
                update_date = DateTime.Now,
                del_flg = Const.FLG_OFF
            };
            context.Product.Add(entityProduct);

            // ブレンドする商品データの登録
            int no = 0;
            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                string code = (string)row.Cells["dgvProductCode"].Value;
                if (!String.IsNullOrEmpty(code))
                {
                    string rateStr = (string)row.Cells["dgvBlendRate"].Value;
                    if (!string.IsNullOrEmpty(rateStr))
                    {
                        rateStr = rateStr.Replace("%", "");
                    }
                    decimal rate = decimal.Divide(Conversion.Parse(rateStr), 100);

                    var entity = new ProductBlend()
                    {
                        year = Const.TARGET_YEAR,
                        product_code = productCode.Text,
                        category = category,
                        no = no++,
                        code = code,
                        blend_rate = rate,
                        update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName),
                        update_date = DateTime.Now,
                        del_flg = Const.FLG_OFF
                    };
                    context.ProductBlend.Add(entity);
                }
            }
        }

        /*************************************************************
         * 削除処理を行う
         *************************************************************/
        private void executeDelete(CostAccountingEntities context, bool delProduct)
        {
            int category = (int)Program.judgeCategory(radioBudget, radioActual);

            if (!delProduct)
            {
                // 同じ商品に紐づく取引先があるかチェックし、存在する場合は商品データを削除しない
                var supplierOther = from t in context.ProductSupplier
                                    where t.year.Equals(Const.TARGET_YEAR)
                                       && t.product_code.Equals(productCode.Text)
                                       && t.category.Equals(category)
                                       && t.type.Equals((int)Const.PRODUCT_TYPE.Blend)
                                    select t;

                if (supplierOther.Count() > decimal.One)
                {
                    return;
                }
            }

            // 商品データ
            var product = from t in context.Product
                          where t.code.Equals(productCode.Text)
                             && t.year.Equals(Const.TARGET_YEAR)
                             && t.category.Equals(category)
                             && t.type.Equals((int)Const.PRODUCT_TYPE.Blend)
                          select t;
            context.Product.RemoveRange(product);

            // ブレンドする商品データ
            var blend = from t in context.ProductBlend
                        where t.year.Equals(Const.TARGET_YEAR)
                           && t.product_code.Equals(productCode.Text)
                           && t.category.Equals(category)
                        select t;
            context.ProductBlend.RemoveRange(blend);
        }

        /*************************************************************
         * 入力チェックの処理
         *************************************************************/
        private bool checkInputData()
        {
            bool ret = true;


            return ret;
        }

        /*************************************************************
         * 原価計算の処理
         *************************************************************/
        private void calcAll()
        {
            unitPrice.Text = Conversion.Parse(unitPrice.Text).ToString("N");

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++原価計算
            decimal materialCostSum = decimal.Zero;
            decimal contractorsCostSum = decimal.Zero;
            decimal laborCostSum = decimal.Zero;
            decimal laborCostDirectSum = decimal.Zero;
            decimal laborCostIndirectSum = decimal.Zero;
            decimal manufacturingCostSum = decimal.Zero;
            decimal materialsFareSum = decimal.Zero;
            decimal packingCostSum = decimal.Zero;
            decimal machineCostSum = decimal.Zero;
            decimal utilitiesCostSum = decimal.Zero;
            decimal otherCostSum = decimal.Zero;
            decimal productCostSum = decimal.Zero;
            decimal packingFareSum = decimal.Zero;
            decimal sellingCostSum = decimal.Zero;
            decimal managementCostSum = decimal.Zero;
            decimal overallCostSum = decimal.Zero;
            decimal rateTotal = decimal.Zero;

            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                string code = (string)row.Cells["dgvProductCode"].Value;
                if (!String.IsNullOrEmpty(code))
                {
                    string rateStr = (string)row.Cells["dgvBlendRate"].Value;
                    if (!string.IsNullOrEmpty(rateStr))
                    {
                        rateStr = rateStr.Replace("%", "");
                    }
                    decimal rate = decimal.Divide(Conversion.Parse(rateStr), 100);

                    materialCostSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvMaterialCost"].Value), rate);
                    contractorsCostSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvContractorsCost"].Value), rate);
                    laborCostSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvLaborCost"].Value), rate);
                    laborCostDirectSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvLaborCostDirect"].Value), rate);
                    laborCostIndirectSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvLaborCostIndirect"].Value), rate);
                    manufacturingCostSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvManufacturingCost"].Value), rate);
                    materialsFareSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvMaterialsFare"].Value), rate);
                    packingCostSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvPackingCost"].Value), rate);
                    machineCostSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvMachineCost"].Value), rate);
                    utilitiesCostSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvUtilitiesCost"].Value), rate);
                    otherCostSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvOtherCost"].Value), rate);
                    productCostSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvProductCost"].Value), rate);
                    packingFareSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvPackingFare"].Value), rate);
                    sellingCostSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvSellingCost"].Value), rate);
                    managementCostSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvManagementCost"].Value), rate);
                    overallCostSum += decimal.Multiply(Conversion.Parse((string)row.Cells["dgvOverallCost"].Value), rate);

                    rateTotal += rate;
                    row.Cells["dgvBlendRate"].Value = rate.ToString("P");
                }
            }
            blendRateTotal.Text = rateTotal.ToString("P");

            materialCost.Text = materialCostSum.ToString("N");
            contractorsCost.Text = contractorsCostSum.ToString("N");
            laborCost.Text = laborCostSum.ToString("N");
            laborCostDirect.Text = laborCostDirectSum.ToString("N");
            laborCostIndirect.Text = laborCostIndirectSum.ToString("N");
            manufacturingCost.Text = manufacturingCostSum.ToString("N");
            materialsFare.Text = materialsFareSum.ToString("N");
            packingCost.Text = packingCostSum.ToString("N");
            machineCost.Text = machineCostSum.ToString("N");
            utilitiesCost.Text = utilitiesCostSum.ToString("N");
            otherCost.Text = otherCostSum.ToString("N");
            productCost.Text = productCostSum.ToString("N");
            packingFare.Text = packingFareSum.ToString("N");
            sellingCost.Text = sellingCostSum.ToString("N");
            managementCost.Text = managementCostSum.ToString("N");
            overallCost.Text = overallCostSum.ToString("N");

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++販売価格体系
            // 販売価格
            sellingPrice.Text = Conversion.Parse(unitPrice.Text).ToString("N");

            // 総原価率
            if (Conversion.Parse(sellingPrice.Text) != decimal.Zero)
            {
                totalCostRate.Text = decimal.Divide(Conversion.Parse(overallCost.Text), Conversion.Parse(sellingPrice.Text)).ToString("P");
            }
            else
            {
                totalCostRate.Text = decimal.Zero.ToString("P");
            }

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++経理計算
            // 実質売価
            realSellingPrice.Text = Conversion.Parse(unitPrice.Text).ToString("N");

            // 利益
            profit.Text = decimal.Subtract(Conversion.Parse(realSellingPrice.Text), Conversion.Parse(overallCost.Text)).ToString("N");

            // 粗利益
            grossProfit.Text = decimal.Subtract(Conversion.Parse(realSellingPrice.Text), Conversion.Parse(productCost.Text)).ToString("N");

            // 限界利益
            Const.CATEGORY_TYPE category = Program.judgeCategory(radioBudget, radioActual);
            decimal rateExpend = decimal.Divide(Parameters.getInstance(category).rateExpend, 100);
            marginalProfit.Text = decimal.Subtract(Conversion.Parse(realSellingPrice.Text)
                                                   , (Conversion.Parse(materialCost.Text) + Conversion.Parse(contractorsCost.Text)
                                                      + Conversion.Parse(laborCostDirect.Text) + Conversion.Parse(materialsFare.Text)
                                                      + Conversion.Parse(packingCost.Text) + Conversion.Parse(utilitiesCost.Text)
                                                      + decimal.Multiply(Conversion.Parse(otherCost.Text), rateExpend)
                                                      + Conversion.Parse(packingFare.Text))).ToString("N");

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++経理計算(割合)
            decimal realSellingPriceNum = Conversion.Parse(realSellingPrice.Text);
            if (realSellingPriceNum > decimal.Zero)
            {
                // 実質売価
                realSellingPriceRatio.Text = decimal.Divide(Conversion.Parse(realSellingPrice.Text), realSellingPriceNum).ToString("P");

                // 利益
                profitRatio.Text = decimal.Divide(Conversion.Parse(profit.Text), realSellingPriceNum).ToString("P");

                // 粗利益
                grossProfitRatio.Text = decimal.Divide(Conversion.Parse(grossProfit.Text), realSellingPriceNum).ToString("P");

                // 限界利益
                marginalProfitRatio.Text = decimal.Divide(Conversion.Parse(marginalProfit.Text), realSellingPriceNum).ToString("P");
            }
            else
            {
                realSellingPriceRatio.Text = decimal.Zero.ToString("P");
                profitRatio.Text = decimal.Zero.ToString("P");
                grossProfitRatio.Text = decimal.Zero.ToString("P");
                marginalProfitRatio.Text = decimal.Zero.ToString("P");
            }
        }

        /*************************************************************
         * データグリッドビューの編集中のイベント追加
         *************************************************************/
        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            // テキストボックスのイベント追加
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                dgvTextBox = (DataGridViewTextBoxEditingControl)e.Control;
                dgvTextBox.Leave += new EventHandler(control_Leave_calc);
                dgvTextBox.KeyPress += new KeyPressEventHandler(Event.textBox_KeyPress_numeric);
            }
        }

        /*************************************************************
         * データグリッドビューの編集後のイベント削除
         *************************************************************/
        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // テキストボックスのイベント削除
            if (dgvTextBox != null)
            {
                dgvTextBox.KeyPress -= new KeyPressEventHandler(Event.textBox_KeyPress_numeric);
                dgvTextBox.Leave -= new EventHandler(control_Leave_calc);
                dgvTextBox = null;
            }
        }

        /*************************************************************
         * データグリッドビューのクリック時のイベント処理
         *************************************************************/
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex > decimal.MinusOne && e.RowIndex > decimal.MinusOne)
            {
                // ブレンド割合のテキストボックス
                if (dgv.Columns[e.ColumnIndex] is DataGridViewTextBoxColumn)
                {
                    SendKeys.Send("{F2}");
                }

                // 商品検索の処理
                if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                {
                    Form_Common_SelectData form = new Form_Common_SelectData(Const.SEARCH_TYPE.Product);
                    form.ShowDialog();
                    if (form.DialogResult == DialogResult.OK)
                    {
                        string code = (string)form.dataGridView.SelectedRows[0].Cells[0].Value;
                        setProductData(code, e.RowIndex);

                        if (e.RowIndex == dgvProduct.NewRowIndex)
                        {
                            dgvProduct.BeginEdit(false);
                            dgvProduct.CurrentCell = dgvProduct[3, dgvProduct.NewRowIndex];
                            dgvProduct.NotifyCurrentCellDirty(true);
                        }
                    }
                    calcAll();
                }
            }
        }

        /*************************************************************
         * データグリッドビューのコンボボックスをワンクリックで編集可能とする
         *************************************************************/
        private void dataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex > decimal.MinusOne)
            {
                if (dgv.Columns[e.ColumnIndex] is DataGridViewTextBoxColumn)
                {
                    dgv.BeginEdit(true);
                }
            }
        }

        /*************************************************************
         * 商品検索／取引先検索の検索データを設定する
         *************************************************************/
        private void setSelectData(Const.SEARCH_TYPE type)
        {
            Form_Common_SelectData form = new Form_Common_SelectData(type);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                string code = (string)form.dataGridView.SelectedRows[0].Cells[0].Value;
                string name = (string)form.dataGridView.SelectedRows[0].Cells[1].Value;
                string note = (string)form.dataGridView.SelectedRows[0].Cells[2].Value;
                string unit = (string)form.dataGridView.SelectedRows[0].Cells[3].Value;
                switch (type)
                {
                    case Const.SEARCH_TYPE.Product:
                        productCode.Text = code;
                        productName.Text = name;
                        saleUnit.Text = unit;
                        break;
                    case Const.SEARCH_TYPE.Supplier:
                        suppllierCode.Text = code;
                        suppllierName.Text = name;
                        break;
                }
            }
            form.Dispose();
            setOperationKbn();
        }

        /*************************************************************
         * ブレンド品データを設定する
         *************************************************************/
        private void setProductData()
        {
            using (var context = new CostAccountingEntities())
            {
                int category = (int)Program.judgeCategory(radioBudget, radioActual);

                var product = from t in context.Product
                              where t.code.Equals(productCode.Text)
                                 && t.year.Equals(Const.TARGET_YEAR)
                                 && t.category.Equals(category)
                                 && t.type.Equals((int)Const.PRODUCT_TYPE.Blend)
                              select t;

                if (product.Count() == decimal.One)
                {
                    // 商品データの設定
                    itemCode.SelectedValue = product.First().item_code;
                    packing.Text = product.First().packing;
                    note.Text = product.First().note;

                    // 取引先データの設定
                    setSupplierData();

                    // データグリッドビューの設定
                    List<ProductBlend> blend = (from t in context.ProductBlend
                                                where t.year.Equals(Const.TARGET_YEAR)
                                                   && t.product_code.Equals(productCode.Text)
                                                   && t.category.Equals(category)
                                                orderby t.no
                                                select t).ToList();
                    dgvProduct.RowCount = blend.Count() + 1;
                    for (int i = 0; i < blend.Count(); i++)
                    {
                        dgvProduct.Rows[i].Cells["dgvBlendRate"].Value = blend[i].blend_rate.ToString("P");
                        setProductData(blend[i].code, i);
                    }
                }
                else
                {
                    // データクリア
                    itemCode.SelectedIndex = 0;
                    unitPrice.Text = string.Empty;
                    updateTime.Text = string.Empty;
                    updatePC.Text = string.Empty;
                    note.Text = string.Empty;
                    dgvProduct.Rows.Clear();
                }
            }
        }

        /*************************************************************
         * データグリッドビューに商品データを設定する
         *************************************************************/
        private void setProductData(string productCode, int rowIndex)
        {
            using (var context = new CostAccountingEntities())
            {
                int category = (int)Program.judgeCategory(radioBudget, radioActual);

                var product = from t_product in context.Product
                              join m_product in context.ProductCode on
                                   new { t_product.code, t_product.year } equals new { m_product.code, m_product.year }
                              where t_product.code.Equals(productCode)
                                 && t_product.year.Equals(Const.TARGET_YEAR)
                                 && t_product.category.Equals(category)
                                 && t_product.type.Equals((int)Const.PRODUCT_TYPE.Normal)
                              select new { t = t_product, m_product.name };

                if (product.Count() == decimal.One)
                {
                    dgvProduct.Rows[rowIndex].Cells["dgvProductCode"].Value = product.First().t.code;
                    dgvProduct.Rows[rowIndex].Cells["dgvProductName"].Value = product.First().name;
                    dgvProduct.Rows[rowIndex].Cells["dgvProductCode"].Style.BackColor = Color.White;
                    dgvProduct.Rows[rowIndex].Cells["dgvProductName"].Style.BackColor = Color.White;

                    dgvProduct.Rows[rowIndex].Cells["dgvMaterialCost"].Value = product.First().t.material_cost.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvContractorsCost"].Value = product.First().t.contractors_cost.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvLaborCost"].Value = product.First().t.labor_cost.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvLaborCostDirect"].Value = product.First().t.labor_cost_direct.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvLaborCostIndirect"].Value = product.First().t.labor_cost_indirect.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvManufacturingCost"].Value = product.First().t.manufacturing_cost.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvMaterialsFare"].Value = product.First().t.materials_fare.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvPackingCost"].Value = product.First().t.packing_cost.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvMachineCost"].Value = product.First().t.machine_cost.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvUtilitiesCost"].Value = product.First().t.utilities_cost.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvOtherCost"].Value = product.First().t.other_cost.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvProductCost"].Value = product.First().t.product_cost.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvPackingFare"].Value = product.First().t.packing_fare.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvSellingCost"].Value = product.First().t.selling_cost.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvManagementCost"].Value = product.First().t.management_cost.ToString("N");
                    dgvProduct.Rows[rowIndex].Cells["dgvOverallCost"].Value = product.First().t.overall_cost.ToString("N");
                }
                else
                {
                    dgvProduct.Rows[rowIndex].Cells["dgvProductCode"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvProductName"].Value = "商品登録されていません";
                    dgvProduct.Rows[rowIndex].Cells["dgvProductCode"].Style.BackColor = Color.Red;
                    dgvProduct.Rows[rowIndex].Cells["dgvProductName"].Style.BackColor = Color.Red;

                    dgvProduct.Rows[rowIndex].Cells["dgvMaterialCost"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvContractorsCost"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvLaborCost"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvLaborCostDirect"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvLaborCostIndirect"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvManufacturingCost"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvMaterialsFare"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvPackingCost"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvMachineCost"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvUtilitiesCost"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvOtherCost"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvProductCost"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvPackingFare"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvSellingCost"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvManagementCost"].Value = string.Empty;
                    dgvProduct.Rows[rowIndex].Cells["dgvOverallCost"].Value = string.Empty;
                }
            }
        }

        /*************************************************************
         * 検索した取引先データを設定する
         *************************************************************/
        private void setSupplierData()
        {
            using (var context = new CostAccountingEntities())
            {
                int category = (int)Program.judgeCategory(radioBudget, radioActual);

                var supplier = from t in context.ProductSupplier
                               where t.year.Equals(Const.TARGET_YEAR)
                                  && t.product_code.Equals(productCode.Text)
                                  && t.supplier_code.Equals(suppllierCode.Text)
                                  && t.category.Equals(category)
                                  && t.type.Equals((int)Const.PRODUCT_TYPE.Blend)
                               select t;

                if (supplier.Count() == decimal.One)
                {
                    unitPrice.Text = supplier.First().unit_price.ToString("N");
                    updateTime.Text = supplier.First().update_date.ToString();
                    updatePC.Text = supplier.First().update_user;
                }
                else
                {
                    unitPrice.Text = String.Empty;
                    updateTime.Text = String.Empty;
                    updatePC.Text = String.Empty;
                }
            }
        }

        /*************************************************************
         * 処理区分の設定を行う
         *************************************************************/
        private void setOperationKbn()
        {
            if (!string.IsNullOrEmpty(productCode.Text) && !string.IsNullOrEmpty(suppllierCode.Text))
            {
                using (var context = new CostAccountingEntities())
                {
                    int category = (int)Program.judgeCategory(radioBudget, radioActual);

                    var target = from t_product in context.Product
                                 join t_supplier in context.ProductSupplier on
                                      new { t_product.year, t_product.code, t_product.category, t_product.type }
                                        equals
                                      new { t_supplier.year, code = t_supplier.product_code, t_supplier.category, t_supplier.type }
                                 where t_product.year.Equals(Const.TARGET_YEAR)
                                    && t_product.code.Equals(productCode.Text)
                                    && t_supplier.supplier_code.Equals(suppllierCode.Text)
                                    && t_product.category.Equals(category)
                                    && t_product.type.Equals((int)Const.PRODUCT_TYPE.Blend)
                                 select t_product;

                    if (target.Count() > decimal.Zero)
                    {
                        operationKbn.Text = "修正";
                        btnAppend.Enabled = false;
                        btnChange.Enabled = true;
                        btnDelete.Enabled = true;
                    }
                    else
                    {
                        operationKbn.Text = "登録";
                        btnAppend.Enabled = true;
                        btnChange.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                }
            }
            else
            {
                operationKbn.Text = "商品と取引先を選択";
                btnAppend.Enabled = false;
                btnChange.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        /*************************************************************
         * テキストボックス編集と同時に原価計算を行う
         *************************************************************/
        private void control_Leave_calc(object sender, EventArgs e)
        {
            calcAll();
        }

        /*************************************************************
         * テキストボックスにて数値のみ入力可能にする
         *************************************************************/
        private void textBox_KeyPress_numeric(object sender, KeyPressEventArgs e)
        {
            Event.textBox_KeyPress_numeric(sender, e);
        }

    }
}
