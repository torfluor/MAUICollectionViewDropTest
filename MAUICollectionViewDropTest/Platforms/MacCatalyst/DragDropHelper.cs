using Foundation;
using UIKit;

namespace MAUICollectionViewDropTest.Platforms.MacCatalyst
{

    public static class DragDropHelper
    {
        /* Blog post describing drag and drop https://vladislavantonyuk.github.io/articles/Drag-and-Drop-any-content-to-a-.NET-MAUI-application/ */
        /* Github repo: https://github.com/VladislavAntonyuk/MauiSamples/tree/main/MauiPaint */

        /*
        public static void RegisterDrag(UIView view, Func<CancellationToken, Task<Stream>> content)
        {
            var dragInteraction = new UIDragInteraction(new DragInteractionDelegate()
            {
                Content = content
            });
            view.AddInteraction(dragInteraction);
        }
        
        public static void UnRegisterDrag(UIView view)
        {
            var dragInteractions = view.Interactions.OfType<UIDragInteraction>();
            foreach (var interaction in dragInteractions)
            {
                view.RemoveInteraction(interaction);
            }
        }
        */

        public static void RegisterDrop(UIView view, Func<List<string>, Task>? content)
        {
            var dropInteraction = new UIDropInteraction(new DropInteractionDelegate()
            {
                Content = content
            });
            view.AddInteraction(dropInteraction);
        }

        public static void UnRegisterDrop(UIView view)
        {
            var dropInteractions = view.Interactions.OfType<UIDropInteraction>();
            foreach (var interaction in dropInteractions)
            {
                view.RemoveInteraction(interaction);
            }
        }
    }

    /*
    class DragInteractionDelegate : UIDragInteractionDelegate
    {
        public Func<CancellationToken, Task<string>>? Content { get; init; }

        public override UIDragItem[] GetItemsForBeginningSession(UIDragInteraction interaction, IUIDragSession session)
        {
            if (Content is null)
            {
                return Array.Empty<UIDragItem>();
            }

            var streamContent = Content.Invoke(CancellationToken.None).GetAwaiter().GetResult();
            var itemProvider = new NSItemProvider(NSData.FromStream(streamContent), UniformTypeIdentifiers.UTTypes.Png.Identifier);
            var dragItem = new UIDragItem(itemProvider);
            return new[] { dragItem };
        }
    }
    */

    class DropInteractionDelegate : UIDropInteractionDelegate
    {
        public Func<List<string>, Task>? Content { get; init; }

        public override bool CanHandleSession(UIDropInteraction interaction, IUIDropSession session)
        {
            return true;
        }

        public override UIDropProposal SessionDidUpdate(UIDropInteraction interaction, IUIDropSession session)
        {
            return new UIDropProposal(UIDropOperation.Copy);
        }

        public override void PerformDrop(UIDropInteraction interaction, IUIDropSession session)
        {
            if (Content is null)
            {
                return;
            }

            var filePaths = new List<string>();

            foreach (var item in session.Items)
            {
                item.ItemProvider.LoadItem(UniformTypeIdentifiers.UTTypes.Pdf.Identifier, null, (data, error) =>
                {
                    if (data is NSUrl nsData && !string.IsNullOrEmpty(nsData.Path))
                    {
                        filePaths.Add(nsData.Path);
                    }
                });
            }

            Content.Invoke(filePaths);
        }
    }
}
