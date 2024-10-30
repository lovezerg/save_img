using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace 保存图片
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
       }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "User Id=<your_user>;Password=<your_password>;Data Source=<your_data_source>";
            string query = "SELECT clob_column FROM your_table WHERE id = :id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.Add(new MySqlParameter(":id", 1)); // 替换为实际的ID  

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (reader.Read())
                            {
                                byte[] imageBytes = (byte[])reader[7]; // 读取为字节数组 
                                richTextBox1.Text = Convert.ToBase64String(imageBytes);
                                string outputPath = "output_image.jpg"; // 替换为实际的输出路径  
                                SaveImageToFile(imageBytes, outputPath);
                                MessageBox.Show("保存成功！");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("保存失败!错误提示："+"\n"+ex.Message);
                        }
                    }
                }
            }

        }


        /*static byte[] ReadClobAsBytes(OracleLob lob)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                lob.CopyTo(ms, (int)lob.Length);
                return ms.ToArray();
            }
        }*/

        static void SaveImageToFile(byte[] imageBytes, string outputPath)
        {
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                Image image = Image.FromStream(ms);
                image.Save(outputPath, ImageFormat.Jpeg); // 根据实际情况选择ImageFormat  
            }
        }
    }
}
