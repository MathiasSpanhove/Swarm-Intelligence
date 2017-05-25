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
    public class PreySpecies : BoidSpecies
    {
        public static RangedDoubleParameter HunterAttractionConstant = new RangedDoubleParameter("Hunter Attraction Constant", defaultValue: -20000, minimum: -100000, maximum: 100000);

        public static RangedDoubleParameter HunterAttractionExponent = new RangedDoubleParameter("Hunter Attraction Exponent", defaultValue: 1.5, minimum: 1, maximum: 10);

        public static RangedDoubleParameter WallRepulsionConstant = new RangedDoubleParameter("Wall Repulsion Constant", defaultValue: 50000, minimum: 0, maximum: 100000);

        public static RangedDoubleParameter WallRepulsionExponent = new RangedDoubleParameter("Wall Repulsion Exponent", defaultValue: 1.1, minimum: 1, maximum: 10);

        public static Parameter<string> EnemySpecies = new Parameter<string>("Repulsive Species", "hunter");

        // created

        public static RangedDoubleParameter FlockExponent = new RangedDoubleParameter("Flock Exponent", defaultValue: 0.03, minimum: 0, maximum: 0.1);

        public static RangedDoubleParameter FlockSightDistance = new RangedDoubleParameter("Flock Sight Distance", defaultValue: 500, minimum: 1, maximum: 1000);

        public static RangedDoubleParameter FlockSpaceDistance = new RangedDoubleParameter("Flock Space Distance", defaultValue: 120, minimum: 1, maximum: 1000);

        public static RangedDoubleParameter FlockSpaceExponent = new RangedDoubleParameter("Flock Space Exponent", defaultValue: 0.2, minimum: 0, maximum: 1);

        public static RangedDoubleParameter FlockAlignExponent = new RangedDoubleParameter("Flock Align Exponent", defaultValue: 0.2, minimum: 0, maximum: 1);

        public static Parameter<string> FlockSpecies = new Parameter<string>("Flock Species", "prey");

        public PreySpecies(World world)
            : base(world, "prey")
        {
            Bindings
                .Initialize(HunterAttractionConstant)
                .Initialize(HunterAttractionExponent)
                .Initialize(WallRepulsionConstant)
                .Initialize(WallRepulsionExponent)
                .Initialize(EnemySpecies)
                .Initialize(FlockExponent)
                .Initialize(FlockSightDistance)
                .Initialize(FlockSpaceDistance)
                .Initialize(FlockSpaceExponent)
                .Initialize(FlockAlignExponent)
                .Initialize(FlockSpecies);
        }

        internal override IArtificialIntelligence CreateAI(Boid boid)
        {
            return new ArtificialIntelligence(World, boid);
        }

        private class ArtificialIntelligence : Model.AI.ArtificialIntelligence
        {
            private readonly IForce enemyForce;

            private readonly IForce wallForce;

            private readonly IForce flockForce;

            public ArtificialIntelligence(World world, Boid self)
                : base(world, self)
            {
                enemyForce = new BoidAttractionForce(HunterAttractionConstant, HunterAttractionExponent, EnemySpecies);
                wallForce = new WallForce(WallRepulsionConstant, WallRepulsionExponent);
                flockForce = new FlockForce(FlockSpaceDistance, FlockSightDistance, FlockSpaceExponent, FlockExponent, FlockAlignExponent, FlockSpecies);
            }

            public override Vector2D ComputeAcceleration()
            {
                var total = new Vector2D(0, 0);

                total += flockForce.Compute(this.self.Bindings, this.world, this.self);
                total += enemyForce.Compute(this.self.Bindings, this.world, this.self);
                total += wallForce.Compute(this.self.Bindings, this.world, this.self);
                

                return total;
            }
        }
    }
}
