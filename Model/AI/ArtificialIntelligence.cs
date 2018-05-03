using Mathematics;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.AI
{
    public abstract class ArtificialIntelligence : IArtificialIntelligence
    {
        protected readonly World world;

        protected readonly Boid self;

        protected ArtificialIntelligence(World world, Boid self)
        {
            this.world = world;
            this.self = self;
        }

        public abstract Vector2D ComputeAcceleration();
    }
}
