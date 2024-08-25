<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.TimerCheckCycle = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.TimerRoof = New System.Windows.Forms.Timer(Me.components)
        Me.TimerCover = New System.Windows.Forms.Timer(Me.components)
        Me.TimerSwitch = New System.Windows.Forms.Timer(Me.components)
        Me.TimerWeather = New System.Windows.Forms.Timer(Me.components)
        Me.TimerStartRun = New System.Windows.Forms.Timer(Me.components)
        Me.TimerMount = New System.Windows.Forms.Timer(Me.components)
        Me.TimerCCD = New System.Windows.Forms.Timer(Me.components)
        Me.TimerDisaster = New System.Windows.Forms.Timer(Me.components)
        Me.TimerColorStatus = New System.Windows.Forms.Timer(Me.components)
        Me.TimerSmartError = New System.Windows.Forms.Timer(Me.components)
        Me.TimerHeartBeat = New System.Windows.Forms.Timer(Me.components)
        Me.TimerHang = New System.Windows.Forms.Timer(Me.components)
        Me.SplitContainer = New System.Windows.Forms.SplitContainer()
        Me.BtnExit = New System.Windows.Forms.Button()
        Me.BtnAbout = New System.Windows.Forms.Button()
        Me.BtnDebug = New System.Windows.Forms.Button()
        Me.BtnViewLog = New System.Windows.Forms.Button()
        Me.BtnClearErrorLog = New System.Windows.Forms.Button()
        Me.BtnStartSentinel = New System.Windows.Forms.Button()
        Me.BtnClearLog = New System.Windows.Forms.Button()
        Me.BtnStopSound = New System.Windows.Forms.Button()
        Me.BtnCalibration = New System.Windows.Forms.Button()
        Me.BtnProperties = New System.Windows.Forms.Button()
        Me.BtnTools = New System.Windows.Forms.Button()
        Me.BtnHADS = New System.Windows.Forms.Button()
        Me.BtnDeepsky = New System.Windows.Forms.Button()
        Me.BtnStopRun = New System.Windows.Forms.Button()
        Me.BtnStartRun = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnMenu = New System.Windows.Forms.Button()
        Me.PictureBox = New System.Windows.Forms.PictureBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LblCloudSafe = New System.Windows.Forms.Label()
        Me.txtRelativeHumidity = New System.Windows.Forms.TextBox()
        Me.lblRain = New System.Windows.Forms.Label()
        Me.lblLight = New System.Windows.Forms.Label()
        Me.lblCloud = New System.Windows.Forms.Label()
        Me.txtAmbientTemperature = New System.Windows.Forms.TextBox()
        Me.lblLastRead = New System.Windows.Forms.Label()
        Me.lblRelativeHumidity = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblSafe = New System.Windows.Forms.Label()
        Me.PanelTop = New System.Windows.Forms.Panel()
        Me.LblHang = New System.Windows.Forms.Label()
        Me.LblVersion = New System.Windows.Forms.Label()
        Me.LblRoof = New System.Windows.Forms.Label()
        Me.LblSwitch = New System.Windows.Forms.Label()
        Me.LblCover = New System.Windows.Forms.Label()
        Me.LblTSX = New System.Windows.Forms.Label()
        Me.LblInternet = New System.Windows.Forms.Label()
        Me.LblUPSCyberPower = New System.Windows.Forms.Label()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GroupBoxTwilight = New System.Windows.Forms.GroupBox()
        Me.LblCivilTwilight = New System.Windows.Forms.Label()
        Me.LblAstronomicalTwilight = New System.Windows.Forms.Label()
        Me.LblAmateurTwilight = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.LblSunDawnFlats = New System.Windows.Forms.Label()
        Me.LblSunDuskFlats = New System.Windows.Forms.Label()
        Me.LblSunStopRun = New System.Windows.Forms.Label()
        Me.LblSunStartRun = New System.Windows.Forms.Label()
        Me.LblSunOpenRoof = New System.Windows.Forms.Label()
        Me.LblLST = New System.Windows.Forms.Label()
        Me.LblGST = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.LblCCDFilter = New System.Windows.Forms.Label()
        Me.LblCCDTemp = New System.Windows.Forms.Label()
        Me.LblCCDExposureStatus = New System.Windows.Forms.Label()
        Me.ChkDisableSafetyCheck = New System.Windows.Forms.CheckBox()
        Me.GrpFocus = New System.Windows.Forms.GroupBox()
        Me.LblLastFocusDateTime = New System.Windows.Forms.Label()
        Me.LblLastFocusTemperature = New System.Windows.Forms.Label()
        Me.LblFocusserTemperature = New System.Windows.Forms.Label()
        Me.LblFocusserPosition = New System.Windows.Forms.Label()
        Me.GroupBox = New System.Windows.Forms.GroupBox()
        Me.grpMoon = New System.Windows.Forms.GroupBox()
        Me.LblMoonSettingRising = New System.Windows.Forms.Label()
        Me.LblDECMoon = New System.Windows.Forms.Label()
        Me.LblPhaseMoon = New System.Windows.Forms.Label()
        Me.LblRisingSettingMoon = New System.Windows.Forms.Label()
        Me.LblAltMoon = New System.Windows.Forms.Label()
        Me.LblRAMoon = New System.Windows.Forms.Label()
        Me.grpSun = New System.Windows.Forms.GroupBox()
        Me.LblSunSettingRising = New System.Windows.Forms.Label()
        Me.LblDECSun = New System.Windows.Forms.Label()
        Me.LblRisingSettingSun = New System.Windows.Forms.Label()
        Me.LblRASun = New System.Windows.Forms.Label()
        Me.LblAltSun = New System.Windows.Forms.Label()
        Me.ChkSimulatorMode = New System.Windows.Forms.CheckBox()
        Me.ChkAutoStart = New System.Windows.Forms.CheckBox()
        Me.RTXLog = New System.Windows.Forms.RichTextBox()
        Me.LblMonitorStatus = New System.Windows.Forms.Label()
        Me.grpMount = New System.Windows.Forms.GroupBox()
        Me.LblMountPierSide = New System.Windows.Forms.Label()
        Me.LblMountRADEC = New System.Windows.Forms.Label()
        Me.LblMountStatus = New System.Windows.Forms.Label()
        Me.lblMountAz = New System.Windows.Forms.Label()
        Me.lblMountAlt = New System.Windows.Forms.Label()
        Me.RTXErrors = New System.Windows.Forms.RichTextBox()
        Me.TimerSplit1 = New System.Windows.Forms.Timer(Me.components)
        Me.TimerSplit2 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer.Panel1.SuspendLayout()
        Me.SplitContainer.Panel2.SuspendLayout()
        Me.SplitContainer.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.PanelTop.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBoxTwilight.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GrpFocus.SuspendLayout()
        Me.GroupBox.SuspendLayout()
        Me.grpMoon.SuspendLayout()
        Me.grpSun.SuspendLayout()
        Me.grpMount.SuspendLayout()
        Me.SuspendLayout()
        '
        'TimerCheckCycle
        '
        Me.TimerCheckCycle.Interval = 10000
        '
        'TimerRoof
        '
        Me.TimerRoof.Interval = 2300
        '
        'TimerCover
        '
        Me.TimerCover.Interval = 2200
        '
        'TimerSwitch
        '
        Me.TimerSwitch.Interval = 2100
        '
        'TimerWeather
        '
        Me.TimerWeather.Interval = 2900
        '
        'TimerStartRun
        '
        Me.TimerStartRun.Interval = 5000
        '
        'TimerMount
        '
        Me.TimerMount.Interval = 2700
        '
        'TimerCCD
        '
        Me.TimerCCD.Interval = 2800
        '
        'TimerDisaster
        '
        Me.TimerDisaster.Enabled = True
        Me.TimerDisaster.Interval = 120000
        '
        'TimerColorStatus
        '
        Me.TimerColorStatus.Enabled = True
        Me.TimerColorStatus.Interval = 500
        '
        'TimerSmartError
        '
        Me.TimerSmartError.Enabled = True
        Me.TimerSmartError.Interval = 30000
        '
        'TimerHeartBeat
        '
        Me.TimerHeartBeat.Enabled = True
        Me.TimerHeartBeat.Interval = 5000
        '
        'TimerHang
        '
        Me.TimerHang.Enabled = True
        Me.TimerHang.Interval = 250
        '
        'SplitContainer
        '
        Me.SplitContainer.BackColor = System.Drawing.Color.FromArgb(CType(CType(140, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.SplitContainer.Dock = System.Windows.Forms.DockStyle.Left
        Me.SplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer.Name = "SplitContainer"
        '
        'SplitContainer.Panel1
        '
        Me.SplitContainer.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(184, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnExit)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnAbout)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnDebug)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnViewLog)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnClearErrorLog)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnStartSentinel)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnClearLog)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnStopSound)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnCalibration)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnProperties)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnTools)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnHADS)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnDeepsky)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnStopRun)
        Me.SplitContainer.Panel1.Controls.Add(Me.BtnStartRun)
        Me.SplitContainer.Panel1.Controls.Add(Me.Panel1)
        Me.SplitContainer.Panel1MinSize = 40
        '
        'SplitContainer.Panel2
        '
        Me.SplitContainer.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(99, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(114, Byte), Integer))
        Me.SplitContainer.Panel2.Controls.Add(Me.Panel2)
        Me.SplitContainer.Panel2.Controls.Add(Me.PanelTop)
        Me.SplitContainer.Panel2.Controls.Add(Me.StatusStrip)
        Me.SplitContainer.Panel2.Controls.Add(Me.GroupBox3)
        Me.SplitContainer.Panel2.Controls.Add(Me.GroupBox1)
        Me.SplitContainer.Panel2.Controls.Add(Me.ChkDisableSafetyCheck)
        Me.SplitContainer.Panel2.Controls.Add(Me.GrpFocus)
        Me.SplitContainer.Panel2.Controls.Add(Me.GroupBox)
        Me.SplitContainer.Panel2.Controls.Add(Me.ChkSimulatorMode)
        Me.SplitContainer.Panel2.Controls.Add(Me.ChkAutoStart)
        Me.SplitContainer.Panel2.Controls.Add(Me.RTXLog)
        Me.SplitContainer.Panel2.Controls.Add(Me.LblMonitorStatus)
        Me.SplitContainer.Panel2.Controls.Add(Me.grpMount)
        Me.SplitContainer.Panel2.Controls.Add(Me.RTXErrors)
        Me.SplitContainer.Size = New System.Drawing.Size(1873, 987)
        Me.SplitContainer.SplitterDistance = 220
        Me.SplitContainer.SplitterWidth = 1
        Me.SplitContainer.TabIndex = 82
        '
        'BtnExit
        '
        Me.BtnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnExit.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnExit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(143, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.BtnExit.FlatAppearance.BorderSize = 0
        Me.BtnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnExit.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnExit.ForeColor = System.Drawing.Color.White
        Me.BtnExit.Image = CType(resources.GetObject("BtnExit.Image"), System.Drawing.Image)
        Me.BtnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnExit.Location = New System.Drawing.Point(0, 645)
        Me.BtnExit.Name = "BtnExit"
        Me.BtnExit.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.BtnExit.Size = New System.Drawing.Size(220, 40)
        Me.BtnExit.TabIndex = 12
        Me.BtnExit.Text = "     Exit"
        Me.BtnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnExit.UseVisualStyleBackColor = True
        '
        'BtnAbout
        '
        Me.BtnAbout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnAbout.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnAbout.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(143, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.BtnAbout.FlatAppearance.BorderSize = 0
        Me.BtnAbout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnAbout.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.ForeColor = System.Drawing.Color.White
        Me.BtnAbout.Image = CType(resources.GetObject("BtnAbout.Image"), System.Drawing.Image)
        Me.BtnAbout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnAbout.Location = New System.Drawing.Point(0, 605)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.BtnAbout.Size = New System.Drawing.Size(220, 40)
        Me.BtnAbout.TabIndex = 11
        Me.BtnAbout.Text = "     About"
        Me.BtnAbout.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'BtnDebug
        '
        Me.BtnDebug.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnDebug.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnDebug.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(143, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.BtnDebug.FlatAppearance.BorderSize = 0
        Me.BtnDebug.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnDebug.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnDebug.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDebug.ForeColor = System.Drawing.Color.White
        Me.BtnDebug.Image = CType(resources.GetObject("BtnDebug.Image"), System.Drawing.Image)
        Me.BtnDebug.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnDebug.Location = New System.Drawing.Point(0, 565)
        Me.BtnDebug.Name = "BtnDebug"
        Me.BtnDebug.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.BtnDebug.Size = New System.Drawing.Size(220, 40)
        Me.BtnDebug.TabIndex = 10
        Me.BtnDebug.Text = "     Debug"
        Me.BtnDebug.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnDebug.UseVisualStyleBackColor = True
        '
        'BtnViewLog
        '
        Me.BtnViewLog.Location = New System.Drawing.Point(76, 737)
        Me.BtnViewLog.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnViewLog.Name = "BtnViewLog"
        Me.BtnViewLog.Size = New System.Drawing.Size(104, 27)
        Me.BtnViewLog.TabIndex = 102
        Me.BtnViewLog.Text = "View Log"
        Me.BtnViewLog.UseVisualStyleBackColor = True
        '
        'BtnClearErrorLog
        '
        Me.BtnClearErrorLog.Location = New System.Drawing.Point(76, 807)
        Me.BtnClearErrorLog.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnClearErrorLog.Name = "BtnClearErrorLog"
        Me.BtnClearErrorLog.Size = New System.Drawing.Size(104, 27)
        Me.BtnClearErrorLog.TabIndex = 100
        Me.BtnClearErrorLog.Text = "Clear Error Log"
        Me.BtnClearErrorLog.UseVisualStyleBackColor = True
        '
        'BtnStartSentinel
        '
        Me.BtnStartSentinel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnStartSentinel.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnStartSentinel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(143, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.BtnStartSentinel.FlatAppearance.BorderSize = 0
        Me.BtnStartSentinel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnStartSentinel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnStartSentinel.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStartSentinel.ForeColor = System.Drawing.Color.White
        Me.BtnStartSentinel.Image = CType(resources.GetObject("BtnStartSentinel.Image"), System.Drawing.Image)
        Me.BtnStartSentinel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnStartSentinel.Location = New System.Drawing.Point(0, 525)
        Me.BtnStartSentinel.Name = "BtnStartSentinel"
        Me.BtnStartSentinel.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.BtnStartSentinel.Size = New System.Drawing.Size(220, 40)
        Me.BtnStartSentinel.TabIndex = 9
        Me.BtnStartSentinel.Text = "     Start sentinel"
        Me.BtnStartSentinel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnStartSentinel.UseVisualStyleBackColor = True
        '
        'BtnClearLog
        '
        Me.BtnClearLog.Location = New System.Drawing.Point(76, 772)
        Me.BtnClearLog.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnClearLog.Name = "BtnClearLog"
        Me.BtnClearLog.Size = New System.Drawing.Size(104, 27)
        Me.BtnClearLog.TabIndex = 99
        Me.BtnClearLog.Text = "Clear Log"
        Me.BtnClearLog.UseVisualStyleBackColor = True
        '
        'BtnStopSound
        '
        Me.BtnStopSound.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnStopSound.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnStopSound.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(143, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.BtnStopSound.FlatAppearance.BorderSize = 0
        Me.BtnStopSound.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnStopSound.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnStopSound.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStopSound.ForeColor = System.Drawing.Color.White
        Me.BtnStopSound.Image = CType(resources.GetObject("BtnStopSound.Image"), System.Drawing.Image)
        Me.BtnStopSound.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnStopSound.Location = New System.Drawing.Point(0, 485)
        Me.BtnStopSound.Name = "BtnStopSound"
        Me.BtnStopSound.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.BtnStopSound.Size = New System.Drawing.Size(220, 40)
        Me.BtnStopSound.TabIndex = 8
        Me.BtnStopSound.Text = "     Stop sound"
        Me.BtnStopSound.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnStopSound.UseVisualStyleBackColor = True
        '
        'BtnCalibration
        '
        Me.BtnCalibration.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnCalibration.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnCalibration.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(143, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.BtnCalibration.FlatAppearance.BorderSize = 0
        Me.BtnCalibration.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnCalibration.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCalibration.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCalibration.ForeColor = System.Drawing.Color.White
        Me.BtnCalibration.Image = CType(resources.GetObject("BtnCalibration.Image"), System.Drawing.Image)
        Me.BtnCalibration.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnCalibration.Location = New System.Drawing.Point(0, 445)
        Me.BtnCalibration.Name = "BtnCalibration"
        Me.BtnCalibration.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.BtnCalibration.Size = New System.Drawing.Size(220, 40)
        Me.BtnCalibration.TabIndex = 7
        Me.BtnCalibration.Text = "     Calibration"
        Me.BtnCalibration.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnCalibration.UseVisualStyleBackColor = True
        '
        'BtnProperties
        '
        Me.BtnProperties.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnProperties.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnProperties.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(143, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.BtnProperties.FlatAppearance.BorderSize = 0
        Me.BtnProperties.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnProperties.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnProperties.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnProperties.ForeColor = System.Drawing.Color.White
        Me.BtnProperties.Image = CType(resources.GetObject("BtnProperties.Image"), System.Drawing.Image)
        Me.BtnProperties.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnProperties.Location = New System.Drawing.Point(0, 405)
        Me.BtnProperties.Name = "BtnProperties"
        Me.BtnProperties.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.BtnProperties.Size = New System.Drawing.Size(220, 40)
        Me.BtnProperties.TabIndex = 6
        Me.BtnProperties.Text = "     Properties"
        Me.BtnProperties.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnProperties.UseVisualStyleBackColor = True
        '
        'BtnTools
        '
        Me.BtnTools.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnTools.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnTools.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(143, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.BtnTools.FlatAppearance.BorderSize = 0
        Me.BtnTools.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnTools.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTools.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnTools.ForeColor = System.Drawing.Color.White
        Me.BtnTools.Image = CType(resources.GetObject("BtnTools.Image"), System.Drawing.Image)
        Me.BtnTools.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnTools.Location = New System.Drawing.Point(0, 365)
        Me.BtnTools.Name = "BtnTools"
        Me.BtnTools.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.BtnTools.Size = New System.Drawing.Size(220, 40)
        Me.BtnTools.TabIndex = 5
        Me.BtnTools.Text = "     Tools"
        Me.BtnTools.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnTools.UseVisualStyleBackColor = True
        '
        'BtnHADS
        '
        Me.BtnHADS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnHADS.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnHADS.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(143, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.BtnHADS.FlatAppearance.BorderSize = 0
        Me.BtnHADS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnHADS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnHADS.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnHADS.ForeColor = System.Drawing.Color.White
        Me.BtnHADS.Image = CType(resources.GetObject("BtnHADS.Image"), System.Drawing.Image)
        Me.BtnHADS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnHADS.Location = New System.Drawing.Point(0, 325)
        Me.BtnHADS.Name = "BtnHADS"
        Me.BtnHADS.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.BtnHADS.Size = New System.Drawing.Size(220, 40)
        Me.BtnHADS.TabIndex = 4
        Me.BtnHADS.Text = "     HADS targets"
        Me.BtnHADS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnHADS.UseVisualStyleBackColor = True
        '
        'BtnDeepsky
        '
        Me.BtnDeepsky.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnDeepsky.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnDeepsky.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(143, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.BtnDeepsky.FlatAppearance.BorderSize = 0
        Me.BtnDeepsky.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnDeepsky.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnDeepsky.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDeepsky.ForeColor = System.Drawing.Color.White
        Me.BtnDeepsky.Image = CType(resources.GetObject("BtnDeepsky.Image"), System.Drawing.Image)
        Me.BtnDeepsky.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnDeepsky.Location = New System.Drawing.Point(0, 285)
        Me.BtnDeepsky.Name = "BtnDeepsky"
        Me.BtnDeepsky.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.BtnDeepsky.Size = New System.Drawing.Size(220, 40)
        Me.BtnDeepsky.TabIndex = 3
        Me.BtnDeepsky.Text = "     Deepsky targets"
        Me.BtnDeepsky.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnDeepsky.UseVisualStyleBackColor = True
        '
        'BtnStopRun
        '
        Me.BtnStopRun.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnStopRun.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnStopRun.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(143, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.BtnStopRun.FlatAppearance.BorderSize = 0
        Me.BtnStopRun.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnStopRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnStopRun.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStopRun.ForeColor = System.Drawing.Color.White
        Me.BtnStopRun.Image = CType(resources.GetObject("BtnStopRun.Image"), System.Drawing.Image)
        Me.BtnStopRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnStopRun.Location = New System.Drawing.Point(0, 245)
        Me.BtnStopRun.Name = "BtnStopRun"
        Me.BtnStopRun.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.BtnStopRun.Size = New System.Drawing.Size(220, 40)
        Me.BtnStopRun.TabIndex = 2
        Me.BtnStopRun.Text = "     Stop run"
        Me.BtnStopRun.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnStopRun.UseVisualStyleBackColor = True
        '
        'BtnStartRun
        '
        Me.BtnStartRun.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnStartRun.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnStartRun.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(143, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.BtnStartRun.FlatAppearance.BorderSize = 0
        Me.BtnStartRun.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnStartRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnStartRun.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStartRun.ForeColor = System.Drawing.Color.White
        Me.BtnStartRun.Image = CType(resources.GetObject("BtnStartRun.Image"), System.Drawing.Image)
        Me.BtnStartRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnStartRun.Location = New System.Drawing.Point(0, 205)
        Me.BtnStartRun.Name = "BtnStartRun"
        Me.BtnStartRun.Padding = New System.Windows.Forms.Padding(5, 0, 0, 0)
        Me.BtnStartRun.Size = New System.Drawing.Size(220, 40)
        Me.BtnStartRun.TabIndex = 1
        Me.BtnStartRun.Text = "     Start run"
        Me.BtnStartRun.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnStartRun.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.BtnMenu)
        Me.Panel1.Controls.Add(Me.PictureBox)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(220, 205)
        Me.Panel1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(33, 1)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(184, 67)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Robotic Observatory "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnMenu
        '
        Me.BtnMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.BtnMenu.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(127, Byte), Integer), CType(CType(143, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.BtnMenu.FlatAppearance.BorderSize = 0
        Me.BtnMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnMenu.ForeColor = System.Drawing.Color.White
        Me.BtnMenu.Image = CType(resources.GetObject("BtnMenu.Image"), System.Drawing.Image)
        Me.BtnMenu.Location = New System.Drawing.Point(6, 114)
        Me.BtnMenu.Name = "BtnMenu"
        Me.BtnMenu.Size = New System.Drawing.Size(28, 32)
        Me.BtnMenu.TabIndex = 1
        Me.BtnMenu.UseVisualStyleBackColor = True
        '
        'PictureBox
        '
        Me.PictureBox.Image = CType(resources.GetObject("PictureBox.Image"), System.Drawing.Image)
        Me.PictureBox.Location = New System.Drawing.Point(63, 71)
        Me.PictureBox.Name = "PictureBox"
        Me.PictureBox.Size = New System.Drawing.Size(128, 128)
        Me.PictureBox.TabIndex = 0
        Me.PictureBox.TabStop = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.LblCloudSafe)
        Me.Panel2.Controls.Add(Me.txtRelativeHumidity)
        Me.Panel2.Controls.Add(Me.lblRain)
        Me.Panel2.Controls.Add(Me.lblLight)
        Me.Panel2.Controls.Add(Me.lblCloud)
        Me.Panel2.Controls.Add(Me.txtAmbientTemperature)
        Me.Panel2.Controls.Add(Me.lblLastRead)
        Me.Panel2.Controls.Add(Me.lblRelativeHumidity)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.lblSafe)
        Me.Panel2.Location = New System.Drawing.Point(5, 112)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(161, 250)
        Me.Panel2.TabIndex = 108
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(6, 4)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(35, 16)
        Me.Label3.TabIndex = 41
        Me.Label3.Text = "AAG"
        '
        'LblCloudSafe
        '
        Me.LblCloudSafe.BackColor = System.Drawing.Color.Transparent
        Me.LblCloudSafe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCloudSafe.ForeColor = System.Drawing.Color.White
        Me.LblCloudSafe.Location = New System.Drawing.Point(8, 146)
        Me.LblCloudSafe.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblCloudSafe.Name = "LblCloudSafe"
        Me.LblCloudSafe.Size = New System.Drawing.Size(147, 25)
        Me.LblCloudSafe.TabIndex = 40
        Me.LblCloudSafe.Text = "Unsafe and open"
        Me.LblCloudSafe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtRelativeHumidity
        '
        Me.txtRelativeHumidity.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtRelativeHumidity.Enabled = False
        Me.txtRelativeHumidity.Location = New System.Drawing.Point(101, 201)
        Me.txtRelativeHumidity.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRelativeHumidity.Name = "txtRelativeHumidity"
        Me.txtRelativeHumidity.Size = New System.Drawing.Size(59, 16)
        Me.txtRelativeHumidity.TabIndex = 39
        '
        'lblRain
        '
        Me.lblRain.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRain.ForeColor = System.Drawing.Color.White
        Me.lblRain.Location = New System.Drawing.Point(9, 108)
        Me.lblRain.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRain.Name = "lblRain"
        Me.lblRain.Size = New System.Drawing.Size(146, 36)
        Me.lblRain.TabIndex = 37
        Me.lblRain.Text = "Unknown"
        Me.lblRain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblLight
        '
        Me.lblLight.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLight.ForeColor = System.Drawing.Color.White
        Me.lblLight.Location = New System.Drawing.Point(9, 69)
        Me.lblLight.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblLight.Name = "lblLight"
        Me.lblLight.Size = New System.Drawing.Size(146, 36)
        Me.lblLight.TabIndex = 36
        Me.lblLight.Text = "Unknown"
        Me.lblLight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCloud
        '
        Me.lblCloud.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCloud.ForeColor = System.Drawing.Color.White
        Me.lblCloud.Location = New System.Drawing.Point(9, 30)
        Me.lblCloud.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCloud.Name = "lblCloud"
        Me.lblCloud.Size = New System.Drawing.Size(146, 36)
        Me.lblCloud.TabIndex = 35
        Me.lblCloud.Text = "Unknown"
        Me.lblCloud.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtAmbientTemperature
        '
        Me.txtAmbientTemperature.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtAmbientTemperature.Enabled = False
        Me.txtAmbientTemperature.Location = New System.Drawing.Point(101, 175)
        Me.txtAmbientTemperature.Margin = New System.Windows.Forms.Padding(4)
        Me.txtAmbientTemperature.Name = "txtAmbientTemperature"
        Me.txtAmbientTemperature.Size = New System.Drawing.Size(59, 16)
        Me.txtAmbientTemperature.TabIndex = 34
        '
        'lblLastRead
        '
        Me.lblLastRead.AutoSize = True
        Me.lblLastRead.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastRead.ForeColor = System.Drawing.Color.White
        Me.lblLastRead.Location = New System.Drawing.Point(5, 219)
        Me.lblLastRead.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblLastRead.Name = "lblLastRead"
        Me.lblLastRead.Size = New System.Drawing.Size(46, 12)
        Me.lblLastRead.TabIndex = 33
        Me.lblLastRead.Text = "Last read:"
        '
        'lblRelativeHumidity
        '
        Me.lblRelativeHumidity.AutoSize = True
        Me.lblRelativeHumidity.ForeColor = System.Drawing.Color.White
        Me.lblRelativeHumidity.Location = New System.Drawing.Point(3, 198)
        Me.lblRelativeHumidity.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRelativeHumidity.Name = "lblRelativeHumidity"
        Me.lblRelativeHumidity.Size = New System.Drawing.Size(61, 16)
        Me.lblRelativeHumidity.TabIndex = 32
        Me.lblRelativeHumidity.Text = "Humidity:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(3, 174)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 16)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "Temperature:"
        '
        'lblSafe
        '
        Me.lblSafe.BackColor = System.Drawing.SystemColors.Control
        Me.lblSafe.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSafe.Location = New System.Drawing.Point(5, 22)
        Me.lblSafe.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSafe.Name = "lblSafe"
        Me.lblSafe.Size = New System.Drawing.Size(155, 151)
        Me.lblSafe.TabIndex = 38
        Me.lblSafe.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'PanelTop
        '
        Me.PanelTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.PanelTop.Controls.Add(Me.LblHang)
        Me.PanelTop.Controls.Add(Me.LblVersion)
        Me.PanelTop.Controls.Add(Me.LblRoof)
        Me.PanelTop.Controls.Add(Me.LblSwitch)
        Me.PanelTop.Controls.Add(Me.LblCover)
        Me.PanelTop.Controls.Add(Me.LblTSX)
        Me.PanelTop.Controls.Add(Me.LblInternet)
        Me.PanelTop.Controls.Add(Me.LblUPSCyberPower)
        Me.PanelTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelTop.Location = New System.Drawing.Point(0, 0)
        Me.PanelTop.Name = "PanelTop"
        Me.PanelTop.Size = New System.Drawing.Size(1652, 42)
        Me.PanelTop.TabIndex = 107
        '
        'LblHang
        '
        Me.LblHang.AutoSize = True
        Me.LblHang.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHang.ForeColor = System.Drawing.Color.White
        Me.LblHang.Location = New System.Drawing.Point(1117, 13)
        Me.LblHang.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblHang.Name = "LblHang"
        Me.LblHang.Size = New System.Drawing.Size(16, 16)
        Me.LblHang.TabIndex = 106
        Me.LblHang.Text = "X"
        '
        'LblVersion
        '
        Me.LblVersion.AutoSize = True
        Me.LblVersion.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblVersion.ForeColor = System.Drawing.Color.White
        Me.LblVersion.Location = New System.Drawing.Point(1021, 13)
        Me.LblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblVersion.Name = "LblVersion"
        Me.LblVersion.Size = New System.Drawing.Size(88, 16)
        Me.LblVersion.TabIndex = 105
        Me.LblVersion.Text = "Version 3.00"
        Me.LblVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'LblRoof
        '
        Me.LblRoof.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRoof.ForeColor = System.Drawing.Color.White
        Me.LblRoof.Location = New System.Drawing.Point(4, 5)
        Me.LblRoof.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblRoof.Name = "LblRoof"
        Me.LblRoof.Size = New System.Drawing.Size(121, 32)
        Me.LblRoof.TabIndex = 87
        Me.LblRoof.Text = "ROOF"
        Me.LblRoof.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblSwitch
        '
        Me.LblSwitch.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSwitch.ForeColor = System.Drawing.Color.White
        Me.LblSwitch.Location = New System.Drawing.Point(127, 5)
        Me.LblSwitch.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblSwitch.Name = "LblSwitch"
        Me.LblSwitch.Size = New System.Drawing.Size(121, 32)
        Me.LblSwitch.TabIndex = 95
        Me.LblSwitch.Text = "SWITCH"
        Me.LblSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblCover
        '
        Me.LblCover.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCover.ForeColor = System.Drawing.Color.White
        Me.LblCover.Location = New System.Drawing.Point(250, 5)
        Me.LblCover.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblCover.Name = "LblCover"
        Me.LblCover.Size = New System.Drawing.Size(121, 32)
        Me.LblCover.TabIndex = 94
        Me.LblCover.Text = "COVER"
        Me.LblCover.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblTSX
        '
        Me.LblTSX.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTSX.ForeColor = System.Drawing.Color.White
        Me.LblTSX.Location = New System.Drawing.Point(374, 5)
        Me.LblTSX.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblTSX.Name = "LblTSX"
        Me.LblTSX.Size = New System.Drawing.Size(121, 32)
        Me.LblTSX.TabIndex = 84
        Me.LblTSX.Text = "TSX"
        Me.LblTSX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblInternet
        '
        Me.LblInternet.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblInternet.ForeColor = System.Drawing.Color.White
        Me.LblInternet.Location = New System.Drawing.Point(498, 5)
        Me.LblInternet.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblInternet.Name = "LblInternet"
        Me.LblInternet.Size = New System.Drawing.Size(121, 32)
        Me.LblInternet.TabIndex = 85
        Me.LblInternet.Text = "INTERNET"
        Me.LblInternet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblUPSCyberPower
        '
        Me.LblUPSCyberPower.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblUPSCyberPower.ForeColor = System.Drawing.Color.White
        Me.LblUPSCyberPower.Location = New System.Drawing.Point(621, 5)
        Me.LblUPSCyberPower.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblUPSCyberPower.Name = "LblUPSCyberPower"
        Me.LblUPSCyberPower.Size = New System.Drawing.Size(120, 32)
        Me.LblUPSCyberPower.TabIndex = 91
        Me.LblUPSCyberPower.Text = "UPS"
        Me.LblUPSCyberPower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'StatusStrip
        '
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 965)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(1652, 22)
        Me.StatusStrip.TabIndex = 106
        Me.StatusStrip.Text = "StatusStrip"
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(39, 17)
        Me.ToolStripStatusLabel.Text = "Status"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.GroupBoxTwilight)
        Me.GroupBox3.Controls.Add(Me.GroupBox2)
        Me.GroupBox3.Controls.Add(Me.LblLST)
        Me.GroupBox3.Controls.Add(Me.LblGST)
        Me.GroupBox3.Location = New System.Drawing.Point(1193, 546)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox3.Size = New System.Drawing.Size(231, 288)
        Me.GroupBox3.TabIndex = 105
        Me.GroupBox3.TabStop = False
        '
        'GroupBoxTwilight
        '
        Me.GroupBoxTwilight.Controls.Add(Me.LblCivilTwilight)
        Me.GroupBoxTwilight.Controls.Add(Me.LblAstronomicalTwilight)
        Me.GroupBoxTwilight.Controls.Add(Me.LblAmateurTwilight)
        Me.GroupBoxTwilight.Location = New System.Drawing.Point(4, 59)
        Me.GroupBoxTwilight.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBoxTwilight.Name = "GroupBoxTwilight"
        Me.GroupBoxTwilight.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBoxTwilight.Size = New System.Drawing.Size(220, 79)
        Me.GroupBoxTwilight.TabIndex = 74
        Me.GroupBoxTwilight.TabStop = False
        Me.GroupBoxTwilight.Text = "Twilight"
        '
        'LblCivilTwilight
        '
        Me.LblCivilTwilight.AutoSize = True
        Me.LblCivilTwilight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCivilTwilight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblCivilTwilight.Location = New System.Drawing.Point(4, 20)
        Me.LblCivilTwilight.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblCivilTwilight.Name = "LblCivilTwilight"
        Me.LblCivilTwilight.Size = New System.Drawing.Size(76, 13)
        Me.LblCivilTwilight.TabIndex = 70
        Me.LblCivilTwilight.Text = "LblCivilTwilight"
        '
        'LblAstronomicalTwilight
        '
        Me.LblAstronomicalTwilight.AutoSize = True
        Me.LblAstronomicalTwilight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAstronomicalTwilight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblAstronomicalTwilight.Location = New System.Drawing.Point(4, 57)
        Me.LblAstronomicalTwilight.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblAstronomicalTwilight.Name = "LblAstronomicalTwilight"
        Me.LblAstronomicalTwilight.Size = New System.Drawing.Size(117, 13)
        Me.LblAstronomicalTwilight.TabIndex = 73
        Me.LblAstronomicalTwilight.Text = "LblAstronomicalTwilight"
        '
        'LblAmateurTwilight
        '
        Me.LblAmateurTwilight.AutoSize = True
        Me.LblAmateurTwilight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAmateurTwilight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblAmateurTwilight.Location = New System.Drawing.Point(4, 38)
        Me.LblAmateurTwilight.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblAmateurTwilight.Name = "LblAmateurTwilight"
        Me.LblAmateurTwilight.Size = New System.Drawing.Size(96, 13)
        Me.LblAmateurTwilight.TabIndex = 72
        Me.LblAmateurTwilight.Text = "LblAmateurTwilight"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.LblSunDawnFlats)
        Me.GroupBox2.Controls.Add(Me.LblSunDuskFlats)
        Me.GroupBox2.Controls.Add(Me.LblSunStopRun)
        Me.GroupBox2.Controls.Add(Me.LblSunStartRun)
        Me.GroupBox2.Controls.Add(Me.LblSunOpenRoof)
        Me.GroupBox2.Location = New System.Drawing.Point(4, 140)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox2.Size = New System.Drawing.Size(220, 116)
        Me.GroupBox2.TabIndex = 79
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Run times"
        '
        'LblSunDawnFlats
        '
        Me.LblSunDawnFlats.AutoSize = True
        Me.LblSunDawnFlats.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSunDawnFlats.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblSunDawnFlats.Location = New System.Drawing.Point(4, 95)
        Me.LblSunDawnFlats.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblSunDawnFlats.Name = "LblSunDawnFlats"
        Me.LblSunDawnFlats.Size = New System.Drawing.Size(90, 13)
        Me.LblSunDawnFlats.TabIndex = 75
        Me.LblSunDawnFlats.Text = "LblSunDawnFlats"
        '
        'LblSunDuskFlats
        '
        Me.LblSunDuskFlats.AutoSize = True
        Me.LblSunDuskFlats.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSunDuskFlats.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblSunDuskFlats.Location = New System.Drawing.Point(4, 38)
        Me.LblSunDuskFlats.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblSunDuskFlats.Name = "LblSunDuskFlats"
        Me.LblSunDuskFlats.Size = New System.Drawing.Size(87, 13)
        Me.LblSunDuskFlats.TabIndex = 74
        Me.LblSunDuskFlats.Text = "LblSunDuskFlats"
        '
        'LblSunStopRun
        '
        Me.LblSunStopRun.AutoSize = True
        Me.LblSunStopRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSunStopRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblSunStopRun.Location = New System.Drawing.Point(4, 76)
        Me.LblSunStopRun.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblSunStopRun.Name = "LblSunStopRun"
        Me.LblSunStopRun.Size = New System.Drawing.Size(82, 13)
        Me.LblSunStopRun.TabIndex = 73
        Me.LblSunStopRun.Text = "LblSunStopRun"
        '
        'LblSunStartRun
        '
        Me.LblSunStartRun.AutoSize = True
        Me.LblSunStartRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSunStartRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblSunStartRun.Location = New System.Drawing.Point(4, 57)
        Me.LblSunStartRun.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblSunStartRun.Name = "LblSunStartRun"
        Me.LblSunStartRun.Size = New System.Drawing.Size(82, 13)
        Me.LblSunStartRun.TabIndex = 72
        Me.LblSunStartRun.Text = "LblSunStartRun"
        '
        'LblSunOpenRoof
        '
        Me.LblSunOpenRoof.AutoSize = True
        Me.LblSunOpenRoof.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSunOpenRoof.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblSunOpenRoof.Location = New System.Drawing.Point(4, 20)
        Me.LblSunOpenRoof.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblSunOpenRoof.Name = "LblSunOpenRoof"
        Me.LblSunOpenRoof.Size = New System.Drawing.Size(89, 13)
        Me.LblSunOpenRoof.TabIndex = 71
        Me.LblSunOpenRoof.Text = "LblSunOpenRoof"
        '
        'LblLST
        '
        Me.LblLST.AutoSize = True
        Me.LblLST.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLST.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblLST.Location = New System.Drawing.Point(7, 16)
        Me.LblLST.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblLST.Name = "LblLST"
        Me.LblLST.Size = New System.Drawing.Size(41, 13)
        Me.LblLST.TabIndex = 68
        Me.LblLST.Text = "LblLST"
        '
        'LblGST
        '
        Me.LblGST.AutoSize = True
        Me.LblGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblGST.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblGST.Location = New System.Drawing.Point(7, 37)
        Me.LblGST.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblGST.Name = "LblGST"
        Me.LblGST.Size = New System.Drawing.Size(43, 13)
        Me.LblGST.TabIndex = 69
        Me.LblGST.Text = "LblGST"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.LblCCDFilter)
        Me.GroupBox1.Controls.Add(Me.LblCCDTemp)
        Me.GroupBox1.Controls.Add(Me.LblCCDExposureStatus)
        Me.GroupBox1.Location = New System.Drawing.Point(911, 238)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Size = New System.Drawing.Size(231, 100)
        Me.GroupBox1.TabIndex = 101
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "CCD"
        '
        'LblCCDFilter
        '
        Me.LblCCDFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblCCDFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold)
        Me.LblCCDFilter.Location = New System.Drawing.Point(8, 55)
        Me.LblCCDFilter.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblCCDFilter.Name = "LblCCDFilter"
        Me.LblCCDFilter.Size = New System.Drawing.Size(55, 36)
        Me.LblCCDFilter.TabIndex = 29
        Me.LblCCDFilter.Text = "L"
        Me.LblCCDFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblCCDTemp
        '
        Me.LblCCDTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblCCDTemp.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCCDTemp.Location = New System.Drawing.Point(71, 55)
        Me.LblCCDTemp.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblCCDTemp.Name = "LblCCDTemp"
        Me.LblCCDTemp.Size = New System.Drawing.Size(152, 36)
        Me.LblCCDTemp.TabIndex = 28
        Me.LblCCDTemp.Text = "Temp"
        Me.LblCCDTemp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblCCDExposureStatus
        '
        Me.LblCCDExposureStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblCCDExposureStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold)
        Me.LblCCDExposureStatus.Location = New System.Drawing.Point(8, 16)
        Me.LblCCDExposureStatus.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblCCDExposureStatus.Name = "LblCCDExposureStatus"
        Me.LblCCDExposureStatus.Size = New System.Drawing.Size(216, 36)
        Me.LblCCDExposureStatus.TabIndex = 26
        Me.LblCCDExposureStatus.Text = "Exposure"
        Me.LblCCDExposureStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ChkDisableSafetyCheck
        '
        Me.ChkDisableSafetyCheck.AutoSize = True
        Me.ChkDisableSafetyCheck.Location = New System.Drawing.Point(1314, 457)
        Me.ChkDisableSafetyCheck.Margin = New System.Windows.Forms.Padding(4)
        Me.ChkDisableSafetyCheck.Name = "ChkDisableSafetyCheck"
        Me.ChkDisableSafetyCheck.Size = New System.Drawing.Size(151, 20)
        Me.ChkDisableSafetyCheck.TabIndex = 98
        Me.ChkDisableSafetyCheck.Text = "Disable safety check ?"
        Me.ChkDisableSafetyCheck.UseVisualStyleBackColor = True
        '
        'GrpFocus
        '
        Me.GrpFocus.Controls.Add(Me.LblLastFocusDateTime)
        Me.GrpFocus.Controls.Add(Me.LblLastFocusTemperature)
        Me.GrpFocus.Controls.Add(Me.LblFocusserTemperature)
        Me.GrpFocus.Controls.Add(Me.LblFocusserPosition)
        Me.GrpFocus.Location = New System.Drawing.Point(919, 351)
        Me.GrpFocus.Margin = New System.Windows.Forms.Padding(4)
        Me.GrpFocus.Name = "GrpFocus"
        Me.GrpFocus.Padding = New System.Windows.Forms.Padding(4)
        Me.GrpFocus.Size = New System.Drawing.Size(231, 140)
        Me.GrpFocus.TabIndex = 97
        Me.GrpFocus.TabStop = False
        Me.GrpFocus.Text = "Focusser"
        '
        'LblLastFocusDateTime
        '
        Me.LblLastFocusDateTime.AutoSize = True
        Me.LblLastFocusDateTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLastFocusDateTime.Location = New System.Drawing.Point(6, 114)
        Me.LblLastFocusDateTime.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblLastFocusDateTime.Name = "LblLastFocusDateTime"
        Me.LblLastFocusDateTime.Size = New System.Drawing.Size(56, 13)
        Me.LblLastFocusDateTime.TabIndex = 29
        Me.LblLastFocusDateTime.Text = "Last focus"
        '
        'LblLastFocusTemperature
        '
        Me.LblLastFocusTemperature.AutoSize = True
        Me.LblLastFocusTemperature.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLastFocusTemperature.Location = New System.Drawing.Point(6, 97)
        Me.LblLastFocusTemperature.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblLastFocusTemperature.Name = "LblLastFocusTemperature"
        Me.LblLastFocusTemperature.Size = New System.Drawing.Size(86, 13)
        Me.LblLastFocusTemperature.TabIndex = 28
        Me.LblLastFocusTemperature.Text = "Last temperature"
        '
        'LblFocusserTemperature
        '
        Me.LblFocusserTemperature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblFocusserTemperature.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFocusserTemperature.Location = New System.Drawing.Point(8, 58)
        Me.LblFocusserTemperature.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblFocusserTemperature.Name = "LblFocusserTemperature"
        Me.LblFocusserTemperature.Size = New System.Drawing.Size(216, 36)
        Me.LblFocusserTemperature.TabIndex = 27
        Me.LblFocusserTemperature.Text = "Temp"
        Me.LblFocusserTemperature.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblFocusserPosition
        '
        Me.LblFocusserPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblFocusserPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFocusserPosition.Location = New System.Drawing.Point(8, 18)
        Me.LblFocusserPosition.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblFocusserPosition.Name = "LblFocusserPosition"
        Me.LblFocusserPosition.Size = New System.Drawing.Size(216, 36)
        Me.LblFocusserPosition.TabIndex = 26
        Me.LblFocusserPosition.Text = "Position"
        Me.LblFocusserPosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox
        '
        Me.GroupBox.Controls.Add(Me.grpMoon)
        Me.GroupBox.Controls.Add(Me.grpSun)
        Me.GroupBox.Location = New System.Drawing.Point(4, 360)
        Me.GroupBox.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox.Size = New System.Drawing.Size(162, 341)
        Me.GroupBox.TabIndex = 96
        Me.GroupBox.TabStop = False
        '
        'grpMoon
        '
        Me.grpMoon.Controls.Add(Me.LblMoonSettingRising)
        Me.grpMoon.Controls.Add(Me.LblDECMoon)
        Me.grpMoon.Controls.Add(Me.LblPhaseMoon)
        Me.grpMoon.Controls.Add(Me.LblRisingSettingMoon)
        Me.grpMoon.Controls.Add(Me.LblAltMoon)
        Me.grpMoon.Controls.Add(Me.LblRAMoon)
        Me.grpMoon.Location = New System.Drawing.Point(4, 9)
        Me.grpMoon.Margin = New System.Windows.Forms.Padding(4)
        Me.grpMoon.Name = "grpMoon"
        Me.grpMoon.Padding = New System.Windows.Forms.Padding(4)
        Me.grpMoon.Size = New System.Drawing.Size(154, 183)
        Me.grpMoon.TabIndex = 59
        Me.grpMoon.TabStop = False
        Me.grpMoon.Text = "Moon"
        '
        'LblMoonSettingRising
        '
        Me.LblMoonSettingRising.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblMoonSettingRising.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMoonSettingRising.Location = New System.Drawing.Point(4, 84)
        Me.LblMoonSettingRising.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblMoonSettingRising.Name = "LblMoonSettingRising"
        Me.LblMoonSettingRising.Size = New System.Drawing.Size(143, 32)
        Me.LblMoonSettingRising.TabIndex = 10
        Me.LblMoonSettingRising.Text = "LblMoonSettingRising"
        Me.LblMoonSettingRising.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblDECMoon
        '
        Me.LblDECMoon.AutoSize = True
        Me.LblDECMoon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDECMoon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblDECMoon.Location = New System.Drawing.Point(4, 142)
        Me.LblDECMoon.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblDECMoon.Name = "LblDECMoon"
        Me.LblDECMoon.Size = New System.Drawing.Size(70, 13)
        Me.LblDECMoon.TabIndex = 9
        Me.LblDECMoon.Text = "LblDECMoon"
        '
        'LblPhaseMoon
        '
        Me.LblPhaseMoon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblPhaseMoon.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPhaseMoon.Location = New System.Drawing.Point(4, 50)
        Me.LblPhaseMoon.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblPhaseMoon.Name = "LblPhaseMoon"
        Me.LblPhaseMoon.Size = New System.Drawing.Size(143, 32)
        Me.LblPhaseMoon.TabIndex = 8
        Me.LblPhaseMoon.Text = "LblPhaseMoon"
        Me.LblPhaseMoon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblRisingSettingMoon
        '
        Me.LblRisingSettingMoon.AutoSize = True
        Me.LblRisingSettingMoon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRisingSettingMoon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblRisingSettingMoon.Location = New System.Drawing.Point(4, 164)
        Me.LblRisingSettingMoon.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblRisingSettingMoon.Name = "LblRisingSettingMoon"
        Me.LblRisingSettingMoon.Size = New System.Drawing.Size(110, 13)
        Me.LblRisingSettingMoon.TabIndex = 7
        Me.LblRisingSettingMoon.Text = "LblRisingSettingMoon"
        '
        'LblAltMoon
        '
        Me.LblAltMoon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblAltMoon.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAltMoon.Location = New System.Drawing.Point(4, 17)
        Me.LblAltMoon.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblAltMoon.Name = "LblAltMoon"
        Me.LblAltMoon.Size = New System.Drawing.Size(143, 32)
        Me.LblAltMoon.TabIndex = 2
        Me.LblAltMoon.Text = "LblAltMoon"
        Me.LblAltMoon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblRAMoon
        '
        Me.LblRAMoon.AutoSize = True
        Me.LblRAMoon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRAMoon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblRAMoon.Location = New System.Drawing.Point(4, 119)
        Me.LblRAMoon.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblRAMoon.Name = "LblRAMoon"
        Me.LblRAMoon.Size = New System.Drawing.Size(63, 13)
        Me.LblRAMoon.TabIndex = 1
        Me.LblRAMoon.Text = "LblRAMoon"
        '
        'grpSun
        '
        Me.grpSun.Controls.Add(Me.LblSunSettingRising)
        Me.grpSun.Controls.Add(Me.LblDECSun)
        Me.grpSun.Controls.Add(Me.LblRisingSettingSun)
        Me.grpSun.Controls.Add(Me.LblRASun)
        Me.grpSun.Controls.Add(Me.LblAltSun)
        Me.grpSun.Location = New System.Drawing.Point(4, 192)
        Me.grpSun.Margin = New System.Windows.Forms.Padding(4)
        Me.grpSun.Name = "grpSun"
        Me.grpSun.Padding = New System.Windows.Forms.Padding(4)
        Me.grpSun.Size = New System.Drawing.Size(154, 145)
        Me.grpSun.TabIndex = 58
        Me.grpSun.TabStop = False
        Me.grpSun.Text = "Sun"
        '
        'LblSunSettingRising
        '
        Me.LblSunSettingRising.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblSunSettingRising.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSunSettingRising.Location = New System.Drawing.Point(4, 49)
        Me.LblSunSettingRising.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblSunSettingRising.Name = "LblSunSettingRising"
        Me.LblSunSettingRising.Size = New System.Drawing.Size(143, 32)
        Me.LblSunSettingRising.TabIndex = 6
        Me.LblSunSettingRising.Text = "LblSunSettingRising"
        Me.LblSunSettingRising.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblDECSun
        '
        Me.LblDECSun.AutoSize = True
        Me.LblDECSun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDECSun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblDECSun.Location = New System.Drawing.Point(4, 103)
        Me.LblDECSun.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblDECSun.Name = "LblDECSun"
        Me.LblDECSun.Size = New System.Drawing.Size(62, 13)
        Me.LblDECSun.TabIndex = 5
        Me.LblDECSun.Text = "LblDECSun"
        '
        'LblRisingSettingSun
        '
        Me.LblRisingSettingSun.AutoSize = True
        Me.LblRisingSettingSun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRisingSettingSun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblRisingSettingSun.Location = New System.Drawing.Point(4, 123)
        Me.LblRisingSettingSun.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblRisingSettingSun.Name = "LblRisingSettingSun"
        Me.LblRisingSettingSun.Size = New System.Drawing.Size(102, 13)
        Me.LblRisingSettingSun.TabIndex = 4
        Me.LblRisingSettingSun.Text = "LblRisingSettingSun"
        '
        'LblRASun
        '
        Me.LblRASun.AutoSize = True
        Me.LblRASun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRASun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblRASun.Location = New System.Drawing.Point(4, 82)
        Me.LblRASun.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblRASun.Name = "LblRASun"
        Me.LblRASun.Size = New System.Drawing.Size(55, 13)
        Me.LblRASun.TabIndex = 1
        Me.LblRASun.Text = "LblRASun"
        '
        'LblAltSun
        '
        Me.LblAltSun.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblAltSun.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAltSun.Location = New System.Drawing.Point(4, 16)
        Me.LblAltSun.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblAltSun.Name = "LblAltSun"
        Me.LblAltSun.Size = New System.Drawing.Size(143, 32)
        Me.LblAltSun.TabIndex = 0
        Me.LblAltSun.Text = "LblAltSun"
        Me.LblAltSun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ChkSimulatorMode
        '
        Me.ChkSimulatorMode.AutoSize = True
        Me.ChkSimulatorMode.Location = New System.Drawing.Point(1235, 179)
        Me.ChkSimulatorMode.Margin = New System.Windows.Forms.Padding(4)
        Me.ChkSimulatorMode.Name = "ChkSimulatorMode"
        Me.ChkSimulatorMode.Size = New System.Drawing.Size(117, 20)
        Me.ChkSimulatorMode.TabIndex = 93
        Me.ChkSimulatorMode.Text = "Simulator mode"
        Me.ChkSimulatorMode.UseVisualStyleBackColor = True
        '
        'ChkAutoStart
        '
        Me.ChkAutoStart.AutoSize = True
        Me.ChkAutoStart.Location = New System.Drawing.Point(1421, 121)
        Me.ChkAutoStart.Margin = New System.Windows.Forms.Padding(4)
        Me.ChkAutoStart.Name = "ChkAutoStart"
        Me.ChkAutoStart.Size = New System.Drawing.Size(137, 20)
        Me.ChkAutoStart.TabIndex = 92
        Me.ChkAutoStart.Text = "Autostart Enabled ?"
        Me.ChkAutoStart.UseVisualStyleBackColor = True
        '
        'RTXLog
        '
        Me.RTXLog.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.RTXLog.Location = New System.Drawing.Point(170, 44)
        Me.RTXLog.Margin = New System.Windows.Forms.Padding(4)
        Me.RTXLog.Name = "RTXLog"
        Me.RTXLog.ReadOnly = True
        Me.RTXLog.Size = New System.Drawing.Size(733, 589)
        Me.RTXLog.TabIndex = 88
        Me.RTXLog.Text = ""
        '
        'LblMonitorStatus
        '
        Me.LblMonitorStatus.BackColor = System.Drawing.Color.Silver
        Me.LblMonitorStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMonitorStatus.ForeColor = System.Drawing.Color.White
        Me.LblMonitorStatus.Location = New System.Drawing.Point(3, 44)
        Me.LblMonitorStatus.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblMonitorStatus.Name = "LblMonitorStatus"
        Me.LblMonitorStatus.Size = New System.Drawing.Size(162, 61)
        Me.LblMonitorStatus.TabIndex = 81
        Me.LblMonitorStatus.Text = "STATUS"
        Me.LblMonitorStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpMount
        '
        Me.grpMount.Controls.Add(Me.LblMountPierSide)
        Me.grpMount.Controls.Add(Me.LblMountRADEC)
        Me.grpMount.Controls.Add(Me.LblMountStatus)
        Me.grpMount.Controls.Add(Me.lblMountAz)
        Me.grpMount.Controls.Add(Me.lblMountAlt)
        Me.grpMount.ImeMode = System.Windows.Forms.ImeMode.Katakana
        Me.grpMount.Location = New System.Drawing.Point(911, 49)
        Me.grpMount.Margin = New System.Windows.Forms.Padding(4)
        Me.grpMount.Name = "grpMount"
        Me.grpMount.Padding = New System.Windows.Forms.Padding(4)
        Me.grpMount.Size = New System.Drawing.Size(231, 181)
        Me.grpMount.TabIndex = 90
        Me.grpMount.TabStop = False
        Me.grpMount.Text = "Mount"
        '
        'LblMountPierSide
        '
        Me.LblMountPierSide.AutoSize = True
        Me.LblMountPierSide.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMountPierSide.Location = New System.Drawing.Point(4, 160)
        Me.LblMountPierSide.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblMountPierSide.Name = "LblMountPierSide"
        Me.LblMountPierSide.Size = New System.Drawing.Size(46, 13)
        Me.LblMountPierSide.TabIndex = 72
        Me.LblMountPierSide.Text = "PierSide"
        '
        'LblMountRADEC
        '
        Me.LblMountRADEC.AutoSize = True
        Me.LblMountRADEC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMountRADEC.Location = New System.Drawing.Point(4, 139)
        Me.LblMountRADEC.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblMountRADEC.Name = "LblMountRADEC"
        Me.LblMountRADEC.Size = New System.Drawing.Size(44, 13)
        Me.LblMountRADEC.TabIndex = 71
        Me.LblMountRADEC.Text = "RADEC"
        '
        'LblMountStatus
        '
        Me.LblMountStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblMountStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMountStatus.Location = New System.Drawing.Point(8, 20)
        Me.LblMountStatus.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblMountStatus.Name = "LblMountStatus"
        Me.LblMountStatus.Size = New System.Drawing.Size(216, 36)
        Me.LblMountStatus.TabIndex = 28
        Me.LblMountStatus.Text = "Tracking"
        Me.LblMountStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblMountAz
        '
        Me.lblMountAz.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblMountAz.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMountAz.Location = New System.Drawing.Point(8, 100)
        Me.lblMountAz.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMountAz.Name = "lblMountAz"
        Me.lblMountAz.Size = New System.Drawing.Size(216, 36)
        Me.lblMountAz.TabIndex = 27
        Me.lblMountAz.Text = "Azimuth"
        Me.lblMountAz.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblMountAlt
        '
        Me.lblMountAlt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblMountAlt.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMountAlt.Location = New System.Drawing.Point(8, 60)
        Me.lblMountAlt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMountAlt.Name = "lblMountAlt"
        Me.lblMountAlt.Size = New System.Drawing.Size(216, 36)
        Me.lblMountAlt.TabIndex = 26
        Me.lblMountAlt.Text = "Altitude"
        Me.lblMountAlt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'RTXErrors
        '
        Me.RTXErrors.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.RTXErrors.Location = New System.Drawing.Point(170, 638)
        Me.RTXErrors.Margin = New System.Windows.Forms.Padding(4)
        Me.RTXErrors.Name = "RTXErrors"
        Me.RTXErrors.Size = New System.Drawing.Size(733, 62)
        Me.RTXErrors.TabIndex = 89
        Me.RTXErrors.Text = ""
        '
        'TimerSplit1
        '
        Me.TimerSplit1.Interval = 1
        '
        'TimerSplit2
        '
        Me.TimerSplit2.Interval = 1
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1856, 987)
        Me.ControlBox = False
        Me.Controls.Add(Me.SplitContainer)
        Me.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " "
        Me.SplitContainer.Panel1.ResumeLayout(False)
        Me.SplitContainer.Panel2.ResumeLayout(False)
        Me.SplitContainer.Panel2.PerformLayout()
        CType(Me.SplitContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        CType(Me.PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.PanelTop.ResumeLayout(False)
        Me.PanelTop.PerformLayout()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBoxTwilight.ResumeLayout(False)
        Me.GroupBoxTwilight.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GrpFocus.ResumeLayout(False)
        Me.GrpFocus.PerformLayout()
        Me.GroupBox.ResumeLayout(False)
        Me.grpMoon.ResumeLayout(False)
        Me.grpMoon.PerformLayout()
        Me.grpSun.ResumeLayout(False)
        Me.grpSun.PerformLayout()
        Me.grpMount.ResumeLayout(False)
        Me.grpMount.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TimerCheckCycle As Timer
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents TimerRoof As Timer
    Friend WithEvents TimerCover As Timer
    Friend WithEvents TimerSwitch As Timer
    Friend WithEvents TimerWeather As Timer
    Friend WithEvents TimerStartRun As Timer
    Friend WithEvents TimerMount As Timer
    Friend WithEvents TimerCCD As Timer
    Friend WithEvents TimerDisaster As Timer
    Friend WithEvents TimerColorStatus As Timer
    Friend WithEvents TimerSmartError As Timer
    Friend WithEvents TimerHeartBeat As Timer
    Friend WithEvents TimerHang As Timer
    Friend WithEvents SplitContainer As SplitContainer
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents GroupBoxTwilight As GroupBox
    Friend WithEvents LblCivilTwilight As Label
    Friend WithEvents LblAstronomicalTwilight As Label
    Friend WithEvents LblAmateurTwilight As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents LblSunDawnFlats As Label
    Friend WithEvents LblSunDuskFlats As Label
    Friend WithEvents LblSunStopRun As Label
    Friend WithEvents LblSunStartRun As Label
    Friend WithEvents LblSunOpenRoof As Label
    Friend WithEvents LblLST As Label
    Friend WithEvents LblGST As Label
    Friend WithEvents BtnViewLog As Button
    Friend WithEvents LblUPSCyberPower As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents LblCCDFilter As Label
    Friend WithEvents LblCCDTemp As Label
    Friend WithEvents LblCCDExposureStatus As Label
    Friend WithEvents BtnClearErrorLog As Button
    Friend WithEvents BtnClearLog As Button
    Friend WithEvents ChkDisableSafetyCheck As CheckBox
    Friend WithEvents GrpFocus As GroupBox
    Friend WithEvents LblLastFocusDateTime As Label
    Friend WithEvents LblLastFocusTemperature As Label
    Friend WithEvents LblFocusserTemperature As Label
    Friend WithEvents LblFocusserPosition As Label
    Friend WithEvents GroupBox As GroupBox
    Friend WithEvents grpMoon As GroupBox
    Friend WithEvents LblMoonSettingRising As Label
    Friend WithEvents LblDECMoon As Label
    Friend WithEvents LblPhaseMoon As Label
    Friend WithEvents LblRisingSettingMoon As Label
    Friend WithEvents LblAltMoon As Label
    Friend WithEvents LblRAMoon As Label
    Friend WithEvents grpSun As GroupBox
    Friend WithEvents LblSunSettingRising As Label
    Friend WithEvents LblDECSun As Label
    Friend WithEvents LblRisingSettingSun As Label
    Friend WithEvents LblRASun As Label
    Friend WithEvents LblAltSun As Label
    Friend WithEvents LblSwitch As Label
    Friend WithEvents LblCover As Label
    Friend WithEvents ChkSimulatorMode As CheckBox
    Friend WithEvents ChkAutoStart As CheckBox
    Friend WithEvents RTXLog As RichTextBox
    Friend WithEvents LblRoof As Label
    Friend WithEvents LblInternet As Label
    Friend WithEvents LblTSX As Label
    Friend WithEvents LblMonitorStatus As Label
    Friend WithEvents grpMount As GroupBox
    Friend WithEvents LblMountPierSide As Label
    Friend WithEvents LblMountRADEC As Label
    Friend WithEvents LblMountStatus As Label
    Friend WithEvents lblMountAz As Label
    Friend WithEvents lblMountAlt As Label
    Friend WithEvents RTXErrors As RichTextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents PictureBox As PictureBox
    Friend WithEvents BtnStartRun As Button
    Friend WithEvents BtnExit As Button
    Friend WithEvents BtnAbout As Button
    Friend WithEvents BtnDebug As Button
    Friend WithEvents BtnStartSentinel As Button
    Friend WithEvents BtnStopSound As Button
    Friend WithEvents BtnCalibration As Button
    Friend WithEvents BtnProperties As Button
    Friend WithEvents BtnTools As Button
    Friend WithEvents BtnHADS As Button
    Friend WithEvents BtnDeepsky As Button
    Friend WithEvents BtnStopRun As Button
    Friend WithEvents TimerSplit1 As Timer
    Friend WithEvents TimerSplit2 As Timer
    Friend WithEvents StatusStrip As StatusStrip
    Friend WithEvents ToolStripStatusLabel As ToolStripStatusLabel
    Friend WithEvents BtnMenu As Button
    Friend WithEvents PanelTop As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents LblHang As Label
    Friend WithEvents LblVersion As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents LblCloudSafe As Label
    Friend WithEvents txtRelativeHumidity As TextBox
    Friend WithEvents lblRain As Label
    Friend WithEvents lblLight As Label
    Friend WithEvents lblCloud As Label
    Friend WithEvents txtAmbientTemperature As TextBox
    Friend WithEvents lblLastRead As Label
    Friend WithEvents lblRelativeHumidity As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblSafe As Label
End Class
