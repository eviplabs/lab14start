using FavoriteMandelbrots.Model;
using MandelbrotCommon;
using SkiaSharp;
using System;
using System.Reflection;
using System.Windows.Input;

namespace FavoriteMandelbrots.ViewModel
{
    public class AreaViewModel : ObservableObject
    {
        public AreaViewModel(Area model, MainViewerViewModel mainViewerVM)
        {
            this.mainViewerViewModel = mainViewerVM;
            this.Model = model;
            model.PropertyChanged += Model_PropertyChanged;
            ShowInMainViewerCommand = new Command(()=> ShowInMainViewer());
            UpdateThumbnail();
        }

        public Command ShowInMainViewerCommand { get; }

        private MainViewerViewModel mainViewerViewModel;
        public void ShowInMainViewer()
        {
            this.Model.CopyTo(mainViewerViewModel.CurrentArea);
        }

        public Area Model { get; private set; }

        public string AsString => Model.AsString;

        internal void UpdateModel(Area newFavorite)
        {
            Model.PropertyChanged -= Model_PropertyChanged;
            Model = newFavorite.Clone();
            Model.PropertyChanged += Model_PropertyChanged;
            // EVIP: all properties may have changed,
            //  so we send notification about all of them.
            NotifyAllPropertiesChanged();
        }

        public void NotifyAllPropertiesChanged()
        {
            Model.NotifyAllPropertiesChanged();
        }

        // EVIP: image for data binding
        public ImageSource Thumbnail { get; set; }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // nameof(): renaming the properties cannot cause inconsistencies.
            Notify(e.PropertyName);
            Notify(nameof(AsString));  // This depends on all others...
            if (e.PropertyName == nameof(Model.Top)
                || e.PropertyName == nameof(Model.Bottom)
                || e.PropertyName == nameof(Model.Left)
                || e.PropertyName == nameof(Model.Right))
                UpdateThumbnail();
        }

        const int thumbnailWidth = 50;
        const int thumbnailHeight = 50;
        private void UpdateThumbnail()
        {
            Thumbnail = ImageRendererBase.SKBitmap2ImageSource(
                Model.Render(thumbnailWidth, thumbnailHeight));
            Notify(nameof(Thumbnail));
        }

    }
}
