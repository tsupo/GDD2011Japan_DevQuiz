using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace slidePuzzle
{
    public partial class Form1 : Form
    {
        private int numberOfProblems = 0;
        private int currentProblem = 0;
        private int maxL = 99999;
        private int maxR = 99999;
        private int maxU = 99999;
        private int maxD = 99999;
        private int remainOfL = 99999;
        private int remainOfR = 99999;
        private int remainOfU = 99999;
        private int remainOfD = 99999;
        private int safeRemainOfL = 99999;
        private int safeRemainOfR = 99999;
        private int safeRemainOfU = 99999;
        private int safeRemainOfD = 99999;
        private int widthOfCurrent = 3;
        private int heightOfCurrent = 3;
        private const int maxWidth = 6;
        private List<Problem> problems = null;
        private List<Button> buttons = null;

        #region Initialize Problems
        public Form1()
        {
            InitializeComponent();

            initButtons();

            currentProblem = 0;
            loadProblems();
            loadAnswers();
            displayProblem(currentProblem);
        }

        private void initButtons()
        {
            buttons = new List<Button>();

            buttons.Add(button1);
            buttons.Add(button2);
            buttons.Add(button3);
            buttons.Add(button4);
            buttons.Add(button5);
            buttons.Add(button6);
            buttons.Add(button7);
            buttons.Add(button8);
            buttons.Add(button9);
            buttons.Add(button10);
            buttons.Add(button11);
            buttons.Add(button12);
            buttons.Add(button13);
            buttons.Add(button14);
            buttons.Add(button15);
            buttons.Add(button16);
            buttons.Add(button17);
            buttons.Add(button18);
            buttons.Add(button19);
            buttons.Add(button20);
            buttons.Add(button21);
            buttons.Add(button22);
            buttons.Add(button23);
            buttons.Add(button24);
            buttons.Add(button25);
            buttons.Add(button26);
            buttons.Add(button27);
            buttons.Add(button28);
            buttons.Add(button29);
            buttons.Add(button30);
            buttons.Add(button31);
            buttons.Add(button32);
            buttons.Add(button33);
            buttons.Add(button34);
            buttons.Add(button35);
            buttons.Add(button36);

            foreach (Button b in buttons)
            {
                b.Click += new EventHandler(button_Click);
                b.KeyPress += new KeyPressEventHandler(button_KeyPress);
            }
        }

        private void loadProblems()
        {
            using (StreamReader sr = new StreamReader("problem.txt", Encoding.GetEncoding("utf-8")))
            {
                string t;
                long line = 0;

                while ((t = sr.ReadLine()) != null)
                {
                    line++;
                    if (line == 1)
                    {
                        string strL, strR, strU, strD;
                        int pos1 = t.IndexOf(' ');
                        int pos2 = t.IndexOf(' ', pos1 + 1);
                        int pos3 = t.IndexOf(' ', pos2 + 1);

                        strL = t.Substring(0, pos1);
                        safeRemainOfL = remainOfL = maxL = int.Parse(strL);

                        strR = t.Substring(pos1 + 1, pos2 - (pos1 + 1));
                        safeRemainOfR = remainOfR = maxR = int.Parse(strR);

                        strU = t.Substring(pos2 + 1, pos3 - (pos2 + 1));
                        safeRemainOfU = remainOfU = maxU = int.Parse(strU);

                        strD = t.Substring(pos3 + 1);
                        safeRemainOfD = remainOfD = maxD = int.Parse(strD);
                        continue;
                    }

                    if (line == 2)
                    {
                        numberOfProblems = int.Parse(t);
                        continue;
                    }

                    if (problems == null)
                        problems = new List<Problem>();

                    string strW, strH, content;
                    int w, h;
                    int posA = t.IndexOf(',');
                    int posB = t.IndexOf(',', posA + 1);

                    strW = t.Substring(0, posA);
                    w = int.Parse(strW);

                    strH = t.Substring(posA + 1, posB - (posA + 1));
                    h = int.Parse(strH);

                    content = t.Substring(posB + 1);

                    Problem item = new Problem(w, h, content);
                    problems.Add(item);
                }
            }
        }

        private void loadAnswers()
        {
            using (StreamReader sr = new StreamReader("answer.txt", Encoding.GetEncoding("utf-8")))
            {
                string t;
                int line = 0;

                while ((t = sr.ReadLine()) != null)
                {
                    if (t.Length > 0)
                    {
                        problems[line].answer = t;
                        problems[line].answerLocked = true;

                        foreach (char c in t)
                        {
                            switch (c)
                            {
                                case 'U':
                                    remainOfU--;
                                    break;
                                case 'D':
                                    remainOfD--;
                                    break;
                                case 'L':
                                    remainOfL--;
                                    break;
                                case 'R':
                                    remainOfR--;
                                    break;
                            }
                        }
                    }
                    line++;
                }
            }
        }
        #endregion

        #region Display specified Problem
        private void displayProblem(int problemNumber)
        {
            Problem problem = problems[problemNumber];
            currentProblem = problemNumber;

            widthOfCurrent = problem.width;
            heightOfCurrent = problem.height;

            record.Text = problem.answerLocked ? problem.answer : "";
            record.Enabled = !problem.answerLocked;

            remainOfL = safeRemainOfL;
            remainOfR = safeRemainOfR;
            remainOfU = safeRemainOfU;
            remainOfD = safeRemainOfD;

            quizNumber.Text = "問題 " + (problemNumber + 1) + "/" + numberOfProblems;
            remainL.Text = "L: 残り " + remainOfL;
            remainR.Text = "R: 残り " + remainOfR;
            remainU.Text = "U: 残り " + remainOfU;
            remainD.Text = "D: 残り " + remainOfD;

            if (currentProblem == 0)
                prevButton.Enabled = false;
            else
                prevButton.Enabled = true;

            if (currentProblem == numberOfProblems - 1)
                nextButton.Enabled = false;
            else
                nextButton.Enabled = true;

            int loc = 0;
            foreach (Button b in buttons)
            {
                b.Text = " ";
                b.Enabled = false;
                b.Visible = false;
            }
            foreach (SlidePanel p in problem.panels)
            {
                int locW = loc % problem.width;
                int locH = loc / problem.width;
                Button b = buttons[locW + locH * maxWidth];

                b.Text = p.view + "";
                b.Enabled = true;
                b.Visible = true;
                b.ForeColor = Color.Black;
                b.BackColor = Color.WhiteSmoke;

                if (p.view == '=')
                {
                    b.Enabled = false;
                    b.ForeColor = Color.Black;
                    b.BackColor = Color.LightGray;
                }
                if (p.view == '0')
                {
                    b.ForeColor = Color.Blue;
                    b.BackColor = Color.Wheat;
                }

                loc++;
            }

            if (problem.answer.Length > 0)
                replay();

            foreach (Button b in buttons)
            {
                if (b.Text == "0")
                {
                    b.Focus();
                    break;
                }
            }
        }

        private void readyToJump()
        {
            if (remainOfL + remainOfR + remainOfU + remainOfD <
                safeRemainOfL + safeRemainOfR + safeRemainOfU + safeRemainOfD)
                problems[currentProblem].answerLocked = true;
            safeRemainOfL = remainOfL;
            safeRemainOfR = remainOfR;
            safeRemainOfU = remainOfU;
            safeRemainOfD = remainOfD;
        }
        #endregion

        #region Go to previous Problem
        private void prevButton_Click(object sender, EventArgs e)
        {
            if (currentProblem > 0)
            {
                readyToJump();
                displayProblem(currentProblem - 1);
            }
        }
        #endregion

        #region Go to next Problem
        private void nextButton_Click(object sender, EventArgs e)
        {
            if (currentProblem < numberOfProblems - 1)
            {
                readyToJump();
                displayProblem(currentProblem + 1);
            }
        }
        #endregion

        #region Go to Specified Problem (Jump)
        private void jump()
        {
            Form2 dlg = new Form2();
            dlg.problemNumber = 0;
            dlg.maxProblemNumber = numberOfProblems;
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                readyToJump();
                displayProblem(dlg.problemNumber - 1);
            }
        }
        #endregion

        #region Move Slides (Slide Panels)
        private void swapButton(int loc1, int loc2)
        {
            swapButton(buttons[loc1], buttons[loc2]);
        }

        private void swapButton(Button b1, Button b2)
        {
            Button temp = new Button();
            temp.Text = b1.Text;
            temp.ForeColor = b1.ForeColor;
            temp.BackColor = b1.BackColor;

            b1.Text = b2.Text;
            b1.ForeColor = b2.ForeColor;
            b1.BackColor = b2.BackColor;

            b2.Text = temp.Text;
            b2.ForeColor = temp.ForeColor;
            b2.BackColor = temp.BackColor;
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.Text == "0")
                return;

            if (problems[currentProblem].answerLocked)
                return;

            int loc = -1;
            foreach (Button bb in buttons)
            {
                loc++;

                int locW = loc % maxWidth;
                int locH = loc / maxWidth;
                if (bb.Text == b.Text)
                {
                    /* 上のパネルが 0 かどうか */
                    if (locH > 0)
                    {
                        if (buttons[locW + (locH - 1) * maxWidth].Text == "0")
                        {
                            // パネル入れ替え
                            swapButton(loc, locW + (locH - 1) * maxWidth);
                            record.Text += "D";
                            remainOfD--;
                            remainD.Text = "D: 残り " + remainOfD;
                        }
                    }

                    /* 下のパネルが 0 かどうか */
                    if (locH < heightOfCurrent - 1)
                    {
                        if (buttons[locW + (locH + 1) * maxWidth].Text == "0")
                        {
                            // パネル入れ替え
                            swapButton(loc, locW + (locH + 1) * maxWidth);
                            record.Text += "U";
                            remainOfU--;
                            remainU.Text = "U: 残り " + remainOfU;
                        }
                    }

                    /* 左のパネルが 0 かどうか */
                    if (locW > 0)
                    {
                        if (buttons[(locW - 1) + locH * maxWidth].Text == "0")
                        {
                            // パネル入れ替え
                            swapButton(loc, (locW - 1) + locH * maxWidth);
                            record.Text += "R";
                            remainOfR--;
                            remainR.Text = "R: 残り " + remainOfR;
                        }
                    }

                    /* 右のパネルが 0 かどうか */
                    if (locW < widthOfCurrent - 1)
                    {
                        if (buttons[(locW + 1) + locH * maxWidth].Text == "0")
                        {
                            // パネル入れ替え
                            swapButton(loc, (locW + 1) + locH * maxWidth);
                            record.Text += "L";
                            remainOfL--;
                            remainL.Text = "L: 残り " + remainOfL;
                        }
                    }

                    break;
                }
            }

            problems[currentProblem].answer = record.Text;
        }
        #endregion

        #region Redo from start
        private void retryButton_Click(object sender, EventArgs e)
        {
            problems[currentProblem].answerLocked = false;
            displayProblem(currentProblem);
        }
        #endregion

        #region Lock Answer
        private void record_DoubleClick(object sender, EventArgs e)
        {
            if (problems[currentProblem].answerLocked)
            {
                problems[currentProblem].answerLocked = false;
                record.Enabled = true;
            }
            else
            {
                problems[currentProblem].answerLocked = true;
                record.Enabled = false;
            }
        }
        #endregion

        #region Undo
        void button_KeyPress(object sender, KeyPressEventArgs e)
        {
            Form1_KeyPress(sender, e);
        }

        private void record_KeyPress(object sender, KeyPressEventArgs e)
        {
            Form1_KeyPress(sender, e);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 26) // ctrl+Z
                undo();
            else if (e.KeyChar == 7 ||  // ctr+G
                     e.KeyChar == 10)   // ctrl+J
                jump();
        }

        private void undo()
        {
            string currentRecord = record.Text;
            if (currentRecord.Length < 1)
                return;
            if (problems[currentProblem].answerLocked)
                return;

            string lastCommand = currentRecord.Substring(currentRecord.Length - 1, 1);
            string preRecord = currentRecord.Substring(0, currentRecord.Length - 1);
            record.Text = preRecord;

            int loc = -1;
            foreach (Button bb in buttons)
            {
                loc++;

                int locW = loc % maxWidth;
                int locH = loc / maxWidth;
                if (bb.Text == "0")
                {
                    if (lastCommand == "U")
                    {
                        // 下のパネルと入れ替え
                        swapButton(loc, locW + (locH + 1) * maxWidth);
                        remainOfU++;
                        remainU.Text = "U: 残り " + remainOfU;
                    }
                    else if (lastCommand == "D")
                    {
                        // 上のパネルと入れ替え
                        swapButton(loc, locW + (locH - 1) * maxWidth);
                        remainOfD++;
                        remainD.Text = "D: 残り " + remainOfD;
                    }
                    else if (lastCommand == "L")
                    {
                        // 右のパネルと入れ替え
                        swapButton(loc, (locW + 1) + locH * maxWidth);
                        remainOfL++;
                        remainL.Text = "L: 残り " + remainOfL;
                    }
                    else if (lastCommand == "R")
                    {
                        // 左のパネルと入れ替え
                        swapButton(loc, (locW - 1) + locH * maxWidth);
                        remainOfR++;
                        remainR.Text = "R: 残り " + remainOfR;
                    }

                    break;
                }
            }
        }
        #endregion

        #region Replay
        private void movePanel(char direction)
        {
            int loc = -1;
            foreach (Button bb in buttons)
            {
                loc++;

                int locW = loc % maxWidth;
                int locH = loc / maxWidth;
                if (bb.Text == "0")
                {
                    switch (direction)
                    {
                        case 'U':
                            swapButton(loc, locW + (locH - 1) * maxWidth);
                            break;
                        case 'D':
                            swapButton(loc, locW + (locH + 1) * maxWidth);
                            break;
                        case 'L':
                            swapButton(loc, (locW - 1) + locH * maxWidth);
                            break;
                        case 'R':
                            swapButton(loc, (locW + 1) + locH * maxWidth);
                            break;
                    }
                    break;
                }
            }
        }

        private void replay()
        {
            string currentRecord = record.Text;
            if (currentRecord.Length < 1)
                return;

            foreach (char c in currentRecord)
            {
                switch (c)
                {
                    case 'U':
                    case 'D':
                    case 'L':
                    case 'R':
                        movePanel( c );
                        break;
                }
            }
        }
        #endregion

        #region Close this form
        protected override void OnClosed(EventArgs e)
        {
            string answerFile = "answer.txt";

            if (File.Exists(answerFile))
            {
                DateTime d = DateTime.Now;
                CultureInfo c = new CultureInfo("en-US", false);
                string backupFile = "answer_" + d.ToString("yyyyMMddHHmmssfff", c) + ".txt";
                File.Copy(answerFile, backupFile);
            }

            using (StreamWriter sw = new StreamWriter(answerFile, false))
            {
                foreach (Problem p in problems)
                {
                    sw.WriteLine(p.answer);
                }
            }

            base.OnClosed(e);
        }
        #endregion
    }
}
