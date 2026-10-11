using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls; // <- ဤ namespace ကို ထည့်သွင်းပေးရန် လိုအပ်ပါသည်
using System.Windows.Media;

namespace IDWorksEnterpriseDesigner.Helpers
{
    public class RulerControl : FrameworkElement
    {
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(RulerControl),
                new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Orientation == Orientation.Horizontal)
            {
                double w = double.IsInfinity(availableSize.Width) ? 300 : availableSize.Width;
                return new Size(w, 20);
            }
            else
            {
                double h = double.IsInfinity(availableSize.Height) ? 300 : availableSize.Height;
                return new Size(20, h);
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            double width = ActualWidth > 0 ? ActualWidth : Width;
            double height = ActualHeight > 0 ? ActualHeight : Height;

            if (double.IsNaN(width) || width <= 0) width = 300;
            if (double.IsNaN(height) || height <= 0) height = 20;

            // Ruler Background Fill
            dc.DrawRectangle(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0F0F0")), null, new Rect(0, 0, width, height));

            Pen linePen = new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555")), 1);
            Pen borderPen = new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0A0A0")), 1);
            linePen.Freeze();
            borderPen.Freeze();

            Typeface typeface = new Typeface("Segoe UI");
            double dpiScale = VisualTreeHelper.GetDpi(this).PixelsPerDip;

            // 1 CM = 37.7952 DIPs (Standard 96 DPI)
            double oneCmPx = 96.0 / 2.54;
            double oneMmPx = oneCmPx / 10.0;

            if (Orientation == Orientation.Horizontal)
            {
                // Bottom Border Line
                dc.DrawLine(borderPen, new Point(0, height - 0.5), new Point(width, height - 0.5));

                for (double x = 0; x <= width; x += oneMmPx)
                {
                    int mmIndex = (int)Math.Round(x / oneMmPx);
                    double tickHeight = (mmIndex % 10 == 0) ? 12 : (mmIndex % 5 == 0 ? 7 : 4);

                    dc.DrawLine(linePen, new Point(x, height), new Point(x, height - tickHeight));

                    if (mmIndex % 10 == 0)
                    {
                        int cmValue = mmIndex / 10;
                        FormattedText text = new FormattedText(
                            cmValue.ToString(CultureInfo.InvariantCulture),
                            CultureInfo.InvariantCulture,
                            FlowDirection.LeftToRight,
                            typeface, 8,
                            Brushes.Black,
                            dpiScale);

                        dc.DrawText(text, new Point(x + 2, 1));
                    }
                }
            }
            else
            {
                // Right Border Line
                dc.DrawLine(borderPen, new Point(width - 0.5, 0), new Point(width - 0.5, height));

                for (double y = 0; y <= height; y += oneMmPx)
                {
                    int mmIndex = (int)Math.Round(y / oneMmPx);
                    double tickWidth = (mmIndex % 10 == 0) ? 12 : (mmIndex % 5 == 0 ? 7 : 4);

                    dc.DrawLine(linePen, new Point(width, y), new Point(width - tickWidth, y));

                    if (mmIndex % 10 == 0)
                    {
                        int cmValue = mmIndex / 10;
                        FormattedText text = new FormattedText(
                            cmValue.ToString(CultureInfo.InvariantCulture),
                            CultureInfo.InvariantCulture,
                            FlowDirection.LeftToRight,
                            typeface, 8,
                            Brushes.Black,
                            dpiScale);

                        dc.PushTransform(new TranslateTransform(2, y + 10));
                        dc.PushTransform(new RotateTransform(-90));
                        dc.DrawText(text, new Point(0, 0));
                        dc.Pop();
                        dc.Pop();
                    }
                }
            }
        }
    }
}
