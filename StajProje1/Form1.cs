using System.Windows.Forms;

namespace StajProje1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string dosyayolu = "C:\\Users\\ohhcu\\source\\repos\\StajProje1\\StajProje1\\DataBase.txt";
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Ekle(Ogrenci NewOgrenci)
        {
            //Öðrenci kontrolü
            List<Ogrenci> Ogrenciler = Okuma();
            bool Kontrol = Ogrenciler.Exists(ogrenci => ogrenci.numara == NewOgrenci.numara);

            int sonID = 0;

            if (Ogrenciler.Count > 0)
            {
                sonID = Ogrenciler.Max(ogrenci => ogrenci.ID);
            }
            sonID += 1;

            NewOgrenci.ID = sonID;

            if (!Kontrol)
            {
                Ogrenciler.Add(NewOgrenci);
                MessageBox.Show("Kayýt yapýldý.");
            }
            else
            {
                MessageBox.Show("Numara zaten mevcut, Kayýt yapýlamadý.");
            }

            Yaz(Ogrenciler);

        }


        //ekleme düðmesi
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
            //numara sayý mý kontrolü
            try
            {
                int sayi = Int32.Parse(textBox3.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Numara kýsmýna sayý giriniz.");
                return;
            }
            //yeni öðrenci oluþturma
            Ogrenci NewOgrenci = new Ogrenci
            {
                isim = textBox1.Text,
                soyad = textBox2.Text,
                numara = Int32.Parse(textBox3.Text),
                Gizli = 1,

            };
            Ekle(NewOgrenci);
            Listele();
        }

        //okuma
        private List<Ogrenci> Okuma()
        {
            List<Ogrenci> Ogrenciler = new List<Ogrenci>();
            if (File.Exists(dosyayolu))
            {
                string[] satirlar = File.ReadAllLines(dosyayolu);
                foreach (string satir in satirlar)
                {
                    string[] kesme = satir.Split(',');
                    Ogrenci NewOgrenci = new Ogrenci
                    {
                        isim = kesme[0],
                        soyad = kesme[1],
                        numara = Int32.Parse(kesme[2]),
                        ID = Int32.Parse(kesme[3]),
                        Gizli = Int32.Parse(kesme[4]),

                    };
                    Ogrenciler.Add(NewOgrenci);
                }
            }
            return Ogrenciler;
        }

        private void Listele()
        {
            //sadece gizli deðeri 1 olanlarý seçer ve alfabetik sýralar
            List<Ogrenci> Ogrenciler = Okuma();
            Ogrenciler = Ogrenciler.Where(ogrenci => ogrenci.Gizli == 1).ToList();
            Ogrenciler = Ogrenciler.OrderBy(x => x.isim).ThenBy(ogr => ogr.soyad).ToList();
            dataGridView1.Rows.Clear();
            foreach (Ogrenci ogrenci in Ogrenciler)
            {
                dataGridView1.Rows.Add(ogrenci.isim, ogrenci.soyad, ogrenci.numara, ogrenci.ID, ogrenci.Gizli);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Listele();
        }
        private void Yaz(List<Ogrenci> Ogrenciler)
        {
 
            Ogrenciler = Ogrenciler.OrderBy(x => x.isim).ThenBy(ogr => ogr.soyad).ToList();

            using (StreamWriter writer = new StreamWriter(dosyayolu))
            {
                foreach (Ogrenci ogrenci in Ogrenciler)
                {
                    writer.WriteLine(ogrenci.isim + "," + ogrenci.soyad + "," + ogrenci.numara + "," + ogrenci.ID + "," + ogrenci.Gizli);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //silincek deðerin gizli deðerini 0 yapar
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            int secilenID = (int)dataGridView1.Rows[selectedRowIndex].Cells["ID"].Value;

            List<Ogrenci> Ogrenciler = Okuma();
            Ogrenci Ogrenci = Ogrenciler.First(a => a.ID == secilenID);
            Ogrenci.Gizli = 0;
            Yaz(Ogrenciler);
            Listele();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Filtreleme
            List<Ogrenci> Ogrenciler = Okuma();
            if (textBox1.Text != "") Ogrenciler = Ogrenciler.Where(ogrenci => ogrenci.isim == textBox1.Text).ToList();
            if (textBox2.Text != "") Ogrenciler = Ogrenciler.Where(ogrenci => ogrenci.soyad == textBox2.Text).ToList();
            if (textBox3.Text != "") Ogrenciler = Ogrenciler.Where(ogrenci => ogrenci.numara == int.Parse(textBox3.Text)).ToList();
            Ogrenciler = Ogrenciler.Where(ogrenci => ogrenci.Gizli == 1).ToList();
            Ogrenciler = Ogrenciler.OrderBy(p => p.isim).ThenBy(p => p.soyad).ThenBy(p => p.numara).ToList();
            dataGridView1.Rows.Clear();
            foreach (Ogrenci ogrenci in Ogrenciler)
            {
                dataGridView1.Rows.Add(ogrenci.isim, ogrenci.soyad, ogrenci.numara, ogrenci.ID, ogrenci.Gizli);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //güncelleme
            List<Ogrenci> Ogrenciler = Okuma();
            int secilenID = (int)dataGridView1.Rows[e.RowIndex].Cells["ID"].Value;
            Ogrenci ogrenci = Ogrenciler.First(p => p.ID == secilenID);
            ogrenci.isim = dataGridView1.Rows[e.RowIndex].Cells["isim"].Value.ToString();
            ogrenci.soyad = dataGridView1.Rows[e.RowIndex].Cells["soyad"].Value.ToString();
            ogrenci.numara = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["numara"].Value.ToString());
            Yaz(Ogrenciler);
        }
    }
}
