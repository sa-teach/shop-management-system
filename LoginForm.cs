using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shop_Management_System
{
    public partial class LoginForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;

        public LoginForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.Connection());
            
            // Устанавливаем обработчик для Enter на полях ввода
            guna2TextBox1.KeyDown += TextBox_KeyDown;
            guna2TextBox2.KeyDown += TextBox_KeyDown;
            
            // Делаем поле пароля скрытым
            guna2TextBox2.PasswordChar = '*';
            
            // Создаем кнопку входа программно
            CreateLoginButton();
        }

        private void CreateLoginButton()
        {
            Guna.UI2.WinForms.Guna2Button btnLogin = new Guna.UI2.WinForms.Guna2Button();
            btnLogin.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            btnLogin.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            btnLogin.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            btnLogin.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            btnLogin.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(90)))), ((int)(((byte)(200)))));
            btnLogin.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            btnLogin.ForeColor = System.Drawing.Color.White;
            btnLogin.Location = new System.Drawing.Point(335, 270);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new System.Drawing.Size(451, 45);
            btnLogin.TabIndex = 17;
            btnLogin.Text = "Войти";
            btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            this.Controls.Add(btnLogin);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/sa-teach");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, что логин заполнен
                if (guna2TextBox1.Text == "")
                {
                    MessageBox.Show("Пожалуйста, введите логин", "Предупреждение", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверяем пользователя в базе данных
                // Для Admin требуется пароль, для Employee пароль не требуется
                string query = "SELECT * FROM tbUser WHERE name = @name";
                cm = new SqlCommand(query, cn);
                cm.Parameters.AddWithValue("@name", guna2TextBox1.Text);

                cn.Open();
                dr = cm.ExecuteReader();

                if (dr.Read())
                {
                    string userName = dr["name"].ToString();
                    string role = dr["role"]?.ToString() ?? "";
                    string password = dr["password"]?.ToString() ?? "";
                    
                    dr.Close();
                    
                    // Для Admin проверяем пароль, для Employee пароль не требуется
                    if (role == "Admin")
                    {
                        if (guna2TextBox2.Text == "" || guna2TextBox2.Text != password)
                        {
                            cn.Close();
                            MessageBox.Show("Неверный пароль для администратора!", "Ошибка входа", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    // Для Employee пароль не проверяем
                    
                    cn.Close();
                    
                    // Пользователь найден и авторизован - открываем главную форму
                    MessageBox.Show("Добро пожаловать, " + userName + "!", "Успешный вход", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    MainForm mainForm = new MainForm();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    // Пользователь не найден
                    dr.Close();
                    cn.Close();
                    MessageBox.Show("Пользователь с таким именем не найден!", "Ошибка входа", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                if (cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
                MessageBox.Show("Ошибка при подключении к базе данных: " + ex.Message, "Ошибка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
