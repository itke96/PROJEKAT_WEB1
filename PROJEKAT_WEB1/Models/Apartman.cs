using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROJEKAT_WEB1.Models
{
    public class Apartman
    {
        public TipApartmana tip { get; set; }
        public int brojSoba { get; set; }
        public int brojGostiju { get; set; }
        public Lokacija lokacija { get; set; }
        public List<DateTime> datumiZaIzdavanje { get; set; }
        public List<DateTime> dostupnost { get; set; }
        public Domacin domacin { get; set; }
        public List<Komentar> komentar { get; set; }
        public List<string> slike { get; set; }
        public double cenaNocenja { get; set; }
        public string vremeZaPrijavu { get; set; }
        public string vremeZaOdjavu { get; set; }
        public Status status { get; set; }
        public List<SadrzajApartmana> sadrzajApartmana { get; set; }

        public Apartman()
        {
            this.datumiZaIzdavanje = new List<DateTime>();
            this.dostupnost = new List<DateTime>();
            this.komentar = new List<Komentar>();
            this.slike = new List<string>();
            this.vremeZaPrijavu = "2PM";
            this.vremeZaOdjavu = "10AM";
            this.status = Status.NEAKTIVNO;
            this.sadrzajApartmana = new List<SadrzajApartmana>();
        }
    }
}