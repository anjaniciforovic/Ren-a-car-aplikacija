using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentACar_IIprojekat
{
    internal class Kategorija
    {
        // Connection string za povezivanje sa bazom podataka
        public static string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\uSER\source\repos\RentACar-IIprojekat\RentACar-IIprojekat\RentACarDB.accdb";

        // Metoda za dohvatanje svih kategorija iz baze
        public static OleDbDataReader PokupiKategorije()
        {
            OleDbConnection con = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Kategorija", con);

            con.Open(); // Open the connection first
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        // Metoda za dodavanje nove kategorije u bazu
        public static bool DodajKategoriju(string naziv, string opis)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO Kategorija (naziv, opis) VALUES (@naziv, @opis)";
                OleDbCommand cmd = new OleDbCommand(query, con);

                // Postavljanje parametara
                cmd.Parameters.AddWithValue("@naziv", naziv);
                cmd.Parameters.AddWithValue("@opis", opis);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Izvršava SQL komandu za ubacivanje
                    return true; // Uspešno dodato
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom dodavanja kategorije: " + ex.Message);
                    return false; // Greška prilikom dodavanja
                }
            }
        }

        // Metoda za ažuriranje podataka o postojećoj kategoriji
        public static bool UpdateKategoriju(int idKategorija, string naziv, string opis)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                string query = "UPDATE Kategorija SET naziv = @naziv, opis = @opis WHERE id_kategorija = @idKategorija";
                OleDbCommand cmd = new OleDbCommand(query, con);

                // Postavljanje parametara
                cmd.Parameters.AddWithValue("@naziv", naziv);
                cmd.Parameters.AddWithValue("@opis", opis);
                cmd.Parameters.AddWithValue("@idKategorija", idKategorija);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Izvršava SQL komandu za ažuriranje
                    return true; // Uspešno ažurirano
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom ažuriranja kategorije: " + ex.Message);
                    return false; // Greška prilikom ažuriranja
                }
            }
        }

        // Metoda za brisanje kategorije iz baze
        public static bool DeleteKategoriju(int idKategorija)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                string query = "DELETE FROM Kategorija WHERE id_kategorija = @idKategorija";
                OleDbCommand cmd = new OleDbCommand(query, con);

                // Postavljanje parametara
                cmd.Parameters.AddWithValue("@idKategorija", idKategorija);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Izvršava SQL komandu za brisanje
                    return true; // Uspešno obrisano
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom brisanja kategorije: " + ex.Message);
                    return false; // Greška prilikom brisanja
                }
            }
        }

       
    }
}
