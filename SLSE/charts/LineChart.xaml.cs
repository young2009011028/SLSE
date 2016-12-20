using System;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using SLSE.Logger;

namespace Wpf.CartesianChart.PointShapeLine
{
    public partial class PointShapeLineExample : UserControl
    {
        #region Properties
        public List<LineSeries> Lines { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        #endregion

        #region Functions
        public PointShapeLineExample()
        {
            Lines = new List<LineSeries>();

            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                //new LineSeries
                //{
                //    Title = "Series 1",
                //    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                //},
                //new LineSeries
                //{
                //    Title = "Series 2",
                //    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                //    PointGeometry = null
                //},
                //new LineSeries
                //{
                //    Title = "Series 3",
                //    Values = new ChartValues<double> { 4,2,7,2,7 },
                //    PointGeometry = DefaultGeometries.Square,
                //    PointGeometrySize = 15
                //}
            };

            Labels = new[] { "00:00" };
           // YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            //SeriesCollection.Add(new LineSeries
            //{
            //    Title = "Series 4",
            //    Values = new ChartValues<double> { 5, 3, 2, 4 },
            //    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
            //    PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    PointGeometrySize = 50,
            //    PointForeround = Brushes.Gray
            //});

            //modifying any series values will also animate and update the chart
           // SeriesCollection[3].Values.Add(5d);

            DataContext = this;
        }


        public void SetTimeStampLabel(List<DateTime> timestamp)
        {
            try
            { Labels = new[] { timestamp[0].ToString(), timestamp[timestamp.Count-1].ToString() }; } 
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }

        public void AddLines(string signalname, List<double> values )
        {
            try
            {
                if (Lines.Exists(x => (x.Title == signalname )))
                {
                    Lines.Find(x => (x.Title == signalname )).Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    var line = new LineSeries
                    {
                        Title = signalname,
                        Values = new ChartValues<double>(values),
                       // PointGeometry = null,
                        LineSmoothness = 1
                    };
                    Lines.Add(line);
                    SeriesCollection.Add(line);
                    
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }

            
        }
        public void DeselectLines(string signalname)
        {
            try
            {
                if (Lines.Exists(x => (x.Title == signalname )))
                {
                    Lines.Find(x => (x.Title == signalname)).Visibility = System.Windows.Visibility.Hidden;
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