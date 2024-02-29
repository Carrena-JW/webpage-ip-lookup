# webpage-ip-lookup

### Sample Codes
```cs
using PuppeteerSharp;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // PuppeteerSharp를 사용하기 위해 브라우저를 다운로드 합니다.
        await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

        // 브라우저를 실행합니다.
        using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        }))
        {
            // 새 탭을 엽니다.
            using (var page = await browser.NewPageAsync())
            {
                // 모든 네트워크 요청을 감지하고 URL을 출력합니다.
                page.Request += (sender, e) => Console.WriteLine(e.Request.Url);

                // 웹 페이지로 이동합니다.
                await page.GoToAsync("http://your-url.com");
            }
        }
    }
}

```

- Download packages
```
dotnet add package PuppeteerSharp
```

### sample codes: lookup ip
```cs
using DnsClient;
using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string url = "adgirl.yes24.com";
        string dnsServer = "8.8.8.8";  // Google의 DNS 서버를 사용

        var lookup = new LookupClient(NameServer.GooglePublicDns);  // Google의 DNS 서버를 사용
        var result = await lookup.QueryAsync(url, QueryType.ANY);
        var record = result.Answers.ARecords().FirstOrDefault();

        if (record != null)
        {
            Console.WriteLine($"{url}의 IP 주소는 {record.Address} 입니다.");
        }
        else
        {
            Console.WriteLine($"{url}에 대한 IP 주소를 찾을 수 없습니다.");
        }
    }
}

```
