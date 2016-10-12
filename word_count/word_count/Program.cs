using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace word_count
{
    class Program
    {
        static void Main(string[] args)
         {
            if (args != null && args.Count() == 2)
            {
                try
                {
                    Scanner scan = new Scanner(args[0]);
                    scan.CountWord(Convert.ToInt32(args[1]));
                }catch(FormatException)
                {
                    Console.WriteLine("Wrong Count Number!");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("{0} is not a file directory!",args[0]);
                }
            }
            else
            {
                Console.WriteLine("Please input right parameter,like this: word_count.exe D:/novel 100");
            }
        }

    }
}
