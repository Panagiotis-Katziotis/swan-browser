namespace SwanBrowser2
{
    public static class ThemeColors
    {
        // ── Dark theme ───────────────────────────────────────────────────
        public static readonly Color DarkFormBg         = Color.FromArgb(18, 18, 18);
        public static readonly Color DarkToolbarBg      = Color.FromArgb(24, 24, 24);
        public static readonly Color DarkTabBarBg       = Color.FromArgb(30, 30, 30);   // area between toolbar and tabs
        public static readonly Color DarkTabActive      = Color.FromArgb(38, 38, 38);
        public static readonly Color DarkTabInactive    = Color.FromArgb(22, 22, 22);
        public static readonly Color DarkButtonBg       = Color.FromArgb(42, 42, 42);
        public static readonly Color DarkButtonFg       = Color.White;
        public static readonly Color DarkUrlBg          = Color.FromArgb(42, 42, 42);
        public static readonly Color DarkUrlFg          = Color.FromArgb(220, 220, 220);
        public static readonly Color DarkTabActiveText  = Color.White;
        public static readonly Color DarkTabInactiveText= Color.FromArgb(160, 160, 160);
        public static readonly Color DarkAccent         = Color.FromArgb(100, 180, 255);

        // ── Light theme ──────────────────────────────────────────────────
        public static readonly Color LightFormBg        = Color.White;
        public static readonly Color LightToolbarBg     = Color.FromArgb(242, 242, 242);
        public static readonly Color LightTabBarBg      = Color.FromArgb(210, 210, 210);  // light gray between toolbar and tabs
        public static readonly Color LightTabActive     = Color.White;
        public static readonly Color LightTabInactive   = Color.FromArgb(225, 225, 225);
        public static readonly Color LightButtonBg      = Color.FromArgb(228, 228, 228);
        public static readonly Color LightButtonFg      = Color.FromArgb(30, 30, 30);
        public static readonly Color LightUrlBg         = Color.White;
        public static readonly Color LightUrlFg         = Color.FromArgb(30, 30, 30);
        public static readonly Color LightTabActiveText = Color.FromArgb(20, 20, 20);
        public static readonly Color LightTabInactiveText=Color.FromArgb(90, 90, 90);
        public static readonly Color LightAccent        = Color.FromArgb(0, 120, 215);

        // ── Helpers ──────────────────────────────────────────────────────
        public static Color FormBg(BrowserTheme t)          => t == BrowserTheme.Dark ? DarkFormBg        : LightFormBg;
        public static Color ToolbarBg(BrowserTheme t)       => t == BrowserTheme.Dark ? DarkToolbarBg     : LightToolbarBg;
        public static Color TabBarBg(BrowserTheme t)        => t == BrowserTheme.Dark ? DarkTabBarBg      : LightTabBarBg;
        public static Color TabActive(BrowserTheme t)       => t == BrowserTheme.Dark ? DarkTabActive     : LightTabActive;
        public static Color TabInactive(BrowserTheme t)     => t == BrowserTheme.Dark ? DarkTabInactive   : LightTabInactive;
        public static Color ButtonBg(BrowserTheme t)        => t == BrowserTheme.Dark ? DarkButtonBg      : LightButtonBg;
        public static Color ButtonFg(BrowserTheme t)        => t == BrowserTheme.Dark ? DarkButtonFg      : LightButtonFg;
        public static Color UrlBg(BrowserTheme t)           => t == BrowserTheme.Dark ? DarkUrlBg         : LightUrlBg;
        public static Color UrlFg(BrowserTheme t)           => t == BrowserTheme.Dark ? DarkUrlFg         : LightUrlFg;
        public static Color TabActiveText(BrowserTheme t)   => t == BrowserTheme.Dark ? DarkTabActiveText : LightTabActiveText;
        public static Color TabInactiveText(BrowserTheme t) => t == BrowserTheme.Dark ? DarkTabInactiveText: LightTabInactiveText;
        public static Color Accent(BrowserTheme t)          => t == BrowserTheme.Dark ? DarkAccent        : LightAccent;
    }
}
