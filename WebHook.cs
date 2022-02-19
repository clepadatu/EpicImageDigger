using System;
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
    class WebHook
    {
        public async Task<string> getDaySet(string imageMode)
        {
            Console.WriteLine();
            Console.Write("Getting list of available day data...");
            //Links  that get you the data
            //setup webrequest
            //return data
            //https://api.nasa.gov/EPIC/api/natural/all?api_key=DEMO_KEY
            //https://api.nasa.gov/EPIC/api/natural/all?api_key=jb6NcchxpP5kYcia7SMBKaAZKYAbfFwrzGD1rcIY

#if DEBUG
            string baseurl = "https://api.nasa.gov/EPIC/api/";
            string urlsuffix = "/all?api_key=jb6NcchxpP5kYcia7SMBKaAZKYAbfFwrzGD1rcIY";
            string fullurl = baseurl + imageMode + urlsuffix;
            
#else
            string baseurl = "https://api.nasa.gov/EPIC/api/";
            string urlsuffix = "/all?api_key="+api_key;
            string fullurl = baseurl + imageMode + urlsuffix;
            
#endif

            string content = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient client = new HttpClient();
            try
            {
                var response = await client.GetAsync(fullurl);
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("success.");
                    content = await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout. " + e.HelpLink);
            }

            Console.Write("Complete.");
            Console.WriteLine();
            return content;

        }

        public async Task<string> getImageSet(string imageMode,string YMD)
        {
            //get set of image names from a certain day from https://api.nasa.gov/EPIC/api/enhanced/date/2019-05-30?api_key=DEMO_KEY
            Console.WriteLine();
            Console.Write("Getting image set for day " + YMD + "...");

#if DEBUG
            string baseurl = "https://api.nasa.gov/EPIC/api/";
            string middle = "/date/";
            string urlsuffix = "?api_key=jb6NcchxpP5kYcia7SMBKaAZKYAbfFwrzGD1rcIY";
            string fullurl = baseurl + imageMode + middle + YMD + urlsuffix;
#else
            string baseurl = "https://api.nasa.gov/EPIC/api/";
            string middle = "/date/";
            string urlsuffix = "?api_key="+api_key;
            string fullurl = baseurl + imageMode + middle + YMD + urlsuffix;
#endif

            string content = null;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient client = new HttpClient();
            try
            {
                var response = await client.GetAsync(fullurl);
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

            Console.Write("Complete.");
            Console.WriteLine();
            return content;

        }


        public void Download(string imageMode, string imagename, string YMD)
        {

            //https://api.nasa.gov/EPIC/archive/natural/2019/05/30/png/epic_1b_20190530011359.png?api_key=DEMO_KEY
            string[] info = generateFolderNames(YMD);
            string year = info[0];
            string month = info[1];
            string day = info[2];

#if DEBUG
            string baseurl = "https://epic.gsfc.nasa.gov/archive/";
            string urlsuffix = "?api_key=jb6NcchxpP5kYcia7SMBKaAZKYAbfFwrzGD1rcIY";
            string fullurl = baseurl + imageMode + "/" + year + "/" + month + "/" + day + "/png/" + imagename + ".png" + urlsuffix;
#else
            string baseurl = "https://epic.gsfc.nasa.gov/archive/";
            string urlsuffix = "?api_key="+api_key;
            string fullurl = baseurl + imageMode + "/" + year + "/" + month + "/" + day + "/png/" + imagename + ".png" + urlsuffix;
#endif

            string currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pathString = System.IO.Path.Combine(currentFolder, "Data\\" + year + @"\" + month + @"\" + day);

            Console.WriteLine();
            Console.Write("Downloading " + fullurl + " to " + pathString + "\\" + imagename + " ...");

            var client = new WebClient();
            var uri = new Uri(fullurl);

            client.DownloadFileCompleted += (sender, e) => Console.Write("Complete.");
            
            client.DownloadFile(uri, pathString + "\\" + imagename + ".png");
            Console.WriteLine();
        }


        private string[] generateFolderNames(string date)
        {
            string[] folderName = date.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            return folderName;

        }
    }
}
