﻿using System;
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
        DateTimePicker dtp = new DateTimePicker();
        Rectangle _Rectangle;

        Database database = new Database();
        int selectedRow;
        public FrmMain()
        {
            InitializeComponent();
            dataGridView3.Controls.Add(dtp);
            dtp.Visible = false;
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.TextChanged += new EventHandler(dtp_TextChange);
        }

        private void dtp_TextChange(Object sender, EventArgs e)
        {
            dataGridView3.CurrentCell.Value=dtp.Text.ToString();
        }
        private void CreateColumns()
        {
            dataGridView3.Columns.Add("id", "id");
            dataGridView3.Columns.Add("descriptions", "Описание");
            dataGridView3.Columns.Add("categoryId", "Тип");
            dataGridView3.Columns.Add("groups", "Группа");
            dataGridView3.Columns.Add("StartDate", "Срок");
            dataGridView3.Columns.Add("EndDate", "Дата назначения");
            dataGridView3.Columns.Add("IsNew", String.Empty);
        }

        private void ClearFields()
        {
            textBox1.Text = "";
            richTextBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetDateTime(4), record.GetDateTime(5), RowState.ModifiedNew);
        }
        private void RefreshDatagrid(DataGridView dgw)
        {
            dgw.Rows.Clear();
            string queryString= $"select * from to_do_list";
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
                dataGridView3.Rows[index].Cells[6].Value = RowState.Deleted;
                return;
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDatagrid(dataGridView3);
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
        
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (dataGridView3.Columns[e.ColumnIndex].Name)
            {
                case "StartDate":
                    _Rectangle = dataGridView3.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    dtp.Size = new Size(_Rectangle.Width, _Rectangle.Height);
                    dtp.Location = new Point(_Rectangle.X, _Rectangle.Y);
                    dtp.Visible = true;
                    break;
                case "EndDate":
                    _Rectangle = dataGridView3.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    dtp.Size = new Size(_Rectangle.Width, _Rectangle.Height);
                    dtp.Location = new Point(_Rectangle.X, _Rectangle.Y);
                    dtp.Visible = true;
                    break;
            }


            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView3.Rows[selectedRow];

                textBox1.Text = row.Cells[0].Value.ToString();
                richTextBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[3].Value.ToString();
                textBox4.Text = row.Cells[4].Value.ToString();
                textBox5.Text = row.Cells[5].Value.ToString();
            }
        }

        private void Update()
        {
            database.openConnection();
            for (int index = 0; index < dataGridView3.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView3.Rows[index].Cells[6].Value;
                if (rowState == RowState.Existed) 
                    continue;

                if (rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView3.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from to_do_list where id = {id}";

                    var command = new SqlCommand(deleteQuery, database.getConnection());
                    command.ExecuteNonQuery();
                }


                if (rowState == RowState.Modified)
                {
                    var id = dataGridView3.Rows[index].Cells[0].Value.ToString();
                    var descr = dataGridView3.Rows[index].Cells[1].Value.ToString();
                    var categ = dataGridView3.Rows[index].Cells[2].Value.ToString();
                    var group = dataGridView3.Rows[index].Cells[3].Value.ToString();
                    var start = dataGridView3.Rows[index].Cells[4].Value.ToString();
                    var end = dataGridView3.Rows[index].Cells[5].Value.ToString();

                    var changeQuery = $"update to_do_list set descriptions='{descr}', categoryId='{categ}',groups='{group}',StartDate='{start}',EndDate='{end}' where id= '{id}'";
                    var command = new SqlCommand(changeQuery, database.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            database.closeConnection();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            RefreshDatagrid(dataGridView3);
            ClearFields();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            deleteRow();
            ClearFields();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void Change()
        {
            var selectedRowIndex = dataGridView3.CurrentCell.RowIndex;

            var id = textBox1.Text;
            var descr = richTextBox1.Text;
            var categ = textBox3.Text;
            var group = textBox4.Text;
            var start = textBox5.Text;
            var end = textBox5.Text;
            dataGridView3.Rows[selectedRowIndex].SetValues(descr, categ, group, start, end);
            dataGridView3.Rows[selectedRowIndex].Cells[6].Value = RowState.Modified;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            Change();
            ClearFields();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dataGridView3_Scroll(object sender, ScrollEventArgs e)
        {
            dtp.Visible = false;
        }
    }
}
