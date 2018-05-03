using Cells;
using Model.AI;
using Bindings;
using Model.Species;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mathematics;

namespace Model
{
    public class Boid
    {
        public Boid(World world, Vector2D position, BoidSpecies species)
        {
            this.World = world;
            this.Position = Cell.Create(position);
            this.Velocity = Cell.Create(new Vector2D(0, 0));
            this.Species = species;
            this.AI = species.CreateAI(this);
            this.Bindings = new ParameterBindings("Boid", species.Bindings);
        }

        public ParameterBindings Bindings { get; }

        public World World { get; }

        public Cell<Vector2D> Position { get; }

        public Cell<Vector2D> Velocity { get; }

        public BoidSpecies Species { get; }

        public IArtificialIntelligence AI { get; }

        public void Update(double dt)
        {
            var maximumAcceleration = Bindings.Read(BoidSpecies.MaximumAcceleration).Value;
            var maximumSpeed = Bindings.Read(BoidSpecies.MaximumSpeed).Value;

            var acceleration = this.AI.ComputeAcceleration().ClampNorm(maximumAcceleration);
            this.Velocity.Value = (this.Velocity.Value + acceleration * dt).ClampNorm(maximumSpeed);
            this.Position.Value += Velocity.Value * dt;

            BounceOffWalls();
        }

        private void BounceOffWalls()
        {
            var boid = this;
            var elasticity = Bindings.Read(BoidSpecies.Elasticity).Value;
            var worldWidth = Bindings.Read(World.Width).Value;
            var worldHeight = Bindings.Read(World.Height).Value;

            var oldPosition = boid.Position.Value;
            var oldVelocity = boid.Velocity.Value;
            double px = oldPosition.X, py = oldPosition.Y, vx = oldVelocity.X, vy = oldVelocity.Y;

            if (px < 0)
            {
                px = -px;
                vx = -vx * elasticity;
            }
            else if (px > worldWidth)
            {
                var transgression = px - worldWidth;
                px -= 2 * transgression;
                vx = -vx * elasticity;
            }

            if (py < 0)
            {
                py = -py;
                vy = -vy * elasticity;
            }
            else if (py > worldHeight)
            {
                var transgression = py - worldHeight;
                py -= 2 * transgression;
                vy = -vy * elasticity;
            }

            boid.Position.Value = new Vector2D(px, py);
            boid.Velocity.Value = new Vector2D(vx, vy);
        }
    }
}
