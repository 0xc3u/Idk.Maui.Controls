using SkiaSharp.Views.Maui.Controls.Hosting;

namespace Indiko.Maui.Controls;

public static class BuilderExtension
{
    public static void UseIndikoControls(this MauiAppBuilder builder)
    {
        builder.UseSkiaSharp();

    }
}
