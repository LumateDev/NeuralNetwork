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


namespace WindowsFormsApp1
{
    public partial class Form1 : Form

    {

        int[] inputData = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int numbericValue = 0;


        public Form1()
        {
            InitializeComponent();
            inputData = new int[15]; 
        }

        private void onChangeButton(Button b, int index)
        {
            if(b.BackColor == Color.LightGray)
            {
                b.BackColor = Color.Black;
                inputData[index] = 1; 
            }
            else
            {
                b.BackColor = Color.LightGray;
                inputData[index] = 0;
            }
        }

        private void digit__input1_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input1,0);
        }
       

        private void digit__input2_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input2, 1);
        }

        private void digit__input3_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input3, 2);
        }

        private void digit__input4_Click(object sender, EventArgs e)

        {
            onChangeButton(digit__input4, 3);
        }

        private void digit__input5_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input5, 4); ;
        }

        private void digit__input6_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input6, 5);
        }

        private void digit__input7_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input7, 6);
        }

        private void digit__input8_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input8, 7);
        }

        private void digit__input9_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input9, 8);
        }

        private void digit__input10_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input10, 9);
        }

        private void digit__input11_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input11, 10);
        }

        private void digit__input12_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input12, 11);
        }

        private void digit__input13_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input13, 12);
        }

        private void digit__input14_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input14, 13);
        }

        private void digit__input15_Click(object sender, EventArgs e)
        {
            onChangeButton(digit__input15, 14);
        }

        private void buttonAnalyze_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numbericValue = Decimal.ToInt32(numericUpDown1.Value);
        }

        private void buttonTrain_Click(object sender, EventArgs e)
        {
            string pathDirMyApp = AppDomain.CurrentDomain.BaseDirectory;
            string strTrainEx = numericUpDown1.Value.ToString() + ":";
            for (int i = 0; i < inputData.Length; i++)
            {
                strTrainEx += " " + inputData[i].ToString();
            }
            strTrainEx += "\n";
            pathDirMyApp += "buttonSaveTrainSample.txt";
            File.AppendAllText(pathDirMyApp, strTrainEx);
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            string pathDirMyApp = AppDomain.CurrentDomain.BaseDirectory;
            string strTrainEx = numericUpDown1.Value.ToString() + ":";
            for (int i = 0; i < inputData.Length; i++)
            {
                strTrainEx += " " + inputData[i].ToString();
            }
            strTrainEx += "\n";
            pathDirMyApp += "buttonSaveTestSample.txt";
            File.AppendAllText(pathDirMyApp, strTrainEx);
        }
    }
}
