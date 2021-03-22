using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hethongcafe.DTO
{
    public class Account
    {
        public Account(string userName, string displayName, int type, string passWord = null)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.PassWord = passWord;
            this.Type = type;
        }
        public Account(DataRow row)
        {
            this.UserName = row["userName"].ToString();
            this.DisplayName = row["displayName"].ToString();
            this.PassWord = row["passWord"].ToString();
            this.Type = (int)row["type"];
        }

        private int type;
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        private string passWord;
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        private string displayName;
        public string DisplayName
        {
          get { return displayName; }
          set { displayName = value; }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
    }
}
