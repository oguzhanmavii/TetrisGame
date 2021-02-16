using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Tetris alanadı içerisinde
namespace Tetris
{   
    // Alan isimli bir sınıf oluşturduk
    public static class Alan
    {
        // Sadece Alan sınıfı içerisinde kullanılacak değişkenler

        // suankiSekil nesnesi, Sekil sınıfından türetilmiştir
        public static Sekil suankiSekil;
        // Her bir karenin pixel büyüklüğü 25'e ayarlanmıştır
        public static int boyut = 25;
        // Varsayılan olarak 16 satır, 8 sütun şeklinde, 25er pixellik bir alan tanımlanmıştır
        public static int[,] map = new int[16, 8];
        // yapılan tetris sayısı bu sınıf içine alınmıştır
        public static int tetrisSayi;
        // yapılan skor sayısı bu sınıf içine alınmıştır
        public static int skor;
        // Interval update() metodu 300ms olarak belirtmiştik, o değer bu değişkendir
        public static int Interval;

        // bir sonraki şekli tanımlamak ve yana çizdirmek için kullandığımız fonksiyon
        public static void sonrakiSekil(Graphics e)
        {
            // Bizim tüm şekillerimiz Sekil sınıfı içerisinde matrisler halinde öntanımlı olarak vardır
            // Bunların açıklamalarını Sekil sınıfı içerisinde bulabilirsiniz

            // Rastgele seçilmiş şeklin 2-boyutlu matrisinin satır ve sütunlarındaki değerleri
            // içiçe 2 döngü halinde döndürüyoruz
            for (int i = 0; i < suankiSekil.boyutSonrakiMatrix; i++)
            {
                for (int j = 0; j < suankiSekil.boyutSonrakiMatrix; j++)
                {
                    // Ve matriste o an aldığınız [x,y] burada [i,j] konumlarındaki değeri çekiyoruz
                    // ve şeklin bulunduğu o an ki koordinatlara, her ızgara 25x25 olduğu için 24x24 bir kare çiziyoruz
                    // 1,2,3,4,5 değerleri ise içine doldurulacak renkleri tanımlar
                    // 1 : Kırmızı
                    // 2 : Sarı
                    // 3 : Yeşil
                    // 4 : Mavi
                    // 5 : Mor
                    // FillRectangle() metodu, verilen koordinatları, verilen renk ile doldurmaya yarar

                    // Tabii burada sonraki şekli çizdireceğimiz için
                    // ilk X değerimiz +220'lik bir ek değer alır, ve alan dışına sağa doğru çizdirilir.
                    if (suankiSekil.sonrakiMatrix[i, j] == 1)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(220 + j * (boyut) + 1, 50 + i * (boyut) + 1, boyut - 1, boyut - 1));
                    }
                    if (suankiSekil.sonrakiMatrix[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Yellow, new Rectangle(220 + j * (boyut) + 1, 50 + i * (boyut) + 1, boyut - 1, boyut - 1));
                    }
                    if (suankiSekil.sonrakiMatrix[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(220 + j * (boyut) + 1, 50 + i * (boyut) + 1, boyut - 1, boyut - 1));
                    }
                    if (suankiSekil.sonrakiMatrix[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Blue, new Rectangle(220 + j * (boyut) + 1, 50 + i * (boyut) + 1, boyut - 1, boyut - 1));
                    }
                    if (suankiSekil.sonrakiMatrix[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(220 + j * (boyut) + 1, 50 + i * (boyut) + 1, boyut - 1, boyut - 1));
                    }
                }
            }
        }

        // Tüm karelerdeki şekilleri ve renkleri temizler
        public static void AlanTemizle()
        {
            // [16,8] alan içiçe 2 döngü ile taranır
            // ve her karenin değeri '0' a eşitlenir
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    map[i, j] = 0;
                }
            }
        }

        // Tüm alanı tarayıp gerekli şekilleri yerleştiren metod
        public static void AlanCiz(Graphics e)
        {
            // [16,8] alan içiçe 2 döngü ile taranır
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // Ve matriste o an aldığınız [x,y] burada [i,j] konumlarındaki değeri çekiyoruz
                    // ve şeklin bulunduğu o an ki koordinatlara, her ızgara 25x25 olduğu için 24x24 bir kare çiziyoruz
                    // 1,2,3,4,5 değerleri ise içine doldurulacak renkleri tanımlar
                    // 1 : Kırmızı
                    // 2 : Sarı
                    // 3 : Yeşil
                    // 4 : Mavi
                    // 5 : Mor
                    // FillRectangle() metodu, verilen koordinatları, verilen renk ile doldurmaya yarar
                    if (map[i, j] == 1)
                    {
                        e.FillRectangle(Brushes.Red, new Rectangle(0 + j * (boyut) + 1, 0 + i * (boyut) + 1, boyut - 1, boyut - 1));
                    }
                    if (map[i, j] == 2)
                    {
                        e.FillRectangle(Brushes.Yellow, new Rectangle(0 + j * (boyut) + 1, 0 + i * (boyut) + 1, boyut - 1, boyut - 1));
                    }
                    if (map[i, j] == 3)
                    {
                        e.FillRectangle(Brushes.Green, new Rectangle(0 + j * (boyut) + 1, 0 + i * (boyut) + 1, boyut - 1, boyut - 1));
                    }
                    if (map[i, j] == 4)
                    {
                        e.FillRectangle(Brushes.Blue, new Rectangle(0 + j * (boyut) + 1, 0 + i * (boyut) + 1, boyut - 1, boyut - 1));
                    }
                    if (map[i, j] == 5)
                    {
                        e.FillRectangle(Brushes.Purple, new Rectangle(0 + j * (boyut) + 1, 0 + i * (boyut) + 1, boyut - 1, boyut - 1));
                    }
                }
            }
        }

        // Izgaraları çizdirdiğimiz fonksiyon
        public static void IzgaraCiz(Graphics g)
        {
            // 16 satırın alt ve üstü taranır
            for (int i = 0; i <= 16; i++)
            {
                // Siyah bir çizgi çizdirilir
                g.DrawLine(Pens.Black, new Point(0, 0 + i * boyut), new Point(0 + 8 * boyut, 0 + i * boyut));
            }

            // 8 sütunun sağ ve solu taranır
            for (int i = 0; i <= 8; i++)
            {
                // Siyah bir çizgi çizdirilir
                g.DrawLine(Pens.Black, new Point(0 + i * boyut, 0), new Point(0 + i * boyut, 0 + 16 * boyut));
            }
        }

        // Tetris oldu mu diye kontrol eden fonksiyon
        public static void TetrisKontrol()
        {
            // sayaç tanımlanır
            int count = 0;
            // Tetris yapılan satır değişkeni tanımlanır
            int curRemovedLines = 0;

            // Satırın tamamı taranır
            for (int i = 0; i < 16; i++)
            {
                count = 0;
                // sadece belirlenen satırdaki sağa doğru tarama yapılır
                for (int j = 0; j < 8; j++)
                {
                    // Eğer alan matrisinde taradığımızda karenin değeri '0' değilse,
                    // yani burada bir şekil ya da şekle ait bir parça bulunuyorsa
                    if (map[i, j] != 0)
                        // sayaç 1 arttırılır
                        count++;
                }

                // Sayaç 8'e tamamlanırsa, bu satırın tamamı dolu demektir
                // yani tetris olacaktır
                if (count == 8)
                {
                    // Tetris yapılan satır değişkenine 1 eklenir
                    curRemovedLines++;
                    
                    // ve o satırın tamamı baştan taranarak
                    // tüm değerler '0'a ayarlanır
                    for (int k = i; k >= 1; k--)
                    {
                        for (int o = 0; o < 8; o++)
                        {
                            map[k, o] = map[k - 1, o];
                        }
                    }
                }
            }

            // Bu döngü Tetris yapılan satır kadar tekrarlanır
            // ve her tekrarlamada 100 puan eklenir
            // yapılanTetris * 100 yani
            for (int i = 0; i < curRemovedLines; i++)
            {
                skor += 100;
            }

            // Tetris sayısı bu sınıf içerisindeki tetrisSayısı değişkenine eklenir
            tetrisSayi += curRemovedLines;
        }

        // Alan içerisinde aşağı düşmekte olan şeklin etrafındaki karelerde şekil var mı diye kontrol eden metod
        public static bool Kesisim()
        {
            // düşmekte olan şeklin -/+1 etrafındaki kareler taranır
            for (int i = suankiSekil.y; i < suankiSekil.y + suankiSekil.boyutMatrix; i++)
            {
                for (int j = suankiSekil.x; j < suankiSekil.x + suankiSekil.boyutMatrix; j++)
                {
                    // Eğer '0' değeri dışında bir değer içeriyorsa 'true' döndürür
                    // bu değer ile şeklin hareketini kısıtlamak gibi işler yapabiliriz
                    if (j >= 0 && j <= 7)
                    {
                        if (map[i, j] != 0 && suankiSekil.matrix[i - suankiSekil.y, j - suankiSekil.x] == 0)
                            return true;
                    }
                }
            }
            // '0' değerini içeriyorsa 'false' döndürür
            // bu sayede şeklin hareketi kısıtlanmaz
            return false;
        }

        // Şekli haritaya kalıcı ve hareketsiz olarak yerleştiren fonksiyon
        public static void Birlestir()
        {
            // Şeklin bulunduğu kareleri tarıyoruz
            for (int i = suankiSekil.y; i < suankiSekil.y + suankiSekil.boyutMatrix; i++)
            {
                for (int j = suankiSekil.x; j < suankiSekil.x + suankiSekil.boyutMatrix; j++)
                {
                    // Bulunduğumuz kareler normalde Alan içerisinde '0' değerindeyse
                    if (suankiSekil.matrix[i - suankiSekil.y, j - suankiSekil.x] != 0)
                        // tam olarak aynı karelere bizim şeklimizin renk değerine eşitliyoruz
                        map[i, j] = suankiSekil.matrix[i - suankiSekil.y, j - suankiSekil.x];
                }
            }
        }

        // Çarpışma kontrolü
        public static bool Carpisma()
        {
            // Çarpışma kontrolü ve işlemleri, Kesisim() metodu ile tamamen aynı şekilde çalışır
            for (int i = suankiSekil.y + suankiSekil.boyutMatrix - 1; i >= suankiSekil.y; i--)
            {
                for (int j = suankiSekil.x; j < suankiSekil.x + suankiSekil.boyutMatrix; j++)
                {
                    // Sadece aşağıya doğru yani y ekseninde +1. koordinat kontrol edilir
                    if (suankiSekil.matrix[i - suankiSekil.y, j - suankiSekil.x] != 0)
                    {
                        // alt kısım dolu ise
                        // çarpışma vardır
                        // 'true' değeri döndürür
                        if (i + 1 == 16)
                            return true;
                        if (map[i + 1, j] != 0)
                        {
                            return true;
                        }
                    }
                }
            }
            // Eğer 'true' değeri dönmediyse, çarpışma yoktur ve 'false' değeri döner
            return false;
        }

        // Sağ ve sol çarpışma kontrolü
        public static bool CarpismaYatay(int dir)
        {
            // yine şeklimizin tüm satır ve sütunları taranır
            for (int i = suankiSekil.y; i < suankiSekil.y + suankiSekil.boyutMatrix; i++)
            {
                for (int j = suankiSekil.x; j < suankiSekil.x + suankiSekil.boyutMatrix; j++)
                {
                    if (suankiSekil.matrix[i - suankiSekil.y, j - suankiSekil.x] != 0)
                    {
                        // Eğer şeklinde sağında veya solunda '0' dışında dolu bir kare var ise
                        // bunun kontrolü yapılır
                        // dir değişkeni x ekseninde (-)1 yani sol yön ya da (+)1 yani sağ yönde kontrol yapar
                        // 'true' dönmesi durumunda belirtline dir yönünde '0' dışında bir değişken
                        // yani dolu bir kare vardır
                        if (j + 1 * dir > 7 || j + 1 * dir < 0)
                            return true;

                        if (map[i, j + 1 * dir] != 0)
                        {
                            if (j - suankiSekil.x + 1 * dir >= suankiSekil.boyutMatrix || j - suankiSekil.x + 1 * dir < 0)
                            {
                                return true;
                            }
                            if (suankiSekil.matrix[i - suankiSekil.y, j - suankiSekil.x + 1 * dir] == 0)
                                return true;
                        }
                    }
                }
            }
            // aksi durumda kare yoktur, '0'dır yani 'false' döner
            return false;
        }

        // Şekil aşağı hareket ettiği her adımda bu metod çalışır
        public static void AlanSifirla()
        {
            for (int i = suankiSekil.y; i < suankiSekil.y + suankiSekil.boyutMatrix; i++)
            {
                for (int j = suankiSekil.x; j < suankiSekil.x + suankiSekil.boyutMatrix; j++)
                {
                    // Şekil aşağı hareket etti ise
                    if (i >= 0 && j >= 0 && i < 16 && j < 8)
                    {
                        // ve aşağı hareket ettikten sonra geride kalan kareler '0' değil ise
                        if (suankiSekil.matrix[i - suankiSekil.y, j - suankiSekil.x] != 0)
                        {
                            // geride kalan kareleri sıfır yapalım ki
                            // görüntü bozuk ve anlaşılmaz hale gelmesin
                            // görsel olarak şeklin aşağı yönde hareket ettiği görülsün
                            map[i, j] = 0;
                        }
                    }
                }
            }
        }

    }
}
