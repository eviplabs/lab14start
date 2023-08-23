using FavoriteMandelbrots.ViewModel;


namespace FavoriteMandelbrots.View
{
    public sealed partial class MainViewer : ContentView
    {
        public MainViewer()
        {
            this.InitializeComponent();
        }

        private void ImageTappedZoomIn(object sender, TappedEventArgs e)
        {
            viewModel.ZoomIn(e.GetPosition(sender as Element).Value);
        }

        private void ImageTappedZoomOut(object sender, TappedEventArgs e)
        {
            viewModel.ZoomOut(e.GetPosition(sender as Element).Value);
        }

        private async void ContentView_Loaded(object sender, EventArgs e)
        {
            await viewModel.RenderCurrentImage();
        }
    }
}
