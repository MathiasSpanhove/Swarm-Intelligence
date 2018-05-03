using Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bindings
{
    public class RootParameterBindings : IParameterBindings
    {
        public IParameterBindings Parent
        {
            get
            {
                return null;
            }
        }

        public Cell<T> Read<T>(Parameter<T> parameter)
        {
            throw new InvalidOperationException("Unknown parameter");
        }

        public IEnumerable<IParameter> Parameters
        {
            get
            {
                return Enumerable.Empty<IParameter>();
            }
        }

        public string Name => "Root";
    }

    public class ParameterBindings : IParameterBindings
    {
        private readonly Dictionary<IParameter, object> mapping;

        public ParameterBindings(string name)
            : this(name, new RootParameterBindings())
        {
            // NOP
        }

        public ParameterBindings(string name, IParameterBindings parent)
        {
            this.Name = name;
            this.Parent = parent;
            this.mapping = new Dictionary<IParameter, object>();
        }

        public string Name { get; }

        public IParameterBindings Parent { get; }

        public Cell<T> Read<T>(Parameter<T> parameter)
        {
            if (this.mapping.ContainsKey(parameter))
            {
                return (Cell<T>)this.mapping[parameter];
            }
            else
            {
                return this.Parent.Read(parameter);
            }
        }

        public IEnumerable<IParameter> Parameters
        {
            get
            {
                return this.mapping.Keys;
            }
        }

        private IEnumerable<IParameter> RemoveDuplicates(IEnumerable<IParameter> parameters)
        {
            return new HashSet<IParameter>(parameters);
        }

        public void Remove(IParameter parameter)
        {
            this.mapping.Remove(parameter);
        }

        public ParameterBindings Initialize<T>(Parameter<T> parameter)
        {
            this.mapping[parameter] = Cell.Create<T>(parameter.DefaultValue);

            return this;
        }
    }
}
