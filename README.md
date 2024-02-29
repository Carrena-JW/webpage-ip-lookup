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

- Sample for exported urls
- https://image.yes24.com/images/01_Banner/quickCate/2024/qDVD_0109.png
- https://image.yes24.com/sysimage/Contents/Scripts/m/common/xds.min.js?v=20210909
- https://image.yes24.com/sysimage/Contents/Scripts/p/jquery/jquery-1.12.4.min.js
- https://image.yes24.com/sysimage/Contents/Scripts/p/jquery/jquery-migrate-1.4.1.min.js?v=20220321
- https://image.yes24.com/sysimage/Contents/Scripts/p/jquery/jquery.menu-aim.js?v=20220321
- https://image.yes24.com/sysimage/Contents/Scripts/p/jquery/jquery.easing.1.3.min.js?v=20220321
- https://www.yes24.com/JavaScript/util.js?v=20230919
- https://www.yes24.com/JavaScript/Yes_header.js?v=20230620
- https://www.yes24.com/JavaScript/hiveSlide.js?v=20231017
- https://www.yes24.com/JavaScript/jqueryExtends.js?v=20231017
- https://www.yes24.com/JavaScript/recentviewgoods.js?v=20230920
- https://www.yes24.com/Javascript/Search/bulletSearch.js?v=20240228
- https://www.yes24.com/24/Scripts/YES24.Common-1.6.js?v=20240226
- https://www.yes24.com/JavaScript/redirectWebSite.js?v=20240223
- https://image.yes24.com/sysimage/Contents/Scripts/d/scl/simplebar.js?v=20240222
- https://image.yes24.com/sysimage/Contents/Scripts/p/jquery/jquery.lazyload.min.1.9.7.js?v=20220321
- https://image.yes24.com/sysimage/yesUI/yesCom.css?v=20240119
- https://image.yes24.com/sysimage/yesUI/layout/yesHeaderN4.css?v=20231206
- https://image.yes24.com/sysimage/yesUI/layout/yesFooterN4.css?v=20240226
- https://image.yes24.com/sysimage/yesUI/layout/wideLayout.css?v=20240229
- https://image.yes24.com/sysimage/yesUI/layout/simplebar.css?v=20240222
- https://image.yes24.com/sysimage/yesUI/welcome/yesWelcomeN4.css?v=20231017
- https://image.yes24.com/sysimage/yesUI/yesUI.css?v=20240220
- https://www.yes24.com/javascript/common.js?v=20231026
- https://www.yes24.com/JavaScript/headerSetting.js?v=20231017
- https://image.yes24.com/sysimage/Contents/Scripts/c/logging/clickstream.min.js?v=20211110
- https://image.yes24.com/sysimage/Contents/Scripts/c/logging/dfinery.min.js?v=20240220
- https://image.yes24.com/sysimage/Contents/Scripts/c/logging/dfineryHelper.js?v=20240220
- https://www.googletagmanager.com/gtag/js?id=G-FJT6RQ6VPQ
- https://image.yes24.com/images/01_Banner/welcome/topBanner/2024/0227_topbn_480x50_L_01.png
- https://image.yes24.com/images/01_Banner/welcome/topBanner/2024/0227_topbn_480x50_R_01.png
- https://image.yes24.com/images/01_Banner/welcome/topBanner/2024/0227_topbn_480x50_L_04.png
- https://image.yes24.com/images/01_Banner/welcome/topBanner/2024/0227_topbn_480x50_R_04.png
- https://image.yes24.com/images/01_Banner/welcome/topBanner/2024/0227_topbn_480x50_L_05.png
- https://image.yes24.com/images/01_Banner/welcome/topBanner/2024/0227_topbn_480x50_R_05.png
- https://adgirl.yes24.com/RealMedia/ads/adstream_mjx.ads/www.yes24.com/Header/SearchBar@x01
- https://adgirl.yes24.com/RealMedia/ads/adstream_jx.ads/www.yes24.com/SearchCenter/BulletSearchBig@Top1,Top2
- https://image.yes24.com/images/00_Event/2024/0116bookfood/GNB_2x_360x224.jpg
- https://image.yes24.com/dms/202011/GNB%EC%96%B4%EA%B9%A8%EB%B0%B0%EB%84%88_180x112(2).jpg
- https://image.yes24.com/momo/TopCate4431/MidCate007/443062264.jpg
- https://image.yes24.com/dms/202402/PCGNB_super_240219.jpg
- https://image.yes24.com/sysimage/renew/blank.gif
- https://image.yes24.com/images/01_Banner/quickCate/2024/qBook_0228.png
- https://image.yes24.com/images/01_Banner/quickCate/2022/qFBook_0126.png
- https://image.yes24.com/images/01_Banner/quickCate/2022/qUsed_0126.png
- https://image.yes24.com/images/01_Banner/quickCate/2024/qEBook_0129.png
- https://image.yes24.com/images/01_Banner/quickCate/2023/qCrema_0818.png
- https://image.yes24.com/images/01_Banner/quickCate/2023/qCDLP_0619.png
- https://image.yes24.com/images/01_Banner/quickCate/2024/qDVD_0109.png
- https://image.yes24.com/images/01_Banner/quickCate/2022/qGift_0126.png
- https://image.yes24.com/images/01_Banner/quickCate/2022/qTicket_0126.png
- https://adgirl.yes24.com/RealMedia/ads/adstream_mjx.ads/www.yes24.com/affiliated_floating@Right
- https://adgirl.yes24.com/RealMedia/ads/adstream_mjx.ads/www.yes24.com/affiliated_pop_up@Frame1
- https://www.yes24.com/Javascript/Welcome/welcome_top.js?v=20220321
- https://www.yes24.com/resource/css/renew/yesWelcomePopup.css?ver=141113
- https://image.yes24.com/momo/TopCate935/MidCate010/93496696.jpg
- https://www.yes24.com/Resource/css/renew/yesPartnerPopup.css
- https://adgirl.yes24.com/RealMedia/ads/adstream_mjx.ads/www.yes24.com/center/@x70
- https://adgirl.yes24.com/RealMedia/ads/adstream_mjx.ads/www.yes24.com/center/@x71,x72,x73
- https://adgirl.yes24.com/RealMedia/ads/adstream_mjx.ads/www.yes24.com/center/@x74,x75,x76
- https://adgirl.yes24.com/RealMedia/ads/adstream_mjx.ads/www.yes24.com/center/@x77,x78,x79
- https://image.yes24.com/images/00_Event/2024/0213YesFunding_a/bn_760x580.jpg
- https://image.yes24.com/images/00_Event/2024/0216YesFunding_b/bn_760x580.jpg
- https://image.yes24.com/images/00_Event/2024/0223YesFunding/bn_760x580.jpg
- https://image.yes24.com/images/00_Event/2024/0208YesFunding/bn_760x580.jpg
- https://image.yes24.com/images/00_Event/2024/0215YesFunding/bn_760x580.jpg
- https://adgirl.yes24.com/RealMedia/ads/adstream_mjx.ads/www.yes24.com/Main/welcome.aspx@BottomLeft
- https://image.yes24.com/Goods/125080094/L
- https://image.yes24.com/Goods/125075292/L
- https://image.yes24.com/Goods/124939156/L
- https://image.yes24.com/Goods/125101034/L
- https://image.yes24.com/Goods/125184026/S
- https://image.yes24.com/Goods/122120495/S
- https://image.yes24.com/Goods/123675187/S
- https://image.yes24.com/Goods/117014613/S
- https://image.yes24.com/Goods/123400303/S
- https://image.yes24.com/Goods/125131238/S
- https://image.yes24.com/Goods/124702623/S
- https://image.yes24.com/Goods/125101034/S
- https://image.yes24.com/Goods/124043812/S
- https://image.yes24.com/Goods/124702605/S
- https://adgirl.yes24.com/RealMedia/ads/adstream_mjx.ads/momo.yes24.com/Main/welcome.aspx@Middle1,Middle2
- https://image.yes24.com/momo/Noimg_L.gif