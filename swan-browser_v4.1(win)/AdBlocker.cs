using Microsoft.Web.WebView2.Core;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SwanBrowser7
{

    //
    //

    public static class AdBlocker
    {
        // ── Filter cache paths ────────────────────────────────────────────
        private static readonly string _rawCachePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SwanBrowser", "filterlists.raw");

        private static readonly string _parsedCachePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SwanBrowser", "filterlists.json");

        // Filter list URLs (EasyList + EasyPrivacy)
        private static readonly string[] _filterListUrls =
        {
            "https://easylist.to/easylist/easylist.txt",
            "https://easylist.to/easylist/easyprivacy.txt",
        };

        // Re-download every 7 days
        private const int CacheDays = 7;

        private static readonly HttpClient _http = new()
        {
            Timeout = TimeSpan.FromSeconds(20),
            DefaultRequestHeaders = { { "User-Agent", "SwanBrowser/2.0 AdBlocker" } }
        };

        // ── Domain set (volatile reference swap for thread safety) ─────────
        private static volatile HashSet<string> _domains =
            new(_builtinDomains, StringComparer.OrdinalIgnoreCase);

        // ── CSS to inject after every page load ───────────────────────────
        private static volatile string _cssToInject = _builtinCss;

        private static bool _initialized;
        private static readonly SemaphoreSlim _initLock = new(1, 1);

        // ─────────────────────────────────────────────────────────────────
        // Public API
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Call once at startup (await in Form1_Load).  Loads the parsed cache
        /// instantly if fresh, otherwise downloads filter lists in the background.
        /// </summary>
        public static async Task InitializeAsync()
        {
            await _initLock.WaitAsync();
            try
            {
                if (_initialized) return;

                bool cacheStale = !File.Exists(_parsedCachePath) ||
                    (DateTime.UtcNow - File.GetLastWriteTimeUtc(_parsedCachePath)).TotalDays > CacheDays;

                if (!cacheStale)
                {
                    await LoadParsedCacheAsync();
                }
                else
                {
                    // Load from parsed cache now (if it exists), fetch fresh copy in background
                    if (File.Exists(_parsedCachePath))
                        await LoadParsedCacheAsync();

                    _ = Task.Run(DownloadAndParseAsync); // fire-and-forget
                }

                _initialized = true;
            }
            catch { }
            finally { _initLock.Release(); }
        }

        public static void Attach(CoreWebView2 core)
        {
            core.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
            core.WebResourceRequested += OnWebResourceRequested;

            core.NavigationCompleted += async (s, e) =>
            {
                try
                {
                    string css = _cssToInject;
                    await core.ExecuteScriptAsync(
                        $@"(function(){{
                            var style = document.createElement('style');
                            style.id = '__swan_adblocker';
                            if (!document.getElementById('__swan_adblocker')){{
                                style.textContent = {JsonSerializer.Serialize(css)};
                                document.head.appendChild(style);
                            }}
                        }})();");
                }
                catch { }
            };
        }

        public static void Detach(CoreWebView2 core)
        {
            core.WebResourceRequested -= OnWebResourceRequested;
        }

        // ─────────────────────────────────────────────────────────────────
        // Network interception
        // ─────────────────────────────────────────────────────────────────

        private static void OnWebResourceRequested(object? sender,
            CoreWebView2WebResourceRequestedEventArgs e)
        {
            try
            {
                if (!Uri.TryCreate(e.Request.Uri, UriKind.Absolute, out Uri? uri)) return;

                string host = uri.Host.ToLowerInvariant();
                // Strip leading "www." for matching
                if (host.StartsWith("www.")) host = host[4..];

                if (IsBlocked(host) && sender is CoreWebView2 core)
                {
                    e.Response = core.Environment.CreateWebResourceResponse(
                        null, 200, "OK", "Content-Type: text/plain");
                }
            }
            catch { }
        }

        private static bool IsBlocked(string host)
        {
            var domains = _domains; // snapshot the reference once
            if (domains.Contains(host)) return true;

            // Walk up subdomains: a.b.example.com -> b.example.com -> example.com
            int dot = host.IndexOf('.');
            while (dot >= 0 && dot < host.Length - 1)
            {
                string parent = host[(dot + 1)..];
                if (domains.Contains(parent)) return true;
                dot = host.IndexOf('.', dot + 1);
            }
            return false;
        }

        // ─────────────────────────────────────────────────────────────────
        // Filter list download + parse
        // ─────────────────────────────────────────────────────────────────

        private static async Task DownloadAndParseAsync()
        {
            try
            {
                var sb = new System.Text.StringBuilder();
                foreach (string url in _filterListUrls)
                {
                    try { sb.AppendLine(await _http.GetStringAsync(url)); }
                    catch { }
                }

                string raw = sb.ToString();
                if (raw.Length < 1000) return; // download clearly failed

                Directory.CreateDirectory(Path.GetDirectoryName(_rawCachePath)!);
                await File.WriteAllTextAsync(_rawCachePath, raw);

                var (domains, cssRules) = ParseFilterList(raw);
                await SaveParsedCacheAsync(domains, cssRules);
                ApplyParsed(domains, cssRules);
            }
            catch { }
        }

        private static async Task LoadParsedCacheAsync()
        {
            try
            {
                string json = await File.ReadAllTextAsync(_parsedCachePath);
                var cache = JsonSerializer.Deserialize<FilterCache>(json);
                if (cache == null) return;
                ApplyParsed(cache.Domains, cache.CssRules);
            }
            catch { }
        }

        private static async Task SaveParsedCacheAsync(List<string> domains, List<string> cssRules)
        {
            try
            {
                var cache = new FilterCache { Domains = domains, CssRules = cssRules };
                string json = JsonSerializer.Serialize(cache,
                    new JsonSerializerOptions { WriteIndented = false });
                await File.WriteAllTextAsync(_parsedCachePath, json);
            }
            catch { }
        }

        private static void ApplyParsed(List<string> domains, List<string> cssRules)
        {
            // Merge downloaded domains with the builtin list
            var merged = new HashSet<string>(_builtinDomains, StringComparer.OrdinalIgnoreCase);
            foreach (string d in domains) merged.Add(d);
            _domains = merged; // atomic reference swap

            // Build combined CSS
            string combined = _builtinCss;
            if (cssRules.Count > 0)
                combined += "\n" + string.Join(",\n", cssRules) +
                            " { display:none!important; visibility:hidden!important; }";
            _cssToInject = combined;
        }

        /// <summary>
        /// Parses EasyList/ABP filter syntax.
        /// Extracts ||domain^ rules and ##selector rules.
        /// </summary>
        private static (List<string> domains, List<string> cssRules) ParseFilterList(string text)
        {
            // Matches: ||ads.example.com^  or  ||ads.example.com^$options
            var domainRx = new Regex(
                @"^\|\|([a-z0-9][a-z0-9\-\.]*[a-z0-9])\^",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Matches global CSS hides: ##.selector  (no domain before ##)
            // We skip exception rules (#@#) and per-domain rules (domain##...)
            var cssRx = new Regex(@"^##(.+)$", RegexOptions.Compiled);

            var domains  = new List<string>(60_000);
            var cssRules = new List<string>(500);

            foreach (string rawLine in text.Split('\n'))
            {
                string line = rawLine.Trim();
                if (line.Length == 0 || line[0] == '!' || line[0] == '[') continue;

                // Domain blocking rule
                var dm = domainRx.Match(line);
                if (dm.Success)
                {
                    domains.Add(dm.Groups[1].Value.ToLowerInvariant());
                    continue;
                }

                // Global CSS hiding rule (limit to keep injection small)
                if (cssRules.Count < 400 && !line.Contains("#@#") && !line.StartsWith("@@"))
                {
                    var cm = cssRx.Match(line);
                    if (cm.Success)
                    {
                        string sel = cm.Groups[1].Value;
                        // Skip overly complex selectors
                        if (sel.Length < 120 && !sel.Contains(':'))
                            cssRules.Add(sel);
                    }
                }
            }

            return (domains, cssRules);
        }

        // ─────────────────────────────────────────────────────────────────
        // Built-in domain list  (fallback, always active)
        // ─────────────────────────────────────────────────────────────────

        private static readonly HashSet<string> _builtinDomains = new(StringComparer.OrdinalIgnoreCase)
        {
            // Google advertising & tracking
            "doubleclick.net", "googleadservices.com", "googlesyndication.com",
            "adservice.google.com", "pagead2.googlesyndication.com",
            "ads.youtube.com", "ad.doubleclick.net", "googletagservices.com",
            "googletagmanager.com", "google-analytics.com", "analytics.google.com",
            "adwords.google.com", "adsense.google.com",

            // Amazon ads
            "amazon-adsystem.com", "s.amazon-adsystem.com", "aax.amazon-adsystem.com",
            "fls-na.amazon.com",

            // Meta / Facebook
            "facebook.com/tr", "connect.facebook.net", "an.facebook.com",
            "pixel.facebook.com", "tr.facebook.com",

            // Twitter / X
            "ads.twitter.com", "static.ads-twitter.com", "analytics.twitter.com",
            "t.co",

            // LinkedIn
            "ads.linkedin.com", "snap.licdn.com", "p.linkedin.com",

            // Microsoft
            "bat.bing.com", "clarity.ms", "ads.microsoft.com", "c.microsoft.com",

            // TikTok / ByteDance
            "analytics.tiktok.com", "ads.tiktok.com", "log.tiktok.com",
            "mon.tiktok.com", "byteoversea.com", "sgsnssdk.com",

            // Snapchat
            "tr.snapchat.com", "sc-static.net",

            // Pinterest
            "ct.pinterest.com", "analytics.pinterest.com",

            // Major ad networks
            "criteo.com", "criteo.net", "rtax.criteo.com",
            "outbrain.com", "taboola.com", "revcontent.com",
            "moatads.com", "adroll.com", "d.adroll.com",
            "rubiconproject.com", "pubmatic.com", "openx.net",
            "casalemedia.com", "appnexus.com", "smartadserver.com",
            "adsrvr.org", "advertising.com", "media.net",
            "lijit.com", "sonobi.com", "sharethrough.com",
            "contextweb.com", "indexww.com", "triplelift.com",
            "33across.com", "rhythmone.com", "yieldmo.com",
            "undertone.com", "spotxchange.com", "teads.tv",
            "adform.net", "adform.com", "adkernel.com",
            "aolcloud.net", "nexage.com", "brealtime.com",
            "conversantmedia.com", "emxdgt.com", "bidr.io",
            "bidswitch.net", "gumgum.com", "nativo.com",
            "adtech.de", "advertising.com", "yieldlab.net",
            "adnxs.com", "ams.amazon.com",

            // Analytics & session recording
            "hotjar.com", "fullstory.com", "loggly.com",
            "scorecardresearch.com", "quantserve.com", "comscore.com",
            "chartbeat.com", "parsely.com", "newrelic.com",
            "mouseflow.com", "luckyorange.com", "inspectlet.com",
            "mixpanel.com", "segment.com", "segment.io",
            "heapanalytics.com", "amplitude.com",
            "optimizely.com", "crazyegg.com", "vwo.com",
            "intercom.com", "intercom.io",

            // CDN-based ad servers
            "cdn.doubleverify.com", "doubleverify.com",
            "securepubads.g.doubleclick.net", "tpc.googlesyndication.com",
            "imasdk.googleapis.com",

            // Redirect / link tracking
            "redirect.viglink.com", "viglink.com",
            "go.skimresources.com", "skimresources.com",
            "tracking.dpbolvw.net", "apmebf.com",
            "anrdoezrs.net", "jdoqocy.com", "tkqlhce.com",

            // Affiliate trackers
            "shareasale.com", "linksynergy.com", "pjtra.com",
            "pepperjamnetwork.com", "cj.com", "commission-junction.com",

            // Misc privacy-invasive
            "ib.adnxs.com", "cdn.jsdelivr.net.ads",
            "trk.email", "aclk.com", "clickserve.dartsearch.net",
        };

        // ─────────────────────────────────────────────────────────────────
        // Built-in CSS  (hides ad slots that bypass network blocking)
        // ─────────────────────────────────────────────────────────────────

        private const string _builtinCss = @"
            /* Google / AdSense */
            [id*='google_ads'], [id*='div-gpt-ad'],
            [class*='google-ad'], [class*='GoogleAd'],
            [class*='adsbygoogle'], ins.adsbygoogle,
            [data-ad-slot], [data-ad-client], [data-ad-format],
            iframe[src*='googlesyndication'], iframe[src*='doubleclick'],

            /* Generic ad IDs/classes */
            [id^='ad-'], [id$='-ad'], [id^='ads-'], [id$='-ads'],
            [id*='_ad_'], [id*='_ads_'],
            [class*='ad-container'], [class*='ad-wrapper'],
            [class*='ad-slot'], [class*='ad-unit'], [class*='ad-block'],
            [class*='ad-banner'], [class*='ad-holder'], [class*='ad-zone'],
            [class*='advertisement'], [class*='advertise-'],

            /* Taboola / Outbrain */
            [id*='taboola'], [class*='taboola'],
            [id*='outbrain'], [class*='outbrain'],
            .OUTBRAIN, [data-widget-id],

            /* Sponsored / promoted */
            [class*='sponsored'], [class*='Sponsored'],
            [aria-label='Sponsored'], [aria-label='Advertisement'],
            [data-label='Sponsored'],

            /* Carbon ads */
            #carbonads, .carbon-ads, #carbon,

            /* Sticky/floating ad bars */
            [class*='sticky-ad'], [class*='floating-ad'],
            [class*='fixed-ad'], [id*='sticky-ad'],

            /* Newsletter / cookie consent modals (optional) */
            [class*='cookie-banner'], [id*='cookie-banner'],
            [class*='gdpr-banner'], [id*='gdpr'] {
                display: none !important;
                visibility: hidden !important;
                height: 0 !important;
                width: 0 !important;
                max-height: 0 !important;
                overflow: hidden !important;
                pointer-events: none !important;
                opacity: 0 !important;
            }";

        // ─────────────────────────────────────────────────────────────────
        // Cache model
        // ─────────────────────────────────────────────────────────────────

        private class FilterCache
        {
            public List<string> Domains  { get; set; } = new();
            public List<string> CssRules { get; set; } = new();
        }
    }
}
