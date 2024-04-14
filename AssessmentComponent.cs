using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidProject_DB
{
    public partial class AssessmentComponent : UserControl
    {
        public AssessmentComponent()
        {
            InitializeComponent();
            AddIntoComboBox1();
            AddIntoComboBox2();
            DisplayAssessmentcomponents();
        }
        
        private void DisplayAssessmentcomponents()
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("(Select * from Rubric )", con);
            SqlCommand cmd = new SqlCommand("SELECT * FROM ASSESSMENTCOMPONENT where left(Name,4) <> 'rm*-'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void AddIntoComboBox1()
        {
                var connection = ConfirgurationFile.getInstance().getConnection();
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select Details from Rubric where left(Details,4) <> 'rm*-'", connection);
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                comboBox1.Items.Add(r.GetString(r.GetOrdinal("Details")));
                }
                r.Close();
                connection.Close();
            
        }
        
        private void AddIntoComboBox2()
        {
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();
            SqlCommand cmd = new SqlCommand("Select Title from Assessment where left(Title,6) <> '(Del*)'", connection);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                comboBox2.Items.Add(r.GetString(r.GetOrdinal("Title")));
            }
            r.Close();
            connection.Close();
        }
        // Create Assessment Component
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            con.Open();

            SqlCommand cmd2 = new SqlCommand("Select count(*) FROM AssessmentComponent WHERE Name=@Name", con);
            cmd2.Parameters.AddWithValue("@Name", textBox1.Text);
            int cnt = (int)cmd2.ExecuteScalar();

            if (cnt > 0)
            {
                con.Close();
                MessageBox.Show("Name is invalid");
                return;
            }

            if (textBox1.Text == "")
            {
                con.Close();
                MessageBox.Show("Please enter the valid Name");
                return;
            }
            SqlCommand cmd = new SqlCommand("INSERT INTO AssessmentComponent VALUES (@Name, (SELECT ID FROM Rubric WHERE Details = @Details), @TotalMarks, @DateCreated, @DateUpdated, (SELECT ID FROM Assessment WHERE Title = @Title))", con);
           // SqlCommand cmd = new SqlCommand("Insert into AssessmentComponent values (Name = @Names,(Select ID FROM Rubric where Details = @Details), TotalMarks = @TotalMarks, DateCreated = @DateCreated, DateUpdated = @DateUpdated,(Select ID FROM Assesment where Title = @Title))", con);
            SqlCommand cmod = new SqlCommand("Select id from Clo where id = (select Cloid from  ");
            cmd.Parameters.AddWithValue("@Name", textBox1.Text);
            cmd.Parameters.AddWithValue("@Details", comboBox1.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@TotalMarks", textBox2.Text);
            cmd.Parameters.AddWithValue("@DateCreated", DateTime.Today);
            cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Today);
            cmd.Parameters.AddWithValue("@Title", comboBox2.SelectedItem.ToString());
            MessageBox.Show("Sucessfully Added Assessment Component !");
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            DisplayAssessmentcomponents();
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();

            int id = (int)dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value;
            SqlCommand cmd = new SqlCommand("Update AssessmentComponent Set Name= @Names where id = @id", connection);
            cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value);
            cmd.Parameters.AddWithValue("@Names", "rm*-" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Component Successfully Removed !");
            connection.Close();
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
                SqlCommand cmd = new SqlCommand("Update AssessmentComponent Set Name = @NewName, DateUpdated = @NewDate Where id = @id ", connection);
                cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                cmd.Parameters.AddWithValue("@NewName", dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                cmd.Parameters.AddWithValue("@NewDate",DateTime.Today);
                cmd.ExecuteNonQuery();
            }

            if (e.ColumnIndex == 3)
            {
                // UPDATE TOTAL MARKS
                SqlCommand cmd = new SqlCommand("Update AssessmentComponent Set TotalMarks = @NewMarks, DateUpdated = @NewDate Where id = @id ", connection);
                cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                cmd.Parameters.AddWithValue("@NewMarks", dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                cmd.Parameters.AddWithValue("@NewDate", DateTime.Today);
                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
