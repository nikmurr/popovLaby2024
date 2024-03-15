using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PopovLaba3
{
    public partial class Form4 : Form
    {
        private List<Font> fontList;
        public Form4(List<Font> fontList)
        {
            InitializeComponent();
            this.fontList = fontList;

            label.Font = fontList[0];
            textBox3.Font = fontList[2];

            textBox3.Focus();
        }

        public string EnteredText { get; private set; }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox3.Text;
            if (!string.IsNullOrEmpty(text))
            {
                EnteredText = text;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Введите хоть что-нибудь :(", "Ошибка", MessageBoxButtons.OK);
            }
        }
    }
}
