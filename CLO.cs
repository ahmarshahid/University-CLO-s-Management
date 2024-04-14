using iTextSharp.text.pdf;
using iTextSharp.text;
using MidProject_DB.BL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidProject_DB
{
    public partial class CLO : UserControl
    {
        public CLO()
        {
            InitializeComponent();
            display();
        }


        private void display()
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("Select * from CLO where left(name,4)<>'rm*-'", con);
            SqlCommand cmd = new SqlCommand("Select * from CLO where left(name,4)<>'rm*-'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            con.Open();
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please enter a valid feild !");
                return;
            }
            SqlCommand cmd2 = new SqlCommand("Select COUNT(*) FROM Clo WHERE Name=@checkName", con);
            cmd2.Parameters.AddWithValue("CheckName", textBox1.Text);
            int cnt = (int)cmd2.ExecuteScalar();
            if (cnt > 0)
            {
                con.Close();
                MessageBox.Show("Name invalid");
                return;
            }
            SqlCommand cmd = new SqlCommand("Insert into Clo values (@Name,@DateCreated,@DateUpdated)", con);
            cmd.Parameters.AddWithValue("@Name", textBox1.Text);
            cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
            cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
            MessageBox.Show("Sucessfully Added");
            cmd.ExecuteNonQuery();
            con.Close();

        }




        private void button2_Click(object sender, EventArgs e)
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("Select * from CLO where left(name,4)<>'rm*-'", con);
            SqlCommand cmd = new SqlCommand("Select * from CLO where left(name,4)<>'rm*-'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) { return; }
            if (e.ColumnIndex == 6) { return; }
            if (e.RowIndex == -1) { return; }
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();
            if (e.ColumnIndex == 1)
            {
                // UPDATE NAME
                SqlCommand cmd = new SqlCommand("Update CLO Set Name = @NewName,DateUpdated = @NewDate Where id = @id ", connection);
                cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                cmd.Parameters.AddWithValue("@NewName", dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                cmd.Parameters.AddWithValue("@NewDate", DateTime.Now);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();

            int id = (int)dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value;
            SqlCommand cmd = new SqlCommand("Update Clo Set name= @newName where id = @id", connection);
            cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value);
            cmd.Parameters.AddWithValue("@newName", "rm*-" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value);


            cmd.ExecuteNonQuery();
            connection.Close();
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

                    PdfPCell headerCell2 = new PdfPCell(new Phrase("CLO WISE Report"));
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
                    PdfPTable pdfTable = new PdfPTable(2); // Assuming you're joining two tables
                    pdfTable.DefaultCell.Padding = 3;
                    pdfTable.WidthPercentage = 100;
                    pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                    try
                    {
                        var connection = ConfirgurationFile.getInstance().getConnection(); // Set your connection string
                        connection.Open();
                        string query = "Select c.Id AS CLOId, c.Name AS CloName, r.Id As RubricID, r.Details AS Rubric_Details, r1.Details As Rubric_Level_Details, r1.MeasurementLevel As Rubric_Measurement_Level FROM Clo c INNER JOIN Rubric r on c.Id = r.CloId LEFT JOIN RubricLevel r1 ON r.Id = r1.RubricId ORDER BY c.ID, r.Id, r1.Id"; // Set your SQL query
                    string query2 = "SELECT C.Name AS CLOName, A.Title AS AssessmentTitle, AC.Name AS ComponentName, AC.TotalMarks AS ComponentTotalMarks, S.FirstName + ' ' + S.LastName AS StudentName, S.RegistrationNumber, SR.RubricMeasurementId, RL.Details AS RubricDetails FROM CLO C INNER JOIN Assessment A ON C.Id = A.CLOId INNER JOIN AssessmentComponent AC ON A.Id = AC.AssessmentId INNER JOIN StudentResult SR ON AC.Id = SR.AssessmentComponentId INNER JOIN Student S ON SR.StudentId = S.Id LEFT JOIN RubricLevel RL ON SR.RubricMeasurementId = RL.Id ORDER BY C.Name, A.Title, AC.Name, S.FirstName, S.LastName";


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
