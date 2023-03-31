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
    public partial class sign_up : Form
    {
        Database dataBase = new Database();
        public sign_up()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var name = textBox1.Text;
            var surname = textBox4.Text;
            var login = textBox3.Text;
            var password = textBox2.Text;

            string querystring = $"insert into register(login_user,password_user) values('{login}','{password}')";

            SqlCommand command = new SqlCommand(querystring,dataBase.getConnection());

            dataBase.openConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан!","Успешно!");
                Log_in frm_login = new Log_in();
                this.Hide();
                frm_login.ShowDialog();

            }
            else
            {
                MessageBox.Show("Аккаунт не зарегистрирован");
            }
            dataBase.closeConnection();

        }

        private Boolean checkuser()
        {
            var loginU = textBox3.Text;
            var passwordU = textBox2.Text;
            SqlDataAdapter adapter=new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select id_user,login_user,password_user from register where login_user='{loginU} and password_user='{passwordU}'";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            if(table.Rows.Count > 0)
            {
                MessageBox.Show("Данный пользователь уже существует!");
                return true;
            }
            else
            {
                return false;
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

    }
}
