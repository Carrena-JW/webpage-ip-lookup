using DnsClient;
using OpenQA.Selenium.Edge;
using PuppeteerSharp;
using System.Net;

class Program
{
    static HashSet<string> _urls = new HashSet<string>();
    static Dictionary<string,HashSet<string>> _results =new Dictionary<string, HashSet<string>>(); // key: url, value: IP
    static HashSet<string> _dns = new HashSet<string>
    {
        "168.126.63.1", // KT
        "192.168.103.140", //Default DNS1
        "192.168.103.141", //Default DNS2
    };


    /// <summary>
    /// Main Method
    /// </summary>
    /// <returns></returns>
    static async Task Main(string[] args)
    {
        var startAtUrl = "https://www.google.com";


        if(args.Length > 0 )
        {
            var paramUri = args[0];

            if (Uri.TryCreate(paramUri, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
               startAtUrl = paramUri.ToString();
            }
 
        }

        //Run browser
        var driver = new EdgeDriver();
        var visitedUrls = new HashSet<string>();
         
        driver.Url = startAtUrl;

        while (true)
        {
            try
            {
                Task.Delay(100).Wait();

                visitedUrls.Add(driver.Url);

                if (driver.WindowHandles.Count == 0)
                {
                    break;
                }
            }
            catch (Exception ex)
            {
                // If occur exceptions, consider exit browser.
                break;
            }
        }

        driver.Quit();
        Console.Clear();
        


        // Init browser
        await new BrowserFetcher().DownloadAsync();

        // Run browser
        using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        
        // Create new tab
        using var page = await browser.NewPageAsync();

        // Catch request urls
        page.Request += (sender, e) => SaveUrl(e.Request.Url);

        // loop visitedUrl
        foreach (var v in visitedUrls)
        {
            // Go to specific url
            await page.GoToAsync(v);
        }

        // Resolve urls
        foreach(var url in _urls)
        {
            await LookupUrl(url);
        }

        // Display results
        foreach(var item in _results)
        {
            foreach(var ip in item.Value)
            {
                Console.WriteLine($"Host: {item.Key}\nResolved: {ip}\n");
            }
        }
    }

    /// <summary>
    /// Step01 
    /// </summary>
    /// <param name="url"></param>
    static void SaveUrl(string url)
    {
       var onlyHost =   GetOnlyHostInUrl(url);

        if(string.IsNullOrEmpty(onlyHost) is not true)
        {
            _urls.Add(onlyHost);
        }
    }

    /// <summary>
    /// Step02
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    static string GetOnlyHostInUrl(string url)
    {
        var uri = new Uri(url);
        return uri.Host;
    }

    /// <summary>
    /// Step03
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    static async Task LookupUrl(string host)
    {
        foreach(var dns in _dns)
        {
            var dnsIp = IPAddress.Parse(dns);

            if(dnsIp is not null)
            {
                var lookup = new LookupClient(dnsIp);
                var result = await lookup.QueryAsync(host, QueryType.ANY);
                var records = result.Answers.ARecords();

                var ips =  records.Select(x =>  x.Address.ToString());

                foreach(var ip in ips)
                {

                    _results.TryGetValue(host, out var preValues);

                    var inputvalues = new HashSet<string> { ip };
                    
                    if(preValues is not null)
                    {
                        var concatArray = inputvalues.Concat(preValues);
                        inputvalues = concatArray.ToHashSet();
                    }

                    inputvalues.Add(ip);

                    _results[host] = inputvalues;

                }
            }
        }
    }
}