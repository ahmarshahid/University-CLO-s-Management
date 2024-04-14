using Guna.UI2.WinForms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidProject_DB
{
    public partial class Attendance : UserControl
    {
        public Attendance()
        {
            InitializeComponent();
            addItemtoComboBox2();
            addinCombo3();
            combobox1();
            DisplayIntoDataGridViewofStudentAttendance();
            DisplayIntoDataGridViewOfClassAttendance();
        }


        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            DisplayIntoDataGridViewofStudentAttendance();
            DisplayIntoDataGridViewOfClassAttendance();
        }


        private void DisplayIntoDataGridViewofStudentAttendance()
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("(Select * from Rubric )", con);
            SqlCommand cmd = new SqlCommand("Select * from StudentAttendance", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }
        private void DisplayIntoDataGridViewOfClassAttendance()
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("(Select * from Rubric )", con);
            SqlCommand cmd = new SqlCommand("Select * from ClassAttendance", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            con.Close();
        }

        private void addItemtoComboBox2()
        {
           
                var connection = ConfirgurationFile.getInstance().getConnection();
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select id from Student", connection);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                   // comboBox2.Items.Add(r.GetString(r.GetOrdinal("id")));
                comboBox2.Items.Add(r["id"]);
                }
                r.Close();
                connection.Close();
            
        }
        private void addinCombo3()
        {
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();
            SqlCommand cmd = new SqlCommand("Select Name from Lookup where Category = 'Attendance_Status'", connection);
            
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
               // comboBox3.Items.Add(r.GetString(r.GetOrdinal("id")));
                comboBox3.Items.Add(r["Name"]);
            }
            r.Close();
            connection.Close();
        }

        private void combobox1()
        {
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();
            SqlCommand cmd = new SqlCommand("Select id from ClassAttendance ", connection);
            //cmd.Parameters.AddWithValue("@Category", "ATTENDANCE_STATUS");
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                //comboBox1.Items.Add(r.GetString(r.GetOrdinal("id")));
                comboBox1.Items.Add(r["id"]);
                
            }
            r.Close();
            connection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();
            SqlCommand cmd = new SqlCommand("Insert into ClassAttendance values(@Date)", connection);
            cmd.Parameters.AddWithValue("Date", dateTimePicker1.Value.Date);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully Added Attendance Date");
            connection.Close();


        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            con.Open();
            
            /*SqlCommand cmd2 = new SqlCommand("Select count(*) FROM RubricLevel WHERE details=@detels", con);
            SqlCommand command = new SqlCommand("Select count(*) FROM RubricLevel where measurement = @measurement", con);
            cmd2.Parameters.AddWithValue("detels", guna2TextBox1.Text);
            command.Parameters.AddWithValue("measurement", guna2TextBox2.Text);
            int cnt = (int)cmd2.ExecuteScalar();
            //int count = (int)command.ExecuteScalar();

            if (cnt > 0)
            {
                con.Close();
                MessageBox.Show("Details are invalid");
                return;
            }
*/

           // SqlCommand cmd = new SqlCommand("Insert into RubricLevel values ((SELECT id from Rubric where details = @RubricID),@Details,@measurementLevel)", con);
            SqlCommand cmd = new SqlCommand("Insert into StudentAttendance values(@AttendanceId,@StudentId,@AttendanceStatus)", con);
            cmd.Parameters.AddWithValue("@AttendanceId", comboBox1.SelectedItem);
            cmd.Parameters.AddWithValue("@StudentId", comboBox2.SelectedItem);
            cmd.Parameters.AddWithValue("@AttendanceStatus", convertAttendance(comboBox3.SelectedItem.ToString()));

            MessageBox.Show("Attendance Marked");
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private int convertAttendance(string str)
        {
            if (str == "Present")
                return 1;
            else if (str == "Absent")
                return 2;
            else if (str == "Leave")
                return 3;
            else
                return 4;//late
        }


        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
                // UPDATE AttendanceStatus of Student Attendance
            if (e.ColumnIndex == 0) { return; }
            if (e.ColumnIndex == 6) { return; }
            if (e.RowIndex == -1) { return; }
            var con = ConfirgurationFile.getInstance().getConnection();
            con.Open();
            int id = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            int Status = convertAttendance(comboBox3.SelectedItem.ToString());

            if (e.ColumnIndex == 2)
            {
                SqlCommand cmd = new SqlCommand("Update StudentAttendance Set AttendanceStatus = @NewStatus Where AttendanceId = @id ", con);
                cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                cmd.Parameters.AddWithValue("@NewStatus", Status);
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (.pdf)|.pdf";
            saveFileDialog.Title = "Export to PDF";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));

                document.Open();

                // Add a header table
                PdfPTable headerTable = new PdfPTable(1);
                headerTable.DefaultCell.Border = 0;
                headerTable.WidthPercentage = 100;

                // Header cell styles
                PdfPCell headerCell1 = new PdfPCell(new Phrase("CLO MANAGING SYSTEM"));
                headerCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCell1.PaddingBottom = 10f; // Add padding to the bottom
                headerCell1.BackgroundColor = new BaseColor(200, 200, 200); // Set background color
                headerTable.AddCell(headerCell1);

                PdfPCell headerCell2 = new PdfPCell(new Phrase("Attendance Report"));
                headerCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCell2.PaddingBottom = 10f;
                headerCell2.BackgroundColor = new BaseColor(200, 200, 200);
                headerTable.AddCell(headerCell2);

                PdfPCell headerCell3 = new PdfPCell(new Phrase("Submitted By: Muhammad Ahmar Shahid 2022-CS-206"));
                headerCell3.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCell3.PaddingBottom = 10f;
                headerCell3.BackgroundColor = new BaseColor(200, 200, 200);
                headerTable.AddCell(headerCell3);

                PdfPCell headerCell4 = new PdfPCell(new Phrase("Submitted TO: Mr.Nazeef-ul-haq"));
                headerCell4.HorizontalAlignment = Element.ALIGN_CENTER;
                headerCell4.PaddingBottom = 10f;
                headerCell4.BackgroundColor = new BaseColor(200, 200, 200);
                headerTable.AddCell(headerCell4);

                document.Add(headerTable);

                // Add a spacer line between header and data table
                document.Add(new Paragraph(" "));

                // Add the data table from database query
                PdfPTable pdfTable = new PdfPTable(5); // Five columns for the data
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                try
                {
                    var connection = ConfirgurationFile.getInstance().getConnection(); // Set your connection string
                    connection.Open();
                    string query = "SELECT SA.AttendanceId, S.Id AS StudentId, S.FirstName + ' ' + S.LastName AS StudentName, CASE WHEN L.Name = 'Present' THEN 'Present' WHEN L.Name = 'Absent' THEN 'Absent' WHEN L.Name = 'Leave' THEN 'Leave' WHEN L.Name = 'Late' THEN 'Late' ELSE 'Unknown' END AS AttendanceStatus, CA.AttendanceDate FROM StudentAttendance SA INNER JOIN Student S ON SA.StudentId = S.Id INNER JOIN ClassAttendance CA ON SA.AttendanceId = CA.Id INNER JOIN Lookup L ON SA.AttendanceStatus = L.LookupId ORDER BY CA.AttendanceDate, S.FirstName, S.LastName";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    // Add headers
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(reader.GetName(i)));
                        cell.BackgroundColor = new BaseColor(150, 150, 150);
                        cell.Padding = 5f;
                        pdfTable.AddCell(cell);
                    }

                    // Add data
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string cellText = reader[i].ToString();
                            PdfPCell dataCell = new PdfPCell(new Phrase(cellText));
                            dataCell.Padding = 5f;
                            pdfTable.AddCell(dataCell);
                        }
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while retrieving data: " + ex.Message);
                }

                document.Add(pdfTable);
                document.Close();

                MessageBox.Show("PDF file has been created!");
            }
        }
    }
}
