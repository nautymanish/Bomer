using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BomberMan
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form2(Int32.Parse(this.textBox1.Text), Int32.Parse(this.textBox2.Text) ,Int32.Parse(this.textBox3.Text)).ShowDialog();
        }


    }
}
