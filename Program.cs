using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Linq;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Text;

namespace Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ask the user to choose an option.  
            Console.WriteLine("Choose an option from the following list:");
            Console.WriteLine("\tv - Video");
            Console.WriteLine("\tj - Job");
            Console.WriteLine("\tm - Movie");
            Console.Write("Your option? ");
            // Use a switch statement to do the math.  
            switch (Console.ReadLine())
            {
                case "v":
                    Console.WriteLine("You chose video");
                    // Ask the user to type the topic.  
                    Console.WriteLine("Type a topic, and then press Enter");
                    string topic = Console.ReadLine();
                    YouTubeScraping(topic);
                    break;
                case "j":
                    Console.WriteLine("You chose job");
                    // Ask the user to type the function.  
                    Console.WriteLine("Type a function, and then press Enter");
                    string function = Console.ReadLine();
                    IndeedScraping(function);
                    break;
                case "m":
                    Console.WriteLine("You chose movie");
                    // Ask the user to type the genre.  
                    Console.WriteLine("Type a genre, and then press Enter");
                    string genre = Console.ReadLine();
                    ImdbScraping(genre);
                    break;
            }
            // Wait for the user to respond before closing.  
            Console.Write("Press any key to close the console app...");
            Console.ReadKey();
        }

        static void YouTubeScraping(string topicc)
        {
            String ytlink = "https://www.youtube.com/results?search_query=" + topicc;
            int vcount = 1;
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Url = ytlink;
            var timeout = 10000;
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            Thread.Sleep(5000);

            By elem_video_link = By.CssSelector("ytd-video-renderer.style-scope.ytd-item-section-renderer");
            ReadOnlyCollection<IWebElement> videos = driver.FindElements(elem_video_link);
            Console.WriteLine("Total number of videos in " + ytlink + " are " + videos.Count);

            var csv = new StringBuilder();

            foreach (IWebElement video in videos.Take(5))
            {
                string str_title, str_views, str_chan, str_link;
                IWebElement elem_video_title = video.FindElement(By.CssSelector("#video-title"));
                str_title = elem_video_title.Text;

                IWebElement elem_video_views = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[1]"));
                str_views = elem_video_views.Text;

                IWebElement elem_video_channel = video.FindElement(By.CssSelector("#channel-info"));
                str_chan = elem_video_channel.Text;

                IWebElement elem_video_li = video.FindElement(By.CssSelector("[class = 'yt-simple-endpoint style-scope ytd-video-renderer']"));
                str_link = elem_video_li.GetAttribute("href");

                var newLine1 = ("******* Video " + vcount + " *******");
                csv.AppendLine(newLine1);
                var newLine2 = ("Video Title: " + str_title);
                csv.AppendLine(newLine2);
                var newLine3 = ("Video Views: " + str_views);
                csv.AppendLine(newLine3);
                var newLine4 = ("Video Channel Name: " + str_chan);
                csv.AppendLine(newLine4);
                var newLine5 = ("Video link: " + str_link);
                csv.AppendLine(newLine5);
                var newLine6 = ("\n");
                csv.AppendLine(newLine6);
                vcount++;
            }
            Console.WriteLine("Scraping Data from YouTube channel Passed");
            File.WriteAllText("C:/Users/Simon/Downloads/youtube.csv", csv.ToString(), Encoding.UTF8);
        }

        static void IndeedScraping(string functionn)
        {
            String indeedlink = "https://be.indeed.com/jobs?q="+  functionn +"&fromage=3";
            int vcount = 1;
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Url = indeedlink;
            var timeout = 10000;
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            Thread.Sleep(5000);

            By elem_result = By.ClassName("job_seen_beacon");
            ReadOnlyCollection<IWebElement> jobs = driver.FindElements(elem_result);
            Console.WriteLine("Total number of videos in " + indeedlink + " are " + jobs.Count);

            var csv = new StringBuilder();

            foreach (IWebElement job in jobs.Take(5))
            {
                string str_title, str_company, str_location;
                IWebElement elem_job_title = job.FindElement(By.ClassName("jobTitle"));
                str_title = elem_job_title.Text;

                IWebElement elem_job_comp = job.FindElement(By.ClassName("companyName"));
                str_company = elem_job_comp.Text;

                IWebElement elem_job_loc = job.FindElement(By.ClassName("companyLocation"));
                str_location = elem_job_loc.Text;

                var newLine1 = ("******* Job " + vcount + " *******");
                csv.AppendLine(newLine1);
                var newLine2 = ("Job Title: " + str_title);
                csv.AppendLine(newLine2);
                var newLine3 = ("Job Company: " + str_company);
                csv.AppendLine(newLine3);
                var newLine4 = ("Job Location: " + str_location);
                csv.AppendLine(newLine4);
                var newLine5 = ("\n");
                csv.AppendLine(newLine5);
                vcount++;

            }
            Console.WriteLine("Scraping Data from Indeed Passed");
            File.WriteAllText("C:/Users/Simon/Downloads/jobs.csv", csv.ToString(), Encoding.UTF8);
        }

        static void ImdbScraping(string genree)
        {
            String imdblink = "https://www.imdb.com/search/title/?genres="+ genree +"&sort=user_rating,desc&title_type=feature&num_votes=25000,&pf_rd_m=A2FGELUUNOQJNL&pf_rd_p=5aab685f-35eb-40f3-95f7-c53f09d542c3&pf_rd_r=ZG7F2CAVBBF8DR9HDSQB&pf_rd_s=right-6&pf_rd_t=15506&pf_rd_i=top&ref_=chttp_gnr_1";
            int vcount = 1;
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            driver.Url = imdblink;
            var timeout = 10000;
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            Thread.Sleep(5000);

            By elem_lister_item = By.CssSelector("div.lister-item.mode-advanced");
            ReadOnlyCollection<IWebElement> movies = driver.FindElements(elem_lister_item);
            Console.WriteLine("Total number of videos in " + imdblink + " are " + movies.Count);

            var csv = new StringBuilder();

            foreach (IWebElement movie in movies.Take(5))
            {
                string str_title, str_length, str_rating;
                IWebElement elem_lister_item_title = movie.FindElement(By.CssSelector(".lister-item-header"));
                str_title = elem_lister_item_title.Text;

                IWebElement elem_lister_item_length = movie.FindElement(By.CssSelector(".runtime"));
                str_length = elem_lister_item_length.Text;

                IWebElement elem_lister_item_rating = movie.FindElement(By.CssSelector(".metascore"));
                str_rating = elem_lister_item_rating.Text;

                var newLine1 = ("******* Movie " + vcount + " *******");
                csv.AppendLine(newLine1);
                var newLine2 = ("Movie Title: " + str_title);
                csv.AppendLine(newLine2);
                var newLine3 = ("Movie length: " + str_length);
                csv.AppendLine(newLine3);
                var newLine4 = ("Movie Rating: " + str_rating);
                csv.AppendLine(newLine4);
                var newLine5 = ("\n");
                csv.AppendLine(newLine5);
                vcount++;
            }

            Console.WriteLine("Scraping Data from YouTube channel Passed");
            File.WriteAllText("C:/Users/Simon/Downloads/imdb.csv", csv.ToString(), Encoding.UTF8);
        }
    }
}
