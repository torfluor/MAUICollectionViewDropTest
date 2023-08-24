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
                dragAndDropTarget.RegisterDrop(Handler?.MauiContext, HandleDrop);
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