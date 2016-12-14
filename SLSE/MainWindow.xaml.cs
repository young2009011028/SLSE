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
using System.IO;
using System.Collections.ObjectModel;
using System.Threading;

using SLSE.Logger;
using SLSE.Engine;
namespace SLSE
{



    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //slsedatahandler
        SLSEDataHandler slse_handler;
        Thread LSEthread;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                //data binding
                dgLog.DataContext = Log4NetHelper.Instance.GetEntries();
                //initialize handler
                slse_handler = new SLSEDataHandler();
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }


        #region InputTab
        private void btnInputFileClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                // Set filter for file extension and default file extension
                dlg.DefaultExt = ".csv";
                dlg.Filter = "CSV documents (.csv)|*.csv";

                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox
                if (result == true)
                {
                    // Open document
                    string filename = dlg.FileName;
                    tbxInputFileName.Text = filename;
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }


        private void btnInputModelClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                // Set filter for file extension and default file extension
                dlg.DefaultExt = ".xml";
                dlg.Filter = "XML documents (.xml)|*.xml";

                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox
                if (result == true)
                {
                    // Open document
                    string filename = dlg.FileName;
                    tbxInputModelName.Text = filename;
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }

        private void btnLoadFileClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(tbxInputModelName.Text) && File.Exists(tbxInputFileName.Text))
                {
                    System.Windows.MessageBox.Show("成功");


                    // Log4NetHelper.Log(this, new LogEntry { Message = "成功" });
                }
                else if (!File.Exists(tbxInputFileName.Text))
                {
                    System.Windows.MessageBox.Show("无法打开数据文件");
                    Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Info", "无法打开数据文件"));
                }
                else
                {
                    System.Windows.MessageBox.Show("无法打开模型文件");
                }

                slse_handler.ModelPath = tbxInputModelName.Text;
                slse_handler.DataPath = tbxInputFileName.Text;
                slse_handler.LoadData();
                slse_handler.LoadModel();
            }

            catch (Exception ex)
            {
                Log4NetHelper.Instance.LogEntries(new LogEntry(DateTime.Now, "Error", ex.Message));
            }
        }


        private void btnCalculateClick(object sender, RoutedEventArgs e)
        {
            slse_handler.ProgressUpdate += (s, status) =>
            {
                Dispatcher.Invoke((Action)delegate() { 
                    pbCalculate.Value = ((ProgressEventArgs)status).Percentage;
                    rtlChart.SetNextPoint(((ProgressEventArgs)status).TimeStamp, ((ProgressEventArgs)status).Value[0]);
                });
            };
            LSEthread = new Thread(new ParameterizedThreadStart(slse_handler.Run));
            LSEthread.Start();

        }
        #endregion
    }



}
