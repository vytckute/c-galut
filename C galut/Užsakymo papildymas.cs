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
            SqlCommand sc2 = new SqlCommand("select Prekės_Id, Prekės_pavadinimas from Prekės ", conn);

            reader = sc2.ExecuteReader();
            DataTable dt2 = new DataTable();


            dt2.Columns.Add("Prekės_id", typeof(string));
            dt2.Columns.Add("Prekės_pavadinimas", typeof(string));
            dt2.Load(reader);

            comboBox2.ValueMember = "Prekės_id";
            comboBox2.DisplayMember = "Prekės_pavadinimas";
            comboBox2.DataSource = dt2;

            //uzkrauna uzsakymus esamus vartotojo
            SqlCommand sc3 = new SqlCommand("select Užsakymo_ID from Užsakymai where Vartotojo_ID=@Value", conn);
            string vartotojoid = comboBox1.SelectedValue.ToString();
            sc3.Parameters.AddWithValue("@Value", vartotojoid);

            reader = sc3.ExecuteReader();
            DataTable dt3 = new DataTable();

            dt3.Columns.Add("Užsakymo_ID", typeof(string));
            dt3.Load(reader);

            comboBox3.ValueMember = "Užsakymo_ID";
            comboBox3.DataSource = dt3;

            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //           pakeidus vartotojo id rodo jo uzsakymus tik
            SqlConnection conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Vita\Desktop\kursai\c\C galut\Database1.mdf; Integrated Security = True");
            SqlCommand sc3 = new SqlCommand("select Užsakymo_ID from Užsakymai where Vartotojo_ID=@Value", conn);
            string vartotojoid = comboBox1.SelectedValue.ToString();
            sc3.Parameters.AddWithValue("@Value", vartotojoid);
            SqlDataReader reader;
            conn.Open();
            reader = sc3.ExecuteReader();
            DataTable dt3 = new DataTable();

            dt3.Columns.Add("Užsakymo_ID", typeof(string));
            dt3.Load(reader);

            comboBox3.ValueMember = "Užsakymo_ID";
            comboBox3.DataSource = dt3;

            conn.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //     string ID2 = comboBox2.SelectedValue.ToString();
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

                if (ctrl is ComboBox)
                {
                    if (((ComboBox)ctrl) !=null)
                    {
                        existsdata = true;
                    }
                    else
                    {
                        MessageBox.Show("Nepasirinkote iš sąrašo");
                        break;
                    }
                }
            }

            if (existsdata == true)
            {
                string Uzsakymid = comboBox3.SelectedValue.ToString();
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
                if (skaiciuoja < Convert.ToInt32(kiekis))
                {
                    MessageBox.Show("Tokio kiekio sandėlyje neturime galimas kiekis:" + skaiciuoja);

                }
                else
                {
                    SqlConnection conn = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Vita\Desktop\kursai\c\C galut\Database1.mdf; Integrated Security = True");

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




                    button1.Hide();
                }
            }
        }
        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.prekėsTableAdapter.FillBy(this.database1DataSet.Prekės);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
    }
}
