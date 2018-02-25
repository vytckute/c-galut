using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C_galut
{
    public partial class Paieška : Form
    {
        public Paieška()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void vartotojaiBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.vartotojaiBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.database1DataSet);

        }

        private void Paieška_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Užsakymai' table. You can move, or remove it, as needed.
            this.užsakymaiTableAdapter.Fill(this.database1DataSet.Užsakymai);
            // TODO: This line of code loads data into the 'database1DataSet.Vartotojai' table. You can move, or remove it, as needed.
            this.vartotojaiTableAdapter.Fill(this.database1DataSet.Vartotojai);

        }

 

        private void button1_Click(object sender, EventArgs e)
        {
            string conection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Vita\Desktop\kursai\c\C galut\Database1.mdf; Integrated Security=True";


            SqlConnection sql = new SqlConnection(conection_string);
            try
            {
                sql.Open();
                richTextBox1.Text = "Prisijungiau prie db, duomenų tokiu nėra";
            }
            catch (Exception ex)
            {
                richTextBox1.Text = "Neprisijungiau";
            }
            sql.Close();
            string Value = textBox1.Text;
            string Value2 = textBox2.Text;
            string query1 = "select * from Užsakymai where Užsakymo_Id=@Value";
            string query2 = "select * from Vartotojai where Vartotojo_id=@Value";
            if (Value != String.Empty)
            {
                using (SqlCommand com = new SqlCommand(query1, sql))
                {
                    sql.Open();
                    com.Parameters.AddWithValue("@Value", Value);
                    SqlDataReader reader = null;
                    reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        richTextBox1.Text = (reader["Užsakymo_ID"].ToString() + " Vartotojo_id:" + reader["Vartotojo_ID"] + " Kaina" + reader["Kaina"] + " adresas: " + reader["Siuntimo_adresas"] + "\n");
                        textBox1.Clear();
                   
                    }
                    sql.Close();
                }
            }
            else if (Value2 != String.Empty)
            {
                using (SqlCommand com = new SqlCommand(query2, sql))
                {
                    sql.Open();
                    com.Parameters.AddWithValue("@Value", Value2);
                    SqlDataReader reader = null;
                    reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        richTextBox1.Text = (reader["Vartotojo_id"].ToString() + reader["Vartotojo_vardas"] +" "+ reader["Vartotojo_pavardė"] + " adresas: " + reader["Vartotojo_adresas"] + "\n");
                        textBox2.Clear();

                    }
                    sql.Close();
                }
            }
            else
            {
                richTextBox1.Text = "Įveskite duomenis";
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
