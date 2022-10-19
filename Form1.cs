using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Windows.Forms;


namespace task21_DB_Agregator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
	VALUES ((SELECT MAX(ID) + 1  FROM GenresDetail), 'Жахи', '')
END

";
                SqlCommand command = new SqlCommand(SqlExpression, connect);
                command.ExecuteNonQuery();
                SqlDataAdapter dataadapter = new SqlDataAdapter(sqlQuery, connect);
                DataSet ds = new DataSet();

                dataadapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables["Table"];

            }
            //sqldataadapter dataadapter = new sqldataadapter(sqlquery, connect);

            //dataadapter.fill(ds);
            //
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}