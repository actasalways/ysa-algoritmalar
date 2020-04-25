using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cok_katmanli_ag_modeli
{
    public partial class Form1 : Form
    {
        int epoch = 5;
        float esikDegeri;
        float momentum = 0.8f;
        private float ogrenmeKatsayisi = 0.9f;


        float[] girdiler; // giriş katman, ara katman
        float[,] agirliklar; //dentritler 3-4 
        float[] agirliklarBeklenen;
        float[] beklenenler;
        float[] beklenenlerDegisim;
        float[] cikislar; // dendrit5
        float[] cikislarDegisim;
        float[] noronlar; // ilk 2 giriş, 2 ara, son çıkış nöron 
        float[] deltalar;
        float[] degisimler;

        //genelleştirilmiş delta kuralı -> iki aşamadan oluşur 
        // ileri doğru hesaplama
        // geri doğru hesaplama 

        /*
         * 1. adım -> bütün ağı rastgele sayılarla(sıfıra yakın ama sıfırdan farklı  0.001 - 0.999 arasında) ilişkilendir
         * eşik değerleri ve ağırlıkları da sıfıra yakın bir değerle 
         * 2. adım -> veri kümesinden ilk satır giriş katmanından verilir -> her öznitelik bir nöron
         * 3. adım -> ileri yönlü yayılım yapılır, ysa istenen sonucu verene kadar güncellenir. (epoch -> istenen sonuç çıkmadı = güncelle )
         * 4. adım -> gerçek ile çıktı arasındaki fark alınır ve hata hesaplanır. 
         * 5. adım -> geri yayılım (orospuluk) her sinaps üzerindeki ağırlık, hatadan sorumlu olduğu miktarda değiştirilir.
         *            değiştirilme miktarı öğrenme oranına da bağlıdır. 
         *            
         * 6. adım -> 1-5 arasındaki adımları istenen sonucu elde edene kadar güncelle 
         * 7. adım -> bütün eğitim kümesi çağırıldıktan sonra bir epoch tamamlanmış olur.
         *            aynı veri kümeleri kullanılarak epoch tekrarı yapılır 
         *            
         *            
         * 
         */


        //private void egit_Click(object sender, EventArgs e) // Bir tek giriş için eğit
        //{
        //    int g1 = 0, g2 = 0, c = 0;
        //    if (!degerleriAl(ref g1, ref g2, ref c)) return;

        //    egit(g1, g2, c);
        //    if (g1 == 0 && g2 == 0) lblKO1.Text = karelerOrtalamasi(c).ToString();
        //    else if (g1 == 0) lblKO2.Text = karelerOrtalamasi(c).ToString();
        //    else if (g2 == 0) lblKO3.Text = karelerOrtalamasi(c).ToString();
        //    else lblKO4.Text = karelerOrtalamasi(c).ToString();
        //}

        private Random rast = new Random();
        float rastgele()
        {
            return (float)rast.Next(1, 999) / 1000.0f; // 0.001 ve 0.999 arasında rastgele sayılar.
        }

        float sigmoid(float x) // Sigmoid fonksiyonu
        {
            return (float)(1.0f / (1.0f + Math.Exp(-x)));
        }

        float karelerOrtalamasi(int hedef)
        {
            return (float)Math.Pow(hedef - noronlar[4], 2) / 2.0f; // 1 çıkış var
        }

        void agiKur()
        {
            // rastgele değerler ata 
            //değişimleri sıfırla 
            // ilk satır giriş katmanı ver 
            for(int i = 0; i < 2; i++)
            {
                beklenenler[i] = rastgele();
                beklenenlerDegisim[i] = 0;
                cikislar[i] = rastgele();
                cikislarDegisim[i] = 0;

                for (int j = 0; j < 2; j++)
                {
                    agirliklar[i, j] = rastgele();
                    //agirliklarBeklenen[i, j] = 0;
                }
            }

        }

        private float cikisHesapla(int giris1, int giris2) // İleri besleme
        {
            noronlar[0] = (float)giris1;
            noronlar[1] = (float)giris1;
            lbl_noron1.Text = noronlar[0].ToString();
            lbl_noron2.Text = noronlar[1].ToString();

            noronlar[2] = sigmoid(noronlar[0] * agirliklar[0, 0] + noronlar[1] + agirliklar[0, 1] + 1 * beklenenler[0] );
            noronlar[3] = sigmoid(noronlar[0] * agirliklar[1, 0] + noronlar[1] + agirliklar[1, 1] + 1 * beklenenler[1] );
            lbl_noron3.Text = noronlar[2].ToString();
            lbl_noron4.Text = noronlar[3].ToString();

            noronlar[4] = sigmoid(noronlar[2] * cikislar[0] + noronlar[3] + cikislar[1] + 1 * beklenenler[2]);
            lbl_noron5.Text = noronlar[4].ToString();

            return noronlar[4];

        }

        private void egit(int giris1, int giris2, int cikis) //Farkların hesaplanması ve geri yayılım
        {
            cikisHesapla(giris1, giris2);

            deltalar[2] = (cikis - noronlar[4]);
            deltalar[2] *= noronlar[4] * (1 - noronlar[4]);

            deltalar[0] = cikislar[0] * deltalar[2];
            deltalar[0] *= noronlar[2] * (1 - noronlar[2]);
            deltalar[1] = cikislar[1] * deltalar[2];
            deltalar[1] *= noronlar[3] * (1 - noronlar[3]);



            // Ağırlık değişimlerini hesaplayıp değişimlere momentum katarak ağırlıkları güncelliyoruz.
            agirliklarBeklenen[0] = ogrenmeKatsayisi * deltalar[0] * noronlar[0] + momentum * agirliklarBeklenen[0];
            agirliklar[0,0] += agirliklarBeklenen[0];
           // _d31.Text = agirliklar[0,0].ToString();
            agirliklarBeklenen[1] = ogrenmeKatsayisi * deltalar[0] * noronlar[1] + momentum * agirliklarBeklenen[1];
            agirliklar[0,1] += agirliklarBeklenen[1];
            // _d32.Text = agirliklar[0,1].ToString();

            degisim3b = ogrenmeKatsayisi * delta3 * 1 + momentum * degisim3b;
            dendrit3b += degisim3b;
           // _d3b.Text = dendrit3b.ToString();
            degisim41 = ogrenmeKatsayisi * delta4 * noron1 + momentum * degisim41;
            dendrit41 += degisim41;
          //  _d41.Text = dendrit41.ToString();
            degisim42 = ogrenmeKatsayisi * delta4 * noron2 + momentum * degisim42;
            dendrit42 += degisim42;
          //  _d42.Text = dendrit42.ToString();
            degisim4b = ogrenmeKatsayisi * delta4 * 1 + momentum * degisim4b;
            dendrit4b += degisim4b;
          //  _d4b.Text = dendrit4b.ToString();

            degisim53 = ogrenmeKatsayisi * delta5 * noron3 + momentum * degisim53;
            dendrit53 += degisim53;
          //  _d53.Text = dendrit53.ToString();
            degisim54 = ogrenmeKatsayisi * delta5 * noron4 + momentum * degisim54;
            dendrit54 += degisim54;
          //  _d54.Text = dendrit54.ToString();
            degisim5b = ogrenmeKatsayisi * delta5 * 1 + momentum * degisim5b;
            dendrit5b += degisim5b;
         //   _d5b.Text = dendrit5b.ToString();
        }



        void ileriYon()
        {

        }

        void geriYonlu()
        {

        }

        void deltaHesapla()
        {

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
