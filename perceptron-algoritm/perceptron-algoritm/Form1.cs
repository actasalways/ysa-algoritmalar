using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace perceptron_algoritm
{
    public partial class Form1 : Form
    {
        public float[,] X = { { 1, 0 }, { 0, 1 } };
        public float [] B= {1f,0f}, W= {1f,2f}; 
        // w-> ağırlıklar,X-> örnek kümeler, B->beklenenler 

        public float o = -1f; //eşik değeri
        public float i = 0.5f; //Lambda 

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            //i = float.Parse(txt_ogrenmeKatsayisi.Text);

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


            lbl.Text = o.ToString();
            //lbl_bitis.Text = i.ToString();

        }

        void iterasyon(float [,] X,float [] Wn,float [] B)
        {
            txt.Text = "";
            int iterasyon = 1;
            bool devam = true;
            while (devam)
            {
                for (int j = 0; j < 2; j++)
                {
                    float net = W[0] * X[j, 0] + W[1] * X[j, 1];
                    txt.Text += "\r\n ";
                    txt.Text += iterasyon + ". iterasyon";
                    iterasyon++;

                    if (net > o)// çıkış 1dir
                    {
                        int cikis = 1;
                        if (cikis != B[j]) // çıkış değeri beklenen değildir, ağırlık yeniden hesaplanır -> Wn = W0 - iX
                        {
                            W[0] = Wn[0] - i * X[j, 0];
                            W[1] = Wn[1] - i * X[j, 1];
                            txt.Text += "\r\n Net Değeri = " + net.ToString();
                            txt.Text += "\r\n Yeni Ağırlıklar = " + W[0].ToString() + "," + W[1].ToString();
                        }
                        else if (cikis == B[j]) { txt.Text += "\r\n ağırlıklar değişmedi. "; }

                    }
                    else // çıkış değeri beklenene eşittir -> çık
                    {
                        devam = false;
                    }

                }
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //degerleriAl(); 
            iterasyon(X, W, B);
            degerleriYaz();

        }



    }
}
