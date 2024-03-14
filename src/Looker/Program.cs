using DnsClient;
using OpenQA.Selenium.Edge;
using PuppeteerSharp;
using System.Net;

class Program
{
    static HashSet<string> _urls = new HashSet<string>();
    static Dictionary<string, HashSet<string>> _results = new Dictionary<string, HashSet<string>>(); // key: url, value: IP
    static HashSet<string> _dns = new HashSet<string>
    {
        "168.126.63.1", // KT
        "210.220.163.82", //Default DNS1
        "164.124.101.2", //Default DNS2
        "8.8.8.8",
        "1.1.1.1",
        "208.67.222.222",
        "9.9.9.9",
    };
    static string _resultSaveFileName = "result.txt";

    /// <summary>
    /// Main Method
    /// </summary>
    /// <returns></returns>
    static async Task Main()
    {
        var startAtUrl = "https://www.google.com";
        var input = string.Empty;

        bool isLoop = true;
        while (isLoop)
        {
            LoggingWithColor("Please enter the address of the web page you want to visit: ", ConsoleColor.Magenta);
            LoggingWithColor("=> i.g. https://www.google.com", ConsoleColor.Green);
            input = Console.ReadLine();
            var paramUri = input;

            if (Uri.TryCreate(paramUri, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                startAtUrl = paramUri.ToString();
                Console.Clear();
                isLoop = false;
            }
            else
            {
                LoggingWithColor("Invalid Url, please re-enter it.\n", ConsoleColor.Red);
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.White;
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
        Console.WriteLine("Starting download BrowserFetcher.....");
        await new BrowserFetcher().DownloadAsync();
        Console.WriteLine("Successed download");


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
                    LoggingWithColor($"Collecting request URL: {v}", ConsoleColor.Yellow);
                    await page.GoToAsync(v);
                }

                Console.WriteLine();
                Task.Delay(2000).Wait();

                // Resolve urls
                foreach (var url in _urls)
                {
                    LoggingWithColor($"Resolve IP from DNS: {url}", ConsoleColor.Yellow);
                    await LookupUrl(url);
                }


                if (File.Exists(_resultSaveFileName))
                {
                    File.Delete(_resultSaveFileName);
                }

                // Display results
                Console.WriteLine();
                LoggingWithColor($"########## Results #########", ConsoleColor.Green);
                foreach (var item in _results)
                {
                    foreach (var ip in item.Value)
                    {
                        var msg = $"Host: {item.Key}\nResolved: {ip}\n";
                        LoggingWithColor(msg, ConsoleColor.Green);
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
                var onlyHost = GetOnlyHostInUrl(url);

                if (string.IsNullOrEmpty(onlyHost) is not true)
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
                foreach (var dns in _dns)
                {
                    var dnsIp = IPAddress.Parse(dns);

                    if (dnsIp is not null)
                    {
                        var ips = await GetDnsQueryResultAsync(dnsIp, new HashSet<string> { host });

                        foreach (var ip in ips)
                        {

                            _results.TryGetValue(host, out var preValues);

                            var inputvalues = new HashSet<string> { ip };

                            if (preValues is not null)
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

            /// <summary>
            /// Step03-1: Recursive method
            /// </summary>
            /// <param name="ip"></param>
            /// <param name="hostNames"></param>
            /// <returns></returns>
            static async Task<HashSet<string>> GetDnsQueryResultAsync(IPAddress ip, HashSet<string> hostNames)
            {
                var ips = new HashSet<string>();


                foreach (var host in hostNames)
                {
                    try
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
                    catch (Exception ex)
                    {
                        LoggingWithColor($"There was a problem solving the name in DNS.", ConsoleColor.Red);
                        LoggingWithColor($"Dns: {ip}\nHostName: {host}", ConsoleColor.Red);
                        LoggingWithColor(ex.Message, ConsoleColor.Red);
                    }

                }

                return ips;
            }



           
        }
    }

    static void LoggingWithColor(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}