using Model.Species;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace View
{
    class SpeciesToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            RadialGradientBrush myBrush = new RadialGradientBrush();
            myBrush.GradientOrigin = new Point(0.5, 0.75);
            myBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            myBrush.GradientStops.Add(new GradientStop(Colors.Orange, 0.3));

            if (value is HunterSpecies)
                myBrush.GradientStops.Add(new GradientStop(Colors.Red, 1.0));
            else
                myBrush.GradientStops.Add(new GradientStop(Colors.Green, 1.0));

            return myBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
