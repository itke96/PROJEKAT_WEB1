using PROJEKAT_WEB1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PROJEKAT_WEB1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start()
        {
            string temp;
            string putanja = stringZaKonekciju();


            Dictionary<string, Administrator> administratori = new Dictionary<string, Administrator>();

            if(!File.Exists(putanja + "Administratori.txt"))
            {
                File.Create(putanja + "Administratori.txt");
            }

            StreamReader fileAdministratori = new StreamReader(putanja + "Administratori.txt");
            while((temp = fileAdministratori.ReadLine()) != null)
            {
                string[] niz = temp.Split(' ');
                Administrator administrator = new Administrator();
                administrator.korisnickoIme = niz[0];
                administrator.ime = niz[1];
                administrator.prezime = niz[2];
                administrator.lozinka = niz[3];
                if(niz[4].Equals("MUSKI"))
                {
                    administrator.pol = Pol.MUSKI;
                }
                else
                {
                    administrator.pol = Pol.ZENSKI;
                }
                administrator.uloga = Uloga.ADMINISTRATOR;
                administratori.Add(administrator.korisnickoIme, administrator);
            }

            HttpContext.Current.Session["Administratori"] = administratori;
            fileAdministratori.Close();



            Dictionary<string, Domacin> domacini = new Dictionary<string, Domacin>();

            if(!File.Exists(putanja + "Domacini.txt"))
            {
                File.Create(putanja + "Domacini.txt");
            }

            StreamReader fileDomacini = new StreamReader(putanja + "Domacini.txt");
            while((temp = fileDomacini.ReadLine()) != null)
            {
                string[] niz = temp.Split(' ');
                Domacin domacin = new Domacin();
                domacin.korisnickoIme = niz[0];
                domacin.ime = niz[1];
                domacin.prezime = niz[2];
                domacin.lozinka = niz[3];
                if(niz[4].Equals("MUSKI"))
                {
                    domacin.pol = Pol.MUSKI;
                }
                else
                {
                    domacin.pol = Pol.ZENSKI;
                }
                domacin.uloga = Uloga.DOMACIN;
                domacini.Add(domacin.korisnickoIme, domacin);
            }

            HttpContext.Current.Session["Domacini"] = domacini;
            fileDomacini.Close();



            Dictionary<string, Gost> gosti = new Dictionary<string, Gost>();

            if(!File.Exists(putanja + "Gosti.txt"))
            {
                File.Create(putanja + "Gosti.txt");
            }

            StreamReader fileGosti = new StreamReader(putanja + "Gosti.txt");
            while((temp = fileGosti.ReadLine()) != null)
            {
                string[] niz = temp.Split(' ');
                Gost gost = new Gost();
                gost.korisnickoIme = niz[0];
                gost.ime = niz[1];
                gost.prezime = niz[2];
                gost.lozinka = niz[3];
                if(niz[4].Equals("MUSKI"))
                {
                    gost.pol = Pol.MUSKI;
                }
                else
                {
                    gost.pol = Pol.ZENSKI;
                }
                gost.uloga = Uloga.GOST;
                gosti.Add(gost.korisnickoIme, gost);
            }

            HttpContext.Current.Session["Gosti"] = gosti;
            fileGosti.Close();



            List<SadrzajApartmana> sadrzajApartmana = new List<SadrzajApartmana>();

            if(!File.Exists(putanja + "SadrzajApartmana.txt"))
            {
                File.Create(putanja + "SadrzajApartmana.txt");
            }

            StreamReader fileSadrzajApartmana = new StreamReader(putanja + "SadrzajApartmana.txt");
            while((temp = fileSadrzajApartmana.ReadLine()) != null)
            {
                string[] nizSadrzajApartmana = temp.Split(':');
                SadrzajApartmana sadApa = new SadrzajApartmana();
                sadApa.id = nizSadrzajApartmana[0];
                sadApa.naziv = nizSadrzajApartmana[1];
                sadrzajApartmana.Add(sadApa);
            }

            HttpContext.Current.Session["SadrzajApartmana"] = sadrzajApartmana;
            fileSadrzajApartmana.Close();



            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();

            if(!File.Exists(putanja + "Apartmani.txt"))
            {
                File.Create(putanja + "Apartmani.txt");
            }

            StreamReader fileApartmani = new StreamReader(putanja + "Apartmani.txt");
            while((temp = fileApartmani.ReadLine()) != null)
            {
                string[] niz = temp.Split('|');
                string[] niz1 = niz[0].Split('&');
                Apartman apartman = new Apartman();

                if(niz1[0].Equals("CEO_APARTMAN"))
                {
                    apartman.tip = TipApartmana.CEO_APARTMAN;
                }
                else
                {
                    apartman.tip = TipApartmana.SOBA;
                }
                apartman.brojSoba = Int32.Parse(niz1[1]);
                apartman.brojGostiju = Int32.Parse(niz1[2]);
                foreach(var d in (Dictionary<string, Domacin>)Session["Domacini"])
                {
                    if(niz1[3] == d.Value.korisnickoIme)
                    {
                        apartman.domacin = d.Value;
                        break;
                    }
                }
                apartman.cenaNocenja = Double.Parse(niz1[4]);
                apartman.vremeZaPrijavu = niz1[5];
                apartman.vremeZaOdjavu = niz1[6];
                if(niz1[7].Equals("AKTIVNO"))
                {
                    apartman.status = Status.AKTIVNO;
                }
                else
                {
                    apartman.status = Status.NEAKTIVNO;
                }

                string[] niz2 = niz[1].Split('&');
                Lokacija l = new Lokacija();
                Adresa a = new Adresa();
    
                l.geografskaDuzina = Double.Parse(niz2[0]);
                l.geografskaSirina = Double.Parse(niz2[1]);
                a.nazivUlice = niz2[2];
                a.broj = niz2[3];
                a.nazivMesta = niz2[4];
                a.postanskiBroj = niz2[5];
                l.adresa = a;

                apartman.lokacija = l;

                apartman.datumiZaIzdavanje = new List<DateTime>();
                string[] nizDatumiZaIzdavanje = niz[2].Split('&');
                foreach(var temp1 in nizDatumiZaIzdavanje)
                {
                    apartman.datumiZaIzdavanje.Add(DateTime.Parse(temp1));
                }

                apartman.dostupnost = new List<DateTime>();
                string[] nizDatumiDostupnost = niz[3].Split('&');
                foreach(var temp2 in nizDatumiDostupnost)
                {
                    apartman.dostupnost.Add(DateTime.Parse(temp2));
                }

                apartman.komentar = new List<Komentar>();
                string[] nizKomentara = niz[4].Split('&');
                if(!nizKomentara.Contains("EMPTY-EMPTY"))
                {
                    foreach(var temp3 in nizKomentara)
                    {
                        string[] kom = temp3.Split('-');
                        Komentar komentar = new Komentar();
                        komentar.ocena = Int32.Parse(kom[0]);
                        komentar.tekst = kom[1];

                        apartman.komentar.Add(komentar);
                    }
                }

                apartman.sadrzajApartmana = new List<SadrzajApartmana>();
                string[] nizSadrzaja = niz[6].Split('&');
                if(!nizSadrzaja.Contains("EMPTY-EMPTY"))
                {
                    foreach (var temp4 in nizSadrzaja)
                    {
                        string[] sad = temp4.Split('-');
                        SadrzajApartmana sadrzaj = new SadrzajApartmana();
                        sadrzaj.id = sad[0];
                        sadrzaj.naziv = sad[1];

                        apartman.sadrzajApartmana.Add(sadrzaj);
                    }
                }
                
                string[] nizSlike = niz[5].Split('&');
                apartman.slike = new List<string>();
                foreach (var temp5 in nizSlike)
                {
                    apartman.slike.Add(temp5);
                }

                apartmani.Add(@apartman.lokacija.adresa.nazivMesta + " " + apartman.lokacija.adresa.nazivUlice + " " + apartman.lokacija.adresa.broj, apartman);
            }

            HttpContext.Current.Session["Apartmani"] = apartmani;
            fileApartmani.Close();
        }

        public string stringZaKonekciju()
        {
            string str = "";
            string[] niz = System.AppDomain.CurrentDomain.BaseDirectory.Split('\\');
            for(int i = 0; i < niz.Length - 2; i++)
            {
                str += @"" + niz[i] + "/";
            }

            return str;
        }
    }
}