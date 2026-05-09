using System.Text.Json;

namespace SwanBrowser2
{
    public enum SearchEngine { Google, Bing, DuckDuckGo, Brave, Yahoo }
    public enum BrowserTheme { Dark, Light }

    public class BrowserSettings
    {
        public SearchEngine SearchEngine { get; set; } = SearchEngine.Google;
        public BrowserTheme Theme { get; set; } = BrowserTheme.Dark;
        public bool AdBlockEnabled { get; set; } = true;
        public List<Bookmark> Bookmarks { get; set; } = new();

        private static readonly string _path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SwanBrowser", "settings.json");

        public static BrowserSettings Load()
        {
            try
            {
                if (File.Exists(_path))
                {
                    string json = File.ReadAllText(_path);
                    return JsonSerializer.Deserialize<BrowserSettings>(json) ?? new BrowserSettings();
                }
            }
            catch { }
            return new BrowserSettings();
        }

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
                File.WriteAllText(_path, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch { }
        }

        public string GetSearchUrl(string query)
        {
            string encoded = Uri.EscapeDataString(query);
            return SearchEngine switch
            {
                SearchEngine.Bing       => $"https://www.bing.com/search?q={encoded}",
                SearchEngine.DuckDuckGo => $"https://duckduckgo.com/?q={encoded}",
                SearchEngine.Brave      => $"https://search.brave.com/search?q={encoded}",
                SearchEngine.Yahoo      => $"https://search.yahoo.com/search?p={encoded}",
                _                       => $"https://www.google.com/search?q={encoded}",
            };
        }

        public string GetHomeUrl() => SearchEngine switch
        {
            SearchEngine.Bing       => "https://www.bing.com",
            SearchEngine.DuckDuckGo => "https://duckduckgo.com",
            SearchEngine.Brave      => "https://search.brave.com",
            SearchEngine.Yahoo      => "https://www.yahoo.com",
            _                       => "https://www.google.com",
        };
    }

    public class Bookmark
    {
        public string Title { get; set; } = "";
        public string Url   { get; set; } = "";
    }
}
