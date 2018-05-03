using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class TimeQuantizer
    {
        private readonly double quantum;

        private double accumulator;

        private readonly Action<double> receiver;

        public TimeQuantizer(double quantum, Action<double> receiver)
        {
            this.quantum = quantum;
            this.accumulator = 0;
            this.receiver = receiver;
        }

        public void Update(double dt)
        {
            this.accumulator += dt;

            while (this.accumulator > quantum)
            {
                receiver(this.quantum);
                this.accumulator -= quantum;
            }
        }
    }
}
