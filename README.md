# WinUI 3 Tabs Demo

A WinUI 3 application demonstrating the `TabView` control with multiple feature-rich tabs.

## Tabs

| Tab | Features |
|-----|----------|
| + button | Dynamically add closable tabs with rename support |
| default page | with + button click, a default "landing" page will be displayed |
| user profile page| when user profile button is click, a new tab is added directing to the user profile and status page |

## Prerequisites

- [Visual Studio 2022](https://visualstudio.microsoft.com/) with the **Windows App SDK** workload
- Windows 10 version 1903 (build 18362) or later
- [Windows App SDK 1.5](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/)

## Build & Run

```bash
# Clone or copy the project, then:
dotnet restore
dotnet build
dotnet run
```

Or open `WinUI3TabsApp.csproj` in Visual Studio 2022 and press **F5**.

## Project Structure

```
WinUI3TabsApp/
├── App.xaml / App.xaml.cs         # Application bootstrap
├── MainWindow.xaml                # Main window with TabView
├── MainWindow.xaml.cs             # Tab logic, counter, notes, settings
├── Program.cs                     # Entry point
└── WinUI3TabsApp.csproj           # Project file (Windows App SDK 1.5)
```

## Key Concepts

- **`TabView`** — the WinUI 3 control for tabbed navigation
- **`TabViewItem.IsClosable`** — controls whether a tab shows a close button
- **`AddTabButtonClick`** — event fired when the user clicks the + button
- **`TabCloseRequested`** — event fired when the user clicks a tab's × button
- **`ElementTheme`** — used to toggle dark/light mode at runtime
