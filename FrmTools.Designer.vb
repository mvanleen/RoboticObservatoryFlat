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
        Me.TimerRoof = New System.Windows.Forms.Timer(Me.components)
        Me.TimerCover = New System.Windows.Forms.Timer(Me.components)
        Me.TimerSwitch = New System.Windows.Forms.Timer(Me.components)
        Me.BtnUnpark = New System.Windows.Forms.Button()
        Me.BtnPark = New System.Windows.Forms.Button()
        Me.BtnDisconnectMount = New System.Windows.Forms.Button()
        Me.BtnConnectMount = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BtnMountNorth = New System.Windows.Forms.Button()
        Me.LblSpeed = New System.Windows.Forms.Label()
        Me.BtnMountEast = New System.Windows.Forms.Button()
        Me.TxtMountSpeed = New System.Windows.Forms.TextBox()
        Me.BtnMountWest = New System.Windows.Forms.Button()
        Me.BtnMountSouth = New System.Windows.Forms.Button()
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
        Me.LblCCDExposureStatus = New System.Windows.Forms.Label()
        Me.BtnAbortImage = New System.Windows.Forms.Button()
        Me.CmbTargetFilter = New System.Windows.Forms.ComboBox()
        Me.CmbTargetBinning = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtCCDExpsoure = New System.Windows.Forms.TextBox()
        Me.BtnTakeImage = New System.Windows.Forms.Button()
        Me.TimerCCDFocusser = New System.Windows.Forms.Timer(Me.components)
        Me.TimerMount = New System.Windows.Forms.Timer(Me.components)
        Me.BtnZipCopyImageSets = New System.Windows.Forms.Button()
        Me.ChkOpenRoof = New System.Windows.Forms.CheckBox()
        Me.BtnPauzeEquipment = New System.Windows.Forms.Button()
        Me.BtnShutdownEquipment = New System.Windows.Forms.Button()
        Me.BtnStartEquipment = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
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
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.LblRoof = New System.Windows.Forms.Label()
        Me.BtnRoofOpenPCT = New System.Windows.Forms.Button()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.BtnDisconnectCover = New System.Windows.Forms.Button()
        Me.BtnConnectCover = New System.Windows.Forms.Button()
        Me.LblCover = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel8.SuspendLayout()
        Me.SuspendLayout()
        '
        'TimerRoof
        '
        Me.TimerRoof.Interval = 500
        '
        'TimerCover
        '
        Me.TimerCover.Interval = 500
        '
        'TimerSwitch
        '
        Me.TimerSwitch.Interval = 500
        '
        'BtnUnpark
        '
        Me.BtnUnpark.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnUnpark.FlatAppearance.BorderSize = 0
        Me.BtnUnpark.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnUnpark.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnUnpark.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnUnpark.ForeColor = System.Drawing.Color.White
        Me.BtnUnpark.Location = New System.Drawing.Point(9, 104)
        Me.BtnUnpark.Name = "BtnUnpark"
        Me.BtnUnpark.Size = New System.Drawing.Size(73, 26)
        Me.BtnUnpark.TabIndex = 33
        Me.BtnUnpark.Text = "Unpark"
        Me.BtnUnpark.UseVisualStyleBackColor = False
        '
        'BtnPark
        '
        Me.BtnPark.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnPark.FlatAppearance.BorderSize = 0
        Me.BtnPark.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnPark.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPark.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnPark.ForeColor = System.Drawing.Color.White
        Me.BtnPark.Location = New System.Drawing.Point(9, 76)
        Me.BtnPark.Name = "BtnPark"
        Me.BtnPark.Size = New System.Drawing.Size(73, 26)
        Me.BtnPark.TabIndex = 32
        Me.BtnPark.Text = "Park"
        Me.BtnPark.UseVisualStyleBackColor = False
        '
        'BtnDisconnectMount
        '
        Me.BtnDisconnectMount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnDisconnectMount.FlatAppearance.BorderSize = 0
        Me.BtnDisconnectMount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnDisconnectMount.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnDisconnectMount.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDisconnectMount.ForeColor = System.Drawing.Color.White
        Me.BtnDisconnectMount.Location = New System.Drawing.Point(9, 48)
        Me.BtnDisconnectMount.Name = "BtnDisconnectMount"
        Me.BtnDisconnectMount.Size = New System.Drawing.Size(73, 26)
        Me.BtnDisconnectMount.TabIndex = 21
        Me.BtnDisconnectMount.Text = "Diconnect"
        Me.BtnDisconnectMount.UseVisualStyleBackColor = False
        '
        'BtnConnectMount
        '
        Me.BtnConnectMount.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnConnectMount.FlatAppearance.BorderSize = 0
        Me.BtnConnectMount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnConnectMount.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnConnectMount.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnConnectMount.ForeColor = System.Drawing.Color.White
        Me.BtnConnectMount.Location = New System.Drawing.Point(9, 20)
        Me.BtnConnectMount.Name = "BtnConnectMount"
        Me.BtnConnectMount.Size = New System.Drawing.Size(73, 26)
        Me.BtnConnectMount.TabIndex = 20
        Me.BtnConnectMount.Text = "Connect"
        Me.BtnConnectMount.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(163, 111)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(25, 13)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "°/s"
        '
        'BtnMountNorth
        '
        Me.BtnMountNorth.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnMountNorth.FlatAppearance.BorderSize = 0
        Me.BtnMountNorth.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnMountNorth.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnMountNorth.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMountNorth.ForeColor = System.Drawing.Color.White
        Me.BtnMountNorth.Location = New System.Drawing.Point(109, 18)
        Me.BtnMountNorth.Name = "BtnMountNorth"
        Me.BtnMountNorth.Size = New System.Drawing.Size(39, 24)
        Me.BtnMountNorth.TabIndex = 22
        Me.BtnMountNorth.Text = "N"
        Me.BtnMountNorth.UseVisualStyleBackColor = False
        '
        'LblSpeed
        '
        Me.LblSpeed.AutoSize = True
        Me.LblSpeed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSpeed.ForeColor = System.Drawing.Color.White
        Me.LblSpeed.Location = New System.Drawing.Point(87, 111)
        Me.LblSpeed.Name = "LblSpeed"
        Me.LblSpeed.Size = New System.Drawing.Size(45, 13)
        Me.LblSpeed.TabIndex = 28
        Me.LblSpeed.Text = "Speed:"
        '
        'BtnMountEast
        '
        Me.BtnMountEast.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnMountEast.FlatAppearance.BorderSize = 0
        Me.BtnMountEast.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnMountEast.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnMountEast.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMountEast.ForeColor = System.Drawing.Color.White
        Me.BtnMountEast.Location = New System.Drawing.Point(133, 44)
        Me.BtnMountEast.Name = "BtnMountEast"
        Me.BtnMountEast.Size = New System.Drawing.Size(39, 25)
        Me.BtnMountEast.TabIndex = 23
        Me.BtnMountEast.Text = "E"
        Me.BtnMountEast.UseVisualStyleBackColor = False
        '
        'TxtMountSpeed
        '
        Me.TxtMountSpeed.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtMountSpeed.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtMountSpeed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMountSpeed.Location = New System.Drawing.Point(133, 111)
        Me.TxtMountSpeed.Name = "TxtMountSpeed"
        Me.TxtMountSpeed.Size = New System.Drawing.Size(28, 14)
        Me.TxtMountSpeed.TabIndex = 27
        Me.TxtMountSpeed.Text = "1"
        '
        'BtnMountWest
        '
        Me.BtnMountWest.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnMountWest.FlatAppearance.BorderSize = 0
        Me.BtnMountWest.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnMountWest.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnMountWest.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMountWest.ForeColor = System.Drawing.Color.White
        Me.BtnMountWest.Location = New System.Drawing.Point(87, 43)
        Me.BtnMountWest.Name = "BtnMountWest"
        Me.BtnMountWest.Size = New System.Drawing.Size(39, 25)
        Me.BtnMountWest.TabIndex = 24
        Me.BtnMountWest.Text = "W"
        Me.BtnMountWest.UseVisualStyleBackColor = False
        '
        'BtnMountSouth
        '
        Me.BtnMountSouth.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnMountSouth.FlatAppearance.BorderSize = 0
        Me.BtnMountSouth.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnMountSouth.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnMountSouth.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMountSouth.ForeColor = System.Drawing.Color.White
        Me.BtnMountSouth.Location = New System.Drawing.Point(109, 71)
        Me.BtnMountSouth.Name = "BtnMountSouth"
        Me.BtnMountSouth.Size = New System.Drawing.Size(39, 25)
        Me.BtnMountSouth.TabIndex = 26
        Me.BtnMountSouth.Text = "S"
        Me.BtnMountSouth.UseVisualStyleBackColor = False
        '
        'BtnClosedLoopSlew
        '
        Me.BtnClosedLoopSlew.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnClosedLoopSlew.FlatAppearance.BorderSize = 0
        Me.BtnClosedLoopSlew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnClosedLoopSlew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnClosedLoopSlew.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClosedLoopSlew.ForeColor = System.Drawing.Color.White
        Me.BtnClosedLoopSlew.Location = New System.Drawing.Point(245, 102)
        Me.BtnClosedLoopSlew.Name = "BtnClosedLoopSlew"
        Me.BtnClosedLoopSlew.Size = New System.Drawing.Size(89, 25)
        Me.BtnClosedLoopSlew.TabIndex = 27
        Me.BtnClosedLoopSlew.Text = "Closed loop slew"
        Me.BtnClosedLoopSlew.UseVisualStyleBackColor = False
        '
        'BtnTargetAbortSlew
        '
        Me.BtnTargetAbortSlew.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnTargetAbortSlew.FlatAppearance.BorderSize = 0
        Me.BtnTargetAbortSlew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnTargetAbortSlew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTargetAbortSlew.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnTargetAbortSlew.ForeColor = System.Drawing.Color.White
        Me.BtnTargetAbortSlew.Location = New System.Drawing.Point(337, 102)
        Me.BtnTargetAbortSlew.Name = "BtnTargetAbortSlew"
        Me.BtnTargetAbortSlew.Size = New System.Drawing.Size(52, 25)
        Me.BtnTargetAbortSlew.TabIndex = 28
        Me.BtnTargetAbortSlew.Text = "Abort"
        Me.BtnTargetAbortSlew.UseVisualStyleBackColor = False
        '
        'BtnTargetSlew
        '
        Me.BtnTargetSlew.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnTargetSlew.FlatAppearance.BorderSize = 0
        Me.BtnTargetSlew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnTargetSlew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTargetSlew.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnTargetSlew.ForeColor = System.Drawing.Color.White
        Me.BtnTargetSlew.Location = New System.Drawing.Point(198, 102)
        Me.BtnTargetSlew.Name = "BtnTargetSlew"
        Me.BtnTargetSlew.Size = New System.Drawing.Size(44, 25)
        Me.BtnTargetSlew.TabIndex = 26
        Me.BtnTargetSlew.Text = "Slew"
        Me.BtnTargetSlew.UseVisualStyleBackColor = False
        '
        'BtnTargetFindTSX
        '
        Me.BtnTargetFindTSX.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnTargetFindTSX.FlatAppearance.BorderSize = 0
        Me.BtnTargetFindTSX.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnTargetFindTSX.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTargetFindTSX.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnTargetFindTSX.ForeColor = System.Drawing.Color.White
        Me.BtnTargetFindTSX.Location = New System.Drawing.Point(336, 24)
        Me.BtnTargetFindTSX.Name = "BtnTargetFindTSX"
        Me.BtnTargetFindTSX.Size = New System.Drawing.Size(40, 20)
        Me.BtnTargetFindTSX.TabIndex = 25
        Me.BtnTargetFindTSX.Text = "Find"
        Me.BtnTargetFindTSX.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(179, 27)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(42, 13)
        Me.Label5.TabIndex = 23
        Me.Label5.Text = "Name:"
        '
        'TxtTargetName
        '
        Me.TxtTargetName.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtTargetName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtTargetName.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTargetName.Location = New System.Drawing.Point(251, 28)
        Me.TxtTargetName.Name = "TxtTargetName"
        Me.TxtTargetName.Size = New System.Drawing.Size(67, 14)
        Me.TxtTargetName.TabIndex = 24
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(181, 47)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 13)
        Me.Label6.TabIndex = 26
        Me.Label6.Text = "RA  (2000):"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.White
        Me.Label14.Location = New System.Drawing.Point(318, 65)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(18, 13)
        Me.Label14.TabIndex = 38
        Me.Label14.Text = "m"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.White
        Me.Label13.Location = New System.Drawing.Point(278, 65)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(13, 13)
        Me.Label13.TabIndex = 37
        Me.Label13.Text = "°"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(277, 48)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(14, 13)
        Me.Label7.TabIndex = 29
        Me.Label7.Text = "h"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.White
        Me.Label15.Location = New System.Drawing.Point(382, 65)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(13, 13)
        Me.Label15.TabIndex = 39
        Me.Label15.Text = "s"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.White
        Me.Label12.Location = New System.Drawing.Point(181, 65)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(72, 13)
        Me.Label12.TabIndex = 36
        Me.Label12.Text = "DEC (2000):"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(318, 48)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(18, 13)
        Me.Label8.TabIndex = 30
        Me.Label8.Text = "m"
        '
        'TxtTargetDEC2000DG
        '
        Me.TxtTargetDEC2000DG.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtTargetDEC2000DG.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtTargetDEC2000DG.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTargetDEC2000DG.Location = New System.Drawing.Point(251, 65)
        Me.TxtTargetDEC2000DG.Name = "TxtTargetDEC2000DG"
        Me.TxtTargetDEC2000DG.Size = New System.Drawing.Size(26, 14)
        Me.TxtTargetDEC2000DG.TabIndex = 31
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(382, 48)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(13, 13)
        Me.Label9.TabIndex = 32
        Me.Label9.Text = "s"
        '
        'TxtTargetRA2000SS
        '
        Me.TxtTargetRA2000SS.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtTargetRA2000SS.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtTargetRA2000SS.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTargetRA2000SS.Location = New System.Drawing.Point(336, 48)
        Me.TxtTargetRA2000SS.Name = "TxtTargetRA2000SS"
        Me.TxtTargetRA2000SS.Size = New System.Drawing.Size(40, 14)
        Me.TxtTargetRA2000SS.TabIndex = 28
        '
        'TxtTargetRA2000HH
        '
        Me.TxtTargetRA2000HH.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtTargetRA2000HH.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtTargetRA2000HH.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTargetRA2000HH.Location = New System.Drawing.Point(251, 47)
        Me.TxtTargetRA2000HH.Name = "TxtTargetRA2000HH"
        Me.TxtTargetRA2000HH.Size = New System.Drawing.Size(26, 14)
        Me.TxtTargetRA2000HH.TabIndex = 25
        '
        'TxtTargetDEC2000MM
        '
        Me.TxtTargetDEC2000MM.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtTargetDEC2000MM.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtTargetDEC2000MM.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTargetDEC2000MM.Location = New System.Drawing.Point(292, 65)
        Me.TxtTargetDEC2000MM.Name = "TxtTargetDEC2000MM"
        Me.TxtTargetDEC2000MM.Size = New System.Drawing.Size(26, 14)
        Me.TxtTargetDEC2000MM.TabIndex = 33
        '
        'TxtTargetDEC2000SS
        '
        Me.TxtTargetDEC2000SS.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtTargetDEC2000SS.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtTargetDEC2000SS.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTargetDEC2000SS.Location = New System.Drawing.Point(336, 65)
        Me.TxtTargetDEC2000SS.Name = "TxtTargetDEC2000SS"
        Me.TxtTargetDEC2000SS.Size = New System.Drawing.Size(40, 14)
        Me.TxtTargetDEC2000SS.TabIndex = 34
        '
        'TxtTargetRA2000MM
        '
        Me.TxtTargetRA2000MM.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtTargetRA2000MM.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtTargetRA2000MM.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTargetRA2000MM.Location = New System.Drawing.Point(292, 48)
        Me.TxtTargetRA2000MM.Name = "TxtTargetRA2000MM"
        Me.TxtTargetRA2000MM.Size = New System.Drawing.Size(26, 14)
        Me.TxtTargetRA2000MM.TabIndex = 27
        '
        'BtnFocusserAutofocus
        '
        Me.BtnFocusserAutofocus.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnFocusserAutofocus.FlatAppearance.BorderSize = 0
        Me.BtnFocusserAutofocus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnFocusserAutofocus.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnFocusserAutofocus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnFocusserAutofocus.ForeColor = System.Drawing.Color.White
        Me.BtnFocusserAutofocus.Location = New System.Drawing.Point(129, 34)
        Me.BtnFocusserAutofocus.Name = "BtnFocusserAutofocus"
        Me.BtnFocusserAutofocus.Size = New System.Drawing.Size(75, 23)
        Me.BtnFocusserAutofocus.TabIndex = 10
        Me.BtnFocusserAutofocus.Text = "Autofocus"
        Me.BtnFocusserAutofocus.UseVisualStyleBackColor = False
        '
        'BtnFocusserMoveTo
        '
        Me.BtnFocusserMoveTo.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnFocusserMoveTo.FlatAppearance.BorderSize = 0
        Me.BtnFocusserMoveTo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnFocusserMoveTo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnFocusserMoveTo.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnFocusserMoveTo.ForeColor = System.Drawing.Color.White
        Me.BtnFocusserMoveTo.Location = New System.Drawing.Point(66, 34)
        Me.BtnFocusserMoveTo.Name = "BtnFocusserMoveTo"
        Me.BtnFocusserMoveTo.Size = New System.Drawing.Size(57, 23)
        Me.BtnFocusserMoveTo.TabIndex = 9
        Me.BtnFocusserMoveTo.Text = "Move to"
        Me.BtnFocusserMoveTo.UseVisualStyleBackColor = False
        '
        'TxtFocusserAbsolutePosition
        '
        Me.TxtFocusserAbsolutePosition.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtFocusserAbsolutePosition.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtFocusserAbsolutePosition.Location = New System.Drawing.Point(9, 36)
        Me.TxtFocusserAbsolutePosition.Name = "TxtFocusserAbsolutePosition"
        Me.TxtFocusserAbsolutePosition.Size = New System.Drawing.Size(51, 21)
        Me.TxtFocusserAbsolutePosition.TabIndex = 8
        '
        'TxtRelPosition
        '
        Me.TxtRelPosition.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtRelPosition.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtRelPosition.Location = New System.Drawing.Point(280, 36)
        Me.TxtRelPosition.Name = "TxtRelPosition"
        Me.TxtRelPosition.Size = New System.Drawing.Size(25, 21)
        Me.TxtRelPosition.TabIndex = 7
        Me.TxtRelPosition.Text = "50"
        '
        'BtnFocusserOUT
        '
        Me.BtnFocusserOUT.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnFocusserOUT.FlatAppearance.BorderSize = 0
        Me.BtnFocusserOUT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnFocusserOUT.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnFocusserOUT.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnFocusserOUT.ForeColor = System.Drawing.Color.White
        Me.BtnFocusserOUT.Location = New System.Drawing.Point(311, 34)
        Me.BtnFocusserOUT.Name = "BtnFocusserOUT"
        Me.BtnFocusserOUT.Size = New System.Drawing.Size(39, 23)
        Me.BtnFocusserOUT.TabIndex = 6
        Me.BtnFocusserOUT.Text = "OUT"
        Me.BtnFocusserOUT.UseVisualStyleBackColor = False
        '
        'BtnFocusserIN
        '
        Me.BtnFocusserIN.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnFocusserIN.FlatAppearance.BorderSize = 0
        Me.BtnFocusserIN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnFocusserIN.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnFocusserIN.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnFocusserIN.ForeColor = System.Drawing.Color.White
        Me.BtnFocusserIN.Location = New System.Drawing.Point(238, 34)
        Me.BtnFocusserIN.Name = "BtnFocusserIN"
        Me.BtnFocusserIN.Size = New System.Drawing.Size(39, 23)
        Me.BtnFocusserIN.TabIndex = 5
        Me.BtnFocusserIN.Text = "IN"
        Me.BtnFocusserIN.UseVisualStyleBackColor = False
        '
        'LblFocusserPosition
        '
        Me.LblFocusserPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LblFocusserPosition.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFocusserPosition.Location = New System.Drawing.Point(109, 5)
        Me.LblFocusserPosition.Name = "LblFocusserPosition"
        Me.LblFocusserPosition.Size = New System.Drawing.Size(95, 20)
        Me.LblFocusserPosition.TabIndex = 4
        Me.LblFocusserPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(52, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Position:"
        '
        'BtnDisconnectCCD
        '
        Me.BtnDisconnectCCD.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnDisconnectCCD.FlatAppearance.BorderSize = 0
        Me.BtnDisconnectCCD.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnDisconnectCCD.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnDisconnectCCD.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDisconnectCCD.ForeColor = System.Drawing.Color.White
        Me.BtnDisconnectCCD.Location = New System.Drawing.Point(9, 55)
        Me.BtnDisconnectCCD.Name = "BtnDisconnectCCD"
        Me.BtnDisconnectCCD.Size = New System.Drawing.Size(85, 26)
        Me.BtnDisconnectCCD.TabIndex = 19
        Me.BtnDisconnectCCD.Text = "Disconnect"
        Me.BtnDisconnectCCD.UseVisualStyleBackColor = False
        '
        'BtnConnectCCD
        '
        Me.BtnConnectCCD.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnConnectCCD.FlatAppearance.BorderSize = 0
        Me.BtnConnectCCD.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnConnectCCD.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnConnectCCD.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnConnectCCD.ForeColor = System.Drawing.Color.White
        Me.BtnConnectCCD.Location = New System.Drawing.Point(9, 27)
        Me.BtnConnectCCD.Name = "BtnConnectCCD"
        Me.BtnConnectCCD.Size = New System.Drawing.Size(85, 26)
        Me.BtnConnectCCD.TabIndex = 18
        Me.BtnConnectCCD.Text = "Connect"
        Me.BtnConnectCCD.UseVisualStyleBackColor = False
        '
        'LblCCDExposureStatus
        '
        Me.LblCCDExposureStatus.AutoSize = True
        Me.LblCCDExposureStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCCDExposureStatus.ForeColor = System.Drawing.Color.White
        Me.LblCCDExposureStatus.Location = New System.Drawing.Point(98, 62)
        Me.LblCCDExposureStatus.Name = "LblCCDExposureStatus"
        Me.LblCCDExposureStatus.Size = New System.Drawing.Size(134, 13)
        Me.LblCCDExposureStatus.TabIndex = 25
        Me.LblCCDExposureStatus.Text = "LblCCDExposureStatus"
        '
        'BtnAbortImage
        '
        Me.BtnAbortImage.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnAbortImage.FlatAppearance.BorderSize = 0
        Me.BtnAbortImage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnAbortImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnAbortImage.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbortImage.ForeColor = System.Drawing.Color.White
        Me.BtnAbortImage.Location = New System.Drawing.Point(238, 55)
        Me.BtnAbortImage.Name = "BtnAbortImage"
        Me.BtnAbortImage.Size = New System.Drawing.Size(59, 26)
        Me.BtnAbortImage.TabIndex = 26
        Me.BtnAbortImage.Text = "Abort"
        Me.BtnAbortImage.UseVisualStyleBackColor = False
        '
        'CmbTargetFilter
        '
        Me.CmbTargetFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.CmbTargetFilter.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbTargetFilter.FormattingEnabled = True
        Me.CmbTargetFilter.Location = New System.Drawing.Point(252, 31)
        Me.CmbTargetFilter.Name = "CmbTargetFilter"
        Me.CmbTargetFilter.Size = New System.Drawing.Size(54, 21)
        Me.CmbTargetFilter.TabIndex = 20
        Me.CmbTargetFilter.Text = "L"
        '
        'CmbTargetBinning
        '
        Me.CmbTargetBinning.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.CmbTargetBinning.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbTargetBinning.FormattingEnabled = True
        Me.CmbTargetBinning.Items.AddRange(New Object() {"1x1", "2x2", "3x3"})
        Me.CmbTargetBinning.Location = New System.Drawing.Point(306, 31)
        Me.CmbTargetBinning.Name = "CmbTargetBinning"
        Me.CmbTargetBinning.Size = New System.Drawing.Size(54, 21)
        Me.CmbTargetBinning.TabIndex = 21
        Me.CmbTargetBinning.Text = "1x1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(100, 34)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(91, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Exposure time:"
        '
        'TxtCCDExpsoure
        '
        Me.TxtCCDExpsoure.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtCCDExpsoure.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtCCDExpsoure.Location = New System.Drawing.Point(197, 31)
        Me.TxtCCDExpsoure.Name = "TxtCCDExpsoure"
        Me.TxtCCDExpsoure.Size = New System.Drawing.Size(54, 21)
        Me.TxtCCDExpsoure.TabIndex = 11
        Me.TxtCCDExpsoure.Text = "5"
        '
        'BtnTakeImage
        '
        Me.BtnTakeImage.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnTakeImage.FlatAppearance.BorderSize = 0
        Me.BtnTakeImage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnTakeImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTakeImage.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnTakeImage.ForeColor = System.Drawing.Color.White
        Me.BtnTakeImage.Location = New System.Drawing.Point(300, 55)
        Me.BtnTakeImage.Name = "BtnTakeImage"
        Me.BtnTakeImage.Size = New System.Drawing.Size(60, 26)
        Me.BtnTakeImage.TabIndex = 10
        Me.BtnTakeImage.Text = "Expose"
        Me.BtnTakeImage.UseVisualStyleBackColor = False
        '
        'TimerCCDFocusser
        '
        Me.TimerCCDFocusser.Interval = 500
        '
        'TimerMount
        '
        Me.TimerMount.Interval = 500
        '
        'BtnZipCopyImageSets
        '
        Me.BtnZipCopyImageSets.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnZipCopyImageSets.FlatAppearance.BorderSize = 0
        Me.BtnZipCopyImageSets.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnZipCopyImageSets.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnZipCopyImageSets.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnZipCopyImageSets.ForeColor = System.Drawing.Color.White
        Me.BtnZipCopyImageSets.Location = New System.Drawing.Point(278, 19)
        Me.BtnZipCopyImageSets.Name = "BtnZipCopyImageSets"
        Me.BtnZipCopyImageSets.Size = New System.Drawing.Size(98, 44)
        Me.BtnZipCopyImageSets.TabIndex = 69
        Me.BtnZipCopyImageSets.Text = "Zip and copy image sets"
        Me.BtnZipCopyImageSets.UseVisualStyleBackColor = False
        '
        'ChkOpenRoof
        '
        Me.ChkOpenRoof.AutoSize = True
        Me.ChkOpenRoof.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ChkOpenRoof.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkOpenRoof.ForeColor = System.Drawing.Color.White
        Me.ChkOpenRoof.Location = New System.Drawing.Point(6, 64)
        Me.ChkOpenRoof.Name = "ChkOpenRoof"
        Me.ChkOpenRoof.Size = New System.Drawing.Size(78, 17)
        Me.ChkOpenRoof.TabIndex = 72
        Me.ChkOpenRoof.Text = "Open roof"
        Me.ChkOpenRoof.UseVisualStyleBackColor = True
        '
        'BtnPauzeEquipment
        '
        Me.BtnPauzeEquipment.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(147, Byte), Integer), CType(CType(43, Byte), Integer))
        Me.BtnPauzeEquipment.FlatAppearance.BorderSize = 0
        Me.BtnPauzeEquipment.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPauzeEquipment.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnPauzeEquipment.ForeColor = System.Drawing.Color.White
        Me.BtnPauzeEquipment.Location = New System.Drawing.Point(101, 19)
        Me.BtnPauzeEquipment.Name = "BtnPauzeEquipment"
        Me.BtnPauzeEquipment.Size = New System.Drawing.Size(85, 44)
        Me.BtnPauzeEquipment.TabIndex = 71
        Me.BtnPauzeEquipment.Text = "PAUSE EQUIPMENT"
        Me.BtnPauzeEquipment.UseVisualStyleBackColor = False
        '
        'BtnShutdownEquipment
        '
        Me.BtnShutdownEquipment.BackColor = System.Drawing.Color.FromArgb(CType(CType(214, Byte), Integer), CType(CType(48, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.BtnShutdownEquipment.FlatAppearance.BorderSize = 0
        Me.BtnShutdownEquipment.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnShutdownEquipment.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnShutdownEquipment.ForeColor = System.Drawing.Color.White
        Me.BtnShutdownEquipment.Location = New System.Drawing.Point(192, 19)
        Me.BtnShutdownEquipment.Name = "BtnShutdownEquipment"
        Me.BtnShutdownEquipment.Size = New System.Drawing.Size(80, 44)
        Me.BtnShutdownEquipment.TabIndex = 70
        Me.BtnShutdownEquipment.Text = "SHUTDOWN EQUIPMENT"
        Me.BtnShutdownEquipment.UseVisualStyleBackColor = False
        '
        'BtnStartEquipment
        '
        Me.BtnStartEquipment.BackColor = System.Drawing.Color.FromArgb(CType(CType(76, Byte), Integer), CType(CType(209, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.BtnStartEquipment.FlatAppearance.BorderSize = 0
        Me.BtnStartEquipment.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnStartEquipment.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStartEquipment.ForeColor = System.Drawing.Color.White
        Me.BtnStartEquipment.Location = New System.Drawing.Point(3, 20)
        Me.BtnStartEquipment.Name = "BtnStartEquipment"
        Me.BtnStartEquipment.Size = New System.Drawing.Size(92, 43)
        Me.BtnStartEquipment.TabIndex = 69
        Me.BtnStartEquipment.Text = "START EQUIPMENT"
        Me.BtnStartEquipment.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.btnStopAllEquipment)
        Me.Panel2.Controls.Add(Me.btnStartAllEquipment)
        Me.Panel2.Controls.Add(Me.lblSwitch8Status)
        Me.Panel2.Controls.Add(Me.lblSwitch3Status)
        Me.Panel2.Controls.Add(Me.lblSwitch4Status)
        Me.Panel2.Controls.Add(Me.lblSwitch5Status)
        Me.Panel2.Controls.Add(Me.lblSwitch6Status)
        Me.Panel2.Controls.Add(Me.lblSwitch7Status)
        Me.Panel2.Controls.Add(Me.lblSwitch2Status)
        Me.Panel2.Controls.Add(Me.lblSwitch1Status)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Location = New System.Drawing.Point(4, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(151, 219)
        Me.Panel2.TabIndex = 109
        '
        'btnStopAllEquipment
        '
        Me.btnStopAllEquipment.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.btnStopAllEquipment.FlatAppearance.BorderSize = 0
        Me.btnStopAllEquipment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.btnStopAllEquipment.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnStopAllEquipment.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStopAllEquipment.ForeColor = System.Drawing.Color.White
        Me.btnStopAllEquipment.Location = New System.Drawing.Point(76, 192)
        Me.btnStopAllEquipment.Name = "btnStopAllEquipment"
        Me.btnStopAllEquipment.Size = New System.Drawing.Size(69, 25)
        Me.btnStopAllEquipment.TabIndex = 51
        Me.btnStopAllEquipment.Text = "OFF"
        Me.btnStopAllEquipment.UseVisualStyleBackColor = False
        '
        'btnStartAllEquipment
        '
        Me.btnStartAllEquipment.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.btnStartAllEquipment.FlatAppearance.BorderSize = 0
        Me.btnStartAllEquipment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.btnStartAllEquipment.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnStartAllEquipment.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStartAllEquipment.ForeColor = System.Drawing.Color.White
        Me.btnStartAllEquipment.Location = New System.Drawing.Point(6, 192)
        Me.btnStartAllEquipment.Name = "btnStartAllEquipment"
        Me.btnStartAllEquipment.Size = New System.Drawing.Size(68, 26)
        Me.btnStartAllEquipment.TabIndex = 50
        Me.btnStartAllEquipment.Text = "ON"
        Me.btnStartAllEquipment.UseVisualStyleBackColor = False
        '
        'lblSwitch8Status
        '
        Me.lblSwitch8Status.BackColor = System.Drawing.Color.Silver
        Me.lblSwitch8Status.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch8Status.ForeColor = System.Drawing.Color.White
        Me.lblSwitch8Status.Location = New System.Drawing.Point(6, 169)
        Me.lblSwitch8Status.Name = "lblSwitch8Status"
        Me.lblSwitch8Status.Size = New System.Drawing.Size(139, 20)
        Me.lblSwitch8Status.TabIndex = 49
        Me.lblSwitch8Status.Text = "Text"
        Me.lblSwitch8Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch3Status
        '
        Me.lblSwitch3Status.BackColor = System.Drawing.Color.Silver
        Me.lblSwitch3Status.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch3Status.ForeColor = System.Drawing.Color.White
        Me.lblSwitch3Status.Location = New System.Drawing.Point(6, 64)
        Me.lblSwitch3Status.Name = "lblSwitch3Status"
        Me.lblSwitch3Status.Size = New System.Drawing.Size(139, 20)
        Me.lblSwitch3Status.TabIndex = 48
        Me.lblSwitch3Status.Text = "Text"
        Me.lblSwitch3Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch4Status
        '
        Me.lblSwitch4Status.BackColor = System.Drawing.Color.Silver
        Me.lblSwitch4Status.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch4Status.ForeColor = System.Drawing.Color.White
        Me.lblSwitch4Status.Location = New System.Drawing.Point(6, 85)
        Me.lblSwitch4Status.Name = "lblSwitch4Status"
        Me.lblSwitch4Status.Size = New System.Drawing.Size(139, 20)
        Me.lblSwitch4Status.TabIndex = 47
        Me.lblSwitch4Status.Text = "Text"
        Me.lblSwitch4Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch5Status
        '
        Me.lblSwitch5Status.BackColor = System.Drawing.Color.Silver
        Me.lblSwitch5Status.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblSwitch5Status.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch5Status.ForeColor = System.Drawing.Color.White
        Me.lblSwitch5Status.Location = New System.Drawing.Point(6, 106)
        Me.lblSwitch5Status.Name = "lblSwitch5Status"
        Me.lblSwitch5Status.Size = New System.Drawing.Size(139, 20)
        Me.lblSwitch5Status.TabIndex = 46
        Me.lblSwitch5Status.Text = "Text"
        Me.lblSwitch5Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch6Status
        '
        Me.lblSwitch6Status.BackColor = System.Drawing.Color.Silver
        Me.lblSwitch6Status.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch6Status.ForeColor = System.Drawing.Color.White
        Me.lblSwitch6Status.Location = New System.Drawing.Point(6, 127)
        Me.lblSwitch6Status.Name = "lblSwitch6Status"
        Me.lblSwitch6Status.Size = New System.Drawing.Size(139, 20)
        Me.lblSwitch6Status.TabIndex = 45
        Me.lblSwitch6Status.Text = "Text"
        Me.lblSwitch6Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch7Status
        '
        Me.lblSwitch7Status.BackColor = System.Drawing.Color.Silver
        Me.lblSwitch7Status.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch7Status.ForeColor = System.Drawing.Color.White
        Me.lblSwitch7Status.Location = New System.Drawing.Point(6, 148)
        Me.lblSwitch7Status.Name = "lblSwitch7Status"
        Me.lblSwitch7Status.Size = New System.Drawing.Size(139, 20)
        Me.lblSwitch7Status.TabIndex = 44
        Me.lblSwitch7Status.Text = "Text"
        Me.lblSwitch7Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch2Status
        '
        Me.lblSwitch2Status.BackColor = System.Drawing.Color.Silver
        Me.lblSwitch2Status.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch2Status.ForeColor = System.Drawing.Color.White
        Me.lblSwitch2Status.Location = New System.Drawing.Point(6, 43)
        Me.lblSwitch2Status.Name = "lblSwitch2Status"
        Me.lblSwitch2Status.Size = New System.Drawing.Size(139, 20)
        Me.lblSwitch2Status.TabIndex = 43
        Me.lblSwitch2Status.Text = "Text"
        Me.lblSwitch2Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblSwitch1Status
        '
        Me.lblSwitch1Status.BackColor = System.Drawing.Color.Silver
        Me.lblSwitch1Status.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSwitch1Status.ForeColor = System.Drawing.Color.White
        Me.lblSwitch1Status.Location = New System.Drawing.Point(6, 22)
        Me.lblSwitch1Status.Name = "lblSwitch1Status"
        Me.lblSwitch1Status.Size = New System.Drawing.Size(139, 20)
        Me.lblSwitch1Status.TabIndex = 42
        Me.lblSwitch1Status.Text = "Text"
        Me.lblSwitch1Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(6, 4)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 41
        Me.Label4.Text = "Dragonfly"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.LblRoof)
        Me.Panel1.Controls.Add(Me.BtnRoofOpenPCT)
        Me.Panel1.Controls.Add(Me.Label22)
        Me.Panel1.Location = New System.Drawing.Point(4, 221)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(151, 79)
        Me.Panel1.TabIndex = 110
        '
        'LblRoof
        '
        Me.LblRoof.BackColor = System.Drawing.Color.Silver
        Me.LblRoof.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRoof.ForeColor = System.Drawing.Color.White
        Me.LblRoof.Location = New System.Drawing.Point(4, 20)
        Me.LblRoof.Name = "LblRoof"
        Me.LblRoof.Size = New System.Drawing.Size(142, 26)
        Me.LblRoof.TabIndex = 66
        Me.LblRoof.Text = "Roof"
        Me.LblRoof.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnRoofOpenPCT
        '
        Me.BtnRoofOpenPCT.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnRoofOpenPCT.FlatAppearance.BorderSize = 0
        Me.BtnRoofOpenPCT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnRoofOpenPCT.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnRoofOpenPCT.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRoofOpenPCT.ForeColor = System.Drawing.Color.White
        Me.BtnRoofOpenPCT.Location = New System.Drawing.Point(3, 49)
        Me.BtnRoofOpenPCT.Name = "BtnRoofOpenPCT"
        Me.BtnRoofOpenPCT.Size = New System.Drawing.Size(143, 25)
        Me.BtnRoofOpenPCT.TabIndex = 65
        Me.BtnRoofOpenPCT.Text = "Open partly"
        Me.BtnRoofOpenPCT.UseVisualStyleBackColor = False
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ForeColor = System.Drawing.Color.White
        Me.Label22.Location = New System.Drawing.Point(6, 4)
        Me.Label22.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(33, 13)
        Me.Label22.TabIndex = 41
        Me.Label22.Text = "Roof"
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.BtnDisconnectCover)
        Me.Panel3.Controls.Add(Me.BtnConnectCover)
        Me.Panel3.Controls.Add(Me.LblCover)
        Me.Panel3.Controls.Add(Me.Label11)
        Me.Panel3.Location = New System.Drawing.Point(4, 294)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(151, 84)
        Me.Panel3.TabIndex = 111
        '
        'BtnDisconnectCover
        '
        Me.BtnDisconnectCover.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnDisconnectCover.FlatAppearance.BorderSize = 0
        Me.BtnDisconnectCover.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnDisconnectCover.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnDisconnectCover.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDisconnectCover.ForeColor = System.Drawing.Color.White
        Me.BtnDisconnectCover.Location = New System.Drawing.Point(64, 49)
        Me.BtnDisconnectCover.Name = "BtnDisconnectCover"
        Me.BtnDisconnectCover.Size = New System.Drawing.Size(82, 25)
        Me.BtnDisconnectCover.TabIndex = 73
        Me.BtnDisconnectCover.Text = "Disconnect"
        Me.BtnDisconnectCover.UseVisualStyleBackColor = False
        '
        'BtnConnectCover
        '
        Me.BtnConnectCover.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnConnectCover.FlatAppearance.BorderSize = 0
        Me.BtnConnectCover.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnConnectCover.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnConnectCover.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnConnectCover.ForeColor = System.Drawing.Color.White
        Me.BtnConnectCover.Location = New System.Drawing.Point(1, 49)
        Me.BtnConnectCover.Name = "BtnConnectCover"
        Me.BtnConnectCover.Size = New System.Drawing.Size(61, 25)
        Me.BtnConnectCover.TabIndex = 72
        Me.BtnConnectCover.Text = "Connect"
        Me.BtnConnectCover.UseVisualStyleBackColor = False
        '
        'LblCover
        '
        Me.LblCover.BackColor = System.Drawing.Color.Silver
        Me.LblCover.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCover.ForeColor = System.Drawing.Color.White
        Me.LblCover.Location = New System.Drawing.Point(4, 20)
        Me.LblCover.Name = "LblCover"
        Me.LblCover.Size = New System.Drawing.Size(142, 26)
        Me.LblCover.TabIndex = 71
        Me.LblCover.Text = "Cover"
        Me.LblCover.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(6, 4)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(40, 13)
        Me.Label11.TabIndex = 41
        Me.Label11.Text = "Cover"
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.BtnZipCopyImageSets)
        Me.Panel4.Controls.Add(Me.ChkOpenRoof)
        Me.Panel4.Controls.Add(Me.Label16)
        Me.Panel4.Controls.Add(Me.BtnPauzeEquipment)
        Me.Panel4.Controls.Add(Me.BtnStartEquipment)
        Me.Panel4.Controls.Add(Me.BtnShutdownEquipment)
        Me.Panel4.Location = New System.Drawing.Point(157, 3)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(399, 85)
        Me.Panel4.TabIndex = 112
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(6, 4)
        Me.Label16.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(67, 13)
        Me.Label16.TabIndex = 41
        Me.Label16.Text = "Equipment"
        '
        'Panel5
        '
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.BtnFocusserAutofocus)
        Me.Panel5.Controls.Add(Me.Label10)
        Me.Panel5.Controls.Add(Me.BtnFocusserMoveTo)
        Me.Panel5.Controls.Add(Me.BtnFocusserOUT)
        Me.Panel5.Controls.Add(Me.TxtFocusserAbsolutePosition)
        Me.Panel5.Controls.Add(Me.Label1)
        Me.Panel5.Controls.Add(Me.TxtRelPosition)
        Me.Panel5.Controls.Add(Me.LblFocusserPosition)
        Me.Panel5.Controls.Add(Me.BtnFocusserIN)
        Me.Panel5.Location = New System.Drawing.Point(157, 320)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(399, 68)
        Me.Panel5.TabIndex = 113
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(6, 4)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(29, 13)
        Me.Label10.TabIndex = 41
        Me.Label10.Text = "CCD"
        '
        'Panel7
        '
        Me.Panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel7.Controls.Add(Me.Label5)
        Me.Panel7.Controls.Add(Me.BtnClosedLoopSlew)
        Me.Panel7.Controls.Add(Me.BtnUnpark)
        Me.Panel7.Controls.Add(Me.BtnTargetAbortSlew)
        Me.Panel7.Controls.Add(Me.Label3)
        Me.Panel7.Controls.Add(Me.BtnTargetSlew)
        Me.Panel7.Controls.Add(Me.BtnPark)
        Me.Panel7.Controls.Add(Me.BtnTargetFindTSX)
        Me.Panel7.Controls.Add(Me.Label18)
        Me.Panel7.Controls.Add(Me.BtnDisconnectMount)
        Me.Panel7.Controls.Add(Me.TxtTargetName)
        Me.Panel7.Controls.Add(Me.Label6)
        Me.Panel7.Controls.Add(Me.BtnMountNorth)
        Me.Panel7.Controls.Add(Me.Label14)
        Me.Panel7.Controls.Add(Me.BtnConnectMount)
        Me.Panel7.Controls.Add(Me.Label13)
        Me.Panel7.Controls.Add(Me.BtnMountWest)
        Me.Panel7.Controls.Add(Me.Label7)
        Me.Panel7.Controls.Add(Me.LblSpeed)
        Me.Panel7.Controls.Add(Me.Label15)
        Me.Panel7.Controls.Add(Me.BtnMountSouth)
        Me.Panel7.Controls.Add(Me.Label12)
        Me.Panel7.Controls.Add(Me.BtnMountEast)
        Me.Panel7.Controls.Add(Me.Label8)
        Me.Panel7.Controls.Add(Me.TxtMountSpeed)
        Me.Panel7.Controls.Add(Me.TxtTargetDEC2000DG)
        Me.Panel7.Controls.Add(Me.TxtTargetRA2000MM)
        Me.Panel7.Controls.Add(Me.Label9)
        Me.Panel7.Controls.Add(Me.TxtTargetDEC2000SS)
        Me.Panel7.Controls.Add(Me.TxtTargetRA2000SS)
        Me.Panel7.Controls.Add(Me.TxtTargetDEC2000MM)
        Me.Panel7.Controls.Add(Me.TxtTargetRA2000HH)
        Me.Panel7.Location = New System.Drawing.Point(157, 90)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(399, 135)
        Me.Panel7.TabIndex = 113
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.Color.White
        Me.Label18.Location = New System.Drawing.Point(6, 4)
        Me.Label18.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(43, 13)
        Me.Label18.TabIndex = 41
        Me.Label18.Text = "Mount"
        '
        'Panel8
        '
        Me.Panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel8.Controls.Add(Me.LblCCDExposureStatus)
        Me.Panel8.Controls.Add(Me.BtnDisconnectCCD)
        Me.Panel8.Controls.Add(Me.BtnAbortImage)
        Me.Panel8.Controls.Add(Me.Label17)
        Me.Panel8.Controls.Add(Me.CmbTargetFilter)
        Me.Panel8.Controls.Add(Me.CmbTargetBinning)
        Me.Panel8.Controls.Add(Me.BtnConnectCCD)
        Me.Panel8.Controls.Add(Me.Label2)
        Me.Panel8.Controls.Add(Me.BtnTakeImage)
        Me.Panel8.Controls.Add(Me.TxtCCDExpsoure)
        Me.Panel8.Location = New System.Drawing.Point(157, 227)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(399, 92)
        Me.Panel8.TabIndex = 114
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.White
        Me.Label17.Location = New System.Drawing.Point(6, 4)
        Me.Label17.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(29, 13)
        Me.Label17.TabIndex = 41
        Me.Label17.Text = "CCD"
        '
        'FrmTools
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(99, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(114, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(562, 394)
        Me.Controls.Add(Me.Panel8)
        Me.Controls.Add(Me.Panel7)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmTools"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tools"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        Me.Panel8.ResumeLayout(False)
        Me.Panel8.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TimerRoof As Timer
    Friend WithEvents TimerCover As Timer
    Friend WithEvents TimerSwitch As Timer
    Friend WithEvents BtnDisconnectCCD As Button
    Friend WithEvents BtnConnectCCD As Button
    Friend WithEvents LblFocusserPosition As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents TimerCCDFocusser As Timer
    Friend WithEvents BtnFocusserOUT As Button
    Friend WithEvents BtnFocusserIN As Button
    Friend WithEvents BtnFocusserMoveTo As Button
    Friend WithEvents TxtFocusserAbsolutePosition As TextBox
    Friend WithEvents TxtRelPosition As TextBox
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
    Friend WithEvents BtnTargetSlew As Button
    Friend WithEvents BtnTargetFindTSX As Button
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
    Friend WithEvents BtnZipCopyImageSets As Button
    Friend WithEvents BtnShutdownEquipment As Button
    Friend WithEvents BtnStartEquipment As Button
    Friend WithEvents BtnPauzeEquipment As Button
    Friend WithEvents ChkOpenRoof As CheckBox
    Friend WithEvents BtnFocusserAutofocus As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label4 As Label
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
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label22 As Label
    Friend WithEvents LblRoof As Label
    Friend WithEvents BtnRoofOpenPCT As Button
    Friend WithEvents Panel3 As Panel
    Friend WithEvents BtnDisconnectCover As Button
    Friend WithEvents BtnConnectCover As Button
    Friend WithEvents LblCover As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Label16 As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label10 As Label
    Friend WithEvents Panel7 As Panel
    Friend WithEvents Label18 As Label
    Friend WithEvents Panel8 As Panel
    Friend WithEvents Label17 As Label
End Class
