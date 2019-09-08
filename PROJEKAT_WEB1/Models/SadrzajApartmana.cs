using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROJEKAT_WEB1.Models
{
    public class SadrzajApartmana
    {
        public string id { get; set; }
        public string naziv { get; set; }

        public SadrzajApartmana()
        {

        }

        public SadrzajApartmana(string id, string naziv)
        {
            this.id = id;
            this.naziv = naziv;
        }
    }
}