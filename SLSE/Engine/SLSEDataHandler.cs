using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLSE.Logger;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Threading;
namespace SLSE.Engine
{

    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs() { this.Percentage = 0; }
        public int Percentage { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<Double> Value { get; set; }
    }

    class SLSEDataHandler
    {
        #region Properties
        //path
        private string _datafile_path;
        private string _modelfile_path;

        //databuffer
        //<timestamp,<measurment,value>>
        Dictionary<string, Dictionary<string, double>> _data_buffer;
        List<string> _signals;
        //result buffer
        Dictionary<string, Dictionary<string, double>> _result_buffer;

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

        public event EventHandler ProgressUpdate;
        #endregion

        #region Function
        public SLSEDataHandler()
        {
            _data_buffer = new Dictionary<string, Dictionary<string, double>>();
            _result_buffer = new Dictionary<string, Dictionary<string, double>>();
            _signals = new List<string>();
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
                                _signals.Add(field);
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
                                        frame.Add(_signals[columncount-1], Convert.ToDouble(field));
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
                    string modeltext = "";
                    File.WriteAllText(_modelfile_path, modeltext);
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
                    //pass frame to engine



                    //get result and insert to _result_buffer with timestamp


                    //update prograss bar

                    status.Percentage = (count * 100 / _data_buffer.Count);
                    status.TimeStamp = Convert.ToDateTime(frame_time.Key);
                    status.Value.Clear();
                    status.Value.Add( frame.First().Value);
                    status.Value.Add(frame.ElementAt(2).Value);
                    if (ProgressUpdate != null)
                        ProgressUpdate(this, status);

                    count++;
                    Thread.Sleep(10);
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
