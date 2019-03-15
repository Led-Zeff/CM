using System;
using System.Timers;

namespace CM.Utilities
{
    class SearchTrigger<T>
    {
        Action<T> filterFunction;
        T previousAppliedFilter;
        T currentFilter;
        Timer timer;

        public double Interval
        {
            get { return timer.Interval; }
            set { timer.Interval = value; }
        }

        public SearchTrigger(Action<T> filterFunction)
        {
            this.filterFunction = filterFunction;
            previousAppliedFilter = default(T);
            SetTimer();
        }

        public SearchTrigger(Action<T> filterFunction, T initialValue)
        {
            this.filterFunction = filterFunction;
            this.previousAppliedFilter = initialValue;
            SetTimer();
        }

        void SetTimer()
        {
            timer = new Timer(400);
            timer.Elapsed += timer_Elapsed;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!object.Equals(currentFilter, previousAppliedFilter))
            {
                previousAppliedFilter = currentFilter;
                filterFunction(currentFilter);
            }
        }

        public void SetFilter(T filter)
        {
            currentFilter = filter;
            timer.Stop();
            timer.Start();
        }
    }
}
