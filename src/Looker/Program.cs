using DnsClient;
using OpenQA.Selenium.Edge;
using PuppeteerSharp;
using System.Net;

class Program
{
    static HashSet<string> _urls = new HashSet<string>();
    static Dictionary<string,HashSet<string>> _results = new Dictionary<string, HashSet<string>>(); // key: url, value: IP
    static HashSet<string> _dns = new HashSet<string>
    {
        "168.126.63.1", // KT
#if DEBUG
       
        "192.168.103.140",
        "192.168.103.141",
#else
        "210.220.163.82", //Default DNS1
        "164.124.101.2", //Default DNS2
        "8.8.8.8",
        "1.1.1.1",
        "208.67.222.222",
        "9.9.9.9",
        "84.200.69.80"
#endif
    };
    static string _resultSaveFileName = "result.txt";

    /// <summary>
    /// Main Method
    /// </summary>
    /// <returns></returns>
    static async Task Main()
    {
        var startAtUrl = "https://www.google.com";

        Console.WriteLine("Enter url:");
        var input = Console.ReadLine();

        if(string.IsNullOrEmpty(input) is not true)
        {
            var paramUri = input;

            if (Uri.TryCreate(paramUri, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
               startAtUrl = paramUri.ToString();
            }
            else
            {
                Console.WriteLine("Invalid Url, exit program");
                return;
            }
 
        }

        //Run browser
        var driver = new EdgeDriver();
        var visitedUrls = new HashSet<string>();
         
        driver.Url = startAtUrl;
        visitedUrls.Add(startAtUrl);

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
        using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
        {
            using (var page = await browser.NewPageAsync())
            {
                // Catch request urls
                page.Request += (sender, e) => SaveUrl(e.Request.Url);

                // loop visitedUrl
                foreach (var v in visitedUrls)
                {
                    SaveUrl(v);
                    // Go to specific url
                    await page.GoToAsync(v);
                }
            }
        }
        
        // Create new tab
        Task.Delay(5000).Wait();

        // Resolve urls
        foreach(var url in _urls)
        {
            await LookupUrl(url);
        }

        
        if (File.Exists(_resultSaveFileName))
        {
            File.Delete(_resultSaveFileName);
        }

        // Display results
        foreach(var item in _results)
        {
            foreach(var ip in item.Value)
            {
                var msg = $"Host: {item.Key}\nResolved: {ip}\n";
                Console.WriteLine(msg);
                await File.AppendAllTextAsync(_resultSaveFileName, msg);
            }
        }

        Console.ReadLine();
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
                var ips = await GetDnsQueryResultAsync(dnsIp, new HashSet<string> { host } );

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

    static async Task<HashSet<string>> GetDnsQueryResultAsync(IPAddress ip, HashSet<string> hostNames)
    {
        var ips = new HashSet<string>();
        
        foreach (var host in hostNames)
        {
            var lookup = new LookupClient(ip);
            var response = await lookup.QueryAsync(host, QueryType.ANY);
            var aRecordIps = response.Answers.ARecords().Select(x => x.Address.ToString()).ToHashSet();

            ips = ips.Concat(aRecordIps).ToHashSet();

            var cNameRecords = response.Answers.CnameRecords();
            if (cNameRecords.Any())
            {
                var cNameResult = await GetDnsQueryResultAsync(ip, cNameRecords.Select(x => x.CanonicalName.Value).ToHashSet());
                ips = ips.Concat(cNameResult).ToHashSet();
            }
        }

        return ips;
    } 
   
}