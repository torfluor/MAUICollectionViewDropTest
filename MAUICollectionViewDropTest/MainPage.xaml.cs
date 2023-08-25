using MAUICollectionViewDropTest.Extensions;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MAUICollectionViewDropTest
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                myCollectionView.RegisterDrop(Handler?.MauiContext, HandleCollectionViewDrop);
            };
        }

        private async Task HandleCollectionViewDrop(List<string> filePaths)
        {
            if (filePaths.Count == 0) { return; }

            Debug.WriteLine("Paths to files dropped into the CollectionView:");
            foreach (string path in filePaths)
            {
                Debug.WriteLine($"{path}");
            }
        }

    }
}