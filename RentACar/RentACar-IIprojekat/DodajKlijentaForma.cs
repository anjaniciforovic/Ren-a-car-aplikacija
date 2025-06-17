using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentACar_IIprojekat
{
    public partial class DodajKlijentaForma : Form
    {
        // Dodatna svojstva za prenos podataka
        public DateTime DatumVremePocetka { get; set; }
        public DateTime DatumVremeKraja { get; set; }
        public decimal Cena { get; set; }
        public int SelectedCarId { get; set; } // ID izabranog automobila

        public DodajKlijentaForma()
        {
            InitializeComponent();
        }

        private void DodajKlijentaForma_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Prikupite podatke o klijentu
            string ime = txtIme.Text;
            string prezime = txtPrezime.Text;
            string adresa = txtAdresa.Text;
            string telefon = txtTelefon.Text;
            string vozackaKategorija = txtVozacka.Text;

            // Sačuvajte novog klijenta u bazu i dobijte njegov ID
            int idKlijenta = Klijent.DodajKlijenta(ime, prezime, adresa, telefon, vozackaKategorija);
            if (idKlijenta != -1)
            {
                MessageBox.Show("Klijent uspešno dodat! ID: " + idKlijenta);

                // Postavite ID klijenta kao DialogResult
                this.DialogResult = DialogResult.OK;
                this.Tag = idKlijenta; // Koristimo Tag da prenesemo ID klijenta
                this.Close(); // Zatvori formu
            }
            else
            {
                MessageBox.Show("Došlo je do greške prilikom dodavanja klijenta.");
            }
        }

       

       
    }
}

