using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace word_count
{
    class Program
    {
        static void Main(string[] args)
        {
            Scanner scan = new Scanner(@"\\msrasia\share\ASE\novel");
            scan.CountWord();
        }

    }
}
