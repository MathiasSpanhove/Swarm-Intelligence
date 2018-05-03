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
    public class HunterSpecies : BoidSpecies
    {
        public static RangedDoubleParameter HunterAttractionConstant = new RangedDoubleParameter("Hunter Attraction Constant", defaultValue: -20000, minimum: -100000, maximum: 100000);

        public static RangedDoubleParameter HunterAttractionExponent = new RangedDoubleParameter("Hunter Attraction Exponent", defaultValue: 2, minimum: 1, maximum: 10);

        public static RangedDoubleParameter ZombieRepulsionConstant = new RangedDoubleParameter("Zombie Repulsion Constant", defaultValue: 20000, minimum: 0, maximum: 100000);

        public static RangedDoubleParameter ZombieRepulsionExponent = new RangedDoubleParameter("Zombie Repulsion Exponent", defaultValue: 2, minimum: 1, maximum: 10);

        public static RangedDoubleParameter PreyAttractionConstant = new RangedDoubleParameter("Prey Attraction Constant", defaultValue: 20000, minimum: 0, maximum: 100000);

        public static RangedDoubleParameter PreyAttractionExponent = new RangedDoubleParameter("Prey Attraction Exponent", defaultValue: 1.5, minimum: 1, maximum: 10);

        public static Parameter<string> EnemySpecies = new Parameter<string>("Repulsive Species", "hunter");

        public static Parameter<string> FriendSpecies = new Parameter<string>("Attractive Species", "prey");

        public HunterSpecies(World world)
            : base(world, "hunter")
        {
            Bindings
                .Initialize(HunterAttractionConstant)
                .Initialize(HunterAttractionExponent)
                .Initialize(PreyAttractionConstant)
                .Initialize(PreyAttractionExponent)
                .Initialize(ZombieRepulsionConstant)
                .Initialize(ZombieRepulsionExponent)
                .Initialize(EnemySpecies)
                .Initialize(FriendSpecies);
        }

        internal override IArtificialIntelligence CreateAI(Boid boid)
        {
            return new ArtificialIntelligence(World, boid);
        }

        private class ArtificialIntelligence : Model.AI.ArtificialIntelligence
        {
            private readonly IForce enemyForce;

            private readonly IForce friendForce;

            public ArtificialIntelligence(World world, Boid self)
                : base(world, self)
            {
                enemyForce = new BoidAttractionForce(HunterAttractionConstant, HunterAttractionExponent, EnemySpecies);
                friendForce = new BoidAttractionForce(PreyAttractionConstant, PreyAttractionExponent, FriendSpecies);
            }

            public override Vector2D ComputeAcceleration()
            {
                var total = new Vector2D(0, 0);

                total += enemyForce.Compute(this.self.Bindings, this.world, this.self);
                total += friendForce.Compute(this.self.Bindings, this.world, this.self);

                return total;
            }
        }
    }
}
