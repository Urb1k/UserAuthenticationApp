using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace UserAuthenticationApp
{
    public partial class AdminForm : Form
    {
        private Admin admin;

        private readonly DataHandler handler;

        public AdminForm(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
            handler = new DataHandler();
            RefreshUserList();

            userListBox.SelectedIndexChanged += userListBox_SelectedIndexChanged;
        }

        // Metoda pro obnovení seznamu uživatelů
        private void RefreshUserList()
        {
            userListBox.Items.Clear();
            foreach (User user in admin.Users)
            {
                userListBox.Items.Add(user.Username);
            }
        }


        private void changePasswordButton_Click_1(object sender, EventArgs e)
        {
            if (userListBox.SelectedIndex != -1)
            {
                string username = userListBox.SelectedItem.ToString();
                User user = admin.Users.Find(u => u.Username == username);
                if (user != null)
                {
                    string newPassword = newPasswordTextBox.Text;
                    user.PasswordHash = handler.GetPasswordHash(newPassword);
                    MessageBox.Show("Heslo uživatele " + username + " bylo změněno.");
                    RefreshUserList();
                    SaveUsersToXml(); // Uložení změněného hesla do souboru XML
                }
            }
            else
            {
                MessageBox.Show("Vyberte uživatele pro změnu hesla.");
            }
        }

        private void addUserButton_Click(object sender, EventArgs e)
        {
            string username = newUsernameTextBox.Text;
            string password = handler.GetPasswordHash(newPasswordTextBox.Text);

            if (!admin.Users.Exists(u => u.Username == username))
            {
                User newUser = new User
                {
                    Username = username,
                    PasswordHash = password
                };
                admin.Users.Add(newUser);
                RefreshUserList();
                SaveUsersToXml(); // Uložení uživatelů do souboru XML
            }
            else
            {
                MessageBox.Show("Uživatel již existuje.");
            }
        }

        private void SaveUsersToXml()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, admin.Users);
            }
        }

        private void userListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userListBox.SelectedIndex != -1)
            {
                string username = userListBox.SelectedItem.ToString();
                newUsernameTextBox.Text = username;
            }
        }

        private void LogOffButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
