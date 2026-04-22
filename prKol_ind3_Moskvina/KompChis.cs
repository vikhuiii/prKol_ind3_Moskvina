using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prKol_ind3_Moskvina
{
    public class KompChis
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }
        public KompChis(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }
        public static KompChis operator +(KompChis a, KompChis b)
        {
            return new KompChis(a.Real + b.Real, a.Imaginary + b.Imaginary);
        }
        public static KompChis operator -(KompChis a, KompChis b)
        {
            return new KompChis(a.Real - b.Real, a.Imaginary - b.Imaginary);
        }
        public static KompChis operator *(KompChis a, KompChis b)
        {
            double real = a.Real * b.Real - a.Imaginary * b.Imaginary;
            double imaginary = a.Real * b.Real + a.Imaginary * b.Imaginary;
            return new KompChis(real, imaginary);
        }
        public override string ToString()
        {
            string sign = Imaginary >= 0 ? "+" : "";
            return $"{Real}{sign}{Imaginary}i";
        }
    }
}
