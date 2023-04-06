using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace test_Database
{
    public partial class Add : Form
    {
        Database database = new Database();
        public Add()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            database.openConnection();
            

            var desc = richTextBox1.Text;
            var cat = textBox2.Text;
            var group = textBox1.Text;
            var Start = dateTimePicker1.Text;
            var End = dateTimePicker2.Text; 

            var addQuery = $"insert into to_do_list (descriptions, categoryId, groups, StartDate, EndDate) values ('{desc}','{cat}','{group}','{Start}','{End}')";
            var command = new SqlCommand(addQuery, database.getConnection());
            command.ExecuteNonQuery();

            database.closeConnection();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dateTimePicker1.CustomFormat = " ";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
