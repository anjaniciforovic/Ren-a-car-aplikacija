using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RentACar_IIprojekat
{
    public partial class Pocetna : Form
    {
        // Dictionary za čuvanje ID-a i naziva kategorija
        private Dictionary<int, string> kategorijeDict = new Dictionary<int, string>();
        public DateTime DatumVremePocetka { get; set; }
        public DateTime DatumVremeKraja { get; set; }
        public decimal Cena { get; set; }


        public Pocetna()
        {
            InitializeComponent();
            PuniComboBoxKategorije();
            PuniListuAutomobila();
        }




        private void PuniListuAutomobila()
        {
            // Clear existing items
            lstAutomobili.Items.Clear();

            using (OleDbDataReader reader = Vozilo.PokupiVozila())
            {
                while (reader.Read())
                {
                    // Read data, including id_kategorije from vozilo
                    int idKategorija = Convert.ToInt32(reader["id_kategorije"]); // Ova kolona je sada dostupna
                    var item = new ListViewItem(reader["naziv_kategorije"].ToString()) // Prikazujemo naziv kategorije
                    {
                        Tag = reader["id_vozila"] // Store the vehicle ID in the Tag property
                    };

                    // Add subitems to the ListViewItem in the specified order
                    item.SubItems.Add(reader["naziv"].ToString()); // Add vehicle name
                    item.SubItems.Add(reader["marka"].ToString()); // Add vehicle brand
                    item.SubItems.Add(reader["model"].ToString()); // Add vehicle model
                    item.SubItems.Add(reader["godina_proizvodnje"].ToString()); // Add year of manufacture
                    item.SubItems.Add(reader["cena_po_satu"].ToString()); // Add price per hour

                    // Add the item to the ListView
                    lstAutomobili.Items.Add(item);
                }
            }
        }






        private void FilterAutomobili()
        {
            

            // Clear existing items
            lstAutomobili.Items.Clear();

            DateTime startDate = dateTimePicker1.Value;
            DateTime endDate = dateTimePicker2.Value;
            decimal selectedPrice = nudCena.Value;

            // Get the selected category ID
            int selectedCategoryId = UzmiIzabraniIdKategorije2();
            

            // Prepare a list to hold filtered vehicles
            List<ListViewItem> filteredItems = new List<ListViewItem>();

            using (OleDbDataReader reader = Vozilo.PokupiVozila())
            {
                while (reader.Read())
                {
                    // Get necessary details
                    int idVozila = Convert.ToInt32(reader["id_vozila"]);
                    decimal carPrice = Convert.ToDecimal(reader["cena_po_satu"]);
                    int idKategorija = Convert.ToInt32(reader["id_kategorije"]); // Ovde uzimamo iz vozila

                    // Check availability
                    bool isAvailable = true;
                    // ... (your existing availability check code)

                    // Filter logic
                    if (isAvailable && carPrice <= selectedPrice)
                    {
                        if (selectedCategoryId == -1 || idKategorija == selectedCategoryId)
                        {
                            // Create a new ListViewItem for the filtered vehicle
                            var item = new ListViewItem(reader["id_kategorije"].ToString())
                            {
                                Tag = reader["id_vozila"]
                            };

                            // Add subitems
                            item.SubItems.Add(reader["naziv"].ToString());
                            item.SubItems.Add(reader["marka"].ToString());
                            item.SubItems.Add(reader["model"].ToString());
                            item.SubItems.Add(reader["godina_proizvodnje"].ToString());
                            item.SubItems.Add(reader["cena_po_satu"].ToString());

                            // Add filtered item to the list
                            filteredItems.Add(item);
                        }
                    }
                }
            }

            // Clear existing items and add filtered items to the ListView
            lstAutomobili.Items.Clear();
            lstAutomobili.Items.AddRange(filteredItems.ToArray());

           
        }









        private void AddCarToListView(OleDbDataReader reader)
        {
            var item = new ListViewItem(reader["naziv"].ToString())
            {
                Tag = reader["id_vozila"] // Store the vehicle ID in the Tag property
            };
            item.SubItems.Add(reader["marka"].ToString());
            item.SubItems.Add(reader["model"].ToString());
            item.SubItems.Add(reader["godina_proizvodnje"].ToString());
            item.SubItems.Add(reader["cena_po_satu"].ToString());
            lstAutomobili.Items.Add(item);
        }



        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void btnDodajKategoriju_Click(object sender, EventArgs e)
        {
            // Preuzimanje unetih vrednosti iz TextBox-ova
            string nazivKategorije = txtNazivK.Text.Trim();
            string opisKategorije = txtOpisK.Text.Trim();

            // Provera da li su polja prazna
            if (string.IsNullOrEmpty(nazivKategorije) || string.IsNullOrEmpty(opisKategorije))
            {
                MessageBox.Show("Molimo unesite naziv i opis kategorije.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Pokušaj dodavanja nove kategorije pomoću metode DodajKategoriju iz klase Kategorija
            bool uspesnoDodato = Kategorija.DodajKategoriju(nazivKategorije, opisKategorije);

            // Provera da li je dodavanje bilo uspešno
            if (uspesnoDodato)
            {
                MessageBox.Show("Kategorija uspešno dodata!", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Osvježavanje forme (npr. brisanje polja nakon uspešnog unosa)
                txtNazivK.Clear();
                txtOpisK.Clear();

                // Opciono, osveži prikaz kategorija ako ih prikazuješ na nekom ListView-u ili DataGrid-u
                // OsvjeziKategorijePrikaz(); // primer metode koju bi dodao za osvežavanje
            }
            else
            {
                MessageBox.Show("Dodavanje kategorije nije uspelo.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSacuvajA_Click(object sender, EventArgs e)
        {
            int idKategorije = UzmiIzabraniIdKategorije();
            string naziv = txtNazivA.Text;
            string marka = txtMarkaA.Text;
            string model = txtModelA.Text;

            int godinaProizvodnje;
            if (!int.TryParse(txtGodinaProizvodnje.Text, out godinaProizvodnje))
            {
                MessageBox.Show("Unesite validnu godinu proizvodnje.");
                return;
            }

            double cenaPoSatu;
            if (!double.TryParse(txtCenaPoSatu.Text, out cenaPoSatu))
            {
                MessageBox.Show("Unesite validnu cenu po satu.");
                return;
            }

            // Proveri da li je ID kategorije validan
            if (idKategorije != -1)
            {
                bool uspesnoDodato = Vozilo.DodajVozilo(idKategorije, naziv, marka, model, godinaProizvodnje, cenaPoSatu);

                if (uspesnoDodato)
                {
                    MessageBox.Show("Vozilo uspešno dodato!");
                }
                else
                {
                    MessageBox.Show("Greška prilikom dodavanja vozila.");
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati kategoriju.");
            }

        }

        private void Pocetna_Load(object sender, EventArgs e)
        {
            lstRezervacije.Columns.Add("ID Rezervacija", 100, HorizontalAlignment.Left);
            lstRezervacije.Columns.Add("ID Vozila", 100, HorizontalAlignment.Left);
            lstRezervacije.Columns.Add("ID Klijenta", 100, HorizontalAlignment.Left);
            lstRezervacije.Columns.Add("Datum Početka", 150, HorizontalAlignment.Left);
            lstRezervacije.Columns.Add("Datum Kraja", 150, HorizontalAlignment.Left);
            lstRezervacije.Columns.Add("Cena", 100, HorizontalAlignment.Left);
            PrikaziSveRezervacije();


            // Define the columns for the ListView
            lstAutomobili.Columns.Add("Kategorija", 100); // Add column for category ID
            lstAutomobili.Columns.Add("Naziv", 150);
            lstAutomobili.Columns.Add("Marka", 100);
            lstAutomobili.Columns.Add("Model", 100);
            lstAutomobili.Columns.Add("Godina Proizvodnje", 150);
            lstAutomobili.Columns.Add("Cena po satu", 100);
            PuniListuAutomobila();
            PuniComboBoxKategorije2();
            
            
        }

        public void PrikaziSveRezervacije()
        {
            // Čišćenje ListView pre nego što dodaš nove podatke
            lstRezervacije.Items.Clear();

            // Poziv metode iz klase Rezervacija da pokupi sve rezervacije
            using (OleDbDataReader reader = Rezervacija.PokupiRezervacije())
            {
                // Prolaz kroz sve rezervacije i dodavanje u ListView
                while (reader.Read())
                {
                    // Dodavanje vrednosti za id_rezervacija
                    ListViewItem item = new ListViewItem(reader["id_rezervacija"] != DBNull.Value ? reader["id_rezervacija"].ToString() : "N/A");

                    // Dodavanje vrednosti za id_vozila
                    item.SubItems.Add(reader["id_vozila"] != DBNull.Value ? reader["id_vozila"].ToString() : "N/A");

                    // Dodavanje vrednosti za id_klijenta
                    item.SubItems.Add(reader["id_klijenta"] != DBNull.Value ? reader["id_klijenta"].ToString() : "N/A");

                    // Provera za datum pocetka
                    if (reader["datumVreme_pocetka"] != DBNull.Value)
                    {
                        item.SubItems.Add(Convert.ToDateTime(reader["datumVreme_pocetka"]).ToString("dd.MM.yyyy HH:mm"));
                    }
                    else
                    {
                        item.SubItems.Add("N/A");
                    }

                    // Provera za datum kraja
                    if (reader["datumVreme_kraja"] != DBNull.Value)
                    {
                        item.SubItems.Add(Convert.ToDateTime(reader["datumVreme_kraja"]).ToString("dd.MM.yyyy HH:mm"));
                    }
                    else
                    {
                        item.SubItems.Add("N/A");
                    }

                    // Provera za cenu
                    item.SubItems.Add(reader["cena"] != DBNull.Value ? reader["cena"].ToString() : "0");

                    // Dodavanje reda u ListView
                    lstRezervacije.Items.Add(item);
                }
            }
        }




        // Metoda za punjenje ComboBox-a kategorijama iz baze
        private void PuniComboBoxKategorije()
        {// Clear existing items to avoid duplicates
            cmbIdKategorije.Items.Clear();
            kategorijeDict.Clear(); // Clear dictionary for fresh load

            using (OleDbDataReader reader = Kategorija.PokupiKategorije())
            {
                while (reader.Read())
                {
                    // Add category name and ID to ComboBox
                    var idKategorije = Convert.ToInt32(reader["id_kategorije"]);
                    var nazivKategorije = reader["naziv"].ToString();
                    cmbIdKategorije.Items.Add(new KeyValuePair<int, string>(idKategorije, nazivKategorije));
                    kategorijeDict.Add(idKategorije, nazivKategorije); // Add to dictionary
                }
            }

            // Set display member and value member for the ComboBox
            cmbIdKategorije.DisplayMember = "Value";
            cmbIdKategorije.ValueMember = "Key";
        
        }
        private void PuniComboBoxKategorije2()
        {
            // Clear existing items to avoid duplicates
            cmbKategorije.Items.Clear();
            kategorijeDict.Clear(); // Clear dictionary for fresh load

            using (OleDbDataReader reader = Kategorija.PokupiKategorije())
            {
                while (reader.Read())
                {
                    // Add category name and ID to ComboBox
                    var idKategorije = Convert.ToInt32(reader["id_kategorije"]);
                    var nazivKategorije = reader["naziv"].ToString();
                    cmbKategorije.Items.Add(new KeyValuePair<int, string>(idKategorije, nazivKategorije));
                    kategorijeDict.Add(idKategorije, nazivKategorije); // Add to dictionary
                }
            }

            // Set display member and value member for the ComboBox
            cmbKategorije.DisplayMember = "Value";
            cmbKategorije.ValueMember = "Key";
        }


        private int UzmiIzabraniIdKategorije()
        {
            if (cmbIdKategorije.SelectedItem != null)
            {
                var selectedItem = (KeyValuePair<int, string>)cmbIdKategorije.SelectedItem;
                return selectedItem.Key; // Return category ID
            }
            return -1; // If nothing is selected
        }
        private int UzmiIzabraniIdKategorije2()
        {
            if (cmbKategorije.SelectedItem != null)
            {
                var selectedItem = (KeyValuePair<int, string>)cmbKategorije.SelectedItem;
                return selectedItem.Key; // Return category ID
            }
            return -1; // If nothing is selected
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            FilterAutomobili();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            FilterAutomobili();
        }

        private void nudCena_ValueChanged(object sender, EventArgs e)
        {
            FilterAutomobili();
        }

        private void cmbKategorije_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterAutomobili();
        }

        private int selectedVehicleId;

        int SelectedCarId;
        private void listViewVozila_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstAutomobili.SelectedItems.Count > 0)
            {
                // Pretpostavljamo da je ID vozila u prvoj koloni (0-index)
                SelectedCarId = Convert.ToInt32(lstAutomobili.SelectedItems[0].SubItems[0].Text);
            }
        }
        private void Rezervisi_Click(object sender, EventArgs e)
        {
            var dodajKlijentaForma = new DodajKlijentaForma();

            // Prikazivanje forme
            if (dodajKlijentaForma.ShowDialog() == DialogResult.OK)
            {
                // Ako je klijent uspešno dodat, dobijamo ID klijenta
                int idKlijenta = (int)dodajKlijentaForma.Tag;

                // Kreiranje rezervacije
                bool isRezervacijaDodata = Rezervacija.DodajRezervaciju(SelectedCarId, idKlijenta, dateTimePicker1.Value, dateTimePicker2.Value, nudCena.Value);

                if (isRezervacijaDodata)
                {
                    MessageBox.Show("Rezervacija uspešno sačuvana!");
                }
                else
                {
                    MessageBox.Show("Došlo je do greške prilikom čuvanja rezervacije.");
                }
            }
        }
    }
}
