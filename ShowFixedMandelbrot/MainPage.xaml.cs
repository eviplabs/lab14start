using MandelbrotCommon;
using SkiaSharp;

namespace ShowFixedMandelbrot
{
    // EVIP: application using the MandelbrotCommon class library. Almost everything is in the library as reuseable components.
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        // EVIP: simple, good old event handler. No command pattern.
        private void Button_Click(object sender, EventArgs e)
        {
            // EVIP: ImageRendererBase allows alternate
            //  implementations for testing purposes
            ImageRendererBase renderer = new Mandelbrot();
            //ImageRendererBase renderer = new TestGridImageRenderer();

            SKBitmap skbitmap = renderer.RenderDefault();
            myImage.Source = ImageRendererBase.SKBitmap2ImageSource(skbitmap);
        }
    }
}
