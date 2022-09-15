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
    public partial class ManageRentalRecords : Form
    {
        public readonly CarRentalEntities _db;
        public ManageRentalRecords()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddEditRentalRecord
            {
                // this is manifestation of object MainWindow here
                MdiParent = this.MdiParent
            };
            addRentalRecord.Show();
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
             try
            {
                 // get Id of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                var record = _db.CarRentalRecords.FirstOrDefault( q => q.id == id );

                // launch AddEditVehicle window with data
                var addEditRentalRecord = new AddEditRentalRecord(record);
                addEditRentalRecord.MdiParent = this.MdiParent;
                addEditRentalRecord.Show();
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
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
             try
            {
                // get Id of selected row
            var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;

            // query database for record
            var record = _db.CarRentalRecords.FirstOrDefault( q => q.id == id );

            // delete vehicle from table
            _db.CarRentalRecords.Remove(record);
            _db.SaveChanges();

            PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }             
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void ManageRentalRecords_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

         private void PopulateGrid()
        {
           // Select a custom model collection of cars from database
            var records = _db.CarRentalRecords
                .Select(q => new
                {
                    Customer = q.CustomerName,
                    DateOut = q.DateRented,
                    DateIn = q.DateReturned,
                    q.Cost,
                    Id = q.id,
                    
                    // Inner Join
                    Car = q.TypesOfCar.Make + " " + q.TypesOfCar.Model
                })
                .ToList();
            gvRecordList.DataSource = records;
            gvRecordList.Columns["DateIn"].HeaderText = "Date In";
            gvRecordList.Columns["DateOut"].HeaderText = "Date Out";
            //Hide the column for ID. Changed from the hard coded column value to the name, 
            // to make it more dynamic. 
            gvRecordList.Columns["Id"].Visible = false;
        }
    }
}
