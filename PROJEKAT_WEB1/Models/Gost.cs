using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROJEKAT_WEB1.Models
{
    public class Gost : Korisnik
    {
        List<Apartman> iznajmljeniApartmani { get; set; }
    }
}