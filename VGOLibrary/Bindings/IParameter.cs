using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bindings
{
    public interface IParameter
    {
        string Id { get; }

        object DefaultValue { get; }
    }
}
