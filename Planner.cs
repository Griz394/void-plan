using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Planner : Form
    {
        public Planner()
        {
            InitializeComponent();
            load();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            String Note = NoteBox.Text;
            String Date = DateBox.Text;
            String Time = TimeBox.Text;
            String File = FileBox.Text;

            DataBase data = new DataBase();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("INSERT INTO `planner` (`user`,`note`, `date`,`time`, `file`) VALUES (@user , @note,@date, @time, @file)", data.getConnection());
            command.Parameters.Add("@user", MySqlDbType.VarChar).Value = AuthForm.Login;
            command.Parameters.Add("@note", MySqlDbType.Text).Value=Note;
            command.Parameters.Add("@date", MySqlDbType.VarChar).Value = Date;
            command.Parameters.Add("@time", MySqlDbType.VarChar).Value = Time;
            command.Parameters.Add("@file", MySqlDbType.VarChar).Value = File;

            data.OpenConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Запись успешно добавлена!", "Planner", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Возникли ошибки при добавлении записи", "Planner", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            data.CloseConnection();
        }

        public void load()
        {
            DateTime date_now = DateTime.Now;

            DataBase codingtodo_auth = new DataBase();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `planner` WHERE `user` = @uL", codingtodo_auth.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value =AuthForm.Login;

            adapter.SelectCommand = command;
            adapter.Fill(table);
            for (short i = 0; i < table.Rows.Count; i++)
            {
                string date_event = table.Rows[i][2].ToString() + " " + table.Rows[i][3].ToString() + ":00";
                int result = DateTime.Compare(date_now, Convert.ToDateTime(date_event));
                if (result > 0)
                {
                    var answer = MessageBox.Show("У Вас есть невыполненная позиция:" + Convert.ToString(table.Rows[i][1]) + " Открыть файл с задачей?", "Новое напоминание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (answer == DialogResult.Yes)
                    {
                        string file_path = table.Rows[i][4].ToString();
                        System.Diagnostics.Process.Start(file_path);
                    }
                }
            }
        }
    }
}