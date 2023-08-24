#nullable enable

using System.Diagnostics;
using Foundation;
using UIKit;

namespace MAUICollectionViewDropTest.Platforms.MacCatalyst
{
    public static class DragDropHelper
    {
        /* Blog post describing drag and drop https://vladislavantonyuk.github.io/articles/Drag-and-Drop-any-content-to-a-.NET-MAUI-application/ */
        /* Github repo: https://github.com/VladislavAntonyuk/MauiSamples/tree/main/MauiPaint */


        public static void RegisterDrop(UIView view, Func<List<string>, Task>? content)
        {
            Debug.WriteLine("RegisterDrop");
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


    class DropInteractionDelegate : UIDropInteractionDelegate
    {
        public Func<List<string>, Task>? Content { get; init; }

        public override bool CanHandleSession(UIDropInteraction interaction, IUIDropSession session)
        {
            Debug.WriteLine("CanHandleSession");
            return true;
        }

        public override UIDropProposal SessionDidUpdate(UIDropInteraction interaction, IUIDropSession session)
        {
            Debug.WriteLine("SessionDidUpdate");
            return new UIDropProposal(UIDropOperation.Copy);
        }

        public override void PerformDrop(UIDropInteraction interaction, IUIDropSession session)
        {
            Debug.WriteLine("PerformDrop");
            if (Content is null)
            {
                return;
            }

            var filePaths = new List<string>();

            foreach (var item in session.Items)
            {
                item.ItemProvider.LoadItem(UniformTypeIdentifiers.UTTypes.Pdf.Identifier, null, async (data, error) =>
                {
                    if (data is NSUrl nsData && !string.IsNullOrEmpty(nsData.Path))
                    {
                        await Content.Invoke(new List<string> { nsData.Path });
                    }
                });
            }
        }
    }
}
