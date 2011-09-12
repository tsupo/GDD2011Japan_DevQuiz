using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace slidePuzzle
{
    public partial class Form2 : Form
    {
        public int problemNumber = 0;
        public int maxProblemNumber = 0;

        public Form2()
        {
            InitializeComponent();

            label1.Text = "問題番号 (1-" + maxProblemNumber + ")";
        }

        private void OK_Click(object sender, EventArgs e)
        {
            bool error = false;

            try
            {
                int n = int.Parse(number.Text);
                if (n > 0 && n < maxProblemNumber)
                    problemNumber = n;
                else
                    error = true;
            }
            catch
            {
                error = true;
            }

            if (error)
                DialogResult = DialogResult.Cancel;
        }

        private bool enterd = false;
        private void number_TextChanged(object sender, EventArgs e)
        {
            if (!enterd)
            {
                enterd = true;

                try
                {
                    int n = int.Parse(number.Text);
                    if (n < 0 || n > maxProblemNumber)
                        MessageBox.Show("1 ～ " + maxProblemNumber +
                                        " の範囲の数値を入力してください",
                                        "入力エラー",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                }
                catch
                {
                    MessageBox.Show("数値を入力してください",
                                    "入力エラー",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    number.Clear();
                }

                enterd = false;
            }
        }
    }
}
