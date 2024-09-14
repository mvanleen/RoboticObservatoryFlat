<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmCalibration
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmCalibration))
        Me.BtnStart = New System.Windows.Forms.Button()
        Me.BtnStop = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ChkDarks5 = New System.Windows.Forms.CheckBox()
        Me.TxtDarkExposureDuration5 = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.ChkDarks4 = New System.Windows.Forms.CheckBox()
        Me.ChkDarks3 = New System.Windows.Forms.CheckBox()
        Me.ChkDarks2 = New System.Windows.Forms.CheckBox()
        Me.TxtDarkExposureDuration4 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TxtDarkExposureDuration3 = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TxtDarkExposureDuration2 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.CmbTargetFilter = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.CmbTargetBinning = New System.Windows.Forms.ComboBox()
        Me.TxtBiasNumberExposures = New System.Windows.Forms.TextBox()
        Me.TxtDarkNumberExposures = New System.Windows.Forms.TextBox()
        Me.TxtDarkExposureDuration1 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ChkBias = New System.Windows.Forms.CheckBox()
        Me.ChkDarks1 = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnStart
        '
        Me.BtnStart.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnStart.FlatAppearance.BorderSize = 0
        Me.BtnStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnStart.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStart.ForeColor = System.Drawing.Color.White
        Me.BtnStart.Location = New System.Drawing.Point(2, 213)
        Me.BtnStart.Name = "BtnStart"
        Me.BtnStart.Size = New System.Drawing.Size(75, 23)
        Me.BtnStart.TabIndex = 10
        Me.BtnStart.Text = "Start"
        Me.BtnStart.UseVisualStyleBackColor = False
        '
        'BtnStop
        '
        Me.BtnStop.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnStop.FlatAppearance.BorderSize = 0
        Me.BtnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnStop.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStop.ForeColor = System.Drawing.Color.White
        Me.BtnStop.Location = New System.Drawing.Point(224, 213)
        Me.BtnStop.Name = "BtnStop"
        Me.BtnStop.Size = New System.Drawing.Size(75, 23)
        Me.BtnStop.TabIndex = 11
        Me.BtnStop.Text = "Stop"
        Me.BtnStop.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ChkDarks5)
        Me.GroupBox1.Controls.Add(Me.TxtDarkExposureDuration5)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.ChkDarks4)
        Me.GroupBox1.Controls.Add(Me.ChkDarks3)
        Me.GroupBox1.Controls.Add(Me.ChkDarks2)
        Me.GroupBox1.Controls.Add(Me.TxtDarkExposureDuration4)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.TxtDarkExposureDuration3)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.TxtDarkExposureDuration2)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.CmbTargetFilter)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.CmbTargetBinning)
        Me.GroupBox1.Controls.Add(Me.TxtBiasNumberExposures)
        Me.GroupBox1.Controls.Add(Me.TxtDarkNumberExposures)
        Me.GroupBox1.Controls.Add(Me.TxtDarkExposureDuration1)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.ChkBias)
        Me.GroupBox1.Controls.Add(Me.ChkDarks1)
        Me.GroupBox1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.White
        Me.GroupBox1.Location = New System.Drawing.Point(2, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(297, 203)
        Me.GroupBox1.TabIndex = 25
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "First turn on equipment and close the Snapcap if present!"
        '
        'ChkDarks5
        '
        Me.ChkDarks5.AutoSize = True
        Me.ChkDarks5.Checked = True
        Me.ChkDarks5.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkDarks5.FlatAppearance.BorderSize = 0
        Me.ChkDarks5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ChkDarks5.ForeColor = System.Drawing.Color.White
        Me.ChkDarks5.Location = New System.Drawing.Point(14, 175)
        Me.ChkDarks5.Name = "ChkDarks5"
        Me.ChkDarks5.Size = New System.Drawing.Size(45, 17)
        Me.ChkDarks5.TabIndex = 49
        Me.ChkDarks5.Text = "Dark"
        Me.ChkDarks5.UseVisualStyleBackColor = True
        '
        'TxtDarkExposureDuration5
        '
        Me.TxtDarkExposureDuration5.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtDarkExposureDuration5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtDarkExposureDuration5.Location = New System.Drawing.Point(65, 173)
        Me.TxtDarkExposureDuration5.Name = "TxtDarkExposureDuration5"
        Me.TxtDarkExposureDuration5.Size = New System.Drawing.Size(50, 21)
        Me.TxtDarkExposureDuration5.TabIndex = 48
        Me.TxtDarkExposureDuration5.Text = "180"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(119, 175)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(12, 13)
        Me.Label8.TabIndex = 47
        Me.Label8.Text = "s"
        '
        'ChkDarks4
        '
        Me.ChkDarks4.AutoSize = True
        Me.ChkDarks4.Checked = True
        Me.ChkDarks4.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkDarks4.FlatAppearance.BorderSize = 0
        Me.ChkDarks4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ChkDarks4.ForeColor = System.Drawing.Color.White
        Me.ChkDarks4.Location = New System.Drawing.Point(14, 149)
        Me.ChkDarks4.Name = "ChkDarks4"
        Me.ChkDarks4.Size = New System.Drawing.Size(45, 17)
        Me.ChkDarks4.TabIndex = 46
        Me.ChkDarks4.Text = "Dark"
        Me.ChkDarks4.UseVisualStyleBackColor = True
        '
        'ChkDarks3
        '
        Me.ChkDarks3.AutoSize = True
        Me.ChkDarks3.Checked = True
        Me.ChkDarks3.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkDarks3.FlatAppearance.BorderSize = 0
        Me.ChkDarks3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ChkDarks3.ForeColor = System.Drawing.Color.White
        Me.ChkDarks3.Location = New System.Drawing.Point(14, 122)
        Me.ChkDarks3.Name = "ChkDarks3"
        Me.ChkDarks3.Size = New System.Drawing.Size(45, 17)
        Me.ChkDarks3.TabIndex = 45
        Me.ChkDarks3.Text = "Dark"
        Me.ChkDarks3.UseVisualStyleBackColor = True
        '
        'ChkDarks2
        '
        Me.ChkDarks2.AutoSize = True
        Me.ChkDarks2.Checked = True
        Me.ChkDarks2.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkDarks2.FlatAppearance.BorderSize = 0
        Me.ChkDarks2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ChkDarks2.ForeColor = System.Drawing.Color.White
        Me.ChkDarks2.Location = New System.Drawing.Point(14, 96)
        Me.ChkDarks2.Name = "ChkDarks2"
        Me.ChkDarks2.Size = New System.Drawing.Size(45, 17)
        Me.ChkDarks2.TabIndex = 44
        Me.ChkDarks2.Text = "Dark"
        Me.ChkDarks2.UseVisualStyleBackColor = True
        '
        'TxtDarkExposureDuration4
        '
        Me.TxtDarkExposureDuration4.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtDarkExposureDuration4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtDarkExposureDuration4.Location = New System.Drawing.Point(65, 147)
        Me.TxtDarkExposureDuration4.Name = "TxtDarkExposureDuration4"
        Me.TxtDarkExposureDuration4.Size = New System.Drawing.Size(50, 21)
        Me.TxtDarkExposureDuration4.TabIndex = 43
        Me.TxtDarkExposureDuration4.Text = "120"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.White
        Me.Label7.Location = New System.Drawing.Point(119, 149)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(12, 13)
        Me.Label7.TabIndex = 42
        Me.Label7.Text = "s"
        '
        'TxtDarkExposureDuration3
        '
        Me.TxtDarkExposureDuration3.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtDarkExposureDuration3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtDarkExposureDuration3.Location = New System.Drawing.Point(65, 121)
        Me.TxtDarkExposureDuration3.Name = "TxtDarkExposureDuration3"
        Me.TxtDarkExposureDuration3.Size = New System.Drawing.Size(50, 21)
        Me.TxtDarkExposureDuration3.TabIndex = 41
        Me.TxtDarkExposureDuration3.Text = "90"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(119, 123)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(12, 13)
        Me.Label6.TabIndex = 40
        Me.Label6.Text = "s"
        '
        'TxtDarkExposureDuration2
        '
        Me.TxtDarkExposureDuration2.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtDarkExposureDuration2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtDarkExposureDuration2.Location = New System.Drawing.Point(65, 95)
        Me.TxtDarkExposureDuration2.Name = "TxtDarkExposureDuration2"
        Me.TxtDarkExposureDuration2.Size = New System.Drawing.Size(50, 21)
        Me.TxtDarkExposureDuration2.TabIndex = 39
        Me.TxtDarkExposureDuration2.Text = "60"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(119, 97)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(12, 13)
        Me.Label5.TabIndex = 38
        Me.Label5.Text = "s"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 13)
        Me.Label2.TabIndex = 37
        '
        'CmbTargetFilter
        '
        Me.CmbTargetFilter.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.CmbTargetFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CmbTargetFilter.FormattingEnabled = True
        Me.CmbTargetFilter.Location = New System.Drawing.Point(63, 19)
        Me.CmbTargetFilter.Name = "CmbTargetFilter"
        Me.CmbTargetFilter.Size = New System.Drawing.Size(50, 21)
        Me.CmbTargetFilter.TabIndex = 33
        Me.CmbTargetFilter.Text = "H"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(11, 24)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(54, 13)
        Me.Label10.TabIndex = 35
        Me.Label10.Text = "Use filter:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(138, 22)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(45, 13)
        Me.Label11.TabIndex = 36
        Me.Label11.Text = "Binning:"
        '
        'CmbTargetBinning
        '
        Me.CmbTargetBinning.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.CmbTargetBinning.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CmbTargetBinning.FormattingEnabled = True
        Me.CmbTargetBinning.Items.AddRange(New Object() {"1x1", "2x2", "3x3"})
        Me.CmbTargetBinning.Location = New System.Drawing.Point(194, 19)
        Me.CmbTargetBinning.Name = "CmbTargetBinning"
        Me.CmbTargetBinning.Size = New System.Drawing.Size(50, 21)
        Me.CmbTargetBinning.TabIndex = 34
        Me.CmbTargetBinning.Text = "1x1"
        '
        'TxtBiasNumberExposures
        '
        Me.TxtBiasNumberExposures.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtBiasNumberExposures.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtBiasNumberExposures.Location = New System.Drawing.Point(133, 45)
        Me.TxtBiasNumberExposures.Name = "TxtBiasNumberExposures"
        Me.TxtBiasNumberExposures.Size = New System.Drawing.Size(50, 21)
        Me.TxtBiasNumberExposures.TabIndex = 32
        Me.TxtBiasNumberExposures.Text = "100"
        '
        'TxtDarkNumberExposures
        '
        Me.TxtDarkNumberExposures.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtDarkNumberExposures.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtDarkNumberExposures.Location = New System.Drawing.Point(133, 70)
        Me.TxtDarkNumberExposures.Name = "TxtDarkNumberExposures"
        Me.TxtDarkNumberExposures.Size = New System.Drawing.Size(50, 21)
        Me.TxtDarkNumberExposures.TabIndex = 31
        Me.TxtDarkNumberExposures.Text = "40"
        '
        'TxtDarkExposureDuration1
        '
        Me.TxtDarkExposureDuration1.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.TxtDarkExposureDuration1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtDarkExposureDuration1.Location = New System.Drawing.Point(65, 70)
        Me.TxtDarkExposureDuration1.Name = "TxtDarkExposureDuration1"
        Me.TxtDarkExposureDuration1.Size = New System.Drawing.Size(50, 21)
        Me.TxtDarkExposureDuration1.TabIndex = 30
        Me.TxtDarkExposureDuration1.Text = "30"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(189, 48)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 13)
        Me.Label4.TabIndex = 29
        Me.Label4.Text = "exposures"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(189, 73)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 13)
        Me.Label3.TabIndex = 28
        Me.Label3.Text = "exposures"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(119, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(12, 13)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "s"
        '
        'ChkBias
        '
        Me.ChkBias.AutoSize = True
        Me.ChkBias.Checked = True
        Me.ChkBias.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkBias.FlatAppearance.BorderSize = 0
        Me.ChkBias.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ChkBias.ForeColor = System.Drawing.Color.White
        Me.ChkBias.Location = New System.Drawing.Point(14, 47)
        Me.ChkBias.Name = "ChkBias"
        Me.ChkBias.Size = New System.Drawing.Size(42, 17)
        Me.ChkBias.TabIndex = 26
        Me.ChkBias.Text = "Bias"
        Me.ChkBias.UseVisualStyleBackColor = True
        '
        'ChkDarks1
        '
        Me.ChkDarks1.AutoSize = True
        Me.ChkDarks1.Checked = True
        Me.ChkDarks1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkDarks1.FlatAppearance.BorderSize = 0
        Me.ChkDarks1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ChkDarks1.ForeColor = System.Drawing.Color.White
        Me.ChkDarks1.Location = New System.Drawing.Point(14, 72)
        Me.ChkDarks1.Name = "ChkDarks1"
        Me.ChkDarks1.Size = New System.Drawing.Size(45, 17)
        Me.ChkDarks1.TabIndex = 25
        Me.ChkDarks1.Text = "Dark"
        Me.ChkDarks1.UseVisualStyleBackColor = True
        '
        'FrmCalibration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(99, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(114, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(304, 240)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.BtnStop)
        Me.Controls.Add(Me.BtnStart)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmCalibration"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Calibration frames"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BtnStart As Button
    Friend WithEvents BtnStop As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents CmbTargetFilter As ComboBox
    Friend WithEvents Label10 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents CmbTargetBinning As ComboBox
    Friend WithEvents TxtBiasNumberExposures As TextBox
    Friend WithEvents TxtDarkNumberExposures As TextBox
    Friend WithEvents TxtDarkExposureDuration1 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents ChkBias As CheckBox
    Friend WithEvents ChkDarks1 As CheckBox
    Friend WithEvents TxtDarkExposureDuration4 As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents TxtDarkExposureDuration3 As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents TxtDarkExposureDuration2 As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents ChkDarks4 As CheckBox
    Friend WithEvents ChkDarks3 As CheckBox
    Friend WithEvents ChkDarks2 As CheckBox
    Friend WithEvents ChkDarks5 As CheckBox
    Friend WithEvents TxtDarkExposureDuration5 As TextBox
    Friend WithEvents Label8 As Label
End Class
