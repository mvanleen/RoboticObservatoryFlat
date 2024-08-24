<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmTools
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmTools))
        Me.grpCover = New System.Windows.Forms.GroupBox()
        Me.BtnDisconnectCover = New System.Windows.Forms.Button()
        Me.BtnConnectCover = New System.Windows.Forms.Button()
        Me.LblCover = New System.Windows.Forms.Label()
        Me.grpRoofRoof = New System.Windows.Forms.GroupBox()
        Me.LblRoof = New System.Windows.Forms.Label()
        Me.BtnRoofOpenPCT = New System.Windows.Forms.Button()
        Me.TimerRoof = New System.Windows.Forms.Timer(Me.components)
        Me.TimerCover = New System.Windows.Forms.Timer(Me.components)
        Me.frpDragonFly = New System.Windows.Forms.GroupBox()
        Me.btnStopAllEquipment = New System.Windows.Forms.Button()
        Me.btnStartAllEquipment = New System.Windows.Forms.Button()
        Me.lblSwitch8Status = New System.Windows.Forms.Label()
        Me.lblSwitch3Status = New System.Windows.Forms.Label()
        Me.lblSwitch4Status = New System.Windows.Forms.Label()
        Me.lblSwitch5Status = New System.Windows.Forms.Label()
        Me.lblSwitch6Status = New System.Windows.Forms.Label()
        Me.lblSwitch7Status = New System.Windows.Forms.Label()
        Me.lblSwitch2Status = New System.Windows.Forms.Label()
        Me.lblSwitch1Status = New System.Windows.Forms.Label()
        Me.TimerSwitch = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BtnUnpark = New System.Windows.Forms.Button()
        Me.BtnPark = New System.Windows.Forms.Button()
        Me.BtnDisconnectMount = New System.Windows.Forms.Button()
        Me.BtnConnectMount = New System.Windows.Forms.Button()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BtnMountNorth = New System.Windows.Forms.Button()
        Me.LblSpeed = New System.Windows.Forms.Label()
        Me.BtnMountEast = New System.Windows.Forms.Button()
        Me.TxtMountSpeed = New System.Windows.Forms.TextBox()
        Me.BtnMountWest = New System.Windows.Forms.Button()
        Me.BtnMountSouth = New System.Windows.Forms.Button()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.BtnClosedLoopSlew = New System.Windows.Forms.Button()
        Me.BtnTargetAbortSlew = New System.Windows.Forms.Button()
        Me.BtnTargetSlew = New System.Windows.Forms.Button()
        Me.BtnTargetFindTSX = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TxtTargetName = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TxtTargetDEC2000DG = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TxtTargetRA2000SS = New System.Windows.Forms.TextBox()
        Me.TxtTargetRA2000HH = New System.Windows.Forms.TextBox()
        Me.TxtTargetDEC2000MM = New System.Windows.Forms.TextBox()
        Me.TxtTargetDEC2000SS = New System.Windows.Forms.TextBox()
        Me.TxtTargetRA2000MM = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.BtnFocusserAutofocus = New System.Windows.Forms.Button()
        Me.BtnFocusserMoveTo = New System.Windows.Forms.Button()
        Me.TxtFocusserAbsolutePosition = New System.Windows.Forms.TextBox()
        Me.TxtRelPosition = New System.Windows.Forms.TextBox()
        Me.BtnFocusserOUT = New System.Windows.Forms.Button()
        Me.BtnFocusserIN = New System.Windows.Forms.Button()
        Me.LblFocusserPosition = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnDisconnectCCD = New System.Windows.Forms.Button()
        Me.BtnConnectCCD = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.LblCCDExposureStatus = New System.Windows.Forms.Label()
        Me.BtnAbortImage = New System.Windows.Forms.Button()
        Me.CmbTargetFilter = New System.Windows.Forms.ComboBox()
        Me.CmbTargetBinning = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtCCDExpsoure = New System.Windows.Forms.TextBox()
        Me.BtnTakeImage = New System.Windows.Forms.Button()
        Me.TimerCCDFocusser = New System.Windows.Forms.Timer(Me.components)
        Me.TimerMount = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.BtnZipCopyImageSets = New System.Windows.Forms.Button()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.ChkOpenRoof = New System.Windows.Forms.CheckBox()
        Me.BtnPauzeEquipment = New System.Windows.Forms.Button()
        Me.BtnShutdownEquipment = New System.Windows.Forms.Button()
        Me.BtnStartEquipment = New System.Windows.Forms.Button()
        Me.grpCover.SuspendLayout()
        Me.grpRoofRoof.SuspendLayout()
        Me.frpDragonFly.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpCover
        '
        Me.grpCover.Controls.Add(Me.BtnDisconnectCover)
        Me.grpCover.Controls.Add(Me.BtnConnectCover)
        Me.grpCover.Controls.Add(Me.LblCover)
        Me.grpCover.Location = New System.Drawing.Point(5, 283)
        Me.grpCover.Name = "grpCover"
        Me.grpCover.Size = New System.Drawing.Size(150, 66)
        Me.grpCover.TabIndex = 63
        Me.grpCover.TabStop = False
        Me.grpCover.Text = "Cover"
        '
        'BtnDisconnectCover
        '
        Me.BtnDisconnectCover.Location = New System.Drawing.Point(75, 42)
        Me.BtnDisconnectCover.Name = "BtnDisconnectCover"
        Me.BtnDisconnectCover.Size = New System.Drawing.Size(71, 21)
        Me.BtnDisconnectCover.TabIndex = 70
        Me.BtnDisconnectCover.Text = "Disconnect"
        Me.BtnDisconnectCover.UseVisualStyleBackColor = True
        '
        'BtnConnectCover
        '
        Me.BtnConnectCover.Location = New System.Drawing.Point(4, 42)
        Me.BtnConnectCover.Name = "BtnConnectCover"
        Me.BtnConnectCover.Size = New System.Drawing.Size(63, 21)
        Me.BtnConnectCover.TabIndex = 69
        Me.BtnConnectCover.Text = "Connect"
        Me.BtnConnectCover.UseVisualStyleBackColor = True
        '
        'LblCover
        '
        Me.LblCover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblCover.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCover.Location = New System.Drawing.Point(4, 15)
        Me.LblCover.Name = "LblCover"
        Me.LblCover.Size = New System.Drawing.Size(142, 26)
        Me.LblCover.TabIndex = 65
        Me.LblCover.Text = "Cover"
        Me.LblCover.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpRoofRoof
        '
        Me.grpRoofRoof.Controls.Add(Me.LblRoof)
        Me.grpRoofRoof.Controls.Add(Me.BtnRoofOpenPCT)
        Me.grpRoofRoof.Location = New System.Drawing.Point(5, 215)
        Me.grpRoofRoof.Name = "grpRoofRoof"
        Me.grpRoofRoof.Size = New System.Drawing.Size(150, 68)
        Me.grpRoofRoof.TabIndex = 62
        Me.grpRoofRoof.TabStop = False
        Me.grpRoofRoof.Text = "Roof"
        '
        'LblRoof
        '
        Me.LblRoof.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblRoof.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRoof.Location = New System.Drawing.Point(4, 13)
        Me.LblRoof.Name = "LblRoof"
        Me.LblRoof.Size = New System.Drawing.Size(142, 26)
        Me.LblRoof.TabIndex = 64
        Me.LblRoof.Text = "Roof"
        Me.LblRoof.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnRoofOpenPCT
        '
        Me.BtnRoofOpenPCT.Location = New System.Drawing.Point(4, 40)
        Me.BtnRoofOpenPCT.Name = "BtnRoofOpenPCT"
        Me.BtnRoofOpenPCT.Size = New System.Drawing.Size(142, 25)
        Me.BtnRoofOpenPCT.TabIndex = 16
        Me.BtnRoofOpenPCT.Text = "Open partly"
        Me.BtnRoofOpenPCT.UseVisualStyleBackColor = True
        '
        'TimerRoof
        '
        Me.TimerRoof.Interval = 500
        '
        'TimerCover
        '
        Me.TimerCover.Interval = 500
        '
        'frpDragonFly
        '
        Me.frpDragonFly.Controls.Add(Me.btnStopAllEquipment)
        Me.frpDragonFly.Controls.Add(Me.btnStartAllEquipment)
        Me.frpDragonFly.Controls.Add(Me.lblSwitch8Status)
        Me.frpDragonFly.Controls.Add(Me.lblSwitch3Status)
        Me.frpDragonFly.Controls.Add(Me.lblSwitch4Status)
        Me.frpDragonFly.Controls.Add(Me.lblSwitch5Status)
        Me.frpDragonFly.Controls.Add(Me.lblSwitch6Status)
        Me.frpDragonFly.Controls.Add(Me.lblSwitch7Status)
        Me.frpDragonFly.Controls.Add(Me.lblSwitch2Status)
        Me.frpDragonFly.Controls.Add(Me.lblSwitch1Status)
        Me.frpDragonFly.Location = New System.Drawing.Point(5, 2)
        Me.frpDragonFly.Name = "frpDragonFly"
        Me.frpDragonFly.Size = New System.Drawing.Size(150, 213)
        Me.frpDragonFly.TabIndex = 64
        Me.frpDragonFly.TabStop = False
        Me.frpDragonFly.Text = "DragonFly"
        '
        'btnStopAllEquipment
        '
        Me.btnStopAllEquipment.Location = New System.Drawing.Point(77, 184)
        Me.btnStopAllEquipment.Name = "btnStopAllEquipment"
        Me.btnStopAllEquipment.Size = New System.Drawing.Size(66, 25)
        Me.btnStopAllEquipment.TabIndex = 17
        Me.btnStopAllEquipment.Text = "OFF"
        Me.btnStopAllEquipment.UseVisualStyleBackColor = True
        '
        'btnStartAllEquipment
        '
        Me.btnStartAllEquipment.Location = New System.Drawing.Point(4, 184)
        Me.btnStartAllEquipment.Name = "btnStartAllEquipment"
        Me.btnStartAllEquipment.Size = New System.Drawing.Size(67, 26)
        Me.btnStartAllEquipment.TabIndex = 16
        Me.btnStartAllEquipment.Text = "ON"
        Me.btnStartAllEquipment.UseVisualStyleBackColor = True
        '
        'lblSwitch8Status
        '
        Me.lblSwitch8Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwitch8Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch8Status.Location = New System.Drawing.Point(4, 163)
        Me.lblSwitch8Status.Name = "lblSwitch8Status"
        Me.lblSwitch8Status.Size = New System.Drawing.Size(142, 20)
        Me.lblSwitch8Status.TabIndex = 15
        Me.lblSwitch8Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch3Status
        '
        Me.lblSwitch3Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwitch3Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch3Status.Location = New System.Drawing.Point(4, 58)
        Me.lblSwitch3Status.Name = "lblSwitch3Status"
        Me.lblSwitch3Status.Size = New System.Drawing.Size(142, 20)
        Me.lblSwitch3Status.TabIndex = 14
        Me.lblSwitch3Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch4Status
        '
        Me.lblSwitch4Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwitch4Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch4Status.Location = New System.Drawing.Point(4, 79)
        Me.lblSwitch4Status.Name = "lblSwitch4Status"
        Me.lblSwitch4Status.Size = New System.Drawing.Size(142, 20)
        Me.lblSwitch4Status.TabIndex = 13
        Me.lblSwitch4Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch5Status
        '
        Me.lblSwitch5Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwitch5Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch5Status.Location = New System.Drawing.Point(4, 100)
        Me.lblSwitch5Status.Name = "lblSwitch5Status"
        Me.lblSwitch5Status.Size = New System.Drawing.Size(142, 20)
        Me.lblSwitch5Status.TabIndex = 12
        Me.lblSwitch5Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch6Status
        '
        Me.lblSwitch6Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwitch6Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch6Status.Location = New System.Drawing.Point(4, 121)
        Me.lblSwitch6Status.Name = "lblSwitch6Status"
        Me.lblSwitch6Status.Size = New System.Drawing.Size(142, 20)
        Me.lblSwitch6Status.TabIndex = 11
        Me.lblSwitch6Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch7Status
        '
        Me.lblSwitch7Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwitch7Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch7Status.Location = New System.Drawing.Point(4, 142)
        Me.lblSwitch7Status.Name = "lblSwitch7Status"
        Me.lblSwitch7Status.Size = New System.Drawing.Size(142, 20)
        Me.lblSwitch7Status.TabIndex = 10
        Me.lblSwitch7Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch2Status
        '
        Me.lblSwitch2Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwitch2Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch2Status.Location = New System.Drawing.Point(4, 37)
        Me.lblSwitch2Status.Name = "lblSwitch2Status"
        Me.lblSwitch2Status.Size = New System.Drawing.Size(142, 20)
        Me.lblSwitch2Status.TabIndex = 4
        Me.lblSwitch2Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch1Status
        '
        Me.lblSwitch1Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSwitch1Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch1Status.Location = New System.Drawing.Point(4, 16)
        Me.lblSwitch1Status.Name = "lblSwitch1Status"
        Me.lblSwitch1Status.Size = New System.Drawing.Size(142, 20)
        Me.lblSwitch1Status.TabIndex = 3
        Me.lblSwitch1Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TimerSwitch
        '
        Me.TimerSwitch.Interval = 500
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BtnUnpark)
        Me.GroupBox1.Controls.Add(Me.BtnPark)
        Me.GroupBox1.Controls.Add(Me.BtnDisconnectMount)
        Me.GroupBox1.Controls.Add(Me.BtnConnectMount)
        Me.GroupBox1.Controls.Add(Me.GroupBox6)
        Me.GroupBox1.Controls.Add(Me.GroupBox5)
        Me.GroupBox1.Location = New System.Drawing.Point(161, 107)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(396, 152)
        Me.GroupBox1.TabIndex = 65
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Mount"
        '
        'BtnUnpark
        '
        Me.BtnUnpark.Location = New System.Drawing.Point(2, 115)
        Me.BtnUnpark.Name = "BtnUnpark"
        Me.BtnUnpark.Size = New System.Drawing.Size(64, 26)
        Me.BtnUnpark.TabIndex = 33
        Me.BtnUnpark.Text = "Unpark"
        Me.BtnUnpark.UseVisualStyleBackColor = True
        '
        'BtnPark
        '
        Me.BtnPark.Location = New System.Drawing.Point(2, 88)
        Me.BtnPark.Name = "BtnPark"
        Me.BtnPark.Size = New System.Drawing.Size(64, 26)
        Me.BtnPark.TabIndex = 32
        Me.BtnPark.Text = "Park"
        Me.BtnPark.UseVisualStyleBackColor = True
        '
        'BtnDisconnectMount
        '
        Me.BtnDisconnectMount.Location = New System.Drawing.Point(2, 52)
        Me.BtnDisconnectMount.Name = "BtnDisconnectMount"
        Me.BtnDisconnectMount.Size = New System.Drawing.Size(64, 36)
        Me.BtnDisconnectMount.TabIndex = 21
        Me.BtnDisconnectMount.Text = "Diconnect Mount"
        Me.BtnDisconnectMount.UseVisualStyleBackColor = True
        '
        'BtnConnectMount
        '
        Me.BtnConnectMount.Location = New System.Drawing.Point(2, 16)
        Me.BtnConnectMount.Name = "BtnConnectMount"
        Me.BtnConnectMount.Size = New System.Drawing.Size(64, 36)
        Me.BtnConnectMount.TabIndex = 20
        Me.BtnConnectMount.Text = "Connect Mount"
        Me.BtnConnectMount.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.Label3)
        Me.GroupBox6.Controls.Add(Me.BtnMountNorth)
        Me.GroupBox6.Controls.Add(Me.LblSpeed)
        Me.GroupBox6.Controls.Add(Me.BtnMountEast)
        Me.GroupBox6.Controls.Add(Me.TxtMountSpeed)
        Me.GroupBox6.Controls.Add(Me.BtnMountWest)
        Me.GroupBox6.Controls.Add(Me.BtnMountSouth)
        Me.GroupBox6.Location = New System.Drawing.Point(67, 9)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(117, 138)
        Me.GroupBox6.TabIndex = 31
        Me.GroupBox6.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(85, 118)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(21, 13)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "°/s"
        '
        'BtnMountNorth
        '
        Me.BtnMountNorth.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMountNorth.Location = New System.Drawing.Point(39, 8)
        Me.BtnMountNorth.Name = "BtnMountNorth"
        Me.BtnMountNorth.Size = New System.Drawing.Size(39, 36)
        Me.BtnMountNorth.TabIndex = 22
        Me.BtnMountNorth.Text = "N"
        Me.BtnMountNorth.UseVisualStyleBackColor = True
        '
        'LblSpeed
        '
        Me.LblSpeed.AutoSize = True
        Me.LblSpeed.Location = New System.Drawing.Point(-1, 118)
        Me.LblSpeed.Name = "LblSpeed"
        Me.LblSpeed.Size = New System.Drawing.Size(41, 13)
        Me.LblSpeed.TabIndex = 28
        Me.LblSpeed.Text = "Speed:"
        '
        'BtnMountEast
        '
        Me.BtnMountEast.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMountEast.Location = New System.Drawing.Point(1, 44)
        Me.BtnMountEast.Name = "BtnMountEast"
        Me.BtnMountEast.Size = New System.Drawing.Size(39, 36)
        Me.BtnMountEast.TabIndex = 23
        Me.BtnMountEast.Text = "E"
        Me.BtnMountEast.UseVisualStyleBackColor = True
        '
        'TxtMountSpeed
        '
        Me.TxtMountSpeed.Location = New System.Drawing.Point(40, 115)
        Me.TxtMountSpeed.Name = "TxtMountSpeed"
        Me.TxtMountSpeed.Size = New System.Drawing.Size(37, 20)
        Me.TxtMountSpeed.TabIndex = 27
        Me.TxtMountSpeed.Text = "1"
        '
        'BtnMountWest
        '
        Me.BtnMountWest.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMountWest.Location = New System.Drawing.Point(77, 44)
        Me.BtnMountWest.Name = "BtnMountWest"
        Me.BtnMountWest.Size = New System.Drawing.Size(39, 36)
        Me.BtnMountWest.TabIndex = 24
        Me.BtnMountWest.Text = "W"
        Me.BtnMountWest.UseVisualStyleBackColor = True
        '
        'BtnMountSouth
        '
        Me.BtnMountSouth.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMountSouth.Location = New System.Drawing.Point(39, 78)
        Me.BtnMountSouth.Name = "BtnMountSouth"
        Me.BtnMountSouth.Size = New System.Drawing.Size(39, 36)
        Me.BtnMountSouth.TabIndex = 26
        Me.BtnMountSouth.Text = "S"
        Me.BtnMountSouth.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.BtnClosedLoopSlew)
        Me.GroupBox5.Controls.Add(Me.BtnTargetAbortSlew)
        Me.GroupBox5.Controls.Add(Me.BtnTargetSlew)
        Me.GroupBox5.Controls.Add(Me.BtnTargetFindTSX)
        Me.GroupBox5.Controls.Add(Me.Label5)
        Me.GroupBox5.Controls.Add(Me.TxtTargetName)
        Me.GroupBox5.Controls.Add(Me.Label6)
        Me.GroupBox5.Controls.Add(Me.Label14)
        Me.GroupBox5.Controls.Add(Me.Label13)
        Me.GroupBox5.Controls.Add(Me.Label7)
        Me.GroupBox5.Controls.Add(Me.Label15)
        Me.GroupBox5.Controls.Add(Me.Label12)
        Me.GroupBox5.Controls.Add(Me.Label8)
        Me.GroupBox5.Controls.Add(Me.TxtTargetDEC2000DG)
        Me.GroupBox5.Controls.Add(Me.Label9)
        Me.GroupBox5.Controls.Add(Me.TxtTargetRA2000SS)
        Me.GroupBox5.Controls.Add(Me.TxtTargetRA2000HH)
        Me.GroupBox5.Controls.Add(Me.TxtTargetDEC2000MM)
        Me.GroupBox5.Controls.Add(Me.TxtTargetDEC2000SS)
        Me.GroupBox5.Controls.Add(Me.TxtTargetRA2000MM)
        Me.GroupBox5.Location = New System.Drawing.Point(187, 9)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(204, 138)
        Me.GroupBox5.TabIndex = 30
        Me.GroupBox5.TabStop = False
        '
        'BtnClosedLoopSlew
        '
        Me.BtnClosedLoopSlew.Location = New System.Drawing.Point(47, 92)
        Me.BtnClosedLoopSlew.Name = "BtnClosedLoopSlew"
        Me.BtnClosedLoopSlew.Size = New System.Drawing.Size(101, 25)
        Me.BtnClosedLoopSlew.TabIndex = 27
        Me.BtnClosedLoopSlew.Text = "Closed loop slew"
        Me.BtnClosedLoopSlew.UseVisualStyleBackColor = True
        '
        'BtnTargetAbortSlew
        '
        Me.BtnTargetAbortSlew.Location = New System.Drawing.Point(148, 92)
        Me.BtnTargetAbortSlew.Name = "BtnTargetAbortSlew"
        Me.BtnTargetAbortSlew.Size = New System.Drawing.Size(52, 25)
        Me.BtnTargetAbortSlew.TabIndex = 28
        Me.BtnTargetAbortSlew.Text = "Abort"
        Me.BtnTargetAbortSlew.UseVisualStyleBackColor = True
        '
        'BtnTargetSlew
        '
        Me.BtnTargetSlew.Location = New System.Drawing.Point(3, 92)
        Me.BtnTargetSlew.Name = "BtnTargetSlew"
        Me.BtnTargetSlew.Size = New System.Drawing.Size(44, 25)
        Me.BtnTargetSlew.TabIndex = 26
        Me.BtnTargetSlew.Text = "Slew"
        Me.BtnTargetSlew.UseVisualStyleBackColor = True
        '
        'BtnTargetFindTSX
        '
        Me.BtnTargetFindTSX.Location = New System.Drawing.Point(148, 10)
        Me.BtnTargetFindTSX.Name = "BtnTargetFindTSX"
        Me.BtnTargetFindTSX.Size = New System.Drawing.Size(40, 20)
        Me.BtnTargetFindTSX.TabIndex = 25
        Me.BtnTargetFindTSX.Text = "Find"
        Me.BtnTargetFindTSX.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 13)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(38, 13)
        Me.Label5.TabIndex = 23
        Me.Label5.Text = "Name:"
        '
        'TxtTargetName
        '
        Me.TxtTargetName.Location = New System.Drawing.Point(68, 10)
        Me.TxtTargetName.Name = "TxtTargetName"
        Me.TxtTargetName.Size = New System.Drawing.Size(65, 20)
        Me.TxtTargetName.TabIndex = 24
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 38)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(61, 13)
        Me.Label6.TabIndex = 26
        Me.Label6.Text = "RA  (2000):"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(133, 64)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(15, 13)
        Me.Label14.TabIndex = 38
        Me.Label14.Text = "m"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(94, 64)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(11, 13)
        Me.Label13.TabIndex = 37
        Me.Label13.Text = "°"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(94, 38)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(13, 13)
        Me.Label7.TabIndex = 29
        Me.Label7.Text = "h"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(188, 64)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(12, 13)
        Me.Label15.TabIndex = 39
        Me.Label15.Text = "s"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(3, 64)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(65, 13)
        Me.Label12.TabIndex = 36
        Me.Label12.Text = "DEC (2000):"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(133, 38)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(15, 13)
        Me.Label8.TabIndex = 30
        Me.Label8.Text = "m"
        '
        'TxtTargetDEC2000DG
        '
        Me.TxtTargetDEC2000DG.Location = New System.Drawing.Point(68, 61)
        Me.TxtTargetDEC2000DG.Name = "TxtTargetDEC2000DG"
        Me.TxtTargetDEC2000DG.Size = New System.Drawing.Size(26, 20)
        Me.TxtTargetDEC2000DG.TabIndex = 31
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(188, 38)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(12, 13)
        Me.Label9.TabIndex = 32
        Me.Label9.Text = "s"
        '
        'TxtTargetRA2000SS
        '
        Me.TxtTargetRA2000SS.Location = New System.Drawing.Point(148, 35)
        Me.TxtTargetRA2000SS.Name = "TxtTargetRA2000SS"
        Me.TxtTargetRA2000SS.Size = New System.Drawing.Size(40, 20)
        Me.TxtTargetRA2000SS.TabIndex = 28
        '
        'TxtTargetRA2000HH
        '
        Me.TxtTargetRA2000HH.Location = New System.Drawing.Point(68, 35)
        Me.TxtTargetRA2000HH.Name = "TxtTargetRA2000HH"
        Me.TxtTargetRA2000HH.Size = New System.Drawing.Size(26, 20)
        Me.TxtTargetRA2000HH.TabIndex = 25
        '
        'TxtTargetDEC2000MM
        '
        Me.TxtTargetDEC2000MM.Location = New System.Drawing.Point(107, 61)
        Me.TxtTargetDEC2000MM.Name = "TxtTargetDEC2000MM"
        Me.TxtTargetDEC2000MM.Size = New System.Drawing.Size(26, 20)
        Me.TxtTargetDEC2000MM.TabIndex = 33
        '
        'TxtTargetDEC2000SS
        '
        Me.TxtTargetDEC2000SS.Location = New System.Drawing.Point(148, 61)
        Me.TxtTargetDEC2000SS.Name = "TxtTargetDEC2000SS"
        Me.TxtTargetDEC2000SS.Size = New System.Drawing.Size(40, 20)
        Me.TxtTargetDEC2000SS.TabIndex = 34
        '
        'TxtTargetRA2000MM
        '
        Me.TxtTargetRA2000MM.Location = New System.Drawing.Point(107, 35)
        Me.TxtTargetRA2000MM.Name = "TxtTargetRA2000MM"
        Me.TxtTargetRA2000MM.Size = New System.Drawing.Size(26, 20)
        Me.TxtTargetRA2000MM.TabIndex = 27
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.GroupBox3)
        Me.GroupBox2.Controls.Add(Me.BtnDisconnectCCD)
        Me.GroupBox2.Controls.Add(Me.BtnConnectCCD)
        Me.GroupBox2.Controls.Add(Me.GroupBox4)
        Me.GroupBox2.Location = New System.Drawing.Point(157, 265)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(400, 146)
        Me.GroupBox2.TabIndex = 66
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "CCD"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.BtnFocusserAutofocus)
        Me.GroupBox3.Controls.Add(Me.BtnFocusserMoveTo)
        Me.GroupBox3.Controls.Add(Me.TxtFocusserAbsolutePosition)
        Me.GroupBox3.Controls.Add(Me.TxtRelPosition)
        Me.GroupBox3.Controls.Add(Me.BtnFocusserOUT)
        Me.GroupBox3.Controls.Add(Me.BtnFocusserIN)
        Me.GroupBox3.Controls.Add(Me.LblFocusserPosition)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 96)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(389, 44)
        Me.GroupBox3.TabIndex = 66
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Focusser"
        '
        'BtnFocusserAutofocus
        '
        Me.BtnFocusserAutofocus.Location = New System.Drawing.Point(324, 11)
        Me.BtnFocusserAutofocus.Name = "BtnFocusserAutofocus"
        Me.BtnFocusserAutofocus.Size = New System.Drawing.Size(64, 23)
        Me.BtnFocusserAutofocus.TabIndex = 10
        Me.BtnFocusserAutofocus.Text = "Autofocus"
        Me.BtnFocusserAutofocus.UseVisualStyleBackColor = True
        '
        'BtnFocusserMoveTo
        '
        Me.BtnFocusserMoveTo.Location = New System.Drawing.Point(261, 12)
        Me.BtnFocusserMoveTo.Name = "BtnFocusserMoveTo"
        Me.BtnFocusserMoveTo.Size = New System.Drawing.Size(57, 23)
        Me.BtnFocusserMoveTo.TabIndex = 9
        Me.BtnFocusserMoveTo.Text = "Move to"
        Me.BtnFocusserMoveTo.UseVisualStyleBackColor = True
        '
        'TxtFocusserAbsolutePosition
        '
        Me.TxtFocusserAbsolutePosition.Location = New System.Drawing.Point(204, 14)
        Me.TxtFocusserAbsolutePosition.Name = "TxtFocusserAbsolutePosition"
        Me.TxtFocusserAbsolutePosition.Size = New System.Drawing.Size(51, 20)
        Me.TxtFocusserAbsolutePosition.TabIndex = 8
        '
        'TxtRelPosition
        '
        Me.TxtRelPosition.Location = New System.Drawing.Point(139, 14)
        Me.TxtRelPosition.Name = "TxtRelPosition"
        Me.TxtRelPosition.Size = New System.Drawing.Size(25, 20)
        Me.TxtRelPosition.TabIndex = 7
        Me.TxtRelPosition.Text = "50"
        '
        'BtnFocusserOUT
        '
        Me.BtnFocusserOUT.Location = New System.Drawing.Point(164, 13)
        Me.BtnFocusserOUT.Name = "BtnFocusserOUT"
        Me.BtnFocusserOUT.Size = New System.Drawing.Size(39, 23)
        Me.BtnFocusserOUT.TabIndex = 6
        Me.BtnFocusserOUT.Text = "OUT"
        Me.BtnFocusserOUT.UseVisualStyleBackColor = True
        '
        'BtnFocusserIN
        '
        Me.BtnFocusserIN.Location = New System.Drawing.Point(100, 13)
        Me.BtnFocusserIN.Name = "BtnFocusserIN"
        Me.BtnFocusserIN.Size = New System.Drawing.Size(39, 23)
        Me.BtnFocusserIN.TabIndex = 5
        Me.BtnFocusserIN.Text = "IN"
        Me.BtnFocusserIN.UseVisualStyleBackColor = True
        '
        'LblFocusserPosition
        '
        Me.LblFocusserPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblFocusserPosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFocusserPosition.Location = New System.Drawing.Point(49, 15)
        Me.LblFocusserPosition.Name = "LblFocusserPosition"
        Me.LblFocusserPosition.Size = New System.Drawing.Size(51, 20)
        Me.LblFocusserPosition.TabIndex = 4
        Me.LblFocusserPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Position:"
        '
        'BtnDisconnectCCD
        '
        Me.BtnDisconnectCCD.Location = New System.Drawing.Point(6, 54)
        Me.BtnDisconnectCCD.Name = "BtnDisconnectCCD"
        Me.BtnDisconnectCCD.Size = New System.Drawing.Size(69, 36)
        Me.BtnDisconnectCCD.TabIndex = 19
        Me.BtnDisconnectCCD.Text = "Disconnect"
        Me.BtnDisconnectCCD.UseVisualStyleBackColor = True
        '
        'BtnConnectCCD
        '
        Me.BtnConnectCCD.Location = New System.Drawing.Point(6, 19)
        Me.BtnConnectCCD.Name = "BtnConnectCCD"
        Me.BtnConnectCCD.Size = New System.Drawing.Size(69, 36)
        Me.BtnConnectCCD.TabIndex = 18
        Me.BtnConnectCCD.Text = "Connect"
        Me.BtnConnectCCD.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.LblCCDExposureStatus)
        Me.GroupBox4.Controls.Add(Me.BtnAbortImage)
        Me.GroupBox4.Controls.Add(Me.CmbTargetFilter)
        Me.GroupBox4.Controls.Add(Me.CmbTargetBinning)
        Me.GroupBox4.Controls.Add(Me.Label2)
        Me.GroupBox4.Controls.Add(Me.TxtCCDExpsoure)
        Me.GroupBox4.Controls.Add(Me.BtnTakeImage)
        Me.GroupBox4.Location = New System.Drawing.Point(76, 10)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(319, 80)
        Me.GroupBox4.TabIndex = 67
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "CCD"
        '
        'LblCCDExposureStatus
        '
        Me.LblCCDExposureStatus.AutoSize = True
        Me.LblCCDExposureStatus.Location = New System.Drawing.Point(8, 43)
        Me.LblCCDExposureStatus.Name = "LblCCDExposureStatus"
        Me.LblCCDExposureStatus.Size = New System.Drawing.Size(117, 13)
        Me.LblCCDExposureStatus.TabIndex = 25
        Me.LblCCDExposureStatus.Text = "LblCCDExposureStatus"
        '
        'BtnAbortImage
        '
        Me.BtnAbortImage.Location = New System.Drawing.Point(264, 36)
        Me.BtnAbortImage.Name = "BtnAbortImage"
        Me.BtnAbortImage.Size = New System.Drawing.Size(54, 26)
        Me.BtnAbortImage.TabIndex = 26
        Me.BtnAbortImage.Text = "Abort"
        Me.BtnAbortImage.UseVisualStyleBackColor = True
        '
        'CmbTargetFilter
        '
        Me.CmbTargetFilter.FormattingEnabled = True
        Me.CmbTargetFilter.Location = New System.Drawing.Point(144, 11)
        Me.CmbTargetFilter.Name = "CmbTargetFilter"
        Me.CmbTargetFilter.Size = New System.Drawing.Size(54, 21)
        Me.CmbTargetFilter.TabIndex = 20
        Me.CmbTargetFilter.Text = "L"
        '
        'CmbTargetBinning
        '
        Me.CmbTargetBinning.FormattingEnabled = True
        Me.CmbTargetBinning.Items.AddRange(New Object() {"1x1", "2x2", "3x3"})
        Me.CmbTargetBinning.Location = New System.Drawing.Point(204, 11)
        Me.CmbTargetBinning.Name = "CmbTargetBinning"
        Me.CmbTargetBinning.Size = New System.Drawing.Size(54, 21)
        Me.CmbTargetBinning.TabIndex = 21
        Me.CmbTargetBinning.Text = "1x1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Exposure time:"
        '
        'TxtCCDExpsoure
        '
        Me.TxtCCDExpsoure.Location = New System.Drawing.Point(84, 11)
        Me.TxtCCDExpsoure.Name = "TxtCCDExpsoure"
        Me.TxtCCDExpsoure.Size = New System.Drawing.Size(54, 20)
        Me.TxtCCDExpsoure.TabIndex = 11
        Me.TxtCCDExpsoure.Text = "5"
        '
        'BtnTakeImage
        '
        Me.BtnTakeImage.Location = New System.Drawing.Point(264, 9)
        Me.BtnTakeImage.Name = "BtnTakeImage"
        Me.BtnTakeImage.Size = New System.Drawing.Size(54, 26)
        Me.BtnTakeImage.TabIndex = 10
        Me.BtnTakeImage.Text = "Expose"
        Me.BtnTakeImage.UseVisualStyleBackColor = True
        '
        'TimerCCDFocusser
        '
        Me.TimerCCDFocusser.Interval = 500
        '
        'TimerMount
        '
        Me.TimerMount.Interval = 500
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.BtnZipCopyImageSets)
        Me.GroupBox7.Location = New System.Drawing.Point(448, 12)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(109, 89)
        Me.GroupBox7.TabIndex = 67
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "Images"
        '
        'BtnZipCopyImageSets
        '
        Me.BtnZipCopyImageSets.Location = New System.Drawing.Point(7, 19)
        Me.BtnZipCopyImageSets.Name = "BtnZipCopyImageSets"
        Me.BtnZipCopyImageSets.Size = New System.Drawing.Size(98, 44)
        Me.BtnZipCopyImageSets.TabIndex = 69
        Me.BtnZipCopyImageSets.Text = "Zip and copy image sets"
        Me.BtnZipCopyImageSets.UseVisualStyleBackColor = True
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.ChkOpenRoof)
        Me.GroupBox8.Controls.Add(Me.BtnPauzeEquipment)
        Me.GroupBox8.Controls.Add(Me.BtnShutdownEquipment)
        Me.GroupBox8.Controls.Add(Me.BtnStartEquipment)
        Me.GroupBox8.Location = New System.Drawing.Point(161, 3)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(281, 98)
        Me.GroupBox8.TabIndex = 68
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Equipment"
        '
        'ChkOpenRoof
        '
        Me.ChkOpenRoof.AutoSize = True
        Me.ChkOpenRoof.Location = New System.Drawing.Point(6, 76)
        Me.ChkOpenRoof.Name = "ChkOpenRoof"
        Me.ChkOpenRoof.Size = New System.Drawing.Size(73, 17)
        Me.ChkOpenRoof.TabIndex = 72
        Me.ChkOpenRoof.Text = "Open roof"
        Me.ChkOpenRoof.UseVisualStyleBackColor = True
        '
        'BtnPauzeEquipment
        '
        Me.BtnPauzeEquipment.BackColor = System.Drawing.Color.Moccasin
        Me.BtnPauzeEquipment.Location = New System.Drawing.Point(101, 14)
        Me.BtnPauzeEquipment.Name = "BtnPauzeEquipment"
        Me.BtnPauzeEquipment.Size = New System.Drawing.Size(89, 58)
        Me.BtnPauzeEquipment.TabIndex = 71
        Me.BtnPauzeEquipment.Text = "PAUSE EQUIPMENT"
        Me.BtnPauzeEquipment.UseVisualStyleBackColor = False
        '
        'BtnShutdownEquipment
        '
        Me.BtnShutdownEquipment.BackColor = System.Drawing.Color.DarkSalmon
        Me.BtnShutdownEquipment.Location = New System.Drawing.Point(192, 14)
        Me.BtnShutdownEquipment.Name = "BtnShutdownEquipment"
        Me.BtnShutdownEquipment.Size = New System.Drawing.Size(80, 58)
        Me.BtnShutdownEquipment.TabIndex = 70
        Me.BtnShutdownEquipment.Text = "SHUTDOWN EQUIPMENT"
        Me.BtnShutdownEquipment.UseVisualStyleBackColor = False
        '
        'BtnStartEquipment
        '
        Me.BtnStartEquipment.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.BtnStartEquipment.Location = New System.Drawing.Point(3, 15)
        Me.BtnStartEquipment.Name = "BtnStartEquipment"
        Me.BtnStartEquipment.Size = New System.Drawing.Size(96, 58)
        Me.BtnStartEquipment.TabIndex = 69
        Me.BtnStartEquipment.Text = "START EQUIPMENT"
        Me.BtnStartEquipment.UseVisualStyleBackColor = False
        '
        'FrmTools
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(562, 409)
        Me.Controls.Add(Me.GroupBox8)
        Me.Controls.Add(Me.GroupBox7)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.frpDragonFly)
        Me.Controls.Add(Me.grpCover)
        Me.Controls.Add(Me.grpRoofRoof)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmTools"
        Me.Text = "Tools"
        Me.TopMost = True
        Me.grpCover.ResumeLayout(False)
        Me.grpRoofRoof.ResumeLayout(False)
        Me.frpDragonFly.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents grpCover As GroupBox
    Friend WithEvents grpRoofRoof As GroupBox
    Friend WithEvents BtnRoofOpenPCT As Button
    Friend WithEvents TimerRoof As Timer
    Friend WithEvents LblRoof As Label
    Friend WithEvents LblCover As Label
    Friend WithEvents TimerCover As Timer
    Friend WithEvents frpDragonFly As GroupBox
    Friend WithEvents btnStopAllEquipment As Button
    Friend WithEvents btnStartAllEquipment As Button
    Friend WithEvents lblSwitch8Status As Label
    Friend WithEvents lblSwitch3Status As Label
    Friend WithEvents lblSwitch4Status As Label
    Friend WithEvents lblSwitch5Status As Label
    Friend WithEvents lblSwitch6Status As Label
    Friend WithEvents lblSwitch7Status As Label
    Friend WithEvents lblSwitch2Status As Label
    Friend WithEvents lblSwitch1Status As Label
    Friend WithEvents TimerSwitch As Timer
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents BtnDisconnectCover As Button
    Friend WithEvents BtnConnectCover As Button
    Friend WithEvents BtnDisconnectCCD As Button
    Friend WithEvents BtnConnectCCD As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents LblFocusserPosition As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents TimerCCDFocusser As Timer
    Friend WithEvents BtnFocusserOUT As Button
    Friend WithEvents BtnFocusserIN As Button
    Friend WithEvents BtnFocusserMoveTo As Button
    Friend WithEvents TxtFocusserAbsolutePosition As TextBox
    Friend WithEvents TxtRelPosition As TextBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TxtCCDExpsoure As TextBox
    Friend WithEvents BtnTakeImage As Button
    Friend WithEvents CmbTargetFilter As ComboBox
    Friend WithEvents CmbTargetBinning As ComboBox
    Friend WithEvents LblCCDExposureStatus As Label
    Friend WithEvents BtnAbortImage As Button
    Friend WithEvents BtnDisconnectMount As Button
    Friend WithEvents BtnConnectMount As Button
    Friend WithEvents TimerMount As Timer
    Friend WithEvents Label3 As Label
    Friend WithEvents LblSpeed As Label
    Friend WithEvents TxtMountSpeed As TextBox
    Friend WithEvents BtnMountSouth As Button
    Friend WithEvents BtnMountWest As Button
    Friend WithEvents BtnMountEast As Button
    Friend WithEvents BtnMountNorth As Button
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents BtnTargetSlew As Button
    Friend WithEvents BtnTargetFindTSX As Button
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents BtnTargetAbortSlew As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents TxtTargetName As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label15 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents TxtTargetDEC2000DG As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents TxtTargetRA2000SS As TextBox
    Friend WithEvents TxtTargetRA2000HH As TextBox
    Friend WithEvents TxtTargetDEC2000MM As TextBox
    Friend WithEvents TxtTargetDEC2000SS As TextBox
    Friend WithEvents TxtTargetRA2000MM As TextBox
    Friend WithEvents BtnClosedLoopSlew As Button
    Friend WithEvents BtnUnpark As Button
    Friend WithEvents BtnPark As Button
    Friend WithEvents GroupBox7 As GroupBox
    Friend WithEvents BtnZipCopyImageSets As Button
    Friend WithEvents GroupBox8 As GroupBox
    Friend WithEvents BtnShutdownEquipment As Button
    Friend WithEvents BtnStartEquipment As Button
    Friend WithEvents BtnPauzeEquipment As Button
    Friend WithEvents ChkOpenRoof As CheckBox
    Friend WithEvents BtnFocusserAutofocus As Button
End Class
