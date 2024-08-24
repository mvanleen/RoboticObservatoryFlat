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
        Me.LblUPSCyberPower = New System.Windows.Forms.Label()
        Me.RTXLog = New System.Windows.Forms.RichTextBox()
        Me.LblRoof = New System.Windows.Forms.Label()
        Me.grpAAG = New System.Windows.Forms.GroupBox()
        Me.LblCloudSafe = New System.Windows.Forms.Label()
        Me.LblBlink = New System.Windows.Forms.Label()
        Me.txtRelativeHumidity = New System.Windows.Forms.TextBox()
        Me.lblRain = New System.Windows.Forms.Label()
        Me.lblLight = New System.Windows.Forms.Label()
        Me.lblCloud = New System.Windows.Forms.Label()
        Me.txtAmbientTemperature = New System.Windows.Forms.TextBox()
        Me.lblLastRead = New System.Windows.Forms.Label()
        Me.lblRelativeHumidity = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblSafe = New System.Windows.Forms.Label()
        Me.LblInternet = New System.Windows.Forms.Label()
        Me.LblTSX = New System.Windows.Forms.Label()
        Me.BtnStop = New System.Windows.Forms.Button()
        Me.BtnStart = New System.Windows.Forms.Button()
        Me.LblMonitorStatus = New System.Windows.Forms.Label()
        Me.lblMountAlt = New System.Windows.Forms.Label()
        Me.grpMount = New System.Windows.Forms.GroupBox()
        Me.LblMountPierSide = New System.Windows.Forms.Label()
        Me.LblMountRADEC = New System.Windows.Forms.Label()
        Me.LblMountStatus = New System.Windows.Forms.Label()
        Me.lblMountAz = New System.Windows.Forms.Label()
        Me.ChkAutoStart = New System.Windows.Forms.CheckBox()
        Me.grpSun = New System.Windows.Forms.GroupBox()
        Me.LblSunSettingRising = New System.Windows.Forms.Label()
        Me.LblDECSun = New System.Windows.Forms.Label()
        Me.LblRisingSettingSun = New System.Windows.Forms.Label()
        Me.LblRASun = New System.Windows.Forms.Label()
        Me.LblAltSun = New System.Windows.Forms.Label()
        Me.grpMoon = New System.Windows.Forms.GroupBox()
        Me.LblMoonSettingRising = New System.Windows.Forms.Label()
        Me.LblDECMoon = New System.Windows.Forms.Label()
        Me.LblPhaseMoon = New System.Windows.Forms.Label()
        Me.LblRisingSettingMoon = New System.Windows.Forms.Label()
        Me.LblAltMoon = New System.Windows.Forms.Label()
        Me.LblRAMoon = New System.Windows.Forms.Label()
        Me.TimerCheckCycle = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ChkSimulatorMode = New System.Windows.Forms.CheckBox()
        Me.TimerRoof = New System.Windows.Forms.Timer(Me.components)
        Me.LblCover = New System.Windows.Forms.Label()
        Me.TimerCover = New System.Windows.Forms.Timer(Me.components)
        Me.LblSwitch = New System.Windows.Forms.Label()
        Me.TimerSwitch = New System.Windows.Forms.Timer(Me.components)
        Me.TimerWeather = New System.Windows.Forms.Timer(Me.components)
        Me.LblLST = New System.Windows.Forms.Label()
        Me.LblGST = New System.Windows.Forms.Label()
        Me.GroupBox = New System.Windows.Forms.GroupBox()
        Me.GroupBoxTwilight = New System.Windows.Forms.GroupBox()
        Me.LblCivilTwilight = New System.Windows.Forms.Label()
        Me.LblAstronomicalTwilight = New System.Windows.Forms.Label()
        Me.LblAmateurTwilight = New System.Windows.Forms.Label()
        Me.TimerStartRun = New System.Windows.Forms.Timer(Me.components)
        Me.TimerMount = New System.Windows.Forms.Timer(Me.components)
        Me.TimerCCD = New System.Windows.Forms.Timer(Me.components)
        Me.RTXErrors = New System.Windows.Forms.RichTextBox()
        Me.TimerDisaster = New System.Windows.Forms.Timer(Me.components)
        Me.GrpFocus = New System.Windows.Forms.GroupBox()
        Me.LblLastFocusDateTime = New System.Windows.Forms.Label()
        Me.LblLastFocusTemperature = New System.Windows.Forms.Label()
        Me.LblFocusserTemperature = New System.Windows.Forms.Label()
        Me.LblFocusserPosition = New System.Windows.Forms.Label()
        Me.ChkDisableSafetyCheck = New System.Windows.Forms.CheckBox()
        Me.BtnClearLog = New System.Windows.Forms.Button()
        Me.BtnClearErrorLog = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.LblCCDFilter = New System.Windows.Forms.Label()
        Me.LblCCDTemp = New System.Windows.Forms.Label()
        Me.LblCCDExposureStatus = New System.Windows.Forms.Label()
        Me.TimerColorStatus = New System.Windows.Forms.Timer(Me.components)
        Me.TimerSmartError = New System.Windows.Forms.Timer(Me.components)
        Me.TimerHeartBeat = New System.Windows.Forms.Timer(Me.components)
        Me.BtnViewLog = New System.Windows.Forms.Button()
        Me.LblVersion = New System.Windows.Forms.Label()
        Me.LblHang = New System.Windows.Forms.Label()
        Me.TimerHang = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.LblSunDawnFlats = New System.Windows.Forms.Label()
        Me.LblSunDuskFlats = New System.Windows.Forms.Label()
        Me.LblSunStopRun = New System.Windows.Forms.Label()
        Me.LblSunStartRun = New System.Windows.Forms.Label()
        Me.LblSunOpenRoof = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.grpAAG.SuspendLayout()
        Me.grpMount.SuspendLayout()
        Me.grpSun.SuspendLayout()
        Me.grpMoon.SuspendLayout()
        Me.GroupBox.SuspendLayout()
        Me.GroupBoxTwilight.SuspendLayout()
        Me.GrpFocus.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'LblUPSCyberPower
        '
        Me.LblUPSCyberPower.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblUPSCyberPower.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblUPSCyberPower.Location = New System.Drawing.Point(669, 5)
        Me.LblUPSCyberPower.Name = "LblUPSCyberPower"
        Me.LblUPSCyberPower.Size = New System.Drawing.Size(103, 26)
        Me.LblUPSCyberPower.TabIndex = 56
        Me.LblUPSCyberPower.Text = "UPS"
        Me.LblUPSCyberPower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'RTXLog
        '
        Me.RTXLog.Location = New System.Drawing.Point(143, 34)
        Me.RTXLog.Name = "RTXLog"
        Me.RTXLog.ReadOnly = True
        Me.RTXLog.Size = New System.Drawing.Size(629, 468)
        Me.RTXLog.TabIndex = 53
        Me.RTXLog.Text = ""
        '
        'LblRoof
        '
        Me.LblRoof.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblRoof.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRoof.Location = New System.Drawing.Point(143, 5)
        Me.LblRoof.Name = "LblRoof"
        Me.LblRoof.Size = New System.Drawing.Size(104, 26)
        Me.LblRoof.TabIndex = 52
        Me.LblRoof.Text = "ROOF"
        Me.LblRoof.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpAAG
        '
        Me.grpAAG.Controls.Add(Me.LblCloudSafe)
        Me.grpAAG.Controls.Add(Me.LblBlink)
        Me.grpAAG.Controls.Add(Me.txtRelativeHumidity)
        Me.grpAAG.Controls.Add(Me.lblRain)
        Me.grpAAG.Controls.Add(Me.lblLight)
        Me.grpAAG.Controls.Add(Me.lblCloud)
        Me.grpAAG.Controls.Add(Me.txtAmbientTemperature)
        Me.grpAAG.Controls.Add(Me.lblLastRead)
        Me.grpAAG.Controls.Add(Me.lblRelativeHumidity)
        Me.grpAAG.Controls.Add(Me.Label2)
        Me.grpAAG.Controls.Add(Me.lblSafe)
        Me.grpAAG.Location = New System.Drawing.Point(2, 115)
        Me.grpAAG.Name = "grpAAG"
        Me.grpAAG.Size = New System.Drawing.Size(138, 191)
        Me.grpAAG.TabIndex = 50
        Me.grpAAG.TabStop = False
        Me.grpAAG.Text = "Clouddata"
        '
        'LblCloudSafe
        '
        Me.LblCloudSafe.BackColor = System.Drawing.Color.Transparent
        Me.LblCloudSafe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCloudSafe.Location = New System.Drawing.Point(5, 115)
        Me.LblCloudSafe.Name = "LblCloudSafe"
        Me.LblCloudSafe.Size = New System.Drawing.Size(124, 20)
        Me.LblCloudSafe.TabIndex = 30
        Me.LblCloudSafe.Text = "Unsafe and open"
        Me.LblCloudSafe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblBlink
        '
        Me.LblBlink.AutoSize = True
        Me.LblBlink.Location = New System.Drawing.Point(147, 147)
        Me.LblBlink.Name = "LblBlink"
        Me.LblBlink.Size = New System.Drawing.Size(10, 13)
        Me.LblBlink.TabIndex = 29
        Me.LblBlink.Text = " "
        '
        'txtRelativeHumidity
        '
        Me.txtRelativeHumidity.Enabled = False
        Me.txtRelativeHumidity.Location = New System.Drawing.Point(85, 159)
        Me.txtRelativeHumidity.Name = "txtRelativeHumidity"
        Me.txtRelativeHumidity.Size = New System.Drawing.Size(51, 20)
        Me.txtRelativeHumidity.TabIndex = 28
        '
        'lblRain
        '
        Me.lblRain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblRain.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRain.Location = New System.Drawing.Point(7, 84)
        Me.lblRain.Name = "lblRain"
        Me.lblRain.Size = New System.Drawing.Size(122, 30)
        Me.lblRain.TabIndex = 26
        Me.lblRain.Text = "Unknown"
        Me.lblRain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblLight
        '
        Me.lblLight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblLight.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLight.Location = New System.Drawing.Point(7, 52)
        Me.lblLight.Name = "lblLight"
        Me.lblLight.Size = New System.Drawing.Size(122, 30)
        Me.lblLight.TabIndex = 25
        Me.lblLight.Text = "Unknown"
        Me.lblLight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCloud
        '
        Me.lblCloud.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCloud.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCloud.Location = New System.Drawing.Point(7, 20)
        Me.lblCloud.Name = "lblCloud"
        Me.lblCloud.Size = New System.Drawing.Size(122, 30)
        Me.lblCloud.TabIndex = 24
        Me.lblCloud.Text = "Unknown"
        Me.lblCloud.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtAmbientTemperature
        '
        Me.txtAmbientTemperature.Enabled = False
        Me.txtAmbientTemperature.Location = New System.Drawing.Point(85, 138)
        Me.txtAmbientTemperature.Name = "txtAmbientTemperature"
        Me.txtAmbientTemperature.Size = New System.Drawing.Size(50, 20)
        Me.txtAmbientTemperature.TabIndex = 23
        '
        'lblLastRead
        '
        Me.lblLastRead.AutoSize = True
        Me.lblLastRead.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLastRead.Location = New System.Drawing.Point(5, 176)
        Me.lblLastRead.Name = "lblLastRead"
        Me.lblLastRead.Size = New System.Drawing.Size(46, 12)
        Me.lblLastRead.TabIndex = 7
        Me.lblLastRead.Text = "Last read:"
        '
        'lblRelativeHumidity
        '
        Me.lblRelativeHumidity.AutoSize = True
        Me.lblRelativeHumidity.Location = New System.Drawing.Point(4, 159)
        Me.lblRelativeHumidity.Name = "lblRelativeHumidity"
        Me.lblRelativeHumidity.Size = New System.Drawing.Size(50, 13)
        Me.lblRelativeHumidity.TabIndex = 2
        Me.lblRelativeHumidity.Text = "Humidity:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 140)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Temperature:"
        '
        'lblSafe
        '
        Me.lblSafe.BackColor = System.Drawing.SystemColors.Control
        Me.lblSafe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSafe.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSafe.Location = New System.Drawing.Point(2, 14)
        Me.lblSafe.Name = "lblSafe"
        Me.lblSafe.Size = New System.Drawing.Size(133, 123)
        Me.lblSafe.TabIndex = 27
        Me.lblSafe.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'LblInternet
        '
        Me.LblInternet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblInternet.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblInternet.Location = New System.Drawing.Point(563, 5)
        Me.LblInternet.Name = "LblInternet"
        Me.LblInternet.Size = New System.Drawing.Size(104, 26)
        Me.LblInternet.TabIndex = 48
        Me.LblInternet.Text = "INTERNET"
        Me.LblInternet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblTSX
        '
        Me.LblTSX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblTSX.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTSX.Location = New System.Drawing.Point(458, 5)
        Me.LblTSX.Name = "LblTSX"
        Me.LblTSX.Size = New System.Drawing.Size(104, 26)
        Me.LblTSX.TabIndex = 47
        Me.LblTSX.Text = "TSX"
        Me.LblTSX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnStop
        '
        Me.BtnStop.Location = New System.Drawing.Point(73, 71)
        Me.BtnStop.Name = "BtnStop"
        Me.BtnStop.Size = New System.Drawing.Size(68, 23)
        Me.BtnStop.TabIndex = 46
        Me.BtnStop.Text = "&Stop"
        Me.BtnStop.UseVisualStyleBackColor = True
        '
        'BtnStart
        '
        Me.BtnStart.Location = New System.Drawing.Point(2, 70)
        Me.BtnStart.Name = "BtnStart"
        Me.BtnStart.Size = New System.Drawing.Size(68, 23)
        Me.BtnStart.TabIndex = 45
        Me.BtnStart.Text = "St&art"
        Me.BtnStart.UseVisualStyleBackColor = True
        '
        'LblMonitorStatus
        '
        Me.LblMonitorStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblMonitorStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMonitorStatus.Location = New System.Drawing.Point(2, 5)
        Me.LblMonitorStatus.Name = "LblMonitorStatus"
        Me.LblMonitorStatus.Size = New System.Drawing.Size(139, 62)
        Me.LblMonitorStatus.TabIndex = 44
        Me.LblMonitorStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMountAlt
        '
        Me.lblMountAlt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblMountAlt.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMountAlt.Location = New System.Drawing.Point(7, 49)
        Me.lblMountAlt.Name = "lblMountAlt"
        Me.lblMountAlt.Size = New System.Drawing.Size(185, 30)
        Me.lblMountAlt.TabIndex = 26
        Me.lblMountAlt.Text = "Altitude"
        Me.lblMountAlt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpMount
        '
        Me.grpMount.Controls.Add(Me.LblMountPierSide)
        Me.grpMount.Controls.Add(Me.LblMountRADEC)
        Me.grpMount.Controls.Add(Me.LblMountStatus)
        Me.grpMount.Controls.Add(Me.lblMountAz)
        Me.grpMount.Controls.Add(Me.lblMountAlt)
        Me.grpMount.ImeMode = System.Windows.Forms.ImeMode.Katakana
        Me.grpMount.Location = New System.Drawing.Point(777, 2)
        Me.grpMount.Name = "grpMount"
        Me.grpMount.Size = New System.Drawing.Size(198, 147)
        Me.grpMount.TabIndex = 55
        Me.grpMount.TabStop = False
        Me.grpMount.Text = "Mount"
        '
        'LblMountPierSide
        '
        Me.LblMountPierSide.AutoSize = True
        Me.LblMountPierSide.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMountPierSide.Location = New System.Drawing.Point(3, 130)
        Me.LblMountPierSide.Name = "LblMountPierSide"
        Me.LblMountPierSide.Size = New System.Drawing.Size(46, 13)
        Me.LblMountPierSide.TabIndex = 72
        Me.LblMountPierSide.Text = "PierSide"
        '
        'LblMountRADEC
        '
        Me.LblMountRADEC.AutoSize = True
        Me.LblMountRADEC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMountRADEC.Location = New System.Drawing.Point(3, 113)
        Me.LblMountRADEC.Name = "LblMountRADEC"
        Me.LblMountRADEC.Size = New System.Drawing.Size(44, 13)
        Me.LblMountRADEC.TabIndex = 71
        Me.LblMountRADEC.Text = "RADEC"
        '
        'LblMountStatus
        '
        Me.LblMountStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblMountStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMountStatus.Location = New System.Drawing.Point(7, 16)
        Me.LblMountStatus.Name = "LblMountStatus"
        Me.LblMountStatus.Size = New System.Drawing.Size(185, 30)
        Me.LblMountStatus.TabIndex = 28
        Me.LblMountStatus.Text = "Tracking"
        Me.LblMountStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblMountAz
        '
        Me.lblMountAz.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblMountAz.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMountAz.Location = New System.Drawing.Point(7, 81)
        Me.lblMountAz.Name = "lblMountAz"
        Me.lblMountAz.Size = New System.Drawing.Size(185, 30)
        Me.lblMountAz.TabIndex = 27
        Me.lblMountAz.Text = "Azimuth"
        Me.lblMountAz.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ChkAutoStart
        '
        Me.ChkAutoStart.AutoSize = True
        Me.ChkAutoStart.Location = New System.Drawing.Point(2, 95)
        Me.ChkAutoStart.Name = "ChkAutoStart"
        Me.ChkAutoStart.Size = New System.Drawing.Size(119, 17)
        Me.ChkAutoStart.TabIndex = 57
        Me.ChkAutoStart.Text = "Autostart Enabled ?"
        Me.ChkAutoStart.UseVisualStyleBackColor = True
        '
        'grpSun
        '
        Me.grpSun.Controls.Add(Me.LblSunSettingRising)
        Me.grpSun.Controls.Add(Me.LblDECSun)
        Me.grpSun.Controls.Add(Me.LblRisingSettingSun)
        Me.grpSun.Controls.Add(Me.LblRASun)
        Me.grpSun.Controls.Add(Me.LblAltSun)
        Me.grpSun.Location = New System.Drawing.Point(3, 156)
        Me.grpSun.Name = "grpSun"
        Me.grpSun.Size = New System.Drawing.Size(132, 118)
        Me.grpSun.TabIndex = 58
        Me.grpSun.TabStop = False
        Me.grpSun.Text = "Sun"
        '
        'LblSunSettingRising
        '
        Me.LblSunSettingRising.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblSunSettingRising.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSunSettingRising.Location = New System.Drawing.Point(3, 40)
        Me.LblSunSettingRising.Name = "LblSunSettingRising"
        Me.LblSunSettingRising.Size = New System.Drawing.Size(123, 26)
        Me.LblSunSettingRising.TabIndex = 6
        Me.LblSunSettingRising.Text = "LblSunSettingRising"
        Me.LblSunSettingRising.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblDECSun
        '
        Me.LblDECSun.AutoSize = True
        Me.LblDECSun.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDECSun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblDECSun.Location = New System.Drawing.Point(3, 84)
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
        Me.LblRisingSettingSun.Location = New System.Drawing.Point(3, 100)
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
        Me.LblRASun.Location = New System.Drawing.Point(3, 67)
        Me.LblRASun.Name = "LblRASun"
        Me.LblRASun.Size = New System.Drawing.Size(55, 13)
        Me.LblRASun.TabIndex = 1
        Me.LblRASun.Text = "LblRASun"
        '
        'LblAltSun
        '
        Me.LblAltSun.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblAltSun.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAltSun.Location = New System.Drawing.Point(3, 13)
        Me.LblAltSun.Name = "LblAltSun"
        Me.LblAltSun.Size = New System.Drawing.Size(123, 26)
        Me.LblAltSun.TabIndex = 0
        Me.LblAltSun.Text = "LblAltSun"
        Me.LblAltSun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpMoon
        '
        Me.grpMoon.Controls.Add(Me.LblMoonSettingRising)
        Me.grpMoon.Controls.Add(Me.LblDECMoon)
        Me.grpMoon.Controls.Add(Me.LblPhaseMoon)
        Me.grpMoon.Controls.Add(Me.LblRisingSettingMoon)
        Me.grpMoon.Controls.Add(Me.LblAltMoon)
        Me.grpMoon.Controls.Add(Me.LblRAMoon)
        Me.grpMoon.Location = New System.Drawing.Point(3, 7)
        Me.grpMoon.Name = "grpMoon"
        Me.grpMoon.Size = New System.Drawing.Size(132, 149)
        Me.grpMoon.TabIndex = 59
        Me.grpMoon.TabStop = False
        Me.grpMoon.Text = "Moon"
        '
        'LblMoonSettingRising
        '
        Me.LblMoonSettingRising.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblMoonSettingRising.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMoonSettingRising.Location = New System.Drawing.Point(3, 68)
        Me.LblMoonSettingRising.Name = "LblMoonSettingRising"
        Me.LblMoonSettingRising.Size = New System.Drawing.Size(123, 26)
        Me.LblMoonSettingRising.TabIndex = 10
        Me.LblMoonSettingRising.Text = "LblMoonSettingRising"
        Me.LblMoonSettingRising.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblDECMoon
        '
        Me.LblDECMoon.AutoSize = True
        Me.LblDECMoon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDECMoon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblDECMoon.Location = New System.Drawing.Point(3, 115)
        Me.LblDECMoon.Name = "LblDECMoon"
        Me.LblDECMoon.Size = New System.Drawing.Size(70, 13)
        Me.LblDECMoon.TabIndex = 9
        Me.LblDECMoon.Text = "LblDECMoon"
        '
        'LblPhaseMoon
        '
        Me.LblPhaseMoon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblPhaseMoon.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPhaseMoon.Location = New System.Drawing.Point(3, 41)
        Me.LblPhaseMoon.Name = "LblPhaseMoon"
        Me.LblPhaseMoon.Size = New System.Drawing.Size(123, 26)
        Me.LblPhaseMoon.TabIndex = 8
        Me.LblPhaseMoon.Text = "LblPhaseMoon"
        Me.LblPhaseMoon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblRisingSettingMoon
        '
        Me.LblRisingSettingMoon.AutoSize = True
        Me.LblRisingSettingMoon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRisingSettingMoon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblRisingSettingMoon.Location = New System.Drawing.Point(3, 133)
        Me.LblRisingSettingMoon.Name = "LblRisingSettingMoon"
        Me.LblRisingSettingMoon.Size = New System.Drawing.Size(110, 13)
        Me.LblRisingSettingMoon.TabIndex = 7
        Me.LblRisingSettingMoon.Text = "LblRisingSettingMoon"
        '
        'LblAltMoon
        '
        Me.LblAltMoon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblAltMoon.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAltMoon.Location = New System.Drawing.Point(3, 14)
        Me.LblAltMoon.Name = "LblAltMoon"
        Me.LblAltMoon.Size = New System.Drawing.Size(123, 26)
        Me.LblAltMoon.TabIndex = 2
        Me.LblAltMoon.Text = "LblAltMoon"
        Me.LblAltMoon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblRAMoon
        '
        Me.LblRAMoon.AutoSize = True
        Me.LblRAMoon.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRAMoon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblRAMoon.Location = New System.Drawing.Point(3, 97)
        Me.LblRAMoon.Name = "LblRAMoon"
        Me.LblRAMoon.Size = New System.Drawing.Size(63, 13)
        Me.LblRAMoon.TabIndex = 1
        Me.LblRAMoon.Text = "LblRAMoon"
        '
        'TimerCheckCycle
        '
        Me.TimerCheckCycle.Interval = 10000
        '
        'ChkSimulatorMode
        '
        Me.ChkSimulatorMode.AutoSize = True
        Me.ChkSimulatorMode.Location = New System.Drawing.Point(563, 562)
        Me.ChkSimulatorMode.Name = "ChkSimulatorMode"
        Me.ChkSimulatorMode.Size = New System.Drawing.Size(98, 17)
        Me.ChkSimulatorMode.TabIndex = 64
        Me.ChkSimulatorMode.Text = "Simulator mode"
        Me.ChkSimulatorMode.UseVisualStyleBackColor = True
        '
        'TimerRoof
        '
        Me.TimerRoof.Interval = 2300
        '
        'LblCover
        '
        Me.LblCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblCover.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCover.Location = New System.Drawing.Point(353, 5)
        Me.LblCover.Name = "LblCover"
        Me.LblCover.Size = New System.Drawing.Size(104, 26)
        Me.LblCover.TabIndex = 65
        Me.LblCover.Text = "COVER"
        Me.LblCover.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TimerCover
        '
        Me.TimerCover.Interval = 2200
        '
        'LblSwitch
        '
        Me.LblSwitch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblSwitch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSwitch.Location = New System.Drawing.Point(248, 5)
        Me.LblSwitch.Name = "LblSwitch"
        Me.LblSwitch.Size = New System.Drawing.Size(104, 26)
        Me.LblSwitch.TabIndex = 66
        Me.LblSwitch.Text = "SWITCH"
        Me.LblSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TimerSwitch
        '
        Me.TimerSwitch.Interval = 2100
        '
        'TimerWeather
        '
        Me.TimerWeather.Interval = 2900
        '
        'LblLST
        '
        Me.LblLST.AutoSize = True
        Me.LblLST.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLST.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblLST.Location = New System.Drawing.Point(6, 13)
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
        Me.LblGST.Location = New System.Drawing.Point(6, 30)
        Me.LblGST.Name = "LblGST"
        Me.LblGST.Size = New System.Drawing.Size(43, 13)
        Me.LblGST.TabIndex = 69
        Me.LblGST.Text = "LblGST"
        '
        'GroupBox
        '
        Me.GroupBox.Controls.Add(Me.grpMoon)
        Me.GroupBox.Controls.Add(Me.grpSun)
        Me.GroupBox.Location = New System.Drawing.Point(2, 305)
        Me.GroupBox.Name = "GroupBox"
        Me.GroupBox.Size = New System.Drawing.Size(139, 277)
        Me.GroupBox.TabIndex = 70
        Me.GroupBox.TabStop = False
        '
        'GroupBoxTwilight
        '
        Me.GroupBoxTwilight.Controls.Add(Me.LblCivilTwilight)
        Me.GroupBoxTwilight.Controls.Add(Me.LblAstronomicalTwilight)
        Me.GroupBoxTwilight.Controls.Add(Me.LblAmateurTwilight)
        Me.GroupBoxTwilight.Location = New System.Drawing.Point(3, 48)
        Me.GroupBoxTwilight.Name = "GroupBoxTwilight"
        Me.GroupBoxTwilight.Size = New System.Drawing.Size(189, 64)
        Me.GroupBoxTwilight.TabIndex = 74
        Me.GroupBoxTwilight.TabStop = False
        Me.GroupBoxTwilight.Text = "Twilight"
        '
        'LblCivilTwilight
        '
        Me.LblCivilTwilight.AutoSize = True
        Me.LblCivilTwilight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCivilTwilight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblCivilTwilight.Location = New System.Drawing.Point(3, 16)
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
        Me.LblAstronomicalTwilight.Location = New System.Drawing.Point(3, 46)
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
        Me.LblAmateurTwilight.Location = New System.Drawing.Point(3, 31)
        Me.LblAmateurTwilight.Name = "LblAmateurTwilight"
        Me.LblAmateurTwilight.Size = New System.Drawing.Size(96, 13)
        Me.LblAmateurTwilight.TabIndex = 72
        Me.LblAmateurTwilight.Text = "LblAmateurTwilight"
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
        'RTXErrors
        '
        Me.RTXErrors.Location = New System.Drawing.Point(143, 503)
        Me.RTXErrors.Name = "RTXErrors"
        Me.RTXErrors.Size = New System.Drawing.Size(629, 51)
        Me.RTXErrors.TabIndex = 54
        Me.RTXErrors.Text = ""
        '
        'TimerDisaster
        '
        Me.TimerDisaster.Enabled = True
        Me.TimerDisaster.Interval = 120000
        '
        'GrpFocus
        '
        Me.GrpFocus.Controls.Add(Me.LblLastFocusDateTime)
        Me.GrpFocus.Controls.Add(Me.LblLastFocusTemperature)
        Me.GrpFocus.Controls.Add(Me.LblFocusserTemperature)
        Me.GrpFocus.Controls.Add(Me.LblFocusserPosition)
        Me.GrpFocus.Location = New System.Drawing.Point(777, 235)
        Me.GrpFocus.Name = "GrpFocus"
        Me.GrpFocus.Size = New System.Drawing.Size(198, 114)
        Me.GrpFocus.TabIndex = 71
        Me.GrpFocus.TabStop = False
        Me.GrpFocus.Text = "Focusser"
        '
        'LblLastFocusDateTime
        '
        Me.LblLastFocusDateTime.AutoSize = True
        Me.LblLastFocusDateTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLastFocusDateTime.Location = New System.Drawing.Point(5, 93)
        Me.LblLastFocusDateTime.Name = "LblLastFocusDateTime"
        Me.LblLastFocusDateTime.Size = New System.Drawing.Size(56, 13)
        Me.LblLastFocusDateTime.TabIndex = 29
        Me.LblLastFocusDateTime.Text = "Last focus"
        '
        'LblLastFocusTemperature
        '
        Me.LblLastFocusTemperature.AutoSize = True
        Me.LblLastFocusTemperature.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLastFocusTemperature.Location = New System.Drawing.Point(5, 79)
        Me.LblLastFocusTemperature.Name = "LblLastFocusTemperature"
        Me.LblLastFocusTemperature.Size = New System.Drawing.Size(86, 13)
        Me.LblLastFocusTemperature.TabIndex = 28
        Me.LblLastFocusTemperature.Text = "Last temperature"
        '
        'LblFocusserTemperature
        '
        Me.LblFocusserTemperature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblFocusserTemperature.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFocusserTemperature.Location = New System.Drawing.Point(7, 47)
        Me.LblFocusserTemperature.Name = "LblFocusserTemperature"
        Me.LblFocusserTemperature.Size = New System.Drawing.Size(185, 30)
        Me.LblFocusserTemperature.TabIndex = 27
        Me.LblFocusserTemperature.Text = "Temp"
        Me.LblFocusserTemperature.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblFocusserPosition
        '
        Me.LblFocusserPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblFocusserPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFocusserPosition.Location = New System.Drawing.Point(7, 15)
        Me.LblFocusserPosition.Name = "LblFocusserPosition"
        Me.LblFocusserPosition.Size = New System.Drawing.Size(185, 30)
        Me.LblFocusserPosition.TabIndex = 26
        Me.LblFocusserPosition.Text = "Position"
        Me.LblFocusserPosition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ChkDisableSafetyCheck
        '
        Me.ChkDisableSafetyCheck.AutoSize = True
        Me.ChkDisableSafetyCheck.Location = New System.Drawing.Point(423, 562)
        Me.ChkDisableSafetyCheck.Name = "ChkDisableSafetyCheck"
        Me.ChkDisableSafetyCheck.Size = New System.Drawing.Size(134, 17)
        Me.ChkDisableSafetyCheck.TabIndex = 72
        Me.ChkDisableSafetyCheck.Text = "Disable safety check ?"
        Me.ChkDisableSafetyCheck.UseVisualStyleBackColor = True
        '
        'BtnClearLog
        '
        Me.BtnClearLog.Location = New System.Drawing.Point(233, 558)
        Me.BtnClearLog.Name = "BtnClearLog"
        Me.BtnClearLog.Size = New System.Drawing.Size(89, 22)
        Me.BtnClearLog.TabIndex = 73
        Me.BtnClearLog.Text = "Clear Log"
        Me.BtnClearLog.UseVisualStyleBackColor = True
        '
        'BtnClearErrorLog
        '
        Me.BtnClearErrorLog.Location = New System.Drawing.Point(323, 558)
        Me.BtnClearErrorLog.Name = "BtnClearErrorLog"
        Me.BtnClearErrorLog.Size = New System.Drawing.Size(89, 22)
        Me.BtnClearErrorLog.TabIndex = 74
        Me.BtnClearErrorLog.Text = "Clear Error Log"
        Me.BtnClearErrorLog.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.LblCCDFilter)
        Me.GroupBox1.Controls.Add(Me.LblCCDTemp)
        Me.GroupBox1.Controls.Add(Me.LblCCDExposureStatus)
        Me.GroupBox1.Location = New System.Drawing.Point(777, 148)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(198, 81)
        Me.GroupBox1.TabIndex = 75
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "CCD"
        '
        'LblCCDFilter
        '
        Me.LblCCDFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblCCDFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold)
        Me.LblCCDFilter.Location = New System.Drawing.Point(7, 45)
        Me.LblCCDFilter.Name = "LblCCDFilter"
        Me.LblCCDFilter.Size = New System.Drawing.Size(48, 30)
        Me.LblCCDFilter.TabIndex = 29
        Me.LblCCDFilter.Text = "L"
        Me.LblCCDFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblCCDTemp
        '
        Me.LblCCDTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblCCDTemp.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCCDTemp.Location = New System.Drawing.Point(61, 45)
        Me.LblCCDTemp.Name = "LblCCDTemp"
        Me.LblCCDTemp.Size = New System.Drawing.Size(131, 30)
        Me.LblCCDTemp.TabIndex = 28
        Me.LblCCDTemp.Text = "Temp"
        Me.LblCCDTemp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblCCDExposureStatus
        '
        Me.LblCCDExposureStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblCCDExposureStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold)
        Me.LblCCDExposureStatus.Location = New System.Drawing.Point(7, 13)
        Me.LblCCDExposureStatus.Name = "LblCCDExposureStatus"
        Me.LblCCDExposureStatus.Size = New System.Drawing.Size(185, 30)
        Me.LblCCDExposureStatus.TabIndex = 26
        Me.LblCCDExposureStatus.Text = "Exposure"
        Me.LblCCDExposureStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        'BtnViewLog
        '
        Me.BtnViewLog.Location = New System.Drawing.Point(143, 558)
        Me.BtnViewLog.Name = "BtnViewLog"
        Me.BtnViewLog.Size = New System.Drawing.Size(89, 22)
        Me.BtnViewLog.TabIndex = 76
        Me.BtnViewLog.Text = "View Log"
        Me.BtnViewLog.UseVisualStyleBackColor = True
        '
        'LblVersion
        '
        Me.LblVersion.AutoSize = True
        Me.LblVersion.Location = New System.Drawing.Point(676, 564)
        Me.LblVersion.Name = "LblVersion"
        Me.LblVersion.Size = New System.Drawing.Size(66, 13)
        Me.LblVersion.TabIndex = 77
        Me.LblVersion.Text = "Version 2.10"
        Me.LblVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'LblHang
        '
        Me.LblHang.AutoSize = True
        Me.LblHang.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblHang.Location = New System.Drawing.Point(747, 562)
        Me.LblHang.Name = "LblHang"
        Me.LblHang.Size = New System.Drawing.Size(16, 15)
        Me.LblHang.TabIndex = 78
        Me.LblHang.Text = "X"
        '
        'TimerHang
        '
        Me.TimerHang.Enabled = True
        Me.TimerHang.Interval = 250
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.LblSunDawnFlats)
        Me.GroupBox2.Controls.Add(Me.LblSunDuskFlats)
        Me.GroupBox2.Controls.Add(Me.LblSunStopRun)
        Me.GroupBox2.Controls.Add(Me.LblSunStartRun)
        Me.GroupBox2.Controls.Add(Me.LblSunOpenRoof)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 114)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(189, 94)
        Me.GroupBox2.TabIndex = 79
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Run times"
        '
        'LblSunDawnFlats
        '
        Me.LblSunDawnFlats.AutoSize = True
        Me.LblSunDawnFlats.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSunDawnFlats.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblSunDawnFlats.Location = New System.Drawing.Point(3, 77)
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
        Me.LblSunDuskFlats.Location = New System.Drawing.Point(3, 31)
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
        Me.LblSunStopRun.Location = New System.Drawing.Point(3, 62)
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
        Me.LblSunStartRun.Location = New System.Drawing.Point(3, 46)
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
        Me.LblSunOpenRoof.Location = New System.Drawing.Point(3, 16)
        Me.LblSunOpenRoof.Name = "LblSunOpenRoof"
        Me.LblSunOpenRoof.Size = New System.Drawing.Size(89, 13)
        Me.LblSunOpenRoof.TabIndex = 71
        Me.LblSunOpenRoof.Text = "LblSunOpenRoof"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.GroupBoxTwilight)
        Me.GroupBox3.Controls.Add(Me.GroupBox2)
        Me.GroupBox3.Controls.Add(Me.LblLST)
        Me.GroupBox3.Controls.Add(Me.LblGST)
        Me.GroupBox3.Location = New System.Drawing.Point(777, 348)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(198, 234)
        Me.GroupBox3.TabIndex = 80
        Me.GroupBox3.TabStop = False
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(978, 585)
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.LblHang)
        Me.Controls.Add(Me.LblVersion)
        Me.Controls.Add(Me.BtnViewLog)
        Me.Controls.Add(Me.LblUPSCyberPower)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.BtnClearErrorLog)
        Me.Controls.Add(Me.BtnClearLog)
        Me.Controls.Add(Me.ChkDisableSafetyCheck)
        Me.Controls.Add(Me.GrpFocus)
        Me.Controls.Add(Me.GroupBox)
        Me.Controls.Add(Me.LblSwitch)
        Me.Controls.Add(Me.LblCover)
        Me.Controls.Add(Me.ChkSimulatorMode)
        Me.Controls.Add(Me.ChkAutoStart)
        Me.Controls.Add(Me.RTXLog)
        Me.Controls.Add(Me.LblRoof)
        Me.Controls.Add(Me.LblInternet)
        Me.Controls.Add(Me.LblTSX)
        Me.Controls.Add(Me.BtnStop)
        Me.Controls.Add(Me.BtnStart)
        Me.Controls.Add(Me.LblMonitorStatus)
        Me.Controls.Add(Me.grpMount)
        Me.Controls.Add(Me.RTXErrors)
        Me.Controls.Add(Me.grpAAG)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Robotic observatory"
        Me.grpAAG.ResumeLayout(False)
        Me.grpAAG.PerformLayout()
        Me.grpMount.ResumeLayout(False)
        Me.grpMount.PerformLayout()
        Me.grpSun.ResumeLayout(False)
        Me.grpSun.PerformLayout()
        Me.grpMoon.ResumeLayout(False)
        Me.grpMoon.PerformLayout()
        Me.GroupBox.ResumeLayout(False)
        Me.GroupBoxTwilight.ResumeLayout(False)
        Me.GroupBoxTwilight.PerformLayout()
        Me.GrpFocus.ResumeLayout(False)
        Me.GrpFocus.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LblUPSCyberPower As Label
    Friend WithEvents RTXLog As RichTextBox
    Friend WithEvents LblRoof As Label
    Friend WithEvents grpAAG As GroupBox
    Friend WithEvents LblBlink As Label
    Friend WithEvents txtRelativeHumidity As TextBox
    Friend WithEvents lblRain As Label
    Friend WithEvents lblLight As Label
    Friend WithEvents lblCloud As Label
    Friend WithEvents txtAmbientTemperature As TextBox
    Friend WithEvents lblLastRead As Label
    Friend WithEvents lblRelativeHumidity As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblSafe As Label
    Friend WithEvents LblInternet As Label
    Friend WithEvents LblTSX As Label
    Friend WithEvents BtnStop As Button
    Friend WithEvents BtnStart As Button
    Friend WithEvents LblMonitorStatus As Label
    Friend WithEvents lblMountAlt As Label
    Friend WithEvents grpMount As GroupBox
    Friend WithEvents lblMountAz As Label
    Friend WithEvents ChkAutoStart As CheckBox
    Friend WithEvents grpSun As GroupBox
    Friend WithEvents LblRisingSettingSun As Label
    Friend WithEvents LblRASun As Label
    Friend WithEvents LblAltSun As Label
    Friend WithEvents grpMoon As GroupBox
    Friend WithEvents LblPhaseMoon As Label
    Friend WithEvents LblRisingSettingMoon As Label
    Friend WithEvents LblAltMoon As Label
    Friend WithEvents LblRAMoon As Label
    Friend WithEvents TimerCheckCycle As Timer
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents ChkSimulatorMode As CheckBox
    Friend WithEvents TimerRoof As Timer
    Friend WithEvents LblCover As Label
    Friend WithEvents TimerCover As Timer
    Friend WithEvents LblSwitch As Label
    Friend WithEvents TimerSwitch As Timer
    Friend WithEvents TimerWeather As Timer
    Friend WithEvents LblLST As Label
    Friend WithEvents LblGST As Label
    Friend WithEvents GroupBox As GroupBox
    Friend WithEvents LblDECMoon As Label
    Friend WithEvents LblDECSun As Label
    Friend WithEvents LblAstronomicalTwilight As Label
    Friend WithEvents LblAmateurTwilight As Label
    Friend WithEvents LblCivilTwilight As Label
    Friend WithEvents GroupBoxTwilight As GroupBox
    Friend WithEvents TimerStartRun As Timer
    Friend WithEvents TimerMount As Timer
    Friend WithEvents LblMountStatus As Label
    Friend WithEvents LblMountRADEC As Label
    Friend WithEvents LblMountPierSide As Label
    Friend WithEvents TimerCCD As Timer
    Friend WithEvents LblSunSettingRising As Label
    Friend WithEvents LblMoonSettingRising As Label
    Friend WithEvents RTXErrors As RichTextBox
    Friend WithEvents TimerDisaster As Timer
    Friend WithEvents GrpFocus As GroupBox
    Friend WithEvents LblFocusserTemperature As Label
    Friend WithEvents LblFocusserPosition As Label
    Friend WithEvents ChkDisableSafetyCheck As CheckBox
    Friend WithEvents BtnClearLog As Button
    Friend WithEvents BtnClearErrorLog As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents LblCCDExposureStatus As Label
    Friend WithEvents TimerColorStatus As Timer
    Friend WithEvents TimerSmartError As Timer
    Friend WithEvents TimerHeartBeat As Timer
    Friend WithEvents LblLastFocusDateTime As Label
    Friend WithEvents LblLastFocusTemperature As Label
    Friend WithEvents BtnViewLog As Button
    Friend WithEvents LblCloudSafe As Label
    Friend WithEvents LblVersion As Label
    Friend WithEvents LblHang As Label
    Friend WithEvents TimerHang As Timer
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents LblSunStopRun As Label
    Friend WithEvents LblSunStartRun As Label
    Friend WithEvents LblSunOpenRoof As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents LblSunDawnFlats As Label
    Friend WithEvents LblSunDuskFlats As Label
    Friend WithEvents LblCCDTemp As Label
    Friend WithEvents LblCCDFilter As Label
End Class
