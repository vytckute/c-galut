using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace C_galut
{
    public partial class Užsakymai : Form
    {
        public Užsakymai()
        {
            InitializeComponent();
        }

        private void irasaiBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.irasaiBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.database1DataSet);

        }

        private void Užsakymai_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Prekės' table. You can move, or remove it, as needed.
            this.prekėsTableAdapter.Fill(this.database1DataSet.Prekės);
            // TODO: This line of code loads data into the 'database1DataSet.Vartotojai' table. You can move, or remove it, as needed.
            this.vartotojaiTableAdapter.Fill(this.database1DataSet.Vartotojai);
            // TODO: This line of code loads data into the 'database1DataSet.Užsakymai' table. You can move, or remove it, as needed.
            this.užsakymaiTableAdapter.Fill(this.database1DataSet.Užsakymai);
            // TODO: This line of code loads data into the 'database1DataSet.Užsakymai' table. You can move, or remove it, as needed.
            this.užsakymaiTableAdapter.Fill(this.database1DataSet.Užsakymai);
            // TODO: This line of code loads data into the 'database1DataSet.Irasai' table. You can move, or remove it, as needed.
            this.irasaiTableAdapter.Fill(this.database1DataSet.Irasai);

            SqlConnection conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Vita\Desktop\kursai\c\C galut\Database1.mdf; Integrated Security = True");
            conn.Open();
            //uzkrauna vartotojus esamus
            SqlCommand sc = new SqlCommand("select Vartotojo_id from Vartotojai", conn);
            SqlDataReader reader;

            reader = sc.ExecuteReader();
            DataTable dt = new DataTable();
                        
            dt.Columns.Add("Vartotojo_id", typeof(string));
            dt.Load(reader);

            comboBox1.ValueMember = "Vartotojo_id";
             comboBox1.DataSource = dt;

            //uzkrauna prekes
            SqlCommand sc2 = new SqlCommand("select Prekės_id from Prekės", conn);

            reader = sc2.ExecuteReader();
            DataTable dt2 = new DataTable();

           
            dt2.Columns.Add("Prekės_id", typeof(string));
            dt2.Load(reader);

            comboBox2.ValueMember = "Prekės_id";
                      comboBox2.DataSource = dt2;

           conn.Close();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
       //           string ID = comboBox1.SelectedValue.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
       //     string ID2 = comboBox2.SelectedValue.ToString();
        }

       private void button1_Click(object sender, EventArgs e)
        {
           string Uzsakymid = textBox2.Text;
              string vartotojoid = comboBox1.SelectedValue.ToString();
        //    string vartotojoid = textBox4.Text;
            string mokejimotipas = textBox3.Text;
            string prekesid = comboBox2.SelectedValue.ToString();
           // string prekesid = textBox5.Text;
            string kiekis = textBox1.Text;
            SqlConnection conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Vita\Desktop\kursai\c\C galut\Database1.mdf; Integrated Security = True");
            conn.Open();
            SqlCommand check = new SqlCommand("SELECT COUNT(*) FROM Užsakymai WHERE (Užsakymo_Id = @user)", conn);
            check.Parameters.AddWithValue("@user", Uzsakymid);
            int UserExist = (int)check.ExecuteScalar();
            conn.Close();
            if (UserExist > 0)
            {
                //uzsakymoid exist
                //idedam irasa
                           string query1 = "Insert into Irasai (Uzsakymo_ID, Prekes_ID, Kaina, Perkamas_kiekis) Values (@Uzsakymo_ID,@Prekes_ID, (select Prekės_kaina from Prekės where Prekės.Prekės_Id = @Prekes_ID),@Perkamas_kiekis )";
                using (SqlCommand com = new SqlCommand(query1, conn))
                {
                  
                    com.Parameters.AddWithValue("@Uzsakymo_ID", Uzsakymid);
                    com.Parameters.AddWithValue("@Prekes_ID", prekesid);
                    com.Parameters.AddWithValue("@Perkamas_kiekis", kiekis);
                    conn.Open();
                    int rezult = com.ExecuteNonQuery();
                    if (rezult < 0)
                    {
                        Console.WriteLine("Error inserting data into Database!");
                    }
                    label6.Text = "Užsakymas papildytas preke";
                                        conn.Close();
                }
                //updatinam kaina uzsakyme
                string query2 = "update Užsakymai set Kaina=(SELECT sum(Kaina * Perkamas_kiekis) FROM Irasai where Uzsakymo_ID=@Uzsakymo_ID) where Užsakymo_id=@Uzsakymo_ID";
                using (SqlCommand com = new SqlCommand(query2, conn))
                {

                    
                    com.Parameters.AddWithValue("@Uzsakymo_ID", Uzsakymid);
                     conn.Open();
                    com.ExecuteNonQuery();
                    conn.Close();
                }
                //updaitinam kieky sandely
                string query3 = "update Prekės set Kiekis_sandelyje=(Kiekis_sandelyje-@Perkamas_kiekis) where Prekės_ID=@Prekes_ID";
                using (SqlCommand com = new SqlCommand(query3, conn))
                {

                    
                    com.Parameters.AddWithValue("@Prekes_ID", prekesid);
                    com.Parameters.AddWithValue("@Perkamas_kiekis", kiekis);
                    conn.Open();
                    com.ExecuteNonQuery();
                    conn.Close();
                }
            }
            else
            {
                //uzsakymoid dont exist. 
                //idedam uzsakyme
                string query4 = "Insert into Užsakymai (Vartotojo_id, Kaina, Siuntimo_adresas, Atsiskaitymo_tipas) Values (@Vartotojo_id, (@Perkamas_kiekis*(select Prekės_kaina from Prekės where Prekės.Prekės_Id = @Prekes_ID)), ( select Vartotojo_adresas from Vartotojai where Vartotojai.Vartotojo_id = @Vartotojo_id), @Atsiskaitymo_tipas) ";
                using (SqlCommand com = new SqlCommand(query4, conn))
                {


                    com.Parameters.AddWithValue("@Uzsakymo_ID", Uzsakymid);
                    com.Parameters.AddWithValue("@Vartotojo_id", vartotojoid);
                    com.Parameters.AddWithValue("@Atsiskaitymo_tipas", mokejimotipas);
                    com.Parameters.AddWithValue("@Perkamas_kiekis", kiekis);
                    com.Parameters.AddWithValue("@Prekes_ID", prekesid);
                    conn.Open();
                    
                    int rezult = com.ExecuteNonQuery();
                    if (rezult < 0)
                    {
                        Console.WriteLine("Error inserting data into Database!");
                    }
                    label6.Text = "Užsakymas naujas atliktas";
                    conn.Close();
                }
            
                //updaitinam kieky sandely
                string query6 = "update Prekės set Kiekis_sandelyje=(Kiekis_sandelyje-@Perkamas_kiekis) where Prekės_ID=@Prekes_ID";
                using (SqlCommand com = new SqlCommand(query6, conn))
                {


                    com.Parameters.AddWithValue("@Prekes_ID", prekesid);
                    com.Parameters.AddWithValue("@Perkamas_kiekis", kiekis);
                    conn.Open();
                    com.ExecuteNonQuery();
                    conn.Close();
                }

            }
            button1.Hide();
        }
    }
}
