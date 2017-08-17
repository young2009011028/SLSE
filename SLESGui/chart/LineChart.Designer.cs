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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.VolChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.CurChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SignaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsCheckedColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbRef = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VolChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurChart)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.84312F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.15688F));
            this.tableLayoutPanel1.Controls.Add(this.VolChart, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.CurChart, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(937, 580);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // VolChart
            // 
            chartArea5.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea5.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea5.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea5.Name = "VMArea";
            chartArea5.Visible = false;
            chartArea6.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea6.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea6.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea6.Name = "VAArea";
            chartArea6.Visible = false;
            this.VolChart.ChartAreas.Add(chartArea5);
            this.VolChart.ChartAreas.Add(chartArea6);
            this.VolChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend5.BackColor = System.Drawing.Color.Transparent;
            legend5.DockedToChartArea = "VMArea";
            legend5.Name = "VMLegend";
            legend5.TitleBackColor = System.Drawing.Color.Transparent;
            legend6.BackColor = System.Drawing.Color.Transparent;
            legend6.DockedToChartArea = "VAArea";
            legend6.Name = "VALegend";
            legend6.TitleBackColor = System.Drawing.Color.Transparent;
            this.VolChart.Legends.Add(legend5);
            this.VolChart.Legends.Add(legend6);
            this.VolChart.Location = new System.Drawing.Point(292, 3);
            this.VolChart.Name = "VolChart";
            this.VolChart.Size = new System.Drawing.Size(642, 284);
            this.VolChart.TabIndex = 1;
            this.VolChart.Text = "VolChart";
            this.VolChart.Click += new System.EventHandler(this.multiplelinechart_Click);
            // 
            // CurChart
            // 
            chartArea7.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea7.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea7.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea7.Name = "IMArea";
            chartArea7.Visible = false;
            chartArea8.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea8.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea8.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea8.Name = "IAArea";
            chartArea8.Visible = false;
            this.CurChart.ChartAreas.Add(chartArea7);
            this.CurChart.ChartAreas.Add(chartArea8);
            this.CurChart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend7.DockedToChartArea = "IMArea";
            legend7.Name = "IMLegend";
            legend8.DockedToChartArea = "IAArea";
            legend8.Name = "IALegend";
            this.CurChart.Legends.Add(legend7);
            this.CurChart.Legends.Add(legend8);
            this.CurChart.Location = new System.Drawing.Point(292, 293);
            this.CurChart.Name = "CurChart";
            this.CurChart.Size = new System.Drawing.Size(642, 284);
            this.CurChart.TabIndex = 2;
            this.CurChart.Text = "IChart";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel1.SetRowSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(283, 574);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SignaName,
            this.IsCheckedColumn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 33);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(277, 538);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.cbRef);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(277, 24);
            this.panel1.TabIndex = 1;
            // 
            // cbRef
            // 
            this.cbRef.FormattingEnabled = true;
            this.cbRef.Location = new System.Drawing.Point(110, 2);
            this.cbRef.Name = "cbRef";
            this.cbRef.Size = new System.Drawing.Size(149, 21);
            this.cbRef.TabIndex = 1;
            this.cbRef.SelectionChangeCommitted += new System.EventHandler(this.cbRef_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "相位角参考";
            // 
            // LineChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LineChart";
            this.Size = new System.Drawing.Size(937, 580);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VolChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurChart)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataVisualization.Charting.Chart VolChart;
        private System.Windows.Forms.DataGridViewTextBoxColumn SignaName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsCheckedColumn;
        private System.Windows.Forms.DataVisualization.Charting.Chart CurChart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbRef;
    }
}
