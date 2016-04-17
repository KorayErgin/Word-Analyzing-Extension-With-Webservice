using net.zemberek.erisim;
using net.zemberek.yapi;
using net.zemberek.tr.yapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Data.SqlClient;

namespace ZemberekAnaliz
{
    /// <summary>
    /// Summary description for ZemberekAnaliz
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class ZemberekAnaliz : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string HelloWorld()
        {

            return "aa World";
        }

        [WebMethod]
        [ScriptMethod]
        public string get_post(string dizi)
        {
            Zemberek zemberek = new Zemberek(new TurkiyeTurkcesi());
            string metin = " ";
            dizi = stop_Word(dizi);
            string[] metin_Kelimeleri = dizi.Split(' ');



            foreach (string item in metin_Kelimeleri)
            {

                Kelime[] cozumler = zemberek.kelimeCozumle(item);
                Kelime kelime1 = new Kelime();
                try
                {
                    kelime1 = cozumler[0];
                }
                catch { };


                if (zemberek.kelimeDenetle(item) && kelime1.kok().tip().ToString() != "FIIL")
                    metin += govde(item) + " ";
                else
                    metin += item + " ";
            }
            string yeni;
            string[] yuksekFrekansKelimeler = enYuksekFrekans(metin).Split(' ');
            for (int i = 0; i < yuksekFrekansKelimeler.Length - 1; i++)
            {
                yeni = " <strong>" + yuksekFrekansKelimeler[i] + "</strong> ";
                metin = metin.Replace(yuksekFrekansKelimeler[i].ToString(), yeni);
            }

            return metin;
        }

        [WebMethod]
        [ScriptMethod]
        public string govde(string giris)
        {
            Zemberek zemberek = new Zemberek(new TurkiyeTurkcesi());



            Kelime[] cozumler = zemberek.kelimeCozumle(giris);
            Kelime kelime1 = new Kelime();
            try
            {
                kelime1 = cozumler[0];
            }
            catch (Exception)
            {
                return giris;
            }

            net.zemberek.yapi.ek.Ek[] ekler = kelime1.ekDizisi();

            IList<net.zemberek.yapi.ek.Ek> yeni_ekler = kelime1.ekler();
            int j = 0;
            for (int i = 1; i < ekler.Length; i++)
            {


                Boolean c = true;
                if ((ekler[i].ToString().Contains("ISIM_DONUSUM_LES")) ||
                   (ekler[i].ToString().Contains("ISIM_BULUNMA_LI")) ||
                   (ekler[i].ToString().Contains("ISIM_BULUNMA_LIK")) ||
                   (ekler[i].ToString().Contains("ISIM_ILGI_CI")) ||
                   (ekler[i].ToString().Contains("ISIM_YOKLUK_SIZ")))
                    c = false;



                if (c)
                {
                    yeni_ekler.Remove(ekler[i]);
                    // j++;

                }
                else
                {

                    j++;

                }

            }
            string kelimeson = "";
            if (j > 0)
            {
                kelimeson = zemberek.kelimeUret(kelime1.kok(), yeni_ekler.ToList());
                //kelimeson=kelime1.kok()+yeni_ekler[0].

            }
            else
            {
                kelimeson = kelime1.kok().icerik();

            }

            // if(kelime1.kok().tip().ToString()=="ISIM")
            if (kelime1.kok().tip().ToString() == "FIIL")
                return giris;
           // veriTabanı(kelimeson.ToString());
            return (kelimeson.ToString());


        }

        public string stop_Word(string gelenMetin)
        {
            Zemberek zemberek = new Zemberek(new TurkiyeTurkcesi());
            string[] words = gelenMetin.Split(' ');

            String[] TURKISH_STOP_WORDS = { "acaba", "ama", "ancak", "artık", "asla", "bir", "aslında", "az", "bana", "bazen", "bazı", "bazıları", "bazısı", "belki", "ben", "beni", "benim", "beş", "bile", "bir", "birçoğu", "birçok", "birçokları", "biri", "birisi", "birkaç", "birkaçı", "birşey", "birşeyi", "biz", "bize", "bizi", "bizim", "böyle", "böylece", "bu", "buna", "bunda", "bundan", "bunu", "bunun", "burada", "bütün", "çoğu", "çoğuna", "çoğunu", "çok", "çünkü", "da", "daha", "de", "değil", "demek", "diğer", "diğeri", "diğerleri", "diye", "dolayı", "elbette", "en", "fakat", "falan", "felan", "filan", "gene", "gibi", "hangi", "hangisi", "hani", "hatta", "hem", "henüz", "hep", "hepsi", "hepsine", "hepsini", "her", "herkes", "herkese", "herkesi", "hiç", "hiçkimse", "hiçbiri", "hiçbirine", "hiçbirini", "için", "içinde", "ile", "ise", "işte", "kaç", "kadar", "kendi", "kendine", "kendini", "ki", "kim", "kime", "kimi", "kimin", "kimisi", "madem", "mı", "mi", "mu", "mü", "nasıl", "ne", "neden", "nedir", "nerde", "nereden", "nereye", "nesi", "neyse", "niçin", "niye", "ona", "ondan", "onlar", "onlara", "onlardan", "onların", "onu", "onun", "orada", "oysa", "oysaki", "öbürü", "ön", "önce", "ötürü", "öyle", "sana", "sen", "senden", "seni", "senin", "siz", "sizden", "size", "sizi", "sizin", "son", "sonra", "şayet", "şey", "şimdi", "şöyle", "şu", "şuna", "şunda", "şundan", "şunlar", "şunu", "şunun", "tabi", "tamam", "tüm", "tümü", "üzere", "var", "ve", "veya", "veyahut", "ya", "yani", "yerine", "yine", "yoksa", "zaten", "zira" };
            List<String> liste = TURKISH_STOP_WORDS.ToList();

            string metin = "";
            foreach (string okunan in words)
            {

                if (!liste.Contains(okunan))
                {
                    string k = okunan;
                    metin += k.ToString() + " ";
                }
            }

            return metin;

        }
        [WebMethod]
        [ScriptMethod]
        public void veriTabanı(string kelime)
        {
            int frekans = 1;
            bool kontrol = false;
            SqlDataReader reader;
            SqlConnection Baglanti = new SqlConnection();
            string BaglantiAdresi = "Data Source=.;Initial Catalog=zemberekAnaliz;Integrated Security=True";
            Baglanti.ConnectionString = BaglantiAdresi;
            Baglanti.Open();
            SqlCommand cmd;




            cmd = new SqlCommand("Select frekans from [zemberekAnaliz].[dbo].[Tbl_Kelime_Frekans] where Kelime=@kelime ", Baglanti);
            cmd.Parameters.AddWithValue("@kelime", kelime);
            reader = cmd.ExecuteReader();

            cmd.Parameters.Clear();

            while (reader.Read())
            {
                frekans = reader.GetInt32(0);
                kontrol = true;
            }

            Baglanti.Close();
            Baglanti.Open();
            if (kontrol == true)
            {

                cmd = new SqlCommand("UPDATE [zemberekAnaliz].[dbo].[Tbl_Kelime_Frekans] SET Frekans =@frekans WHERE Kelime =@kelime; ", Baglanti);
                cmd.Parameters.AddWithValue("@kelime", kelime);
                cmd.Parameters.AddWithValue("@frekans", frekans + 1);
                cmd.ExecuteNonQuery();

            }

            if (kontrol == false)
            {
                cmd = new SqlCommand("INSERT INTO Tbl_Kelime_Frekans (Kelime,Frekans) VALUES(@kelime, @frekans)", Baglanti);
                cmd.Parameters.AddWithValue("@kelime", kelime);
                cmd.Parameters.AddWithValue("@frekans", frekans);
                cmd.ExecuteNonQuery();
            }

            Baglanti.Close();

        }

        [WebMethod]
        [ScriptMethod]
        public string enYuksekFrekans(string metin)
        {
            int k = 0;
            string[] kelimeler = metin.Split(' ');
            string yuksekFrekans_kelimeler = "";

            SqlDataReader reader;
            SqlConnection Baglanti = new SqlConnection();
            string BaglantiAdresi = "Data Source=.;Initial Catalog=zemberekAnaliz;Integrated Security=True";
            Baglanti.ConnectionString = BaglantiAdresi;
            Baglanti.Open();
            SqlCommand cmd;




            cmd = new SqlCommand("  SELECT Kelime  FROM[zemberekAnaliz].[dbo].[Tbl_Kelime_Frekans]  ORDER BY Frekans desc ", Baglanti);
            reader = cmd.ExecuteReader();

            cmd.Parameters.Clear();

            while (reader.Read())
            {
                for (int i = 0; i < kelimeler.Length; i++)
                {

                    if (kelimeler[i] == reader.GetString(0))
                    {
                        yuksekFrekans_kelimeler += kelimeler[i] + " ";
                        k++;
                        break;
                    }
                }
                if (k == 5)
                    break;
            }

            Baglanti.Close();


            return yuksekFrekans_kelimeler;
        }

        [WebMethod]
        [ScriptMethod]
        public void veritabani_Sil()
        {
            SqlConnection Baglanti = new SqlConnection();
            string BaglantiAdresi = "Data Source=.;Initial Catalog=zemberekAnaliz;Integrated Security=True";
            Baglanti.ConnectionString = BaglantiAdresi;
            Baglanti.Open();
            SqlCommand cmd;
            cmd = new SqlCommand("    DELETE FROM [zemberekAnaliz].[dbo].[Tbl_Kelime_Frekans] ", Baglanti);
            cmd.ExecuteNonQuery();
            Baglanti.Close();

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string top5_frekans()
        {
            string top5_kelime_frekans = "";
            SqlDataReader reader;
            SqlConnection Baglanti = new SqlConnection();
            string BaglantiAdresi = "Data Source=.;Initial Catalog=zemberekAnaliz;Integrated Security=True";
            Baglanti.ConnectionString = BaglantiAdresi;
            Baglanti.Open();
            SqlCommand cmd;
            cmd = new SqlCommand("      SELECT top 5 [Kelime]   ,[Frekans]  FROM[zemberekAnaliz].[dbo].[Tbl_Kelime_Frekans]  ORDER BY Frekans desc ", Baglanti);
            reader = cmd.ExecuteReader();
            string kelime;
            while (reader.Read())
            {
                kelime = " " + reader.GetString(0) + " | " + reader.GetInt32(1) + " _";
                for (; kelime.Length < 30;)
                {
                    kelime += "_";
                }
                top5_kelime_frekans += kelime;
            }
            Baglanti.Close();

            return top5_kelime_frekans;

        }

    }
}

