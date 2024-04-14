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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MidProject_DB
{
    public partial class Rubric : UserControl
    {
        public Rubric()
        {
            InitializeComponent();
            addDataIntoComboBox();
            display();

        }

        private void display()
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("(Select * from Rubric )", con);
            SqlCommand cmd = new SqlCommand("(Select * from Rubric where left(details,4)<>'rm*-' )", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void addDataIntoComboBox()
        {
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();
            SqlCommand cmd = new SqlCommand("Select Name from clo", connection);
            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                comboBox1.Items.Add(r.GetString(r.GetOrdinal("Name")));
            }
            r.Close();
            connection.Close();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            var con = ConfirgurationFile.getInstance().getConnection();
            con.Open();

            SqlCommand cmd2 = new SqlCommand("Select count(*) FROM Rubric WHERE details=@detels", con);
            cmd2.Parameters.AddWithValue("detels", textBox1.Text);
            int cnt = (int)cmd2.ExecuteScalar();

            if (cnt > 0)
            {
                con.Close();
                MessageBox.Show("Details are invalid");
                return;
            }

            if (textBox1.Text == "")
            {
                con.Close();
                MessageBox.Show("Please enter the valid details");
                return;
            }

            SqlCommand cmd = new SqlCommand("Insert into Rubric values ((SELECT COUNT(*)+1 from rubric),@Details, (SELECT Id FROM Clo WHERE name=@Cloid))", con);
            SqlCommand cmod = new SqlCommand("Select id from Clo where id = (select Cloid from  ");
            cmd.Parameters.AddWithValue("@Details", textBox1.Text);
            cmd.Parameters.AddWithValue("@Cloid",comboBox1.SelectedItem.ToString());
            MessageBox.Show("Sucessfully Added");
            cmd.ExecuteNonQuery();
            con.Close();

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            var con = ConfirgurationFile.getInstance().getConnection();
          //SqlCommand cmd = new SqlCommand("(Select * from Rubric )", con);
            SqlCommand cmd = new SqlCommand("(Select * from Rubric where left(details,4)<>'rm*-' )", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            var connection = ConfirgurationFile.getInstance().getConnection();
            connection.Open();

            int id = (int)dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value;
            SqlCommand cmd = new SqlCommand("Update Rubric Set details= @details where id = @id", connection);
            cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value);
            cmd.Parameters.AddWithValue("@details", "rm*-" + dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value);


            cmd.ExecuteNonQuery();
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
                // UPDATE DETAILS
                SqlCommand cmd = new SqlCommand("Update Rubric Set Details = @Details Where id = @id ", connection);
                cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                cmd.Parameters.AddWithValue("@Details", dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
