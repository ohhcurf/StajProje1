using System.Windows.Forms;

namespace StajProje1
{
    public partial class Form1 : Form
    {
        static string dosya = "database.txt";
        static string dizin = Directory.GetCurrentDirectory();
        string dosyayolu = Path.Combine(dizin, dosya);
        public Form1()
        {
            InitializeComponent();
            if (File.Exists(dosyayolu))
            {

            }
            else
            {
                File.WriteAllText(dosyayolu, "");
            }
        }




        private void Form1_Load(object sender, EventArgs e)
        {

        }



        //ekle
        private void Ekle(Ogrenci newOgrenci)
        {
            //��renci kontrol�
            List<Ogrenci> ogrenciler = Okuma();
            bool kontrol = ogrenciler.Exists(ogrenci => ogrenci.numara == newOgrenci.numara);

            int sonID = 0;

            if (ogrenciler.Count > 0)
            {
                sonID = ogrenciler.Max(ogrenci => ogrenci.ID);
            }
            sonID += 1;

            newOgrenci.ID = sonID;

            if (!kontrol)
            {
                ogrenciler.Add(newOgrenci);
                MessageBox.Show("Kay�t yap�ld�.");
            }
            else
            {
                MessageBox.Show("Numara zaten mevcut, Kay�t yap�lamad�.");
            }

            Yaz(ogrenciler);

        }




        //ekleme d��mesi
        private void button1_Click(object sender, EventArgs e)
        {
            //kontrol etme
            if (textBox1.Text == "")
            {
                MessageBox.Show("isim giriniz.");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("soyad giriniz.");
                return;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("numara giriniz.");
                return;
            }
            //numara say� m� kontrol�
            try
            {
                int sayi = Int32.Parse(textBox3.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Numara k�sm�na say� giriniz.");
                return;
            }
            //yeni ��renci olu�turma
            Ogrenci newOgrenci = new Ogrenci
            {
                isim = textBox1.Text,
                soyad = textBox2.Text,
                numara = textBox3.Text,
                gizli = 1,

            };
            Ekle(newOgrenci);
            Listele();
        }




        //okuma
        private List<Ogrenci> Okuma()
        {
            List<Ogrenci> ogrenciler = new List<Ogrenci>();
            if (File.Exists(dosyayolu))
            {
                using (StreamReader sr = new StreamReader(dosyayolu))
                {
                    string satir;
                    while ((satir = sr.ReadLine()) != null)
                    {
                        string[] kesme = satir.Split(',');
                        Ogrenci newOgrenci = new Ogrenci
                        {
                            isim = kesme[0],
                            soyad = kesme[1],
                            numara = kesme[2],
                            ID = Int32.Parse(kesme[3]),
                            gizli = Int32.Parse(kesme[4]),

                        };
                        ogrenciler.Add(newOgrenci);
                    }
                }
            }
            return ogrenciler;
        }




        //listele
        private void Listele()
        {
            //sadece gizli de�eri 1 olanlar� se�er ve alfabetik s�ralar
            List<Ogrenci> ogrenciler = Okuma();
            ogrenciler = ogrenciler.Where(ogrenci => ogrenci.gizli == 1).ToList();
            ogrenciler = ogrenciler.OrderBy(x => x.isim).ThenBy(ogr => ogr.soyad).ToList();
            dataGridView1.Rows.Clear();
            foreach (Ogrenci ogrenci in ogrenciler)
            {
                dataGridView1.Rows.Add(ogrenci.isim, ogrenci.soyad, ogrenci.numara, ogrenci.ID, ogrenci.gizli);
            }


        }




        //listele d��mesi
        private void button2_Click(object sender, EventArgs e)
        {
            Listele();
        }



        //yaz
        private void Yaz(List<Ogrenci> ogrenciler)
        {

            ogrenciler = ogrenciler.OrderBy(x => x.isim).ThenBy(ogr => ogr.soyad).ToList();

            using (StreamWriter writer = new StreamWriter(dosyayolu))
            {
                foreach (Ogrenci ogrenci in ogrenciler)
                {
                    writer.WriteLine(ogrenci.isim + "," + ogrenci.soyad + "," + ogrenci.numara + "," + ogrenci.ID + "," + ogrenci.gizli);
                }
            }
        }




        //sil d��mesi
        private void button3_Click(object sender, EventArgs e)
        {
            //silincek de�erin gizli de�erini 0 yapar
            if(dataGridView1.SelectedCells.Count<= 0)
            {
                MessageBox.Show("Bir ��renci se�iniz.");
                return;
            }
            
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            int secilenID = (int)dataGridView1.Rows[selectedRowIndex].Cells["ID"].Value;
            List<Ogrenci> Ogrenciler = Okuma();
            Ogrenci Ogrenci = Ogrenciler.First(a => a.ID == secilenID);
            Ogrenci.gizli = 0;
            Yaz(Ogrenciler);
            Listele();
            MessageBox.Show("��renci ba�ar�yla silindi.");
        }




        //Arama d��mesi
        private void button4_Click(object sender, EventArgs e)
        {
            //Filtreleme
            List<Ogrenci> ogrenciler = Okuma();
            if (textBox1.Text != "") ogrenciler = ogrenciler.Where(ogrenci => ogrenci.isim == textBox1.Text).ToList();
            if (textBox2.Text != "") ogrenciler = ogrenciler.Where(ogrenci => ogrenci.soyad == textBox2.Text).ToList();
            if (textBox3.Text != "") ogrenciler = ogrenciler.Where(ogrenci => ogrenci.numara == textBox3.Text).ToList();
            ogrenciler = ogrenciler.Where(ogrenci => ogrenci.gizli == 1).ToList();
            ogrenciler = ogrenciler.OrderBy(p => p.isim).ThenBy(p => p.soyad).ThenBy(p => p.numara).ToList();
            dataGridView1.Rows.Clear();
            foreach (Ogrenci ogrenci in ogrenciler)
            {
                dataGridView1.Rows.Add(ogrenci.isim, ogrenci.soyad, ogrenci.numara, ogrenci.ID, ogrenci.gizli);
            }
        }




        //Datagrid de�i�iklik alg�lama
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //g�ncelleme
            List<Ogrenci> ogrenciler = Okuma();
            int secilenID = (int)dataGridView1.Rows[e.RowIndex].Cells["ID"].Value;
            Ogrenci ogrenci = ogrenciler.First(p => p.ID == secilenID);
            ogrenci.isim = dataGridView1.Rows[e.RowIndex].Cells["isim"].Value.ToString();
            ogrenci.soyad = dataGridView1.Rows[e.RowIndex].Cells["soyad"].Value.ToString();
            ogrenci.numara = dataGridView1.Rows[e.RowIndex].Cells["numara"].Value.ToString();
            Yaz(ogrenciler);
        }




        //ad-soyad ge�erli karakterler
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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




        //numara ge�erli karakterler
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
