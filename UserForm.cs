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
    public partial class UserForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbcon = new DbConnect();
        SqlDataReader dr;
        string title = "Hike Shop Management System"; 
        public UserForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.Connection());
            this.Text = title; // Используем поле title для установки заголовка формы
            dgvUser.CellContentClick += DgvUser_CellContentClick; // Добавляем обработчик клика по ячейкам
            LoadUser();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadUser(); // Обновляем список пользователей при изменении текста поиска
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            UserModule module = new UserModule(this);
            module.ShowDialog();
        }
        private void DgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Обработка клика по кнопкам Edit и Delete
            string colName = dgvUser.Columns[e.ColumnIndex].Name;
            
            if (colName == "Delete")
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Удаление пользователя", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        // Получаем ID пользователя из второй колонки (Column2)
                        string userId = dgvUser.Rows[e.RowIndex].Cells["Column2"].Value.ToString();
                        
                        cm = new SqlCommand("DELETE FROM tbUser WHERE id = @id", cn);
                        cm.Parameters.AddWithValue("@id", userId);
                        
                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();
                        
                        MessageBox.Show("Пользователь успешно удален!", title);
                        LoadUser(); // Обновляем список
                    }
                    catch (Exception ex)
                    {
                        cn.Close();
                        MessageBox.Show(ex.Message, title);
                    }
                }
            }
            
            if (colName == "Edit")
            {
                // Получаем данные пользователя из строки
                string userId = dgvUser.Rows[e.RowIndex].Cells["Column2"].Value.ToString();
                string userName = dgvUser.Rows[e.RowIndex].Cells["Column5"].Value.ToString();
                string address = dgvUser.Rows[e.RowIndex].Cells["Column3"].Value.ToString();
                string phone = dgvUser.Rows[e.RowIndex].Cells["Column4"].Value.ToString();
                string role = dgvUser.Rows[e.RowIndex].Cells["Column6"].Value.ToString();
                string dob = dgvUser.Rows[e.RowIndex].Cells["Column7"].Value.ToString();
                string password = dgvUser.Rows[e.RowIndex].Cells["Column8"].Value.ToString();
                
                // Открываем форму редактирования (пока просто показываем сообщение)
                // TODO: Реализовать редактирование через UserModule
                MessageBox.Show($"Редактирование пользователя: {userName}\n(Функция редактирования будет реализована)", title);
            }
        }

        #region Method
        public void LoadUser()
        {
            try
            {
                int i = 0;
                dgvUser.Rows.Clear();
                
                // Используем параметризованный запрос для защиты от SQL-инъекций
                cm = new SqlCommand("SELECT * FROM tbUser WHERE CONCAT(name,address,phone,dob,role) LIKE @searchPattern", cn);
                cm.Parameters.AddWithValue("@searchPattern", "%" + txtSearch.Text + "%");

                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read()) 
                { 
                    i++;
                    dgvUser.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show($"Ошибка при загрузке пользователей: {ex.Message}", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Method
    }
}
