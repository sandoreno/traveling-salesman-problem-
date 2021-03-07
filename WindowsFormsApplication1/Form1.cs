using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        public static int numb;
        List<int> xx = new List<int>();
        List<int> yy = new List<int>();
        public int x_MouseDown_1, y_MouseDown_1;
        int click_counter;

        static double LineDlin(int[] put, double[,] L)
        {
            double L0 = 0;
            for (int j = 0; j < put.Length - 1; j++)
            {
                L0 = L[put[j], put[j + 1]] + L0;
            }
            return L0;
        }       
        static int[] FormulaPut(double[,] L, float[,] T, int n, int i, double a, double b)
        {
            double dLT = 0;
            Boolean[] bol = new Boolean[n];
            double[] P = new double[n];
            int[] put = new int[n + 1];
            for (int j = 0; j < n; j++)
            {
                if (L[i, j] == 0)
                {
                    bol[j] = false;
                }
                else
                {
                    bol[j] = true;
                }
            }
            for (int x = 0; x < n; x++)
            {
                put[x] = i;
                for (int j = 0; j < n; j++)
                {
                    if (bol[j] == false)
                    {
                        bol[j] = false;
                    }
                    else
                    {
                        dLT = Math.Pow(1 / L[i, j], b) * Math.Pow(T[i, j], a) + dLT;
                    }
                }
                for (int j = 0; j < n; j++)
                {
                    if (bol[j] == false)
                    {
                        bol[j] = false;
                    }
                    else
                    {
                        P[j] = ((Math.Pow(1 / L[i, j], b) * Math.Pow(T[i, j], a)) / dLT) * 100;
                    }
                }
                Random r = new Random();
                int rand = r.Next(0, 100);
                int k = 0;
                int[] mas1 = new int[P.Length];
                double[] mas2 = new double[P.Length];
                double Q = 0;
                for (int j = 0; j < P.Length; j++)
                {
                    if (bol[j] != false)
                    {
                        mas1[k] = j;
                        k++;
                    }
                }
                k = 0;
                for (int j = 0; j < P.Length; j++)
                {
                    if (bol[j] != false)
                    {
                        mas2[k] = Q;
                        Q = P[j] + Q;
                        k++;
                    }
                    mas2[k] = Q;
                }
                Q = 0;
                int t = 0;
                for (int j = 0; j < mas2.Length - 1; j++)
                {
                    if (rand <= mas2[j + 1] && mas2[j] != 0)
                    {
                        if (rand >= mas2[j] && mas2[j] != 0)
                        {
                            t = j;
                        }
                    }
                }
                bol[mas1[t]] = false;
                i = mas1[t];
                dLT = 0;
            }
            put[put.Length - 1] = put[0];
            return put;
        }
        static double Temperatura(double T, double a)
        {
            double Tk;
            return Tk = a * T;
        }
        static double Dlina(double[,] mas, int[] Put)
        {
            double Dlina = 0;
            for (int i = 0; i < Put.Length - 1; i++)
            {
                Dlina = mas[Put[i], Put[i + 1]] + Dlina;
            }
            return Dlina;
        }
        static int[] Zamena(int[] Put)
        {
            int a, b;
            int c;
            Random r = new Random();
            a = r.Next(1, Put.Length - 1);
            b = r.Next(1, Put.Length - 1);
            while (a == b)
            {
                b = r.Next(1, Put.Length - 1);
            }
            c = Put[a];
            Put[a] = Put[b];
            Put[b] = c;
            return Put;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (xx.Count < 4)
                {
                    MessageBox.Show("Давай-ка сам");
                    panel1.Refresh();
                    xx.Clear();
                    yy.Clear();
                    numb = 0;
                    click_counter = 0;
                    textBox1.Text = Convert.ToString(numb);
                }
                else
                {
                    double[,] mas = new double[numb, numb];
                    for (int i = 0; i < numb; i++)
                    {
                        for (int j = 0; j < numb; j++)
                        {
                            if (i == j)
                            {
                                mas[i, j] = 0;
                            }
                            else if (mas[i, j] != 0)
                            {
                                j++;
                            }
                            else
                            {
                                mas[i, j] = Math.Sqrt(Math.Pow(xx[i] - xx[j], 2) + Math.Pow(yy[i] - yy[j], 2));
                                mas[j, i] = mas[i, j];

                            }
                        }
                    }
                    Random r = new Random();
                    double T = Convert.ToDouble(textBox2.Text);
                    double Tmin = Convert.ToDouble(textBox3.Text);
                    double a = Convert.ToDouble(textBox4.Text);
                    int n = numb + 1;
                    int[] Put = new int[n];
                    int[] vPut = new int[n];
                    for (int i = 0; i < numb; i++)
                    {
                        Put[i] = i;
                    }
                    Put[numb] = Put[0];
                    double D = Dlina(mas, Put);
                    double D1;
                    double P = 0;
                    double Rand;
                    while (T > Tmin)
                    {
                        T = Temperatura(T, a);
                        Put = Zamena(Put);
                        D1 = Dlina(mas, Put);
                        if (D1 - D <= 0)
                        {
                            D = D1;
                            vPut = Put;
                        }
                        else
                        {

                            P = 100 * Math.Pow(Math.E, -(D1 - D) / T);
                            Rand = r.Next(0, 100);
                            if (Rand < P)
                            {
                                D = D1;
                                vPut = Put;
                            }
                        }
                        panel1.Refresh();
                        Graphics g = panel1.CreateGraphics();
                        Pen p = new Pen(Color.Red, 3);
                        for (int i = 0; i < numb; i++)
                        {
                            g.DrawLine(p, xx[vPut[i]], yy[vPut[i]], xx[vPut[i + 1]], yy[vPut[i + 1]]);
                        }

                        label2.Text = D.ToString();
                    }
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (xx.Count < 4)
                {
                    MessageBox.Show("Давай-ка сам");
                    panel1.Refresh();
                    xx.Clear();
                    yy.Clear();
                    numb = 0;
                    click_counter = 0;
                    textBox1.Text = Convert.ToString(numb);
                }
                else
                {
                    double[,] mas = new double[numb, numb];
                    for (int i = 0; i < numb; i++)
                    {
                        for (int j = 0; j < numb; j++)
                        {
                            if (i == j)
                            {
                                mas[i, j] = 0;
                            }
                            else if (mas[i, j] != 0)
                            {
                                j++;
                            }
                            else
                            {
                                mas[i, j] = Math.Sqrt(Math.Pow(xx[i] - xx[j], 2) + Math.Pow(yy[i] - yy[j], 2));
                                mas[j, i] = mas[i, j];

                            }
                        }
                    }
                    Random r = new Random();
                    float[,] fer = new float[numb, numb];
                    for (int i = 0; i < numb; i++)
                    {
                        for (int j = 0; j < numb; j++)
                        {
                            if (i == j)
                            {
                                fer[i, j] = 0;
                            }
                            else if (fer[i, j] != 0)
                            {
                                j++;
                            }
                            else
                            {
                                fer[i, j] = r.Next(1, 3);
                                fer[j, i] = fer[i, j];
                            }
                        }
                    }
                    double L1 = 99999999999999999;
                    double dT;
                    int n;
                    int[] vPut = new int[numb + 1];
                    double a = Convert.ToDouble(textBox6.Text);
                    double b = Convert.ToDouble(textBox7.Text);
                    double pp = Convert.ToDouble(textBox9.Text);
                    double M = Convert.ToDouble(textBox5.Text);
                    double Q = Convert.ToDouble(textBox8.Text);
                    for (int j = 0; j < M; j++)
                    {
                        n = r.Next(0, numb);
                        int[] put = FormulaPut(mas, fer, numb, n, a, b);
                        double L0 = LineDlin(put, mas);
                        if (L0 <= L1)
                        {
                            L1 = L0;
                            vPut = put;
                        }
                        else
                        {
                            dT = Q / L0;
                            for (int i = 0; i < put.Length - 1; i++)
                            {
                                fer[put[i], put[i + 1]] = (float)(1 - pp) * fer[put[i], put[i + 1]] + (float)dT;
                                fer[put[i + 1], put[i]] = (float)(1 - pp) * fer[put[i + 1], put[i]] + (float)dT;
                            }
                        }
                    }
                    panel1.Refresh();
                    Graphics g = panel1.CreateGraphics();
                    Pen p = new Pen(Color.Green, 3);
                    for (int i = 0; i < numb; i++)
                    {
                        g.DrawLine(p, xx[vPut[i]], yy[vPut[i]], xx[vPut[i + 1]], yy[vPut[i + 1]]);
                    }
                    label5.Text = L1.ToString();
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
        panel1.Refresh();
        numb = Convert.ToInt32(textBox1.Text);
        Random r = new Random();
        Graphics g = panel1.CreateGraphics();
        Pen p = new Pen(Color.Black, 3);
        SolidBrush sb = new SolidBrush(Color.Black);
        int h = panel1.Size.Height;
        int w = panel1.Size.Width;
        xx.Clear();
        yy.Clear();
        for (int i = 0; i < numb; i++)
            {
            int x = r.Next(0, w);
            int y = r.Next(0, h);
            xx.Add(x);
            yy.Add(y);
            g.DrawEllipse(p, x, y, 3, 3);
            g.FillEllipse(sb, x, y, 3, 3);
            }
            for (int i = 0; i < numb; i++)
            {
                for (int j = 0; j < numb; j++)
                {
                    g.DrawLine(p, xx[i], yy[i], xx[j], yy[j]);
                }
            }

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Label10_Click(object sender, EventArgs e)
        {

        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {        
            if (checkBox1.Checked)
            {
                panel1.Refresh();
                panel1.Update();
                x_MouseDown_1 = e.X;
                y_MouseDown_1 = e.Y;
                Graphics gp = panel1.CreateGraphics();
                Pen myPen = new Pen(Color.Black, 3);
                gp.DrawEllipse(myPen, x_MouseDown_1, y_MouseDown_1, 5, 5);
                xx.Add(x_MouseDown_1);
                yy.Add(y_MouseDown_1);
                click_counter++;
                numb = click_counter;
                textBox1.Text = Convert.ToString(numb);
                for (int i = 0; i < numb; i++)
                {
                    for (int j = 0; j < numb; j++)
                    {
                        gp.DrawLine(myPen, xx[i], yy[i], xx[j], yy[j]);
                    }
                }

            }
            else { MessageBox.Show("Не нажата галочка"); }
        }
    }
}
