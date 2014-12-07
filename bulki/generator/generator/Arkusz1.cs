using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator
{
    class Arkusz1
    {
        public int idterminu;
        public int idskladowej;
        public int idprowadzacego;

        public Arkusz1(int _idt, int _ids, int _idp)
        {
            idterminu = _idt;
            idskladowej = _ids;
            idprowadzacego = _idp;
        }
        public string toString2()
        {
            return idterminu.ToString() + ";" + idskladowej.ToString() + ";" + idprowadzacego.ToString() + "\r\n";
        }
    }
}
