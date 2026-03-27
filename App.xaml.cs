using Microsoft.UI.Xaml;

namespace WinUI3TabsApp;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        // Parse command-line arguments: --url <url> --x <x> --y <y> --width <w> --height <h>
        var cmdArgs = Environment.GetCommandLineArgs();
        string? url = null;
        int? x = null, y = null, width = null, height = null;

        for (int i = 1; i < cmdArgs.Length - 1; i++)
        {
            switch (cmdArgs[i])
            {
                case "--url":    url = cmdArgs[++i]; break;
                case "--x":      x = int.Parse(cmdArgs[++i]); break;
                case "--y":      y = int.Parse(cmdArgs[++i]); break;
                case "--width":  width = int.Parse(cmdArgs[++i]); break;
                case "--height": height = int.Parse(cmdArgs[++i]); break;
            }
        }

        var window = new MainWindow(url);
        window.Closed += (s, e) => Application.Current.Exit();

        if (x.HasValue && y.HasValue)
            window.AppWindow.Move(new Windows.Graphics.PointInt32(x.Value, y.Value));
        if (width.HasValue && height.HasValue)
            window.AppWindow.Resize(new Windows.Graphics.SizeInt32(width.Value, height.Value));

        window.Activate();
    }
}
