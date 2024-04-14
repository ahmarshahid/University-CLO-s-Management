using iTextSharp.text.pdf;
using iTextSharp.text;
using MidProject_DB.BL;
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
    public partial class ManageStudents : UserControl
    {
        public ManageStudents()
        {
            InitializeComponent();
            load();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            StudentBL St = new StudentBL();
            if (textBox1.Text != "" || textBox2.Text != "" || textBox3.Text != "" || textBox4.Text != "" || textBox5.Text != "")
                St.InsertStudent(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);
            else
                MessageBox.Show("Empty feilds are not allowed! " +
                    "Please enter valid feilds");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                var con = ConfirgurationFile.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select * from Student where status = (Select lookupid from lookup where name ='Active')", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        private void load()
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from Student where status = (Select lookupid from lookup where name ='Active')", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 0) { return; }
            if (e.ColumnIndex == 6) { return; }
            if(e.RowIndex == -1) { return; }
            var con = ConfirgurationFile.getInstance().getConnection();
            con.Open();
            int id = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            if(e.ColumnIndex==1)
            {
                // UPDATE First NAME
                SqlCommand cmd = new SqlCommand("Update Student Set FirstName = @NewFn Where id = @id " , con);
                cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                cmd.Parameters.AddWithValue("@NewFN", dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                cmd.ExecuteNonQuery();
                con.Close();
            }

            if(e.ColumnIndex == 2)
            {
                // UPDATE LAST NAME
                SqlCommand cmd = new SqlCommand("Update Student Set LastName = @NewLN where id = @id)", con);
                cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                cmd.Parameters.AddWithValue("NewLN", dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                cmd.BeginExecuteNonQuery();
                con.Close();
            }

            if (e.ColumnIndex == 3)
            {
                // UPDATE CONTACT NUMBER
                SqlCommand cmd = new SqlCommand("Update Student Set Contact = @NewContact where id = @id", con);
                cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                cmd.Parameters.AddWithValue("NewContact", dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                cmd.BeginExecuteNonQuery();
                con.Close();
            }

            if (e.ColumnIndex == 4)
            {
                // UPDATE EMAIL
                SqlCommand cmd = new SqlCommand("Update Student Set Email = @NewEmail where id = @id", con);
                cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                cmd.Parameters.AddWithValue("NewEmail", dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                cmd.BeginExecuteNonQuery();
                con.Close();
            }

            if (e.ColumnIndex == 5)
            {
                // UPDATE REGISTRATION NUMBER
                SqlCommand cmd = new SqlCommand("Update Student Set RegistrationNumber = @NewRN where id = @id", con);
                cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                cmd.Parameters.AddWithValue("NewRN", dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                cmd.BeginExecuteNonQuery();
                con.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)

        {
            var con = ConfirgurationFile.getInstance().getConnection();
            con.Open();
            int id = (int)dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value;
            SqlCommand cmd = new SqlCommand("Update Student Set Status = (Select Lookupid from Lookup where name = 'InActive') where id = @id", con);
            cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
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

                // Add header information
                PdfPTable headerTable = new PdfPTable(1);
                headerTable.DefaultCell.Border = 0;
                headerTable.WidthPercentage = 100;

                PdfPCell[] headerCells = 
                {
                  new PdfPCell(new Phrase("CLO MANAGING SYSTEM")),
                  new PdfPCell(new Phrase("STUDENT PROGRESS REPORT")),
                  new PdfPCell(new Phrase("Submitted By: Muhammad Ahmar Shahid 2022-CS-206")),
                  new PdfPCell(new Phrase("Submitted TO: Mr.Nazeef-ul-haq"))
                };

                foreach (var cell in headerCells)
                {
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.PaddingBottom = 10f;
                    cell.BackgroundColor = new BaseColor(200, 200, 200);
                    headerTable.AddCell(cell);
                }

                document.Add(headerTable);
                document.Add(new Paragraph(" "));

                // Add the data table from database query
                PdfPTable pdfTable = new PdfPTable(7); // Seven columns for the data
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                try
                {
                    var connection = ConfirgurationFile.getInstance().getConnection(); // Set your connection string
                    connection.Open();
                    string query = "SELECT S.FirstName + ' ' + S.LastName AS StudentName, A.Title AS AssessmentTitle, AC.Name AS ComponentName, AC.TotalMarks AS ComponentTotalMarks, SR.RubricMeasurementId AS RubricMeasurementId, RL.Details AS RubricDetails, SR.EvaluationDate FROM StudentResult SR INNER JOIN Student S ON SR.StudentId = S.Id INNER JOIN AssessmentComponent AC ON SR.AssessmentComponentId = AC.Id INNER JOIN Assessment A ON AC.AssessmentId = A.Id LEFT JOIN RubricLevel RL ON SR.RubricMeasurementId = RL.Id ORDER BY S.FirstName, S.LastName, A.Title, AC.Name, SR.EvaluationDate ";
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

                // Add header information
                PdfPTable headerTable = new PdfPTable(1);
                headerTable.DefaultCell.Border = 0;
                headerTable.WidthPercentage = 100;

                PdfPCell[] headerCells =
                {
                  new PdfPCell(new Phrase("CLO MANAGING SYSTEM")),
                  new PdfPCell(new Phrase("STUDENT PROGRESS REPORT")),
                  new PdfPCell(new Phrase("Submitted By: Muhammad Ahmar Shahid 2022-CS-206")),
                  new PdfPCell(new Phrase("Submitted TO: Mr.Nazeef-ul-haq"))
                };

                foreach (var cell in headerCells)
                {
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.PaddingBottom = 10f;
                    cell.BackgroundColor = new BaseColor(200, 200, 200);
                    headerTable.AddCell(cell);
                }

                document.Add(headerTable);
                document.Add(new Paragraph(" "));

                // Add the data table from database query
                PdfPTable pdfTable = new PdfPTable(6); // Seven columns for the data
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                try
                {
                    var connection = ConfirgurationFile.getInstance().getConnection(); // Set your connection string
                    connection.Open();
                    string query = "SELECT S.Id AS StudentId, CONCAT(S.FirstName, ' ', S.LastName) AS Name, S.Contact, S.Email, S.RegistrationNumber, L1.Name AS StudentStatus FROM Student S LEFT JOIN Lookup L1 ON S.Status = L1.LookupId ORDER BY S.Id";
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
