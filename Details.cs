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
        protected string imageMode = "";
        protected int imageSetReady = 0;
        protected string jobMode = "";
        protected WebHook WebHelper=new WebHook();
        public void MainJob()
        {
            config();

            while (imageSetReady==0)
            {
                imageSetReady = GetNStoreDates();
            }
            BuildPathTree();
            ImageManager();
        }







        private void config()
        {
            Console.WriteLine("Select image type (N)atural | (E)nhanced: ");
            while (imageMode != "N" && imageMode != "E")
            {
                imageMode = Console.ReadLine();
            }
            switch (imageMode)
            {
                case "N":
                    imageMode = "natural";
                    break;
                case "E":
                    imageMode = "enhanced";
                    break;
            }
            //when released, need to set app-wideAPI or user registration for personal key
#if DEBUG

#else
            Console.WriteLine("API Key:");
            api_key = Console.ReadLine();
#endif

        }    


        private int GetNStoreDates()
        {
            var task = WebHelper.getDaySet(imageMode);
            task.Wait();
            return ProcessDates(task.Result);
        }

        
        private int ProcessDates(string response)
        {
            if (response == "")
            {
                Console.WriteLine("Response stream is empty. Cancelling process...");
                return 0;
            }
            data = JsonConvert.DeserializeObject<List<Dates>>(response);
#if DEBUG            
            foreach (var zi in data)
            {
                Console.WriteLine(zi.date);
            }
#else

#endif
            return 1;
        }


        private void BuildPathTree()
        {
            Console.WriteLine();
            Console.Write("Building data structure on disk...");
            string currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pathString = System.IO.Path.Combine(currentFolder, "Data");
            System.IO.Directory.CreateDirectory(pathString);
            foreach (var zi in data)
            {
                //P0-year, P1-month,P2-day
                string[] folderName = generateFolderNames(zi.date);

                string year = System.IO.Path.Combine(pathString, folderName[0]);
                System.IO.Directory.CreateDirectory(year);
                string month = System.IO.Path.Combine(year, folderName[1]);
                System.IO.Directory.CreateDirectory(month);
                string day = System.IO.Path.Combine(month, folderName[2]);
                System.IO.Directory.CreateDirectory(day);

#if DEBUG
                
#else

#endif
            }
            Console.Write("Complete.");
            Console.WriteLine();
        }
        private string[] generateFolderNames(string date)
        {
            string[] folderName = date.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            return folderName;

        }


        private void ImageManager()
        {
            Console.WriteLine("Select operating mode");
            Console.WriteLine("(B)atch (downloads all image data available) | (S)elective (specify a date to download image data for) ");
            while (jobMode != "B" && jobMode != "S")
            {
                jobMode = Console.ReadLine();
            }
            switch (jobMode)
            {
                case "B":

                    break;

                case "S":
                    //get Year,Month,Day, prepare YMD format, then retrieve image set
                    Console.WriteLine("Type in year:");
                    string year = Console.ReadLine();
                    Console.WriteLine("Type in month:");
                    string month = Console.ReadLine();
                    Console.WriteLine("Type in day:");
                    string day = Console.ReadLine();
                    
                    string YMD = year + "-" + month + "-" + day;
                    foreach (var zi in data)
                    {
                        if (zi.date == YMD)
                        {
                            int getImageReady = 0;
                            while (getImageReady == 0)
                            {
                                var task = WebHelper.getImageSet(imageMode, YMD);
                                task.Wait();
                                getImageReady=ProcessImageSet(task.Result, YMD);
                            }
                        }
                    }
                    break;
            }
            
            Console.WriteLine("Jobs complete.")
            
            
        }


        private int ProcessImageSet(string response,string YMD)
        {
            if (response == "")
            {
                Console.WriteLine("Response stream is empty. Cancelling process...");
                return 0;
            }

            List<string> images=generateImageObjects(response);
            foreach (var zi in data)
            {
                if (zi.date == YMD)
                {
                    zi.Images = new List<Rootobject>();
                    foreach (string image in images)
                    {
                        zi.Images.Add(JsonConvert.DeserializeObject<Rootobject>(image));                        
                        
                    }
                    foreach (var item in zi.Images)
                    {
                        //Console.WriteLine(item.image);
                        WebHelper.Download(imageMode,item.image, YMD);
                    }
                }
               
            }
            return 1;
#if DEBUG
           
            
#else

#endif
        }

        private List<string> generateImageObjects(string data)
        {           
            int position;
            int start = 1;
            var images = new List<string>();
            do
            {
                position = data.IndexOf("},{", start);
                if (position >= 0)
                {
                    images.Add(data.Substring(start, position - start+1).Trim());
                    start = position + 2;
                }
            } while (position > 0);
            foreach (var item in images)
            {
                //Console.WriteLine(item);
            }
            return images;

        }
    }
}
