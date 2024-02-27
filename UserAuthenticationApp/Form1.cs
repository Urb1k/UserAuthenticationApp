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
    public partial class Form1 : Form
    {
        private Admin admin;
        private readonly DataHandler handler;
        public Form1()
        {
            InitializeComponent();
            handler = new DataHandler();
            // Vytvoření defaultního účtu admina
            admin = new Admin
            
            {
                Username = "admin",
                PasswordHash = handler.GetPasswordHash("admin")
            };

            LoadUsersFromXml();
        }


        private void LoadUsersFromXml()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.xml");
            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    admin.Users = (List<User>)serializer.Deserialize(fileStream);
                }
            }
        }



        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            // Kontrola, zda je uživatel v seznamu
            User user = admin.Users.Find(u => u.Username == username && u.CheckPassword(password));

            if (user != null)
            {
                MessageBox.Show("Přihlášení úspěšné jako uživatel");
                UserForm userForm = new UserForm(user);
                userForm.ShowDialog();
            }
            else if (admin.Username == username && admin.CheckPassword(password))
            {
                MessageBox.Show("Přihlášení úspěšné jako admin");
                AdminForm adminForm = new AdminForm(admin);
                adminForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Chybné uživatelské jméno nebo heslo");
            }
        }
    }
}
