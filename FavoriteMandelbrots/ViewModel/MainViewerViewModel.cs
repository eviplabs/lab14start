using FavoriteMandelbrots.Model;
using FavoriteMandelbrots.View;
using MandelbrotCommon;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoriteMandelbrots.ViewModel
{
    public class MainViewerViewModel : ObservableObject
    {
        #region Currently shown area
        public ImageSource CurrentImageSource { get; set; }

        public Area CurrentArea { get; set; }

        private readonly Area StartingArea = new Area(left: -1.6F, right: 0.6F, top: -1.2F, bottom: 1.2F);

        // EVIP: we need this as we cannot send PropertyChanged event for a property inside CurrentArea.
        public string AreaAsText => CurrentArea.AsString;
        #endregion

        #region Favorites management
        public ObservableCollection<AreaViewModel> Favorites { get; set; }
            = new ObservableCollection<AreaViewModel>();

        public AreaViewModel CurrentlySelectedFavorite { get; set; }

        // EVIP: Commands to bind to.
        public Command AddToFavoritesCommand { get; set; }
        public Command UpdateFavoriteCommand { get; set; }
        public Command RemoveFavoriteCommand { get; set; }
        public Command SaveFavoritesCommand { get; set; }
        public Command AddFavoritesFromFileCommand { get; set; }
        #endregion

        public MainViewerViewModel()
        {
            // Add starting area to the list of favorites
            Favorites = new ObservableCollection<AreaViewModel>();
            var startingAreaVm = new AreaViewModel(StartingArea.Clone(), this);
            Favorites.Add(startingAreaVm);
            startingAreaVm.NotifyAllPropertiesChanged();

            // Create remaining parts of view model
            var ops = new FavoriteOperations(this);
            AddToFavoritesCommand = new Command( () => ops.AddCurrentArea());

            // EVIP: Cloning to avoid manipulation of
            //  the same instance from many points.
            CurrentArea = StartingArea.Clone();
            // Note: After cloning, we need to subscribe to the event of the new instance.
            CurrentArea.PropertyChanged += CurrentArea_PropertyChanged;
        }

        #region Zooming functionality
        internal void ZoomIn(Point clickPositionInImageCoordinates)
        {
            var clickPositionInComplexPlane = Image2Mandelbrot(clickPositionInImageCoordinates);
            Zoom(clickPositionInComplexPlane, 0.5);
        }

        internal void ZoomOut(Point clickPositionInImageCoordinates)
        {
            var clickPositionInComplexPlane = Image2Mandelbrot(clickPositionInImageCoordinates);
            Zoom(clickPositionInComplexPlane, 2.0);
        }

        void Zoom(Point newCenter, double zoomFactor)
        {
            var currentWidthInComplexPlane = CurrentArea.Right - CurrentArea.Left;
            var currentHeightInComplexPlane = CurrentArea.Bottom - CurrentArea.Top;
            CurrentArea.Set(
                left: newCenter.X - currentWidthInComplexPlane / 2.0 * zoomFactor,
                right: newCenter.X + currentWidthInComplexPlane / 2.0 * zoomFactor,
                top: newCenter.Y - currentHeightInComplexPlane / 2.0 * zoomFactor,
                bottom: newCenter.Y + currentHeightInComplexPlane / 2.0 * zoomFactor);
        }

        private Point Image2Mandelbrot(Point clickPositionInImageCoordinates)
        {
            double proportionX = clickPositionInImageCoordinates.X / FullImageRenderWidth;
            double proportionY = clickPositionInImageCoordinates.Y / FullImageRenderHeight;

            double x = CurrentArea.Left + proportionX * (CurrentArea.Right - CurrentArea.Left);
            double y = CurrentArea.Top + proportionY * (CurrentArea.Bottom - CurrentArea.Top);
            return new Point(x, y);
        }

        #endregion

        private async void CurrentArea_PropertyChanged(object sender, PropertyChangedEventArgs e)   // Note: async void cannot be awaited!
        {
            Notify(nameof(AreaAsText));
            await RenderCurrentImage();
        }

        #region Rendering CurrentImage
        // EVIP: Constant attributes to avoid hardwired and redundant
        //  (and hard-to-read) numeric constants throughout the code.
        public const int FullImageRenderWidth = 800;
        public const int FullImageRenderHeight = 800;

        internal async Task RenderCurrentImage()
        {
            int renderWidth = FullImageRenderWidth;
            int renderHeight = FullImageRenderHeight;

            SKBitmap skbitmap = null;
            // Run rendering in a background thread
            await Task.Run(() => skbitmap = CurrentArea.Render( renderWidth, renderHeight ));
            CurrentImageSource = ImageRendererBase.SKBitmap2ImageSource(skbitmap);
            Notify(nameof(CurrentImageSource));
        }
        #endregion
    }
}
