using MySql.Data.MySqlClient;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsFelhasznalok
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox_Name_Enter(object sender, EventArgs e)
        {
            if (textBox_Name.Text == "Név")
            {
                textBox_Name.Text = "";
                textBox_Name.ForeColor = Color.Black;
            }
        }

        private void textBox_Name_Leave(object sender, EventArgs e)
        {
            if (textBox_Name.Text == "")
            {
                textBox_Name.Text = "Név";
                textBox_Name.ForeColor = Color.Silver;
            }
        }
        MySqlConnection conn = null;
        MySqlCommand cmd = null;
        private void button_save_Click(object sender, EventArgs e)
        {
            if (textBox_Name.ForeColor == Color.Silver || string.IsNullOrEmpty(textBox_Name.Text))
            {
                return;
            }
            cmd.CommandText = "INSERT INTO `users`(`name`, `date`, `pfp`) VALUES (@name, @date, @pfp)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@name", textBox_Name.Text);
            cmd.Parameters.AddWithValue("@date", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@pfp", openFileDialog1.FileName);
            try
            {
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Sikeresen rögzítve!");
                    textBox_Name.Text = "";
                    dateTimePicker1.Value = DateTime.Today;
                    openFileDialog1.FileName = "";
                    pictureBox1.Image = null;

                }
                else
                {
                    MessageBox.Show("sikertelen rögzítés!");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            users_lista_update();
        }
        private void users_lista_update()
        {
            listBox1.Items.Clear();
            cmd.CommandText = "SELECT * FROM `users` WHERE 1";
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    users uj = new users(dr.GetInt32("id"), dr.GetString("name"), dr.GetDateTime("date"), dr.GetString("pfp"));
                    listBox1.Items.Add(uj);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "felhasznalok";
            builder.ConvertZeroDateTime = true;
            conn = new MySqlConnection(builder.ConnectionString);
            try
            {
                conn.Open();
                cmd = conn.CreateCommand();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "A program leáll!");
                Environment.Exit(0);
                throw;
            }
            users_lista_update();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                return;
            }
            textBox_Name.ForeColor = Color.Black;
            users kivalasztott_user = (users)listBox1.SelectedItem;
            textBox_Name.Text = kivalasztott_user.Name;
            dateTimePicker1.Value = kivalasztott_user.Date;
            if (File.Exists(kivalasztott_user.Pfp))
            {
                pictureBox1.Image = Image.FromFile(kivalasztott_user.Pfp);
            }
            else
            {
                pictureBox1.Image = null;
            }
        }

        private void button_talloz_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Képek|*.png;*.jpg;*.jpeg;*.webp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string kivPfp = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(kivPfp);
            }
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Nincs kijelölve felhasználó!");
                return;
            }
            cmd.Parameters.Clear();
            users kivalasztott_user = (users)listBox1.SelectedItem;
            cmd.CommandText = "UPDATE `users` SET `name`=@name,`date`=@date,`pfp`=@pfp WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", kivalasztott_user.Id);
            cmd.Parameters.AddWithValue("@name", textBox_Name.Text);
            cmd.Parameters.AddWithValue("@date", dateTimePicker1.Value);
            if (File.Exists(kivalasztott_user.Pfp))
            {
                cmd.Parameters.AddWithValue("@pfp", kivalasztott_user.Pfp);

            }
            else
            {
                cmd.Parameters.AddWithValue("@pfp", openFileDialog1.FileName);
            }

            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Módosítás sikeres votl!");
                textBox_Name.Text = "";
                dateTimePicker1.Value = DateTime.Today;
                openFileDialog1.FileName = "";
                pictureBox1.Image = null;

                users_lista_update();
            }
            else
            {
                MessageBox.Show("Az adatok módosítása sikertelen!");
            }
        }
        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                return;
            }
            cmd.CommandText = "DELETE FROM `users` WHERE `id` = @id";
            cmd.Parameters.Clear();
            users kivalasztott_user = (users)listBox1.SelectedItem;
            cmd.Parameters.AddWithValue("@id", kivalasztott_user.Id);
            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Törlés sikeres votl!");
                textBox_Name.Text = "";
                dateTimePicker1.Value = DateTime.Today;
                openFileDialog1.FileName = "";
                pictureBox1.Image = null;
                users_lista_update();
            }
            else
            {
                MessageBox.Show("Törlés sikertelen");
            }
        }
    }
}
