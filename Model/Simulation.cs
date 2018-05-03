using Model.Species;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Simulation
    {
        private readonly TimeQuantizer quantizer;

        private readonly List<BoidSpecies> species;

        public Simulation()
        {
            this.World = new World();
            this.quantizer = new TimeQuantizer(0.005, World.Update);
            this.species = CreateSpecies().ToList();
        }

        public World World { get; }

        public void Update(double dt)
        {
            this.quantizer.Update(dt);
        }

        public IList<BoidSpecies> Species => species;

        private IEnumerable<BoidSpecies> CreateSpecies()
        {
            yield return new HunterSpecies(World);
            yield return new PreySpecies(World);
        }
    }
}
