using Bindings;
using Mathematics;
using Model.Species;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Forces
{
    public class BoidAttractionForce : IForce
    {
        private readonly Parameter<double> constant;

        private readonly Parameter<double> exponent;

        private readonly Parameter<string> species;

        public BoidAttractionForce(Parameter<double> constant, Parameter<double> exponent, Parameter<string> species)
        {
            this.constant = constant;
            this.exponent = exponent;
            this.species = species;
        }

        public Vector2D Compute(IParameterBindings bindings, World world, Boid self)
        {
            var sight = bindings.Read(BoidSpecies.Sight).Value;
            var constant = bindings.Read(this.constant).Value;
            var exponent = bindings.Read(this.exponent).Value;
            var species = bindings.Read(this.species).Value;
            var total = new Vector2D(0, 0);

            foreach (var boid in world.Population)
            {
                if (boid != self && boid.Species.Name == species)
                {
                    var selfToBoid = boid.Position.Value - self.Position.Value;
                    var distance = selfToBoid.Norm;
                    if (distance < sight)
                    {
                        var result = constant * selfToBoid / Math.Pow(distance, exponent);
                        total += result;
                    }
                }
            }

            return total;
        }
    }
}
