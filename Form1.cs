using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection.Emit;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ZedGraph;

namespace PopovLaba3 
{
    public partial class Form1 : Form
    {
        //Визуальная часть
        public PrivateFontCollection fontCollection = new PrivateFontCollection();

        Image butAuthOn = Image.FromFile(FileHelper.GetItem("but_auth.png"));
        Image butAuthOff = Image.FromFile(FileHelper.GetItem("but_auth_off.png"));
        Image butAuthHover = Image.FromFile(FileHelper.GetItem("but_auth_hover.png"));
        Image butreplaceOn = Image.FromFile(FileHelper.GetItem("but_replace.png"));
        Image butreplaceOff = Image.FromFile(FileHelper.GetItem("but_replace_off.png"));
        Image butreplaceHover = Image.FromFile(FileHelper.GetItem("but_replace_hover.png"));
        Image butRegisterOn = Image.FromFile(FileHelper.GetItem("but_register.png"));
        Image butRegisterOff = Image.FromFile(FileHelper.GetItem("but_register_off.png"));
        Image butRegisterHover = Image.FromFile(FileHelper.GetItem("but_register_hover.png"));
        Image butHideOn = Image.FromFile(FileHelper.GetItem("hide_but_on.png"));
        Image butHideOff = Image.FromFile(FileHelper.GetItem("hide_but_off.png"));
        Image minibutHomeOn = Image.FromFile(FileHelper.GetItem("minibut_home_on.png"));
        Image minibutHomeOnHover = Image.FromFile(FileHelper.GetItem("minibut_home_on_hover.png"));
        Image minibutHomeOff = Image.FromFile(FileHelper.GetItem("minibut_home_off.png"));
        Image minibutHomeOffHover = Image.FromFile(FileHelper.GetItem("minibut_home_off_hover.png"));
        Image minibutPrintOn = Image.FromFile(FileHelper.GetItem("minibut_print_on.png"));
        Image minibutPrintOff = Image.FromFile(FileHelper.GetItem("minibut_print_off.png"));
        Image minibutStatsOn = Image.FromFile(FileHelper.GetItem("minibut_stats_on.png"));
        Image minibutStatsOff = Image.FromFile(FileHelper.GetItem("minibut_stats_off.png"));
        Image minibutRecordsOn = Image.FromFile(FileHelper.GetItem("minibut_records_on.png"));
        Image minibutRecordsOff = Image.FromFile(FileHelper.GetItem("minibut_records_off.png"));

        Image bg = Image.FromFile(FileHelper.GetItem("bg.png"));
        Image inputField = Image.FromFile(FileHelper.GetItem("input_field.png"));
        Image cat_1 = Image.FromFile(FileHelper.GetItem("cat_1.png"));

        List<Font> fontList = new List<Font>();

        //Функциональная часть
        List<Func<double, double>> functions = new List<Func<double, double>>();

        private Timer blockTimer;
        private DatabaseHelper databaseHelper;

        private int attempts = 0;
        string login = "";
        string password = "";

        public Form1()
        {
            InitializeComponent();

            floatNavigation.Visible = false;

            fontCollection.AddFontFile(FileHelper.GetItem("Inter-Regular.ttf"));
            fontCollection.AddFontFile(FileHelper.GetItem("Inter-SemiBold.ttf"));

            Font fontHeaders = new Font(fontCollection.Families[1], 24);
            Font fontLabels = new Font(fontCollection.Families[0], 14);
            Font fontTextBox = new Font(fontCollection.Families[0], 12);

            fontList.Add(fontHeaders);
            fontList.Insert(1, fontLabels);
            fontList.Insert(2, fontTextBox);

            label.Font = fontHeaders;
            textBox2.Font = fontTextBox;
            textBox3.Font = fontTextBox;
            label2.Font = fontLabels;
            label3.Font = fontLabels;
            label21.Font = fontLabels;
            label31.Font = fontLabels;
            label5.Font = fontHeaders;
            richTextBox1.Font = fontTextBox;
            button1.Enabled = true;
            button1.BackgroundImage = butAuthOn;

            ConstHandler.CheckLength(textBox2, label21, 4);
            ConstHandler.CheckLength(textBox3, label31, 8);

            blockTimer = new Timer();
            blockTimer.Interval = 20000;
            blockTimer.Tick += UnblockPanel;

            databaseHelper = new DatabaseHelper();

            functions.Add(Math.Sin);
            functions.Add(x => x * x);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    textBox3.Focus();
                }
                else button1.PerformClick();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    textBox2.Focus();
                }
                else button1.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string loginText = textBox2.Text;
            string passwordText = textBox3.Text;
            string errorMessage = "";
            if (ConstHandler.CheckLoginAndPassword(loginText, passwordText, out errorMessage))
            {
                int result = databaseHelper.CheckUser(loginText, passwordText);
                if (result == 0)
                {
                    floatNavigation.Visible = true;
                    program_panel.Visible = true;
                    string username = databaseHelper.internalLogin;
                    label5.Text = $"Добро пожаловать, {username}!";
                    attempts = 0;
                }
                else if (result == 3)
                {
                    Form3 dialogForm1 = new Form3(fontList);
                    DialogResult result1 = dialogForm1.ShowDialog();
                    if (result1 == DialogResult.OK)
                    {
                        databaseHelper.SetPermission(true);
                        attempts = 0;
                        button1.PerformClick();
                    }
                }
                else
                {
                    attempts++;
                    if (result == 1)
                    {
                        MessageBox.Show("Пользователь с указанным логином не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (result == 2)
                    {
                        MessageBox.Show("Неправильный пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (result == 5)
                    {
                        MessageBox.Show("Неизвестная ошибка. Попробуйте позже.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (attempts == 3)
                    {
                        attempts = 0;
                        MessageBox.Show("Превышено число запросов. Форма заблокирована на 20 секунд.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        BlockPanel();
                    }
                }
            }
            else
            {
                textBox3.Clear();
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {

        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {

        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackgroundImage = butAuthHover;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackgroundImage = butAuthOn;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            program_panel.Visible = false;
            floatNavigation.Visible = false;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ConstHandler.CheckLength(textBox2, label21, 4);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            ConstHandler.CheckLength(textBox3, label31, 8);
        }

        private void BlockPanel()
        {
            // Блокировка Panel
            auth_panel.Enabled = false;
            button1.BackgroundImage = butAuthOff;

            // Запуск таймера на 20 секунд
            blockTimer.Start();
        }

        private void UnblockPanel(object sender, EventArgs e)
        {
            // Разблокировка Panel
            auth_panel.Enabled = true;
            button1.BackgroundImage = butAuthOn;

            // Остановка таймера
            blockTimer.Stop();
        }

        private void button_register_MouseEnter(object sender, EventArgs e)
        {
            button_register.BackgroundImage = butRegisterHover;
        }

        private void button_register_MouseLeave(object sender, EventArgs e)
        {
            button_register.BackgroundImage = butRegisterOn;
        }

        private void button_register_Click(object sender, EventArgs e)
        {
            Form2 dialogForm = new Form2(fontList, databaseHelper);
            DialogResult result = dialogForm.ShowDialog();
        }

        private void buttonHide1_Click(object sender, EventArgs e)
        {
            if (textBox3.PasswordChar == '•')
            {
                textBox3.PasswordChar = '\0';
                buttonHide1.BackgroundImage = butHideOn;
            }
            else
            {
                textBox3.PasswordChar = '•';
                buttonHide1.BackgroundImage = butHideOff;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Form2 dialogForm = new Form2(fontList, databaseHelper);
            DialogResult result = dialogForm.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button_home_Click(object sender, EventArgs e)
        {
            if (button_home.Tag.ToString().Equals("0"))
            {
                this.SuspendLayout();
                button_home.Tag = 1;
                button_home.BackgroundImage = minibutHomeOn;
                button_print.Tag = 0;
                button_graph.Tag = 0;
                button_records.Tag = 0;
                button_print.BackgroundImage = minibutPrintOff;
                button_graph.BackgroundImage = minibutStatsOff;
                button_records.BackgroundImage = minibutRecordsOff;
                program_panel.Visible = true;
                print_panel.Visible = false;
                graph_panel.Visible = false;
                records_panel.Visible = false;
                this.ResumeLayout();
            }
        }

        private void button_print_Click(object sender, EventArgs e)
        {
            if (button_print.Tag.ToString().Equals("0"))
            {
                this.SuspendLayout();
                button_print.Tag = 1;
                button_print.BackgroundImage = minibutPrintOn;
                button_home.Tag = 0;
                button_graph.Tag = 0;
                button_records.Tag = 0;
                button_home.BackgroundImage = minibutHomeOff;
                button_graph.BackgroundImage = minibutStatsOff;
                button_records.BackgroundImage = minibutRecordsOff;
                program_panel.Visible = false;
                print_panel.Visible = true;
                graph_panel.Visible = false;
                records_panel.Visible = false;
                this.ResumeLayout();
            }
        }

        private void button_graph_Click(object sender, EventArgs e)
        {
            if (button_graph.Tag.ToString().Equals("0"))
            {
                this.SuspendLayout();
                button_graph.Tag = 1;
                button_graph.BackgroundImage = minibutStatsOn;
                button_home.Tag = 0;
                button_print.Tag = 0;
                button_records.Tag = 0;
                button_home.BackgroundImage = minibutHomeOff;
                button_print.BackgroundImage = minibutPrintOff;
                button_records.BackgroundImage = minibutRecordsOff;
                program_panel.Visible = false;
                print_panel.Visible = false;
                records_panel.Visible = false;
                graph_panel.Visible = true;
                this.ResumeLayout();
            }
        }

        private void button_records_Click(object sender, EventArgs e)
        {
            if (button_records.Tag.ToString().Equals("0"))
            {
                this.SuspendLayout();
                button_graph.Tag = 0;
                button_graph.BackgroundImage = minibutStatsOff;
                button_home.Tag = 0;
                button_print.Tag = 0;
                button_records.Tag = 1;
                button_home.BackgroundImage = minibutHomeOff;
                button_print.BackgroundImage = minibutPrintOff;
                button_records.BackgroundImage = minibutRecordsOn;
                program_panel.Visible = false;
                print_panel.Visible = false;
                graph_panel.Visible = false;
                records_panel.Visible = true;
                this.ResumeLayout();
            }
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PdfPrinter.SaveAndPrintPdf(richTextBox1);
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void buttonGraph_Click(object sender, EventArgs e)
        {
            // Построение графика
            GraphicHandler.InitialiseGraph(zedGraphControl1);
        }

        private void button_reset_graph_Click(object sender, EventArgs e)
        {
            GraphicHandler.ResetView(zedGraphControl1);
        }

        private void records_panel_VisibleChanged(object sender, EventArgs e)
        {
            if (records_panel.Visible)
            {
                DataGridHelper.SetDataSource(databaseHelper, dataGridView1); 
            }
        }

        private void button_rec_add_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(fontList);

            if (form4.ShowDialog() == DialogResult.OK)
            {
                string enteredText = form4.EnteredText;
                DataGridHelper.AddRecord(databaseHelper, dataGridView1, enteredText);
            }
        }

        private void button_rec_edit_Click(object sender, EventArgs e)
        {
            DataGridHelper.EditRecord(databaseHelper, dataGridView1, fontList);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataGridHelper.DeleteRecords(databaseHelper, dataGridView1);
        }

        private void button_rec_upd_Click(object sender, EventArgs e)
        {
            DataGridHelper.SetDataSource(databaseHelper, dataGridView1);
        }
    }
}

