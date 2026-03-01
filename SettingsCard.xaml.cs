using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinUI3TabsApp;

public sealed partial class SettingsCard : UserControl
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(SettingsCard),
            new PropertyMetadata(string.Empty, (d, e) => ((SettingsCard)d).TitleText.Text = (string)e.NewValue));

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(nameof(Description), typeof(string), typeof(SettingsCard),
            new PropertyMetadata(string.Empty, (d, e) => ((SettingsCard)d).DescriptionText.Text = (string)e.NewValue));

    public static readonly DependencyProperty IconGlyphProperty =
        DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(SettingsCard),
            new PropertyMetadata("\uE713", (d, e) => ((SettingsCard)d).IconElement.Glyph = (string)e.NewValue));

    public static readonly DependencyProperty ActionContentProperty =
        DependencyProperty.Register(nameof(ActionContent), typeof(object), typeof(SettingsCard),
            new PropertyMetadata(null, (d, e) => ((SettingsCard)d).ActionPresenter.Content = e.NewValue));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public string IconGlyph
    {
        get => (string)GetValue(IconGlyphProperty);
        set => SetValue(IconGlyphProperty, value);
    }

    public object ActionContent
    {
        get => GetValue(ActionContentProperty);
        set => SetValue(ActionContentProperty, value);
    }

    public SettingsCard()
    {
        this.InitializeComponent();
    }
}
