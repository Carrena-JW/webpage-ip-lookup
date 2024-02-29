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

### Dns list
한국의 주요 통신사들이 제공하는 DNS 서버의 주소는 아래와 같습니다:

KT (Korea Telecom):
기본 DNS: 168.126.63.1
보조 DNS: 168.126.63.2
SK Broadband (구 SK 텔레콤):
기본 DNS: 210.220.163.82
보조 DNS: 219.250.36.130
LG U+:
기본 DNS: 164.124.101.2
보조 DNS: 203.248.252.2

Google Public DNS:
기본 DNS: 8.8.8.8
보조 DNS: 8.8.4.4
Cloudflare DNS:
기본 DNS: 1.1.1.1
보조 DNS: 1.0.0.1
OpenDNS:
기본 DNS: 208.67.222.222
보조 DNS: 208.67.220.220
Quad9:
기본 DNS: 9.9.9.9
보조 DNS: 149.112.112.112
DNS.WATCH:
기본 DNS: 84.200.69.80
보조 DNS: 84.200.70.40

### sample codes (javascript)
```javascript
var visitedUrls = [];

window.onpopstate = function(event) {
    addUrl(window.location.href);
};

// 처음 페이지를 로딩할 때 현재 URL을 추가합니다.
addUrl(window.location.href);

function addUrl(url) {
    if (!visitedUrls.includes(url)) {
        visitedUrls.push(url);
    }
}
```

### sample codes
```cs
using System;

class Program
{
    static void Main()
    {
        string urlString = "http://www.example.com";

        Uri uriResult;
        bool result = Uri.TryCreate(urlString, UriKind.Absolute, out uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        if (result)
        {
            Console.WriteLine($"'{urlString}' is a valid URL.");
        }
        else
        {
            Console.WriteLine($"'{urlString}' is not a valid URL.");
        }
    }
}

```
