using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public partial class options : Form
    {
        public options()
        {
            InitializeComponent();

            speed.Value = Form1.speed;
            repeat.Checked = Form1.repeart;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            Form1.speed = Decimal.ToInt32(speed.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void options_Load(object sender, EventArgs e)
        {
            playRepeats.Value = Decimal.ToInt32(Form1.playRepeatValuedefault);
            Console.WriteLine(playRepeats.Value);
            repeat.Checked = Form1.repearttemp;
            Console.WriteLine(Form1.repearttemp);
        }

        private void repeat_CheckedChanged(object sender, EventArgs e)
        {
            Form1.repearttemp = repeat.Checked;
            Console.WriteLine(Form1.repearttemp);
        }

        private void playRepeat_ValueChanged(object sender, EventArgs e)
        {

            Form1.playRepeatValue = Decimal.ToInt32(playRepeats.Value);
            Console.WriteLine(Form1.playRepeatValue);
        }
    }
}
