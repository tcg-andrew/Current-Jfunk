namespace Styleline.WinAnalyzer.DAL.Entities
{
    using System;

    public class CurrentPowerReading : NotifyChangedObservable
    {
        private decimal amps;
        private decimal ohms;
        private decimal powerFactor;
        private decimal volts;
        private decimal watts;

        public decimal Amps
        {
            get
            {
                return this.amps;
            }
            set
            {
                bool changed = this.amps != value;
                this.amps = value;
                if (changed)
                {
                    this.RaisePropertyChanged<CurrentPowerReading, decimal>(p => p.Amps);
                }
            }
        }

        public decimal Ohms
        {
            get
            {
                return this.ohms;
            }
            set
            {
                bool changed = this.ohms != value;
                this.ohms = value;
                if (changed)
                {
                    this.RaisePropertyChanged<CurrentPowerReading, decimal>(p => p.Ohms);
                }
            }
        }

        public decimal PowerFactor
        {
            get
            {
                return this.powerFactor;
            }
            set
            {
                bool changed = this.powerFactor != value;
                this.powerFactor = value;
                if (changed)
                {
                    this.RaisePropertyChanged<CurrentPowerReading, decimal>(p => p.PowerFactor);
                }
            }
        }

        public decimal Volts
        {
            get
            {
                return this.volts;
            }
            set
            {
                bool changed = this.volts != value;
                this.volts = value;
                if (changed)
                {
                    this.RaisePropertyChanged<CurrentPowerReading, decimal>(p => p.Volts);
                }
            }
        }

        public decimal Watts
        {
            get
            {
                return this.watts;
            }
            set
            {
                bool changed = this.watts != value;
                this.watts = value;
                if (changed)
                {
                    this.RaisePropertyChanged<CurrentPowerReading, decimal>(p => p.Watts);
                }
            }
        }
    }
}

