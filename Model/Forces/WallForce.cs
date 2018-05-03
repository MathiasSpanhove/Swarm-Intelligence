using Bindings;
using Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Forces
{
    public class WallForce : IForce
    {
        private readonly Parameter<double> constant;

        private readonly Parameter<double> exponent;

        public WallForce(Parameter<double> constant, Parameter<double> exponent)
        {
            this.constant = constant;
            this.exponent = exponent;
        }

        public Vector2D Compute(IParameterBindings bindings, World world, Boid self)
        {
            var constant = bindings.Read(this.constant).Value;
            var exponent = bindings.Read(this.exponent).Value;
            var worldWidth = self.Bindings.Read(World.Width).Value;
            var worldHeight = self.Bindings.Read(World.Height).Value;
            var total = new Vector2D(0, 0);

            total += WestForce(self.Position.Value, constant, exponent);
            total += EastForce(worldWidth, self.Position.Value, constant, exponent);
            total += NorthForce(self.Position.Value, constant, exponent);
            total += SouthForce(worldHeight, self.Position.Value, constant, exponent);

            return total;
        }

        protected Vector2D WestForce(Vector2D position, double constant, double exponent)
        {
            return Force(position, new Vector2D(0, position.Y), constant, exponent);
        }

        protected Vector2D EastForce(double worldWidth, Vector2D position, double constant, double exponent)
        {
            return Force(position, new Vector2D(worldWidth, position.Y), constant, exponent);
        }

        protected Vector2D NorthForce(Vector2D position, double constant, double exponent)
        {
            return Force(position, new Vector2D(position.X, 0), constant, exponent);
        }

        protected Vector2D SouthForce(double worldHeight, Vector2D position, double constant, double exponent)
        {
            return Force(position, new Vector2D(position.X, worldHeight), constant, exponent);
        }

        protected Vector2D Force(Vector2D boidPosition, Vector2D wallPosition, double constant, double exponent)
        {
            var wallToBoid = boidPosition - wallPosition;
            var distance = wallToBoid.Norm;
            var result = constant * wallToBoid / Math.Pow(distance, exponent);

            return result;
        }
    }
}
