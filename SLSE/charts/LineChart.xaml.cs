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


            linechart.DataTooltip = null;
            linechart.DisableAnimations = true;
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
                        PointGeometry = null,
                        LineSmoothness = 1,
                        Fill = Brushes.Transparent,

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
                   // Lines.Find(x => (x.Title == signalname)).Visibility = System.Windows.Visibility.Collapsed;
                    SeriesCollection.Remove(Lines.Find(x => (x.Title == signalname)));
                    Lines.Remove(Lines.Find(x => (x.Title == signalname)));

                }

            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }
        #endregion

        //private void btnhide_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    ((LineSeries)SeriesCollection[0]).Visibility = System.Windows.Visibility.Collapsed;
        //}


    }
}