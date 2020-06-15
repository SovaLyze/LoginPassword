using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginPassword
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private NpgsqlConnection conn;
        string connstring = String.Format("Server = {0}; Port = {1};" +
            "User Id = {2}; Password = {3}; Database = {4};",
            "localhost", "5432", "postgres",
            "yura12345", "Demo");
        private NpgsqlCommand cmd;
        private string sql = null;

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from u_login(:_username, :_password)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_username", txtUsername.Text);
                cmd.Parameters.AddWithValue("_password", txtPassword.Text);

                int result = (int)cmd.ExecuteScalar();
                conn.Close();

                if (result == 1)
                {
                    this.Hide();
                    new frmMain(txtUsername.Text).Show();
                }
                else
                {
                    MessageBox.Show("Please check your username or password", "Login fail", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Something went wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conn.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
        }
    }
}
