using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator
{
    class Przedmiot
    {
        public string nazwa;
        public int semestr;
        public int godziny;
        public int ects;
        public int idglownegoprowadzacego;

        public Przedmiot(string nazwa, int semestr, int godziny, int ects, int idglownegoprowadzacego)
        {
            this.nazwa = nazwa;
            this.semestr = semestr;
            this.godziny = godziny;
            this.ects = ects;
            this.idglownegoprowadzacego = idglownegoprowadzacego;
        }
        public String toString2()
        {
            return nazwa + ";" + semestr.ToString() + ";" + godziny.ToString() + ";" + ects.ToString() + ";" + idglownegoprowadzacego.ToString() + "\r\n";
        }
    }
}
