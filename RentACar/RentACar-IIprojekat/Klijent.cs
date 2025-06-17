using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentACar_IIprojekat
{
    internal class Klijent
    {
        // Connection string za povezivanje sa bazom podataka
        private static string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\uSER\source\repos\RentACar-IIprojekat\RentACar-IIprojekat\RentACarDB.accdb";

        // Metoda za dohvatanje svih klijenata iz baze
        public static OleDbDataReader PokupiKlijente()
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                string query = "SELECT * FROM Klijent";
                OleDbCommand cmd = new OleDbCommand(query, con);

                con.Open();
                return cmd.ExecuteReader(); // Vraća čitač podataka sa svim klijentima
            }
        }

        // Metoda za dodavanje novog klijenta u bazu
        public static int DodajKlijenta(string ime, string prezime, string adresa, string telefon, string vozackaKategorija)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO Klijent (ime, prezime, adresa, telefon, vozacka_kategorija) VALUES (@ime, @prezime, @adresa, @telefon, @vozackaKategorija)";
                OleDbCommand cmd = new OleDbCommand(query, con);

                // Postavljanje parametara
                cmd.Parameters.AddWithValue("@ime", ime);
                cmd.Parameters.AddWithValue("@prezime", prezime);
                cmd.Parameters.AddWithValue("@adresa", adresa);
                cmd.Parameters.AddWithValue("@telefon", telefon);
                cmd.Parameters.AddWithValue("@vozackaKategorija", vozackaKategorija);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Izvršava SQL komandu za ubacivanje

                    // Sada dobijamo ID poslednje umetnute stavke
                    cmd.CommandText = "SELECT @@IDENTITY"; // Vraća ID poslednje umetnute stavke
                    return Convert.ToInt32(cmd.ExecuteScalar()); // Vraćamo ID klijenta
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom dodavanja klijenta: " + ex.Message);
                    return -1; // Greška prilikom dodavanja
                }
            }
        }

        // Metoda za ažuriranje podataka o postojećem klijentu
        public static bool UpdateKlijenta(int idKlijent, string ime, string prezime, string adresa, string telefon, string vozackaKategorija)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                string query = "UPDATE Klijent SET ime = @ime, prezime = @prezime, adresa = @adresa, telefon = @telefon, vozacka_kategorija = @vozackaKategorija WHERE id_klienta = @idKlijent";
                OleDbCommand cmd = new OleDbCommand(query, con);

                // Postavljanje parametara
                cmd.Parameters.AddWithValue("@ime", ime);
                cmd.Parameters.AddWithValue("@prezime", prezime);
                cmd.Parameters.AddWithValue("@adresa", adresa);
                cmd.Parameters.AddWithValue("@telefon", telefon);
                cmd.Parameters.AddWithValue("@vozackaKategorija", vozackaKategorija);
                cmd.Parameters.AddWithValue("@idKlijent", idKlijent);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Izvršava SQL komandu za ažuriranje
                    return true; // Uspešno ažurirano
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom ažuriranja klijenta: " + ex.Message);
                    return false; // Greška prilikom ažuriranja
                }
            }
        }

        // Metoda za brisanje klijenta iz baze
        public static bool DeleteKlijenta(int idKlijent)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                string query = "DELETE FROM Klijent WHERE id_klienta = @idKlijent";
                OleDbCommand cmd = new OleDbCommand(query, con);

                // Postavljanje parametara
                cmd.Parameters.AddWithValue("@idKlijent", idKlijent);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Izvršava SQL komandu za brisanje
                    return true; // Uspešno obrisano
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom brisanja klijenta: " + ex.Message);
                    return false; // Greška prilikom brisanja
                }
            }
        }
    }
}
