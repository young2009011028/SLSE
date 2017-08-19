
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using System.Threading;
using SLSE.Logger;
using SLSE.Engine;
using SLESGui.chart;
namespace SLESGui
{
    public partial class MainGui : Form
    {

        //slsedatahandler
        SLSEDataHandler slse_handler;
        Thread LSEthread;

        public MainGui()
        {
            try
            {
                InitializeComponent();


                //initialize handler
                slse_handler = new SLSEDataHandler();
                resultchart.Data_Handler = slse_handler;
                //data binding
                dgLog.DataSource = Log4NetHelper.Instance.GetEntries();
                dgAlarm.DataSource = AlarmLogger.Instance.GetEntries();
                dgSignals.AutoGenerateColumns = false;
                dgSignals.DataSource = slse_handler.Signals;

                //settable view
                tableLayoutresult.Enabled = false;

               // dgResultSignals.DataContext = slse_handler.Signals;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }

        private void btnBrowseInputModel_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                // Set filter for file extension and default file extension
                ofd.DefaultExt = ".xml";
                ofd.Filter = "XML documents (.xml)|*.xml";

                DialogResult dr = ofd.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    tbxInputModelName.Text = ofd.FileName;
                }


            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }


        private void btnBrowseScadaFile_Click(object sender, EventArgs e)
        {
            try
            {


                OpenFileDialog ofd = new OpenFileDialog();
                // Set filter for file extension and default file extension
                ofd.DefaultExt = ".csv";
                ofd.Filter = "CSV documents (.csv)|*.csv";

                DialogResult dr = ofd.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    tbxInputScadaDataName.Text = ofd.FileName;
                }


            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }


        private void btnBrowseInputFile_Click(object sender, EventArgs e)
        {
            try
            {


                OpenFileDialog ofd = new OpenFileDialog();
                // Set filter for file extension and default file extension
                ofd.DefaultExt = ".csv";
                ofd.Filter = "CSV documents (.csv)|*.csv";

                DialogResult dr = ofd.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    tbxInputFileName.Text = ofd.FileName;
                }

               
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }


        private void CalculationStatusReceived(object sender, ProgressEventArgs e)
        {
            try
            {
                if (pbCalculate.InvokeRequired)
                {
                    pbCalculate.Invoke((MethodInvoker)delegate
                    {
                        CalculationStatusReceived(sender, e);
                    });
                }
                else
                {
                    pbCalculate.Value = ((ProgressEventArgs)e).Percentage;
                    if (pbCalculate.Value == 100)
                    {
                        tbMain.SelectedTab = tabpageresult;
                        tableLayoutresult.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(tbxInputFileName.Text) && File.Exists(tbxInputModelName.Text))
                {
                    AlarmLogger.Instance.SetAlarmThreshold(Convert.ToInt32(tbxAlarm.Text));
                    //MessageBox.Show("成功");
                    slse_handler.ModelPath = tbxInputModelName.Text;
                    slse_handler.DataPath = tbxInputFileName.Text;

                    if (File.Exists(tbxInputScadaDataName.Text))
                    {
                        slse_handler.Scada_Path = tbxInputScadaDataName.Text;
                    }
                    slse_handler.LoadData();
                    slse_handler.LoadModel();

                    //set sample rate
                    if (tbxsamplecount.Text != "")
                        slse_handler.SampleRate = Convert.ToInt32(tbxsamplecount.Text);
                    else
                        slse_handler.SampleRate = 0;

                    //initialize linechart datagrid
                    resultchart.InitializeDataGrid();
                    //Calculate
                    slse_handler.ProgressUpdate += (s, status) =>
                    {
                        CalculationStatusReceived(s, (ProgressEventArgs)status);
                        //if (((ProgressEventArgs)status).Percentage == 100)
                        //{

        
                        //}
                    };
                    LSEthread = new Thread(new ParameterizedThreadStart(slse_handler.Run));
                    LSEthread.Start();

                }
                else if (!File.Exists(tbxInputModelName.Text))
                {  
                    MessageBox.Show("无法打开数据文件");
                    Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Info", "无法打开数据文件"));
                }
                else
                {
                   MessageBox.Show("无法打开模型文件");
                }      
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }

        private void btnAddChart_Click(object sender, EventArgs e)
        {
            try
            {//add new tab
                var tabpage = new TabPage();
                tabpage.Text = "图表";
                // add new chart
                var newchart = new LineChart();
                newchart.Data_Handler = slse_handler;
                newchart.InitializeDataGrid();
                newchart.Dock = DockStyle.Fill;

                tabpage.Controls.Add(newchart);
                tabresult.TabPages.Add(tabpage);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }

        private void tabresult_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                e.Graphics.DrawString("x", e.Font, Brushes.Black, e.Bounds.Right - 15, e.Bounds.Top + 4);
                e.Graphics.DrawString(this.tabresult.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + 12, e.Bounds.Top + 4);
                e.DrawFocusRectangle();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }

        private void tabresult_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                for (int i = 0; i < this.tabresult.TabPages.Count; i++)
                {
                    Rectangle r = tabresult.GetTabRect(i);
                    //Getting the position of the "x" mark.
                    Rectangle closeButton = new Rectangle(r.Right - 15, r.Top + 4, 9, 7);
                    if (closeButton.Contains(e.Location))
                    {
                        if (MessageBox.Show("Would you like to Close this Tab?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.tabresult.TabPages.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }

        private void btnSaveAsCSV_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    slse_handler.DumpToCSV(saveFileDialog1.FileName);
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void MainGui_Load(object sender, EventArgs e)
        {

        }
        private void new_proj_menu_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("新建项目会丢失现有的所有内容，继续？","新建项目", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Restart();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
             }

        }
    }
}
