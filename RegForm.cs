using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class RegForm : Form
    {
        public RegForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void RegButton_Click(object sender, EventArgs e)
        {
            if(RegLoginBox.Text == "" || RegLoginBox.Text == " " || RegPassBox.Text==""|| RegPassBox.Text==" ")
            {
                MessageBox.Show("Введите имя пользователя и пароль!", "Ошибка регистрации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (checkUser())
            {
                return;
            }

            checkUser();
            DataBase db = new DataBase();
            MySqlCommand command = new MySqlCommand("INSERT INTO `users` (`id`, `username`, `password`) VALUES (NULL, @login, @password)", db.getConnection());

            command.Parameters.Add("@login", MySqlDbType.VarChar).Value= RegLoginBox.Text;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = RegPassBox.Text;

            db.OpenConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Успешная регистрация!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Возникли ошибки при регистрации", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            db.CloseConnection();
        }

        public Boolean checkUser()
        {
            String Login = RegLoginBox.Text;
            String Password = RegPassBox.Text;

            DataBase codingtodo_auth = new DataBase();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `username` = @uL", codingtodo_auth.getConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Login;

            adapter.SelectCommand = command;
            adapter.Fill(table);
            
            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Такое имя пользователя уже существует!", "Ошибка регистрации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}