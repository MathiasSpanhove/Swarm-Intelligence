using Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bindings
{
    public class Parameter<T> : IParameter
    {
        public Parameter(string id, T defaultValue)
        {
            this.Id = id;
            this.DefaultValue = defaultValue;
        }

        object IParameter.DefaultValue => this.DefaultValue;

        public T DefaultValue { get; }

        public string Id { get; }

        public override string ToString()
        {
            return Id;
        }
    }

    
}
