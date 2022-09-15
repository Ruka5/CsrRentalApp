using System;
using System.Linq;
using System.Windows.Forms;

namespace CsrRentalApp
{
    public partial class AddUser : Form
    {
        private readonly CarRentalEntities _db;
        private ManageUsers _manageUsers;


        public AddUser(ManageUsers manageUsers)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _manageUsers = manageUsers;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {

            var roles = _db.Roles
                //.Select( q => new{ Id = q.id, Name = q.name }  )
                .ToList();

            cbRole.DisplayMember = "Name";
            cbRole.ValueMember = "Id";
            cbRole.DataSource = roles;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var username = tbUsrname.Text;
                var RoleID = (int)cbRole.SelectedValue;
                var password = Utils.DefaultHashPassword();
                var isValid = true;

                var user = new User
                {
                    username = username,
                    password = password,
                    isActive = true
                };

                if (string.IsNullOrWhiteSpace(username))
                {
                    isValid = false;
                    MessageBox.Show("Error: Please enter missing data.\n\r");
                    return;
                }

                _db.Users.Add(user);
                _db.SaveChanges();

                // fetch the id
                var userid = user.id;

                var userRole = new UserRole
                {
                    roleid = RoleID,
                    userid = userid,
                };

                _db.UserRoles.Add(userRole);
                _db.SaveChanges();

                _manageUsers.PopulateGrid();

                MessageBox.Show("New User Added Successfully");
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("An Error Has Occured");
                //throw;
            }
        }
    }
}
