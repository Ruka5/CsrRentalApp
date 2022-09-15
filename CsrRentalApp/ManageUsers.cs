using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsrRentalApp
{
    public partial class ManageUsers : Form
    {
        private readonly CarRentalEntities _db;
        public ManageUsers()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            PopulateGrid();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddUser"))
            {
                AddUser addUser = new AddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }            
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {            
            try
            {
                // get Id of selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;

                // query database for record
                var user = _db.Users.FirstOrDefault(q => q.id == id);
                var hashed_password  = Utils.DefaultHashPassword();
                user.password = hashed_password;
                _db.SaveChanges();
                MessageBox.Show($"{user.username}'s Password has been reset!");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show(
                    "Click on the left panel and assign the right row",
                    "window title",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            PopulateGrid();
        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;

                // query database for record
                var user = _db.Users.FirstOrDefault(q => q.id == id);

                //if(user.isActive == true)
                //  user.isActive == false;
                //else
                //  user.isActive == true;
                user.isActive = user.isActive == true ? false : true;
                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s active status has changed!");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show(
                    "Click on the left panel and assign the right row",
                    "window title",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            // Select a custom model collection of cars from database
            var users = _db.Users
                .Select(q => new
                {
                    q.username,
                    q.UserRoles.FirstOrDefault().Role.name,
                    q.isActive,
                    q.id
                })
                .ToList();
            gvUserList.DataSource = users;
            gvUserList.Columns["id"].Visible = false;
            gvUserList.Columns["username"].HeaderText = "Username";
            gvUserList.Columns["name"].HeaderText = "Role name";
            gvUserList.Columns["isActive"].HeaderText = "Active";

            //var er = "";
            //foreach (var user in gvUserList.Columns.)
            //{
            //    er += user + "\r\n"; 
            //}
            //MessageBox.Show(er);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}
