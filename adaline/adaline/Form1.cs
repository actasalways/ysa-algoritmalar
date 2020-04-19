using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace adaline
{
    public partial class Form1 : Form
    {
         public float[,] X = { { 1, 0 }, { 0, 1 } };

        public float[] B = { -1f, 1f }, 
            W = { 0.3f, 0.2f };
        // w-> ağırlıklar,X-> örnek kümeler, B->beklenenler 

        public float o = 0.1f; //eşik değeri
        public float a = 0.5f; // alpha 

        public bool x1icin = false, x2icin = false;


        void calistir()
        {

            txt.Text = "";
            //degerleriAl();

            int cikti;
            float e = 0;
            bool boz = true,
                x1t = false,
                x2t = false;
            int cik = 0;

            while (boz)
            {
                for (int i = 0; i < 2; i++)
                {
                    txt.Text += "\r\n";

                    txt.Text += "alınan değerler= W[0]=" + W[0] + "   X[j, 0]=" + X[i, 0] + "  W[1]=" + W[1] + "  X[j, 1]=" + X[i, 1] + "  o=" + o;
                    float net = (W[0] * X[i, 0] + W[1] * X[i, 1]) + o;
                    txt.Text += "  net değer= " + net.ToString();

                    if (net >= 0) { cikti = 1; }
                    else { cikti = -1; }

                    if (cikti != B[i]) // ağırlıklar yeniden hesaplanmalıdır
                    {
                        e = B[i] - cikti;
                        txt.Text += "\r\n e değeri= " + e;
                        txt.Text += "\r\n";

                        W[0] = W[0] + a * e * X[i, 0];
                        W[1] = W[1] + a * e * X[i, 1];
                        txt.Text += " W[0]=" + W[0] + "  W[1]=" + W[1];
                        txt.Text += "\r\n";

                        o = o + a * e;
                        txt.Text += " o= " + o;
                        txt.Text += "\r\n";

                    }

                    if(cikti==B[i])
                    {
                        txt.Text += "\r\n";
                        txt.Text += "Ağırlıklarda bir değişiklik yapılmadı.";
                        txt.Text += "\r\n";
                        if (i == 0) { x1t = true; }
                        if (i == 1) { x2t = true; }
                    }
                    if(x1t && x2t) { boz = false; }
                }


            }

        }

        void degerleriAl()
        {
            X[0, 0] = float.Parse(x00.Text);
            X[0, 1] = float.Parse(x01.Text);
            X[1, 0] = float.Parse(x10.Text);
            X[1, 1] = float.Parse(x11.Text);

            B[0] = float.Parse(b1.Text);
            B[1] = float.Parse(b2.Text);

            W[0] = float.Parse(w1.Text);
            W[1] = float.Parse(w2.Text);

            o = float.Parse(txt_esikDegeri.Text);

        }

        void degerleriYaz()
        {
            sx00.Text = X[0, 0].ToString();
            sx01.Text = X[0, 1].ToString();
            sx10.Text = X[1, 0].ToString();
            sx11.Text = X[1, 1].ToString();

            sb1.Text = B[0].ToString();
            sb2.Text = B[1].ToString();

            sw1.Text = W[0].ToString();
            sw2.Text = W[1].ToString();

            so1.Text = o.ToString();

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            calistir();

        }

        private void btn_Click(object sender, EventArgs e)
        {
            calistir();
            degerleriYaz();

        }
    }
}
