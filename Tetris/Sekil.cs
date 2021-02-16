using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Tetris alanadında
namespace Tetris
{
    // Sekil sınıfını oluşturduk
    public class Sekil
    {
        // Sadece bu sınıfta kullanılacak değişkenler

        // Şeklin x koordinatı
        public int x;
        // Şeklin y koordinatı
        public int y;
        // Şeklin matrisi
        public int[,] matrix;
        // Sonraki şeklin matrisi
        public int[,] sonrakiMatrix;
        // Şeklin boyutunun matrisi
        public int boyutMatrix;
        // Sonraki şeklin boyutunun matrisi
        public int boyutSonrakiMatrix;

        // Burada şekiller ve renkler tanımlanıyor
        // 1 : Kırmızı
        // 2 : Sarı
        // 3 : Yeşil
        // 4 : Mavi
        // 5 : Mor
        // Şekillerin tanımlanması ise kare olan matrisler ile sağlanmış
        // ve şeklin max sınırları kadar bir kare çizilmiş

        // Dikkat ederseniz matrislerin içi oyunda çizilen
        // şekillerle doldurulmuş ve kodlardan bakınca da anlaşılıyor
        // içine girdiğimiz değerler renklerini tanımlar

        public int[,] tetr1 = new int[4, 4]{
            {0,0,1,0  },
            {0,0,1,0  },
            {0,0,1,0  },
            {0,0,1,0  },
        };

        public int[,] tetr2 = new int[3, 3]{
            {0,2,0  },
            {0,2,2 },
            {0,0,2 },
        };

        public int[,] tetr3 = new int[3, 3]{
            {0,0,0  },
            {3,3,3 },
            {0,3,0 },
        };

        public int[,] tetr4 = new int[3, 3]{
            { 4,0,0  },
            {4,0,0 },
            {4,4,0 },
        };
        public int[,] tetr5 = new int[2, 2]{
            { 5,5  },
            {5,5 },
        };

        // Sekil sınıfının oluşturma fonksiyonu
        public Sekil(int _x,int _y)
        {
            // tanımlanan x, y değerlerinde oluşturuluyor
            x = _x;
            y = _y;
            // Rastgele bir şekil matrisi tanımlanıyor
            matrix = GenerateMatrix();
            // matrisin max sınır boyutu tespit edilmesi için
            // bu matrisin karekökünü alıyoruz
            // şekil [4,4] boyutunda ise karekökü 4'tür.
            // ve matrisin boyutu da 4 olacaktır
            boyutMatrix = (int)Math.Sqrt(matrix.Length);
            // aynı zamanda sonraki şekil de rastgele olarak atanır
            sonrakiMatrix = GenerateMatrix();
            // ve aynı boyut alma işlemi bu matriste de yapılır
            boyutSonrakiMatrix = (int)Math.Sqrt(sonrakiMatrix.Length);
        }

        // Bu fonksiyon şuan ki şeklin yerine sonraki şekli koyar
        public void SekilSifirla(int _x, int _y)
        {
            // yine yeni x ve y değerleri alınır
            x = _x;
            y = _y;
            // yeni şekil bir sonraki olarak tanımlanan şekil olur
            matrix = sonrakiMatrix;
            // boyutu hesaplanır
            boyutMatrix = (int)Math.Sqrt(matrix.Length);
            // sonraki şekil için yeni şekil tanımlanır
            sonrakiMatrix = GenerateMatrix();
            // boyutu hesaplanır
            boyutSonrakiMatrix = (int)Math.Sqrt(sonrakiMatrix.Length);
        }

        // Matris oluşturan fonksiyon
        public int[,] GenerateMatrix()
        {
            // yukarıda tanımlanan şekil matrisleri buradan rastgele olarak
            // tanımlanır ve döndürülür
            int[,] _matrix = tetr1;
            
            // Rastgele değer almak için
            // Random nesnesi tanımlıyoruz
            Random r = new Random();

            // r.Next(sayi1,sayi2) metodu sayi1 dahil, sayi6ya kadar, sayi6 dahil değil
            // rastgele tam sayı değeri verir
            // ve değerler ile yukarıda öntanımlı
            // şekil matrisleri eşleştirilir
            switch (r.Next(1, 6))
            {
                case 1:
                    _matrix = tetr1;
                    break;
                case 2:
                    _matrix = tetr2;
                    break;
                case 3:
                    _matrix = tetr3;
                    break;
                case 4:
                    _matrix = tetr4;
                    break;
                case 5:
                    _matrix = tetr5;
                    break;
            }

            // Sonuç olarak şekil matrisi döndürülür
            return _matrix;
        }

        // İleri yön tuşuna basılınca çağırılan metod
        // Bu matris şekli çevirir
        public void SekilCevir()
        {
            // Şuan ki şeklimizin boyutunda geçici bir matris oluşturur
            int[,] tempMatrix = new int[boyutMatrix,boyutMatrix];

            // geçici matrisin tüm elemanlarını tarar
            for(int i = 0; i < boyutMatrix; i++)
            {
                for (int j = 0; j < boyutMatrix; j++)
                {
                    // geçici matrisin tüm elemanlarını
                    // önceki matristen aldığı elemanların döndürülmüş koordinatlarına eşitler
                    // yani bu döngü tamamlanınca bu matris
                    // önceki matrisin döndürülmüş hali olur
                    tempMatrix[i, j] = matrix[j, (boyutMatrix - 1) - i];
                }
            }

            // Matrisimiz artık döndürülmüş bir matristir
            matrix = tempMatrix;

            // Matrisin alandan taşıp taşmadığına bakılır
            int offset1 = (8 - (x + boyutMatrix));

            /// Eğer sağ tarafı dolu olduğu için dışarı taşıyorsa
            if (offset1 < 0)
            {
                // Taştığı kadar Sola() metodu çağırılır
                // ve taştığı kadar sola gider
                for (int i = 0; i < Math.Abs(offset1); i++)
                    Sola();
            }
            
            // Eğer sol tarafı dolu ise
            if (x < 0)
            {
                // Taştığı kadar Saga() metodu çağırılır
                // ve taştığı kadar sağa gider
                for (int i = 0; i < Math.Abs(x)+1; i++)
                    Saga();
            }

        }

        // Geri yön tuşuna basılınca çağırılan metod
        // y ekseninde +1 değer alır ve şekli aşağı kaydırır
        public void Asagi()
        {
            y++;
        }

        // Sağ yön tuşuna basılınca çağırılan metod
        // x ekseninde +1 değer alır ve şekli saga kaydırır
        public void Saga()
        {
            x++;
        }

        // Sol yön tuşuna basılınca çağırılan metod
        // x ekseninde -1 değer alır ve şekli sola kaydırır
        public void Sola()
        {
            x--;
        }
    }
}
