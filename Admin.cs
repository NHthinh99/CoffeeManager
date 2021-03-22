using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using hethongcafe.DAO;
using hethongcafe.DTO;

namespace hethongcafe
{
    public partial class Admin : Form
    {
        BindingSource foodlist = new BindingSource();

        BindingSource accountList = new BindingSource();

        BindingSource categoryList = new BindingSource();

        public Account loginAccount;
        public Admin()
        {
            InitializeComponent();
            LoadAll();
        }
        #region methods

        List<Food> SearchFoodByName(string name)
        {
            List<Food> listfood = FoodDAO.Instance.SearchFoodByName(name);
            return listfood;
        }
        void LoadAll()
        {
            dtgvFood.DataSource = foodlist;
            dtgvAccount.DataSource = accountList;
            dtgvCategory.DataSource = categoryList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromTime.Value, dtpkToTime.Value);
            LoadListFood();
            LoadCategory();
            LoadAccount();
            AddFoodBinding();
            LoadCategoryIntoComboBox(cbFoodCategory);
            AddAccountBinding();
        }

        void AddCategory()
        {
            txtCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }
        void LoadCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }
        void AddAccountBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromTime.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToTime.Value = dtpkFromTime.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }
        void AddFoodBinding()
        {
            txtFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txtFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        void LoadCategoryIntoComboBox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name"; 
        }
        void LoadListFood()
        {
            foodlist.DataSource = FoodDAO.Instance.GetListFood();
        }
        void AddCategory(int id, string name)
        {
            if (CategoryDAO.Instance.InsertCategory(id, name))
            {
                MessageBox.Show("Thêm danh mục thành công!");
            }
            else
            {
                MessageBox.Show("Thêm danh mục thất bại!");
            }
            LoadCategory();
        }
        void EditCategory(int id, string name)
        {
            if (CategoryDAO.Instance.UpdateCategory(id, name))
            {
                MessageBox.Show("Sửa danh mục thành công!");
            }
            else
            {
                MessageBox.Show("Sửa danh mục thất bại!");
            }
            LoadCategory();
        }
        void DeleteCategory(int id)
        {
            if (CategoryDAO.Instance.DeleteCategory(id))
            {
                MessageBox.Show("Xóa danh mục thành công!");
            }
            else
            {
                MessageBox.Show("Xóa danh mục thất bại!");
            }
            LoadCategory();
        }
        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại!");
            }
            LoadAccount();
        }
        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại!");
            }
            LoadAccount();
        }
        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Đừng có xóa chính bản thân bạn chứ!");
                return;
            }

            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại!");
            }
            LoadAccount();
        }
        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công!");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại!");
            }
        }
        #endregion

        #region Events
        private void btnDeleteCategoty_Click(object sender, EventArgs e)
        {
        
        }
        private void button10_Click(object sender, EventArgs e)
        {
            
        }
        private void btnEditCategory_Click(object sender, EventArgs e)
        {
           
        }

        private void btnAddAccount_Click_1(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int)nmAccountType.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click_1(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click_1(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int)nmAccountType.Value;

            EditAccount(userName, displayName, type);
        }

        private void btnResetPassword_Click_1(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            ResetPass(userName);
        }
        private void btnShowAccount_Click_1(object sender, EventArgs e)
        {
            LoadAccount();
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodlist.DataSource = SearchFoodByName(txtSearchFoodName.Text);
        }
        private void txtCategoryID_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void txtFoodID_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);
                    cbFoodCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;

                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }
        }
        private void btnAddFood_Click_1(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món");
            }
        }
        private void btnEditFood_Click_1(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txtFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món");
            }
        }
        private void btnDeleteFood_Click_1(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deletetFood != null)
                    deletetFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa món");
            }
        }
        private void btnShowFood_Click_1(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromTime.Value, dtpkToTime.Value);
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deletetFood;
        public event EventHandler DeleteFood
        {
            add { deletetFood += value; }
            remove { deletetFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        #endregion

        private void Admin_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'QuanLyQuanCafeDataSet.USP_GetListBillByDate' table. You can move, or remove it, as needed.

            this.reportViewer1.RefreshReport();
        }
    }
}
