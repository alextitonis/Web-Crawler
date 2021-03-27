using System;
using System.Collections.Generic;
using static Web_Crawler.Crawler;

namespace Web_Crawler
{
    class Program
    {
        static List<Crawler> crawlers = new List<Crawler>();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Command: ");
                string command = Console.ReadLine();
                command = command.Trim();
                if (string.IsNullOrEmpty(command))
                    return;

                string[] data = command.Split(' ');
                if (data[0] == "crawl")
                {
                    if (data.Length != 4 && data.Length != 6)
                    {
                        Console.WriteLine("Invalid crawl command parameters ("+data.Length+") !");
                        continue;
                    }

                    Crawler crawler = null;

                    string url = data[1];
                    string className = data[2];
                    Dictionary<string, ObjectType> _data = Utils.stringToDictionary(data[3]);
                    if (data.Length == 4)
                        crawler = new Crawler(url, className, _data);
                    else
                    {
                        string attributeName = data[4];
                        string div = data[5];
                        crawler = new Crawler(url, className, _data, attributeName, div);
                    }

                    if (crawler == null)
                        continue;

                    crawler.onCrawlFinished += (Dictionary<int, (ObjectType, string)> d) =>
                      {
                          Console.WriteLine("A crawler finished");

                          if (crawlers.Contains(crawler))
                              crawlers.Remove(crawler);

                          foreach (var i in d)
                          {
                              Console.WriteLine(i.Value + " | " + i.Key);
                          }
                      };

                    crawlers.Add(crawler);
                }
                else if (data[0] == "help" || data[0] == "?")
                {
                    Console.WriteLine("To start crawling use:");
                    Console.WriteLine("crawl url className object1;object1Type|objectN;ObjectNType attributeName div");
                    Console.WriteLine("You can ignore attributeName and div");
                }
            }
        }
    }
}
