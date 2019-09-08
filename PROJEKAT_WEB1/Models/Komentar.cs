using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROJEKAT_WEB1.Models
{
    public class Komentar
    {
        public Korisnik gost { get; set; }
        public Apartman apartman { get; set; }
        public string tekst { get; set; }
        public int ocena { get; set; }

        public Komentar()
        {

        }

        public Komentar(string tekst, Korisnik gost, Apartman apartman, int ocena)
        {
            this.gost = gost;
            this.apartman = apartman;
            this.tekst = tekst;
            this.ocena = ocena;
        }
    }
}