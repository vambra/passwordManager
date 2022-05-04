using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ketvirtasPraktinis
{
    public partial class FormHome : Form
    {
        private List<Password> passwords;
        private bool passwordShown = false;
        public FormHome()
        {
            InitializeComponent();
            passwords = PasswordManager.ReadFile();
            listBox1.DataSource = passwords;
            listBox1.DisplayMember = "Title";
            homeView();
        }

        private void homeView()
        {
            homeButtonsVisible(true);
            newButtonsVisible(false);
            editButtonsVisible(false);
            textBoxTitle.ReadOnly = true;
            textBoxPassword.ReadOnly = true;
            textBoxURL.ReadOnly = true;
            richTextBoxNotes.ReadOnly = true;
            listBox1.Enabled = true;
        }

        private void newView()
        {
            homeButtonsVisible(false);
            newButtonsVisible(true);
            editButtonsVisible(false);
            listBox1.Enabled = false;
            textBoxTitle.ReadOnly = false;
            textBoxPassword.ReadOnly = false;
            textBoxURL.ReadOnly = false;
            richTextBoxNotes.ReadOnly = false;
            textBoxTitle.Text = "";
            textBoxPassword.Text = "";
            textBoxURL.Text = "";
            richTextBoxNotes.Text = "";
        }

        private void editView()
        {
            homeButtonsVisible(false);
            newButtonsVisible(false);
            editButtonsVisible(true);
            listBox1.Enabled = false;
            textBoxTitle.ReadOnly = false;
            textBoxPassword.ReadOnly = false;
            textBoxURL.ReadOnly = false;
            richTextBoxNotes.ReadOnly = false;
            if (!passwordShown)
            {
                passwordShown = true;
                CipherAES aes = new CipherAES();
                textBoxPassword.Text = aes.Decrypt(textBoxPassword.Text);
            }
        }

        private void homeButtonsVisible(bool visibility)
        {
            buttonAddNew.Visible = visibility;
            buttonEdit.Visible = visibility;
            buttonDelete.Visible = visibility;
            buttonCopy.Visible = visibility;
            buttonShow.Visible = visibility;
        }
        private void newButtonsVisible(bool visibility)
        {
            buttonAdd.Visible = visibility;
            buttonCancelNew.Visible = visibility;
            buttonGenerate.Visible = visibility;
        }

        private void editButtonsVisible(bool visibility)
        {
            buttonConfirmEdit.Visible = visibility;
            buttonCancelEdit.Visible = visibility;
            buttonGenerate.Visible = visibility;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            passwordShown = false;
            Password pass = (Password)listBox1.SelectedItem;
            textBoxTitle.Text = pass.title;
            textBoxPassword.Text = pass.password;
            textBoxURL.Text = pass.url;
            richTextBoxNotes.Text = pass.notes;
            passwordShown = false;
        }

        private void buttonAddNew_Click(object sender, EventArgs e)
        {
            newView();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            editView();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure?", "Delete password", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                passwords.Remove((Password)listBox1.SelectedItem);
                PasswordManager.WriteToFile(passwords);
                passwords = PasswordManager.ReadFile();
                listBox1.DataSource = passwords;
            }
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            if (!passwordShown)
            {
                passwordShown = true;
                CipherAES aes = new CipherAES();
                textBoxPassword.Text = aes.Decrypt(textBoxPassword.Text);
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            string psw;
            if (passwordShown == true)
            {
                psw = textBoxPassword.Text;
            }
            else
            {
                CipherAES aes = new CipherAES();
                psw = aes.Decrypt(textBoxPassword.Text);
            }
            Clipboard.SetText(psw);
        }

        private void buttonCancelNew_Click(object sender, EventArgs e)
        {
            homeView();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBoxTitle.Text) || String.IsNullOrWhiteSpace(textBoxPassword.Text))
                MessageBox.Show("Please specify the title and password");
            else
            {
                CipherAES aes = new CipherAES();
                Password newPsw = new Password();
                newPsw.id = passwords.Count();
                newPsw.title = textBoxTitle.Text;
                newPsw.password = aes.Encrypt(textBoxPassword.Text);
                newPsw.url = textBoxURL.Text;
                newPsw.notes = richTextBoxNotes.Text;
                passwords.Add(newPsw);
                PasswordManager.WriteToFile(passwords);
                passwords = PasswordManager.ReadFile();
                listBox1.DataSource = passwords;
                MessageBox.Show("Password added");
                homeView();
            }
        }

        private void buttonConfirmEdit_Click(object sender, EventArgs e)
        {
            Password psw = (Password)listBox1.SelectedItem;
            int id = psw.id;
            foreach (Password password in passwords)
            {
                if (password.id == id)
                {
                    CipherAES aes = new CipherAES();
                    password.title = textBoxTitle.Text;
                    password.password = aes.Encrypt(textBoxPassword.Text);
                    password.url = textBoxURL.Text;
                    password.notes = richTextBoxNotes.Text;
                    break;
                }
            }
            PasswordManager.WriteToFile(passwords);
            passwords = PasswordManager.ReadFile();
            listBox1.DataSource = passwords;
            MessageBox.Show("Password saved");
            homeView();
        }

        private void buttonCancelEdit_Click(object sender, EventArgs e)
        {
            homeView();
            //select index
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            textBoxPassword.Text = PasswordManager.GeneratePassword();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.Enabled)
            {
                List<Password> shownPasswords = new List<Password>();
                foreach (Password psw in passwords)
                {
                    if (psw.title.Contains(textBoxSearch.Text))
                    {
                        shownPasswords.Add(psw);
                    }

                }
                listBox1.DataSource = shownPasswords;
            }
            else
                textBoxSearch.Text = "";
        }
    }
}
