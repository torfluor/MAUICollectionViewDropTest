using MAUICollectionViewDropTest.Extensions;
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
                // Registering the drop handler on the Grid works fine. The file path is printed when a PDF file is dropped.
                //myGrid.RegisterDrop(Handler?.MauiContext, HandleDrop);

                // Registering the drop handler on the CollectionView doesn't work on MacCatalyst
                myCollectionView.RegisterDrop(Handler?.MauiContext, HandleDrop);
            };
        }

        private async Task HandleDrop(List<string> filePaths)
        {
            foreach (string path in filePaths)
            {
                Debug.WriteLine($"Dropped file: {path}");
            }
        }
    }
}