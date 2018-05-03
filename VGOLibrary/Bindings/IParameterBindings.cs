using Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bindings
{
    public interface IParameterBindings
    {
        Cell<T> Read<T>(Parameter<T> parameter);

        IParameterBindings Parent { get; }

        IEnumerable<IParameter> Parameters { get; }

        string Name { get; }
    }
}
