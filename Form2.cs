using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PopovLaba3
{
    public partial class Form2 : Form
    {
        private List<Font> fontList;
        private DatabaseHelper databaseHelper;

        Image butAuthOn = Image.FromFile(FileHelper.GetItem("but_auth.png"));
        Image butAuthOff = Image.FromFile(FileHelper.GetItem("but_auth_off.png"));
        Image butAuthHover = Image.FromFile(FileHelper.GetItem("but_auth_hover.png"));
        Image butreplaceOn = Image.FromFile(FileHelper.GetItem("but_replace.png"));
        Image butreplaceOff = Image.FromFile(FileHelper.GetItem("but_replace_off.png"));
        Image butreplaceHover = Image.FromFile(FileHelper.GetItem("but_replace_hover.png"));
        Image butRegisterOn = Image.FromFile(FileHelper.GetItem("but_register.png"));
        Image butRegisterOff = Image.FromFile(FileHelper.GetItem("but_register_off.png"));
        Image butRegisterHover = Image.FromFile(FileHelper.GetItem("but_register_hover.png"));
        Image butBackOn = Image.FromFile(FileHelper.GetItem("but_back.png"));
        Image butBackOff = Image.FromFile(FileHelper.GetItem("but_back_off.png"));
        Image butBackHover = Image.FromFile(FileHelper.GetItem("but_back_hover.png"));
        Image butHideOn = Image.FromFile(FileHelper.GetItem("hide_but_on.png"));
        Image butHideOff = Image.FromFile(FileHelper.GetItem("hide_but_off.png"));

        Image bg = Image.FromFile(FileHelper.GetItem("bg.png"));
        Image inputField = Image.FromFile(FileHelper.GetItem("input_field.png"));
        Image cat_1 = Image.FromFile(FileHelper.GetItem("cat_1.png"));

        private int attempts = 0;

        public Form2(List<Font> fontList, DatabaseHelper databaseHelper)
        {
            InitializeComponent();
            this.fontList = fontList;
            this.databaseHelper = databaseHelper;

            label.Font= fontList[0];
            label2.Font = fontList[1];
            label3.Font = fontList[1];
            label4.Font = fontList[1];
            label21.Font = fontList[1];
            label31.Font = fontList[1];
            textBox3.Font = fontList[2];
            textBox1.Font = fontList[2];
            textBox2.Font = fontList[2];

            ConstHandler.CheckLength(textBox3, label21, 4);
            ConstHandler.CheckLength(textBox1, label31, 8);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackgroundImage = butBackHover;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackgroundImage = butBackOn;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ConstHandler.CheckLength(textBox1, label21, 4);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ConstHandler.CheckLength(textBox2, label31, 8);
        }

        private void buttonHide1_Click(object sender, EventArgs e)
        {
            if (textBox2.PasswordChar == '•')
            {
                textBox2.PasswordChar = '\0';
                textBox3.PasswordChar = '\0';
                buttonHide1.BackgroundImage = butHideOn;
                buttonHide2.BackgroundImage = butHideOn;
            }
            else
            {
                textBox2.PasswordChar = '•';
                textBox3.PasswordChar = '•';
                buttonHide1.BackgroundImage = butHideOff;
                buttonHide2.BackgroundImage = butHideOff;
            }
        }

        private void buttonHide2_Click(object sender, EventArgs e)
        {
            if (textBox2.PasswordChar == '•')
            {
                textBox2.PasswordChar = '\0';
                textBox3.PasswordChar = '\0';
                buttonHide1.BackgroundImage = butHideOn;
                buttonHide2.BackgroundImage = butHideOn;
            }
            else
            {
                textBox2.PasswordChar = '•';
                textBox3.PasswordChar = '•';
                buttonHide1.BackgroundImage = butHideOff;
                buttonHide2.BackgroundImage = butHideOff;
            }
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackgroundImage = butRegisterHover;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackgroundImage = butRegisterOn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string loginText = textBox1.Text;
            string passwordText = textBox2.Text;
            string repeatedPasswordText = textBox3.Text;
            string errorMessage = "";
            if (ConstHandler.CheckLoginAndPassword(loginText, passwordText, out errorMessage))
            {
                if (ConstHandler.CheckPasswordSimilarity(passwordText, repeatedPasswordText, out errorMessage))
                {
                    int result = databaseHelper.InsertUser(loginText, passwordText);
                    if (result == 0)
                    {
                        MessageBox.Show($"Пользователь {loginText} добавлен.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else if (result == 1)
                    {
                        MessageBox.Show($"Пользователь с таким логином уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Неизвестная ошибка. Попробуйте позже.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } 
                }
                else
                {
                    textBox3.Clear();
                    MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                textBox3.Clear();
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
