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
    public partial class SearchAnime : Form
    {
        public SearchAnime()
        {
            InitializeComponent();
        }

        private void roundedPictureBox1_Click(object sender, EventArgs e)
        {
           this.Close();
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
    }
}
