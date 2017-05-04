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
using System.Windows.Input;
using System.Windows.Threading;

namespace ViewModel
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            this.simulation = new Simulation();
            CreateHunter(50, 50);
            CreatePrey(150, 150);
            CreatePrey(20, 20);

            this.timer = new DispatcherTimer(TimeSpan.FromMilliseconds(20), DispatcherPriority.Render, (x, y) =>
            {
                Update(0.02);
            }, _dispatcher);

            timer.Start();
        }

        private readonly Dispatcher _dispatcher;
        private DispatcherTimer timer;

        private Simulation simulation;

        public SimulationViewModel Simulation => new SimulationViewModel(this.simulation);

        private void CreateHunter(double x, double y)
        {
            this.simulation.Species[0].CreateBoid(new Vector2D(x, y));
        }

        private void CreatePrey(double x, double y)
        {
            this.simulation.Species[1].CreateBoid(new Vector2D(x, y));
        }

        public void ToggleTimer()
        {
            if (this.timer.IsEnabled)
                this.timer.Stop();
            else
                this.timer.Start();
        }

        private ICommand _pause;
        public ICommand Pause
        {
            get
            {
                return _pause ?? (_pause = new CommandHandler(() => ToggleTimer(), true));
            }
        }

        public class CommandHandler : ICommand
        {
            private Action _action;
            private bool _canExecute;
            public CommandHandler(Action action, bool canExecute)
            {
                _action = action;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _action();
            }
        }

        public void Update(double dt)
        {
            this.simulation.Update(dt);
        }
    }

    public class SimulationViewModel : INotifyPropertyChanged {
        private readonly Simulation simulation;

        public SimulationViewModel(Simulation simulation)
        {
            this.simulation = simulation;
            this.Species = simulation.Species.Select(x => new SpeciesViewModel(x)).ToList();
            this.SelectedSpecie = Species.FirstOrDefault();
        }

        private World world => simulation.World;
        public WorldViewModel World => new WorldViewModel(world);

        private SpeciesViewModel _selectedSpecie;
        public SpeciesViewModel SelectedSpecie
        {
            get { return _selectedSpecie; }
            set
            {
                _selectedSpecie = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSpecie)));
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

        public WorldViewModel(World world)
        {
            this.world = world;

            Population = new ObservableCollection<BoidViewModel>();
            foreach (Boid boid in population)
                Population.Add(new BoidViewModel(boid));
        }

        private ObservableCollection<Boid> population => world.Population;

        private ObservableCollection<BoidViewModel> _population;
        public ObservableCollection<BoidViewModel> Population
        {
            get { return _population; }
            set
            {
                if (value == _population)
                    return;

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
        private readonly BoidSpecies specie;

        public SpeciesViewModel(BoidSpecies specie)
        {
            this.specie = specie;

            Parameters = new ObservableCollection<RangedParamViewModel>();
            foreach (IParameter param in Bindings.Parameters)
            {
                if (param is RangedDoubleParameter)
                {
                    Parameters.Add(new RangedParamViewModel(Bindings, (RangedDoubleParameter)param));
                }
            }
        }

        public String Name => specie.Name;
        public ParameterBindings Bindings => specie.Bindings;

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
        public IArtificialIntelligence AI => boid.AI;
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
