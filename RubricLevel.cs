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
    public partial class RubricLevel : UserControl
    {
        public RubricLevel()
        {
            InitializeComponent();
            AddintoCombobox();
            display();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

      private void AddintoCombobox()
        {
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();
            SqlCommand cmd = new SqlCommand("Select Details from Rubric where left(Details,4) <> 'rm*-'", connection);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                guna2ComboBox1.Items.Add(r.GetString(r.GetOrdinal("Details")));
            }
            r.Close();
            connection.Close();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            con.Open(); 

            SqlCommand cmd2 = new SqlCommand("Select count(*) FROM RubricLevel WHERE details=@detels", con);
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

            if (guna2TextBox1.Text == "" || guna2TextBox2.Text == "")
            {
                con.Close();
                MessageBox.Show("Please enter the valid details");
                return;
            }

           
            SqlCommand cmd = new SqlCommand("Insert into RubricLevel values ((SELECT id from Rubric where details = @RubricID),@Details,@measurementLevel)", con);
           
            cmd.Parameters.AddWithValue("@Details", guna2TextBox1.Text);
            cmd.Parameters.AddWithValue("@measurementLevel", guna2TextBox2.Text);
            cmd.Parameters.AddWithValue("@RubricID", guna2ComboBox1.SelectedItem.ToString());
            MessageBox.Show("Sucessfully Added");
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("(Select * from Rubric )", con);
            SqlCommand cmd = new SqlCommand("(Select * from RubricLevel where left(Details,6) <> '(Del*)')", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            con.Close();
        }
        private void display()
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("(Select * from Rubric )", con);
            SqlCommand cmd = new SqlCommand("(Select * from RubricLevel where left(Details,6) <> '(Del*)')", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            con.Close();
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) { return; }
            if (e.ColumnIndex == 6) { return; }
            if (e.RowIndex == -1) { return; }
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();
            if (e.ColumnIndex == 2)
            {
                // UPDATE DETAILS
                SqlCommand cmd = new SqlCommand("Update RubricLevel Set Details = @Details Where id = @id ", connection);
                cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                cmd.Parameters.AddWithValue("@Details", dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();

            int id = (int)dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value;
            SqlCommand cmd = new SqlCommand("Update RubricLevel Set details= @details where id = @id", connection);
            cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value);
            cmd.Parameters.AddWithValue("@details", "(Del*)" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[2].Value);


            cmd.ExecuteNonQuery();
            connection.Close();
        }


    }
}
