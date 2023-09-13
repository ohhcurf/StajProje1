using System.Windows.Forms;

namespace StajProje1
{
    public partial class Form1 : Form
    {
        static string dizin = Directory.GetCurrentDirectory(); 
        string dosyayolu = Path.Combine(dizin, "database.txt"); 
        string dosyayoluyeni = Path.Combine(dizin, "databaseyeni.txt"); 
        public static int satirSayisi = 0; 

        public Form1()
        {
            // Uygulama a��ld���nda database belgesi bulunamad�ysa yarat�r
            InitializeComponent();
            if (!File.Exists(dosyayolu))
            {
                File.WriteAllText(dosyayolu, "");
            }
        }





        // Veri Okuma
        private List<Ogrenci> VeriOku(int tane)
        {

            List<Ogrenci> ogrenciler = new List<Ogrenci>(); 
            int maksimumSatir = tane + satirSayisi; // Okunacak maksimum sat�r say�s� girilen parametreye e�itleniyor

            // maksimumSatir de�eri kadar sat�r okunup ��renciler listeye eklendikten sonra bu de�er return'lenir
            using (StreamReader sr = new StreamReader(dosyayolu))
            {
                string satir;
                List<string> satirlar = new List<string>();

                for (int i = 0; i < satirSayisi; i++)
                {
                    sr.ReadLine();
                }

                while ((satir = sr.ReadLine()) != null)
                {
                    string[] kesme = satir.Split(',');
                    Ogrenci newOgrenci = new Ogrenci
                    {
                        isim = kesme[0],
                        soyad = kesme[1],
                        numara = kesme[2],
                        gizli = Int32.Parse(kesme[3]),

                    };
                    ogrenciler.Add(newOgrenci);
                    satirlar.Add(satir);
                    satirSayisi++;

                    if (satirSayisi >= maksimumSatir)
                    {
                        satirlar.Clear();
                        return ogrenciler;
                    }
                }
                return ogrenciler;
            }
        }

        // Yazma
        private void VeriYaz(Ogrenci ogrenci)
        {
            // Yeni metin belgesine ogrenci eklenir
            using (StreamWriter writer = new StreamWriter(dosyayoluyeni, true))
            {
                writer.WriteLine(ogrenci.isim + "," + ogrenci.soyad + "," + ogrenci.numara + "," + ogrenci.gizli);
                writer.Close();
            }
        }

        // G�ncelleme
        private void VeriG�ncelle(Ogrenci ogrenci)
        {
            using (File.Create(dosyayoluyeni))
            {
                // Yeni metin belgesi olu�turulur
            }

            List<Ogrenci> okunanOgrenci = VeriOku(1); 
            while (okunanOgrenci.Count != 0) // Okuncak ��renci kalmayana kadar d�ng�
            {
                if (okunanOgrenci[0].numara != ogrenci.numara) 
                {
                    VeriYaz(okunanOgrenci[0]);
                }

                okunanOgrenci = VeriOku(1); // S�radaki ��renciyi oku
            }
            satirSayisi = 0;

            if (File.Exists(dosyayolu))
            {
                using (FileStream fs = File.Open(dosyayolu, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    // Dosyay� kapat
                    fs.Close();
                }
            }

            File.Delete(dosyayolu); // Eski metin belgesini sil
            File.Move(dosyayoluyeni, dosyayolu); // Yeni metin belgesini yeniden adland�r

            ekleFunc(ogrenci);
        }





        //Ekleme D��mesi
        private void ekleButton_Click(object sender, EventArgs e)
        {
            //kontrol etme
            if (isimBox.Text == "")
            {
                MessageBox.Show("isim giriniz.");
                return;
            }
            if (soyadBox.Text == "")
            {
                MessageBox.Show("soyad giriniz.");
                return;
            }
            if (numaraBox.Text == "")
            {
                MessageBox.Show("numara giriniz.");
                return;
            }
            //numara say� m� kontrol�
            try
            {
                int sayi = Int32.Parse(numaraBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Numara k�sm�na say� giriniz.");
                return;
            }
            //Yeni ��renci olu�turma
            Ogrenci newOgrenci = new Ogrenci
            {
                isim = isimBox.Text,
                soyad = soyadBox.Text,
                numara = numaraBox.Text,
                gizli = 1,
            };
            ekleFunc(newOgrenci);
            listeleFunc();
        }

        //Ekleme Fonksiyonu (alfabetik s�rayla ekleme yapar)
        private void ekleFunc(Ogrenci ogrenci)
        {
            var eklendi = false;
            using (File.Create(dosyayoluyeni))
            {
                // Yeni metin belgesi olu�turulur
            }

            List<Ogrenci> ogrenciler = new List<Ogrenci>();
            List<Ogrenci> okunanOgrenci = VeriOku(1); 
            while (okunanOgrenci.Count != 0) // Okuncak ��renci kalmayana kadar d�ng�
            {
                ogrenciler.Clear();
                ogrenciler.Add(okunanOgrenci[0]);
                ogrenciler.Add(ogrenci); // Eklenecek ��renciyi okunan ��renci ile bir listeye koy
                if (ogrenciler[0].numara == ogrenciler[1].numara) // ayn� numaraya sahipler
                {
                    MessageBox.Show("Bu numara zaten mevcut.");
                    File.Delete(dosyayoluyeni);
                    listeleFunc();
                    return;
                }
                ogrenciler = ogrenciler.OrderBy(x => x.isim).ThenBy(ogr => ogr.soyad).ToList(); // Listeyi alfabetik s�rala
                if (ogrenciler[0].numara == ogrenci.numara && eklendi == false) // yeni eklencek ��renci okunan ��renciden �nce mi
                {
                    VeriYaz(ogrenci); // O zaman eklenecek ��renciyi yaz
                    eklendi = true;
                }
                VeriYaz(okunanOgrenci[0]); // Yeni eklencek ��renci okunan ��renciden sonra ise okunan ��renciyi yaz


                okunanOgrenci = VeriOku(1); // S�radaki ��renciyi oku
            }
            satirSayisi = 0;

            if (!eklendi) VeriYaz(ogrenci); // T�m ��renciler okundu ve hala yeni ��renci eklenmemi�se yeni ��renciyi en alta ekle

            if (File.Exists(dosyayolu))
            {
                using (FileStream fs = File.Open(dosyayolu, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    // Dosyay� kapat
                    fs.Close();
                }
            }

            File.Delete(dosyayolu); // Eski metin belgesini sil
            File.Move(dosyayoluyeni, dosyayolu); // Yeni metin belgesini yeniden adland�r
        }





        //Listele d��mesi
        private void listeleButton_Click(object sender, EventArgs e)
        {
            listeleFunc();
        }

        //ListeleFunc Fonksiyonu (50'�er 50'�er veri okuyarak DataGrid'e yazd�r�r)
        private void listeleFunc()
        {
            int kisiSayisi = 0;
            dataGridView1.Rows.Clear(); 
            List<Ogrenci> ogrenciler = VeriOku(50); // 50 tane ��renci oku
            while (ogrenciler.Count != 0) // Okunacak ��renci kalmayana kadar d�ng�
            {
                ogrenciler = ogrenciler.Where(ogrenci => ogrenci.gizli == 1).ToList(); // Okunan ��rencilerden gizli de�eri 1 olmayanlar� ele
                kisiSayisi += ogrenciler.Count();
                foreach (Ogrenci ogrenci in ogrenciler) // Geriye kalan her ��renci i�in...
                {
                    dataGridView1.Rows.Add(ogrenci.isim, ogrenci.soyad, ogrenci.numara, ogrenci.gizli); // ...DataGrid'e ekleme yap.
                }
                ogrenciler = VeriOku(50); // S�radaki 50 ��renciyi oku
            }
            kisiSayisiLabel.Text = kisiSayisi.ToString() + " sonu� bulundu. " + "(Toplam: " + satirSayisi + ")";
            satirSayisi = 0;
        }





        //Arama D��mesi (50'�er okuyarak kriterlere uyanlar� datagrid'e ekler)
        private void aramaButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear(); // DataGrid'i temizle
            List<Ogrenci> ogrenciler = VeriOku(50); // �lk 50 ��renciyi oku
            while (ogrenciler.Count != 0) // Okunacak ��renci kalmayana kadar d�ng�
            {
                //Filtreleme
                if (isimBox.Text != "") ogrenciler = ogrenciler.Where(ogrenci => ogrenci.isim == isimBox.Text).ToList();
                if (soyadBox.Text != "") ogrenciler = ogrenciler.Where(ogrenci => ogrenci.soyad == soyadBox.Text).ToList();
                if (numaraBox.Text != "") ogrenciler = ogrenciler.Where(ogrenci => ogrenci.numara == numaraBox.Text).ToList();
                ogrenciler = ogrenciler.Where(ogrenci => ogrenci.gizli == 1).ToList();

                foreach (Ogrenci ogrenci in ogrenciler) // Filtreleme sonucu geriye kalan her ��renci i�in...
                {
                    dataGridView1.Rows.Add(ogrenci.isim, ogrenci.soyad, ogrenci.numara, ogrenci.gizli); // ...DataGrid'e ekleme yap
                }

                ogrenciler = VeriOku(50); // S�radaki 50 ��renciyi oku
            }
            satirSayisi = 0;
        }





        //Sil D��mesi
        private void silButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count <= 0) // DataGrid'de se�ili bir h�cre var m� kontrol�
            {
                MessageBox.Show("Bir ��renci se�iniz."); 
                return;
            }
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex; // Se�ili h�crenin sat�r�n� bul
            string secilenNumara = dataGridView1.Rows[selectedRowIndex].Cells["Numara"].Value.ToString(); // Bulunan sat�r�n Numara s�tun'unu al
            Ogrenci ogrenci = null;
            List<Ogrenci> ogrenciler = VeriOku(50); // �lk 50 ��renciyi oku
            while (ogrenciler.Count != 0) // Okunacak ��renci kalmayana kadar d�ng�
            {
                ogrenci = ogrenciler.First(p => p.numara == secilenNumara); // Okunan ��renciler aras�nda se�ili numara var m�
                if (ogrenci == null)
                {
                    ogrenciler = VeriOku(50); // Yoksa s�radaki 50 ��renciyi oku
                }
                else
                {
                    break; 
                }
            }
            satirSayisi = 0;

            ogrenci.gizli = 0; 
            VeriG�ncelle(ogrenci); 
            listeleFunc(); 
            MessageBox.Show("��renci ba�ar�yla silindi.");
        }





        //Datagrid de�i�iklik alg�lama
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            List<Ogrenci> ogrenciler = VeriOku(50); // �lk 50 ��renciyi oku
            string secilenNumara = dataGridView1.Rows[e.RowIndex].Cells["Numara"].Value.ToString(); // Se�ili h�creden Numara de�erini al
            Ogrenci ogrenci = null;

            while (ogrenciler.Count != 0) // Okunacak ��renci kalmayana kadar d�ng�
            {
                ogrenci = ogrenciler.First(p => p.numara == secilenNumara); // Okunan ��renciler aras�nda se�ili numara var m�
                if (ogrenci == null)
                {
                    ogrenciler = VeriOku(50); // Yoksa s�radaki 50 ��renciyi oku
                }
                else
                {
                    break; 
                }
            }
            satirSayisi = 0;

            ogrenci.isim = dataGridView1.Rows[e.RowIndex].Cells["isim"].Value.ToString();
            ogrenci.soyad = dataGridView1.Rows[e.RowIndex].Cells["soyad"].Value.ToString();
            ogrenci.numara = dataGridView1.Rows[e.RowIndex].Cells["numara"].Value.ToString();

            VeriG�ncelle(ogrenci);
            listeleFunc();
        }





        //Ad-Soyad ge�erli karakterler
        private void isimBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && (sender as TextBox).Text.EndsWith(" "))
            {
                e.Handled = true;
            }
        }

        //Numara ge�erli karakterler
        private void numaraBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
