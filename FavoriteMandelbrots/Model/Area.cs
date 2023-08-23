using MandelbrotCommon;
using SkiaSharp;
using System.Numerics;


namespace FavoriteMandelbrots.Model
{
    public class Area : ObservableObject
    {
        #region Properties
        private double top;
        public double Top => top;

        private double bottom;
        public double Bottom => bottom;

        private double left;
        public double Left => left;

        private double right;
        public double Right => right;
        #endregion

        // EVIP: Getter-only property and string interpolation
        public string AsString => $"{left:G4}+i{top:G4} - {right:G4}+i{bottom:G4}";

        public Area()
        {
        }

        public Area(double left, double top, double right, double bottom)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }

        public SKBitmap Render(int width, int height)
        {
            ImageRendererBase m = new Mandelbrot();
            return m.Render(new Complex(left, top), new Complex(right, bottom),
                width, height);
        }

        // Helper method used when replacing the model to notify the UI
        //  about all changes.
        // EViP: the properties cannot be set externally separately, only via ctor or CopyTo.
        //  We do not want 4 PropertyChanged events to fire and trigger 4 renderings...
        internal void NotifyAllPropertiesChanged()
        {
            // "" Property name means all properties.
            Notify();
        }

        public void Set(double left, double top, double right, double bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            NotifyAllPropertiesChanged();
        }

        public Area Clone()
        {
            // EViP: we can access private setters as we are in the same class.
            return new Area(left: this.left, top: this.top, right: this.right, bottom: this.bottom);
        }

        internal void CopyTo(Area target)
        {
            target.Set(left,top,right,bottom);
        }

    }
}
