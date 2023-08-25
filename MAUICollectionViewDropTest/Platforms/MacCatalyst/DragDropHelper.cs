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
            // Spesial handling for CollectionViews
            if (view.Subviews.Length == 1 && view.Subviews[0] as UICollectionView is var collectionView && collectionView != null)
            {
                collectionView.DropDelegate = new CollectionViewDropDelegate()
                {
                    Content = content
                };
            }
            else
            {
                var dropInteraction = new UIDropInteraction(new DropInteractionDelegate()
                {
                    Content = content
                });
                view.AddInteraction(dropInteraction);
            }
        }

        public static void UnRegisterDrop(UIView view)
        {
            // Spesial handling for CollectionViews
            if (view.Subviews.Length == 1 && view.Subviews[0] as UICollectionView is var collectionView && collectionView != null)
            {
                collectionView.DropDelegate = null;
            }
            else
            {
                var dropInteractions = view.Interactions.OfType<UIDropInteraction>();
                foreach (var interaction in dropInteractions)
                {
                    view.RemoveInteraction(interaction);
                }
            }
        }
    }


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
            var itemsToProcess = session.Items.Length;

            foreach (var item in session.Items)
            {
                item.ItemProvider.LoadItem(UniformTypeIdentifiers.UTTypes.Item.Identifier, null, async (data, error) =>
                {
                    var isLastItem = false;

                    if (data is NSUrl nsData && !string.IsNullOrEmpty(nsData.Path))
                    {
                        lock (filePaths)
                        {
                            filePaths.Add(nsData.Path);
                        }
                    }

                    lock (filePaths)
                    {
                        itemsToProcess--;
                        if (itemsToProcess == 0)
                        {
                            isLastItem = true;
                        }
                    }

                    if (isLastItem)
                    {
                        await Content.Invoke(filePaths);
                    }
                });
            }
        }
    }


    class CollectionViewDropDelegate : UICollectionViewDropDelegate
    {
        public Func<List<string>, Task>? Content { get; init; }


        public override bool CanHandleDropSession(UICollectionView collectionView, IUIDropSession session)
        {
            return true;
        }

        public override void PerformDrop(UICollectionView collectionView, IUICollectionViewDropCoordinator coordinator)
        {
            if (Content is null)
            {
                return;
            }

            var filePaths = new List<string>();
            var itemsToProcess = coordinator.Items.Length;

            foreach (var item in coordinator.Items)
            {
                item.DragItem.ItemProvider.LoadItem(UniformTypeIdentifiers.UTTypes.Item.Identifier, null, async (data, error) =>
                {
                    var isLastItem = false;

                    if (data is NSUrl nsData && !string.IsNullOrEmpty(nsData.Path))
                    {
                        lock (filePaths)
                        {
                            filePaths.Add(nsData.Path);
                        }
                    }

                    lock (filePaths)
                    {
                        itemsToProcess--;
                        if (itemsToProcess == 0)
                        {
                            isLastItem = true;
                        }
                    }

                    if (isLastItem)
                    {
                        await Content.Invoke(filePaths);
                    }

                });
            }
        }
    }
}

