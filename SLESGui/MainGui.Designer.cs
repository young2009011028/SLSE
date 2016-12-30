namespace SLESGui
{
    partial class MainGui
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pbCalculate = new System.Windows.Forms.ProgressBar();
            this.dgLog = new System.Windows.Forms.DataGridView();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxInputFileName = new System.Windows.Forms.TextBox();
            this.btnBrowseInputModel = new System.Windows.Forms.Button();
            this.btnBrowseInputFile = new System.Windows.Forms.Button();
            this.tbxInputModelName = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.defaulttab = new System.Windows.Forms.TabPage();
            this.tabresult = new System.Windows.Forms.TabControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddChart = new System.Windows.Forms.Button();
            this.btnSaveAsCSV = new System.Windows.Forms.Button();
            this.resultchart = new SLESGui.chart.LineChart();
            this.lineChart1 = new SLESGui.chart.LineChart();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLog)).BeginInit();
            this.tbMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.defaulttab.SuspendLayout();
            this.tabresult.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.menuMain, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pbCalculate, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.dgLog, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbMain, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(977, 553);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // menuMain
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.menuMain, 2);
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(977, 24);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // pbCalculate
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pbCalculate, 2);
            this.pbCalculate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbCalculate.Location = new System.Drawing.Point(3, 366);
            this.pbCalculate.Name = "pbCalculate";
            this.pbCalculate.Size = new System.Drawing.Size(971, 14);
            this.pbCalculate.TabIndex = 1;
            // 
            // dgLog
            // 
            this.dgLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time,
            this.Type,
            this.Message});
            this.tableLayoutPanel1.SetColumnSpan(this.dgLog, 2);
            this.dgLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgLog.Location = new System.Drawing.Point(3, 386);
            this.dgLog.Name = "dgLog";
            this.dgLog.Size = new System.Drawing.Size(971, 164);
            this.dgLog.TabIndex = 2;
            // 
            // Time
            // 
            this.Time.DataPropertyName = "Time";
            this.Time.HeaderText = "错误时间";
            this.Time.Name = "Time";
            // 
            // Type
            // 
            this.Type.DataPropertyName = "Type";
            this.Type.HeaderText = "错误类型";
            this.Type.Name = "Type";
            // 
            // Message
            // 
            this.Message.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Message.DataPropertyName = "Message";
            this.Message.HeaderText = "错误信息";
            this.Message.Name = "Message";
            // 
            // tbMain
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbMain, 2);
            this.tbMain.Controls.Add(this.tabPage1);
            this.tbMain.Controls.Add(this.tabPage2);
            this.tbMain.Controls.Add(this.tabPage3);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMain.Location = new System.Drawing.Point(3, 28);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(971, 332);
            this.tbMain.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(963, 306);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "输入";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.54933F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.39733F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.05334F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tbxInputFileName, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseInputModel, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseInputFile, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.tbxInputModelName, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnStart, 1, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.36364F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.77273F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.77273F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.090909F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(957, 300);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(190, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(457, 34);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入控制";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(3, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 119);
            this.label2.TabIndex = 1;
            this.label2.Text = "输入模型";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(3, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 119);
            this.label3.TabIndex = 2;
            this.label3.Text = "输入数据";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbxInputFileName
            // 
            this.tbxInputFileName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxInputFileName.Enabled = false;
            this.tbxInputFileName.Location = new System.Drawing.Point(268, 202);
            this.tbxInputFileName.Name = "tbxInputFileName";
            this.tbxInputFileName.Size = new System.Drawing.Size(300, 20);
            this.tbxInputFileName.TabIndex = 4;
            // 
            // btnBrowseInputModel
            // 
            this.btnBrowseInputModel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowseInputModel.Location = new System.Drawing.Point(766, 82);
            this.btnBrowseInputModel.Name = "btnBrowseInputModel";
            this.btnBrowseInputModel.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseInputModel.TabIndex = 5;
            this.btnBrowseInputModel.Text = "浏览文件";
            this.btnBrowseInputModel.UseVisualStyleBackColor = true;
            this.btnBrowseInputModel.Click += new System.EventHandler(this.btnBrowseInputModel_Click);
            // 
            // btnBrowseInputFile
            // 
            this.btnBrowseInputFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowseInputFile.Location = new System.Drawing.Point(766, 201);
            this.btnBrowseInputFile.Name = "btnBrowseInputFile";
            this.btnBrowseInputFile.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseInputFile.TabIndex = 6;
            this.btnBrowseInputFile.Text = "浏览文件";
            this.btnBrowseInputFile.UseVisualStyleBackColor = true;
            this.btnBrowseInputFile.Click += new System.EventHandler(this.btnBrowseInputFile_Click);
            // 
            // tbxInputModelName
            // 
            this.tbxInputModelName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxInputModelName.Enabled = false;
            this.tbxInputModelName.Location = new System.Drawing.Point(268, 83);
            this.tbxInputModelName.Name = "tbxInputModelName";
            this.tbxInputModelName.Size = new System.Drawing.Size(300, 20);
            this.tbxInputModelName.TabIndex = 3;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnStart.Location = new System.Drawing.Point(381, 275);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 22);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "开始运算";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(963, 306);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "实时监控和警报";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(963, 306);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "结果";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.tabresult, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(963, 306);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // defaulttab
            // 
            this.defaulttab.Controls.Add(this.resultchart);
            this.defaulttab.Location = new System.Drawing.Point(4, 22);
            this.defaulttab.Name = "defaulttab";
            this.defaulttab.Padding = new System.Windows.Forms.Padding(3);
            this.defaulttab.Size = new System.Drawing.Size(949, 239);
            this.defaulttab.TabIndex = 0;
            this.defaulttab.Text = "图表";
            this.defaulttab.UseVisualStyleBackColor = true;
            // 
            // tabresult
            // 
            this.tabresult.Controls.Add(this.defaulttab);
            this.tabresult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabresult.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabresult.Location = new System.Drawing.Point(3, 38);
            this.tabresult.Name = "tabresult";
            this.tabresult.Padding = new System.Drawing.Point(21, 3);
            this.tabresult.SelectedIndex = 0;
            this.tabresult.Size = new System.Drawing.Size(957, 265);
            this.tabresult.TabIndex = 0;
            this.tabresult.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabresult_DrawItem);
            this.tabresult.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabresult_MouseDown);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnAddChart);
            this.flowLayoutPanel1.Controls.Add(this.btnSaveAsCSV);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(193, 29);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btnAddChart
            // 
            this.btnAddChart.Location = new System.Drawing.Point(3, 3);
            this.btnAddChart.Name = "btnAddChart";
            this.btnAddChart.Size = new System.Drawing.Size(75, 23);
            this.btnAddChart.TabIndex = 0;
            this.btnAddChart.Text = "新增图表";
            this.btnAddChart.UseVisualStyleBackColor = true;
            this.btnAddChart.Click += new System.EventHandler(this.btnAddChart_Click);
            // 
            // btnSaveAsCSV
            // 
            this.btnSaveAsCSV.Location = new System.Drawing.Point(84, 3);
            this.btnSaveAsCSV.Name = "btnSaveAsCSV";
            this.btnSaveAsCSV.Size = new System.Drawing.Size(75, 23);
            this.btnSaveAsCSV.TabIndex = 1;
            this.btnSaveAsCSV.Text = "导出csv文件";
            this.btnSaveAsCSV.UseVisualStyleBackColor = true;
            this.btnSaveAsCSV.Click += new System.EventHandler(this.btnSaveAsCSV_Click);
            // 
            // resultchart
            // 
            this.resultchart.Data_Handler = null;
            this.resultchart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultchart.Location = new System.Drawing.Point(3, 3);
            this.resultchart.Name = "resultchart";
            this.resultchart.Size = new System.Drawing.Size(943, 233);
            this.resultchart.TabIndex = 0;
            // 
            // lineChart1
            // 
            this.lineChart1.Data_Handler = null;
            this.lineChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineChart1.Location = new System.Drawing.Point(0, 0);
            this.lineChart1.Name = "lineChart1";
            this.lineChart1.Size = new System.Drawing.Size(963, 306);
            this.lineChart1.TabIndex = 0;
            // 
            // MainGui
            // 
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(977, 553);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MainMenuStrip = this.menuMain;
            this.Name = "MainGui";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLog)).EndInit();
            this.tbMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.defaulttab.ResumeLayout(false);
            this.tabresult.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ProgressBar pbCalculate;
        private System.Windows.Forms.DataGridView dgLog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxInputModelName;
        private System.Windows.Forms.TextBox tbxInputFileName;
        private System.Windows.Forms.Button btnBrowseInputModel;
        private System.Windows.Forms.Button btnBrowseInputFile;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
        private chart.LineChart lineChart1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TabControl tabresult;
        private System.Windows.Forms.TabPage defaulttab;
        private chart.LineChart resultchart;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnAddChart;
        private System.Windows.Forms.Button btnSaveAsCSV;


    }
}

