using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication1
{
    public partial class VentanaEscalafon : Form
    {
        public VentanaEscalafon(DataTable escalafon)
        {
            InitializeComponent();
            this.dataGridView1.DataSource = escalafon;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
