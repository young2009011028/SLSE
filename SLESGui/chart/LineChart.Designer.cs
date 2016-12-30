namespace SLESGui.chart
{
    partial class LineChart
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SignaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsCheckedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.VolChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.CurChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurChart)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.VolChart, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.CurChart, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(937, 580);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SignaName,
            this.IsCheckedColumn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.tableLayoutPanel1.SetRowSpan(this.dataGridView1, 2);
            this.dataGridView1.Size = new System.Drawing.Size(462, 574);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // SignaName
            // 
            this.SignaName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SignaName.DataPropertyName = "SignalName";
            this.SignaName.HeaderText = "信号";
            this.SignaName.Name = "SignaName";
            // 
            // IsCheckedColumn
            // 
            this.IsCheckedColumn.DataPropertyName = "IsChecked";
            this.IsCheckedColumn.FalseValue = "false";
            this.IsCheckedColumn.HeaderText = "选择";
            this.IsCheckedColumn.Name = "IsCheckedColumn";
            this.IsCheckedColumn.TrueValue = "true";
            // 
            // VolChart
            // 
            chartArea1.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea1.Name = "VMArea";
            chartArea1.Visible = false;
            chartArea2.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea2.Name = "VAArea";
            chartArea2.Visible = false;
            this.VolChart.ChartAreas.Add(chartArea1);
            this.VolChart.ChartAreas.Add(chartArea2);
            this.VolChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.DockedToChartArea = "VMArea";
            legend1.Name = "VMLegend";
            legend2.DockedToChartArea = "VAArea";
            legend2.Name = "VALegend";
            this.VolChart.Legends.Add(legend1);
            this.VolChart.Legends.Add(legend2);
            this.VolChart.Location = new System.Drawing.Point(471, 3);
            this.VolChart.Name = "VolChart";
            this.VolChart.Size = new System.Drawing.Size(463, 284);
            this.VolChart.TabIndex = 1;
            this.VolChart.Text = "VolChart";
            this.VolChart.Click += new System.EventHandler(this.multiplelinechart_Click);
            // 
            // CurChart
            // 
            chartArea3.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea3.Name = "IMArea";
            chartArea3.Visible = false;
            chartArea4.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea4.Name = "IAArea";
            chartArea4.Visible = false;
            this.CurChart.ChartAreas.Add(chartArea3);
            this.CurChart.ChartAreas.Add(chartArea4);
            this.CurChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.DockedToChartArea = "IMArea";
            legend3.Name = "IMLegend";
            legend4.DockedToChartArea = "IAArea";
            legend4.Name = "IALegend";
            this.CurChart.Legends.Add(legend3);
            this.CurChart.Legends.Add(legend4);
            this.CurChart.Location = new System.Drawing.Point(471, 293);
            this.CurChart.Name = "CurChart";
            this.CurChart.Size = new System.Drawing.Size(463, 284);
            this.CurChart.TabIndex = 2;
            this.CurChart.Text = "IChart";
            // 
            // LineChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LineChart";
            this.Size = new System.Drawing.Size(937, 580);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataVisualization.Charting.Chart VolChart;
        private System.Windows.Forms.DataGridViewTextBoxColumn SignaName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsCheckedColumn;
        private System.Windows.Forms.DataVisualization.Charting.Chart CurChart;
    }
}
