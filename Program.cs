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
            worker.startJob();

            Console.ReadLine();
            Console.ReadKey();           
        }
    }
}
