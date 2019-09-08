using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROJEKAT_WEB1.Models
{
    public class Korisnik
    {
        public string korisnickoIme { get; set; }
        public string lozinka { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public Pol pol { get; set; }
        public Uloga uloga { get; set; }
    }
}