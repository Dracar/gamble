using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace gamble
{
    public partial class Form1 : Form
    {
        public int[] redNumber = new int[] {1, 3, 5, 7, 9, 12, 14, 16, 
                                        18, 19, 21, 23, 25, 27, 30, 32, 34, 36};
        public int[] blackNumber = new int[] {2, 4, 6, 8, 10, 11, 13, 15, 
                                        17, 20, 22, 24, 26, 28, 29, 31, 33, 35};

        int numberOfRuns = 0;
        int introBet = 0;
        int introBalance = 0;
        int moneyGoal = 0;
        int spinsToSit = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (formCompleted())
            {
                button1.Enabled = false;
                Thread t = new Thread(new ThreadStart(run));
                t.IsBackground = true;
                t.Start();
            }
        }

        public void run()
        {
            introBet = 0;
            introBalance = 0;
            moneyGoal = 0;
            spinsToSit = 0;

            if (radioButton1.Checked)
            {
                numberOfRuns = 5000000;
            }
            else if (radioButton2.Checked)
            {
                numberOfRuns = 100000;
            }
            else if (radioButton3.Checked)
            {
                numberOfRuns = 10000;
            }

            if (formCompleted())
            {
                for (int n = 0; n < numberOfRuns; n++)
                {
                    if (color(Spin()) == "red")
                    {
                        Invoke(new dUpdatePercentGoal(updatePercentGoal), new object[] { 1 });
                    }
                    else
                    {
                        Invoke(new dUpdatePercentGoal(updatePercentGoal), new object[] { 0 });
                    }
                    Invoke(new dUpdateProgress(updateProgress), new object[] { 1 });
                    Thread.Sleep(1000);
                }
            }
        }

        public void updatePercentGoal(int i)
        {
            if (i == 1)
            {
                label7.Text = "You hit Red!";
            }
            else
            {
                label7.Text = "You hit Black!";
            }
        }

        public String color(int num)
        {
            if (Array.IndexOf(redNumber, num) != -1){
                return "red";
            }
            else if (Array.IndexOf(blackNumber, num) != -1)
            {
                return "black";
            }
            else
            {
                return "green";
            }
        }

        Random rnd = new Random();
        protected int Spin()
        {
            return rnd.Next(0, 37);
        }

        public void updateProgress(int i)
        {
            if (progressBar1.Value == 100)
            {
                progressBar1.Value = 0;
            }
            else
            {
                progressBar1.Value += i;
            }
        }
        private void textBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (int.TryParse(textBox1.Text.Trim(), out introBalance))
            {
                if (introBalance > 0)
                {
                    error(textBox1, true);
                }
                else {
                    error(textBox1);
                }
            }
            else
            {
                error(textBox1);
            }
            if (int.TryParse(textBox2.Text.Trim(), out introBet))
            {
                if (introBet < introBalance)
                {
                    error(textBox2, true);
                }
                else
                {
                    error(textBox2);
                }
            }
            else
            {
                error(textBox2);
            }
            if (int.TryParse(textBox3.Text.Trim(), out moneyGoal))
            {
                if (moneyGoal > introBalance)
                {
                    error(textBox3, true);
                }
                else
                {
                    error(textBox3);
                }
            }
            else
            {
                error(textBox3);
            }
            if (int.TryParse(textBox4.Text.Trim(), out spinsToSit))
            {
                if (spinsToSit > 0)
                {
                    error(textBox4, true);
                }
                else
                {
                    error(textBox4);
                }
            }
            else
            {
                error(textBox4);
            }
        }
        public void error(Control o, bool clear = false)
        {
            if (!clear){
                errorProvider1.SetError(o, "Invalid Entry!");
            } else {
                errorProvider1.SetError(o, "");
            }
            
        }

        public bool formCompleted()
        {
            if (errorProvider1.GetError(this.textBox1).Length > 0 ||
                errorProvider1.GetError(this.textBox2).Length > 0 ||
                errorProvider1.GetError(this.textBox3).Length > 0 ||
                errorProvider1.GetError(this.textBox4).Length > 0){
                return false;
            } 
            else 
            {
                return true;
            }
        }
    }

    public delegate void dUpdateProgress(int i);
    public delegate void dUpdatePercentGoal(int i);
    public delegate void dUpdatePercentWalked(int i);

}