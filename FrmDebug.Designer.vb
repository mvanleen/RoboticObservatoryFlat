﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmDebug
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
        Me.ButtonKillInternet = New System.Windows.Forms.Button()
        Me.ButtonEnableInternet = New System.Windows.Forms.Button()
        Me.ButtonEnableUPS = New System.Windows.Forms.Button()
        Me.ButtonKillUPS = New System.Windows.Forms.Button()
        Me.BtnSAFE = New System.Windows.Forms.Button()
        Me.BtnUNSAFE = New System.Windows.Forms.Button()
        Me.BtnDARK = New System.Windows.Forms.Button()
        Me.BtnLIGHT = New System.Windows.Forms.Button()
        Me.TxtSunAltitude = New System.Windows.Forms.TextBox()
        Me.TxtMoonAltitude = New System.Windows.Forms.TextBox()
        Me.CmbSunSettingRising = New System.Windows.Forms.ComboBox()
        Me.CmbMoonSettingRising = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ChkDebugCoordinates = New System.Windows.Forms.CheckBox()
        Me.ChkDebugAAGData = New System.Windows.Forms.CheckBox()
        Me.ChkRunStatus = New System.Windows.Forms.CheckBox()
        Me.ButtonKillAAG = New System.Windows.Forms.Button()
        Me.ButtonEnableAAG = New System.Windows.Forms.Button()
        Me.ChkISSequenceRunning = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TxtMoonIllumination = New System.Windows.Forms.TextBox()
        Me.LblpIsActionRunning = New System.Windows.Forms.Label()
        Me.LblpIsSequenceRunning = New System.Windows.Forms.Label()
        Me.LblpIsEquipmentInitializing = New System.Windows.Forms.Label()
        Me.LblpEquipmentStatus = New System.Windows.Forms.Label()
        Me.LblpRunStatus = New System.Windows.Forms.Label()
        Me.LblpManualAbort = New System.Windows.Forms.Label()
        Me.LblpAbort = New System.Windows.Forms.Label()
        Me.GrpStatus = New System.Windows.Forms.GroupBox()
        Me.LblpMoonCooldownStatus = New System.Windows.Forms.Label()
        Me.LblMoonSafetyStatus = New System.Windows.Forms.Label()
        Me.lblpOldSequenceTimeSMART = New System.Windows.Forms.Label()
        Me.LblpOldProgramTimeSMART = New System.Windows.Forms.Label()
        Me.LblpSmartError = New System.Windows.Forms.Label()
        Me.LblStartRun = New System.Windows.Forms.Label()
        Me.TimerDebug = New System.Windows.Forms.Timer(Me.components)
        Me.BtnShowTargets = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TxtMosaicCenterRA = New System.Windows.Forms.TextBox()
        Me.TxtMosaicCenterDEC = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TxtMosaic = New System.Windows.Forms.TextBox()
        Me.CmbMosaicType = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TxtMosaicOverlap = New System.Windows.Forms.TextBox()
        Me.BtnError = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GrpStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonKillInternet
        '
        Me.ButtonKillInternet.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.ButtonKillInternet.FlatAppearance.BorderSize = 0
        Me.ButtonKillInternet.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.ButtonKillInternet.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonKillInternet.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonKillInternet.ForeColor = System.Drawing.Color.White
        Me.ButtonKillInternet.Location = New System.Drawing.Point(3, 3)
        Me.ButtonKillInternet.Name = "ButtonKillInternet"
        Me.ButtonKillInternet.Size = New System.Drawing.Size(91, 23)
        Me.ButtonKillInternet.TabIndex = 0
        Me.ButtonKillInternet.Text = "Kill Internet"
        Me.ButtonKillInternet.UseVisualStyleBackColor = False
        '
        'ButtonEnableInternet
        '
        Me.ButtonEnableInternet.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.ButtonEnableInternet.FlatAppearance.BorderSize = 0
        Me.ButtonEnableInternet.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.ButtonEnableInternet.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonEnableInternet.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEnableInternet.ForeColor = System.Drawing.Color.White
        Me.ButtonEnableInternet.Location = New System.Drawing.Point(3, 32)
        Me.ButtonEnableInternet.Name = "ButtonEnableInternet"
        Me.ButtonEnableInternet.Size = New System.Drawing.Size(91, 23)
        Me.ButtonEnableInternet.TabIndex = 1
        Me.ButtonEnableInternet.Text = "Enable Internet"
        Me.ButtonEnableInternet.UseVisualStyleBackColor = False
        '
        'ButtonEnableUPS
        '
        Me.ButtonEnableUPS.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.ButtonEnableUPS.FlatAppearance.BorderSize = 0
        Me.ButtonEnableUPS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.ButtonEnableUPS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonEnableUPS.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEnableUPS.ForeColor = System.Drawing.Color.White
        Me.ButtonEnableUPS.Location = New System.Drawing.Point(100, 32)
        Me.ButtonEnableUPS.Name = "ButtonEnableUPS"
        Me.ButtonEnableUPS.Size = New System.Drawing.Size(91, 23)
        Me.ButtonEnableUPS.TabIndex = 3
        Me.ButtonEnableUPS.Text = "Enable UPS"
        Me.ButtonEnableUPS.UseVisualStyleBackColor = False
        '
        'ButtonKillUPS
        '
        Me.ButtonKillUPS.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.ButtonKillUPS.FlatAppearance.BorderSize = 0
        Me.ButtonKillUPS.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.ButtonKillUPS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonKillUPS.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonKillUPS.ForeColor = System.Drawing.Color.White
        Me.ButtonKillUPS.Location = New System.Drawing.Point(100, 3)
        Me.ButtonKillUPS.Name = "ButtonKillUPS"
        Me.ButtonKillUPS.Size = New System.Drawing.Size(91, 23)
        Me.ButtonKillUPS.TabIndex = 2
        Me.ButtonKillUPS.Text = "Kill UPS"
        Me.ButtonKillUPS.UseVisualStyleBackColor = False
        '
        'BtnSAFE
        '
        Me.BtnSAFE.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnSAFE.FlatAppearance.BorderSize = 0
        Me.BtnSAFE.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnSAFE.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSAFE.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSAFE.ForeColor = System.Drawing.Color.White
        Me.BtnSAFE.Location = New System.Drawing.Point(3, 178)
        Me.BtnSAFE.Name = "BtnSAFE"
        Me.BtnSAFE.Size = New System.Drawing.Size(91, 23)
        Me.BtnSAFE.TabIndex = 4
        Me.BtnSAFE.Text = "SAFE"
        Me.BtnSAFE.UseVisualStyleBackColor = False
        '
        'BtnUNSAFE
        '
        Me.BtnUNSAFE.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnUNSAFE.FlatAppearance.BorderSize = 0
        Me.BtnUNSAFE.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnUNSAFE.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnUNSAFE.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnUNSAFE.ForeColor = System.Drawing.Color.White
        Me.BtnUNSAFE.Location = New System.Drawing.Point(3, 207)
        Me.BtnUNSAFE.Name = "BtnUNSAFE"
        Me.BtnUNSAFE.Size = New System.Drawing.Size(91, 23)
        Me.BtnUNSAFE.TabIndex = 5
        Me.BtnUNSAFE.Text = "UNSAFE"
        Me.BtnUNSAFE.UseVisualStyleBackColor = False
        '
        'BtnDARK
        '
        Me.BtnDARK.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnDARK.FlatAppearance.BorderSize = 0
        Me.BtnDARK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnDARK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnDARK.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDARK.ForeColor = System.Drawing.Color.White
        Me.BtnDARK.Location = New System.Drawing.Point(100, 178)
        Me.BtnDARK.Name = "BtnDARK"
        Me.BtnDARK.Size = New System.Drawing.Size(91, 23)
        Me.BtnDARK.TabIndex = 6
        Me.BtnDARK.Text = "DARK"
        Me.BtnDARK.UseVisualStyleBackColor = False
        '
        'BtnLIGHT
        '
        Me.BtnLIGHT.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnLIGHT.FlatAppearance.BorderSize = 0
        Me.BtnLIGHT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnLIGHT.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnLIGHT.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnLIGHT.ForeColor = System.Drawing.Color.White
        Me.BtnLIGHT.Location = New System.Drawing.Point(100, 207)
        Me.BtnLIGHT.Name = "BtnLIGHT"
        Me.BtnLIGHT.Size = New System.Drawing.Size(91, 23)
        Me.BtnLIGHT.TabIndex = 7
        Me.BtnLIGHT.Text = "LIGHT"
        Me.BtnLIGHT.UseVisualStyleBackColor = False
        '
        'TxtSunAltitude
        '
        Me.TxtSunAltitude.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSunAltitude.Location = New System.Drawing.Point(40, 93)
        Me.TxtSunAltitude.Name = "TxtSunAltitude"
        Me.TxtSunAltitude.Size = New System.Drawing.Size(32, 21)
        Me.TxtSunAltitude.TabIndex = 8
        '
        'TxtMoonAltitude
        '
        Me.TxtMoonAltitude.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMoonAltitude.Location = New System.Drawing.Point(40, 117)
        Me.TxtMoonAltitude.Name = "TxtMoonAltitude"
        Me.TxtMoonAltitude.Size = New System.Drawing.Size(32, 21)
        Me.TxtMoonAltitude.TabIndex = 9
        '
        'CmbSunSettingRising
        '
        Me.CmbSunSettingRising.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbSunSettingRising.FormattingEnabled = True
        Me.CmbSunSettingRising.Items.AddRange(New Object() {"SETTING", "RISING"})
        Me.CmbSunSettingRising.Location = New System.Drawing.Point(100, 93)
        Me.CmbSunSettingRising.Name = "CmbSunSettingRising"
        Me.CmbSunSettingRising.Size = New System.Drawing.Size(91, 21)
        Me.CmbSunSettingRising.TabIndex = 10
        '
        'CmbMoonSettingRising
        '
        Me.CmbMoonSettingRising.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbMoonSettingRising.FormattingEnabled = True
        Me.CmbMoonSettingRising.Items.AddRange(New Object() {"SETTING", "RISING"})
        Me.CmbMoonSettingRising.Location = New System.Drawing.Point(100, 117)
        Me.CmbMoonSettingRising.Name = "CmbMoonSettingRising"
        Me.CmbMoonSettingRising.Size = New System.Drawing.Size(91, 21)
        Me.CmbMoonSettingRising.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(3, 96)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Sun:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(3, 120)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Moon:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(73, 96)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(28, 13)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "° Alt"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(73, 120)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(28, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "° Alt"
        '
        'ChkDebugCoordinates
        '
        Me.ChkDebugCoordinates.AutoSize = True
        Me.ChkDebugCoordinates.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkDebugCoordinates.ForeColor = System.Drawing.Color.White
        Me.ChkDebugCoordinates.Location = New System.Drawing.Point(7, 70)
        Me.ChkDebugCoordinates.Name = "ChkDebugCoordinates"
        Me.ChkDebugCoordinates.Size = New System.Drawing.Size(136, 17)
        Me.ChkDebugCoordinates.TabIndex = 16
        Me.ChkDebugCoordinates.Text = "Use debug coordinates"
        Me.ChkDebugCoordinates.UseVisualStyleBackColor = True
        '
        'ChkDebugAAGData
        '
        Me.ChkDebugAAGData.AutoSize = True
        Me.ChkDebugAAGData.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkDebugAAGData.ForeColor = System.Drawing.Color.White
        Me.ChkDebugAAGData.Location = New System.Drawing.Point(3, 155)
        Me.ChkDebugAAGData.Name = "ChkDebugAAGData"
        Me.ChkDebugAAGData.Size = New System.Drawing.Size(169, 17)
        Me.ChkDebugAAGData.TabIndex = 17
        Me.ChkDebugAAGData.Text = "Use debug AAG weather data"
        Me.ChkDebugAAGData.UseVisualStyleBackColor = True
        '
        'ChkRunStatus
        '
        Me.ChkRunStatus.AutoSize = True
        Me.ChkRunStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkRunStatus.ForeColor = System.Drawing.Color.White
        Me.ChkRunStatus.Location = New System.Drawing.Point(3, 245)
        Me.ChkRunStatus.Name = "ChkRunStatus"
        Me.ChkRunStatus.Size = New System.Drawing.Size(206, 17)
        Me.ChkRunStatus.TabIndex = 18
        Me.ChkRunStatus.Text = "Run status: done = nothing left to do"
        Me.ChkRunStatus.UseVisualStyleBackColor = True
        '
        'ButtonKillAAG
        '
        Me.ButtonKillAAG.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.ButtonKillAAG.FlatAppearance.BorderSize = 0
        Me.ButtonKillAAG.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.ButtonKillAAG.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonKillAAG.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonKillAAG.ForeColor = System.Drawing.Color.White
        Me.ButtonKillAAG.Location = New System.Drawing.Point(197, 3)
        Me.ButtonKillAAG.Name = "ButtonKillAAG"
        Me.ButtonKillAAG.Size = New System.Drawing.Size(91, 23)
        Me.ButtonKillAAG.TabIndex = 19
        Me.ButtonKillAAG.Text = "Kill AAG"
        Me.ButtonKillAAG.UseVisualStyleBackColor = False
        '
        'ButtonEnableAAG
        '
        Me.ButtonEnableAAG.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.ButtonEnableAAG.FlatAppearance.BorderSize = 0
        Me.ButtonEnableAAG.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.ButtonEnableAAG.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonEnableAAG.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEnableAAG.ForeColor = System.Drawing.Color.White
        Me.ButtonEnableAAG.Location = New System.Drawing.Point(197, 32)
        Me.ButtonEnableAAG.Name = "ButtonEnableAAG"
        Me.ButtonEnableAAG.Size = New System.Drawing.Size(91, 23)
        Me.ButtonEnableAAG.TabIndex = 20
        Me.ButtonEnableAAG.Text = "Enable AAG"
        Me.ButtonEnableAAG.UseVisualStyleBackColor = False
        '
        'ChkISSequenceRunning
        '
        Me.ChkISSequenceRunning.AutoSize = True
        Me.ChkISSequenceRunning.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkISSequenceRunning.ForeColor = System.Drawing.Color.White
        Me.ChkISSequenceRunning.Location = New System.Drawing.Point(166, 70)
        Me.ChkISSequenceRunning.Name = "ChkISSequenceRunning"
        Me.ChkISSequenceRunning.Size = New System.Drawing.Size(123, 17)
        Me.ChkISSequenceRunning.TabIndex = 23
        Me.ChkISSequenceRunning.Text = "Is sequence running"
        Me.ChkISSequenceRunning.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(230, 123)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(75, 13)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "Illumination %"
        '
        'TxtMoonIllumination
        '
        Me.TxtMoonIllumination.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMoonIllumination.Location = New System.Drawing.Point(197, 117)
        Me.TxtMoonIllumination.Name = "TxtMoonIllumination"
        Me.TxtMoonIllumination.Size = New System.Drawing.Size(32, 21)
        Me.TxtMoonIllumination.TabIndex = 24
        '
        'LblpIsActionRunning
        '
        Me.LblpIsActionRunning.AutoSize = True
        Me.LblpIsActionRunning.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblpIsActionRunning.ForeColor = System.Drawing.Color.White
        Me.LblpIsActionRunning.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblpIsActionRunning.Location = New System.Drawing.Point(6, 58)
        Me.LblpIsActionRunning.Name = "LblpIsActionRunning"
        Me.LblpIsActionRunning.Size = New System.Drawing.Size(91, 13)
        Me.LblpIsActionRunning.TabIndex = 94
        Me.LblpIsActionRunning.Text = "pIsActionRunning"
        '
        'LblpIsSequenceRunning
        '
        Me.LblpIsSequenceRunning.AutoSize = True
        Me.LblpIsSequenceRunning.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblpIsSequenceRunning.ForeColor = System.Drawing.Color.White
        Me.LblpIsSequenceRunning.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblpIsSequenceRunning.Location = New System.Drawing.Point(6, 36)
        Me.LblpIsSequenceRunning.Name = "LblpIsSequenceRunning"
        Me.LblpIsSequenceRunning.Size = New System.Drawing.Size(108, 13)
        Me.LblpIsSequenceRunning.TabIndex = 93
        Me.LblpIsSequenceRunning.Text = "pIsSequenceRunning"
        '
        'LblpIsEquipmentInitializing
        '
        Me.LblpIsEquipmentInitializing.AutoSize = True
        Me.LblpIsEquipmentInitializing.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblpIsEquipmentInitializing.ForeColor = System.Drawing.Color.White
        Me.LblpIsEquipmentInitializing.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblpIsEquipmentInitializing.Location = New System.Drawing.Point(6, 156)
        Me.LblpIsEquipmentInitializing.Name = "LblpIsEquipmentInitializing"
        Me.LblpIsEquipmentInitializing.Size = New System.Drawing.Size(119, 13)
        Me.LblpIsEquipmentInitializing.TabIndex = 92
        Me.LblpIsEquipmentInitializing.Text = "pIsEquipmentInitializing"
        '
        'LblpEquipmentStatus
        '
        Me.LblpEquipmentStatus.AutoSize = True
        Me.LblpEquipmentStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblpEquipmentStatus.ForeColor = System.Drawing.Color.White
        Me.LblpEquipmentStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblpEquipmentStatus.Location = New System.Drawing.Point(6, 179)
        Me.LblpEquipmentStatus.Name = "LblpEquipmentStatus"
        Me.LblpEquipmentStatus.Size = New System.Drawing.Size(94, 13)
        Me.LblpEquipmentStatus.TabIndex = 91
        Me.LblpEquipmentStatus.Text = "pEquipmentStatus"
        '
        'LblpRunStatus
        '
        Me.LblpRunStatus.AutoSize = True
        Me.LblpRunStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblpRunStatus.ForeColor = System.Drawing.Color.White
        Me.LblpRunStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblpRunStatus.Location = New System.Drawing.Point(6, 79)
        Me.LblpRunStatus.Name = "LblpRunStatus"
        Me.LblpRunStatus.Size = New System.Drawing.Size(63, 13)
        Me.LblpRunStatus.TabIndex = 90
        Me.LblpRunStatus.Text = "pRunStatus"
        '
        'LblpManualAbort
        '
        Me.LblpManualAbort.AutoSize = True
        Me.LblpManualAbort.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblpManualAbort.ForeColor = System.Drawing.Color.White
        Me.LblpManualAbort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblpManualAbort.Location = New System.Drawing.Point(6, 100)
        Me.LblpManualAbort.Name = "LblpManualAbort"
        Me.LblpManualAbort.Size = New System.Drawing.Size(74, 13)
        Me.LblpManualAbort.TabIndex = 89
        Me.LblpManualAbort.Text = "pManualAbort"
        '
        'LblpAbort
        '
        Me.LblpAbort.AutoSize = True
        Me.LblpAbort.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblpAbort.ForeColor = System.Drawing.Color.White
        Me.LblpAbort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblpAbort.Location = New System.Drawing.Point(6, 16)
        Me.LblpAbort.Name = "LblpAbort"
        Me.LblpAbort.Size = New System.Drawing.Size(40, 13)
        Me.LblpAbort.TabIndex = 88
        Me.LblpAbort.Text = "pAbort"
        '
        'GrpStatus
        '
        Me.GrpStatus.Controls.Add(Me.LblpMoonCooldownStatus)
        Me.GrpStatus.Controls.Add(Me.LblMoonSafetyStatus)
        Me.GrpStatus.Controls.Add(Me.lblpOldSequenceTimeSMART)
        Me.GrpStatus.Controls.Add(Me.LblpOldProgramTimeSMART)
        Me.GrpStatus.Controls.Add(Me.LblpSmartError)
        Me.GrpStatus.Controls.Add(Me.LblStartRun)
        Me.GrpStatus.Controls.Add(Me.LblpIsActionRunning)
        Me.GrpStatus.Controls.Add(Me.LblpIsSequenceRunning)
        Me.GrpStatus.Controls.Add(Me.LblpIsEquipmentInitializing)
        Me.GrpStatus.Controls.Add(Me.LblpEquipmentStatus)
        Me.GrpStatus.Controls.Add(Me.LblpRunStatus)
        Me.GrpStatus.Controls.Add(Me.LblpManualAbort)
        Me.GrpStatus.Controls.Add(Me.LblpAbort)
        Me.GrpStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpStatus.ForeColor = System.Drawing.Color.White
        Me.GrpStatus.Location = New System.Drawing.Point(306, 3)
        Me.GrpStatus.Name = "GrpStatus"
        Me.GrpStatus.Size = New System.Drawing.Size(404, 279)
        Me.GrpStatus.TabIndex = 95
        Me.GrpStatus.TabStop = False
        Me.GrpStatus.Text = "Status"
        '
        'LblpMoonCooldownStatus
        '
        Me.LblpMoonCooldownStatus.AutoSize = True
        Me.LblpMoonCooldownStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblpMoonCooldownStatus.ForeColor = System.Drawing.Color.White
        Me.LblpMoonCooldownStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblpMoonCooldownStatus.Location = New System.Drawing.Point(193, 36)
        Me.LblpMoonCooldownStatus.Name = "LblpMoonCooldownStatus"
        Me.LblpMoonCooldownStatus.Size = New System.Drawing.Size(117, 13)
        Me.LblpMoonCooldownStatus.TabIndex = 100
        Me.LblpMoonCooldownStatus.Text = "pMoonCooldownStatus"
        '
        'LblMoonSafetyStatus
        '
        Me.LblMoonSafetyStatus.AutoSize = True
        Me.LblMoonSafetyStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMoonSafetyStatus.ForeColor = System.Drawing.Color.White
        Me.LblMoonSafetyStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblMoonSafetyStatus.Location = New System.Drawing.Point(193, 16)
        Me.LblMoonSafetyStatus.Name = "LblMoonSafetyStatus"
        Me.LblMoonSafetyStatus.Size = New System.Drawing.Size(102, 13)
        Me.LblMoonSafetyStatus.TabIndex = 99
        Me.LblMoonSafetyStatus.Text = "pMoonSafetyStatus"
        '
        'lblpOldSequenceTimeSMART
        '
        Me.lblpOldSequenceTimeSMART.AutoSize = True
        Me.lblpOldSequenceTimeSMART.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblpOldSequenceTimeSMART.ForeColor = System.Drawing.Color.White
        Me.lblpOldSequenceTimeSMART.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblpOldSequenceTimeSMART.Location = New System.Drawing.Point(7, 252)
        Me.lblpOldSequenceTimeSMART.Name = "lblpOldSequenceTimeSMART"
        Me.lblpOldSequenceTimeSMART.Size = New System.Drawing.Size(132, 13)
        Me.lblpOldSequenceTimeSMART.TabIndex = 98
        Me.lblpOldSequenceTimeSMART.Text = "pOldSequenceTimeSMART"
        '
        'LblpOldProgramTimeSMART
        '
        Me.LblpOldProgramTimeSMART.AutoSize = True
        Me.LblpOldProgramTimeSMART.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblpOldProgramTimeSMART.ForeColor = System.Drawing.Color.White
        Me.LblpOldProgramTimeSMART.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblpOldProgramTimeSMART.Location = New System.Drawing.Point(7, 233)
        Me.LblpOldProgramTimeSMART.Name = "LblpOldProgramTimeSMART"
        Me.LblpOldProgramTimeSMART.Size = New System.Drawing.Size(125, 13)
        Me.LblpOldProgramTimeSMART.TabIndex = 97
        Me.LblpOldProgramTimeSMART.Text = "pOldProgramTimeSMART"
        '
        'LblpSmartError
        '
        Me.LblpSmartError.AutoSize = True
        Me.LblpSmartError.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblpSmartError.ForeColor = System.Drawing.Color.White
        Me.LblpSmartError.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblpSmartError.Location = New System.Drawing.Point(6, 214)
        Me.LblpSmartError.Name = "LblpSmartError"
        Me.LblpSmartError.Size = New System.Drawing.Size(65, 13)
        Me.LblpSmartError.TabIndex = 96
        Me.LblpSmartError.Text = "pSmartError"
        '
        'LblStartRun
        '
        Me.LblStartRun.AutoSize = True
        Me.LblStartRun.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStartRun.ForeColor = System.Drawing.Color.White
        Me.LblStartRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.LblStartRun.Location = New System.Drawing.Point(6, 120)
        Me.LblStartRun.Name = "LblStartRun"
        Me.LblStartRun.Size = New System.Drawing.Size(56, 13)
        Me.LblStartRun.TabIndex = 95
        Me.LblStartRun.Text = "pStartRun"
        '
        'TimerDebug
        '
        Me.TimerDebug.Enabled = True
        Me.TimerDebug.Interval = 300
        '
        'BtnShowTargets
        '
        Me.BtnShowTargets.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnShowTargets.FlatAppearance.BorderSize = 0
        Me.BtnShowTargets.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnShowTargets.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnShowTargets.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnShowTargets.ForeColor = System.Drawing.Color.White
        Me.BtnShowTargets.Location = New System.Drawing.Point(197, 178)
        Me.BtnShowTargets.Name = "BtnShowTargets"
        Me.BtnShowTargets.Size = New System.Drawing.Size(91, 23)
        Me.BtnShowTargets.TabIndex = 96
        Me.BtnShowTargets.Text = "Show targets"
        Me.BtnShowTargets.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.Button2.FlatAppearance.BorderSize = 0
        Me.Button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.Color.White
        Me.Button2.Location = New System.Drawing.Point(8, 281)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(91, 23)
        Me.Button2.TabIndex = 97
        Me.Button2.Text = "MOSAIC"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'TxtMosaicCenterRA
        '
        Me.TxtMosaicCenterRA.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMosaicCenterRA.Location = New System.Drawing.Point(180, 284)
        Me.TxtMosaicCenterRA.Name = "TxtMosaicCenterRA"
        Me.TxtMosaicCenterRA.Size = New System.Drawing.Size(67, 21)
        Me.TxtMosaicCenterRA.TabIndex = 98
        Me.TxtMosaicCenterRA.Text = "13.26369"
        '
        'TxtMosaicCenterDEC
        '
        Me.TxtMosaicCenterDEC.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMosaicCenterDEC.Location = New System.Drawing.Point(180, 310)
        Me.TxtMosaicCenterDEC.Name = "TxtMosaicCenterDEC"
        Me.TxtMosaicCenterDEC.Size = New System.Drawing.Size(67, 21)
        Me.TxtMosaicCenterDEC.TabIndex = 99
        Me.TxtMosaicCenterDEC.Text = "42.02937"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(112, 286)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(61, 13)
        Me.Label6.TabIndex = 100
        Me.Label6.Text = "Center RA:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(112, 313)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 13)
        Me.Label7.TabIndex = 101
        Me.Label7.Text = "Center DEC:"
        '
        'TxtMosaic
        '
        Me.TxtMosaic.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMosaic.Location = New System.Drawing.Point(253, 283)
        Me.TxtMosaic.Multiline = True
        Me.TxtMosaic.Name = "TxtMosaic"
        Me.TxtMosaic.Size = New System.Drawing.Size(457, 97)
        Me.TxtMosaic.TabIndex = 102
        '
        'CmbMosaicType
        '
        Me.CmbMosaicType.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbMosaicType.FormattingEnabled = True
        Me.CmbMosaicType.Items.AddRange(New Object() {"1x2", "2x1", "2x2"})
        Me.CmbMosaicType.Location = New System.Drawing.Point(180, 336)
        Me.CmbMosaicType.Name = "CmbMosaicType"
        Me.CmbMosaicType.Size = New System.Drawing.Size(67, 21)
        Me.CmbMosaicType.TabIndex = 103
        Me.CmbMosaicType.Text = "2x2"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(112, 339)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(35, 13)
        Me.Label8.TabIndex = 104
        Me.Label8.Text = "Type:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(112, 360)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(49, 13)
        Me.Label9.TabIndex = 105
        Me.Label9.Text = "Overlap:"
        '
        'TxtMosaicOverlap
        '
        Me.TxtMosaicOverlap.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMosaicOverlap.Location = New System.Drawing.Point(180, 360)
        Me.TxtMosaicOverlap.Name = "TxtMosaicOverlap"
        Me.TxtMosaicOverlap.Size = New System.Drawing.Size(67, 21)
        Me.TxtMosaicOverlap.TabIndex = 106
        Me.TxtMosaicOverlap.Text = "10"
        '
        'BtnError
        '
        Me.BtnError.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnError.FlatAppearance.BorderSize = 0
        Me.BtnError.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnError.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnError.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnError.ForeColor = System.Drawing.Color.White
        Me.BtnError.Location = New System.Drawing.Point(8, 329)
        Me.BtnError.Name = "BtnError"
        Me.BtnError.Size = New System.Drawing.Size(91, 23)
        Me.BtnError.TabIndex = 107
        Me.BtnError.Text = "Play Error"
        Me.BtnError.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.White
        Me.Button1.Location = New System.Drawing.Point(8, 355)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(91, 23)
        Me.Button1.TabIndex = 108
        Me.Button1.Text = "Play Error"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'FrmDebug
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(99, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(114, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(719, 382)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.BtnError)
        Me.Controls.Add(Me.TxtMosaicOverlap)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.CmbMosaicType)
        Me.Controls.Add(Me.TxtMosaic)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TxtMosaicCenterDEC)
        Me.Controls.Add(Me.TxtMosaicCenterRA)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.BtnShowTargets)
        Me.Controls.Add(Me.GrpStatus)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TxtMoonIllumination)
        Me.Controls.Add(Me.ChkISSequenceRunning)
        Me.Controls.Add(Me.ButtonEnableAAG)
        Me.Controls.Add(Me.ButtonKillAAG)
        Me.Controls.Add(Me.ChkRunStatus)
        Me.Controls.Add(Me.ChkDebugAAGData)
        Me.Controls.Add(Me.ChkDebugCoordinates)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.CmbMoonSettingRising)
        Me.Controls.Add(Me.CmbSunSettingRising)
        Me.Controls.Add(Me.TxtMoonAltitude)
        Me.Controls.Add(Me.TxtSunAltitude)
        Me.Controls.Add(Me.BtnLIGHT)
        Me.Controls.Add(Me.BtnDARK)
        Me.Controls.Add(Me.BtnUNSAFE)
        Me.Controls.Add(Me.BtnSAFE)
        Me.Controls.Add(Me.ButtonEnableUPS)
        Me.Controls.Add(Me.ButtonKillUPS)
        Me.Controls.Add(Me.ButtonEnableInternet)
        Me.Controls.Add(Me.ButtonKillInternet)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FrmDebug"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Debugging"
        Me.TopMost = True
        Me.GrpStatus.ResumeLayout(False)
        Me.GrpStatus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ButtonKillInternet As Button
    Friend WithEvents ButtonEnableInternet As Button
    Friend WithEvents ButtonEnableUPS As Button
    Friend WithEvents ButtonKillUPS As Button
    Friend WithEvents BtnSAFE As Button
    Friend WithEvents BtnUNSAFE As Button
    Friend WithEvents BtnDARK As Button
    Friend WithEvents BtnLIGHT As Button
    Friend WithEvents TxtSunAltitude As TextBox
    Friend WithEvents TxtMoonAltitude As TextBox
    Friend WithEvents CmbSunSettingRising As ComboBox
    Friend WithEvents CmbMoonSettingRising As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents ChkDebugCoordinates As CheckBox
    Friend WithEvents ChkDebugAAGData As CheckBox
    Friend WithEvents ChkRunStatus As CheckBox
    Friend WithEvents ButtonKillAAG As Button
    Friend WithEvents ButtonEnableAAG As Button
    Friend WithEvents ChkISSequenceRunning As CheckBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TxtMoonIllumination As TextBox
    Friend WithEvents LblpIsActionRunning As Label
    Friend WithEvents LblpIsSequenceRunning As Label
    Friend WithEvents LblpIsEquipmentInitializing As Label
    Friend WithEvents LblpEquipmentStatus As Label
    Friend WithEvents LblpRunStatus As Label
    Friend WithEvents LblpManualAbort As Label
    Friend WithEvents LblpAbort As Label
    Friend WithEvents GrpStatus As GroupBox
    Friend WithEvents TimerDebug As Timer
    Friend WithEvents LblStartRun As Label
    Friend WithEvents LblpSmartError As Label
    Friend WithEvents LblpOldProgramTimeSMART As Label
    Friend WithEvents BtnShowTargets As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents lblpOldSequenceTimeSMART As Label
    Friend WithEvents TxtMosaicCenterRA As TextBox
    Friend WithEvents TxtMosaicCenterDEC As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents TxtMosaic As TextBox
    Friend WithEvents CmbMosaicType As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents TxtMosaicOverlap As TextBox
    Friend WithEvents BtnError As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents LblpMoonCooldownStatus As Label
    Friend WithEvents LblMoonSafetyStatus As Label
End Class
