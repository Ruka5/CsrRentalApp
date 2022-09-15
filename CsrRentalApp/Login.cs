using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsrRentalApp
{
    public partial class Login : Form
    {
        // trackable which I am going to: is or isn't edit mode
        public readonly CarRentalEntities _db;
        public Login()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Only use one hashing algorithm
                SHA256 sha = SHA256.Create();

                string userName = tbUserName.Text.Trim();
                string password = tbPassword.Text;
                
                var hashed_password = Utils.HashedPassword(password);

                // Check for matching username , password and active flag
                var user = _db.Users.FirstOrDefault(q => q.username == userName
                    && q.password == hashed_password);

                
                if (user == null)
                {
                    MessageBox.Show("Please provide valid credentials");
                    return;
                }
                if(user.isActive == false)
                {
                    MessageBox.Show("Account is deactivated");
                    return;
                }
                else
                {
                    //var role = user.UserRoles.FirstOrDefault(); //give me the first one
                    //var roleShortName = role.Role.shortname;
                    var mainWindow = new MainWindow(this, user);
                    mainWindow.Show();
                    Hide();
                }

                //_db.Users.Add(user);
                //_db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Something get wrong");
            }
        }
    }
}
