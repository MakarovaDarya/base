using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test_Database.Properties;
using System.Data.SqlClient;

namespace test_Database
{
    enum RowState
    {
        Existed, New, Modified, ModifiedNew, Deleted
    }
    public partial class FrmMain : Form
    {

        Database database = new Database();
        int selectedRow;
        public FrmMain()
        {
            InitializeComponent();
        }
        private bool check;
        private void CreateColumns()
        {
            dataGridView3.Columns.Add("id", "id");
            dataGridView3.Columns.Add("descriptions", "Описание");
            dataGridView3.Columns.Add("categoryId", "Тип");
            dataGridView3.Columns.Add("StartDate", "Срок");
            dataGridView3.Columns.Add("EndDate", "Дата назначения");
            dataGridView3.Columns.Add("IsNew", String.Empty);
        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetDateTime(3), record.GetDateTime(4), RowState.ModifiedNew);
        }
        private void RefreshDatagrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString= $"select * from Personal_task";
            SqlCommand command = new SqlCommand(queryString, database.getConnection());
            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
        }
        private void deleteRow()
        {
            int index = dataGridView3.CurrentCell.RowIndex;
            dataGridView3.Rows[index].Visible = false;
            if (dataGridView3.Rows[index].Cells[0].Value.ToString() == String.Empty)
            {
                dataGridView3.Rows[index].Cells[4].Value = RowState.Deleted;
                return;
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDatagrid(dataGridView3);
        }
        
        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (check)
            {
                panel1.Width += 10;
                if (panel1.Size == panel1.MaximumSize)
                {
                    pictureBox1.Left += 110;
                    timer1.Stop();
                    check = false;
                    pictureBox1.Image = Resources.arrowleft;
                }
            }
            else
            {
                panel1.Width -= 10;
                if (panel1.Size == panel1.MinimumSize)
                {
                    pictureBox1.Left = 23;
                    timer1.Stop();
                    check = true;

                    pictureBox1.Image = Resources.dot;
                }
            }
        }
        private void ShowMenuPopup()
        {
            if (panel4.Visible == false)
            {
                panel4.Visible = true;
            }
            else
            {
                panel4.Visible = false;
            }
        }




        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ShowMenuPopup();
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            panel2.Width = 306;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Add addfrm = new Add();
            addfrm.Show();
        }


        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView3.Rows[selectedRow];

                textBox1.Text = row.Cells[0].Value.ToString();
                richTextBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[3].Value.ToString();
                textBox4.Text = row.Cells[4].Value.ToString();
            }
        }

        private void update()
        {
            database.openConnection();
            for (int index = 0; index < dataGridView3.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView3.Rows[index].Cells[4].Value;
                if (rowState == RowState.Existed) 
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView3.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from Personal_task where id = {id}";

                    var command = new SqlCommand(deleteQuery, database.getConnection());
                    command.ExecuteNonQuery();
                }

            }
            database.closeConnection();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            RefreshDatagrid(dataGridView3);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            deleteRow();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            update();
        }
    }
}
