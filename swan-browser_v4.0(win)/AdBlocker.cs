using Microsoft.Web.WebView2.Core;

namespace SwanBrowser2
{
    /// <summary>
    /// Lightweight ad-blocker that works in two layers:
    ///   1. Network layer  – blocks requests whose URL matches known ad/tracker domains
    ///   2. DOM layer      – injects CSS to hide common ad slots that slip through
    /// </summary>
    public static class AdBlocker
    {
        // Common ad/tracker domains — extend freely
        private static readonly HashSet<string> BlockedDomains = new(StringComparer.OrdinalIgnoreCase)
        {
            "doubleclick.net", "googleadservices.com", "googlesyndication.com",
            "adservice.google.com", "pagead2.googlesyndication.com",
            "ads.youtube.com", "ad.doubleclick.net",
            "amazon-adsystem.com", "s.amazon-adsystem.com",
            "facebook.com/tr", "connect.facebook.net",
            "analytics.google.com", "google-analytics.com", "googletagmanager.com",
            "hotjar.com", "fullstory.com", "loggly.com",
            "outbrain.com", "taboola.com", "revcontent.com",
            "moatads.com", "criteo.com", "criteo.net",
            "adsrvr.org", "advertising.com", "adroll.com",
            "rubiconproject.com", "pubmatic.com", "openx.net",
            "casalemedia.com", "appnexus.com", "smartadserver.com",
            "scorecardresearch.com", "quantserve.com", "comscore.com",
            "chartbeat.com", "parsely.com", "newrelic.com",
            "ads.twitter.com", "ads.linkedin.com", "snap.licdn.com",
            "bat.bing.com", "clarity.ms",
        };

        // CSS injected into every page to hide residual ad elements
        private const string HideAdsCss = @"
            [id*='google_ads'], [id*='div-gpt-ad'],
            [class*='google-ad'], [class*='GoogleAd'],
            [id*='ad-'], [id*='-ad'], [class*=' ad '],
            [class*='adsbygoogle'], ins.adsbygoogle,
            [id*='taboola'], [id*='outbrain'],
            [class*='sponsored'], [data-ad-slot],
            iframe[src*='doubleclick'], iframe[src*='googlesyndication'],
            #carbonads, .carbon-ads {
                display: none !important;
                visibility: hidden !important;
                height: 0 !important;
                width: 0 !important;
                pointer-events: none !important;
            }";

        public static void Attach(CoreWebView2 core)
        {
            // Layer 1: block at network level
            core.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
            core.WebResourceRequested += OnWebResourceRequested;

            // Layer 2: inject hiding CSS after every navigation
            core.NavigationCompleted += async (s, e) =>
            {
                try
                {
                    await core.ExecuteScriptAsync(
                        $@"(function(){{
                            var style = document.createElement('style');
                            style.textContent = {System.Text.Json.JsonSerializer.Serialize(HideAdsCss)};
                            document.head.appendChild(style);
                        }})();");
                }
                catch { }
            };
        }

        public static void Detach(CoreWebView2 core)
        {
            core.WebResourceRequested -= OnWebResourceRequested;
        }

        private static void OnWebResourceRequested(object? sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            try
            {
                if (Uri.TryCreate(e.Request.Uri, UriKind.Absolute, out Uri? uri))
                {
                    string host = uri.Host.TrimStart('w', '.');
                    if (IsBlocked(host))
                    {
                        // Return empty 200 — avoids console errors while still blocking
                        if (sender is CoreWebView2 core)
                        {
                            e.Response = core.Environment.CreateWebResourceResponse(
                                null, 200, "OK", "Content-Type: text/plain");
                        }
                    }
                }
            }
            catch { }
        }

        private static bool IsBlocked(string host)
        {
            foreach (string blocked in BlockedDomains)
            {
                if (host.Equals(blocked, StringComparison.OrdinalIgnoreCase) ||
                    host.EndsWith("." + blocked, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
