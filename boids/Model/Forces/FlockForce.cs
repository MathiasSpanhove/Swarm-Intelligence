using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bindings;
using Mathematics;

namespace Model.Forces
{
    class FlockForce : IForce
    {
        private readonly Parameter<double> sight;

        private readonly Parameter<double> space;

        private readonly Parameter<double> space_exp;

        private readonly Parameter<double> flock_exp;

        private readonly Parameter<double> align_exp;

        private readonly Parameter<string> species;

        public FlockForce(Parameter<double> space, Parameter<double> sight, Parameter<double> space_exp, Parameter<double> flock_exp,
            Parameter<double> align_exp, Parameter<string> species)
        {
            this.space = space;
            this.space_exp = space_exp;
            this.sight = sight;
            this.flock_exp = flock_exp;
            this.align_exp = align_exp;
            this.species = species;
        }

        public Vector2D Compute(IParameterBindings bindings, World world, Boid self)
        {
            var space = bindings.Read(this.space).Value;
            var space_exp = bindings.Read(this.space_exp).Value;
            var sight = bindings.Read(this.sight).Value;
            var species = bindings.Read(this.species).Value;
            var flock_exp = bindings.Read(this.flock_exp).Value;
            var align_exp = bindings.Read(this.align_exp).Value;
            var total = new Vector2D(0, 0);

            foreach (var boid in world.Population)
            {
                if (boid != self && boid.Species.Name == species)
                {
                    var selfToBoid = boid.Position.Value - self.Position.Value;
                    var distance = selfToBoid.Norm;
                    if (distance < space)
                    {
                        // Create space.
                        total += new Vector2D(self.Position.Value.X - boid.Position.Value.X,
                            self.Position.Value.Y - boid.Position.Value.Y) * space_exp;
                    }
                    else if (distance < sight)
                    {
                        // Flock together.
                        total += new Vector2D(boid.Position.Value.X - self.Position.Value.X, 
                            boid.Position.Value.Y - self.Position.Value.Y) * flock_exp;
                    }
                    if (distance < sight)
                    {
                        // Align movement.
                        total += total * align_exp;
                    }
                }
            }

            return total;
        }
    }
}
