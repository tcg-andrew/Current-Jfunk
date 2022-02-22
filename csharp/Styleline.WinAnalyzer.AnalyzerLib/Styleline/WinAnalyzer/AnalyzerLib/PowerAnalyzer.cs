namespace Styleline.WinAnalyzer.AnalyzerLib
{
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class PowerAnalyzer
    {
        private Styleline.WinAnalyzer.AnalyzerLib.RunState _runState = Styleline.WinAnalyzer.AnalyzerLib.RunState.Stopped;
        private string comPort;
        private bool hertz;
        private int iWrite;
        private ManualResetEvent PauseTrigger = new ManualResetEvent(false);
        private Queue<int> queue = new Queue<int>(5);
        private decimal[] readings = new decimal[7];
        private SerialPort serialPort;
        private object SyncRoot = new object();
        private static int WAIT_TIMEOUT = 500;

        public event ReadingsUpdateHandler ReadingsUpdated;

        public event EventHandler Running;

        public event RunStateChangeHandler RunStateChanged;

        static PowerAnalyzer()
        {
            WAIT_TIMEOUT = 0x3e8;
        }

        public PowerAnalyzer(string comPort)
        {
            this.comPort = comPort;
        }

        public decimal CalculateOhms()
        {
            try
            {
                decimal[] currentReadings = this.Current;
                if (currentReadings[3] == 0M)
                {
                    return 0M;
                }
                return Math.Round((decimal)(currentReadings[2] / this.readings[3]), 2);
            }
            catch (Exception ex)
            {
                throw new Exception("CalculateOhms  - " + ex.Message + " - " + ex.StackTrace);
            }
        }

        public void GetReadings()
        {
            try
            {
                while (true)
                {
                    object someObj;
                    Monitor.Enter(someObj = this.SyncRoot);
                    try
                    {
                        if (this.ReadingsUpdated != null)
                        {
                            this.ReadingsUpdated(this.readings);
                        }
                    }
                    finally
                    {
                        Monitor.Exit(someObj);
                    }
                    if (this.RunState == Styleline.WinAnalyzer.AnalyzerLib.RunState.Stopped)
                    {
                        return;
                    }
                    Thread.Sleep(WAIT_TIMEOUT);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetReadings - " + ex.Message + " - " + ex.StackTrace);
            }
        }

        private void MainLoop()
        {
            try
            {
                using (this.serialPort = new SerialPort(this.comPort, 0x2580, Parity.None, 8, StopBits.One))
                {
                    this.serialPort.DataReceived += new SerialDataReceivedEventHandler(this.serial_DataReceived);
                    this.serialPort.ReceivedBytesThreshold = 1;
                    this.serialPort.DtrEnable = true;
                    this.serialPort.Handshake = Handshake.None;
                    this.serialPort.DiscardNull = false;
                    this.serialPort.RtsEnable = false;
                    this.serialPort.Open();
                    while (true)
                    {
                        int lociWrite = 0;
                        lock (this.SyncRoot)
                        {
                            lociWrite = this.iWrite++;
                        }
                        if (lociWrite >= 2)
                        {
                            this.WriteValue(Readings.STATUS, 0M);
                        }
                        this.serialPort.Write(new byte[] { 0xff }, 0, 1);
                        if (this.Running != null)
                        {
                            this.Running(this, new EventArgs());
                        }
                        if (this.RunState == Styleline.WinAnalyzer.AnalyzerLib.RunState.Stopped)
                        {
                            this.queue.Clear();
                            break;
                        }
                        Thread.Sleep(WAIT_TIMEOUT);
                    }
                    this.serialPort.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MainLoop - " + ex.Message + " - " + ex.StackTrace);
            }
        }

        private void process()
        {
            try
            {
                this.queue.Dequeue();
                int int2 = this.queue.Dequeue();
                int int3 = this.queue.Dequeue();
                int int4 = this.queue.Dequeue();
                this.queue.Dequeue();
                string binStr = Convert.ToString(int4, 2).PadLeft(8, '0') + Convert.ToString(int3, 2).PadLeft(8, '0');
                binStr = Utility.Reverse(binStr, binStr.Length);
                decimal d = 0M;
                string type = string.Format("{0:x2}", int2).ToUpper();
                bool lochertz = type == "05";
                Utility.ParseNumber(binStr, lochertz, out d);
                switch (type)
                {
                    case "03":
                        this.WriteValue(Readings.Volts, d);
                        return;

                    case "04":
                        this.WriteValue(Readings.Volts, d);
                        return;

                    case "05":
                        this.WriteValue(Readings.Hertz, d);
                        lock (this.SyncRoot)
                        {
                            this.hertz = true;
                            return;
                        }

                    case "31":
                        break;

                    case "21":
                        this.WriteValue(Readings.Amps, d);
                        return;

                    case "C0":
                        this.WriteValue(Readings.Watts, d);
                        return;

                    case "C1":
                        this.WriteValue(Readings.Watts, d);
                        return;

                    case "D0":
                        this.WriteValue(Readings.PF, d);
                        return;

                    default:
                        return;
                }
                this.WriteValue(Readings.Amps, d);
            }
            catch (Exception ex)
            {
                throw new Exception("process - " + ex.Message + " - " + ex.StackTrace);
            }
        }

        private void serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                lock (this.SyncRoot)
                {
                    this.iWrite = 0;
                }
                this.WriteValue(Readings.STATUS, 1M);
                while (this.serialPort.BytesToRead > 0)
                {
                    if (this.queue.Count == 5)
                    {
                        this.queue.Dequeue();
                    }
                    int queueByte = this.serialPort.ReadByte();
                    string str = string.Format("{0:x2}", queueByte);
                    if ((this.queue.Count > 0) || (str == "02"))
                    {
                        this.queue.Enqueue(queueByte);
                    }
                    if (((this.queue.Count == 5) && (string.Format("{0:x2}", this.queue.Peek()) == "02")) && (str == "03"))
                    {
                        this.process();
                        this.queue.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("serial_DataReceived - " + ex.Message + " - " + ex.StackTrace);
            }
        }

        public void Start()
        {
            try
            {
                if (this.RunState == Styleline.WinAnalyzer.AnalyzerLib.RunState.Stopped)
                {
                    this.RunState = Styleline.WinAnalyzer.AnalyzerLib.RunState.Started;
                    if (this.RunStateChanged != null)
                    {
                        this.RunStateChanged(this.RunState);
                    }
                    ThreadStart ts = new ThreadStart(this.MainLoop);
                    ThreadStart ts2 = new ThreadStart(this.GetReadings);
                    Thread t = new Thread(ts);
                    Thread t2 = new Thread(ts2);
                    t.Start();
                    t2.Start();
                    this.PauseTrigger.Set();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Start - " + ex.Message + " - " + ex.StackTrace);
            }
        }

        public void Stop()
        {
            try
            {
            if (this.RunState != Styleline.WinAnalyzer.AnalyzerLib.RunState.Stopped)
            {
                this.RunState = Styleline.WinAnalyzer.AnalyzerLib.RunState.Stopped;
                if (this.RunStateChanged != null)
                {
                    this.RunStateChanged(this.RunState);
                }
            }
            this.PauseTrigger.Reset();
            }

            catch (Exception ex)
            {
                throw new Exception("Stop - " + ex.Message + " - " + ex.StackTrace);
            }
        }

        private void WriteValue(Readings reading, decimal value)
        {
            try
            {
                lock (this.SyncRoot)
                {
                    this.readings[(int)reading] = value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("WriteValue - " + ex.Message + " - " + ex.StackTrace);
            }
        }

        public decimal[] Current
        {
            get
            {
                try
                {
                    lock (this.SyncRoot)
                    {
                        return (decimal[])this.readings.Clone();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("CalculateOhms - " + ex.Message + " - " + ex.StackTrace);
                }
            }
        }

        public Styleline.WinAnalyzer.AnalyzerLib.RunState RunState
        {
            get
            {
                try
                {
                    lock (this.SyncRoot)
                    {
                        return this._runState;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("CalculateOhms - " + ex.Message + " - " + ex.StackTrace);
                }
            }
            private set
            {
                try
                {
                    lock (this.SyncRoot)
                    {
                        this._runState = value;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("CalculateOhms - " + ex.Message + " - " + ex.StackTrace);
                }
            }
        }
    }
}

