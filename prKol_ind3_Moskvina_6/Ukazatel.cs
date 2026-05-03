using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prKol_ind3_Moskvina_6
{
    class Ukazatel
    {
        public string Slovo { get; set; }
        public List<int> Str { get; set; }

        public Ukazatel(string slovo, List<int> str)
        {
            Slovo = slovo;
            Str = str;
        }

        public override string ToString()
        {
            return $"{Slovo}: {string.Join(", ", Str)}";
        }
    }
}
