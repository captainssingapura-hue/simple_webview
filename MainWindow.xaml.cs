using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.UI;

namespace WinUI3TabsApp;

public sealed partial class MainWindow : Window
{
    private int _counterValue = 0;
    private int _dynamicTabCount = 0;

    public MainWindow()
    {
        this.InitializeComponent();
        this.ExtendsContentIntoTitleBar = true;
        SetTitleBar(CustomDragRegion);
        AppWindow.Resize(new Windows.Graphics.SizeInt32(1000, 700));
        this.TabView_AddTabButtonClick(MainTabView, null);
    }

    private void TabView_AddTabButtonClick(TabView sender, object args)
    {
        addTabWithUrl(sender);
    }

    private void addTabWithUrl(TabView tabView, string url = "http://localhost:8888/html/landing.html")
    {
        _dynamicTabCount++;
        var newTab = CreateDynamicTab(_dynamicTabCount, url);
        tabView.TabItems.Add(newTab);
        tabView.SelectedItem = newTab;
    }

    private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        sender.TabItems.Remove(args.Tab);
    }

    private TabViewItem CreateDynamicTab(int number, string url)
    {
        var tab = new TabViewItem { Header = "🌐 New Tab", IsClosable = true };

        // Root layout
        var root = new Grid();
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        // Toolbar
        var toolbar = new Grid { Padding = new Thickness(8, 6, 8, 6) };
        toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        var backBtn = new Button { Content = "←", Width = 36, Margin = new Thickness(0, 0, 4, 0), IsEnabled = false };
        var forwardBtn = new Button { Content = "→", Width = 36, Margin = new Thickness(0, 0, 4, 0), IsEnabled = false };
        var refreshBtn = new Button { Content = "⟳", Width = 36, Margin = new Thickness(0, 0, 8, 0), IsEnabled = false };
        var addressBox = new TextBox { PlaceholderText = "Enter a URL...", Text = url, VerticalAlignment = VerticalAlignment.Center };
        var userBtn = new Button { Content = Environment.UserName, Margin = new Thickness(8, 0, 0, 0), IsEnabled = true,
            Style = (Style)Microsoft.UI.Xaml.Application.Current.Resources["AccentButtonStyle"] };
        //var userBtn = CreateAvatarButton("SB", Color.FromArgb(255, 178, 190, 181));
        userBtn.Click += (s, e) =>
        {
            this.addTabWithUrl(MainTabView, "http://localhost:8888/html/me.html");
        };

        Grid.SetColumn(backBtn,    0); toolbar.Children.Add(backBtn);
        Grid.SetColumn(forwardBtn, 1); toolbar.Children.Add(forwardBtn);
        Grid.SetColumn(refreshBtn, 2); toolbar.Children.Add(refreshBtn);
        Grid.SetColumn(addressBox, 3); toolbar.Children.Add(addressBox);
        //Grid.SetColumn(goBtn,      4); toolbar.Children.Add(goBtn);
        Grid.SetColumn(userBtn, 5);    toolbar.Children.Add(userBtn);
        Grid.SetRow(toolbar, 0);

        // WebView2
        var webView = new Microsoft.UI.Xaml.Controls.WebView2();
        Grid.SetRow(webView, 1);

        void NavigateTo(string url)
        {
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                url = "https://" + url;
            webView.Source = new Uri(url);
        }

        // Step 1: wire everything inside CoreWebView2Initialized — guaranteed core exists here
        webView.CoreWebView2Initialized += (s, e) =>
        {
           // goBtn.IsEnabled      = true;
            refreshBtn.IsEnabled = true;

            webView.CoreWebView2.DocumentTitleChanged += (core, _) =>
            {
                var t = core.DocumentTitle;
                tab.Header = string.IsNullOrWhiteSpace(t) ? "🌐 Tab" : $"🌐 {t}";
            };

            webView.NavigationStarting  += (_, ne) => { addressBox.Text = ne.Uri; tab.Header = "🌐 Loading…"; };
            webView.NavigationCompleted += (_, _)  => { backBtn.IsEnabled = webView.CanGoBack; forwardBtn.IsEnabled = webView.CanGoForward; };

            //goBtn.Click      += (_, _) => NavigateTo(addressBox.Text.Trim());
            refreshBtn.Click += (_, _) => webView.Reload();
            backBtn.Click    += (_, _) => { if (webView.CanGoBack)    webView.GoBack(); };
            forwardBtn.Click += (_, _) => { if (webView.CanGoForward) webView.GoForward(); };
            addressBox.KeyDown += (_, ke) => { if (ke.Key == Windows.System.VirtualKey.Enter) NavigateTo(addressBox.Text.Trim()); };

            NavigateTo(url);
        };

        // Step 2: call EnsureCoreWebView2Async only once the control is in the visual tree
        webView.Loaded += async (s, e) =>
        {
            await webView.EnsureCoreWebView2Async();
            webView.CoreWebView2.NewWindowRequested += (sender, e) =>
            {
                // Block the new window from opening
                e.Handled = true;
            };
        };

        root.Children.Add(toolbar);
        root.Children.Add(webView);
        tab.CloseRequested += (_, _) => webView.Close();
        tab.Content = root;
      

        return tab;
    }

    private Button CreateAvatarButton(string initials, Color bgColor, double size = 28)
    {
        string user = Environment.UserName;
        var circle = new Ellipse
        {
            Width = size,
            Height = size,
            Fill = new SolidColorBrush(bgColor)
        };

        var text = new TextBlock
        {
            Text = user.Substring(0, 2).ToUpper(),
            FontFamily = new FontFamily("Segoe UI"),
            FontSize = size * 0.58,
            FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
            Foreground = new SolidColorBrush(Colors.White),
            HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Center,
            VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Center
        };

        var grid = new Grid { Width = size, Height = size };
        grid.Children.Add(circle);
        grid.Children.Add(text);

        return new Button
        {
            Content = grid,
            Padding = new Microsoft.UI.Xaml.Thickness(0),
            BorderBrush = new SolidColorBrush(Colors.Transparent),
            BorderThickness = new Thickness(0)
        };

    }

}
