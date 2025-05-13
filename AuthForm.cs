using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class AuthForm : Form
    {
        public static string Login;
        public AuthForm()
        {
            InitializeComponent();
            using (FileStream CheckStream = File.OpenRead("SavedUsers.txt"))
            {
                byte[] array = new byte[CheckStream.Length];
                CheckStream.Read(array, 0, array.Length);
                string SavedUsers = System.Text.Encoding.Default.GetString(array);
                if (SavedUsers != "")
                {
                    Login = SavedUsers;
                    MessageBox.Show("Успешная авторизация!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Planner plannerWindow = new Planner();
                    plannerWindow.ShowDialog();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            Login = LoginBox.Text;
            String Password = PasswordBox.Text;

            DataBase codingtodo_auth = new DataBase();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @uL AND `password` = @uP", codingtodo_auth.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value= Login;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = Password;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if(table.Rows.Count > 0)
            {
                MessageBox.Show("Успешная авторизация!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Planner plannerWindow = new Planner();
                if (keepEnter.Checked == true)
                {
                    using (FileStream stream = new FileStream("SavedUsers.txt", FileMode.OpenOrCreate))
                    {
                        byte[] array = System.Text.Encoding.Default.GetBytes(Login);
                        stream.Write(array, 0, array.Length);
                    }
                }
                plannerWindow.ShowDialog();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegForm regWindow = new RegForm();
            regWindow.Show();
        }
    }
}
