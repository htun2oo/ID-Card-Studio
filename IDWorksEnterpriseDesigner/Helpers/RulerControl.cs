using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IDWorksEnterpriseDesigner.Helpers
{
    public class RulerControl : Control
    {
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(RulerControl),
                new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsRender));

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        static RulerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RulerControl), new FrameworkPropertyMetadata(typeof(RulerControl)));
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            double width = ActualWidth > 0 ? ActualWidth : Width;
            double height = ActualHeight > 0 ? ActualHeight : Height;

            if (double.IsNaN(width) || width <= 0) width = 500;
            if (double.IsNaN(height) || height <= 0) height = 20;

            // Background Fill (Light Gray Background for Ruler)
            dc.DrawRectangle(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E2E2")), null, new Rect(0, 0, width, height));

            Pen linePen = new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333")), 1);
            Pen borderPen = new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0A0A0")), 1);
            linePen.Freeze();
            borderPen.Freeze();

            Typeface typeface = new Typeface("Segoe UI");
            double dpiScale = VisualTreeHelper.GetDpi(this).PixelsPerDip;

            // 1 CM = 37.7952 DIPs (at 96 DPI)
            double oneCmPx = 96.0 / 2.54;
            double oneMmPx = oneCmPx / 10.0;

            if (Orientation == Orientation.Horizontal)
            {
                // Bottom Border Line
                dc.DrawLine(borderPen, new Point(0, height - 1), new Point(width, height - 1));

                for (double x = 0; x <= width; x += oneMmPx)
                {
                    int mmIndex = (int)Math.Round(x / oneMmPx);
                    double tickHeight = (mmIndex % 10 == 0) ? 12 : (mmIndex % 5 == 0 ? 7 : 4);

                    dc.DrawLine(linePen, new Point(x, height - 1), new Point(x, height - 1 - tickHeight));

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
                dc.DrawLine(borderPen, new Point(width - 1, 0), new Point(width - 1, height));

                for (double y = 0; y <= height; y += oneMmPx)
                {
                    int mmIndex = (int)Math.Round(y / oneMmPx);
                    double tickWidth = (mmIndex % 10 == 0) ? 12 : (mmIndex % 5 == 0 ? 7 : 4);

                    dc.DrawLine(linePen, new Point(width - 1, y), new Point(width - 1 - tickWidth, y));

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
