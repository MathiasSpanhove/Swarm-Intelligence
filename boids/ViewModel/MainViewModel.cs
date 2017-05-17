using Mathematics;
using Microsoft.Practices.ServiceLocation;
using Model;
using Model.Species;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Timer.Interfaces;

namespace ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.simulation = new Simulation();
            this.Simulation = new SimulationViewModel(this.simulation);
            this.startupBoids();

            this.TickMilliseconds = 20;
            this.SpeedUpMultiplier = 1.0;
            _timer = ServiceLocator.Current.GetInstance<ITimerService>();
            _timer.Tick += Timer_Tick;
            _timer.Start(new TimeSpan(0, 0, 0, 0, TickMilliseconds));
        }

        // Simulation

        private Simulation simulation;

        public SimulationViewModel Simulation { get; }

        private void CreateHunter(double x, double y)
        {
            this.simulation.Species[0].CreateBoid(new Vector2D(x, y));
        }

        private void CreatePrey(double x, double y)
        {
            this.simulation.Species[1].CreateBoid(new Vector2D(x, y));
        }

        private void startupBoids()
        {
            CreateHunter(50, 50);
            CreateHunter(200, 200);
            for (var i = 0; i < 25; i++)
            {
                CreatePrey(150+i, 150+i);
            }
        }

        public void Update(double dt)
        {
            this.simulation.Update(dt);
        }

        private ICommand _addBoid;
        public ICommand AddBoid
        {
            get
            {
                return _addBoid ?? (_addBoid = new CommandParameterHandler((parameter) => CreateBoid((BoidSpecies)parameter), true));
            }
        }

        private ICommand _removeBoid;
        public ICommand RemoveBoid
        {
            get
            {
                return _removeBoid ?? (_removeBoid = new CommandParameterHandler((parameter) => DeleteBoid((BoidSpecies)parameter), true));
            }
        }

        private void CreateBoid(BoidSpecies specie)
        {
            int index = this.simulation.Species.IndexOf(specie);
            Random rnd = new Random();
            double x = rnd.Next(1, (int) Simulation.World.Width.Value);
            double y = rnd.Next(1, (int) Simulation.World.Height.Value);
            this.simulation.Species[index].CreateBoid(new Vector2D(x, y));
        }

        private void DeleteBoid(BoidSpecies specie)
        {
            foreach(Boid boid in this.simulation.World.Population)
            {
                if (boid.Species == specie)
                {
                    this.simulation.World.Population.Remove(boid);
                    break;
                }
            }
        }

        // Timer

        private ITimerService _timer;
        public int TickMilliseconds { get; set; }

        private double _speedUpMultiplier;
        public double SpeedUpMultiplier
        {
            get { return _speedUpMultiplier; }
            set
            {
                if (value == _speedUpMultiplier)
                    return;

                _speedUpMultiplier = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SpeedUpMultiplier)));
            }
        }

        private void Timer_Tick(ITimerService obj)
        {
            Update(Convert.ToDouble(TickMilliseconds)/1000 * SpeedUpMultiplier);
        }

        public void ToggleTimer()
        {
            _timer.ToggleOnOff();                
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

        public class CommandParameterHandler : ICommand
        {
            private Action<object> _action;
            private bool _canExecute;
            public CommandParameterHandler(Action<object> action, bool canExecute)
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
                _action(parameter);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}