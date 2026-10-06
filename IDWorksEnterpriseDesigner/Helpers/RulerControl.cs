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

            // Ruler နောက်ခံ
            dc.DrawRectangle(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0F0F0")), null, new Rect(0, 0, ActualWidth, ActualHeight));
            
            Pen linePen = new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8E8E8E")), 1);
            Pen borderPen = new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C0C0C0")), 1);

            Typeface typeface = new Typeface("Segoe UI");
            double dpiScale = VisualTreeHelper.GetDpi(this).PixelsPerDip;

            if (Orientation == Orientation.Horizontal)
            {
                // အောက်ခြေ နယ်နိမိတ်မျဉ်း
                dc.DrawLine(borderPen, new Point(0, ActualHeight - 0.5), new Point(ActualWidth, ActualHeight - 0.5));

                // Horizontal Ruler Ticks & Numbers
                for (double x = 0; x < ActualWidth; x += 5)
                {
                    double tickHeight;
                    if (x % 50 == 0)
                        tickHeight = 12; // မျဉ်းရှည်
                    else if (x % 10 == 0)
                        tickHeight = 7;  // အလယ်အလတ်မျဉ်း
                    else
                        tickHeight = 4;  // မျဉ်းတို

                    dc.DrawLine(linePen, new Point(x + 0.5, ActualHeight), new Point(x + 0.5, ActualHeight - tickHeight));

                    if (x % 50 == 0 && x > 0)
                    {
                        FormattedText text = new FormattedText(
                            x.ToString(CultureInfo.InvariantCulture),
                            CultureInfo.CurrentCulture,
                            FlowDirection.LeftToRight,
                            typeface, 8.5,
                            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555")),
                            dpiScale);

                        dc.DrawText(text, new Point(x + 2, 2));
                    }
                }
            }
            else
            {
                // ညာဘက် နယ်နိမိတ်မျဉ်း
                dc.DrawLine(borderPen, new Point(ActualWidth - 0.5, 0), new Point(ActualWidth - 0.5, ActualHeight));

                // Vertical Ruler Ticks & Numbers
                for (double y = 0; y < ActualHeight; y += 5)
                {
                    double tickWidth;
                    if (y % 50 == 0)
                        tickWidth = 12;
                    else if (y % 10 == 0)
                        tickWidth = 7;
                    else
                        tickWidth = 4;

                    dc.DrawLine(linePen, new Point(ActualWidth, y + 0.5), new Point(ActualWidth - tickWidth, y + 0.5));

                    if (y % 50 == 0 && y > 0)
                    {
                        FormattedText text = new FormattedText(
                            y.ToString(CultureInfo.InvariantCulture),
                            CultureInfo.CurrentCulture,
                            FlowDirection.LeftToRight,
                            typeface, 8.5,
                            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555555")),
                            dpiScale);

                        // စာသားကို ဒေါင်လိုက် လှည့်ပြခြင်း
                        dc.PushTransform(new TranslateTransform(2, y + 12));
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
