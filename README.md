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
