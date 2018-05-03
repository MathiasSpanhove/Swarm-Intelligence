using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bindings
{
    public class RangedDoubleParameter : Parameter<double>
    {
        public RangedDoubleParameter(string id, double defaultValue, double minimum, double maximum)
            : base(id, defaultValue)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        public double Minimum { get; }

        public double Maximum { get; }
    }
}
