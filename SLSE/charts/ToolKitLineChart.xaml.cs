using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.DataVisualization.Charting;
using SLSE.Logger;

namespace SLSE.charts
{

    /// <summary>
    /// ToolKitLineChart.xaml 的交互逻辑
    /// </summary>
    public partial class ToolKitLineChart : UserControl
    {        
        #region Properties
        public List<LineSeries> Lines { get; set; }
        #endregion

        #region Functions
        public ToolKitLineChart()
        {
            Lines = new List<LineSeries>();
            //lineChart.Series = new System.Collections.ObjectModel.Collection<ISeries>();
            InitializeComponent();
            DataContext = this;

        }

        public void AddLines(string signalname, List<KeyValuePair<DateTime, double>> valuelist)
        {
            try
            {
                if (Lines.Exists(x => ((string)x.Title == signalname)))
                {
                    Lines.Find(x => ((string)x.Title == signalname)).Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    var line = new LineSeries
                    {
                        Title = signalname,
                        DataPointStyle = new Style
                        {
                            TargetType = typeof(DataPoint),
                            Setters = { new Setter(TemplateProperty, null) }
                        }
                    };

                    //List<KeyValuePair<string, double>> valueList = new List<KeyValuePair<string,double>>();
                    //foreach (var value in values)
                    //{
                    //    valueList.Add(new KeyValuePair<string, double>("",value));
                    //}
                    line.DependentValuePath = "Value";
                    line.IndependentValuePath = "Key";
                    line.ItemsSource = valuelist;
                    
                    Lines.Add(line);
                    lineChart.Series.Add(line);

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
                if (Lines.Exists(x => ((string)x.Title == signalname)))
                {
                    Lines.Find(x => ((string)x.Title == signalname)).Visibility = System.Windows.Visibility.Hidden;
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
