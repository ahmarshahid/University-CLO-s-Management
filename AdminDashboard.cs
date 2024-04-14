using MidProject_DB.ProjectBDataSet7TableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidProject_DB
{
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            CLO clo = new CLO();
            tableLayoutPanel2.Controls.Add(clo, 2, 1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
        
        }

        private void button6_Click(object sender, EventArgs e)
        {
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
       
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AssessmentComponent AssessCompo = new AssessmentComponent();
            tableLayoutPanel2.Controls.Add(AssessCompo, 2, 1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
         
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ManageStudents MS = new ManageStudents();
            tableLayoutPanel2.Controls.Add(MS, 2, 1);
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            var con = ConfirgurationFile.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from StudentResult",con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            StudentResults SR = new StudentResults();
            tableLayoutPanel2.Controls.Add(SR, 2, 1);

            var Form = this.FindForm();
            DataGridView dgv = (DataGridView)Form.Controls.Find("dataGridView1",true)[0];
            dgv.DataSource = dt;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Attendance attendence = new Attendance();
            tableLayoutPanel2.Controls.Add(attendence, 2, 1);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            CLO clo = new CLO();
            tableLayoutPanel2.Controls.Add(clo, 2, 1);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Assesment assessment = new Assesment();
            tableLayoutPanel2.Controls.Add(assessment, 2, 1);
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            AssessmentComponent AssessCompo = new AssessmentComponent();
            tableLayoutPanel2.Controls.Add(AssessCompo, 2, 1);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Rubric rubric = new Rubric();
            tableLayoutPanel2.Controls.Add(rubric, 2, 1);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            RubricLevel RL = new RubricLevel();
            tableLayoutPanel2.Controls.Add(RL, 2, 1);
        }
    }
}
