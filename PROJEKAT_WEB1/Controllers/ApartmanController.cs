using PROJEKAT_WEB1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PROJEKAT_WEB1.Controllers
{
    public class ApartmanController : Controller
    {
        // GET: Apartman
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GoToHomeAdmin()
        {
            return View("HomepageAdministrator");
        }

        public ActionResult GoToHomeDomacin()
        {
            return View("HomepageDomacin");
        }

        public ActionResult GoToHomeGost()
        {
            return View("HomepageGost");
        }

        public ActionResult IdiNaRegistracijaKorisnika()
        {
            return View("RegistracijaKorisnika");
        }

        public ActionResult IdiNaUlogujSe()
        {
            return View("LogInKorisnika");
        }

        private Dictionary<string, Gost> getGosti
        {
            get
            {
                return (Dictionary<string, Gost>)Session["Gosti"];
            }
        }

        private Dictionary<string, Domacin> getDomacini
        {
            get
            {
                return (Dictionary<string, Domacin>)Session["Domacini"];
            }
        }

        private Dictionary<string, Administrator> getAdministratori
        {
            get
            {
                return (Dictionary<string, Administrator>)Session["Administratori"];
            }
        }

        private Dictionary<string, Apartman> getApartmani
        {
            get
            {
                return (Dictionary<string, Apartman>)Session["Apartmani"];
            }
        }

        private List<SadrzajApartmana> getSadrzajApartmana
        {
            get
            {
                return (List<SadrzajApartmana>)Session["SadrzajApartmana"];
            }
        }

        public string stringZaKonekciju()
        {
            string str = "";
            string[] niz = System.AppDomain.CurrentDomain.BaseDirectory.Split('\\');
            for (int i = 0; i < niz.Length - 2; i++)
            {
                str += @"" + niz[i] + "/";
            }

            return str;
        }

        public ActionResult GoToKreiranjeDomacina()
        {
            return View("KreiranjeDomacina");
        }

        [HttpPost]
        public ActionResult KreiranjeDomacina(Domacin d)
        {
            Domacin domacin = new Domacin();
            domacin.korisnickoIme = d.korisnickoIme;
            domacin.ime = d.ime;
            domacin.prezime = d.prezime;
            domacin.lozinka = d.lozinka;
            domacin.pol = d.pol;
            domacin.uloga = Uloga.DOMACIN;
            if(getGosti.ContainsKey(d.korisnickoIme))
            {
                ViewBag.domacin = domacin;
                return View("KreiranjeDomacinaError");
            }

            getDomacini.Add(domacin.korisnickoIme, domacin);
            UpisiDomacina();
            return View("HomepageAdministrator");
        }

        [HttpPost]
        public ActionResult RegistracijaKorisnika(Korisnik k)
        {
            k.uloga = Uloga.GOST;
            if(getGosti.ContainsKey(k.korisnickoIme) || getAdministratori.ContainsKey(k.korisnickoIme) || getDomacini.ContainsKey(k.korisnickoIme))
            {
                ViewBag.korisnik = k;
                return View("RegistracijaKorisnikaError");
            }
            else
            {
                Gost gost = new Gost();
                gost.uloga = Uloga.GOST;
                gost.prezime = k.prezime;
                gost.pol = k.pol;
                gost.lozinka = k.lozinka;
                gost.korisnickoIme = k.korisnickoIme;
                gost.ime = k.ime;
                getGosti.Add(gost.korisnickoIme, gost);
            }

            UpisiGosta();
            return View("Index");
        }

        [HttpPost]
        public ActionResult LogInKorisnika(string korisnickoIme, string lozinka)
        {
            if(!getGosti.ContainsKey(korisnickoIme))
            {
                if(getAdministratori.ContainsKey(korisnickoIme))
                {
                    if(!getAdministratori[korisnickoIme].lozinka.Equals(lozinka))
                    {
                        return View("LozinkaError");
                    }
                    else
                    {
                        Session["Ulogovan"] = getAdministratori[korisnickoIme];
                    }
                    return View("HomepageAdministrator");
                }
                if(getDomacini.ContainsKey(korisnickoIme))
                {
                    if(!getDomacini[korisnickoIme].lozinka.Equals(lozinka))
                    {
                        return View("LozinkaError");
                    }
                    else
                    {
                        Session["Ulogovan"] = getDomacini[korisnickoIme];
                    }
                    return View("HomepageDomacin");
                }

                return View("KorisnickoImeError");
            }

            if(!getGosti[korisnickoIme].lozinka.Equals(lozinka))
            {
                return View("LozinkaError");
            }
            else
            {
                Session["Ulogovan"] = getGosti[korisnickoIme];
            }
            return View("HomepageGost");
        }

        public ActionResult GoToIzmenaGost()
        {
            return View("IzmenaGost");
        }

        public ActionResult GoToIzmenaDomacin()
        {
            return View("IzmenaDomacin");
        }

        public ActionResult GoToIzmenaAdmin()
        {
            return View("IzmenaAdmin");
        }

        public void UpisiAdministratora()
        {
            string putanja = stringZaKonekciju();
            System.IO.StreamWriter upisAdministrator = new System.IO.StreamWriter(putanja + "Administratori.txt");
            foreach(KeyValuePair<string, Administrator> a in getAdministratori)
            {
                upisAdministrator.WriteLine(a.Value.korisnickoIme + " " + a.Value.ime + " " + a.Value.prezime + " " + a.Value.lozinka + " " + a.Value.pol);
            }
            upisAdministrator.Close();
        }

        private void UpisiDomacina()
        {
            string putanja = stringZaKonekciju();
            System.IO.StreamWriter upisDomacin = new System.IO.StreamWriter(putanja + "Domacini.txt");
            foreach(KeyValuePair<string, Domacin> d in getDomacini)
            {
                upisDomacin.WriteLine(d.Value.korisnickoIme + " " + d.Value.ime + " " + d.Value.prezime + " " + d.Value.lozinka + " " + d.Value.pol);
            }
            upisDomacin.Close();
        }

        private void UpisiGosta()
        {
            string putanja = stringZaKonekciju();
            System.IO.StreamWriter upisKorisnik = new System.IO.StreamWriter(putanja + "Gosti.txt");
            foreach(KeyValuePair<string, Gost> g in getGosti)
            {
                upisKorisnik.WriteLine(g.Value.korisnickoIme + " " + g.Value.ime + " " + g.Value.prezime + " " + g.Value.lozinka + " " + g.Value.pol);
            }
            upisKorisnik.Close();
        }

        public ActionResult IzmenaGost(Korisnik k)
        {
            if(!((Korisnik)Session["Ulogovan"]).korisnickoIme.Equals(k.korisnickoIme) && (getGosti.ContainsKey(k.korisnickoIme) || getAdministratori.ContainsKey(k.korisnickoIme) || getDomacini.ContainsKey(k.korisnickoIme)))
            {
                ViewBag.korisnik = k;
                return View("SignUpError2");
            }
            if(!((Korisnik)Session["Ulogovan"]).korisnickoIme.Equals(k.korisnickoIme))
            {
                getGosti.Remove(((Korisnik)Session["Ulogovan"]).korisnickoIme);
            }
            k.uloga = Uloga.GOST;
            Gost gost = new Gost();
            gost.uloga = Uloga.GOST;
            gost.ime = k.ime;
            gost.prezime = k.prezime;
            gost.korisnickoIme = k.korisnickoIme;
            gost.lozinka = k.lozinka;
            gost.pol = k.pol;

            getGosti[gost.korisnickoIme] = gost;
            Session["Ulogovan"] = getGosti[gost.korisnickoIme];

            UpisiGosta();
            return View("HomepageGost");
        }

        public ActionResult IzmenaAdmin(Korisnik k)
        {
            if(!((Korisnik)Session["Ulogovan"]).korisnickoIme.Equals(k.korisnickoIme) && (getGosti.ContainsKey(k.korisnickoIme) || getAdministratori.ContainsKey(k.korisnickoIme) || getDomacini.ContainsKey(k.korisnickoIme)))
            {
                ViewBag.admin = k;
                return View("SignUpError");
            }
            if(!((Korisnik)Session["Ulogovan"]).korisnickoIme.Equals(k.korisnickoIme))
            {
                getAdministratori.Remove(((Korisnik)Session["Ulogovan"]).korisnickoIme);
            }
            k.uloga = Uloga.ADMINISTRATOR;
            Administrator admin = new Administrator();
            admin.uloga = Uloga.ADMINISTRATOR;
            admin.ime = k.ime;
            admin.prezime = k.prezime;
            admin.korisnickoIme = k.korisnickoIme;
            admin.lozinka = k.lozinka;
            admin.pol = k.pol;

            getAdministratori[admin.korisnickoIme] = admin;
            Session["Ulogovan"] = getAdministratori[admin.korisnickoIme];

            UpisiAdministratora();
            return View("HomepageAdministrator");
        }

        public ActionResult IzmenaDomacin(Korisnik k)
        {
            if(!((Korisnik)Session["Ulogovan"]).korisnickoIme.Equals(k.korisnickoIme) && (getGosti.ContainsKey(k.korisnickoIme) || getAdministratori.ContainsKey(k.korisnickoIme) || getDomacini.ContainsKey(k.korisnickoIme)))
            {
                ViewBag.domacin = k;
                return View("SingUpError3");
            }
            if(!((Korisnik)Session["Ulogovan"]).korisnickoIme.Equals(k.korisnickoIme))
            {
                getDomacini.Remove(((Korisnik)Session["Ulogovan"]).korisnickoIme);
            }
            k.uloga = Uloga.DOMACIN;
            Domacin domacin = new Domacin();
            domacin.uloga = Uloga.DOMACIN;
            domacin.ime = k.ime;
            domacin.prezime = k.prezime;
            domacin.korisnickoIme = k.korisnickoIme;
            domacin.lozinka = k.lozinka;
            domacin.pol = k.pol;

            getDomacini[domacin.korisnickoIme] = domacin;
            Session["Ulogovan"] = getDomacini[domacin.korisnickoIme];

            UpisiDomacina();
            return View("HomepageDomacin");
        }

        public ActionResult PregledRegKorisnika()
        {
            return View("PregledRegKorisnika");
        }

        public ActionResult Odjava()
        {
            Session["Ulogovan"] = null;
            return View("Index");
        }

        public ActionResult GoToPregledSvihApartmanaDomacina()
        {
            return View("PregledSvihApartmanaDomacina");
        }

        public ActionResult GoToKreirajApartman()
        {
            return View("KreirajApartman");
        }

        public List<DateTime> GetAllDates(DateTime startingDate, DateTime endingDate)
        {
            List<DateTime> allDates = new List<DateTime>();

            int starting = startingDate.Day;
            int ending = endingDate.Day;

            for (DateTime date = startingDate; date <= endingDate; date = date.AddDays(1))
            {
                allDates.Add(date);
            }
            return allDates;
        }

        [HttpPost]
        public ActionResult KreiranjeApartmana(string brojSoba, DateTime datumDostupnostiOd, DateTime datumDostupnostiDo, Apartman a, Lokacija l, Adresa adr, List<SadrzajApartmana> sA)
        {
            if(Request.Form["submit"] != null)
            {
                if(Session["ApartmanTemp"] == null)
                {
                    Session["ApartmanTemp"] = new Apartman();
                    Apartman apartmanTemp = new Apartman();
                    apartmanTemp.brojGostiju = a.brojGostiju;
                    apartmanTemp.brojSoba = a.brojSoba;
                    apartmanTemp.cenaNocenja = a.cenaNocenja;
                    apartmanTemp.datumiZaIzdavanje = new List<DateTime>();
                    foreach(var temp in GetAllDates(datumDostupnostiOd, datumDostupnostiDo))
                    {
                        apartmanTemp.datumiZaIzdavanje.Add(temp);
                    }
                    apartmanTemp.domacin = (Domacin)Session["Ulogovan"];
                    apartmanTemp.dostupnost = new List<DateTime>();
                    foreach(var temp in GetAllDates(datumDostupnostiOd, datumDostupnostiDo))
                    {
                        apartmanTemp.dostupnost.Add(temp);
                    }
                    apartmanTemp.lokacija = new Lokacija();
                    apartmanTemp.lokacija.geografskaDuzina = l.geografskaDuzina;
                    apartmanTemp.lokacija.geografskaSirina = l.geografskaSirina;
                    apartmanTemp.lokacija.adresa = new Adresa();
                    apartmanTemp.lokacija.adresa = adr;
                    apartmanTemp.vremeZaPrijavu = Request.Form["pocetakp"].ToString();
                    apartmanTemp.vremeZaOdjavu = Request.Form["krajp"].ToString();
                    apartmanTemp.tip = a.tip;
                    apartmanTemp.status = a.status;
                    apartmanTemp.komentar = new List<Komentar>();
                    apartmanTemp.sadrzajApartmana = new List<SadrzajApartmana>();

                    foreach(var temp in getSadrzajApartmana)
                    {
                        if(Request.Form[temp.id.ToString()] != null)
                        {
                            apartmanTemp.sadrzajApartmana.Add(temp);
                        }
                    }
                    apartmanTemp.domacin.apartmaniZaIzdavanje = new List<Apartman>();
                    apartmanTemp.domacin.apartmaniZaIzdavanje.Add(apartmanTemp);
                    Session["ApartmanTemp"] = apartmanTemp;
                }
            }
            
            return View("KreirajApartmanSlika");
        }

        [HttpPost]
        public ActionResult KreirajApartmanSlika()
        {
            var temp = Request.Form["slike"];
            string[] nizSlika = temp.Split(',');

            var apartman = (Apartman)Session["ApartmanTemp"];
            apartman.slike = new List<string>();
            foreach(var slika in nizSlika)
            {
                apartman.slike.Add(slika);
            }
            getApartmani.Add(@apartman.lokacija.adresa.nazivMesta + " " + apartman.lokacija.adresa.nazivUlice + " " + apartman.lokacija.adresa.broj, apartman);

            UpisiApartman();
            Session["ApartmanTemp"] = null;
            return View("PregledSvihApartmanaDomacina");
        }

        public void UpisiApartman()
        {
            string putanja = stringZaKonekciju();
            System.IO.StreamWriter upisApartmani = new System.IO.StreamWriter(putanja + "Apartmani.txt");
            foreach(var a in getApartmani)
            {
                upisApartmani.Write(a.Value.tip + "&");
                upisApartmani.Write(a.Value.brojSoba + "&");
                upisApartmani.Write(a.Value.brojGostiju + "&");
                upisApartmani.Write(a.Value.domacin.korisnickoIme + "&");
                upisApartmani.Write(a.Value.cenaNocenja + "&");
                upisApartmani.Write(a.Value.vremeZaPrijavu + "&");
                upisApartmani.Write(a.Value.vremeZaOdjavu + "&");
                upisApartmani.Write(a.Value.status + "|");
                upisApartmani.Write(a.Value.lokacija.geografskaSirina + "&");
                upisApartmani.Write(a.Value.lokacija.geografskaDuzina + "&");
                upisApartmani.Write(a.Value.lokacija.adresa.nazivUlice + "&");
                upisApartmani.Write(a.Value.lokacija.adresa.broj + "&");
                upisApartmani.Write(a.Value.lokacija.adresa.nazivMesta + "&");
                upisApartmani.Write(a.Value.lokacija.adresa.postanskiBroj + "|");

                string datumi = "";
                foreach(var temp in a.Value.datumiZaIzdavanje)
                {
                    datumi += temp.Date + "&";
                }

                datumi = datumi.Substring(0, datumi.Length - 1);
                datumi += "|";
                upisApartmani.Write(datumi);


                string datumiDostupnost = "";
                if(a.Value.dostupnost != null)
                {
                    foreach(var temp in a.Value.dostupnost)
                    {
                        datumiDostupnost += temp + "&";
                    }
                    datumiDostupnost = datumiDostupnost.Substring(0, datumiDostupnost.Length - 1);
                    datumiDostupnost += "|";
                    upisApartmani.Write(datumiDostupnost);
                }

                string komentari = "";
                if(a.Value.komentar.Count > 0)
                {
                    foreach(var temp in a.Value.komentar)
                    {
                        komentari += temp.ocena + "-";
                        komentari += temp.tekst + "&";
                    }
                    komentari = komentari.Substring(0, komentari.Length - 1);
                    komentari += "|";
                    upisApartmani.Write(komentari);
                }
                else
                {
                    komentari += "EMPTY" + "-";
                    komentari += "EMPTY" + "|";
                    upisApartmani.Write(komentari);
                }

                string slike = "";
                if(a.Value.slike.Count > 0)
                {
                    foreach(var temp in a.Value.slike)
                    {
                        slike += temp + "&";
                    }
                    slike = slike.Substring(0, slike.Length - 1);
                    slike += "|";
                    upisApartmani.Write(slike);
                }
                else
                {
                    slike += "EMPTY" + "|";
                    upisApartmani.Write(slike);
                }

                string sadrzaj = "";
                if(a.Value.sadrzajApartmana.Count > 0)
                {
                    foreach(var temp in a.Value.sadrzajApartmana)
                    {
                        sadrzaj += temp.id + "-";
                        sadrzaj += temp.naziv + "&";
                    }
                    sadrzaj = sadrzaj.Substring(0, sadrzaj.Length - 1);
                    sadrzaj += "|";
                    upisApartmani.Write(sadrzaj);
                }
                else
                {
                    sadrzaj += "EMPTY" + "-";
                    sadrzaj += "EMPTY" + "|";
                    upisApartmani.Write(sadrzaj);
                }
            }

            upisApartmani.Close();
        }

        [HttpPost]
        public ActionResult ObrisiApartmanAdmin()
        {
            var temp = Request.Form["obrisi"].ToString();
            foreach(var temp1 in getApartmani)
            {
                if(temp == temp1.Key.ToString())
                {
                    getApartmani.Remove(temp1.Key);
                    UpisiApartman();
                    return View("PregledSvihApartmanaAdmin");
                }
            }

            return View("PregledSvihApartmanaAdmin");
        }

        [HttpPost]
        public ActionResult ObrisiApartmanDomacin()
        {
            var temp = Request.Form["obrisi"].ToString();
            foreach(var temp1 in getApartmani)
            {
                if(temp == temp1.Key.ToString())
                {
                    getApartmani.Remove(temp1.Key);
                    UpisiApartman();
                    return View("PregledSvihApartmanaDomacina");
                }
            }

            return View("PregledSvihApartmanaDomacina");
        }

        [HttpPost]
        public ActionResult ObrisiSadrzajApartmanaAdmin()
        {
            var temp = Request.Form["obrisi"].ToString();
            foreach(var temp1 in getSadrzajApartmana)
            {
                if(temp == temp1.id.ToString())
                {
                    getSadrzajApartmana.Remove(temp1);
                    UpisiApartman();
                    return View("PregledSvihSadrzajaZaApartmane");
                }
            }

            return View("PregledSvihApartmanaDomacin");
        }

        [HttpPost]
        public ActionResult GoToModifikujApartmanAdmin()
        {
            var temp = Request.Form["modifikuj"].ToString();
            foreach(var temp2 in getApartmani)
            {
                if (temp == temp2.Key.ToString())
                {
                    ViewBag.s = new Apartman();
                    ViewBag.s = temp2.Value;
                    return View("IzmenaApartmanaAdmin");
                }
            }

            return View("IzmenaApartmanaAdmin");
        }

        [HttpPost]
        public ActionResult GoToModifikujSadrzajApartmanaAdmin()
        {
            var temp = Request.Form["modifikuj"].ToString();
            foreach (var temp2 in getSadrzajApartmana)
            {
                if (temp == temp2.id.ToString())
                {
                    ViewBag.s = new SadrzajApartmana();
                    ViewBag.s = temp2;
                    return View("IzmenaSadrzajaApartmanaAdmin");
                }
            }

            return View("IzmenaSadrzajaApartmanaAdmin");
        }

        [HttpPost]
        public ActionResult GoToModifikujApartmanDomacin()
        {
            var temp = Request.Form["modifikuj"].ToString();
            foreach (var temp2 in getApartmani)
            {
                if (temp == temp2.Key.ToString())
                {
                    ViewBag.s = new Apartman();
                    ViewBag.s = temp2.Value;
                    return View("IzmenaApartmanaDomacin");
                }
            }

            return View("IzmenaApartmanaDomacin");
        }

        [HttpPost]
        public ActionResult GoToPrikaziApartmanAdmin()
        {
            var temp = Request.Form["prikazi"].ToString();
            foreach (var temp2 in getApartmani)
            {
                if (temp == temp2.Key.ToString())
                { 
                    ViewBag.apaAdmin = new Apartman();
                    ViewBag.apaAdmin = temp2.Value;
                    return View("PrikaziApartmanAdmin");
                }
            }

            return View("PrikaziApartmanAdmin");
        }

        [HttpPost]
        public ActionResult GoToPrikaziApartmanNeregistrovaniKorisnici()
        {
            var temp = Request.Form["prikazi"].ToString();
            foreach (var temp2 in getApartmani)
            {
                if (temp == temp2.Key.ToString())
                {

                    ViewBag.apaIndex = new Apartman();
                    ViewBag.apaIndex = temp2.Value;
                    return View("PrikaziApartmanIndex");
                }
            }

            return View("PrikaziApartmanIndex");
        }

        [HttpPost]
        public ActionResult GoToPrikaziApartmanGost()
        {
            var temp = Request.Form["prikazi"].ToString();
            foreach (var temp2 in getApartmani)
            {
                if (temp == temp2.Key.ToString())
                {

                    ViewBag.apaGost = new Apartman();
                    ViewBag.apaGost = temp2.Value;
                    return View("PrikaziApartmanGost");
                }
            }

            return View("PrikaziApartmanGost");
        }

        [HttpPost]
        public ActionResult GoToPrikaziApartmanDomacin()
        {
            var temp = Request.Form["prikazi"].ToString();
            foreach (var temp2 in getApartmani)
            {
                if (temp == temp2.Key.ToString())
                {

                    ViewBag.apaDomacin = new Apartman();
                    ViewBag.apaDomacin = temp2.Value;
                    return View("PrikaziApartmanDomacin");
                }
            }

            return View("PrikaziApartmanDomacin");
        }

        [HttpPost]
        public ActionResult IzmenaApartmanaAdminStep1(string brojSoba, DateTime datumDostupnostiOd, DateTime datumDostupnostiDo, Apartman a, Lokacija l, Adresa adr, List<SadrzajApartmana> sA)
        {
            List<Komentar> komentari = new List<Komentar>();

            if(Request.Form["submit"] != null)
            {
                if(Session["IzmenaApartmanTemp"] == null)
                {
                    foreach (var apa in getApartmani)
                    {
                        var dd = apa.Key.ToString();
                        var sss = Request.Form["idApartmana"].ToString();
                        if(Request.Form["idApartmana"].ToString() == apa.Key.ToString())
                        {
                            foreach(var temp in apa.Value.komentar)
                            {
                                Komentar komentar = new Komentar();
                                komentar.apartman = temp.apartman;
                                komentar.gost = temp.gost;
                                komentar.ocena = temp.ocena;
                                komentar.tekst = temp.tekst;
                                komentari.Add(komentar);
                            }

                            getApartmani.Remove(apa.Key);
                            break;
                        }
                    }

                    Session["IzmenaApartmanTemp"] = new Apartman();
                    Apartman apartmanTemp = new Apartman();
                    apartmanTemp.brojGostiju = a.brojGostiju;
                    apartmanTemp.brojSoba = a.brojSoba;
                    apartmanTemp.cenaNocenja = a.cenaNocenja;

                    apartmanTemp.datumiZaIzdavanje = new List<DateTime>();
                    foreach (var temp in GetAllDates(datumDostupnostiOd, datumDostupnostiDo))
                    {
                        apartmanTemp.datumiZaIzdavanje.Add(temp);
                    }

                    apartmanTemp.domacin = getDomacini[Request.Form["domacin"].ToString()];
          
                    apartmanTemp.dostupnost = new List<DateTime>();
                    foreach (var temp in GetAllDates(datumDostupnostiOd, datumDostupnostiDo))
                    {
                        apartmanTemp.dostupnost.Add(temp);
                    }

                    apartmanTemp.lokacija = new Lokacija();
                    apartmanTemp.lokacija.geografskaDuzina = l.geografskaDuzina;
                    apartmanTemp.lokacija.geografskaSirina = l.geografskaSirina;
                    apartmanTemp.lokacija.adresa = new Adresa();
                    apartmanTemp.lokacija.adresa = adr;
                    apartmanTemp.vremeZaPrijavu = Request.Form["pocetakp"].ToString();
                    apartmanTemp.vremeZaOdjavu = Request.Form["krajp"].ToString();
                    apartmanTemp.tip = a.tip;
                    apartmanTemp.status = a.status;
                    apartmanTemp.komentar = komentari;
                    apartmanTemp.sadrzajApartmana = new List<SadrzajApartmana>();

                    foreach (var temp in getSadrzajApartmana)
                    {
                        if (Request.Form[temp.id.ToString()] != null)
                        {
                            apartmanTemp.sadrzajApartmana.Add(temp);
                        }
                    }

                    apartmanTemp.domacin.apartmaniZaIzdavanje = new List<Apartman>();
                    apartmanTemp.domacin.apartmaniZaIzdavanje.Add(apartmanTemp);
                    Session["IzmenaApartmanTemp"] = apartmanTemp;
                }
            }

            return View("IzmenaApartmanaAdminStep2");
        }

        [HttpPost]
        public ActionResult IzmenaApartmanaDomacinStep1(string brojSoba, DateTime datumDostupnostiOd, DateTime datumDostupnostiDo, Apartman a, Lokacija l, Adresa adr, List<SadrzajApartmana> sA)
        {
            List<Komentar> komentari = new List<Komentar>();

            if(Request.Form["submit"] != null)
            {
                if(Session["IzmenaApartmanTemp"] == null)
                {
                    foreach (var apa in getApartmani)
                    {
                        if (Request.Form["idApartmanaDomacin"].ToString() == apa.Key.ToString())
                        {
                            foreach (var temp in apa.Value.komentar)
                            {
                                Komentar komentar = new Komentar();
                                komentar.apartman = temp.apartman;
                                komentar.gost = temp.gost;
                                komentar.ocena = temp.ocena;
                                komentar.tekst = temp.tekst;
                                komentari.Add(komentar);
                            }

                            getApartmani.Remove(apa.Key);
                            break;
                        }
                    }

                    Session["IzmenaApartmanTemp"] = new Apartman();
                    Apartman apartmanTemp = new Apartman();
                    apartmanTemp.brojGostiju = a.brojGostiju;
                    apartmanTemp.brojSoba = a.brojSoba;
                    apartmanTemp.cenaNocenja = a.cenaNocenja;

                    apartmanTemp.datumiZaIzdavanje = new List<DateTime>();
                    foreach (var temp in GetAllDates(datumDostupnostiOd, datumDostupnostiDo))
                    {
                        apartmanTemp.datumiZaIzdavanje.Add(temp);
                    }
                    
                    apartmanTemp.domacin = getDomacini[Request.Form["domacin"].ToString()];
        
                    apartmanTemp.dostupnost = new List<DateTime>();
                    foreach (var temp in GetAllDates(datumDostupnostiOd, datumDostupnostiDo))
                    {
                        apartmanTemp.dostupnost.Add(temp);
                    }

                    apartmanTemp.lokacija = new Lokacija();
                    apartmanTemp.lokacija.geografskaDuzina = l.geografskaDuzina;
                    apartmanTemp.lokacija.geografskaSirina = l.geografskaSirina;
                    apartmanTemp.lokacija.adresa = new Adresa();
                    apartmanTemp.lokacija.adresa = adr;
                    apartmanTemp.vremeZaPrijavu = Request.Form["pocetakp"].ToString();
                    apartmanTemp.vremeZaOdjavu = Request.Form["krajp"].ToString();
                    apartmanTemp.tip = a.tip;
                    apartmanTemp.status = a.status;
                    apartmanTemp.komentar = komentari;
                    apartmanTemp.sadrzajApartmana = new List<SadrzajApartmana>();

                    foreach (var temp in getSadrzajApartmana)
                    {
                        if (Request.Form[temp.id.ToString()] != null)
                        {
                            apartmanTemp.sadrzajApartmana.Add(temp);
                        }
                    }

                    apartmanTemp.domacin.apartmaniZaIzdavanje = new List<Apartman>();
                    apartmanTemp.domacin.apartmaniZaIzdavanje.Add(apartmanTemp);
                    Session["IzmenaApartmanTemp"] = apartmanTemp;
                }
            }
            
            return View("IzmenaApartmanaDomacinStep2");
        }

        [HttpPost]
        public ActionResult IzmenaSadrzajaApartmanaAdmin(SadrzajApartmana sA)
        {
            foreach (var sad in getSadrzajApartmana)
            {
                if (Request.Form["idSadrzajaApartmana"].ToString() == sad.id.ToString())
                {
                    getSadrzajApartmana.Remove(sad);
                    break;
                }
            }

            getSadrzajApartmana.Add(sA);

            UpisiSadrzajApartmana();
            return View("PregledSvihSadrzajaZaApartmane");
        }

        public void UpisiSadrzajApartmana()
        {
            string putanja = stringZaKonekciju();
            System.IO.StreamWriter upisSadrzajApartmana = new System.IO.StreamWriter(putanja + "SadrzajApartmana.txt");
            foreach (var temp in getSadrzajApartmana)
            {
                upisSadrzajApartmana.WriteLine(temp.id + ":" + temp.naziv);
            }
            upisSadrzajApartmana.Close();
        }

        [HttpPost]
        public ActionResult IzmenaApartmanaAdminStep2()
        {
            var temp = Request.Form["slike"];
            string[] nizSlika = temp.Split(',');

            var apartman = (Apartman)Session["IzmenaApartmanTemp"];
            apartman.slike = new List<string>();
            foreach (var slika in nizSlika)
            {
                apartman.slike.Add(slika);
            }
            getApartmani.Add(@apartman.lokacija.adresa.nazivMesta + " " + apartman.lokacija.adresa.nazivUlice + " " + apartman.lokacija.adresa.broj, apartman);

            UpisiApartman();
            Session["IzmenaApartmanTemp"] = null;
            return View("PregledSvihApartmanaAdmin");
        }

        [HttpPost]
        public ActionResult IzmenaApartmanaDomacinStep2()
        {
            var temp = Request.Form["slike"];
            string[] nizSlika = temp.Split(',');

            var apartman = (Apartman)Session["IzmenaApartmanTemp"];
            apartman.slike = new List<string>();
            foreach (var slika in nizSlika)
            {
                apartman.slike.Add(slika);
            }
            getApartmani.Add(@apartman.lokacija.adresa.nazivMesta + " " + apartman.lokacija.adresa.nazivUlice + " " + apartman.lokacija.adresa.broj, apartman);

            UpisiApartman();
            Session["IzmenaApartmanTemp"] = null;
            return View("PregledSvihApartmanaDomacina");
        }

        public ActionResult GoToSviApartmaniAdmin()
        {
            return View("PregledSvihApartmanaAdmin");
        }

        public ActionResult IdiNaPregledSvihApartmanaNeregistrovaniKorisnici()
        {
            return View("PregledSvihApartmanaNeregistrovaniKorisnici");
        }

        public ActionResult IdiNaPregledSvihApartmanaGost()
        {
            return View("PregledSvihApartmanaGost");
        }

        public ActionResult GoToSviSadrzajiZaApartmane()
        {
            return View("PregledSvihSadrzajaZaApartmane");
        }

        public ActionResult GoToKreirajSadrzajZaApartman()
        {
            return View("KreirajSadrzajZaApartman");
        }

        [HttpPost]
        public ActionResult KreiranjeSadrzajaZaApartman(SadrzajApartmana sA)
        {
            foreach (var temp in getSadrzajApartmana)
            {
                if (temp.id == sA.id)
                {
                    return View("SadrzajApartmanaError");
                }
            }
            getSadrzajApartmana.Add(sA);
            UpisiSadrzajApartmana();
            return View("PregledSvihSadrzajaZaApartmane");
        }

        public ActionResult IdiNaPretragaApartmanaIndex()
        {
            return View("PretragaApartmanaIndex");
        }

        public ActionResult IdiNaPretragaApartmanaGost()
        {
            return View("PretragaApartmanaGost");
        }

        public ActionResult IdiNaPretragaApartmanaAdmin()
        {
            return View("PretragaApartmanaAdmin");
        }

        public ActionResult IdiNaPretragaApartmanaDomacin()
        {
            return View("PretragaApartmanaDomacin");
        }

        public ActionResult IdiNaPretragaKorisnikaAdmin()
        {
            return View("PretragaKorisnikaAdmin");
        }

        [HttpPost]
        public ActionResult PretragaApartmanaIndex(string datumDolaska, string datumOdlaska, string nazivGrada, double cenaOd, double cenaDo, int brSobaOd, int brSobaDo, int brOsobaOd, int brOsobaDo)
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>(getApartmani);
            string[] nizDolazak = datumDolaska.Split('-');
            string[] nizOdlazak = datumOdlaska.Split('-');


            foreach (var apa in getApartmani)
            {
                try
                {
                    DateTime datumDolaska2 = new DateTime(Int32.Parse(nizDolazak[0]), Int32.Parse(nizDolazak[1]), Int32.Parse(nizDolazak[2]));
                    DateTime datumOdlaska2 = new DateTime(Int32.Parse(nizOdlazak[0]), Int32.Parse(nizOdlazak[1]), Int32.Parse(nizOdlazak[2]));

                    List<DateTime> listaDatuma = GetAllDates(datumDolaska2, datumOdlaska2);
                    int provera = 0;

                    foreach (var temp1 in listaDatuma)
                    {
                        foreach (var temp in apa.Value.dostupnost)
                        {
                            if (temp1.Date == temp.Date)
                            {
                                provera++;
                            }
                        }
                    }

                    if (provera != listaDatuma.Count)
                    {
                        apartmani.Remove(apa.Key);
                    }
                }
                catch { }

                try
                {
                    if (nazivGrada != "" && nazivGrada != apa.Value.lokacija.adresa.nazivMesta)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    if (cenaOd == 0 && cenaDo != 0 && cenaDo < apa.Value.cenaNocenja)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (cenaOd != 0 && cenaDo == 0 && cenaOd > apa.Value.cenaNocenja)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (cenaOd != 0 && cenaDo != 0 && (cenaOd > apa.Value.cenaNocenja || cenaDo < apa.Value.cenaNocenja))
                    {
                        apartmani.Remove(apa.Key);
                    }
                    if (brSobaOd == 0 && brSobaDo != 0 && brSobaDo < apa.Value.brojSoba)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brSobaOd != 0 && brSobaDo == 0 && brSobaOd > apa.Value.brojSoba)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brSobaOd != 0 && brSobaDo != 0 && (brSobaOd > apa.Value.brojSoba || brSobaDo < apa.Value.brojSoba))
                    {
                        apartmani.Remove(apa.Key);
                    }
                    if (brOsobaOd == 0 && brOsobaOd != 0 && brOsobaOd < apa.Value.brojGostiju)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brOsobaOd != 0 && brOsobaOd == 0 && brOsobaOd > apa.Value.brojGostiju)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brOsobaOd != 0 && brOsobaOd != 0 && (brOsobaOd > apa.Value.brojGostiju || brOsobaOd < apa.Value.brojGostiju))
                    {
                        apartmani.Remove(apa.Key);
                    }

                }
                catch { }
            }

            ViewBag.apartmani = apartmani;
            Session["apartmaniPretragaIndex"] = apartmani;
            return View("PregledApartmanaPretragaIndex");
        }

        [HttpPost]
        public ActionResult PretragaApartmanaGost(string datumDolaska, string datumOdlaska, string nazivGrada, double cenaOd, double cenaDo, int brSobaOd, int brSobaDo, int brOsobaOd, int brOsobaDo)
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>(getApartmani);
            string[] nizDolazak = datumDolaska.Split('-');
            string[] nizOdlazak = datumOdlaska.Split('-');


            foreach (var apa in getApartmani)
            {
                try
                {
                    DateTime datumDolaska2 = new DateTime(Int32.Parse(nizDolazak[0]), Int32.Parse(nizDolazak[1]), Int32.Parse(nizDolazak[2]));
                    DateTime datumOdlaska2 = new DateTime(Int32.Parse(nizOdlazak[0]), Int32.Parse(nizOdlazak[1]), Int32.Parse(nizOdlazak[2]));
                    List<DateTime> listaDatuma = GetAllDates(datumDolaska2, datumOdlaska2);
                    int provera = 0;

                    foreach (var temp1 in listaDatuma)
                    {
                        foreach (var temp in apa.Value.dostupnost)
                        {
                            if (temp1.Date == temp.Date)
                            {
                                provera++;
                            }
                        }
                    }

                    if (provera != listaDatuma.Count)
                    {
                        apartmani.Remove(apa.Key);
                    }
                }
                catch { }

                try
                {
                    if (nazivGrada != "" && nazivGrada != apa.Value.lokacija.adresa.nazivMesta)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    if (cenaOd == 0 && cenaDo != 0 && cenaDo < apa.Value.cenaNocenja)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (cenaOd != 0 && cenaDo == 0 && cenaOd > apa.Value.cenaNocenja)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (cenaOd != 0 && cenaDo != 0 && (cenaOd > apa.Value.cenaNocenja || cenaDo < apa.Value.cenaNocenja))
                    {
                        apartmani.Remove(apa.Key);
                    }
                    if (brSobaOd == 0 && brSobaDo != 0 && brSobaDo < apa.Value.brojSoba)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brSobaOd != 0 && brSobaDo == 0 && brSobaOd > apa.Value.brojSoba)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brSobaOd != 0 && brSobaDo != 0 && (brSobaOd > apa.Value.brojSoba || brSobaDo < apa.Value.brojSoba))
                    {
                        apartmani.Remove(apa.Key);
                    }
                    if (brOsobaOd == 0 && brOsobaOd != 0 && brOsobaOd < apa.Value.brojGostiju)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brOsobaOd != 0 && brOsobaOd == 0 && brOsobaOd > apa.Value.brojGostiju)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brOsobaOd != 0 && brOsobaOd != 0 && (brOsobaOd > apa.Value.brojGostiju || brOsobaOd < apa.Value.brojGostiju))
                    {
                        apartmani.Remove(apa.Key);
                    }

                }
                catch { }
            }

            ViewBag.apartmani = apartmani;
            Session["apartmaniPretragaGost"] = apartmani;
            return View("PregledApartmanaPretragaGost");
        }

        [HttpPost]
        public ActionResult PretragaApartmanaAdmin(string datumDolaska, string datumOdlaska, string nazivGrada, double cenaOd, double cenaDo, int brSobaOd, int brSobaDo, int brOsobaOd, int brOsobaDo)
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>(getApartmani);
            string[] nizDolazak = datumDolaska.Split('-');
            string[] nizOdlazak = datumOdlaska.Split('-');


            foreach (var apa in getApartmani)
            {
                try
                {
                    DateTime datumDolaska2 = new DateTime(Int32.Parse(nizDolazak[0]), Int32.Parse(nizDolazak[1]), Int32.Parse(nizDolazak[2]));
                    DateTime datumOdlaska2 = new DateTime(Int32.Parse(nizOdlazak[0]), Int32.Parse(nizOdlazak[1]), Int32.Parse(nizOdlazak[2]));
                    List<DateTime> listaDatuma = GetAllDates(datumDolaska2, datumOdlaska2);
                    int provera = 0;

                    foreach (var temp1 in listaDatuma)
                    {
                        foreach (var temp in apa.Value.dostupnost)
                        {
                            if (temp1.Date == temp.Date)
                            {
                                provera++;
                            }
                        }
                    }

                    if (provera != listaDatuma.Count)
                    {
                        apartmani.Remove(apa.Key);
                    }
                }
                catch { }

                try
                {
                    if (nazivGrada != "" && nazivGrada != apa.Value.lokacija.adresa.nazivMesta)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    if (cenaOd == 0 && cenaDo != 0 && cenaDo < apa.Value.cenaNocenja)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (cenaOd != 0 && cenaDo == 0 && cenaOd > apa.Value.cenaNocenja)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (cenaOd != 0 && cenaDo != 0 && (cenaOd > apa.Value.cenaNocenja || cenaDo < apa.Value.cenaNocenja))
                    {
                        apartmani.Remove(apa.Key);
                    }
                    if (brSobaOd == 0 && brSobaDo != 0 && brSobaDo < apa.Value.brojSoba)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brSobaOd != 0 && brSobaDo == 0 && brSobaOd > apa.Value.brojSoba)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brSobaOd != 0 && brSobaDo != 0 && (brSobaOd > apa.Value.brojSoba || brSobaDo < apa.Value.brojSoba))
                    {
                        apartmani.Remove(apa.Key);
                    }
                    if (brOsobaOd == 0 && brOsobaOd != 0 && brOsobaOd < apa.Value.brojGostiju)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brOsobaOd != 0 && brOsobaOd == 0 && brOsobaOd > apa.Value.brojGostiju)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brOsobaOd != 0 && brOsobaOd != 0 && (brOsobaOd > apa.Value.brojGostiju || brOsobaOd < apa.Value.brojGostiju))
                    {
                        apartmani.Remove(apa.Key);
                    }

                }
                catch { }
            }

            ViewBag.apartmani = apartmani;
            Session["apartmaniPretragaAdmin"] = apartmani;
            return View("PregledApartmanaPretragaAdmin");
        }

        [HttpPost]
        public ActionResult PretragaApartmanaDomacin(string datumDolaska, string datumOdlaska, string nazivGrada, double cenaOd, double cenaDo, int brSobaOd, int brSobaDo, int brOsobaOd, int brOsobaDo)
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>(getApartmani);
            string[] nizDolazak = datumDolaska.Split('-');
            string[] nizOdlazak = datumOdlaska.Split('-');

            foreach (var apa in getApartmani)
            {
                try
                {
                    DateTime datumDolaska2 = new DateTime(Int32.Parse(nizDolazak[0]), Int32.Parse(nizDolazak[1]), Int32.Parse(nizDolazak[2]));
                    DateTime datumOdlaska2 = new DateTime(Int32.Parse(nizOdlazak[0]), Int32.Parse(nizOdlazak[1]), Int32.Parse(nizOdlazak[2]));

                    List<DateTime> listaDatuma = GetAllDates(datumDolaska2, datumOdlaska2);
                    int provera = 0;

                    foreach (var temp1 in listaDatuma)
                    {
                        foreach (var temp in apa.Value.dostupnost)
                        {
                            if (temp1.Date == temp.Date)
                            {
                                provera++;
                            }
                        }
                    }

                    if (provera != listaDatuma.Count)
                    {
                        apartmani.Remove(apa.Key);
                    }
                }
                catch
                { }

                try
                {
                    if (nazivGrada != "" && nazivGrada != apa.Value.lokacija.adresa.nazivMesta)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    if (cenaOd == 0 && cenaDo != 0 && cenaDo < apa.Value.cenaNocenja)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (cenaOd != 0 && cenaDo == 0 && cenaOd > apa.Value.cenaNocenja)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (cenaOd != 0 && cenaDo != 0 && (cenaOd > apa.Value.cenaNocenja || cenaDo < apa.Value.cenaNocenja))
                    {
                        apartmani.Remove(apa.Key);
                    }
                    if (brSobaOd == 0 && brSobaDo != 0 && brSobaDo < apa.Value.brojSoba)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brSobaOd != 0 && brSobaDo == 0 && brSobaOd > apa.Value.brojSoba)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brSobaOd != 0 && brSobaDo != 0 && (brSobaOd > apa.Value.brojSoba || brSobaDo < apa.Value.brojSoba))
                    {
                        apartmani.Remove(apa.Key);
                    }
                    if (brOsobaOd == 0 && brOsobaOd != 0 && brOsobaOd < apa.Value.brojGostiju)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brOsobaOd != 0 && brOsobaOd == 0 && brOsobaOd > apa.Value.brojGostiju)
                    {
                        apartmani.Remove(apa.Key);
                    }
                    else if (brOsobaOd != 0 && brOsobaOd != 0 && (brOsobaOd > apa.Value.brojGostiju || brOsobaOd < apa.Value.brojGostiju))
                    {
                        apartmani.Remove(apa.Key);
                    }

                }
                catch { }
            }

            ViewBag.apartmani = apartmani;
            Session["apartmaniPretragaDomacin"] = apartmani;
            return View("PregledApartmanaPretragaDomacin");
        }

        [HttpPost]
        public ActionResult PretragaKorisnikaAdmin(string uloga, string pol, string korisnickoIme)
        {
            Dictionary<string, Domacin> domacini = new Dictionary<string, Domacin>(getDomacini);
            Dictionary<string, Gost> gosti = new Dictionary<string, Gost>(getGosti);

            foreach (var domacin in getDomacini)
            {
                var tt = domacin.Value.uloga.ToString();
                try
                {
                    if (uloga != "" && uloga.ToString() != domacin.Value.uloga.ToString())
                    {
                        domacini.Remove(domacin.Key);
                    }
                    if (pol != "" && pol.ToString() != domacin.Value.pol.ToString())
                    {
                        domacini.Remove(domacin.Key);
                    }
                    if (korisnickoIme != "" && korisnickoIme.ToString() != domacin.Value.korisnickoIme.ToString())
                    {
                        domacini.Remove(domacin.Key);
                    }
                }
                catch { }
            }
            foreach (var gost in getGosti)
            {
                try
                {
                    if (uloga != "" && uloga.ToString() != gost.Value.uloga.ToString())
                    {
                        gosti.Remove(gost.Key);
                    }
                    if (pol != "" && pol.ToString() != gost.Value.pol.ToString())
                    {
                        gosti.Remove(gost.Key);
                    }
                    if (korisnickoIme != "" && korisnickoIme.ToString() != gost.Value.korisnickoIme.ToString())
                    {
                        gosti.Remove(gost.Key);
                    }
                }
                catch { }
            }

            ViewBag.domacini = domacini;
            ViewBag.gosti = gosti;
            return View("PregledKorisnikaPretragaAdmin");
        }

        [HttpPost]
        public ActionResult FiltrirajApartmaneStatusAdmin()
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            foreach (var apa in getApartmani)
            {
                try
                {
                    if (apa.Value.status.ToString() == Request.Form["status"])
                    {
                        apartmani.Add(apa.Key, apa.Value);
                    }
                }
                catch { }
            }
            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaAdmin");
        }

        [HttpPost]
        public ActionResult FiltrirajApartmaneStatusDomacin()
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            foreach (var apa in getApartmani)
            {
                try
                {
                    if (apa.Value.status.ToString() == Request.Form["status"])
                    {
                        apartmani.Add(apa.Key, apa.Value);
                    }
                }
                catch { }
            }
            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaDomacin");
        }

        [HttpPost]
        public ActionResult FiltrirajApartmaneTipAdmin()
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            foreach (var apa in getApartmani)
            {
                try
                {
                    if (apa.Value.tip.ToString() == Request.Form["tip"])
                    {
                        apartmani.Add(apa.Key, apa.Value);
                    }
                }
                catch { }
            }
            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaAdmin");
        }

        [HttpPost]
        public ActionResult FiltrirajApartmaneTipGost()
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            foreach (var apa in getApartmani)
            {
                try
                {
                    if (apa.Value.tip.ToString() == Request.Form["tip"])
                    {
                        apartmani.Add(apa.Key, apa.Value);
                    }
                }
                catch { }
            }
            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaGost");
        }

        [HttpPost]
        public ActionResult FiltrirajApartmaneTipIndex()
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            foreach (var apa in getApartmani)
            {
                try
                {
                    if (apa.Value.tip.ToString() == Request.Form["tip"])
                    {
                        apartmani.Add(apa.Key, apa.Value);
                    }
                }
                catch { }
            }
            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaIndex");
        }

        [HttpPost]
        public ActionResult FiltrirajApartmaneTipDomacin()
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            foreach (var apa in getApartmani)
            {
                try
                {
                    if (apa.Value.tip.ToString() == Request.Form["tip"])
                    {
                        apartmani.Add(apa.Key, apa.Value);
                    }
                }
                catch { }
            }
            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaDomacin");
        }

        [HttpPost]
        public ActionResult FiltrirajApartmaneSadrzajAdmin()
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            foreach (var apa in getApartmani)
            {
                foreach (var sadrzaj in apa.Value.sadrzajApartmana)
                {
                    try
                    {
                        if (sadrzaj.naziv.ToString() == Request.Form["sadrzaj"])
                        {
                            apartmani.Add(apa.Key, apa.Value);
                        }
                    }
                    catch { }
                }
            }
            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaAdmin");
        }

        [HttpPost]
        public ActionResult FiltrirajApartmaneSadrzajGost()
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            foreach (var apa in getApartmani)
            {
                foreach (var sadrzaj in apa.Value.sadrzajApartmana)
                {
                    try
                    {
                        if (sadrzaj.naziv.ToString() == Request.Form["sadrzaj"])
                        {
                            apartmani.Add(apa.Key, apa.Value);
                        }
                    }
                    catch { }
                }
            }
            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaGost");
        }

        [HttpPost]
        public ActionResult FiltrirajApartmaneSadrzajIndex()
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            foreach (var apa in getApartmani)
            {
                foreach (var sadrzaj in apa.Value.sadrzajApartmana)
                {
                    try
                    {
                        if (sadrzaj.naziv.ToString() == Request.Form["sadrzaj"])
                        {
                            apartmani.Add(apa.Key, apa.Value);
                        }
                    }
                    catch { }
                }
            }
            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaIndex");
        }

        [HttpPost]
        public ActionResult FiltrirajApartmaneSadrzajDomacin()
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            foreach (var apa in getApartmani)
            {
                foreach (var sadrzaj in apa.Value.sadrzajApartmana)
                {
                    try
                    {
                        if (sadrzaj.naziv.ToString() == Request.Form["sadrzaj"])
                        {
                            apartmani.Add(apa.Key, apa.Value);
                        }
                    }
                    catch { }
                }
            }
            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaDomacin");
        }

        [HttpPost]
        public ActionResult SortiranjeIndex(string sortiranje)
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            Dictionary<string, Apartman> apartmaniPretraga = new Dictionary<string, Apartman>((Dictionary<string, Apartman>)Session["apartmaniPretragaIndex"]);
            if (sortiranje.Equals("cenaRastuce"))
                apartmani = apartmaniPretraga.ToList().OrderBy(a => a.Value.cenaNocenja).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("cenaOpadajuce"))
                apartmani = apartmaniPretraga.ToList().OrderByDescending(a => a.Value.cenaNocenja).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brSobaRastuce"))
                apartmani = apartmaniPretraga.ToList().OrderBy(a => a.Value.brojSoba).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brSobaOpadajuce"))
                apartmani = apartmaniPretraga.ToList().OrderByDescending(a => a.Value.brojSoba).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brOsobaRastuce"))
                apartmani = apartmaniPretraga.ToList().OrderBy(a => a.Value.brojGostiju).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brOsobaOpadajuce"))
                apartmani = apartmaniPretraga.ToList().OrderByDescending(a => a.Value.brojGostiju).ToDictionary(x => x.Key, y => y.Value);


            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaIndex");
        }

        [HttpPost]
        public ActionResult SortiranjeGost(string sortiranje)
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            Dictionary<string, Apartman> apartmaniPretraga = new Dictionary<string, Apartman>((Dictionary<string, Apartman>)Session["apartmaniPretragaGost"]);
            if (sortiranje.Equals("cenaRastuce"))
                apartmani = apartmaniPretraga.ToList().OrderBy(a => a.Value.cenaNocenja).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("cenaOpadajuce"))
                apartmani = apartmaniPretraga.ToList().OrderByDescending(a => a.Value.cenaNocenja).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brSobaRastuce"))
                apartmani = apartmaniPretraga.ToList().OrderBy(a => a.Value.brojSoba).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brSobaOpadajuce"))
                apartmani = apartmaniPretraga.ToList().OrderByDescending(a => a.Value.brojSoba).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brOsobaRastuce"))
                apartmani = apartmaniPretraga.ToList().OrderBy(a => a.Value.brojGostiju).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brOsobaOpadajuce"))
                apartmani = apartmaniPretraga.ToList().OrderByDescending(a => a.Value.brojGostiju).ToDictionary(x => x.Key, y => y.Value);



            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaGost");
        }

        [HttpPost]
        public ActionResult SortiranjeAdmin(string sortiranje)
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            Dictionary<string, Apartman> apartmaniPretraga = new Dictionary<string, Apartman>((Dictionary<string, Apartman>)Session["apartmaniPretragaAdmin"]);
            if (sortiranje.Equals("cenaRastuce"))
                apartmani = apartmaniPretraga.ToList().OrderBy(a => a.Value.cenaNocenja).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("cenaOpadajuce"))
                apartmani = apartmaniPretraga.ToList().OrderByDescending(a => a.Value.cenaNocenja).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brSobaRastuce"))
                apartmani = apartmaniPretraga.ToList().OrderBy(a => a.Value.brojSoba).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brSobaOpadajuce"))
                apartmani = apartmaniPretraga.ToList().OrderByDescending(a => a.Value.brojSoba).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brOsobaRastuce"))
                apartmani = apartmaniPretraga.ToList().OrderBy(a => a.Value.brojGostiju).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brOsobaOpadajuce"))
                apartmani = apartmaniPretraga.ToList().OrderByDescending(a => a.Value.brojGostiju).ToDictionary(x => x.Key, y => y.Value);



            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaAdmin");
        }

        [HttpPost]
        public ActionResult SortiranjeDomacin(string sortiranje)
        {
            Dictionary<string, Apartman> apartmani = new Dictionary<string, Apartman>();
            Dictionary<string, Apartman> apartmaniPretraga = new Dictionary<string, Apartman>((Dictionary<string, Apartman>)Session["apartmaniPretragaDomacin"]);
            if (sortiranje.Equals("cenaRastuce"))
                apartmani = apartmaniPretraga.ToList().OrderBy(a => a.Value.cenaNocenja).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("cenaOpadajuce"))
                apartmani = apartmaniPretraga.ToList().OrderByDescending(a => a.Value.cenaNocenja).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brSobaRastuce"))
                apartmani = apartmaniPretraga.ToList().OrderBy(a => a.Value.brojSoba).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brSobaOpadajuce"))
                apartmani = apartmaniPretraga.ToList().OrderByDescending(a => a.Value.brojSoba).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brOsobaRastuce"))
                apartmani = apartmaniPretraga.ToList().OrderBy(a => a.Value.brojGostiju).ToDictionary(x => x.Key, y => y.Value);
            else if (sortiranje.Equals("brOsobaOpadajuce"))
                apartmani = apartmaniPretraga.ToList().OrderByDescending(a => a.Value.brojGostiju).ToDictionary(x => x.Key, y => y.Value);



            ViewBag.apartmani = apartmani;
            return View("PregledApartmanaPretragaDomacin");
        }
    }
}