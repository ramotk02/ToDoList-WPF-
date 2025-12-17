using MySql.Data.MySqlClient;
using System;
using System.Globalization;
using System.Windows;

namespace ToDoApp
{
    public partial class MainWindow : Window
    {
        // Connection to the server
        private readonly string cs = "server=localhost;port=3306;uid=root;pwd=root;database=test;";

        private int selectedId = -1;

        public MainWindow()
        {
            InitializeComponent();
            LoadTasks();
        }

        // Load data
        private void LoadTasks()
        {
            TasksList.Items.Clear();

            using var con = new MySqlConnection(cs);
            con.Open();

            using var cmd = new MySqlCommand(
                "SELECT id, title, expense, done FROM task ORDER BY id DESC", con);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                TasksList.Items.Add(new
                {
                    Id = r.GetInt32("id"),
                    Title = r.GetString("title"),
                    Expense = r.IsDBNull(r.GetOrdinal("expense"))
                        ? ""
                        : r.GetDecimal("expense").ToString("0.00", CultureInfo.InvariantCulture),
                    Done = r.GetBoolean("done")
                });
            }
        }

        // Add
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text?.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Title required");
                return;
            }

            decimal? expense = ParseExpenseOrNull(ExpenseBox.Text);

            using var con = new MySqlConnection(cs);
            con.Open();

            using var cmd = new MySqlCommand(
                "INSERT INTO task (title, expense, done, categoryId) VALUES (@t,@e,@d,1)", con);

            cmd.Parameters.AddWithValue("@t", title);
            cmd.Parameters.AddWithValue("@e", (object?)expense ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@d", DoneBox.IsChecked == true);

            cmd.ExecuteNonQuery();

            ClearForm();
            LoadTasks();
        }

        // Update
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId == -1) return;

            string title = TitleBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Title required");
                return;
            }

            decimal? expense = ParseExpenseOrNull(ExpenseBox.Text);

            using var con = new MySqlConnection(cs);
            con.Open();

            using var cmd = new MySqlCommand(
                "UPDATE task SET title=@t, expense=@e, done=@d WHERE id=@id", con);

            cmd.Parameters.AddWithValue("@id", selectedId);
            cmd.Parameters.AddWithValue("@t", title);
            cmd.Parameters.AddWithValue("@e", (object?)expense ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@d", DoneBox.IsChecked == true);

            cmd.ExecuteNonQuery();

            LoadTasks();
        }

        // Delete
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedId == -1) return;

            using var con = new MySqlConnection(cs);
            con.Open();

            using var cmd = new MySqlCommand(
                "DELETE FROM task WHERE id=@id", con);

            cmd.Parameters.AddWithValue("@id", selectedId);
            cmd.ExecuteNonQuery();

            ClearForm();
            LoadTasks();
        }

        private void TasksList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            dynamic item = TasksList.SelectedItem;
            if (item == null) return;

            selectedId = item.Id;
            TitleBox.Text = item.Title;
            ExpenseBox.Text = item.Expense; 
            DoneBox.IsChecked = item.Done;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e) => LoadTasks();

        private void ClearForm()
        {
            selectedId = -1;
            TitleBox.Text = "";
            ExpenseBox.Text = "";
            DoneBox.IsChecked = false;
        }

        private static decimal? ParseExpenseOrNull(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            input = input.Trim();

            //  "12.50" ou "12,50"
            input = input.Replace(',', '.');

            if (!decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
            {
                MessageBox.Show("Expense invalide. Exemple: 12.50");
                return null;
            }

            return value;
        }
    }
}
