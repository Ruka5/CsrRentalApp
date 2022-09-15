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
    public partial class MainWindow : Form
    {
        private Login _login;
        private User _user;
        private string _roleName;


        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(Login login, User user)
        {
            InitializeComponent();
            _login = login;
            _user = user;
            _roleName = _user.UserRoles.FirstOrDefault().Role.shortname;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (_user.password == Utils.DefaultHashPassword())
            {
                var resetPassword = new ResetPassword(_user);
                resetPassword.ShowDialog();
            }

            var username = _user.username;
            tsiLoginText.Text = $"Logged In As: {username}";
            if(_roleName != "admin")
            {
                manageUsersToolStripMenuItem.Visible = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //AddEditRentalRecord form1 = new AddEditRentalRecord();
            this.Close();
            //form1.ShowDialog();
        }

        private void addRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord();
            // this is manifestation of object MainWindow here
            addRentalRecord.ShowDialog(); // I can't do anything behind the window until I close it
            addRentalRecord.MdiParent = this;   // child window stay inside borders of parent
        }

        private void manageVehicleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("ManageVehicleListing"))
            {
                var vehicleListing = new ManageVehicleListing();
                vehicleListing.MdiParent = this;
                vehicleListing.Show();
            }
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("ManageRentalRecords"))
            {
                var manageRentalRecords = new ManageRentalRecords();
                manageRentalRecords.MdiParent = this;
                manageRentalRecords.Show();
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TODO when Login activated
            _login.Close();
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("ManageUsers"))
            {
                var manageUsers = new ManageUsers();
                manageUsers.MdiParent = this;
                manageUsers.Show();
            }
        }
    }
}
