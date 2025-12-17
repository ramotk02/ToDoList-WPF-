using MySql.Data.MySqlClient;
using System;
using System.Windows;
using ToDoApp.Security;

namespace ToDoApp
{
    public partial class LoginWindow : Window
    {
        private readonly string cs = "server=localhost;port=3306;uid=root;pwd=root;database=test;";

        public LoginWindow()
        {
            InitializeComponent();
            CreateAdminOnce(); 
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = "";

            var username = UserBox.Text?.Trim();
            var password = PassBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorText.Text = "Username and password required.";
                return;
            }

            try
            {
                using var con = new MySqlConnection(cs);
                con.Open();

                using var cmd = new MySqlCommand(
                    "SELECT passwordHash, passwordSalt FROM users WHERE username=@u LIMIT 1", con);
                cmd.Parameters.AddWithValue("@u", username);

                using var r = cmd.ExecuteReader();
                if (!r.Read())
                {
                    ErrorText.Text = "User not found.";
                    return;
                }

                var hash = (byte[])r["passwordHash"];
                var salt = (byte[])r["passwordSalt"];

                if (!PasswordHasher.Verify(password, hash, salt))
                {
                    ErrorText.Text = "Wrong password.";
                    return;
                }

                new MainWindow().Show();
                Close();
            }
            catch (Exception ex)
            {
                ErrorText.Text = "Error: " + ex.Message;
            }
        }


        // admin/admin123
        private void CreateAdminOnce()
        {
            try
            {
                using var con = new MySqlConnection(cs);
                con.Open();

                using (var check = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username='admin'", con))
                {
                    var exists = Convert.ToInt32(check.ExecuteScalar());
                    if (exists > 0) return;
                }

                var (hash, salt) = PasswordHasher.HashPassword("admin123");

                using var cmd = new MySqlCommand(
                    "INSERT INTO users(username, passwordHash, passwordSalt) VALUES(@u,@h,@s)", con);

                cmd.Parameters.AddWithValue("@u", "admin");
                cmd.Parameters.Add("@h", MySqlDbType.VarBinary, hash.Length).Value = hash;
                cmd.Parameters.Add("@s", MySqlDbType.VarBinary, salt.Length).Value = salt;

                cmd.ExecuteNonQuery();
            }
            catch
            {
            }
        }
    }
}
