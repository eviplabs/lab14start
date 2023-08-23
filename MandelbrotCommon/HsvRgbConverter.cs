
using SkiaSharp;

namespace MandelbrotCommon
{
    class HsvRgbConverter
    {
        public static SKColor Hue2RGB(int hue)
        {
            return SKColor.FromHsv(hue, 255, 128);
        }
    }
}
