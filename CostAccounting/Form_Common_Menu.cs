using System;
using System.Windows.Forms;

namespace CostAccounting
{
    public partial class Form_Common_Menu : Form
    {
        /*************************************************************
         * コンストラクタ
         *************************************************************/
        public Form_Common_Menu()
        {
            InitializeComponent();
        }

        /*************************************************************
         * 導入処理ボタン押下時の処理
         *************************************************************/
        private void btnMainPrepare_Click(object sender, EventArgs e)
        {
            this.btnSubRawMaterialsReg.Visible = true;
            this.btnSubMaterialsReg.Visible = true;
            this.btnSubMachineReg.Visible = true;
            this.btnSubFareReg.Visible = true;
            this.btnSubProductCdReg.Visible = true;
            this.btnSubSupplierReg.Visible = true;
            this.btnSubItemReg.Visible = true;
            this.btnSubOtherReg.Visible = true;
            this.btnSubDataCopy.Visible = true;
            this.btnSubProductReg.Visible = false;
            this.btnSubBlendReg.Visible = false;
            this.btnSubBudgetReg.Visible = false;
            this.btnSubActualReg.Visible = false;
            this.btnSubActualTotal.Visible = false;
            this.btnSubComparison.Visible = false;
            this.btnSubDivergence.Visible = false;
        }

        /*************************************************************
         * 商品管理ボタン押下時の処理
         *************************************************************/
        private void btnMainProductMng_Click(object sender, EventArgs e)
        {
            this.btnSubRawMaterialsReg.Visible = false;
            this.btnSubMaterialsReg.Visible = false;
            this.btnSubMachineReg.Visible = false;
            this.btnSubFareReg.Visible = false;
            this.btnSubProductCdReg.Visible = false;
            this.btnSubSupplierReg.Visible = false;
            this.btnSubItemReg.Visible = false;
            this.btnSubOtherReg.Visible = false;
            this.btnSubDataCopy.Visible = false;
            this.btnSubProductReg.Visible = true;
            this.btnSubBlendReg.Visible = true;
            this.btnSubBudgetReg.Visible = false;
            this.btnSubActualReg.Visible = false;
            this.btnSubActualTotal.Visible = false;
            this.btnSubComparison.Visible = false;
            this.btnSubDivergence.Visible = false;
        }

        /*************************************************************
         * 原価管理ボタン押下時の処理
         *************************************************************/
        private void btnMainCostMng_Click(object sender, EventArgs e)
        {
            this.btnSubRawMaterialsReg.Visible = false;
            this.btnSubMaterialsReg.Visible = false;
            this.btnSubMachineReg.Visible = false;
            this.btnSubFareReg.Visible = false;
            this.btnSubProductCdReg.Visible = false;
            this.btnSubSupplierReg.Visible = false;
            this.btnSubItemReg.Visible = false;
            this.btnSubOtherReg.Visible = false;
            this.btnSubDataCopy.Visible = false;
            this.btnSubProductReg.Visible = false;
            this.btnSubBlendReg.Visible = false;
            this.btnSubBudgetReg.Visible = true;
            this.btnSubActualReg.Visible = true;
            this.btnSubActualTotal.Visible = true;
            this.btnSubComparison.Visible = true;
            this.btnSubDivergence.Visible = true;
        }

        /*************************************************************
         * 導入処理－原材料登録ボタン押下時の処理
         *************************************************************/
        private void btnSubRawMaterialsReg_Click(object sender, EventArgs e)
        {
            openForm(new Form_Prepare_RawMaterialsReg());
        }

        /*************************************************************
         * 導入処理－資材登録ボタン押下時の処理
         *************************************************************/
        private void btnSubMaterialsReg_Click(object sender, EventArgs e)
        {
            openForm(new Form_Prepare_MaterialsReg());
        }

        /*************************************************************
         * 導入処理－マシン登録ボタン押下時の処理
         *************************************************************/
        private void btnSubMachineReg_Click(object sender, EventArgs e)
        {
            openForm(new Form_Prepare_MachineReg());
        }

        /*************************************************************
         * 導入処理－運賃登録ボタン押下時の処理
         *************************************************************/
        private void btnSubFareReg_Click(object sender, EventArgs e)
        {
            openForm(new Form_Prepare_FareReg());
        }

        /*************************************************************
         * 導入処理－商品コード登録ボタン押下時の処理
         *************************************************************/
        private void btnSubProductCdReg_Click(object sender, EventArgs e)
        {
            openForm(new Form_Prepare_ProductReg());
        }

        /*************************************************************
         * 導入処理－取引先コード登録ボタン押下時の処理
         *************************************************************/
        private void btnSubSupplierReg_Click(object sender, EventArgs e)
        {
            openForm(new Form_Prepare_SupplierReg());
        }

        /*************************************************************
         * 導入処理－種目コード登録ボタン押下時の処理
         *************************************************************/
        private void btnSubItemReg_Click(object sender, EventArgs e)
        {
            openForm(new Form_Prepare_ItemReg());
        }

        /*************************************************************
         * 導入処理－その他登録ボタン押下時の処理
         *************************************************************/
        private void btnSubOtherReg_Click(object sender, EventArgs e)
        {
            openForm(new Form_Prepare_OtherReg());
        }

        /*************************************************************
         * 導入処理－データコピーボタン押下時の処理
         *************************************************************/
        private void btnSubDataCopy_Click(object sender, EventArgs e)
        {
            openForm(new Form_PrePare_DataCopy());
        }

        /*************************************************************
         * 原価管理－予算登録ボタン押下時の処理
         *************************************************************/
        private void btnSubBudgetReg_Click(object sender, EventArgs e)
        {
            openForm(new Form_CostMng_BudgetReg());
        }

        /*************************************************************
         * 原価管理－予算登録ボタン押下時の処理
         *************************************************************/
        private void btnSubActualReg_Click(object sender, EventArgs e)
        {
            openForm(new Form_CostMng_ActualReg());
        }

        /*************************************************************
         * 原価管理－予算実績対比ボタン押下時の処理
         *************************************************************/
        private void btnSubComparison_Click(object sender, EventArgs e)
        {
            openForm(new Form_CostMng_Comparison());
        }

        /*************************************************************
         * 原価管理－実績集計ボタン押下時の処理
         *************************************************************/
        private void btnSubActualTotal_Click(object sender, EventArgs e)
        {
            openForm(new Form_CostMng_ActualTotal());
        }

        /*************************************************************
         * 原価管理－乖離幅測定ボタン押下時の処理
         *************************************************************/
        private void btnSubDivergence_Click(object sender, EventArgs e)
        {
            openForm(new Form_CostMng_Divergence());
        }

        /*************************************************************
         * 商品管理－商品登録ボタン押下時の処理
         *************************************************************/
        private void btnSubProductReg_Click(object sender, EventArgs e)
        {
            openForm(new Form_ProductMng_ProductReg());
        }

        /*************************************************************
         * 商品管理－ブレンド品登録ボタン押下時の処理
         *************************************************************/
        private void btnSubBlendReg_Click(object sender, EventArgs e)
        {
            openForm(new Form_ProductMng_BlendReg());
        }

        /*************************************************************
         * フォームロード時の処理
         *************************************************************/
        private void Form_Menu_Load(object sender, EventArgs e)
        {
            this.btnMainPrepare.PerformClick();
        }

        /*************************************************************
         * 対象フォームを呼び出す
         *************************************************************/
        private void openForm(Form target)
        {
            try
            {
                target.ShowDialog();
                target.Dispose();
                DataTableSupport.refresh();
                Parameters.refresh();
            }
            catch (Exception e)
            {
                Program.MessageBoxError(string.Concat("予期しないエラーが発生しました。"
                                              , Environment.NewLine
                                              , "お手数ですが、ログファイルの送付をお願いします。"
                                              , Environment.NewLine
                                              , "_(._.)_"));
                Logger.Error("予期しないエラーが発生しました。", e);
            }
        }
    }
}
