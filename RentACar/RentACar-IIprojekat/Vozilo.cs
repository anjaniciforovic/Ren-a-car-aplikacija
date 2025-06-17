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
    internal class Vozilo
    {
            // Connection string za povezivanje sa bazom podataka
            private static string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\uSER\source\repos\RentACar-IIprojekat\RentACar-IIprojekat\RentACarDB.accdb";

        // Metoda za dohvatanje svih vozila iz baze
        public static OleDbDataReader PokupiVozila()
        {
            OleDbConnection con = new OleDbConnection(connectionString);
            string query = "SELECT vozilo.id_vozila, vozilo.id_kategorije, vozilo.naziv, vozilo.marka, vozilo.model, vozilo.godina_proizvodnje, vozilo.cena_po_satu, kategorija.naziv AS naziv_kategorije " +
               "FROM vozilo " +
               "INNER JOIN kategorija ON vozilo.id_kategorije = kategorija.id_kategorije";


            OleDbCommand cmd = new OleDbCommand(query, con);

            con.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        // Metoda za dodavanje novog vozila u bazu
        public static bool DodajVozilo(int idKategorije, string naziv, string marka, string model, int godina, double cena)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO Vozilo (id_kategorije, naziv, marka, model, godina_proizvodnje, cena_po_satu) VALUES (@idKategorije, @naziv, @marka, @model, @godina, @cena)";
                OleDbCommand cmd = new OleDbCommand(query, con);

                // Postavljanje parametara
                cmd.Parameters.AddWithValue("@idKategorije", idKategorije);
                cmd.Parameters.AddWithValue("@naziv", naziv);
                cmd.Parameters.AddWithValue("@marka", marka);
                cmd.Parameters.AddWithValue("@model", model);
                cmd.Parameters.AddWithValue("@godina", godina);
                cmd.Parameters.AddWithValue("@cena", cena);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Izvršava SQL komandu za ubacivanje
                    return true; // Uspešno dodato
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom dodavanja vozila: " + ex.Message);
                    return false; // Greška prilikom dodavanja
                }
            }
        }

        // Metoda za ažuriranje podataka o postojećem vozilu
        public static bool UpdateVozilo(int idVozilo, int idKategorije, string naziv, string marka, string model, int godina, double cena)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                string query = "UPDATE Vozilo SET id_kategorije = @idKategorije, naziv = @naziv, marka = @marka, model = @model, godina_proizvodnje = @godina, cena_po_satu = @cena WHERE id_vozila = @idVozilo";
                OleDbCommand cmd = new OleDbCommand(query, con);

                // Postavljanje parametara
                cmd.Parameters.AddWithValue("@idKategorije", idKategorije);
                cmd.Parameters.AddWithValue("@naziv", naziv);
                cmd.Parameters.AddWithValue("@marka", marka);
                cmd.Parameters.AddWithValue("@model", model);
                cmd.Parameters.AddWithValue("@godina", godina);
                cmd.Parameters.AddWithValue("@cena", cena);
                cmd.Parameters.AddWithValue("@idVozilo", idVozilo);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Izvršava SQL komandu za ažuriranje
                    return true; // Uspešno ažurirano
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom ažuriranja vozila: " + ex.Message);
                    return false; // Greška prilikom ažuriranja
                }
            }
        }

        // Metoda za brisanje vozila iz baze
        public static bool DeleteVozilo(int idVozilo)
        {
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                string query = "DELETE FROM Vozilo WHERE id_vozila = @idVozilo";
                OleDbCommand cmd = new OleDbCommand(query, con);

                // Postavljanje parametara
                cmd.Parameters.AddWithValue("@idVozilo", idVozilo);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Izvršava SQL komandu za brisanje
                    return true; // Uspešno obrisano
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška prilikom brisanja vozila: " + ex.Message);
                    return false; // Greška prilikom brisanja
                }
            }
        }
    }
}
