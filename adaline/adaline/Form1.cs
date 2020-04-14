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
        public float[] B = { -1f, 1f }, W = { 0,3f, 0,2f };
        // w-> ağırlıklar,X-> örnek kümeler, B->beklenenler 

        public float o = 0.1f; //eşik değeri
        public float a = 0.5f; // alpha 

        public bool x1icin = false, x2icin = false;

        void iterasyon()
        {
            txt.Text = "";
            int iterasyon = 1;
            bool devam = true;

            while (devam)
            {
                for (int j = 0; j < 2; j++)
                {
                    float net = W[0] * X[j, 0] + W[1] * X[j, 1] + o ;
                    txt.Text += "\r\n ";
                    txt.Text += iterasyon + ". iterasyon";
                    iterasyon++;

                    if (net >= 0)// çıkış 1dir
                    {
                        int cikis = 1;
                        if (cikis != B[j])//ağırlık-eşik değeri yeniden hesaplanır 
                        {
                            float e = B[j] - cikis;
                            W[0] = W[0] + (a * e * X[j, 0]) ;
                            W[1] = W[1] + (a * e * X[j, 1]) ;
                            o = o + a * e;
                            txt.Text += "\r\n Net Değeri = " + net.ToString();
                            txt.Text += "\r\n Yeni Ağırlıklar = " + W[0].ToString() + "," + W[1].ToString();
                            txt.Text += "\r\n Yeni Eşik Değeri = " + o.ToString(); ;
                            //txt.Text += "\r\n Fark  = " + e.ToString(); ;
                        }
                        else // çıkış değeri beklenene eşittir -> çık
                        {
                            txt.Text += "\r\n ağırlıklar değişmedi. ";
                            x1icin = true;
                        }
                    }
                    else
                    {
                        int cikis = -1;
                        if (cikis != B[j])//eşik değeri yeniden hesaplanır 
                        {
                            float e = B[j] - cikis;
                            W[0] = W[0] + a * e * X[j, 0];
                            W[1] = W[1] + a * e * X[j, 1];
                            o = o + a * e;
                            txt.Text += "\r\n Net Değeri = " + net.ToString();
                            txt.Text += "\r\n Yeni Ağırlıklar = " + W[0].ToString() + "," + W[1].ToString();
                            txt.Text += "\r\n Yeni Eşik Değeri = " + o.ToString(); ;

                        }
                        else // çıkış değeri beklenene eşittir -> çık
                        {
                            txt.Text += "\r\n ağırlıklar değişmedi. ";
                            x2icin = true;
                        }

                    }

                }
                if (x1icin && x2icin)
                    devam = false;
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

        }

        private void btn_Click(object sender, EventArgs e)
        {
            //degerleriAl();
            iterasyon();
            degerleriYaz();
        }
    }
}
