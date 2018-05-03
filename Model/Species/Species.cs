using Cells;
using Model.AI;
using Bindings;
using Model.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mathematics;

namespace Model.Species
{
    public abstract class BoidSpecies
    {
        public static RangedDoubleParameter MaximumSpeed = new RangedDoubleParameter("Maximum Speed", defaultValue: 1000.0, minimum: 0, maximum: 5000);

        public static RangedDoubleParameter MaximumAcceleration = new RangedDoubleParameter("Maximum Acceleration", defaultValue: 5000.0, minimum: 0, maximum: 50000);

        public static RangedDoubleParameter Elasticity = new RangedDoubleParameter("Elasticity", defaultValue: 0.25, minimum: 0, maximum: 1);

        public static RangedDoubleParameter Sight = new RangedDoubleParameter("Sight", defaultValue: 1500, minimum: 100, maximum: 1500);

        protected BoidSpecies(World world, string name)
        {
            this.World = world;
            this.Name = name;
            this.Bindings = new ParameterBindings("Species", world.Bindings)
                .Initialize(MaximumSpeed)
                .Initialize(MaximumAcceleration)
                .Initialize(Elasticity)
                .Initialize(Sight);
        }

        public World World { get; }

        public string Name { get; }

        public ParameterBindings Bindings { get; }

        internal abstract IArtificialIntelligence CreateAI(Boid boid);

        public Boid CreateBoid(Vector2D initialPosition)
        {
            var boid = new Boid(World, initialPosition, this);
            World.Population.Add(boid);

            return boid;
        }
    }
}
