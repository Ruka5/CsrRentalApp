using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsrRentalApp
{
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode;
        //property of entire carRentalEntities data model
        private readonly CarRentalEntities _db;

        public AddEditRentalRecord()
        {
             InitializeComponent();
            lblTitle.Text = "Add New Rental Record";
            this.Text = "Add New Rental Record";
            isEditMode = false;
            _db = new CarRentalEntities();
        }

        public AddEditRentalRecord(CarRentalRecord recordToEdit)
        {
           InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            this.Text = "Edit Rental Record";
            if (recordToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities();
                PopulateFields(recordToEdit);
            }
        }
           private void PopulateFields(CarRentalRecord recordToEdit)
        {
            tbCustomerName.Text = recordToEdit.CustomerName;
            dtDateRented.Value = (DateTime)recordToEdit.DateRented;
            dtDateReturned.Value = (DateTime)recordToEdit.DateReturned;
            tbCost.Text = recordToEdit.Cost.ToString();
            cbTypeOfCar.Text = recordToEdit.TypeOfCarId.ToString();
            lblRecorId.Text = recordToEdit.id.ToString();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = tbCustomerName.Text;
                var dateOut = dtDateRented.Value;
                var dateIn = dtDateReturned.Value;
                double cost = Convert.ToDouble(tbCost.Text);

                var carType = cbTypeOfCar.Text;
                var isValid = true;
                var errorMessage = "";

                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    errorMessage += "Error: Please enter missing data.\n\r";
                }
                if (dateOut > dateIn)
                {
                    isValid = false;
                    errorMessage += "Error: Illegal Date Selection\n\r";
                }

                //if(isValid == true)
                if (isValid)
                {
                    // Declare an object of the record to be added
                    var rentalRecord = new CarRentalRecord();
                    if (isEditMode)
                    {
                        // If is edit mode, then get the ID and retreive the record from the database and place
                        // the result in the record object
                        var id = int.Parse(lblRecorId.Text);
                        rentalRecord = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);
                    }
                    // Populate record object with values from the form
                    rentalRecord.CustomerName = customerName;
                    rentalRecord.DateRented = dateOut;
                    rentalRecord.DateReturned = dateIn;
                    rentalRecord.Cost = (decimal)cost;
                    rentalRecord.TypeOfCarId = (int)cbTypeOfCar.SelectedValue;
                    // If not in edit mode, then add record object to the database
                    if (!isEditMode)
                    {
                        _db.CarRentalRecords.Add(rentalRecord);
                    }
                    // Save Changes made to the entity
                    _db.SaveChanges();

                    MessageBox.Show($"Customer Name: {customerName}\n\r" +
                    $"Date Rented: {dateOut}\n\r" +
                    $"Date Returned: {dateIn}\n\r" +
                    $"Cost: {cost}\n\r" +
                    $"Car Type: {carType}\n\r\"" +
                    $"THANK YOU FOR YOUR BUSINESS");

                    Close();    

                    //if (isEditMode)
                    //{
                    //    var id = int.Parse(lblRecorId.Text);
                    //    var rentalRecord = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);
                    //    rentalRecord.CustomerName = customerName;
                    //    rentalRecord.DateRented = dateOut;
                    //    rentalRecord.DateReturned = dateIn;
                    //    rentalRecord.Cost = (decimal)cost;
                    //    rentalRecord.TypeOfCarId = (int)cbTypeOfCar.SelectedValue;

                    //    _db.SaveChangesAsync();

                    //    MessageBox.Show($"Customer Name: {customerName}\n\r" +
                    //   $"Date Rented: {dateOut}\n\r" +
                    //   $"Date Returned: {dateIn}\n\r" +
                    //   $"Cost: {cost}\n\r" +
                    //   $"Car Type: {carType}\n\r\"" +
                    //   $"THANK YOU FOR YOUR BUSINESS");
                    //}
                    //else
                    //{
                    //    //create record
                    //    var rentalRecord = new CarRentalRecord();
                    //    rentalRecord.CustomerName = customerName;
                    //    rentalRecord.DateRented = dateOut;
                    //    rentalRecord.DateReturned = dateIn;
                    //    rentalRecord.Cost = (decimal)cost;
                    //    rentalRecord.TypeOfCarId = (int)cbTypeOfCar.SelectedValue;

                    //    //save record
                    //    _db.CarRentalRecords.Add(rentalRecord);
                    //    _db.SaveChangesAsync();


                    //    MessageBox.Show($"Customer Name: {customerName}\n\r" +
                    //    $"Date Rented: {dateOut}\n\r" +
                    //    $"Date Returned: {dateIn}\n\r" +
                    //    $"Cost: {cost}\n\r" +
                    //    $"Car Type: {carType}\n\r\"" +
                    //    $"THANK YOU FOR YOUR BUSINESS");
                    //}   
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //throw;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Select * from TypesOfCars
            //var cars = carRentalEntities.TypesOfCars.ToList();
            //cbTypeOfCar.DisplayMember = "Name";
            //cbTypeOfCar.ValueMember = "Id";

            var cars = _db.TypesOfCars
                .Select( q => new{ Id = q.Id, Name = q.Make + " " + q.Model }  )
                .ToList();

            cbTypeOfCar.DisplayMember = "Name";
            cbTypeOfCar.ValueMember = "Id";
            cbTypeOfCar.DataSource = cars;
        }
    }
}
