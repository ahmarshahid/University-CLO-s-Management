using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidProject_DB
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Ahmar" && textBox2.Text == "Pakistan")
           {
               MessageBox.Show("Welcomeeeeeeeeee");
            ManageStudents uc = new ManageStudents();
            uc.Show();
           }
           else
           {
               MessageBox.Show("Enter valid credentials");
           }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "Ahmar" && textBox2.Text == "Pakistan")
            {
                MessageBox.Show("Welcomeeeeeeeeee");
                Form2 fm = new Form2();
                fm.Show();
            }
            else
            {
                MessageBox.Show("Enter valid credentials");
            }
        }
    }
}
