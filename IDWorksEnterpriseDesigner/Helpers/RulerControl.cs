using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
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
            // Ruler Control Size မပျောက်စေရန် အနည်းဆုံး Height/Width Dynamic ပေးခြင်း
            if (Orientation == Orientation.Horizontal)
            {
                double h = double.IsNaN(Height) || Height == 0 ? 20 : Height;
                double w = double.IsInfinity(availableSize.Width) ? 200 : availableSize.Width;
                return new Size(w, h);
            }
            else
            {
                double w = double.IsNaN(Width) || Width == 0 ? 20 : Width;
                double h = double.IsInfinity(availableSize.Height) ? 200 : availableSize.Height;
                return new Size(w, h);
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            // Background Fill
            dc.DrawRectangle(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0F0F0")), null, new Rect(0, 0, ActualWidth, ActualHeight));

            Pen linePen = new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444444")), 1);
            Pen borderPen = new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B0B0B0")), 1);

            Typeface typeface = new Typeface("Segoe UI");
            double dpiScale = VisualTreeHelper.GetDpi(this).PixelsPerDip;

            // 1 cm in WPF Device Independent Pixels (96 DPI / 2.54 cm = 37.7952 px)
            double oneCmInPx = 96.0 / 2.54;
            double oneMmInPx = oneCmInPx / 10.0;

            if (Orientation == Orientation.Horizontal)
            {
                // Bottom Line Border
                dc.DrawLine(borderPen, new Point(0, ActualHeight - 0.5), new Point(ActualWidth, ActualHeight - 0.5));

                for (double x = 0; x <= ActualWidth; x += oneMmInPx)
                {
                    int mmIndex = (int)Math.Round(x / oneMmInPx);
                    double tickHeight;

                    if (mmIndex % 10 == 0)
                        tickHeight = 12; // 1 cm
                    else if (mmIndex % 5 == 0)
                        tickHeight = 7;  // 0.5 cm
                    else
                        tickHeight = 4;  // 1 mm

                    dc.DrawLine(linePen, new Point(x + 0.5, ActualHeight), new Point(x + 0.5, ActualHeight - tickHeight));

                    if (mmIndex % 10 == 0)
                    {
                        int cmValue = mmIndex / 10;
                        FormattedText text = new FormattedText(
                            cmValue.ToString(CultureInfo.InvariantCulture),
                            CultureInfo.CurrentCulture,
                            FlowDirection.LeftToRight,
                            typeface, 8,
                            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#222222")),
                            dpiScale);

                        dc.DrawText(text, new Point(x + 2, 2));
                    }
                }
            }
            else
            {
                // Right Line Border
                dc.DrawLine(borderPen, new Point(ActualWidth - 0.5, 0), new Point(ActualWidth - 0.5, ActualHeight));

                for (double y = 0; y <= ActualHeight; y += oneMmInPx)
                {
                    int mmIndex = (int)Math.Round(y / oneMmInPx);
                    double tickWidth;

                    if (mmIndex % 10 == 0)
                        tickWidth = 12; // 1 cm
                    else if (mmIndex % 5 == 0)
                        tickWidth = 7;  // 0.5 cm
                    else
                        tickWidth = 4;  // 1 mm

                    dc.DrawLine(linePen, new Point(ActualWidth, y + 0.5), new Point(ActualWidth - tickWidth, y + 0.5));

                    if (mmIndex % 10 == 0)
                    {
                        int cmValue = mmIndex / 10;
                        FormattedText text = new FormattedText(
                            cmValue.ToString(CultureInfo.InvariantCulture),
                            CultureInfo.CurrentCulture,
                            FlowDirection.LeftToRight,
                            typeface, 8,
                            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#222222")),
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
