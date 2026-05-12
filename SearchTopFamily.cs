using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TixNova__Final
{
    public partial class SearchTopFamily : Form
    {
        public SearchTopFamily()
        {
            InitializeComponent();
        }

        private void fadingImageButton1_Click(object sender, EventArgs e)
        {
            int scrollAmount = 840;
            movieTrackPanel.Left += scrollAmount;

            if (movieTrackPanel.Left > 0)
            {
                movieTrackPanel.Left = 0;
            }
        }

        private void fadingImageButton2_Click(object sender, EventArgs e)
        {
            int scrollAmount = 840;

            movieTrackPanel.Left -= scrollAmount;

            int maxScrollLimit = viewportPanel.Width - movieTrackPanel.Width;
            if (movieTrackPanel.Left < maxScrollLimit)
            {
                movieTrackPanel.Left = maxScrollLimit;
            }
        }

        private void roundedPictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void blueGradientLabel1_Click(object sender, EventArgs e)
        {

        }

        private void gradientLabel2_Click(object sender, EventArgs e)
        {

        }

        private void roundedPictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void roundedPictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void roundedPictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void roundedPictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void roundedPictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void gradientLabel1_Click(object sender, EventArgs e)
        {

        }

        private void roundedPictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void roundedPictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void roundedPictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void roundedPictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void roundedPictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void movieTrackPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void viewportPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
    }
}
