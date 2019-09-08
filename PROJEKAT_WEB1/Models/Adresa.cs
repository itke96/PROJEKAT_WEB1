using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROJEKAT_WEB1.Models
{
    public class Adresa
    {
        public string nazivUlice { get; set; }
        public string broj { get; set; }
        public string nazivMesta { get; set; }
        public string postanskiBroj { get; set; }

        public Adresa()
        {

        }

        public Adresa(string nazivUlice, string broj, string nazivMesta, string postanskiBroj)
        {
            this.nazivUlice = nazivUlice;
            this.broj = broj;
            this.nazivMesta = nazivMesta;
            this.postanskiBroj = postanskiBroj;
        }
    }
}