using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Reflection;

namespace ImageDigger
{
    class Details
    {
        //Get User API KEY
        //Get image data date set
        //Order image date set
        //Create folder structure on disk
        //Start downloading images, use delay. cap is 1000 images per hour

        protected string api_key = "";
        protected List<Dates> data;
        public void startJob()
        {
            Console.Clear();
#if DEBUG
            Console.WriteLine("Worker thread started.");
#else
                Console.WriteLine("DSCOVR: EPIC image download tool. Use at your own discretion, please input your own API Key to use.");
                getInfo();
#endif
            var task = getDateSet();
            task.Wait();
            ProcessDates(task.Result);
            BuildPathTree();
        }
        private void getInfo()
        {
            Console.WriteLine("API Key:");
            api_key = Console.ReadLine();
        }
        private async Task<string> getDateSet()
        {
            Console.WriteLine("Getting list of YMD...");
            //Links  that get you the data
            //setup webrequest
            //return data
            //https://api.nasa.gov/EPIC/api/natural/all?api_key=DEMO_KEY
            //https://api.nasa.gov/EPIC/api/natural/all?api_key=jb6NcchxpP5kYcia7SMBKaAZKYAbfFwrzGD1rcIY
#if DEBUG
            string url = "https://api.nasa.gov/EPIC/api/natural/all?api_key=DEMO_KEY";
#else
            string url = "https://api.nasa.gov/EPIC/api/natural/all?api_key="+api_key;
#endif

            string content = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient client = new HttpClient();
            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("success.");
                    content = await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout. " + e.HelpLink);
            }

            Console.WriteLine("Complete.");
            return content;

        }
        private void getImageSets()
        {
            
        }

        //private void Download()
        //{
        //    var client = new WebClient();
        //    var uri = new Uri(address);

        //    client.DownloadFileCompleted += (sender, e) => Console.WriteLine("Finished");
        //    client.DownloadFileAsync(uri, "Hamsters.txt");
        //}
        private void ProcessDates(string response)
        {
            if (response == "")
            {
                Console.WriteLine("Response stream is empty. Cancelling process...");
                return;
            }
            data = JsonConvert.DeserializeObject<List<Dates>>(response);
#if DEBUG
            Console.WriteLine(response);
            foreach (var zi in data)
            {

                Console.WriteLine(zi.date);
            }
#else

#endif
        }

        private void BuildPathTree()
        {
            Console.WriteLine("Building data structure on disk...");
            string currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pathString = System.IO.Path.Combine(currentFolder, "Data");
            System.IO.Directory.CreateDirectory(pathString);
            foreach (var zi in data)
            {
                //P0-year, P1-month,P2-day
                string[] folderName=generateFolderNames(zi.date);
                
                string year = System.IO.Path.Combine(pathString, folderName[0]);
                System.IO.Directory.CreateDirectory(year);
                string month= System.IO.Path.Combine(year, folderName[1]);
                System.IO.Directory.CreateDirectory(month);
                string day = System.IO.Path.Combine(month, folderName[2]);
                System.IO.Directory.CreateDirectory(day);

#if DEBUG
                Console.WriteLine(folderName[0] + "," + folderName[1] + "," + folderName[2]);
#else

#endif
            }
            Console.WriteLine("Complete.");
        }

        private string[] generateFolderNames(string date)
        {
            string[] folderName = date.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            return folderName;
                        
        }

    }
}
