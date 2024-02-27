using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserAuthenticationApp
{
    public partial class UserForm : Form
    {
        private User user;

        private readonly DataHandler handler;

        public UserForm(User user)
        {
            InitializeComponent();
            handler = new DataHandler();
            this.user = user;
            label4.Text = user.Username;
        }

        private void changePasswordButton_Click(object sender, EventArgs e)
        {
            string currentPassword = currentPasswordTextBox.Text;
            string newPassword = newPasswordTextBox.Text;

            if (user.CheckPassword(currentPassword))
            {
                user.PasswordHash = handler.GetPasswordHash(newPassword);
                MessageBox.Show("Heslo bylo změněno.");
            }
            else
            {
                MessageBox.Show("Chybně zadané stávající heslo.");
            }
        }

        private void LogOffButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
