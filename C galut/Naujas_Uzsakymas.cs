using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace C_galut
{
    public partial class Naujas_Uzsakymas : Form
    {
        public Naujas_Uzsakymas()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Boolean existsdata = false;
            foreach (Control ctrl in Controls)
            {
                if (ctrl is TextBox)
                {
                    if (((TextBox)ctrl).Text != "")
                    {
                        existsdata = true;
                    }
                    else
                    {
                        MessageBox.Show("Užpildykite tuščius laukelius");
                        break;
                    }
                }
               
            }

            if (existsdata == true)
            {

                //    string Uzsakymid = textBox2.Text;
                string vartotojoid = comboBox1.SelectedValue.ToString();
                //    string vartotojoid = textBox4.Text;
                string mokejimotipas = textBox3.Text;
                string prekesid = comboBox2.SelectedValue.ToString();
                // string prekesid = textBox5.Text;
                string kiekis = textBox1.Text;

                DataContext db = new DataContext(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Vita\Desktop\kursai\c\C galut\Database1.mdf;Integrated Security=True");
                Table<Prekės> prekėslentele = db.GetTable<Prekės>();
                var skaiciuoja = ((from pr in prekėslentele
                                 where pr.Prekės_Id == Convert.ToInt32(prekesid)
                                 select pr.Kiekis_sandelyje).Single());
                if (skaiciuoja < Convert.ToInt32(kiekis)) //suskaiciuoja ir palygina
                {
                    MessageBox.Show("Tokio kiekio sandėlyje neturime galimas kiekis:" + skaiciuoja);

                }
                else
                {
                    SqlConnection conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Vita\Desktop\kursai\c\C galut\Database1.mdf; Integrated Security = True");

                    string query4 = "Insert into Užsakymai (Vartotojo_id, Kaina, Siuntimo_adresas, Atsiskaitymo_tipas) Values (@Vartotojo_id, (@Perkamas_kiekis*(select Prekės_kaina from Prekės where Prekės.Prekės_Id = @Prekes_ID)), ( select Vartotojo_adresas from Vartotojai where Vartotojai.Vartotojo_id = @Vartotojo_id), @Atsiskaitymo_tipas) ";
                    using (SqlCommand com = new SqlCommand(query4, conn))
                    {


                        //  com.Parameters.AddWithValue("@Uzsakymo_ID", Uzsakymid);
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
                    button1.Hide();
                button2.Show();
                }
                }           
               
            
        }
        private void Naujas_Uzsakymas_Load(object sender, EventArgs e)
        {
                       // TODO: This line of code loads data into the 'database1DataSet.Prekės' table. You can move, or remove it, as needed.
            this.prekėsTableAdapter.Fill(this.database1DataSet.Prekės);
            // TODO: This line of code loads data into the 'database1DataSet.Vartotojai' table. You can move, or remove it, as needed.
            this.vartotojaiTableAdapter.Fill(this.database1DataSet.Vartotojai);
       
        button2.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Užsakymai myForm = new Užsakymai();
            this.Hide();
            myForm.ShowDialog();
            this.Show();
        }

        private void Mokejimtipas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
               

        }
    }
}


