using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TixNova__Final;

namespace TixNova_Final
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            
            this.ResizeRedraw = true;

           
            this.DoubleBuffered = true;

            

            
            roundedTextBox1.Text = "Enter username...";
            roundedTextBox1.ForeColor = Color.Gray;

            roundedTextBox2.Text = "Enter password...";
            roundedTextBox2.ForeColor = Color.Gray;

            
            roundedTextBox2.UsePasswordChar = false;
            roundedTextBox2.PasswordChar = '\0';
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            
            base.OnPaint(e);

            
            Color topColor = ColorTranslator.FromHtml("#0A2538");    
            Color bottomColor = ColorTranslator.FromHtml("#040F17"); 

            
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                topColor,
                bottomColor,
                45F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }

            
            ControlPaint.DrawGrid(e.Graphics, this.ClientRectangle, new Size(20, 20), Color.FromArgb(5, 255, 255, 255));
        }

        private void RoundedButton1_Click(object sender, System.EventArgs e)
        {
            string typedusername = roundedTextBox1.Text;
            string typedpassword = roundedTextBox2.Text;

            
            if (typedusername == "Enter username...") typedusername = "";
            if (typedpassword == "Enter password...") typedpassword = "";

            
            if (string.IsNullOrWhiteSpace(typedusername) && string.IsNullOrWhiteSpace(typedpassword))
            {
                MessageBox.Show("Please fill out the fields.", "Fields Empty", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                roundedTextBox1.Focus(); 
                return; 
            }

            
            if (string.IsNullOrWhiteSpace(typedpassword))
            {
                MessageBox.Show("Password is missing.", "Missing Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                
                roundedTextBox1.Text = "";
                roundedTextBox2.Text = "";
                roundedTextBox2.PasswordChar = '\0'; 
                roundedTextBox1.Focus();
                return;
            }

            
            if (string.IsNullOrWhiteSpace(typedusername))
            {
                MessageBox.Show("Username is missing.", "Missing Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                roundedTextBox1.Focus();
                return;
            }

            

            string validUsername = "JuanDelaCruz";
            string validPassword = "juanpogi123";

            
            if (typedusername == validUsername && typedpassword == validPassword)
            {
                
                UserSession.CurrentUsername = typedusername;
                this.DialogResult = DialogResult.OK;  
                this.Close();

            }
            else
            {
               
                MessageBox.Show("Incorrect username or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                
                roundedTextBox2.Text = "";
                roundedTextBox2.Focus();
            }
        }

        private void LoginForm_Load(object sender, System.EventArgs e)
        {
            
            roundedTextBox1.Text = "Enter username...";
            roundedTextBox1.ForeColor = Color.Gray;

            roundedTextBox2.Text = "Enter password...";
            roundedTextBox2.ForeColor = Color.Gray;

           
            roundedTextBox2.PasswordChar = '\0';

            
            this.ActiveControl = null;
        }

        private void RoundedTextBox1_Enter(object sender, System.EventArgs e)
        {
            if (roundedTextBox1.Text == "Enter username...")
            {
                roundedTextBox1.Text = "";
                
                roundedTextBox1.ForeColor = Color.White;
            }
        }

        private void RoundedTextBox1_Leave(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(roundedTextBox1.Text))
            {
                roundedTextBox1.Text = "Enter username...";
                
                roundedTextBox1.ForeColor = Color.Gray;
            }
        }

        private void RoundedTextBox2_Enter(object sender, System.EventArgs e)
        {
            if (roundedTextBox2.Text == "Enter password...")
            {
                roundedTextBox2.Text = "";
                roundedTextBox2.ForeColor = Color.White;

               
                roundedTextBox2.PasswordChar = '●';
            }
        }

        private void  RoundedTextBox2_Leave(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(roundedTextBox2.Text))
            {
                
                roundedTextBox2.PasswordChar = '\0';

                roundedTextBox2.Text = "Enter password...";
                roundedTextBox2.ForeColor = Color.Gray;
            }
        }

        private void PictureBox4_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }
    }
}