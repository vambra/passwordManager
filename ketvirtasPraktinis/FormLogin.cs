using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Konscious.Security.Cryptography;

namespace ketvirtasPraktinis
{
    public partial class FormLogin : Form
    {
        public bool Login = false;
        private bool newUser = false;
        private string userLoginFile = "userLogin.txt";

        public FormLogin()
        {
            InitializeComponent();
            var info = new FileInfo(userLoginFile);
            if ((!info.Exists) || info.Length == 0)
            {
                newUser = true;
                buttonLogin.Text = "Sign up";
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (newUser == true)
            {
                using (StreamWriter writer = new StreamWriter(userLoginFile))
                {
                    CipherAES aes = new CipherAES();
                    writer.Write(aes.Encrypt(textBoxUsername.Text)+";");
                    string hashedPassword = CipherArgon.encrypt(textBoxPassword.Text);
                    writer.Write(aes.Encrypt(hashedPassword));
                    File.Create("passwords.txt");
                    Login = true;
                }
                this.Close();
            }
            else
            {
                string fileText;
                using (StreamReader reader = new StreamReader(userLoginFile))
                {
                    fileText = reader.ReadToEnd();
                }
                string[] loginDetails = fileText.Split(';');

                CipherAES aes = new CipherAES();
                string retrievedUsername = aes.Decrypt(loginDetails[0]);
                if (textBoxUsername.Text == retrievedUsername)
                {
                    string enteredPassword = CipherArgon.encrypt(textBoxPassword.Text);
                    string retrievedPassword = aes.Decrypt(loginDetails[1]);
                    if (enteredPassword == retrievedPassword)
                    {
                        Login = true;
                        this.Close();
                    }
                    else
                        MessageBox.Show("Password is incorrect");
                }
                else
                    MessageBox.Show("Username not found");
            }
        }
    }
}
