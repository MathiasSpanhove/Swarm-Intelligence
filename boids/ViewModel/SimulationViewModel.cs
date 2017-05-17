using Bindings;
using Cells;
using Mathematics;
using Model;
using Model.AI;
using Model.Species;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class SimulationViewModel : INotifyPropertyChanged
    {
        private readonly Simulation simulation;

        public SimulationViewModel(Simulation simulation)
        {
            this.simulation = simulation;
            this.Species = simulation.Species.Select(x => new SpeciesViewModel(x)).ToList();
            this.SelectedSpecies = Species.FirstOrDefault();
        }

        private World world => simulation.World;
        public WorldViewModel World => new WorldViewModel(world);

        private SpeciesViewModel _selectedSpecies;
        public SpeciesViewModel SelectedSpecies
        {
            get { return _selectedSpecies; }
            set
            {
                _selectedSpecies = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSpecies)));
            }
        }

        private IEnumerable<SpeciesViewModel> _species;
        public IEnumerable<SpeciesViewModel> Species
        {
            get { return _species; }
            set
            {
                _species = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Species)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class WorldViewModel : INotifyPropertyChanged
    {
        private readonly World world;
        private ObservableCollection<Boid> population => world.Population;

        public WorldViewModel(World world)
        {
            this.world = world;

            Population = new ObservableCollection<BoidViewModel>();
            population.CollectionChanged += ConvertBoidToViewModels;
            ConvertBoidToViewModels(null, null);
        }

        private void ConvertBoidToViewModels(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Population.Clear();
            foreach (Boid boid in population)
            {
                Population.Add(new BoidViewModel(boid));
            }
        }       

        private ObservableCollection<BoidViewModel> _population;
        public ObservableCollection<BoidViewModel> Population
        {
            get { return _population; }
            set
            {
                _population = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Population)));
            }
        }

        private ParameterBindings bindings => world.Bindings;
        public Cell<double> Width => bindings.Read(World.Width);
        public Cell<double> Height => bindings.Read(World.Height);

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class SpeciesViewModel : INotifyPropertyChanged
    {
        private readonly BoidSpecies species;

        public SpeciesViewModel(BoidSpecies species)
        {
            this.species = species;

            Parameters = new ObservableCollection<RangedParamViewModel>();
            foreach (IParameter param in Bindings.Parameters)
            {
                if (param is RangedDoubleParameter)
                {
                    Parameters.Add(new RangedParamViewModel(Bindings, (RangedDoubleParameter)param));
                }
            }
        }

        public string Name => char.ToUpper(species.Name[0]) + species.Name.Substring(1);
        public BoidSpecies Species => species;

        public ParameterBindings Bindings => species.Bindings;

        private ObservableCollection<RangedParamViewModel> _parameters;
        public ObservableCollection<RangedParamViewModel> Parameters
        {
            get { return _parameters; }
            set
            {
                if (value == _parameters)
                    return;

                _parameters = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Parameters)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class BoidViewModel
    {
        private readonly Boid boid;

        public BoidViewModel(Boid boid)
        {
            this.boid = boid;
        }

        public ParameterBindings Bindings => boid.Bindings;
        public World World => boid.World;
        public Cell<Vector2D> Position => boid.Position;
        public Cell<Vector2D> Velocity => boid.Velocity;
        public BoidSpecies Species => boid.Species;
    }

    public class RangedParamViewModel
    {
        private readonly RangedDoubleParameter param;
        private readonly ParameterBindings bindings;

        public RangedParamViewModel(ParameterBindings bindings, RangedDoubleParameter param)
        {
            this.param = param;
            this.bindings = bindings;
        }

        public String Label => this.param.Id;
        public Cell<double> Current => bindings.Read(this.param);
        public double Minimum => param.Minimum;
        public double Maximum => param.Maximum;
    }
}
