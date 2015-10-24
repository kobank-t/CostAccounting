using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CostAccounting
{
    public partial class Form_ProductMng_ProductReg : Form
    {
        // データグリッドビューのコンボボックスのコントロール
        private DataGridViewComboBoxEditingControl dgvComboBox = null;

        // データグリッドビューのテキストボックスのコントロール
        private DataGridViewTextBoxEditingControl dgvTextBox = null;

        private static string msgTemplate1 = "ロス[x]%加算";

        private static string msgTemplate2 = "製造原価の[x]%";

        private static string msgTemplate3 = string.Concat(
                                            "製造原価の一般管理費[x]%", Environment.NewLine, "+営業外費[y]%");

        public Form_ProductMng_ProductReg()
        {
            InitializeComponent();
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_ProductMng_ProductReg_Load(object sender, EventArgs e)
        {
            // 種目コードのコンボボックス生成
            DataTable itemTable = DataTableSupport.item;
            itemCode.DataSource = itemTable;
            itemCode.ValueMember = itemTable.Columns[0].Caption;
            itemCode.DisplayMember = itemTable.Columns[1].Caption;

            // コンボボックスなどの価格やレートを設定する
            Const.CATEGORY_TYPE category = Program.judgeCategory(radioBudget, radioActual);
            changeRateValue(category);

            // データグリッドビューの合計行を設定する
            initDgvTotalRow(dgvMaterialCostTotal);
            initDgvTotalRow(dgvContractorsTotal);
            initDgvTotalRow(dgvMaterialsFareTotal);
            initDgvTotalRow(dgvPackingTotal);
            initDgvTotalRow(dgvMachineTotal);
            initDgvMachineRow();
            initDgvTotalRow(dgvPackingFareTotal);

            // 処理区分の設定
            setOperationKbn();

            // 初期設定での原価計算
            calcAll();
        }

        /*************************************************************
         * コンボボックスなどの価格やレートの予算／実績を切り替える
         *************************************************************/
        private void changeRateValue(Const.CATEGORY_TYPE category)
        {
            // 原料費タブの設定
            initDgvComboBox(dgvMaterialCostName, DataTableSupport.getInstance(category).rowMaterial, "rowMaterialTable");

            // 製造経費－包装資材費タブの設定
            initDgvComboBox(dgvPackingName, DataTableSupport.getInstance(category).material, "materialTable");

            // 製造経費－設備費タブの設定
            initDgvComboBox(dgvMachineName, DataTableSupport.getInstance(category).machine, "machineTable");

            // 製造経費－荷造運賃タブの設定
            initDgvComboBox(dgvPackingFareName, DataTableSupport.getInstance(category).fare, "fareTable");

            // 労務費タブの設定
            wageRateM_direct.Text = Parameters.getInstance(category).wageM.ToString("N");
            wageRateF.Text = Parameters.getInstance(category).wageF.ToString("N");
            wageRateM_indirect.Text = Parameters.getInstance(category).wageIndirect.ToString("N");

            // 製造経費－水道光熱費タブの設定
            fuelAmountPerHourB.Text = Parameters.getInstance(category).utilitiesAD.ToString("N");
            fuelAmountPerHourA.Text = Parameters.getInstance(category).utilitiesFD.ToString("N");

            // 製造経費－その他経費タブの設定
            fuelAmountPerHourFD.Text = Parameters.getInstance(category).allocationFD.ToString("N");
            fuelAmountPerHourAD.Text = Parameters.getInstance(category).allocationAD.ToString("N");
            fuelAmountPerHourLabor.Text = Parameters.getInstance(category).allocationLabor.ToString("N");

            // 算出式を設定
            message1.Text = msgTemplate1.Replace("[x]", Parameters.getInstance(category).rateLoss.ToString());
            message2.Text = msgTemplate2.Replace("[x]", Parameters.getInstance(category).allocationSale.ToString());
            message3.Text = msgTemplate3.Replace("[x]", Parameters.getInstance(category).allocationMng.ToString())
                                        .Replace("[y]", Parameters.getInstance(category).allocationExt.ToString());
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

            // コンボボックスなどの価格やレートの予算／実績を切り替える
            Const.CATEGORY_TYPE category = Program.judgeCategory(radioBudget, radioActual);
            changeRateValue(category);

            // 原料費タブの原価を再設定
            foreach (DataGridViewRow row in dgvMaterialCost.Rows)
            {
                string code = (string)row.Cells["dgvMaterialCostName"].Value;
                if (!String.IsNullOrEmpty(code))
                    row.Cells["dgvMaterialCostPrice"].Value
                        = DataTableSupport.getPrice((DataTable)dgvMaterialCostName.DataSource, code).ToString("N");
            }

            // 製造経費－包装資材費タブの単価を再設定
            foreach (DataGridViewRow row in dgvPacking.Rows)
            {
                string code = (string)row.Cells["dgvPackingName"].Value;
                if (!String.IsNullOrEmpty(code))
                    row.Cells["dgvPackingCost"].Value
                        = DataTableSupport.getPrice((DataTable)dgvPackingName.DataSource, code).ToString("N");
            }

            // 製造経費－設備費タブのレートを再設定
            foreach (DataGridViewRow row in dgvMachine.Rows)
            {
                string code = (string)row.Cells["dgvMachineName"].Value;
                if (!String.IsNullOrEmpty(code))
                    row.Cells["dgvMachineRate"].Value
                        = DataTableSupport.getPrice((DataTable)dgvMachineName.DataSource, code).ToString("N");
            }

            // 製造経費－荷造運賃タブのレートを再設定
            foreach (DataGridViewRow row in dgvPackingFare.Rows)
            {
                string code = (string)row.Cells["dgvPackingFareName"].Value;
                if (!String.IsNullOrEmpty(code))
                    row.Cells["dgvPackingFareCost"].Value
                        = DataTableSupport.getPrice((DataTable)dgvPackingFareName.DataSource, code).ToString("N");
            }

            // 商品データと取引先データの再設定
            if (!string.IsNullOrEmpty(productCode.Text)
                && Program.MessageBoxBefore(productName.Text + "の" + radio.Text + "情報に切り替えますか？") == DialogResult.Yes)
            {
                setProductData();
                setSupplierData();
            }
            setOperationKbn();

            // 設定内容で再計算を行う
            calcAll();
        }

        /*************************************************************
         * データグリッドビューのコンボボックス初期設定
         *************************************************************/
        private void initDgvComboBox(DataGridViewComboBoxColumn target, DataTable table, string tableName)
        {
            table.TableName = tableName;
            target.DataSource = table;
            target.ValueMember = table.Columns[0].Caption;
            target.DisplayMember = table.Columns[1].Caption;
        }

        /*************************************************************
         * データグリッドビュー合計行の初期設定
         *************************************************************/
        private void initDgvTotalRow(DataGridView target)
        {
            target.RowCount = 1;
            target.Rows[0].Cells[0].Value = "合計";
            target.Rows[0].Frozen = true;
            target.ColumnHeadersVisible = false;
        }

        /*************************************************************
         * データグリッドビュー設備費タブの初期設定
         *************************************************************/
        private void initDgvMachineRow()
        {
            if (DataTableSupport.containsKey((DataTable)dgvMachineName.DataSource, "A", "B"))
            {
                dgvMachine.RowCount = 3;
                string[] initCode = { "A", "B" };
                for (int i = 0; i < initCode.Length; i++)
                {
                    dgvMachine.Rows[i].Cells["dgvMachineName"].Value = initCode[i];
                    dgvMachine.Rows[i].Cells["dgvMachineRate"].Value = DataTableSupport.getPrice((DataTable)dgvMachineName.DataSource, initCode[i]).ToString("N");
                    dgvMachine.Rows[i].Cells["dgvMachineName"].Style.BackColor = Color.White;
                    dgvMachine.Rows[i].Cells["dgvMachineName"].ReadOnly = true;
                }

                trayLabel.Text = DataTableSupport.getName((DataTable)dgvMachineName.DataSource, initCode[0]) + " トレー枚数：";
            }
            else
            {
                Program.MessageBoxError("マシンの初期登録が未実施です。");
                return;
            }
        }

        /*************************************************************
         * データグリッドビューの編集中のイベント追加
         *************************************************************/
        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            // コンボックスのイベント追加
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                if (dgv.CurrentCell.OwningColumn.Name == "dgvMaterialCostName"
                    || dgv.CurrentCell.OwningColumn.Name == "dgvPackingName"
                    || dgv.CurrentCell.OwningColumn.Name == "dgvMachineName"
                    || dgv.CurrentCell.OwningColumn.Name == "dgvPackingFareName")
                {
                    dgvComboBox = (DataGridViewComboBoxEditingControl)e.Control;
                    dgvComboBox.SelectedIndexChanged += new EventHandler(dgvComboBox_SelectedIndexChanged);
                    dgvComboBox.Leave += new EventHandler(control_Leave_calc);
                }
            }
            // テキストボックスのイベント追加
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                dgvTextBox = (DataGridViewTextBoxEditingControl)e.Control;
                dgvTextBox.Leave += new EventHandler(control_Leave_calc);

                if (dgv.CurrentCell.OwningColumn.Name == "dgvMaterialCostQuantity"
                    || dgv.CurrentCell.OwningColumn.Name == "dgvContractorsQuantity"
                    || dgv.CurrentCell.OwningColumn.Name == "dgvContractorsCost"
                    || dgv.CurrentCell.OwningColumn.Name == "dgvMaterialsFareQuantity"
                    || dgv.CurrentCell.OwningColumn.Name == "dgvMaterialsFareCost"
                    || dgv.CurrentCell.OwningColumn.Name == "dgvPackingQuantity"
                    || dgv.CurrentCell.OwningColumn.Name == "dgvMachineTime"
                    || dgv.CurrentCell.OwningColumn.Name == "dgvPackingFareQuantity")
                {
                    dgvTextBox.KeyPress += new KeyPressEventHandler(Event.textBox_KeyPress_numeric);
                }
            }
        }

        /*************************************************************
         * データグリッドビューの編集後のイベント削除
         *************************************************************/
        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // コンボックスのイベント削除
            if (dgvComboBox != null)
            {
                dgvComboBox.SelectedIndexChanged -= new EventHandler(dgvComboBox_SelectedIndexChanged);
                dgvComboBox.Leave -= new EventHandler(control_Leave_calc);
                dgvComboBox = null;
            }
            // テキストボックスのイベント削除
            if (dgvTextBox != null)
            {
                dgvTextBox.KeyPress -= new KeyPressEventHandler(Event.textBox_KeyPress_numeric);
                dgvTextBox.Leave -= new EventHandler(control_Leave_calc);
                dgvTextBox = null;
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
                if (dgv.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
                {
                    SendKeys.Send("{F4}");
                }
                if (dgv.Columns[e.ColumnIndex] is DataGridViewTextBoxColumn)
                {
                    dgv.BeginEdit(true);
                }
            }
        }

        /*************************************************************
         * データグリッドビューのテキストをワンクリックで編集可能とする
         *************************************************************/
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (e.ColumnIndex > decimal.MinusOne)
            {
                if (dgv.Columns[e.ColumnIndex] is DataGridViewTextBoxColumn)
                {
                    SendKeys.Send("{F2}");
                }
            }
        }

        /*************************************************************
         * データグリッドビューのコンボボックスの選択項目に応じた処理
         *************************************************************/
        private void dgvComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataGridViewComboBoxEditingControl cb = (DataGridViewComboBoxEditingControl)sender;

            // 原料費タブの原料名に対応する原価を原価列に設定
            if (cb.DataSource.ToString() == "rowMaterialTable")
            {
                decimal cost = (decimal)((DataRowView)cb.SelectedItem)["price"];
                dgvMaterialCost.SelectedRows[0].Cells["dgvMaterialCostPrice"].Value = cost.ToString("N");
            }

            // 製造経費－包装資材費タブの資材名に対応する原価を単価列に設定
            if (cb.DataSource.ToString() == "materialTable")
            {
                decimal cost = (decimal)((DataRowView)cb.SelectedItem)["price"];
                dgvPacking.SelectedRows[0].Cells["dgvPackingCost"].Value = cost.ToString("N");
            }

            // 製造経費－設備費タブの設備名に対応するレートをレート列に設定
            if (cb.DataSource.ToString() == "machineTable")
            {
                decimal rate = (decimal)((DataRowView)cb.SelectedItem)["price"];
                dgvMachine.SelectedRows[0].Cells["dgvMachineRate"].Value = rate.ToString("N");
            }

            // 製造経費－荷造運賃タブの設備名に対応する単価を単価列に設定
            if (cb.DataSource.ToString() == "fareTable")
            {
                decimal price = (decimal)((DataRowView)cb.SelectedItem)["price"];
                dgvPackingFare.SelectedRows[0].Cells["dgvPackingFareCost"].Value = price.ToString("N");
            }
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
                               select t;

                if (supplier.Count() == decimal.One)
                {
                    unitPrice.Text = supplier.First().unit_price.ToString();
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
         * 検索した商品データを設定する
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
                              select t;

                if (product.Count() == decimal.One)
                {
                    // 商品データの設定
                    itemCode.SelectedValue = product.First().item_code;
                    volume.Text = product.First().volume.ToString();
                    packing.Text = product.First().packing;
                    note.Text = product.First().note;
                    preprocessTimeM.Text = product.First().preprocess_time_m.ToString();
                    preprocessTimeF.Text = product.First().preprocess_time_f.ToString();
                    nightTimeM_indirect.Text = product.First().night_time_m.ToString();
                    nightTimeF.Text = product.First().night_time_f.ToString();
                    dryTimeM.Text = product.First().dry_time_m.ToString();
                    dryTimeF.Text = product.First().dry_time_f.ToString();
                    selectionTimeM.Text = product.First().selection_time_m.ToString();
                    selectionTimeF.Text = product.First().selection_time_f.ToString();
                    trayNum.Text = product.First().tray_num.ToString();

                    // 取引先データの設定
                    var supplier = from t in context.ProductSupplier
                                   where t.year.Equals(Const.TARGET_YEAR)
                                      && t.product_code.Equals(productCode.Text)
                                      && t.supplier_code.Equals(suppllierCode.Text)
                                      && t.category.Equals(category)
                                   select t;

                    if (supplier.Count() == decimal.One)
                    {
                        unitPrice.Text = supplier.First().unit_price.ToString();
                        updateTime.Text = supplier.First().update_date.ToString();
                        updatePC.Text = supplier.First().update_user;
                    }
                    else
                    {
                        unitPrice.Text = String.Empty;
                        updateTime.Text = String.Empty;
                        updatePC.Text = String.Empty;
                    }

                    // 関連テーブルの主キー（年＋商品コード＋取引先コード＋予定／実績）を生成
                    string id = string.Concat(Const.TARGET_YEAR, productCode.Text, category);

                    // 原料費データ
                    List<ProductMaterial> material = (from t in context.ProductMaterial
                                                      where t.id.Equals(id)
                                                      select t).ToList();
                    dgvMaterialCost.RowCount = material.Count() + 1;
                    for (int i = 0; i < material.Count(); i++)
                    {
                        dgvMaterialCost.Rows[i].Cells["dgvMaterialCostName"].Value = material[i].code;
                        dgvMaterialCost.Rows[i].Cells["dgvMaterialCostQuantity"].Value = material[i].quantity.ToString();
                        dgvMaterialCost.Rows[i].Cells["dgvMaterialCostPrice"].Value = DataTableSupport.getPrice((DataTable)dgvMaterialCostName.DataSource, material[i].code).ToString("N");
                    }

                    // 外注費データ
                    List<ProductContractor> contractor = (from t in context.ProductContractor
                                                          where t.id.Equals(id)
                                                          select t).ToList();
                    dgvContractors.RowCount = contractor.Count() + 1;
                    for (int i = 0; i < contractor.Count(); i++)
                    {
                        dgvContractors.Rows[i].Cells["dgvContractorsName"].Value = contractor[i].name;
                        dgvContractors.Rows[i].Cells["dgvContractorsQuantity"].Value = contractor[i].quantity.ToString();
                        dgvContractors.Rows[i].Cells["dgvContractorsCost"].Value = contractor[i].cost.ToString();
                    }

                    // 製造経費－原料運賃データ
                    List<ProductMaterialsFare> materialFare = (from t in context.ProductMaterialsFare
                                                               where t.id.Equals(id)
                                                               select t).ToList();
                    dgvMaterialsFare.RowCount = materialFare.Count() + 1;
                    for (int i = 0; i < materialFare.Count(); i++)
                    {
                        dgvMaterialsFare.Rows[i].Cells["dgvMaterialsFareName"].Value = materialFare[i].name;
                        dgvMaterialsFare.Rows[i].Cells["dgvMaterialsFareQuantity"].Value = materialFare[i].quantity.ToString();
                        dgvMaterialsFare.Rows[i].Cells["dgvMaterialsFareCost"].Value = materialFare[i].cost.ToString();
                    }

                    // 製造経費－包装資材費データ
                    List<ProductPacking> packingList = (from t in context.ProductPacking
                                                        where t.id.Equals(id)
                                                        select t).ToList();
                    dgvPacking.RowCount = packingList.Count() + 1;
                    for (int i = 0; i < packingList.Count(); i++)
                    {
                        dgvPacking.Rows[i].Cells["dgvPackingName"].Value = packingList[i].code;
                        dgvPacking.Rows[i].Cells["dgvPackingQuantity"].Value = packingList[i].quantity.ToString();
                        dgvPacking.Rows[i].Cells["dgvPackingCost"].Value = DataTableSupport.getPrice((DataTable)dgvPackingName.DataSource, packingList[i].code).ToString("N");
                    }


                    // 製造経費－設備費データ
                    List<ProductMachine> machine = (from t in context.ProductMachine
                                                    where t.id.Equals(id)
                                                    select t).ToList();
                    dgvMachine.RowCount = machine.Count() + 1;
                    for (int i = 2; i < machine.Count(); i++)
                    {
                        dgvMachine.Rows[i].Cells["dgvMachineName"].Value = machine[i].code;
                        dgvMachine.Rows[i].Cells["dgvMachineTime"].Value = machine[i].time.ToString();
                        dgvMachine.Rows[i].Cells["dgvMachineRate"].Value = DataTableSupport.getPrice((DataTable)dgvMachineName.DataSource, machine[i].code).ToString("N");
                    }

                    // 製造経費－荷造運賃データ
                    List<ProductPackingFare> packingFare = (from t in context.ProductPackingFare
                                                            where t.id.Equals(id)
                                                            select t).ToList();
                    dgvPackingFare.RowCount = packingFare.Count() + 1;
                    for (int i = 0; i < packingFare.Count(); i++)
                    {
                        dgvPackingFare.Rows[i].Cells["dgvPackingFareName"].Value = packingFare[i].code;
                        dgvPackingFare.Rows[i].Cells["dgvPackingFareQuantity"].Value = packingFare[i].quantity.ToString();
                        dgvPackingFare.Rows[i].Cells["dgvPackingFareCost"].Value = DataTableSupport.getPrice((DataTable)dgvPackingFareName.DataSource, packingFare[i].code).ToString("N");
                    }
                }
                else
                {
                    // データクリア
                    itemCode.SelectedIndex = 0;
                    volume.Text = string.Empty;
                    trayNum.Text = string.Empty;
                    unitPrice.Text = string.Empty;
                    updateTime.Text = string.Empty;
                    updatePC.Text = string.Empty;
                    note.Text = string.Empty;
                    preprocessTimeM.Text = string.Empty;
                    preprocessTimeF.Text = string.Empty;
                    nightTimeM_indirect.Text = string.Empty;
                    nightTimeF.Text = string.Empty;
                    dryTimeM.Text = string.Empty;
                    dryTimeF.Text = string.Empty;
                    selectionTimeM.Text = string.Empty;
                    selectionTimeF.Text = string.Empty;
                    dgvMaterialCost.Rows.Clear();
                    dgvContractors.Rows.Clear();
                    dgvMaterialsFare.Rows.Clear();
                    dgvPacking.Rows.Clear();
                    dgvMachine.Rows.Clear();
                    initDgvMachineRow();
                    dgvPackingFare.Rows.Clear();
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
                                      new { t_product.year, t_product.code, t_product.category }
                                        equals
                                      new { t_supplier.year, code = t_supplier.product_code, t_supplier.category }
                                 where t_product.year.Equals(Const.TARGET_YEAR)
                                    && t_product.code.Equals(productCode.Text)
                                    && t_supplier.supplier_code.Equals(suppllierCode.Text)
                                    && t_product.category.Equals(category)
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
         * 計算処理
         *************************************************************/
        private void calcAll()
        {
            volume.Text = Conversion.Parse(volume.Text).ToString("N");
            unitPrice.Text = Conversion.Parse(unitPrice.Text).ToString("N");

            // 原料費タブの計算
            calcMaterialCost();

            // 外注費タブの計算
            calcContractorsCost();

            // 労務費タブの計算
            calcLaborCost();

            // 製造経費－原料運賃タブの計算
            calcMaterialsFare();

            // 製造経費－包装資材費タブの計算
            calcPackingCost();

            // 製造経費－設備費タブの計算
            calcMachineCost();

            // 製造経費－荷造運賃タブの計算
            calcPackingFare();

            // 原価計算
            calcCost();

            // 構成比率計算
            calcRatio();
        }

        /*************************************************************
         * 原価計算を行う
         *************************************************************/
        private void calcCost()
        {
            Const.CATEGORY_TYPE category = Program.judgeCategory(radioBudget, radioActual);

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++原価計算
            // ①原料費
            decimal lossRate = decimal.Add(decimal.One, decimal.Divide(Parameters.getInstance(category).rateLoss, 100));
            materialCost.Text = decimal.Multiply(Conversion.Parse((string)dgvMaterialCostTotal.Rows[0].Cells[4].Value), lossRate).ToString("N");

            // ②外注費
            contractorsCost.Text = Conversion.Parse((string)dgvContractorsTotal.Rows[0].Cells[4].Value).ToString("N");

            // 直接労務費
            laborCostDirect.Text = decimal.Subtract(Conversion.Parse(kgPerwagesSum.Text), Conversion.Parse(kgPerwagesM_indirect.Text)).ToString("N");

            // 間接労務費
            laborCostIndirect.Text = Conversion.Parse(kgPerwagesM_indirect.Text).ToString("N");

            // ③労務費
            laborCost.Text = decimal.Add(Conversion.Parse(laborCostDirect.Text), Conversion.Parse(laborCostIndirect.Text)).ToString("N");

            // 原料運賃
            materialsFare.Text = Conversion.Parse((string)dgvMaterialsFareTotal.Rows[0].Cells[4].Value).ToString("N");

            // 包装資材費
            packingCost.Text = Conversion.Parse((string)dgvPackingTotal.Rows[0].Cells[4].Value).ToString("N");

            // 設備費
            machineCost.Text = Conversion.Parse((string)dgvMachineTotal.Rows[0].Cells[4].Value).ToString("N");

            // 水道光熱費
            utilitiesCost.Text = Conversion.Parse(utilitiesKgPerAmountSum.Text).ToString("N");

            // その他経費
            otherCost.Text = Conversion.Parse(otherKgPerAmountSum.Text).ToString("N");

            // ④製造経費
            manufacturingCost.Text = (Conversion.Parse(materialsFare.Text) + Conversion.Parse(packingCost.Text)
                                      + Conversion.Parse(machineCost.Text) + Conversion.Parse(utilitiesCost.Text)
                                      + Conversion.Parse(otherCost.Text)).ToString("N");

            // 合計製品原価
            productCost.Text = (Conversion.Parse(materialCost.Text) + Conversion.Parse(contractorsCost.Text)
                                + Conversion.Parse(laborCost.Text) + Conversion.Parse(manufacturingCost.Text)).ToString("N");

            // 荷造運賃
            packingFare.Text = Conversion.Parse((string)dgvPackingFareTotal.Rows[0].Cells[4].Value).ToString("N");

            // 販売費
            decimal rateSelling = decimal.Divide(Parameters.getInstance(category).allocationSale, 100);
            sellingCost.Text = decimal.Multiply(Conversion.Parse(productCost.Text), rateSelling).ToString("N");

            // 一般管理費営業外費用
            decimal rateMng = decimal.Divide(Parameters.getInstance(category).allocationMng + Parameters.getInstance(category).allocationExt, 100);
            managementCost.Text = decimal.Multiply(Conversion.Parse(productCost.Text), rateMng).ToString("N");

            // 総原価
            overallCost.Text = (Conversion.Parse(productCost.Text) + Conversion.Parse(packingFare.Text)
                                + Conversion.Parse(sellingCost.Text) + Conversion.Parse(managementCost.Text)).ToString("N");

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
            decimal rateExpend = decimal.Divide(Parameters.getInstance(category).rateExpend, 100);
            marginalProfit.Text = decimal.Subtract(Conversion.Parse(realSellingPrice.Text)
                                                   , (Conversion.Parse(materialCost.Text) + Conversion.Parse(contractorsCost.Text)
                                                      + Conversion.Parse(laborCostDirect.Text) + Conversion.Parse(materialsFare.Text)
                                                      + Conversion.Parse(packingCost.Text) + Conversion.Parse(utilitiesCost.Text)
                                                      + decimal.Multiply(Conversion.Parse(otherCost.Text), rateExpend)
                                                      + Conversion.Parse(packingFare.Text))).ToString("N");
        }

        /*************************************************************
         * 構成比率計算
         *************************************************************/
        private void calcRatio()
        {
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++原価計算
            decimal overallCostNum = Conversion.Parse(overallCost.Text);
            if (overallCostNum > decimal.Zero)
            {
                // ①原料費
                materialCostRatio.Text = decimal.Divide(Conversion.Parse(materialCost.Text), overallCostNum).ToString("P");

                // ②外注費
                contractorsCostRatio.Text = decimal.Divide(Conversion.Parse(contractorsCost.Text), overallCostNum).ToString("P");

                // ③労務費
                laborCostRatio.Text = decimal.Divide(Conversion.Parse(laborCost.Text), overallCostNum).ToString("P");

                // 直接労務費
                laborCostDirectRatio.Text = decimal.Divide(Conversion.Parse(laborCostDirect.Text), overallCostNum).ToString("P");

                // 間接労務費
                laborCostIndirectRatio.Text = decimal.Divide(Conversion.Parse(laborCostIndirect.Text), overallCostNum).ToString("P");

                // ④製造経費
                manufacturingCostRatio.Text = decimal.Divide(Conversion.Parse(manufacturingCost.Text), overallCostNum).ToString("P");

                // 原料運賃
                materialsFareRatio.Text = decimal.Divide(Conversion.Parse(materialsFare.Text), overallCostNum).ToString("P");

                // 包装資材費
                packingCostRatio.Text = decimal.Divide(Conversion.Parse(packingCost.Text), overallCostNum).ToString("P");

                // 設備費
                machineCostRatio.Text = decimal.Divide(Conversion.Parse(machineCost.Text), overallCostNum).ToString("P");

                // 水道光熱費
                utilitiesCostRatio.Text = decimal.Divide(Conversion.Parse(utilitiesCost.Text), overallCostNum).ToString("P");

                // その他経費
                otherCostRatio.Text = decimal.Divide(Conversion.Parse(otherCost.Text), overallCostNum).ToString("P");

                // 合計製品合計
                productCostRatio.Text = decimal.Divide(Conversion.Parse(productCost.Text), overallCostNum).ToString("P");

                // 荷造運賃
                packingFareRatio.Text = decimal.Divide(Conversion.Parse(packingFare.Text), overallCostNum).ToString("P");

                // 販売費
                sellingCostRatio.Text = decimal.Divide(Conversion.Parse(sellingCost.Text), overallCostNum).ToString("P");

                // 一般管理費営業外費用
                managementCostRatio.Text = decimal.Divide(Conversion.Parse(managementCost.Text), overallCostNum).ToString("P");

                // 総原価
                overallCostRatio.Text = decimal.Divide(Conversion.Parse(overallCost.Text), overallCostNum).ToString("P");
            }
            else
            {
                materialCostRatio.Text = decimal.Zero.ToString("P");
                contractorsCostRatio.Text = decimal.Zero.ToString("P");
                laborCostRatio.Text = decimal.Zero.ToString("P");
                laborCostDirectRatio.Text = decimal.Zero.ToString("P");
                laborCostIndirectRatio.Text = decimal.Zero.ToString("P");
                manufacturingCostRatio.Text = decimal.Zero.ToString("P");
                materialsFareRatio.Text = decimal.Zero.ToString("P");
                packingCostRatio.Text = decimal.Zero.ToString("P");
                machineCostRatio.Text = decimal.Zero.ToString("P");
                utilitiesCostRatio.Text = decimal.Zero.ToString("P");
                otherCostRatio.Text = decimal.Zero.ToString("P");
                productCostRatio.Text = decimal.Zero.ToString("P");
                packingFareRatio.Text = decimal.Zero.ToString("P");
                sellingCostRatio.Text = decimal.Zero.ToString("P");
                managementCostRatio.Text = decimal.Zero.ToString("P");
                overallCostRatio.Text = decimal.Zero.ToString("P");
            }
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++経理計算
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
         * 原料費タブの計算
         *************************************************************/
        private void calcMaterialCost()
        {
            decimal sumQuantity = 0;
            decimal sumAmount = 0;
            decimal sumCostKgPerAmount = 0;
            decimal sumCostRatio = 0;
            decimal volume = Conversion.Parse(this.volume.Text);

            foreach (DataGridViewRow row in dgvMaterialCost.Rows)
            {
                string code = (string)row.Cells["dgvMaterialCostName"].Value;
                if (!String.IsNullOrEmpty(code))
                {
                    decimal quantity = Conversion.Parse((string)row.Cells["dgvMaterialCostQuantity"].Value);
                    decimal cost = Conversion.Parse((string)row.Cells["dgvMaterialCostPrice"].Value);
                    decimal amount = decimal.Multiply(quantity, cost);
                    decimal kgPerAmount = (volume != decimal.Zero ? decimal.Divide(amount, volume) : decimal.Zero);

                    row.Cells["dgvMaterialCostQuantity"].Value = quantity.ToString("#,0.0");
                    row.Cells["dgvMaterialCostAmount"].Value = amount.ToString("N");
                    row.Cells["dgvMaterialCostKgPerAmount"].Value = kgPerAmount.ToString("N");
                    row.Cells["dgvMaterialCostRatio"].Value = decimal.Zero.ToString("P");

                    sumQuantity += quantity;
                    sumAmount += amount;
                    sumCostKgPerAmount += kgPerAmount;
                }
            }

            if (sumCostKgPerAmount != decimal.Zero)
            {
                foreach (DataGridViewRow row in dgvMaterialCost.Rows)
                {
                    string code = (string)row.Cells["dgvMaterialCostName"].Value;
                    if (!String.IsNullOrEmpty(code))
                    {
                        decimal costKgPerAmount = Conversion.Parse((string)row.Cells["dgvMaterialCostKgPerAmount"].Value);
                        decimal costRatio = decimal.Divide(costKgPerAmount, sumCostKgPerAmount);
                        row.Cells["dgvMaterialCostRatio"].Value = costRatio.ToString("P");
                        sumCostRatio += costRatio;
                    }
                }
            }

            dgvMaterialCostTotal.Rows[0].Cells[1].Value = sumQuantity.ToString("#,0.0");
            dgvMaterialCostTotal.Rows[0].Cells[2].Value = String.Empty;
            dgvMaterialCostTotal.Rows[0].Cells[3].Value = sumAmount.ToString("N");
            dgvMaterialCostTotal.Rows[0].Cells[4].Value = sumCostKgPerAmount.ToString("N");
            dgvMaterialCostTotal.Rows[0].Cells[5].Value = sumCostRatio.ToString("P");
        }

        /*************************************************************
         * 外注費タブの計算
         *************************************************************/
        private void calcContractorsCost()
        {
            decimal sumQuantity = 0;
            decimal sumAmount = 0;
            decimal sumCostKgPerAmount = 0;
            decimal sumCostRatio = 0;
            decimal volume = Conversion.Parse(this.volume.Text);
            foreach (DataGridViewRow row in dgvContractors.Rows)
            {
                string name = (string)row.Cells["dgvContractorsName"].Value;
                if (!String.IsNullOrEmpty(name))
                {
                    decimal quantity = Conversion.Parse((string)row.Cells["dgvContractorsQuantity"].Value);
                    decimal cost = Conversion.Parse((string)row.Cells["dgvContractorsCost"].Value);
                    decimal amount = decimal.Multiply(quantity, cost);
                    decimal kgPerAmount = (volume != decimal.Zero ? decimal.Divide(amount, volume) : decimal.Zero);

                    row.Cells["dgvContractorsQuantity"].Value = quantity.ToString("#,0.0");
                    row.Cells["dgvContractorsCost"].Value = cost.ToString("#,0.0");
                    row.Cells["dgvContractorsAmount"].Value = amount.ToString("N");
                    row.Cells["dgvContractorsKgPerAmount"].Value = kgPerAmount.ToString("N");
                    row.Cells["dgvContractorsRatio"].Value = decimal.Zero.ToString("P");

                    sumQuantity += quantity;
                    sumAmount += amount;
                    sumCostKgPerAmount += kgPerAmount;
                }
            }

            if (sumCostKgPerAmount != decimal.Zero)
            {
                foreach (DataGridViewRow row in dgvContractors.Rows)
                {
                    string name = (string)row.Cells["dgvContractorsName"].Value;
                    if (!String.IsNullOrEmpty(name))
                    {
                        decimal costKgPerAmount = Conversion.Parse((string)row.Cells["dgvContractorsKgPerAmount"].Value);
                        decimal costRatio = decimal.Divide(costKgPerAmount, sumCostKgPerAmount);
                        row.Cells["dgvContractorsRatio"].Value = costRatio.ToString("P");
                        sumCostRatio += costRatio;
                    }
                }
            }

            dgvContractorsTotal.Rows[0].Cells[1].Value = sumQuantity.ToString("#,0.0");
            dgvContractorsTotal.Rows[0].Cells[2].Value = String.Empty;
            dgvContractorsTotal.Rows[0].Cells[3].Value = sumAmount.ToString("N");
            dgvContractorsTotal.Rows[0].Cells[4].Value = sumCostKgPerAmount.ToString("N");
            dgvContractorsTotal.Rows[0].Cells[5].Value = sumCostRatio.ToString("P");
        }

        /*************************************************************
         * 労務費タブの計算
         *************************************************************/
        private void calcLaborCost()
        {
            // 前処理
            decimal preprocessTimeM = Conversion.Parse(this.preprocessTimeM.Text);
            decimal preprocessTimeF = Conversion.Parse(this.preprocessTimeF.Text);
            preprocessTimeSum.Text = decimal.Add(preprocessTimeM, preprocessTimeF).ToString("#,0");
            this.preprocessTimeM.Text = preprocessTimeM.ToString("#,0");
            this.preprocessTimeF.Text = preprocessTimeF.ToString("#,0");

            // 夜勤
            decimal nightTimeM = Conversion.Parse(this.nightTimeM_indirect.Text);
            decimal nightTimeF = Conversion.Parse(this.nightTimeF.Text);

            decimal tray = Parameters.getInstance(Program.judgeCategory(radioBudget, radioActual)).trayNum;
            decimal nightTimeM_direct = (tray != decimal.Zero ? 
                                            decimal.Divide(decimal.Multiply(nightTimeM, (decimal)(2 * Conversion.Parse(trayNum.Text)))
                                                           , tray)
                                            : decimal.Zero);

            this.nightTimeM_direct.Text = nightTimeM_direct.ToString("#,0");
            nightTimeSum.Text = decimal.Add(nightTimeM, nightTimeF).ToString("#,0");
            this.nightTimeM_indirect.Text = nightTimeM.ToString("#,0");
            this.nightTimeF.Text = nightTimeF.ToString("#,0");

            // 乾燥
            decimal dryTimeM = Conversion.Parse(this.dryTimeM.Text);
            decimal dryTimeF = Conversion.Parse(this.dryTimeF.Text);
            dryTimeSum.Text = decimal.Add(dryTimeM, dryTimeF).ToString("#,0");
            this.dryTimeM.Text = dryTimeM.ToString("#,0");
            this.dryTimeF.Text = dryTimeF.ToString("#,0");

            // 選別
            decimal selectionTimeM = Conversion.Parse(this.selectionTimeM.Text);
            decimal selectionTimeF = Conversion.Parse(this.selectionTimeF.Text);
            selectionTimeSum.Text = decimal.Add(selectionTimeM, selectionTimeF).ToString("#,0");
            this.selectionTimeM.Text = selectionTimeM.ToString("#,0");
            this.selectionTimeF.Text = selectionTimeF.ToString("#,0");

            // 合計
            decimal workingTimeM = preprocessTimeM + nightTimeM_direct + dryTimeM + selectionTimeM;
            decimal workingTimeF = preprocessTimeF + nightTimeF + dryTimeF + selectionTimeF;
            this.workingTimeM.Text = workingTimeM.ToString("#,0");
            this.workingTimeF.Text = workingTimeF.ToString("#,0");
            workingTimeSum.Text = decimal.Add(workingTimeM, workingTimeF).ToString("#,0");

            // 労賃
            decimal wageRateM_direct = Conversion.Parse(this.wageRateM_direct.Text);
            decimal wageRateM_indirect = Conversion.Parse(this.wageRateM_indirect.Text);
            decimal wageRateF = Conversion.Parse(this.wageRateF.Text);
            decimal wagesM_direct = decimal.Multiply(workingTimeM, wageRateM_direct);
            decimal wagesM_indirect = decimal.Multiply(workingTimeM, wageRateM_indirect);
            decimal wagesF = decimal.Multiply(workingTimeF, wageRateF);
            this.wagesM_direct.Text = wagesM_direct.ToString("N");
            this.wagesM_indirect.Text = wagesM_indirect.ToString("N");
            this.wagesF.Text = wagesF.ToString("N");
            wagesSum.Text = (wagesM_direct + wagesM_indirect + wagesF).ToString("N");

            // kg当り労賃
            decimal volume = Conversion.Parse(this.volume.Text);
            decimal kgPerwagesM_direct = decimal.Zero;
            decimal kgPerwagesM_indirect = decimal.Zero;
            decimal kgPerwagesF = decimal.Zero;
            if (volume != decimal.Zero)
            {
                kgPerwagesM_direct = decimal.Divide(wagesM_direct, volume);
                kgPerwagesM_indirect = decimal.Divide(wagesM_indirect, volume);
                kgPerwagesF = decimal.Divide(wagesF, volume);
            }
            this.kgPerwagesM_direct.Text = kgPerwagesM_direct.ToString("N");
            this.kgPerwagesM_indirect.Text = kgPerwagesM_indirect.ToString("N");
            this.kgPerwagesF.Text = kgPerwagesF.ToString("N");
            this.kgPerwagesSum.Text = (kgPerwagesM_direct + kgPerwagesM_indirect + kgPerwagesF).ToString("N");

            // 製造経費－その他経費タブの計算
            calcOtherCost();
        }

        /*************************************************************
         * 製造経費－原料運賃タブの計算
         *************************************************************/
        private void calcMaterialsFare()
        {
            decimal sumAmount = 0;
            decimal sumCostKgPerAmount = 0;
            decimal volume = Conversion.Parse(this.volume.Text);

            foreach (DataGridViewRow row in dgvMaterialsFare.Rows)
            {
                string name = (string)row.Cells["dgvMaterialsFareName"].Value;
                if (!String.IsNullOrEmpty(name))
                {
                    decimal quantity = Conversion.Parse((string)row.Cells["dgvMaterialsFareQuantity"].Value);
                    decimal cost = Conversion.Parse((string)row.Cells["dgvMaterialsFareCost"].Value);
                    decimal amount = decimal.Multiply(quantity, cost);
                    decimal kgPerAmount = (volume != decimal.Zero ? decimal.Divide(amount, volume) : decimal.Zero);

                    row.Cells["dgvMaterialsFareQuantity"].Value = quantity.ToString("#,0.0");
                    row.Cells["dgvMaterialsFareCost"].Value = cost.ToString("#,0.0");
                    row.Cells["dgvMaterialsFareAmount"].Value = amount.ToString("N");
                    row.Cells["dgvMaterialsFareKgPerAmount"].Value = kgPerAmount.ToString("N");

                    sumAmount += amount;
                    sumCostKgPerAmount += kgPerAmount;
                }
            }
            dgvMaterialsFareTotal.Rows[0].Cells[1].Value = String.Empty;
            dgvMaterialsFareTotal.Rows[0].Cells[2].Value = String.Empty;
            dgvMaterialsFareTotal.Rows[0].Cells[3].Value = sumAmount.ToString("N");
            dgvMaterialsFareTotal.Rows[0].Cells[4].Value = sumCostKgPerAmount.ToString("N");
        }

        /*************************************************************
         * 製造経費－包装資材費タブの計算
         *************************************************************/
        private void calcPackingCost()
        {
            decimal sumAmount = 0;
            decimal sumCostKgPerAmount = 0;
            decimal volume = Conversion.Parse(this.volume.Text);

            foreach (DataGridViewRow row in dgvPacking.Rows)
            {
                string name = (string)row.Cells["dgvPackingName"].Value;
                if (!String.IsNullOrEmpty(name))
                {
                    decimal quantity = Conversion.Parse((string)row.Cells["dgvPackingQuantity"].Value);
                    decimal cost = Conversion.Parse((string)row.Cells["dgvPackingCost"].Value);
                    decimal amount = decimal.Multiply(quantity, cost);
                    decimal kgPerAmount = (volume != decimal.Zero ? decimal.Divide(amount, volume) : decimal.Zero);

                    row.Cells["dgvPackingQuantity"].Value = quantity.ToString("#,0.0");
                    row.Cells["dgvPackingAmount"].Value = amount.ToString("N");
                    row.Cells["dgvPackingKgPerAmount"].Value = kgPerAmount.ToString("N");

                    sumAmount += amount;
                    sumCostKgPerAmount += kgPerAmount;
                }
            }
            dgvPackingTotal.Rows[0].Cells[1].Value = String.Empty;
            dgvPackingTotal.Rows[0].Cells[2].Value = String.Empty;
            dgvPackingTotal.Rows[0].Cells[3].Value = sumAmount.ToString("N");
            dgvPackingTotal.Rows[0].Cells[4].Value = sumCostKgPerAmount.ToString("N");
        }

        /*************************************************************
         * 製造経費－設備費タブの計算
         *************************************************************/
        private void calcMachineCost()
        {
            decimal sumAmount = 0;
            decimal sumCostKgPerAmount = 0;
            decimal volume = Conversion.Parse(this.volume.Text);

            foreach (DataGridViewRow row in dgvMachine.Rows)
            {
                string name = (string)row.Cells["dgvMachineName"].Value;
                if (!String.IsNullOrEmpty(name))
                {
                    decimal time = Conversion.Parse((string)row.Cells["dgvMachineTime"].Value);
                    decimal rate = Conversion.Parse((string)row.Cells["dgvMachineRate"].Value);
                    decimal amount = decimal.Multiply(time, rate);
                    if ("A".Equals(name))
                        amount = decimal.Multiply(amount, Conversion.Parse(trayNum.Text));

                    decimal kgPerAmount = (volume != decimal.Zero ? decimal.Divide(amount, volume) : decimal.Zero);

                    row.Cells["dgvMachineTime"].Value = time.ToString("#,0.0");
                    row.Cells["dgvMachineAmount"].Value = amount.ToString("N");
                    row.Cells["dgvMachineKgPerAmount"].Value = kgPerAmount.ToString("N");

                    sumAmount += amount;
                    sumCostKgPerAmount += kgPerAmount;
                }
            }
            dgvMachineTotal.Rows[0].Cells[1].Value = String.Empty;
            dgvMachineTotal.Rows[0].Cells[2].Value = String.Empty;
            dgvMachineTotal.Rows[0].Cells[3].Value = sumAmount.ToString("N");
            dgvMachineTotal.Rows[0].Cells[4].Value = sumCostKgPerAmount.ToString("N");

            trayNum.Text = Conversion.Parse(trayNum.Text).ToString("N");

            // 製造経費－水道光熱費タブの計算
            calcUtilitiesCost();

            // 製造経費－その他経費タブの計算
            calcOtherCost();
        }

        /*************************************************************
         * 製造経費－水道光熱費タブの計算
         *************************************************************/
        private void calcUtilitiesCost()
        {
            decimal machineUseTimeB = decimal.Divide(Conversion.Parse((string)dgvMachine.Rows[0].Cells["dgvMachineTime"].Value), 60);
            decimal machineUseTimeA = decimal.Divide(decimal.Multiply(Conversion.Parse(trayNum.Text)
                                                     , Conversion.Parse((string)dgvMachine.Rows[1].Cells["dgvMachineTime"].Value)), 60);
            decimal utilitiesAmountB = decimal.Multiply(machineUseTimeB, Conversion.Parse(fuelAmountPerHourB.Text));
            decimal utilitiesAmountA = decimal.Multiply(machineUseTimeA, Conversion.Parse(fuelAmountPerHourA.Text));

            decimal volume = Conversion.Parse(this.volume.Text);
            decimal utilitiesKgPerAmountB = (volume != decimal.Zero ? decimal.Divide(utilitiesAmountB, volume) : decimal.Zero);
            decimal utilitiesKgPerAmountA = (volume != decimal.Zero ? decimal.Divide(utilitiesAmountA, volume) : decimal.Zero);

            this.machineUseTimeB.Text = machineUseTimeB.ToString("N");
            this.machineUseTimeA.Text = machineUseTimeA.ToString("N");
            this.utilitiesAmountB.Text = utilitiesAmountB.ToString("N");
            this.utilitiesAmountA.Text = utilitiesAmountA.ToString("N");
            this.utilitiesAmountSum.Text = decimal.Add(utilitiesAmountB, utilitiesAmountA).ToString("N");
            this.utilitiesKgPerAmountB.Text = utilitiesKgPerAmountB.ToString("N");
            this.utilitiesKgPerAmountA.Text = utilitiesKgPerAmountA.ToString("N");
            this.utilitiesKgPerAmountSum.Text = decimal.Add(utilitiesKgPerAmountB, utilitiesKgPerAmountA).ToString("N");
        }

        /*************************************************************
         * 製造経費－その他経費タブの計算
         *************************************************************/
        private void calcOtherCost()
        {
            decimal timeFD = decimal.Divide(decimal.Multiply(Conversion.Parse(trayNum.Text)
                                            , Conversion.Parse((string)dgvMachine.Rows[0].Cells["dgvMachineTime"].Value)), 60);
            decimal timeAD = decimal.Divide(Conversion.Parse((string)dgvMachine.Rows[1].Cells["dgvMachineTime"].Value), 60);
            decimal timeLabor = decimal.Divide(Conversion.Parse(workingTimeM.Text), 60);
            decimal allocationFD = decimal.Multiply(timeFD, Conversion.Parse(fuelAmountPerHourFD.Text));
            decimal allocationAD = decimal.Multiply(timeAD, Conversion.Parse(fuelAmountPerHourAD.Text));
            decimal allocationLabor = decimal.Multiply(timeLabor, Conversion.Parse(fuelAmountPerHourLabor.Text));

            decimal volume = Conversion.Parse(this.volume.Text);
            decimal utilitiesKgPerAmountFD = (volume != decimal.Zero ? decimal.Divide(allocationFD, volume) : decimal.Zero);
            decimal utilitiesKgPerAmountAD = (volume != decimal.Zero ? decimal.Divide(allocationAD, volume) : decimal.Zero);
            decimal utilitiesKgPerAmountLabor = (volume != decimal.Zero ? decimal.Divide(allocationLabor, volume) : decimal.Zero);

            this.timeFD.Text = timeFD.ToString("N");
            this.timeAD.Text = timeAD.ToString("N");
            this.timeLabor.Text = timeLabor.ToString("N");

            this.allocationFD.Text = allocationFD.ToString("N");
            this.allocationAD.Text = allocationAD.ToString("N");
            this.allocationLabor.Text = allocationLabor.ToString("N");
            this.allocationSum.Text = decimal.Add(allocationFD + allocationAD, allocationLabor).ToString("N");

            this.otherKgPerAmountFD.Text = utilitiesKgPerAmountFD.ToString("N");
            this.otherKgPerAmountAD.Text = utilitiesKgPerAmountAD.ToString("N");
            this.otherKgPerAmountLabor.Text = utilitiesKgPerAmountLabor.ToString("N");
            this.otherKgPerAmountSum.Text = decimal.Add(utilitiesKgPerAmountFD + utilitiesKgPerAmountAD, utilitiesKgPerAmountLabor).ToString("N");
        }

        /*************************************************************
         * 製造経費－荷造運賃タブの計算
         *************************************************************/
        private void calcPackingFare()
        {
            decimal sumAmount = 0;
            decimal sumCostKgPerAmount = 0;
            decimal volume = Conversion.Parse(this.volume.Text);

            foreach (DataGridViewRow row in dgvPackingFare.Rows)
            {
                string name = (string)row.Cells["dgvPackingFareName"].Value;
                if (!String.IsNullOrEmpty(name))
                {
                    decimal quantity = Conversion.Parse((string)row.Cells["dgvPackingFareQuantity"].Value);
                    decimal cost = Conversion.Parse((string)row.Cells["dgvPackingFareCost"].Value);
                    decimal amount = decimal.Multiply(quantity, cost);
                    decimal kgPerAmount = (volume != decimal.Zero ? decimal.Divide(amount, volume) : decimal.Zero);

                    row.Cells["dgvPackingFareQuantity"].Value = quantity.ToString("#,0.0");
                    row.Cells["dgvPackingFareAmount"].Value = amount.ToString("N");
                    row.Cells["dgvPackingFareKgPerAmount"].Value = kgPerAmount.ToString("N");

                    sumAmount += amount;
                    sumCostKgPerAmount += kgPerAmount;
                }
            }
            dgvPackingFareTotal.Rows[0].Cells[1].Value = String.Empty;
            dgvPackingFareTotal.Rows[0].Cells[2].Value = String.Empty;
            dgvPackingFareTotal.Rows[0].Cells[3].Value = sumAmount.ToString("N");
            dgvPackingFareTotal.Rows[0].Cells[4].Value = sumCostKgPerAmount.ToString("N");
        }

        /*************************************************************
         * テキストボックス編集と同時に原価計算を行う
         *************************************************************/
        private void control_Leave_calc(object sender, EventArgs e)
        {
            calcAll();
        }

        /*************************************************************
         * 登録ボタン押下時の処理
         *************************************************************/
        private void btnAppend_Click(object sender, EventArgs e)
        {
            if (Program.MessageBoxBefore("登録しますか？") != DialogResult.Yes)
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

                // 予定の場合は実績も同データで登録する
                if (radioBudget.Checked)
                {
                    radioActual.CheckedChanged -= new EventHandler(radio_CheckedChanged);
                    radioBudget.CheckedChanged -= new EventHandler(radio_CheckedChanged);

                    radioActual.Checked = true;
                    executeDelete(context, true);
                    executeAppend(context);
                    radioBudget.Checked = true;

                    radioActual.CheckedChanged += new EventHandler(radio_CheckedChanged);
                    radioBudget.CheckedChanged += new EventHandler(radio_CheckedChanged);
                }
                context.SaveChanges();
            }

            setOperationKbn();
            Program.MessageBoxAfter("登録しました。");
        }

        /*************************************************************
         * 修正ボタン押下時の処理
         *************************************************************/
        private void btnChange_Click(object sender, EventArgs e)
        {
            if (Program.MessageBoxBefore("修正しますか？") != DialogResult.Yes)
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
                context.SaveChanges();
            }

            Program.MessageBoxAfter("修正しました。");
        }

        /*************************************************************
         * 削除ボタン押下時の処理
         *************************************************************/
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Program.MessageBoxBefore("削除しますか？") != DialogResult.Yes)
            {
                return;
            }

            // 削除処理を行う
            using (var context = new CostAccountingEntities())
            {
                executeDelete(context, false);
                context.SaveChanges();
            }

            setProductData();
            setSupplierData();
            setOperationKbn();
            Program.MessageBoxAfter("削除しました。");
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
                item_code = itemCode.SelectedValue.ToString(),
                volume = Conversion.Parse(volume.Text),
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
                preprocess_time_m = Conversion.Parse(preprocessTimeM.Text),
                preprocess_time_f = Conversion.Parse(preprocessTimeF.Text),
                night_time_m = Conversion.Parse(nightTimeM_indirect.Text),
                night_time_f = Conversion.Parse(nightTimeF.Text),
                dry_time_m = Conversion.Parse(dryTimeM.Text),
                dry_time_f = Conversion.Parse(dryTimeF.Text),
                selection_time_m = Conversion.Parse(selectionTimeM.Text),
                selection_time_f = Conversion.Parse(selectionTimeF.Text),
                tray_num = Conversion.Parse(trayNum.Text),
                update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName),
                update_date = DateTime.Now,
                del_flg = Const.FLG_OFF
            };
            context.Product.Add(entityProduct);

            // 取引先データの登録
            var entitySupplier = new ProductSupplier()
            {
                year = Const.TARGET_YEAR,
                product_code = productCode.Text,
                supplier_code = suppllierCode.Text,
                category = category,
                unit_price = Conversion.Parse(unitPrice.Text),
                update_user = string.Concat(SystemInformation.ComputerName, "/", SystemInformation.UserName),
                update_date = DateTime.Now,
                del_flg = Const.FLG_OFF
            };
            context.ProductSupplier.Add(entitySupplier);

            // 関連テーブルの主キー（年＋商品コード＋取引先コード＋予定／実績）を生成
            string id = string.Concat(Const.TARGET_YEAR, productCode.Text, category);

            // 原料費データの登録
            foreach (DataGridViewRow row in dgvMaterialCost.Rows)
            {
                string code = (string)row.Cells["dgvMaterialCostName"].Value;
                if (!String.IsNullOrEmpty(code))
                {
                    var entity = new ProductMaterial()
                    {
                        id = id,
                        code = code,
                        quantity = Conversion.Parse((string)row.Cells["dgvMaterialCostQuantity"].Value),
                        update_user = SystemInformation.ComputerName,
                        update_date = DateTime.Now,
                        del_flg = Const.FLG_OFF
                    };
                    context.ProductMaterial.Add(entity);
                }
            }

            // 外注費データの登録
            foreach (DataGridViewRow row in dgvContractors.Rows)
            {
                string name = (string)row.Cells["dgvContractorsName"].Value;
                if (!String.IsNullOrEmpty(name))
                {
                    var entity = new ProductContractor()
                    {
                        id = id,
                        name = name,
                        quantity = Conversion.Parse((string)row.Cells["dgvContractorsQuantity"].Value),
                        cost = Conversion.Parse((string)row.Cells["dgvContractorsCost"].Value),
                        update_user = SystemInformation.ComputerName,
                        update_date = DateTime.Now,
                        del_flg = Const.FLG_OFF
                    };
                    context.ProductContractor.Add(entity);
                }
            }

            // 原料運賃データの登録
            foreach (DataGridViewRow row in dgvMaterialsFare.Rows)
            {
                string name = (string)row.Cells["dgvMaterialsFareName"].Value;
                if (!String.IsNullOrEmpty(name))
                {
                    var entity = new ProductMaterialsFare()
                    {
                        id = id,
                        name = name,
                        quantity = Conversion.Parse((string)row.Cells["dgvMaterialsFareQuantity"].Value),
                        cost = Conversion.Parse((string)row.Cells["dgvMaterialsFareCost"].Value),
                        update_user = SystemInformation.ComputerName,
                        update_date = DateTime.Now,
                        del_flg = Const.FLG_OFF
                    };
                    context.ProductMaterialsFare.Add(entity);
                }
            }

            // 包装資材費データの登録
            foreach (DataGridViewRow row in dgvPacking.Rows)
            {
                string code = (string)row.Cells["dgvPackingName"].Value;
                if (!String.IsNullOrEmpty(code))
                {
                    var entity = new ProductPacking()
                    {
                        id = id,
                        code = code,
                        quantity = Conversion.Parse((string)row.Cells["dgvPackingQuantity"].Value),
                        update_user = SystemInformation.ComputerName,
                        update_date = DateTime.Now,
                        del_flg = Const.FLG_OFF
                    };
                    context.ProductPacking.Add(entity);
                }
            }

            // 設備費データの登録
            foreach (DataGridViewRow row in dgvMachine.Rows)
            {
                string code = (string)row.Cells["dgvMachineName"].Value;
                if (!String.IsNullOrEmpty(code))
                {
                    var entity = new ProductMachine()
                    {
                        id = id,
                        code = code,
                        time = Conversion.Parse((string)row.Cells["dgvMachineTime"].Value),
                        update_user = SystemInformation.ComputerName,
                        update_date = DateTime.Now,
                        del_flg = Const.FLG_OFF
                    };
                    context.ProductMachine.Add(entity);
                }
            }

            // 荷造運賃
            foreach (DataGridViewRow row in dgvPackingFare.Rows)
            {
                string code = (string)row.Cells["dgvPackingFareName"].Value;
                if (!String.IsNullOrEmpty(code))
                {
                    var entity = new ProductPackingFare()
                    {
                        id = id,
                        code = code,
                        quantity = Conversion.Parse((string)row.Cells["dgvPackingFareQuantity"].Value),
                        update_user = SystemInformation.ComputerName,
                        update_date = DateTime.Now,
                        del_flg = Const.FLG_OFF
                    };
                    context.ProductPackingFare.Add(entity);
                }
            }
        }

        /*************************************************************
         * 削除処理を行う
         *************************************************************/
        private void executeDelete(CostAccountingEntities context, bool delProduct)
        {
            int category = (int)Program.judgeCategory(radioBudget, radioActual);


            // 取引先データ
            var supplier = from t in context.ProductSupplier
                           where t.year.Equals(Const.TARGET_YEAR)
                              && t.product_code.Equals(productCode.Text)
                              && t.supplier_code.Equals(suppllierCode.Text)
                              && t.category.Equals(category)
                           select t;
            context.ProductSupplier.RemoveRange(supplier);

            if (!delProduct)
            {
                // 同じ商品に紐づく取引先があるかチェックし、存在する場合は商品データを削除しない
                var supplierOther = from t in context.ProductSupplier
                                    where t.year.Equals(Const.TARGET_YEAR)
                                       && t.product_code.Equals(productCode.Text)
                                       && t.category.Equals(category)
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
                          select t;
            context.Product.RemoveRange(product);

            // 関連テーブルの主キー（年＋商品コード＋取引先コード＋予定／実績）を生成
            string id = string.Concat(Const.TARGET_YEAR, productCode.Text, category);

            // 原料費データ
            var material = from t in context.ProductMaterial
                           where t.id.Equals(id)
                           select t;
            context.ProductMaterial.RemoveRange(material);

            // 外注費データ
            var contractor = from t in context.ProductContractor
                             where t.id.Equals(id)
                             select t;
            context.ProductContractor.RemoveRange(contractor);

            // 製造経費－原料運賃データ
            var materialFare = from t in context.ProductMaterialsFare
                               where t.id.Equals(id)
                               select t;
            context.ProductMaterialsFare.RemoveRange(materialFare);

            // 製造経費－包装資材費データ
            var packing = from t in context.ProductPacking
                          where t.id.Equals(id)
                          select t;
            context.ProductPacking.RemoveRange(packing);

            // 製造経費－設備費データ
            var machine = from t in context.ProductMachine
                          where t.id.Equals(id)
                          select t;
            context.ProductMachine.RemoveRange(machine);

            // 製造経費－荷造運賃データ
            var packingFare = from t in context.ProductPackingFare
                              where t.id.Equals(id)
                              select t;
            context.ProductPackingFare.RemoveRange(packingFare);
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
