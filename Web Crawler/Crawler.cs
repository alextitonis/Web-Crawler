using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Web_Crawler
{
    public class Crawler
    {
        public delegate void OnCrawlFinished(Dictionary<int, (ObjectType, string)> data);
        public event OnCrawlFinished onCrawlFinished;

        public Task crawlingTask { get; private set; }

        //Image returns the image link
        public enum ObjectType
        {
            Text, Link, Image
        }

        public Crawler(string url, string className, Dictionary<string, ObjectType> dataNeeded, string attributeName = "class", string div = "div")
        {
            crawlingTask = startCralwerAsync(url, className, dataNeeded, attributeName, div);
        }

        async Task startCralwerAsync(string url, string className, Dictionary<string, ObjectType> dataNeeded, string attributeName = "class", string div = "div")
        {
            HttpClient client = new HttpClient();
            string html = await client.GetStringAsync(url);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            List<HtmlNode> divs = doc.DocumentNode.Descendants(div).Where(x => x.GetAttributeValue(attributeName, "").Equals(className)).ToList();

            Dictionary<int, (ObjectType, string)> data = new Dictionary<int, (ObjectType, string)>();
            for (int i = 0; i < divs.Count; i++)
            {
                foreach(var d in dataNeeded)
                {
                    int index = i;
                    ObjectType value1 = d.Value;
                    string value2 = "";
                    switch (d.Value)
                    {
                        case ObjectType.Text:
                            value2 = divs[i].Descendants(d.Key).FirstOrDefault().InnerText;
                            break;
                        case ObjectType.Link:
                            value2 = divs[i].Descendants(d.Key).FirstOrDefault().ChildAttributes("href").FirstOrDefault().Value;
                            break;
                        case ObjectType.Image:
                            value2 = divs[i].Descendants(d.Key).FirstOrDefault().ChildAttributes("src").FirstOrDefault().Value;
                            break;
                    }

                    data.Add(index, (value1, value2));
                }
            }
            onCrawlFinished?.Invoke(data);
        }
    }
}
