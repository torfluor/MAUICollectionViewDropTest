#nullable enable

using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS
using MAUICollectionViewDropTest.Platforms.Windows;
#elif MACCATALYST
using MAUICollectionViewDropTest.Platforms.MacCatalyst;
#endif

namespace MAUICollectionViewDropTest.Extensions
{
    public static class DragDropExtensions
    {
        /* Blog post describing drag and drop https://vladislavantonyuk.github.io/articles/Drag-and-Drop-any-content-to-a-.NET-MAUI-application/ */
        /* Github repo: https://github.com/VladislavAntonyuk/MauiSamples/tree/main/MauiPaint */

        /*
        public static void RegisterDrag(this IElement element, IMauiContext? mauiContext, Func<CancellationToken, Task<Stream>> content)
        {
            ArgumentNullException.ThrowIfNull(mauiContext);
            var view = element.ToPlatform(mauiContext);
            DragDropHelper.RegisterDrag(view, content);
        }

        public static void UnRegisterDrag(this IElement element, IMauiContext? mauiContext)
        {
            ArgumentNullException.ThrowIfNull(mauiContext);
            var view = element.ToPlatform(mauiContext);
            DragDropHelper.UnRegisterDrag(view);
        }
        */

        public static void RegisterDrop(this IElement element, IMauiContext? mauiContext, Func<List<string>, Task>? content)
        {
            ArgumentNullException.ThrowIfNull(mauiContext);
            var view = element.ToPlatform(mauiContext);
            DragDropHelper.RegisterDrop(view, content);
        }

        public static void UnRegisterDrop(this IElement element, IMauiContext? mauiContext)
        {
            ArgumentNullException.ThrowIfNull(mauiContext);
            var view = element.ToPlatform(mauiContext);
            DragDropHelper.UnRegisterDrop(view);
        }
    }
}
