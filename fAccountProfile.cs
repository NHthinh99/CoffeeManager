using hethongcafe.DAO;
using hethongcafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hethongcafe
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }
        public string userName { get; set; }

        public fAccountProfile(Account acc)
        {
            InitializeComponent();
            LoginAccount = acc;
        }
        void ChangeAccount(Account acc)
        {
            txtUserName.Text = LoginAccount.UserName;
            txtDisplayName.Text = LoginAccount.DisplayName;          
        }
        void UpdateAccount()
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            string passWord = txtPassWord.Text;
            string newPass = txtNewPass.Text;
            string reEnterPass = txtRePassWord.Text;

            if (!newPass.Equals(reEnterPass))
            {
                MessageBox.Show("Nhập lại mật khẩu đúng vs mật khẩu mới!");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(userName, displayName, passWord, newPass))
                {
                    MessageBox.Show("Cập nhật thành công!");
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!");
                }
            }
        }
        private void ThongTInCaNhan_Load(object sender, EventArgs e)
        {      
            txtDisplayName.Text = loginAccount.DisplayName;
        }
        private void btThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btCapNhat_Click(object sender, EventArgs e)
        {
            UpdateAccount();
        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }       
    }
}
