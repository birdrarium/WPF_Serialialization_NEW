using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WpfApp10
{
    public class Ubrania
    {
        public int Id { get; set; }
        public string Kod { get; set; }
        public string Producent { get; set; }
        public double Cena { get; set; }
        public string Kategoria { get; set; }
        public string Podkategoria { get; set; }

        public Ubrania(int id, string kod, string p, double c, string kat, string podkat)
        {
            Id = id;
            Kod = kod;
            Producent = p;
            Cena = c;
            Kategoria = kat;
            Podkategoria = podkat;
        }

        public Ubrania() { }

    }

}
