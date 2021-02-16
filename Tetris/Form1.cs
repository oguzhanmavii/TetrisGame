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

namespace Tetris
{
    public partial class Form1 : Form
    {
        // kontrolTusu değişkeni : bastığımız tuşları atadığımız değişkendir
        private Keys kontrolTusu;
        // Oyun bittiğinde 'true' değerine dönen değişken.
        private bool oyunBitti = false;

        // Bu fonksiyon sayesinde yön tuşlarına basıldığında butonlar arasında geçiş
        // olayını engelliyoruz ve bu tuşları 'kontrolTusu' değişkenine atıyoruz
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // basılan tuşu 'kontrolTusu' değişkenine atama
            kontrolTusu = keyData;

            // Basılan tuşlar yön tuşları ise Form üzerinde herhangi bir
            // işlem yapılmasını engelledik ve sadece true verisini döndürdük,
            // ve bu veri bir tuş değeri olmadığı için tuşa basılmamış sayıldı
            if (!msg.HWnd.Equals(this.Handle) &&
                (keyData == Keys.Left || keyData == Keys.Right ||
                keyData == Keys.Up || keyData == Keys.Down))
            {
                return true;
            }
            
            // Eğer yön tuşları dışında bir tuşa basılırsa bunun işlemiş tabii ki
            // gerçekleşecektir, buna engel olmadık
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        public Form1()
        {
            // Form tasarımındaki komponentleri çağırdık
            InitializeComponent();

            // Oyunumuzu başlatan fonksiyon
            Init();
        }

        // Oyunumuzu başlatan fonksiyon
        public void Init()
        {
            // Skorumuzu sıfırladık, skor değişkenimiz Alan class'ı içerisinde tanımlanmıştır
            Alan.skor = 0;
            // Yapılan tetris sayısını da sıfırladık
            Alan.tetrisSayi = 0;
            // X = 3 ve Y = 0, konumunda yeni bir şekil oluşturduk
            Alan.suankiSekil = new Sekil(3, 0);
            // Oyun hızı 300ms olarak ayarlandı, tüm hareketler 300ms içinde gerçekleşir
            Alan.Interval = 300;

            // Skor ve Tetris sayısı değerlerini ekrana yazdırdık
            label1.Text = "Puan : " + Alan.skor;
            label2.Text = "Tetris : " + Alan.tetrisSayi;
        }
        
        // timer1 her çalıştığında yani normal durumda 300ms'de bir
        // bu fonksiyon çalışacaktır
        private void update(object sender, EventArgs e)
        {
            // Şekil her hareket ettiğinde önceki kareler eski haline gelmeli
            // ve şeklin bulunduğu kareler tekrar renklendirilmelidir
            // bu fonksiyon ile bu işlemi sağlıyoruz
            Alan.AlanSifirla();

            // Eğer şekil yer veya başka bir şekil ile çarpışmadıysa
            if (!Alan.Carpisma())
            {
                // Şekil aşağı yönde harekete devam eder
                Alan.suankiSekil.Asagi();
            }
            // Eğer şekil yer veya başka bir şekil ile çarpıştıysa
            else
            {
                // Şuanki şekli haritanın bütünü ile birleştir ve kaldığı konuma kaydet
                Alan.Birlestir();
                // Tetris yapılmış mı? diye kontrol et
                Alan.TetrisKontrol();
                // timer1 tekrar 300ms'e ayarlansın
                timer1.Interval = Alan.Interval;
                // Inıt() kısmında zaten şekli tanımlamıştık,
                // Şekli sıfırlıyoruz, rastgele yeni bir şekil atıyoruz
                // X = 3 ve Y = 0, konumlarına yerleştiriyoruz
                Alan.suankiSekil.SekilSifirla(3,0);

                // Eğer hala çarpışma var ise
                if (Alan.Carpisma())
                {
                    // timer1.Tick eventinden update() fonksiyonunu kaldır.
                    timer1.Tick -= new EventHandler(update);
                    // Bütün timerları durdur
                    timer1.Stop();
                    puanSay.Stop();
                    tusKontrol.Stop();

                    // 'kontrolTusu' değişkenine de ESC tuşunu atıyoruz ki, bir sonraki başlatma son bastığımız
                    // tuş çalışmasın
                    kontrolTusu = Keys.Escape;
                    // oyun bittiği için bu değişken true oluyor
                    oyunBitti = true;
                }
            }
            // Eğer hiç bir sıkıntı yoksa şeklimiz ve mevcut alan birleşir
            Alan.Birlestir();

            // OnPaint() eventi tekrar çalışmasın istiyoruz, bu yüzden Invalidate() metodunu çağırdık
            Invalidate();
        }

        // Arayüz ekrana ilk çizdirildiğinde bu event çalışmaktadır
        private void OnPaint(object sender, PaintEventArgs e)
        {
            // Öncelikle (16,8)'lik alan çalışır
            Alan.AlanCiz(e.Graphics);
            // Bu alana ızgaraları çizdiriyoruz
            Alan.IzgaraCiz(e.Graphics);
            // Ve bir sonraki şekli çizdir
            Alan.sonrakiSekil(e.Graphics);
        }

        // Bu Timer ile puanımız saniyede 1 artmaktadır
        // ayrıca labelların yazıları güncellenmektedir
        private void puanSay_Tick(object sender, EventArgs e)
        {
            Alan.skor += 1;
            label1.Text = "Puan : " + Alan.skor;
            label2.Text = "Tetris : " + Alan.tetrisSayi;
        }

        // Başlat butonuna bastığımızda buradaki olaylar gerçekleşir
        private void button1_Click(object sender, EventArgs e)
        {
            // Alan üzerindeki tüm şekiller temizlenir
            Alan.AlanTemizle();
            // timer1 tekrarlama değeri 300ms'e alınır
            timer1.Interval = Alan.Interval;
            // timer1.Tick eventine update() fonksiyonunu ekle.
            timer1.Tick += new EventHandler(update);
            // tüm timerlar çalışır
            timer1.Start();
            puanSay.Start();
            tusKontrol.Start();
            // Başlat butonu kapanır
            button1.Enabled = false;
            // Yenile butonu açılır
            button2.Enabled = true;
            // Oyunu başlatan fonksiyon tekrar çağırılır
            Init();
            // OnPaint() eventi tekrar çalışmasın istiyoruz, bu yüzden Invalidate() metodunu çağırdık
            Invalidate();
        }

        // Yenile butonuna bastığımızda buradaki olaylar gerçekleşir
        private void button2_Click(object sender, EventArgs e)
        {
            // timer1 tekrarlama değeri 300ms'e ayarlanır
            timer1.Interval = Alan.Interval;

            // Eğer oyun bitmişse, bu değişken true olacaktır
            if (oyunBitti)
            {
                // timer1_Tick eventine update() metodu tekrar eklenir
                timer1.Tick += new EventHandler(update);
                // bu değişken false olur ve bu sayede oyunun tekrar başladığı anlaşılabilir
                oyunBitti = false;
            }

            // Alan üzerindeki tüm şekiller temizlenir
            Alan.AlanTemizle();
            // Tüm timerlar başlatılır
            timer1.Start();
            puanSay.Start();
            tusKontrol.Start();
            // Oyunu başlatan fonksiyon tekrar çağırılır
            Init();
            // OnPaint() eventi tekrar çalışmasın istiyoruz, bu yüzden Invalidate() metodunu çağırdık
            Invalidate();
        }

        // Tuşların kontrol edildiği Timer, 100ms'de bir basılan tuşları kontrol eder
        private void tusKontrol_Tick(object sender, EventArgs e)
        {
            // Eğer oyun başladıysa yani Başlat butonu çalışmıyorsa
            if (button1.Enabled == false)
            {
                // kontrolTusu kontrol metoduna alınır
                switch (kontrolTusu)
                {
                    // İleri yön tuşuna basıldıysa
                    case Keys.Up:

                        // ve etrafında dönmesine engel olacak bir şekil yok ise
                        if (!Alan.Kesisim())
                        {
                            // Şekil her hareket ettiğinde önceki kareler eski haline gelmeli
                            // ve şeklin bulunduğu kareler tekrar renklendirilmelidir
                            // bu fonksiyon ile bu işlemi sağlıyoruz
                            Alan.AlanSifirla();
                            // Şekli döndürüyoruz
                            Alan.suankiSekil.SekilCevir();
                            // Mevcut harita ile şekli birleştiriyoruz
                            Alan.Birlestir();
                            // OnPaint() eventi tekrar çalışmasın istiyoruz, bu yüzden Invalidate() metodunu çağırdık
                            Invalidate();
                            // Tuş işlemini tamamladı artık bu işlemden çıkabiliriz
                            break;
                        }
                        break;

                    // Geri yön tuşuna basıldıyse
                    case Keys.Down:
                        // timer1 tekrarlama değeri 10ms olur,
                        // ve update() metodu 10ms'de 1 tekrarladığı için
                        // biz müdahale edemeden
                        // şekil hızlıca aşağıya düşer
                        timer1.Interval = 10;
                        break;

                    // Eğer sağ yön tuşuna basıldıysa
                    case Keys.Right:
                        // Şeklin sağında çarpacağı bir nesne yok ise
                        if (!Alan.CarpismaYatay(1))
                        {
                            // Şekil sağa gider
                            Alan.AlanSifirla();
                            Alan.suankiSekil.Saga();
                            Alan.Birlestir();
                            Invalidate();
                            break;
                        }
                        break;
                    case Keys.Left:
                        // Şeklin solunda çarpacağı bir nesne yok ise
                        if (!Alan.CarpismaYatay(-1))
                        {
                            // Şekil sola gider
                            Alan.AlanSifirla();
                            Alan.suankiSekil.Sola();
                            Alan.Birlestir();
                            Invalidate();
                            break;
                        }
                        break;
                }
                // 'kontrolTusu' değişkeni herhangi bir tuşa bir kere basıldıktan sonra
                // Esc tuşuna ayarlanır, bunun sebebi ise program tuşa basılı kalmış gibi davranmamalı
                kontrolTusu = Keys.Escape;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
