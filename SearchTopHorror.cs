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
    public partial class SearchTopHorror : Form
    {
        public SearchTopHorror()
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
            // 1. Set the scroll amount for a "Page"
            // Change 840 if your pictures in this form are a different size!
            int scrollAmount = 840;

            // 2. Move the track to the left (reveals movies on the right)
            movieTrackPanel.Left -= scrollAmount;

            // 3. Stop perfectly at the end of the list
            int maxScrollLimit = viewportPanel.Width - movieTrackPanel.Width;
            if (movieTrackPanel.Left < maxScrollLimit)
            {
                movieTrackPanel.Left = maxScrollLimit;
            }
        }
    }
}