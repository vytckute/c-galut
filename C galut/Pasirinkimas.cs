using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C_galut
{
    public partial class Pasirinkimas : Form
    {
        public Pasirinkimas()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Naujas_Uzsakymas myForm = new Naujas_Uzsakymas();
            this.Hide();
            myForm.ShowDialog();
            this.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Užsakymai myForm = new Užsakymai();
            this.Hide();
            myForm.ShowDialog();
            this.Show();
        }
    }
}
