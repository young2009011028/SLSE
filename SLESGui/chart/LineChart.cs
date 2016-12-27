using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using SLSE.Logger;
using SLSE.Engine;

namespace SLESGui.chart
{
    public partial class LineChart : UserControl
    {
        #region Properties
        private List<Series> Lines;
        public  SLSEDataHandler Data_Handler{get;set;}
        #endregion
        #region Functions
        public LineChart()
        {
            InitializeComponent();

            Lines = new List<Series>();
            SetChartAreaProperties();
            //dataGridView1.DataSource = Data_Handler.Signals;
        }

        public void InitializeDataGrid()
        {
            dataGridView1.DataSource = Data_Handler.Signals;
           
        }

        public void SetChartAreaYMaxMin()
        {
            foreach (var ca in VolChart.ChartAreas)
            {
                if (ca.Visible)
                {
                    var series = VolChart.Series.Where(o => o.ChartArea == ca.Name);
                    List<System.Windows.Forms.DataVisualization.Charting.Series> newseris = new List<System.Windows.Forms.DataVisualization.Charting.Series>();
                    //only count enabled series
                    foreach (var s in series)
                    {
                        if (s.Enabled)
                        {
                            newseris.Add(s);
                        }
                    }
                    double max = newseris.Max(o => o.Points.Max(p => p.YValues[0]));
                    double min = newseris.Min(o => o.Points.Min(p => p.YValues[0]));

                    double offset = (max - min) * 0.1;

                    ca.AxisY.Minimum = (min >= 0 && (min - offset) <= 0) ? 0 : min - offset;
                    ca.AxisY.Maximum = max + offset;
                }
            }

            foreach (var ca in CurChart.ChartAreas)
            {
                if (ca.Visible)
                {
                    var series = CurChart.Series.Where(o => o.ChartArea == ca.Name);
                    List<System.Windows.Forms.DataVisualization.Charting.Series> newseris = new List<System.Windows.Forms.DataVisualization.Charting.Series>();
                    //only count enabled series
                    foreach (var s in series)
                    {
                        if (s.Enabled)
                        {
                            newseris.Add(s);
                        }
                    }
                    double max = newseris.Max(o => o.Points.Max(p => p.YValues[0]));
                    double min = newseris.Min(o => o.Points.Min(p => p.YValues[0]));

                    double offset = (max - min) * 0.1;

                    ca.AxisY.Minimum = (min >= 0 && (min - offset) <= 0) ? 0 : min - offset;
                    ca.AxisY.Maximum = max + offset;
                }
            }
        }
        private void SetChartAreaProperties()
        {
            foreach (var ca in VolChart.ChartAreas)
            {
                ca.AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                ca.AlignWithChartArea = VolChart.ChartAreas[0].Name;
                ca.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial Narrow", 6.0f);
                ca.AxisY.LabelStyle.Font = new System.Drawing.Font("Arial Narrow", 7.0f);
            }
            foreach (var ca in CurChart.ChartAreas)
            {
                ca.AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                ca.AlignWithChartArea = CurChart.ChartAreas[0].Name;
                ca.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial Narrow", 6.0f);
                ca.AxisY.LabelStyle.Font = new System.Drawing.Font("Arial Narrow", 7.0f);
            }
        }
        public void AddLines(string signalname, List<KeyValuePair<DateTime, double>> valuelist)
        {
            try
            {
                if (Lines.Exists(x => (x.Name == signalname)))
                {
                    Lines.Find(x => (x.Name == signalname)).Enabled = true;
                }
                else
                {
                    var line = new Series
                    {
                        Name = signalname,
                        XValueType=ChartValueType.DateTime,
                        ChartType = SeriesChartType.Line

                    };

                    foreach (var value in valuelist)
                    {
                        line.Points.AddXY(value.Key, value.Value);
                    }

                    if (line.Name.Contains(".VM"))
                    {
                        line.ChartArea = "VMArea";
                        line.Legend = "VMLegend";
                        VolChart.Series.Add(line);

                    }
                    else if (line.Name.Contains(".VA"))
                    {
                        line.ChartArea = "VAArea";
                        line.Legend = "VALegend";
                        VolChart.Series.Add(line);
                    }
                    else if (line.Name.Contains(".IM"))
                    {
                        line.ChartArea = "IMArea";
                        line.Legend = "IMLegend";
                        CurChart.Series.Add(line);
                    }
                    else if (line.Name.Contains(".IA"))
                    {
                        line.ChartArea = "IAArea";
                        line.Legend = "IALegend";
                        CurChart.Series.Add(line);
    
                    }
                    Lines.Add(line); 
                                


                }

                SetChartVisibility();
                SetChartAreaYMaxMin();

                VolChart.Update();
                CurChart.Update();
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
                if (Lines.Exists(x => (x.Name== signalname)))
                {
                     Lines.Find(x => (x.Name == signalname)).Enabled =false;
                }

                SetChartVisibility();
                SetChartAreaYMaxMin();
                VolChart.Update();
                CurChart.Update();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }

        public void SetChartVisibility()
        {
            bool vmflag = false;
            bool vaflag = false;
            bool imflag = false;
            bool iaflag = false;
            foreach (var line in Lines)
            {
                if (line.Name.Contains(".VM") && line.Enabled)
                {
                    vmflag = true;
                }
                if (line.Name.Contains(".VA") && line.Enabled)
                {
                    vaflag = true;
                }
                if (line.Name.Contains(".IM") && line.Enabled)
                {
                    imflag = true;

                }
                if (line.Name.Contains(".IA") && line.Enabled)
                {
                    iaflag = true;
                }
            }
            VolChart.ChartAreas["VMArea"].Visible = vmflag;
            VolChart.ChartAreas["VAArea"].Visible = vaflag;
            CurChart.ChartAreas["IMArea"].Visible = imflag;
            CurChart.ChartAreas["IAArea"].Visible = iaflag;

            VolChart.Visible = (vmflag || vaflag);
            CurChart.Visible = (imflag || iaflag);
            if (VolChart.Visible && !CurChart.Visible)
            {
                tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Percent;
                tableLayoutPanel1.RowStyles[1].Height = 0;
                tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Percent;
                tableLayoutPanel1.RowStyles[0].Height = 100;
            }
            else if (!VolChart.Visible && CurChart.Visible)
            {
                tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Percent;
                tableLayoutPanel1.RowStyles[0].Height = 0;
                tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Percent;
                tableLayoutPanel1.RowStyles[1].Height = 100;
            }
            else
            {
                tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Percent;
                tableLayoutPanel1.RowStyles[0].Height = 50;
                tableLayoutPanel1.RowStyles[1].SizeType = SizeType.Percent;
                tableLayoutPanel1.RowStyles[1].Height = 50;
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 1 && e.RowIndex >= 0)
                    this.dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                string signalname = this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                //Check the value of cell
                if ((bool)this.dataGridView1.CurrentCell.Value == true)
                {
                    Dictionary<DateTime, double[]> result = new Dictionary<DateTime, double[]>();
                    Data_Handler.GetSignalResult(signalname, result);
                    List<KeyValuePair<DateTime, double>> original_values = new List<KeyValuePair<DateTime, double>>();
                    List<KeyValuePair<DateTime, double>> estimated_values = new List<KeyValuePair<DateTime, double>>();
                    foreach (var frame in result)
                    {
                        original_values.Add(new KeyValuePair<DateTime, double>(frame.Key, frame.Value[0]));
                        estimated_values.Add(new KeyValuePair<DateTime, double>(frame.Key, frame.Value[1]));

                    }

                    AddLines(signalname, original_values);
                    AddLines("Estimated_" + signalname, estimated_values);
                }
                else
                {
                    DeselectLines(signalname);
                    DeselectLines("Estimated_" + signalname);
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }

        #endregion

        private void multiplelinechart_Click(object sender, EventArgs e)
        {

        }

    }
}
