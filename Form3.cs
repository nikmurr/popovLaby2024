using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PopovLaba3
{
    public partial class Form3 : Form
    {
        private List<Font> fontList;

        string originalCaptchaText;
        int resetAttempts = 0;
        int attempts = 0;

        Image butApproveOn = Image.FromFile(FileHelper.GetItem("but_approve.png"));
        Image butApproveOff = Image.FromFile(FileHelper.GetItem("but_approve_off.png"));
        Image butApproveHover = Image.FromFile(FileHelper.GetItem("but_approve_hover.png"));
        Image butRetryOn = Image.FromFile(FileHelper.GetItem("but_retry.png"));
        Image butRetryOff = Image.FromFile(FileHelper.GetItem("but_retry_off.png"));
        Image butRetryHover = Image.FromFile(FileHelper.GetItem("but_retry_hover.png"));

        public Form3(List<Font> fontList)
        {
            InitializeComponent();
            this.fontList = fontList;

            label.Font = fontList[2];
            textBox3.Font = fontList[2];
            SetCaptcha();

            textBox3.Focus();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private string GenerateCaptcha()
        {
            // Генерация текста капчи
            Random random = new Random();
            string captchaText = "";
            for (int i = 0; i < 5; i++)
            {
                captchaText += (char)random.Next(65, 91); // A-Z
            }

            return captchaText;
        }

        private Bitmap GenerateCaptchaImage(string captchaText)
        {
            // Размер изображения
            int width = 200;
            int height = 50;

            // Создание изображения
            Bitmap image = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                // Отрисовка текста капчи
                Random random = new Random();

                for (int i = 0; i < 300; i++) 
                {
                    int x = random.Next(0, width);
                    int y = random.Next(0, height);
                    Color randomColor = GetRandomPastelColor(random);
                    image.SetPixel(x, y, Color.FromArgb(128, randomColor.R, randomColor.G, randomColor.B));
                }

                FontFamily fontFamily = new FontFamily("Arial");
                for (int i = 0; i < captchaText.Length; i++)
                {
                    char letter = captchaText[i];

                    // Размер и положение буквы
                    float fontSize = random.Next(20, 26); 
                    float x = (width / captchaText.Length) * i + (width / captchaText.Length - fontSize) / 2;
                    float y = height / 2 - fontSize / 2;

                    // Угол наклона
                    float angle = random.Next(-25, 26);

                    using (Font font = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel))
                    using (Brush brush = new SolidBrush(Color.FromArgb(128, GetRandomPastelColor(random))))
                    using (Matrix matrix = new Matrix())
                    {
                        matrix.RotateAt(angle, new PointF(x + fontSize / 2, y + fontSize / 2));
                        graphics.Transform = matrix;
                        graphics.DrawString(letter.ToString(), font, brush, x, y);
                    }
                }

                // Добавление более тонких и длинных линий
                for (int i = 0; i < 3; i++)
                {
                    int x1 = random.Next(0, width);
                    int y1 = random.Next(0, height);
                    int x2 = random.Next(0, width);
                    int y2 = random.Next(0, height);

                    Color lineColor = GetRandomPastelColor(random);
                    using (Pen pen = new Pen(lineColor, 2)) // Уменьшил толщину линий
                    {
                        graphics.DrawLine(pen, x1, y1, x2, y2);
                    }
                }

                // Добавление более крупных окружностей
                for (int i = 0; i < 2; i++)
                {
                    int centerX = random.Next(0, width);
                    int centerY = random.Next(0, height);
                    int radius = random.Next(20, 30); // Увеличил радиус окружности

                    Color circleColor = GetRandomPastelColor(random);
                    using (Pen pen = new Pen(circleColor, 2)) // Уменьшил толщину окружности
                    {
                        graphics.DrawEllipse(pen, centerX - radius, centerY - radius, 2 * radius, 2 * radius);
                    }
                }
            }

            return image;
        }


        private void SetCaptcha()
        {
            originalCaptchaText = GenerateCaptcha(); // Генерация текста капчи
            Bitmap captchaImage = GenerateCaptchaImage(originalCaptchaText); // Генерация изображения капчи

            pictureBox1.Image = captchaImage;
        }

        private void ValidateCaptcha(System.Windows.Forms.TextBox textBox)
        {
            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                string enteredText = textBox3.Text;
                bool isCaptchaValid = string.Equals(originalCaptchaText, enteredText, StringComparison.OrdinalIgnoreCase);
                if (isCaptchaValid)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Введен неверный код. Повторите попытку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox.Clear();
                    ResetCaptcha();
                }
            }
            else
            {
                MessageBox.Show("Введите код с картинки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetCaptcha()
        {
            if (attempts >= 5)
            {
                MessageBox.Show("Превышено число неверных запросов. Попробуйте позже.");
                Application.Exit();
            }
            else
            {
                SetCaptcha();
                attempts++;
            }
        }

        private Color GetRandomPastelColor(Random random)
        {
            int r = random.Next(128, 256);
            int g = random.Next(128, 256);
            int b = random.Next(128, 256);
            return Color.FromArgb(r, g, b);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ValidateCaptcha(textBox3);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (attempts >= 5) 
            {
                button2.Enabled = false;
                button2.Visible= false;
            }
            else
            {
                attempts++;
                SetCaptcha();
            }
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackgroundImage = butApproveHover;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackgroundImage = butApproveOn;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackgroundImage = butRetryHover;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackgroundImage = butRetryOn;
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ValidateCaptcha(textBox3);
            }
        }
    }
}
