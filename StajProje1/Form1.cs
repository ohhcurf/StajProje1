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
            // Uygulama açýldýðýnda database belgesi bulunamadýysa yaratýr
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
            int maksimumSatir = tane + satirSayisi; // Okunacak maksimum satýr sayýsý girilen parametreye eþitleniyor

            // maksimumSatir deðeri kadar satýr okunup öðrenciler listeye eklendikten sonra bu deðer return'lenir
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

        // Güncelleme
        private void VeriGüncelle(Ogrenci ogrenci)
        {
            using (File.Create(dosyayoluyeni))
            {
                // Yeni metin belgesi oluþturulur
            }

            List<Ogrenci> okunanOgrenci = VeriOku(1); 
            while (okunanOgrenci.Count != 0) // Okuncak öðrenci kalmayana kadar döngü
            {
                if (okunanOgrenci[0].numara != ogrenci.numara) 
                {
                    VeriYaz(okunanOgrenci[0]);
                }

                okunanOgrenci = VeriOku(1); // Sýradaki öðrenciyi oku
            }
            satirSayisi = 0;

            if (File.Exists(dosyayolu))
            {
                using (FileStream fs = File.Open(dosyayolu, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    // Dosyayý kapat
                    fs.Close();
                }
            }

            File.Delete(dosyayolu); // Eski metin belgesini sil
            File.Move(dosyayoluyeni, dosyayolu); // Yeni metin belgesini yeniden adlandýr

            ekleFunc(ogrenci);
        }





        //Ekleme Düðmesi
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
            //numara sayý mý kontrolü
            try
            {
                int sayi = Int32.Parse(numaraBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Numara kýsmýna sayý giriniz.");
                return;
            }
            //Yeni öðrenci oluþturma
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

        //Ekleme Fonksiyonu (alfabetik sýrayla ekleme yapar)
        private void ekleFunc(Ogrenci ogrenci)
        {
            var eklendi = false;
            using (File.Create(dosyayoluyeni))
            {
                // Yeni metin belgesi oluþturulur
            }

            List<Ogrenci> ogrenciler = new List<Ogrenci>();
            List<Ogrenci> okunanOgrenci = VeriOku(1); 
            while (okunanOgrenci.Count != 0) // Okuncak öðrenci kalmayana kadar döngü
            {
                ogrenciler.Clear();
                ogrenciler.Add(okunanOgrenci[0]);
                ogrenciler.Add(ogrenci); // Eklenecek öðrenciyi okunan öðrenci ile bir listeye koy
                if (ogrenciler[0].numara == ogrenciler[1].numara) // ayný numaraya sahipler
                {
                    MessageBox.Show("Bu numara zaten mevcut.");
                    File.Delete(dosyayoluyeni);
                    listeleFunc();
                    return;
                }
                ogrenciler = ogrenciler.OrderBy(x => x.isim).ThenBy(ogr => ogr.soyad).ToList(); // Listeyi alfabetik sýrala
                if (ogrenciler[0].numara == ogrenci.numara && eklendi == false) // yeni eklencek öðrenci okunan öðrenciden önce mi
                {
                    VeriYaz(ogrenci); // O zaman eklenecek öðrenciyi yaz
                    eklendi = true;
                }
                VeriYaz(okunanOgrenci[0]); // Yeni eklencek öðrenci okunan öðrenciden sonra ise okunan öðrenciyi yaz


                okunanOgrenci = VeriOku(1); // Sýradaki öðrenciyi oku
            }
            satirSayisi = 0;

            if (!eklendi) VeriYaz(ogrenci); // Tüm öðrenciler okundu ve hala yeni öðrenci eklenmemiþse yeni öðrenciyi en alta ekle

            if (File.Exists(dosyayolu))
            {
                using (FileStream fs = File.Open(dosyayolu, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    // Dosyayý kapat
                    fs.Close();
                }
            }

            File.Delete(dosyayolu); // Eski metin belgesini sil
            File.Move(dosyayoluyeni, dosyayolu); // Yeni metin belgesini yeniden adlandýr
        }





        //Listele düðmesi
        private void listeleButton_Click(object sender, EventArgs e)
        {
            listeleFunc();
        }

        //ListeleFunc Fonksiyonu (50'þer 50'þer veri okuyarak DataGrid'e yazdýrýr)
        private void listeleFunc()
        {
            int kisiSayisi = 0;
            dataGridView1.Rows.Clear(); 
            List<Ogrenci> ogrenciler = VeriOku(50); // 50 tane öðrenci oku
            while (ogrenciler.Count != 0) // Okunacak öðrenci kalmayana kadar döngü
            {
                ogrenciler = ogrenciler.Where(ogrenci => ogrenci.gizli == 1).ToList(); // Okunan öðrencilerden gizli deðeri 1 olmayanlarý ele
                kisiSayisi += ogrenciler.Count();
                foreach (Ogrenci ogrenci in ogrenciler) // Geriye kalan her öðrenci için...
                {
                    dataGridView1.Rows.Add(ogrenci.isim, ogrenci.soyad, ogrenci.numara, ogrenci.gizli); // ...DataGrid'e ekleme yap.
                }
                ogrenciler = VeriOku(50); // Sýradaki 50 öðrenciyi oku
            }
            kisiSayisiLabel.Text = kisiSayisi.ToString() + " sonuç bulundu. " + "(Toplam: " + satirSayisi + ")";
            satirSayisi = 0;
        }





        //Arama Düðmesi (50'þer okuyarak kriterlere uyanlarý datagrid'e ekler)
        private void aramaButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear(); // DataGrid'i temizle
            List<Ogrenci> ogrenciler = VeriOku(50); // Ýlk 50 öðrenciyi oku
            while (ogrenciler.Count != 0) // Okunacak öðrenci kalmayana kadar döngü
            {
                //Filtreleme
                if (isimBox.Text != "") ogrenciler = ogrenciler.Where(ogrenci => ogrenci.isim == isimBox.Text).ToList();
                if (soyadBox.Text != "") ogrenciler = ogrenciler.Where(ogrenci => ogrenci.soyad == soyadBox.Text).ToList();
                if (numaraBox.Text != "") ogrenciler = ogrenciler.Where(ogrenci => ogrenci.numara == numaraBox.Text).ToList();
                ogrenciler = ogrenciler.Where(ogrenci => ogrenci.gizli == 1).ToList();

                foreach (Ogrenci ogrenci in ogrenciler) // Filtreleme sonucu geriye kalan her öðrenci için...
                {
                    dataGridView1.Rows.Add(ogrenci.isim, ogrenci.soyad, ogrenci.numara, ogrenci.gizli); // ...DataGrid'e ekleme yap
                }

                ogrenciler = VeriOku(50); // Sýradaki 50 öðrenciyi oku
            }
            satirSayisi = 0;
        }





        //Sil Düðmesi
        private void silButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count <= 0) // DataGrid'de seçili bir hücre var mý kontrolü
            {
                MessageBox.Show("Bir öðrenci seçiniz."); 
                return;
            }
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex; // Seçili hücrenin satýrýný bul
            string secilenNumara = dataGridView1.Rows[selectedRowIndex].Cells["Numara"].Value.ToString(); // Bulunan satýrýn Numara sütun'unu al
            Ogrenci ogrenci = null;
            List<Ogrenci> ogrenciler = VeriOku(50); // Ýlk 50 öðrenciyi oku
            while (ogrenciler.Count != 0) // Okunacak öðrenci kalmayana kadar döngü
            {
                ogrenci = ogrenciler.First(p => p.numara == secilenNumara); // Okunan öðrenciler arasýnda seçili numara var mý
                if (ogrenci == null)
                {
                    ogrenciler = VeriOku(50); // Yoksa sýradaki 50 öðrenciyi oku
                }
                else
                {
                    break; 
                }
            }
            satirSayisi = 0;

            ogrenci.gizli = 0; 
            VeriGüncelle(ogrenci); 
            listeleFunc(); 
            MessageBox.Show("Öðrenci baþarýyla silindi.");
        }





        //Datagrid deðiþiklik algýlama
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            List<Ogrenci> ogrenciler = VeriOku(50); // Ýlk 50 öðrenciyi oku
            string secilenNumara = dataGridView1.Rows[e.RowIndex].Cells["Numara"].Value.ToString(); // Seçili hücreden Numara deðerini al
            Ogrenci ogrenci = null;

            while (ogrenciler.Count != 0) // Okunacak öðrenci kalmayana kadar döngü
            {
                ogrenci = ogrenciler.First(p => p.numara == secilenNumara); // Okunan öðrenciler arasýnda seçili numara var mý
                if (ogrenci == null)
                {
                    ogrenciler = VeriOku(50); // Yoksa sýradaki 50 öðrenciyi oku
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

            VeriGüncelle(ogrenci);
            listeleFunc();
        }





        //Ad-Soyad geçerli karakterler
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

        //Numara geçerli karakterler
        private void numaraBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
