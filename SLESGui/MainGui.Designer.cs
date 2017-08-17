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
            this.tabinput = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowseInputModel = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.tbxAlarm = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxsamplecount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxInputModelName = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbxInputScadaDataName = new System.Windows.Forms.TextBox();
            this.tbxInputFileName = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnBrowseScadaFile = new System.Windows.Forms.Button();
            this.btnBrowseInputFile = new System.Windows.Forms.Button();
            this.dgSignals = new System.Windows.Forms.DataGridView();
            this.SignalName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabrealtime = new System.Windows.Forms.TabPage();
            this.dgAlarm = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabpageresult = new System.Windows.Forms.TabPage();
            this.tableLayoutresult = new System.Windows.Forms.TableLayoutPanel();
            this.tabresult = new System.Windows.Forms.TabControl();
            this.defaulttab = new System.Windows.Forms.TabPage();
            this.resultchart = new SLESGui.chart.LineChart();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddChart = new System.Windows.Forms.Button();
            this.btnSaveAsCSV = new System.Windows.Forms.Button();
            this.lineChart1 = new SLESGui.chart.LineChart();
            this.new_proj_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.menuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLog)).BeginInit();
            this.tbMain.SuspendLayout();
            this.tabinput.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSignals)).BeginInit();
            this.tabrealtime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAlarm)).BeginInit();
            this.tabpageresult.SuspendLayout();
            this.tableLayoutresult.SuspendLayout();
            this.tabresult.SuspendLayout();
            this.defaulttab.SuspendLayout();
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
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76.92307F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.07693F));
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
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.new_proj_menu});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // pbCalculate
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.pbCalculate, 2);
            this.pbCalculate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbCalculate.Location = new System.Drawing.Point(3, 418);
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
            this.dgLog.Location = new System.Drawing.Point(3, 438);
            this.dgLog.Name = "dgLog";
            this.dgLog.Size = new System.Drawing.Size(971, 112);
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
            this.tbMain.Controls.Add(this.tabinput);
            this.tbMain.Controls.Add(this.tabrealtime);
            this.tbMain.Controls.Add(this.tabpageresult);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMain.Location = new System.Drawing.Point(3, 28);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(971, 384);
            this.tbMain.TabIndex = 4;
            // 
            // tabinput
            // 
            this.tabinput.Controls.Add(this.tableLayoutPanel4);
            this.tabinput.Location = new System.Drawing.Point(4, 22);
            this.tabinput.Name = "tabinput";
            this.tabinput.Padding = new System.Windows.Forms.Padding(3);
            this.tabinput.Size = new System.Drawing.Size(963, 358);
            this.tabinput.TabIndex = 0;
            this.tabinput.Text = "输入";
            this.tabinput.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.dgSignals, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(957, 352);
            this.tableLayoutPanel4.TabIndex = 4;
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
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseInputModel, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnStart, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.tbxInputModelName, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 2, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.36364F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.77273F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.78912F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.53061F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(472, 346);
            this.tableLayoutPanel2.TabIndex = 3;
            this.tableLayoutPanel2.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel2_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(95, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入控制";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(3, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 136);
            this.label2.TabIndex = 1;
            this.label2.Text = "输入模型";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(3, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 78);
            this.label3.TabIndex = 2;
            this.label3.Text = "输入数据";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBrowseInputModel
            // 
            this.btnBrowseInputModel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowseInputModel.Location = new System.Drawing.Point(358, 95);
            this.btnBrowseInputModel.Name = "btnBrowseInputModel";
            this.btnBrowseInputModel.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseInputModel.TabIndex = 5;
            this.btnBrowseInputModel.Text = "浏览文件";
            this.btnBrowseInputModel.UseVisualStyleBackColor = true;
            this.btnBrowseInputModel.Click += new System.EventHandler(this.btnBrowseInputModel_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnStart.Location = new System.Drawing.Point(358, 288);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 22);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "开始运算";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.tbxAlarm);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.tbxsamplecount);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(95, 256);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(222, 87);
            this.panel1.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(175, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "帧";
            // 
            // tbxAlarm
            // 
            this.tbxAlarm.Location = new System.Drawing.Point(142, 40);
            this.tbxAlarm.Name = "tbxAlarm";
            this.tbxAlarm.Size = new System.Drawing.Size(26, 20);
            this.tbxAlarm.TabIndex = 3;
            this.tbxAlarm.Text = "5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "警报持续时间";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // tbxsamplecount
            // 
            this.tbxsamplecount.Location = new System.Drawing.Point(142, 11);
            this.tbxsamplecount.Name = "tbxsamplecount";
            this.tbxsamplecount.Size = new System.Drawing.Size(26, 20);
            this.tbxsamplecount.TabIndex = 1;
            this.tbxsamplecount.Text = "30";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "每秒钟数据量";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // tbxInputModelName
            // 
            this.tbxInputModelName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbxInputModelName.Enabled = false;
            this.tbxInputModelName.Location = new System.Drawing.Point(95, 97);
            this.tbxInputModelName.Name = "tbxInputModelName";
            this.tbxInputModelName.Size = new System.Drawing.Size(222, 20);
            this.tbxInputModelName.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.tbxInputScadaDataName);
            this.panel2.Controls.Add(this.tbxInputFileName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(95, 178);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(222, 72);
            this.panel2.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "scada数据";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "pmu数据";
            // 
            // tbxInputScadaDataName
            // 
            this.tbxInputScadaDataName.Enabled = false;
            this.tbxInputScadaDataName.Location = new System.Drawing.Point(98, 37);
            this.tbxInputScadaDataName.Name = "tbxInputScadaDataName";
            this.tbxInputScadaDataName.Size = new System.Drawing.Size(121, 20);
            this.tbxInputScadaDataName.TabIndex = 5;
            // 
            // tbxInputFileName
            // 
            this.tbxInputFileName.Enabled = false;
            this.tbxInputFileName.Location = new System.Drawing.Point(98, 0);
            this.tbxInputFileName.Name = "tbxInputFileName";
            this.tbxInputFileName.Size = new System.Drawing.Size(121, 20);
            this.tbxInputFileName.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnBrowseScadaFile);
            this.panel3.Controls.Add(this.btnBrowseInputFile);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(323, 178);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(146, 72);
            this.panel3.TabIndex = 10;
            // 
            // btnBrowseScadaFile
            // 
            this.btnBrowseScadaFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowseScadaFile.Location = new System.Drawing.Point(35, 40);
            this.btnBrowseScadaFile.Name = "btnBrowseScadaFile";
            this.btnBrowseScadaFile.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseScadaFile.TabIndex = 7;
            this.btnBrowseScadaFile.Text = "浏览文件";
            this.btnBrowseScadaFile.UseVisualStyleBackColor = true;
            this.btnBrowseScadaFile.Click += new System.EventHandler(this.btnBrowseScadaFile_Click);
            // 
            // btnBrowseInputFile
            // 
            this.btnBrowseInputFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowseInputFile.Location = new System.Drawing.Point(35, 4);
            this.btnBrowseInputFile.Name = "btnBrowseInputFile";
            this.btnBrowseInputFile.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseInputFile.TabIndex = 6;
            this.btnBrowseInputFile.Text = "浏览文件";
            this.btnBrowseInputFile.UseVisualStyleBackColor = true;
            this.btnBrowseInputFile.Click += new System.EventHandler(this.btnBrowseInputFile_Click);
            // 
            // dgSignals
            // 
            this.dgSignals.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgSignals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSignals.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SignalName});
            this.dgSignals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSignals.Location = new System.Drawing.Point(481, 3);
            this.dgSignals.Name = "dgSignals";
            this.dgSignals.Size = new System.Drawing.Size(473, 346);
            this.dgSignals.TabIndex = 4;
            // 
            // SignalName
            // 
            this.SignalName.DataPropertyName = "SignalName";
            this.SignalName.HeaderText = "信号";
            this.SignalName.Name = "SignalName";
            // 
            // tabrealtime
            // 
            this.tabrealtime.Controls.Add(this.dgAlarm);
            this.tabrealtime.Location = new System.Drawing.Point(4, 22);
            this.tabrealtime.Name = "tabrealtime";
            this.tabrealtime.Padding = new System.Windows.Forms.Padding(3);
            this.tabrealtime.Size = new System.Drawing.Size(963, 358);
            this.tabrealtime.TabIndex = 1;
            this.tabrealtime.Text = "实时监控和警报";
            this.tabrealtime.UseVisualStyleBackColor = true;
            // 
            // dgAlarm
            // 
            this.dgAlarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAlarm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.Column1,
            this.dataGridViewTextBoxColumn3});
            this.dgAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgAlarm.Location = new System.Drawing.Point(3, 3);
            this.dgAlarm.Name = "dgAlarm";
            this.dgAlarm.Size = new System.Drawing.Size(957, 352);
            this.dgAlarm.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Time";
            this.dataGridViewTextBoxColumn1.HeaderText = "警报时间";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "Type";
            this.Column1.HeaderText = "警报类型";
            this.Column1.Name = "Column1";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Message";
            this.dataGridViewTextBoxColumn3.HeaderText = "警报信息";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // tabpageresult
            // 
            this.tabpageresult.Controls.Add(this.tableLayoutresult);
            this.tabpageresult.Location = new System.Drawing.Point(4, 22);
            this.tabpageresult.Name = "tabpageresult";
            this.tabpageresult.Size = new System.Drawing.Size(963, 358);
            this.tabpageresult.TabIndex = 2;
            this.tabpageresult.Text = "结果";
            this.tabpageresult.UseVisualStyleBackColor = true;
            // 
            // tableLayoutresult
            // 
            this.tableLayoutresult.ColumnCount = 1;
            this.tableLayoutresult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutresult.Controls.Add(this.tabresult, 0, 1);
            this.tableLayoutresult.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutresult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutresult.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutresult.Name = "tableLayoutresult";
            this.tableLayoutresult.RowCount = 2;
            this.tableLayoutresult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutresult.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutresult.Size = new System.Drawing.Size(963, 358);
            this.tableLayoutresult.TabIndex = 1;
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
            this.tabresult.Size = new System.Drawing.Size(957, 317);
            this.tabresult.TabIndex = 0;
            this.tabresult.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabresult_DrawItem);
            this.tabresult.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabresult_MouseDown);
            // 
            // defaulttab
            // 
            this.defaulttab.Controls.Add(this.resultchart);
            this.defaulttab.Location = new System.Drawing.Point(4, 22);
            this.defaulttab.Name = "defaulttab";
            this.defaulttab.Padding = new System.Windows.Forms.Padding(3);
            this.defaulttab.Size = new System.Drawing.Size(949, 291);
            this.defaulttab.TabIndex = 0;
            this.defaulttab.Text = "图表";
            this.defaulttab.UseVisualStyleBackColor = true;
            // 
            // resultchart
            // 
            this.resultchart.Data_Handler = null;
            this.resultchart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultchart.Location = new System.Drawing.Point(3, 3);
            this.resultchart.Name = "resultchart";
            this.resultchart.Ref = null;
            this.resultchart.Size = new System.Drawing.Size(943, 285);
            this.resultchart.TabIndex = 0;
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
            // lineChart1
            // 
            this.lineChart1.Data_Handler = null;
            this.lineChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineChart1.Location = new System.Drawing.Point(0, 0);
            this.lineChart1.Name = "lineChart1";
            this.lineChart1.Ref = null;
            this.lineChart1.Size = new System.Drawing.Size(963, 306);
            this.lineChart1.TabIndex = 0;
            // 
            // new_proj_menu
            // 
            this.new_proj_menu.Name = "new_proj_menu";
            this.new_proj_menu.Size = new System.Drawing.Size(152, 22);
            this.new_proj_menu.Text = "新建项目";
            this.new_proj_menu.Click += new System.EventHandler(this.new_proj_menu_Click);
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
            this.tabinput.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgSignals)).EndInit();
            this.tabrealtime.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgAlarm)).EndInit();
            this.tabpageresult.ResumeLayout(false);
            this.tableLayoutresult.ResumeLayout(false);
            this.tabresult.ResumeLayout(false);
            this.defaulttab.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnBrowseInputModel;
        private System.Windows.Forms.Button btnBrowseInputFile;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tabinput;
        private System.Windows.Forms.TabPage tabrealtime;
        private chart.LineChart lineChart1;
        private System.Windows.Forms.TabPage tabpageresult;
        private System.Windows.Forms.TableLayoutPanel tableLayoutresult;
        private System.Windows.Forms.TabControl tabresult;
        private System.Windows.Forms.TabPage defaulttab;
        private chart.LineChart resultchart;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnAddChart;
        private System.Windows.Forms.Button btnSaveAsCSV;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.DataGridView dgSignals;
        private System.Windows.Forms.DataGridViewTextBoxColumn SignalName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxsamplecount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
        private System.Windows.Forms.DataGridView dgAlarm;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbxAlarm;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbxInputScadaDataName;
        private System.Windows.Forms.TextBox tbxInputFileName;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnBrowseScadaFile;
        private System.Windows.Forms.ToolStripMenuItem new_proj_menu;
    }
}

