using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZebraPrint.lib.print
{
    public class Code
    {
        string codigo;
        string barcode;
        int cantidad;
        int idcodigo;

        public string Codigo { get => codigo; set => codigo = value; }
        public string Barcode { get => barcode; set => barcode = value; }
        public int Cantidad { get => cantidad; set => cantidad = value; }
        public int Idcodigo { get => idcodigo; set => idcodigo = value; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
