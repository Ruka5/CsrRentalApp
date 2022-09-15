using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsrRentalApp
{
    public partial class AddEditVehicle : Form
    {
        // trackable which I am going to: is or isn't edit mode
        private bool isEditMode;
        private ManageVehicleListing _manageVehicleListing;
        public readonly CarRentalEntities _db;

        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            this.Text = "Add New Vehicle";
            isEditMode = false;
            _manageVehicleListing = manageVehicleListing;
            _db = new CarRentalEntities();
        }

        public AddEditVehicle(TypesOfCar carToEdit, ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            this.Text = "Edit Vehicle";
            _manageVehicleListing = manageVehicleListing;
            if (carToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities();
                PopulateFields(carToEdit);
            }
        }

        private void PopulateFields(TypesOfCar car)
        {
            lblId.Text = car.Id.ToString();
            tbMake.Text = car.Make;
            tbModel.Text = car.Model;
            tbVIN.Text = car.VIN;
            tbYear.Text = car.Year.ToString();
            tbLicencePlateNumber.Text = car.LicensePlateNumber;
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                // Added Validation for make and model
                if (string.IsNullOrWhiteSpace(tbMake.Text) || string.IsNullOrWhiteSpace(tbModel.Text))
                {
                    MessageBox.Show("Pleas ensure that you provide a make and a model");
                }
                else
                {
                    if (isEditMode)
                    {
                        // Edit Code Here
                        var id = int.Parse(lblId.Text); //actually I don't need to update Id
                        var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);
                        car.Model = tbModel.Text;
                        car.Make = tbMake.Text;
                        car.VIN = tbVIN.Text;
                        car.Year = tbYear.Text == null ? 0 : int.Parse(tbYear.Text);  //convert into int back to db     
                        car.LicensePlateNumber = tbLicencePlateNumber.Text;

                        //_db.SaveChanges();  //this will submit all changes
                        //MessageBox.Show("Update Operation Completed. Refresh Grid To see Changes");
                        //Close();
                    }
                    else
                    {
                        // Added validation for make and model of cars being added
                        var newCar = new TypesOfCar()
                        {
                            LicensePlateNumber = tbLicencePlateNumber.Text,
                            Model = tbModel.Text,
                            Make = tbMake.Text,
                            VIN = tbVIN.Text,
                            Year = int.Parse(tbYear.Text),
                        };

                        _db.TypesOfCars.Add(newCar);
                    }
                }

                _db.SaveChanges();  //this will submit all changes
                _manageVehicleListing.PopulateGrid();
                MessageBox.Show("Operation Completed. Refresh Grid To see Changes");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Close The Window
            Close();
        }
    }
}
