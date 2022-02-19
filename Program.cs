using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace ImageDigger
{

 
#if DEBUG

#else

#endif
    
    class Program
    {      
        static void Main()
        {
            Details worker;  
            
            worker=new Details();
            Console.Clear();
            Console.WriteLine("EpicImageDigger - A “BLUE MARBLE” image downloader");
            Console.WriteLine("By: Cristian Lepadatu");
            Console.WriteLine("Images by: NASA DSCOVR: EPIC Earth Polychromatic Imaging Camera ");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            worker.MainJob();

            Console.ReadLine();
            Console.ReadKey();           
        }
    }
}
