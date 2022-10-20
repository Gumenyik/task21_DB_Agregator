using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace task21_DB_Agregator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        bool DataError(string name, string direction, string year)
        {
            var regex1 = new Regex(@"^[а-яА-Яі0-9\s]+$");
            bool state1 = regex1.IsMatch(name); 

            var regex2 = new Regex(@"^[а-яА-Яі\s]+$");
            bool state2 = regex2.IsMatch(direction);

            var regex3 = new Regex(@"^[0-9]+$");
            bool state3 = regex3.IsMatch(year);

            
            return (state1 && state2 && state3);
        }

 
        private void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = "Data Source=TARAS;Initial Catalog=Movie;Integrated Security=True";
            string sqlQuery = @"SELECT * FROM GenresDetail";

            //DataSet ds = new DataSet();
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string SqlExpression = @"IF NOT EXISTS(SELECT * FROM GenresDetail WHERE Genre = 'Жахи')
BEGIN
	INSERT INTO GenresDetail 
	VALUES ((SELECT MAX(ID) + 1  FROM GenresDetail), 'Жахи', 'До фільмів жахів належать стрічки, що покликані налякати глядача, вселити почуття хвилювання або розсмішити, створити атмосферу напруження чи нестерпного очікування чогось жахливого')
END

IF NOT EXISTS(SELECT * FROM GenresDetail WHERE Genre = 'Драма')
BEGIN
	INSERT INTO GenresDetail 
	VALUES ((SELECT MAX(ID) + 1  FROM GenresDetail), 'Драма', 'літературний жанр, пєса соціального, історичного чи побутового характеру з гострим конфліктом, який розвивається в постійній напрузі. Герої — переважно звичайні люди. Автор прагне розкрити їхню психологію, дослідити еволюцію характерів, мотивацію вчинків і дій')
END
";
                SqlCommand command = new SqlCommand(SqlExpression, connect);
                command.ExecuteNonQuery();
                SqlDataAdapter dataadapter = new SqlDataAdapter(sqlQuery, connect);
                DataSet ds = new DataSet();

                dataadapter.Fill(ds);
                comboBox1.DataSource = ds.Tables["Table"];
                comboBox1.DisplayMember = "Genre";
                comboBox1.ValueMember = "ID";
               // dataadapter.Fill(ds);
               // dataGridView1.DataSource = ds.Tables["Table"];

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            bool state = DataError(textBox2.Text, textBox3.Text, textBox5.Text);
            if (state || radioButton1.Checked || radioButton4.Checked)
            {
                string connectionString = "Data Source=TARAS;Initial Catalog=Movie;Integrated Security=True";

                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    connect.Open();
                    if (radioButton1.Checked)
                    {
                        string SqlQuery = "";
                        if (checkBox1.Checked)
                        {
                            SqlQuery = @"SELECT MoviesDetail.ID,  MoviesDetail.Title, MoviesDetail.Director, GenresDetail.Genre, MoviesDetail.Year, MoviesDetail.Country
FROM MoviesDetail
INNER JOIN GenresDetail ON MoviesDetail.Ganre_ID = GenresDetail.ID;";

                        }
                        else if (checkBox2.Checked)
                        {
                            SqlQuery = @"SELECT *
FROM MoviesDetail";
                        }
                        else
                        {
                            SqlQuery = @"SELECT MoviesDetail.ID,  MoviesDetail.Title, MoviesDetail.Director, GenresDetail.Genre, GenresDetail.Description, MoviesDetail.Year, MoviesDetail.Country
FROM MoviesDetail
INNER JOIN GenresDetail ON MoviesDetail.Ganre_ID = GenresDetail.ID;";
                        }
                        SqlDataAdapter dataadapter = new SqlDataAdapter(SqlQuery, connect);
                        DataSet ds = new DataSet();
                        dataadapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables["Table"];
                    }
                    else if (radioButton2.Checked)
                    { 

                        string SqlExpression = $"INSERT INTO MoviesDetail VALUES('{textBox1.Text}', '{textBox2.Text}', '{textBox3.Text}', '{comboBox1.SelectedValue}', '{textBox5.Text}', '{textBox6.Text}');";
                        SqlCommand command = new SqlCommand(SqlExpression, connect);
                        command.ExecuteNonQuery();
                    }
                    else if (radioButton3.Checked)
                    {
                        string SqlExpression = $"UPDATE MoviesDetail SET Title = '{textBox2.Text}', Director = '{textBox3.Text}', Ganre_ID = '{comboBox1.SelectedValue}', Year = '{textBox5.Text}', Country = '{textBox6.Text}' WHERE ID = {textBox1.Text};";
                        SqlCommand command = new SqlCommand(SqlExpression, connect);
                        command.ExecuteNonQuery();
                    }
                    else if (radioButton4.Checked)
                    {
                        string SqlExpression = $"DELETE FROM MoviesDetail WHERE ID = {textBox1.Text};";
                        SqlCommand command = new SqlCommand(SqlExpression, connect);
                        command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                MessageBox.Show("Невірні дані, заповніть заново");
            }
        }
    }
}