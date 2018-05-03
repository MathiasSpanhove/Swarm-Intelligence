using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Timer.Interfaces;

namespace Timer.Services {
    public class TimerService : ITimerService {
        public TimerService() {
            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e) {
            Tick?.Invoke(this);
        }

        public void ChangeInterval(TimeSpan interval)
        {
            _timer.Interval = interval;
        }

        public void Start(TimeSpan interval) {
            _timer.Interval = interval;
            if(!_timer.IsEnabled) {
                _timer.Start();
            }
        }

        public void Stop() {
            _timer.Stop();
        }

        public void ToggleOnOff()
        {
            if (_timer.IsEnabled)
                _timer.Stop();
            else
                _timer.Start();
        }

        public event Action<ITimerService> Tick;
        private DispatcherTimer _timer;
    }
}
