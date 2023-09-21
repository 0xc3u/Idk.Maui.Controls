using SkiaSharp.Views.Maui.Controls.Hosting;

namespace Idk.Maui.Controls;

public static class BuilderExtension
{
    public static void UseIndikoControls(this MauiAppBuilder builder)
    {
        builder.UseSkiaSharp();

    }
}
