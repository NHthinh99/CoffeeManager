using hethongcafe.DAO;
using hethongcafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hethongcafe
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;
        public Account LoginAccount
        {
          get { return loginAccount; }
          set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }   
        public fTableManager(Account acc)
        {
            InitializeComponent();
            this.loginAccount = acc;
            LoadTable();
            LoadCategory();
            LoadComboBoxTable(cbSwitchTable);
        }
        #region Method
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }
        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();
            foreach(Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableHeight, Height = TableDAO.TableHeight};
                btn.Text = item.Name + Environment.NewLine + item.Status;

                btn.Click += btn_Click;
                btn.Tag = item;
                switch (item.Status)
                {
                    case "Có người":
                        btn.BackColor = Color.Pink;                  
                        break;
                    default:
                        btn.BackColor = Color.Aqua;
                        break;
                }
                flpTable.Controls.Add(btn);
            }
        }
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<hethongcafe.DTO.Menu> listbillinfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (hethongcafe.DTO.Menu item in listbillinfo)
            {
                ListViewItem lvitem = new ListViewItem(item.FoodName.ToString());
                lvitem.SubItems.Add(item.Count.ToString());
                lvitem.SubItems.Add(item.Price.ToString());
                lvitem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lvitem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            txtTotalPrice.Text = totalPrice.ToString("c", culture);
        }
        void LoadComboBoxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }
        #endregion      

        #region Events
        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void thôngTinCáNhậnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);         
            f.ShowDialog();
        }
        private void flpTable_Paint(object sender, PaintEventArgs e)
        {

        }        
        private void aDMINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin ad = new Admin();
            ad.loginAccount = LoginAccount;
            ad.InsertFood += Admin_InsertFood;
            ad.DeleteFood += Admin_DeleteFood;
            ad.UpdateFood += Admin_UpdateFood;
            ad.ShowDialog();
        }
        private void Admin_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }
        private void Admin_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }
        private void Admin_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if(lsvBill.Tag!=null)
                ShowBill((lsvBill.Tag as Table).ID);
        }
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null)
                return;
            Category selected = cb.SelectedItem as Category;
            id = selected.ID;
            LoadFoodListByCategoryID(id);
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn", "Thông báo");
                return;
            }

            int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }
            ShowBill(table.ID);
            LoadTable();
        }
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.ID);
            int discount = (int)nmDisCount.Value;

            double totalPrice = Convert.ToDouble(txtTotalPrice.Text.Split(',')[0]);
            double finalTotalPrice = totalPrice - (totalPrice / 100)*discount;

            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc muốn thanh toán hóa đơn cho bàn {0}\n Tổng tiền - (Tổng tiền / 100) x Giảm giá\n=> {1} - ({1} / 100) x {2} = {3}", table.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {                   
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    ShowBill(table.ID);
                    LoadTable();
                }
            }
        }
        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).ID;
            int id2 = (cbSwitchTable.SelectedItem as Table).ID;

            if (MessageBox.Show(string.Format("Bạn có thực sự muốn chuyển bàn {0} qua bàn {1}", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                LoadTable();
            }
        }
        #endregion                
    }
}
