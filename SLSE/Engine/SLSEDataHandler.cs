﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLSE.Logger;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Threading;
using SubstationLSE;

namespace SLSE.Engine
{
    public class Signal
    {
        public Signal(string name, bool ischecked) { SignalName = name; IsChecked = ischecked; }
        public string SignalName{get;set;}
        public bool IsChecked { get; set; }
    }

    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs() { this.Percentage = 0; }
        public int Percentage { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<Double> Value { get; set; }
    }

    class SLSEDataHandler
    {
        #region Private Member
        //path
        private string _datafile_path;
        private string _modelfile_path;

        //databuffer
        //<timestamp,<measurment,value>>
        private Dictionary<string, Dictionary<string, double>> _data_buffer;
        private List<Signal> _signals;

        //result buffer
        private Dictionary<string, Dictionary<string, double>> _result_buffer;

        //SLSE Engine

        private Substation _substation;

        #endregion
        #region Properties
        public string DataPath
        {
            set
            { this._datafile_path = value; }
            get
            { return this._datafile_path; }
        }

        public string ModelPath
        {
            set
            { this._modelfile_path = value; }
            get
            { return this._modelfile_path; }
        }
        public List<Signal> Signals
        {
            get { return this._signals; }
        }
        public event EventHandler ProgressUpdate;
        #endregion

        #region Function
        public SLSEDataHandler()
        {
            _data_buffer = new Dictionary<string, Dictionary<string, double>>();
            _result_buffer = new Dictionary<string, Dictionary<string, double>>();
            _signals = new List<Signal>();
            _substation = new Substation();
        }

        public bool LoadData()
        {

            if (!File.Exists(_datafile_path))
            { throw new Exception(_datafile_path + "打开错误"); }

            try
            {
                _data_buffer.Clear();
                _signals.Clear();
                using (TextFieldParser parser = new TextFieldParser(_datafile_path))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    int rowcount = 1;
                    while (!parser.EndOfData)
                    {

                        int columncount = 1;
                        string timestamp = "";
                        Dictionary<string, double> frame = new Dictionary<string, double>();
                        //clear frame
                       // frame.Clear();
                        //Process row
                        string[] fields = parser.ReadFields();
                        foreach (string field in fields)
                        {
                            if (rowcount == 1)
                            {
                                _signals.Add(new Signal(field,false));
                            }
                            else
                            {
                                if (fields.Count() != _signals.Count)
                                {
                                    throw new Exception("CSV 文件格式有误，发生在第" + rowcount.ToString() + "行");
                                }
                                else
                                {
                                    if (columncount == 1)
                                    {
                                        timestamp = field;
                                    }
                                    else
                                    {
                                        frame.Add(_signals[columncount-1].SignalName, Convert.ToDouble(field));
                                    }
                                }
                            }
                            columncount++;
                        }
                        if (rowcount != 1)
                        { _data_buffer.Add(timestamp, frame); }
                        rowcount++;
                    }
                }

                ////remove first column of csv for _signals
                _signals.RemoveAt(0);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
                return false;
            }

            return true;
        }

        public bool LoadModel()
        {
            try
            {
                if (!File.Exists(_modelfile_path))
                {
                    throw new Exception(_modelfile_path + "打开错误");
                }
                else
                {
                    _substation = _substation.DeserializeFromXml(_modelfile_path);
                    _substation.Initialize();
                    ////////pass modeltext to engine
                }

            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
                return false;
            }
            return true;

        }

        public void Run(object obj)
        {
            try
            {
                ProgressEventArgs status = new ProgressEventArgs();
                status.Value = new List<double>();
                int count = 0;
                foreach (var frame_time in _data_buffer)
                {
                    var frame = frame_time.Value;
                    _substation.InputMeasurements = new Dictionary<string,double>(frame);
                    _substation.SLSE();
                    //pass frame to engine

                    var outputframe = new Dictionary<string,double>(_substation.OutputMeasurements);

                    //get result and insert to _result_buffer with timestamp

                    _result_buffer.Add(frame_time.Key, outputframe);
                    //update prograss bar

                    status.Percentage = (count * 100 / _data_buffer.Count);
                    status.TimeStamp = Convert.ToDateTime(frame_time.Key);
                    if (ProgressUpdate != null)
                        ProgressUpdate(this, status);

                    count++;
                    //Thread.Sleep(10);
                }


                status.Percentage =100;
                if (ProgressUpdate != null)
                    ProgressUpdate(this, status);

            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }


        public void GetSignalResult(string signalname,  Dictionary<DateTime,double[]> result )
        {

            try
            {
                //result = new Dictionary<DateTime, double[]>();
                if (_data_buffer.Count == _result_buffer.Count)
                {
                    foreach (var signal in _data_buffer)
                    {
                        foreach (var frame in signal.Value)
                        {
                            if (frame.Key == signalname)
                            {
                                var result_frame = (_result_buffer[signal.Key])[frame.Key];

                                result.Add(Convert.ToDateTime(signal.Key), new double[] { frame.Value, result_frame });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
                
            }
        }
        #endregion



    }
}
