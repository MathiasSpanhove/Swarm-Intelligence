using Bindings;
using Cells;
using Mathematics;
using Model;
using Model.AI;
using Model.Species;
using System;
using System.Collections.ObjectModel;
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

    public class SimulationViewModel
    {
        private readonly Simulation simulation;

        public SimulationViewModel(Simulation simulation)
        {
            this.simulation = simulation;
        }

        private World world => simulation.World;

        public WorldViewModel World => new WorldViewModel(world);
    }

    public class WorldViewModel
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

        private  ObservableCollection<BoidViewModel> _population;
        public ObservableCollection<BoidViewModel> Population
        {
            get { return _population; }
            set
            {
                if (value == _population)
                    return;

                _population = value;
            }
        }

        private ParameterBindings bindings => world.Bindings;

        public Cell<double> Width => bindings.Read(World.Width);
        public Cell<double> Height => bindings.Read(World.Height);
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

        public Cell<double> MaximumSpeed => Bindings.Read(BoidSpecies.MaximumSpeed);

        public void SetMaximumSpeed()
        {
            MaximumSpeed.Value = BoidSpecies.MaximumSpeed.Maximum;
        }

        private ICommand _maxSpeed;
        public ICommand MaxSpeed
        {
            get
            {
                return _maxSpeed ?? (_maxSpeed = new CommandHandler(() => SetMaximumSpeed(), true));
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
    }
}
