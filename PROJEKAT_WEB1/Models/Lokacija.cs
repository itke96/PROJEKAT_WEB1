using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROJEKAT_WEB1.Models
{
    public class Lokacija
    {
        public double geografskaSirina { get; set; }
        public double geografskaDuzina { get; set; }
        public Adresa adresa { get; set; }

        public Lokacija()
        {

        }

        public Lokacija(double geoSirina, double geoDuzina, Adresa adr)
        {
            this.geografskaSirina = geoSirina;
            this.geografskaDuzina = geoDuzina;
            this.adresa = adr;
        }
    }
}