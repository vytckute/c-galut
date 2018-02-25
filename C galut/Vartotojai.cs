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
    public partial class Vartotojai : Form
    {
        public Vartotojai()
        {
            InitializeComponent();
        }

        private void vartotojaiBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.vartotojaiBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.database1DataSet);

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'database1DataSet.Vartotojai' table. You can move, or remove it, as needed.
            this.vartotojaiTableAdapter.Fill(this.database1DataSet.Vartotojai);

        }
    }
}
