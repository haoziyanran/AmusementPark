using System;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace ConsoleApplication
{
    public class Program
    {
        private static readonly string BaseURL = "https://www.douban.com/search?cat=1002&q=";
        public static void Main(string[] args)
        {
            Console.WriteLine("Query: ");
            var q = Console.ReadLine().Trim();
            Console.WriteLine("Requesting...");
            var o = Observable.FromAsync(() => HttpApiClient.GetAsync(BaseURL + q));
            var r = from req in o
                    from str in Observable.FromAsync(() => req.Content.ReadAsStringAsync())
                    select str;

            r.Subscribe((s) => {
                var doc = new HtmlDocument();
                doc.LoadHtml(s);
                var resultListNodes = doc.DocumentNode.SelectSingleNode("//div[@class='result-list']").SelectNodes("div[@class='result']");
                foreach (var resultNode in resultListNodes)
                {
                    var content = resultNode.SelectSingleNode("div[@class='content']");
                    var titleNode = content.SelectSingleNode("div[@class='title']");
                    var categoryString = titleNode.SelectSingleNode("h3/span").InnerText;
                    var titleString = titleNode.SelectSingleNode("h3/a").InnerText;
                    var ratingNode = titleNode.SelectSingleNode("div[@class='rating-info']/span[@class='rating_nums']");
                    var ratingString = "";
                    if (ratingNode != null && ratingNode.InnerText.Length > 0)
                    {
                        ratingString = ratingNode.InnerText;
                        Console.WriteLine(String.Format("{0}{1}({2})", categoryString, titleString, ratingString));
                    }
                    else
                    {
                        Console.WriteLine(String.Format("{0}{1}", categoryString, titleString));
                    }
                }
            });
            Thread.Sleep(5000);
            Console.ReadKey();
        }
    }
}
