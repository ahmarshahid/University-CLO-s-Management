using Guna.UI2.WinForms;
using iTextSharp.text.pdf.draw;
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
    public partial class StudentResults : UserControl
    {
        public StudentResults()
        {
            InitializeComponent();
            addincomboBox1();
            addincomboBox2();
            displaytable();
        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.studentResultTableAdapter.FillBy(this.projectBDataSet11.StudentResult);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
        private void addincomboBox1()
        {
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();
            SqlCommand cmd = new SqlCommand("Select ID from Student where Status = (Select Lookupid from lookup where name = 'Active')", connection);
            SqlDataReader r = cmd.ExecuteReader();
            
            while (r.Read())
            {
                //comboBox1.Items.Add(r.GetString(r.GetOrdinal("ID")));
                comboBox1.Items.Add(r["ID"]);
            }
            r.Close();
            connection.Close();
        }

        private void addincomboBox2()
        {
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();
            SqlCommand cmd = new SqlCommand("Select Name from AssessmentComponent where left(Name,4) <> 'rm*-'", connection);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                comboBox2.Items.Add(r.GetString(r.GetOrdinal("Name")));
            }
            r.Close();
            connection.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();
            SqlCommand cmd = new SqlCommand($"Select Details FROM rubriclevel where rubriclevel.rubricid = (Select rubricID from AssessmentComponent where AssessmentComponent.Name = '{comboBox2.SelectedItem.ToString()}')"  , connection);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                comboBox3.Items.Add(r.GetString(r.GetOrdinal("Details")));
            }
            r.Close();
            connection.Close();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            //DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("Insert into StudentResult values (@ID,(SELECT ID FROM ASSESSMENTCOMPONENT WHERE NAME = @NAME),(Select ID from RubricLevel where details = @details),@EvaluationDate)", con);

            cmd.Parameters.AddWithValue("@ID", comboBox1.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@NAME", comboBox2.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@details", comboBox3.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@EvaluationDate", DateTime.Today);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(dt);
            MessageBox.Show("Sucessfully Added");
            
            cmd.ExecuteNonQuery();
            calculateComponentMarks();

            //dataGridView1.DataSource = dt;

            con.Close();
        }
        private void calculateComponentMarks()
        {
            var con = ConfirgurationFile.getInstance().getConnection();

            SqlCommand cmd = new SqlCommand("SELECT (SELECT MeasurementLevel FROM RubricLevel WHERE RubricLevel.Id=StudentResult.RubricMeasurementId)/(Select Max(MeasurementLevel) FROM RubricLevel WHERE RubricLevel.rubricId=(SELECT rubricId from AssessmentComponent WHERE AssessmentComponent.Id=StudentResult.AssessmentComponentId))*(SELECT TotalMarks from AssessmentComponent WHere AssessmentComponent.Id=StudentResult.AssessmentComponentId) From StudentResult\r\n", con);
            object res = cmd.ExecuteScalar();
            float result = float.Parse(res.ToString());
            textBox1.Text = result.ToString();
        }

        private void displaytable()
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("(Select * from Rubric )", con);
            SqlCommand cmd = new SqlCommand("Select * from StudentResult", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }


        private void ExportToPDF(DataGridView dgv, string name, string headingText)
        {
            try
            {
                // Create PDF document
                using (Document document = new Document(PageSize.A4, 20, 20, 20, 20))
                {
                    // Initialize PDF writer
                    PdfWriter.GetInstance(document, new FileStream(name + ".pdf", FileMode.Create));

                    // Open document
                    document.Open();

                    // Add heading
                    Paragraph heading = new Paragraph(headingText, FontFactory.GetFont("Times New Roman", 18, iTextSharp.text.Font.BOLD))
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingBefore = 10f,
                        SpacingAfter = 10f
                    };
                    document.Add(heading);

                    // Add line separator
                    document.Add(new LineSeparator());

                    // Add course information
                    Paragraph course = new Paragraph(headingText, FontFactory.GetFont("Times New Roman", 12))
                    {
                        Alignment = Element.ALIGN_CENTER,
                        IndentationLeft = 55f,
                        SpacingAfter = 20f
                    };
                    document.Add(course);

                    // Add line separator
                    document.Add(new LineSeparator());

                    // Create PDF table
                    PdfPTable table = new PdfPTable(dgv.Columns.Count)
                    {
                        WidthPercentage = 100
                    };

                    // Add column headers
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        table.AddCell(new PdfPCell(new Phrase(column.HeaderText)));
                    }

                    // Add data rows
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            table.AddCell(new PdfPCell(new Phrase(cell.Value?.ToString())));
                        }
                    }

                    // Add table to document
                    document.Add(table);

                    // Close document
                    document.Close();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("An error occurred while exporting to PDF: " + exp.Message);
            }
        }
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            displaytable();
        }
        private void guna2GradientButton3_Click(object sender, EventArgs e)
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

                PdfPCell headerCell2 = new PdfPCell(new Phrase("Result WISE Report"));
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
                PdfPTable pdfTable = new PdfPTable(3); // Assuming you're joining two tables
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                try
                {
                    var connection = ConfirgurationFile.getInstance().getConnection(); // Set your connection string
                    connection.Open();
                    string query = "SELECT AC.Name AS AssessmentComponentName, SR.RubricMeasurementId, CONCAT(S.FirstName, ' ', S.LastName) AS StudentName FROM StudentResult SR INNER JOIN Student S ON SR.StudentId = S.Id INNER JOIN AssessmentComponent AC ON SR.AssessmentComponentId = AC.Id"; // Set your SQL query
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
