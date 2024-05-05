using MySql.Data;
using MySql.Data.MySqlClient;

namespace IdeaManagement
{
    public partial class Form1 : Form
    {
        string server = "localhost";
        string database = "ideas_2itb";
        string username = "root";
        string password = "";

        MySqlConnection connection;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = $"SERVER={server};DATABASE={database};UID={username};PASSWORD={password};";
            connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
            }catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }

            LoadData();

        }

        private void LoadData()
        {
            listBox1.Items.Clear();
            string query = "SELECT * FROM ideas";

            using(MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                var dataReader = cmd.ExecuteReader();
                while(dataReader.Read()) {
                    Idea idea = new Idea() {
                        Id = dataReader["id"].ToString(),
                        Title = dataReader["title"].ToString(),
                        Description = dataReader["description"].ToString(),
                        Creation_Date = dataReader["creation_date"].ToString()
                    };
                    listBox1.Items.Add(idea);
                }
                dataReader.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text) || textBox2.Text == "0")
            {
                AddIdea();
            }
            else
            {
                EditIdea(textBox2.Text);
            }
            textBox2.Text = "0";
            textBox1.Text = null;
            richTextBox1.Text = null;
        }

        private void EditIdea(string id)
        {
            string query = "UPDATE ideas SET title=@title, description=@description WHERE id=@id";
            var title = textBox1.Text;
            var desc = richTextBox1.Text;

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@description", desc);

                try
                {
                    var count = cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            LoadData();
        }

        private void AddIdea()
        {
            string query = "INSERT INTO ideas(title,description) values(@title,@description)";
            var title = textBox1.Text;
            var desc = richTextBox1.Text;

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@description", desc);

                try {
                    var count = cmd.ExecuteNonQuery();
                } 
                catch(MySqlException ex) { 
                    MessageBox.Show(ex.Message);
                }
            }
            LoadData();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idea = listBox1.SelectedItem as Idea;
            textBox2.Text = idea.Id.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RemoveData();
            textBox2.Text = "0";
        }

        private void RemoveData()
        {
            //DELETE FROM `ideas` WHERE 0
            string query = "DELETE FROM `ideas` WHERE id = @id";
            var ID = textBox2.Text;

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", ID);

                try
                {
                    var count = cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            LoadData();
        }
    }
}