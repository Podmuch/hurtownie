using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator
{
    class Arkusz2
    {
        public int idstudenta;
        public int idterminu;
        public string[] terminy;
        public float[] uzyskanywynik;
        public float[] mozliwepunktydozdobycia;

        public Arkusz2(int _ids, int _idter, string[] ter, float[] uzyskpkt, float[] mozliwpkt)
        {
            idstudenta = _ids;
            idterminu = _idter;
            terminy = ter.ToArray();
            uzyskanywynik = uzyskpkt.ToArray();
            mozliwepunktydozdobycia = mozliwpkt.ToArray();
        }
        public string toString2()
        {
            string str = idstudenta.ToString() + ";" + idterminu.ToString();
            for (int i = 0; i < terminy.Length; i++)
            {
                str = str + ";" + terminy[i].ToString() + ";" + uzyskanywynik[i].ToString() + ";" + mozliwepunktydozdobycia[i].ToString();
            }
            return str + "\r\n";
        }
    }
}
