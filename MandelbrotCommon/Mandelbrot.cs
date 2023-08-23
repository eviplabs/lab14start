using SkiaSharp;
using System;
using System.Linq;
using System.Numerics;


namespace MandelbrotCommon
{
    public class Mandelbrot : ImageRendererBase
    {
        public override SKBitmap RenderDefault()
        {
            // This is the location of a nice place in the Mandelbrot set.
            return Render(new Complex(-0.563F, -0.561F), new Complex(-0.55F, -0.548F),
                1000, 1000);
        }

        protected override void Render(SKBitmap image)
        {
            // EVIP: parallel rendering of Mandelbrot set with
            //  Linq (some elements of functional programming)
            image.GetAllPixelLocations()    // SKBitmap ---> IEnumerable<Point>
                .AsParallel()
                .Select(p => ToScaledComplex(p))    // Point ---> (Point, Complex) 
                .Select(c => GetMandelbrotDivergenceIndex(c))   // (Point, Complex) ---> (Point, int)
                .Select(i => Index2HSV(i))  // (Point, int) ---> (Point, Color)
                .SetPixels(image);  // IEnumerable<(Point, Color)>, SKBitmap ---> void
        }

        #region Transformations for Linq expressions
        private (Point, int) GetMandelbrotDivergenceIndex((Point p, Complex c) input)
        {
            int index = 0;
            Complex z = new Complex(0, 0);
            while (z.Magnitude < 100.0 && index < 255)
            {
                z = z * z + input.c;
                index++;
            }
            return (input.p, index);
        }

        // EVIP: Tuple as input and return value
        private (Point, SKColor) Index2HSV((Point p, int index) input)
            => (input.p, SKColor.FromHsv((float)Math.Min(input.index, 360), 100.0f, 100.0f));   // Degrees, percent, percent
        #endregion
    }
}
