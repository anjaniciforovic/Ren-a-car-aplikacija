using System;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace RentACar_IIprojekat
{
    internal class Rezervacija
    {
        // Connection string za povezivanje sa bazom podataka
        private static string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\uSER\source\repos\RentACar-IIprojekat\RentACar-IIprojekat\RentACarDB.accdb";

        // Metoda za dohvatanje svih rezervacija iz baze
        public static OleDbDataReader PokupiRezervacije()
        {
            OleDbConnection con = new OleDbConnection(connectionString);
            string query = "SELECT * FROM Rezervacija";
            OleDbCommand cmd = new OleDbCommand(query, con);

            con.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        // Metoda za dodavanje nove rezervacije u bazu
        public static bool DodajRezervaciju(int idVozila, int idKlijenta, DateTime datumVremePocetka, DateTime datumVremeKraja, decimal cena)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO Rezervacija (id_vozila, id_klijenta, datumVreme_pocetka, datumVreme_kraja, cena) VALUES (@idVozila, @idKlijenta, @datumVremePocetka, @datumVremeKraja, @cena)";
                OleDbCommand cmd = new OleDbCommand(query, con);

                // Postavljanje parametara
                cmd.Parameters.AddWithValue("@idVozila", idVozila);
                cmd.Parameters.AddWithValue("@idKlijenta", idKlijenta);
                cmd.Parameters.AddWithValue("@datumVremePocetka", datumVremePocetka);
                cmd.Parameters.AddWithValue("@datumVremeKraja", datumVremeKraja);
                cmd.Parameters.AddWithValue("@cena", cena);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Izvršava SQL komandu za ubacivanje
                    return true; // Uspešno dodato
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom dodavanja rezervacije: " + ex.Message);
                    return false; // Greška prilikom dodavanja
                }
            }
        }

        // Metoda za ažuriranje podataka o postojećoj rezervaciji
        public static bool UpdateRezervaciju(int idRezervacija, int idVozila, int idKlijenta, DateTime datumVremePocetka, DateTime datumVremeKraja, decimal cena)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                string query = "UPDATE Rezervacija SET id_vozila = @idVozila, id_klijenta = @idKlijenta, datumVreme_pocetka = @datumVremePocetka, datumVreme_kraja = @datumVremeKraja, cena = @cena WHERE id_rezervacija = @idRezervacija";
                OleDbCommand cmd = new OleDbCommand(query, con);

                // Postavljanje parametara
                cmd.Parameters.AddWithValue("@idVozila", idVozila);
                cmd.Parameters.AddWithValue("@idKlijenta", idKlijenta);
                cmd.Parameters.AddWithValue("@datumVremePocetka", datumVremePocetka);
                cmd.Parameters.AddWithValue("@datumVremeKraja", datumVremeKraja);
                cmd.Parameters.AddWithValue("@cena", cena);
                cmd.Parameters.AddWithValue("@idRezervacija", idRezervacija);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Izvršava SQL komandu za ažuriranje
                    return true; // Uspešno ažurirano
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom ažuriranja rezervacije: " + ex.Message);
                    return false; // Greška prilikom ažuriranja
                }
            }
        }

        // Metoda za brisanje rezervacije iz baze
        public static bool DeleteRezervaciju(int idRezervacija)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                string query = "DELETE FROM Rezervacija WHERE id_rezervacija = @idRezervacija";
                OleDbCommand cmd = new OleDbCommand(query, con);

                // Postavljanje parametara
                cmd.Parameters.AddWithValue("@idRezervacija", idRezervacija);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Izvršava SQL komandu za brisanje
                    return true; // Uspešno obrisano
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom brisanja rezervacije: " + ex.Message);
                    return false; // Greška prilikom brisanja
                }
            }
        }
    }
}
