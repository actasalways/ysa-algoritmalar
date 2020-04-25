using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cok_katmanli_ysa_xor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private float ogrenmeKatsayisi = 0.9f;
        private float momentum = 0.5f;

        float[] noronlar = new float[10];
        float[] deltalar = new float[10];
        float[,] agirlik = new float[5, 5];
        float[] ağirlikBeklenen = new float[10];
        float[,] degisim = new float[5, 5];
        float[] degisimBeklenen = new float[10];

        private Random rand = new Random();

        private float rastgeleSayi()
        {
            return (float)rand.Next(1, 999) / 1000.0f; // 0.001 ve 0.999 arasında rastgele sayılar.
        }

        static float sigm(double value)//Sigmoid fonksiyonu
        {
            return 1.0f / (1.0f + (float)Math.Exp(-value));
        }

        void txtYaz()
        {
            _d31.Text = agirlik[0, 0].ToString();
            _d32.Text = agirlik[0, 1].ToString();
            _d3b.Text = ağirlikBeklenen[0].ToString();
            _d41.Text = agirlik[1, 0].ToString();
            _d42.Text = agirlik[1, 1].ToString();
            _d4b.Text = ağirlikBeklenen[1].ToString();
            _d53.Text = agirlik[2, 0].ToString();
            _d54.Text = agirlik[2, 1].ToString();
            _d5b.Text = degisimBeklenen[2].ToString();
        }

        void agiKur()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    agirlik[i, j] = rastgeleSayi();
                    ağirlikBeklenen[j] = rastgeleSayi();

                    degisim[i, j] = 0;
                    degisimBeklenen[i] = 0;
                }
            }

            txtYaz();

        }

        float karelerOrtalamasi(int hedef)
        {
            return (float)Math.Pow(hedef - noronlar[4], 2) / 2.0f; //1 çıkış var.
        }

        void egit(int giris1, int giris2, int cikis) //Farkların hesaplanması ve geri yayılım
        {
            cikisHesapla(giris1, giris2);
            Application.DoEvents();
            deltalar[2] = (cikis - noronlar[4]);
            deltalar[2] *= noronlar[4] * (1 - noronlar[4]); // işlem tekrarı yapmamak için
            deltalar[0] = agirlik[2, 0] * deltalar[2];
            deltalar[0] *= noronlar[2] * (1 - noronlar[2]);
            deltalar[1] = agirlik[2, 1] * deltalar[2];
            deltalar[1] *= noronlar[3] * (1 - noronlar[3]);

            for (int i = 0; i < 3; i++) //katman
            {
                for (int j = 0; j < 2; j++) //nöron 
                {
                    degisim[i, j] = ogrenmeKatsayisi * deltalar[i] * noronlar[i] + momentum * degisim[i, j];
                    agirlik[i, j] += degisim[i, j];
                }
                degisimBeklenen[i] = ogrenmeKatsayisi * deltalar[i] * 1 + momentum * degisimBeklenen[i];
                ağirlikBeklenen[i] += degisimBeklenen[i];
            }

            _d31.Text = agirlik[0, 0].ToString();
            _d32.Text = agirlik[0, 1].ToString();
            _d3b.Text = ağirlikBeklenen[0].ToString();
            _d41.Text = agirlik[1, 0].ToString();
            _d42.Text = agirlik[1, 1].ToString();
            _d4b.Text = ağirlikBeklenen[1].ToString();
            _d53.Text = agirlik[2, 0].ToString();
            _d54.Text = agirlik[2, 1].ToString();
            _d5b.Text = degisimBeklenen[2].ToString();

        }

        float cikisHesapla(int giris1, int giris2) // İleri besleme
        {
            noronlar[0] = (float)giris1;
            noronlar[1] = (float)giris2;

            for (int i = 0; i < 3; i++) // noronlar[2-3-4]
            {
                noronlar[i + 2] = sigm(noronlar[i] * agirlik[i, 0] + noronlar[i + 1] * agirlik[i, 1] + 1 * ağirlikBeklenen[i]);
            }

            lblN1.Text = noronlar[0].ToString();
            lblN2.Text = noronlar[1].ToString();
            lblN3.Text = noronlar[2].ToString();
            lblN4.Text = noronlar[3].ToString();
            lblN5.Text = noronlar[4].ToString();

            return noronlar[4];
        }

        void hataHesapla() // hatalar - tüm çıkışlar için 
        {
            cikisHesapla(0, 0);
            lblKO1.Text = karelerOrtalamasi(0).ToString();
            cikisHesapla(0, 1);
            lblKO2.Text = karelerOrtalamasi(1).ToString();
            cikisHesapla(1, 0);
            lblKO3.Text = karelerOrtalamasi(1).ToString();
            cikisHesapla(1, 1);
            lblKO4.Text = karelerOrtalamasi(0).ToString();

        }

        bool degerleriAl(ref int g1, ref int g2, ref int c)
        {
            g1 = Convert.ToInt32(txtGiris1.Text);
            g2 = Convert.ToInt32(txtGiris2.Text);
            c = Convert.ToInt32(txtCikis.Text);
            if ((g1 != 0 && g1 != 1) || (g2 != 0 && g2 != 1) || (c != 0 && c != 1)) return false;
            return true;

        }

        private void btn_egit_Click(object sender, EventArgs e)
        {
            int kac = (int)kacDefa.Value;
            for (int i = 0; i < kac; i++)
            {
                kacDefa.Value--;
                egit(0, 0, 0);
                lblKO1.Text = karelerOrtalamasi(0).ToString();
                egit(0, 1, 1);
                lblKO2.Text = karelerOrtalamasi(1).ToString();
                egit(1, 0, 1);
                lblKO3.Text = karelerOrtalamasi(1).ToString();
                egit(1, 1, 0);
                lblKO4.Text = karelerOrtalamasi(0).ToString();
            }
            kacDefa.Value = kac;

            hataHesapla();

        }

        private void btn_adim_Click(object sender, EventArgs e)// Bir tek giriş için eğit
        {
            int g1 = 0, g2 = 0, c = 0;
            if (!degerleriAl(ref g1, ref g2, ref c)) return;

            egit(g1, g2, c);
            if (g1 == 0 && g2 == 0) lblKO1.Text = karelerOrtalamasi(c).ToString();
            else if (g1 == 0) lblKO2.Text = karelerOrtalamasi(c).ToString();
            else if (g2 == 0) lblKO3.Text = karelerOrtalamasi(c).ToString();
            else lblKO4.Text = karelerOrtalamasi(c).ToString();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            agiKur();
        }

        private void btn_agiKur_Click(object sender, EventArgs e)
        {
            agiKur();
        }



    }
}
