using MySql.Data.MySqlClient;
using System;
using System.Windows;
using ToDoApp.Security;

namespace ToDoApp
{
    public partial class LoginWindow : Window
    {
        string cs = "server=localhost;port=3306;uid=root;pwd=root;database=test;";

        public LoginWindow()
        {
            InitializeComponent();
            CreateAdmin();   
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            string username = UserBox.Text;
            string password = PassBox.Password;

            if (username == "" || password == "")
            {
                ErrorText.Text = "Please fill in all fields.";
                return;
            }

            try
            {
                MySqlConnection con = new MySqlConnection(cs);
                con.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "SELECT passwordHash, passwordSalt FROM users WHERE username=@u", con);
                cmd.Parameters.AddWithValue("@u", username);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    ErrorText.Text = "User not found.";
                    con.Close();
                    return;
                }

                byte[] hash = (byte[])reader["passwordHash"];
                byte[] salt = (byte[])reader["passwordSalt"];

                if (!PasswordHasher.Verify(password, hash, salt))
                {
                    ErrorText.Text = "Wrong password.";
                    con.Close();
                    return;
                }

                con.Close();

                new MainWindow().Show();
                Close();
            }
            catch
            {
                ErrorText.Text = "Database error.";
            }
        }
         //admin / admin123
        private void CreateAdmin()
        {
            try
            {
                MySqlConnection con = new MySqlConnection(cs);
                con.Open();

                MySqlCommand check = new MySqlCommand(
                    "SELECT COUNT(*) FROM users WHERE username='admin'", con);

                int count = Convert.ToInt32(check.ExecuteScalar());

                if (count == 0)
                {
                    var result = PasswordHasher.HashPassword("admin");
                    byte[] hash = result.hash;
                    byte[] salt = result.salt;

                    MySqlCommand insert = new MySqlCommand(
                        "INSERT INTO users(username, passwordHash, passwordSalt) VALUES('admin', @h, @s)", con);

                    insert.Parameters.AddWithValue("@h", hash);
                    insert.Parameters.AddWithValue("@s", salt);

                    insert.ExecuteNonQuery();
                }

                con.Close();
            }
            catch
            {
            }
        }
    }
}
