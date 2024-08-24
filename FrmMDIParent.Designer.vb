<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMDIParent
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMDIParent))
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.FileMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemSound = New System.Windows.Forms.ToolStripMenuItem()
        Me.StartSentinelToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuDebug = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuProperties = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ObservationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeepskyTargetsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VariableTargetsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.CalibrationFramesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.MenuStrip.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip
        '
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileMenu, Me.ObservationToolStripMenuItem, Me.WindowToolStripMenuItem})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.MdiWindowListItem = Me.WindowToolStripMenuItem
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(1134, 24)
        Me.MenuStrip.TabIndex = 5
        Me.MenuStrip.Text = "MenuStrip"
        '
        'FileMenu
        '
        Me.FileMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemSound, Me.StartSentinelToolStripMenuItem, Me.ToolStripMenuTools, Me.ToolStripSeparator1, Me.ToolStripMenuDebug, Me.ToolStripMenuProperties, Me.ToolStripSeparator5, Me.ExitToolStripMenuItem})
        Me.FileMenu.ImageTransparentColor = System.Drawing.SystemColors.ActiveBorder
        Me.FileMenu.Name = "FileMenu"
        Me.FileMenu.Size = New System.Drawing.Size(37, 20)
        Me.FileMenu.Text = "&File"
        '
        'ToolStripMenuItemSound
        '
        Me.ToolStripMenuItemSound.Name = "ToolStripMenuItemSound"
        Me.ToolStripMenuItemSound.Size = New System.Drawing.Size(143, 22)
        Me.ToolStripMenuItemSound.Text = "Stop sound!"
        '
        'StartSentinelToolStripMenuItem
        '
        Me.StartSentinelToolStripMenuItem.Name = "StartSentinelToolStripMenuItem"
        Me.StartSentinelToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.StartSentinelToolStripMenuItem.Text = "Start Sentinel"
        '
        'ToolStripMenuTools
        '
        Me.ToolStripMenuTools.Name = "ToolStripMenuTools"
        Me.ToolStripMenuTools.Size = New System.Drawing.Size(143, 22)
        Me.ToolStripMenuTools.Text = "Tools"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(140, 6)
        '
        'ToolStripMenuDebug
        '
        Me.ToolStripMenuDebug.Name = "ToolStripMenuDebug"
        Me.ToolStripMenuDebug.Size = New System.Drawing.Size(143, 22)
        Me.ToolStripMenuDebug.Text = "Debug"
        '
        'ToolStripMenuProperties
        '
        Me.ToolStripMenuProperties.Name = "ToolStripMenuProperties"
        Me.ToolStripMenuProperties.Size = New System.Drawing.Size(143, 22)
        Me.ToolStripMenuProperties.Text = "Properties"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(140, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(143, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'ObservationToolStripMenuItem
        '
        Me.ObservationToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeepskyTargetsToolStripMenuItem, Me.VariableTargetsToolStripMenuItem, Me.ToolStripMenuItem1, Me.CalibrationFramesToolStripMenuItem})
        Me.ObservationToolStripMenuItem.Name = "ObservationToolStripMenuItem"
        Me.ObservationToolStripMenuItem.Size = New System.Drawing.Size(83, 20)
        Me.ObservationToolStripMenuItem.Text = "Observation"
        '
        'DeepskyTargetsToolStripMenuItem
        '
        Me.DeepskyTargetsToolStripMenuItem.Name = "DeepskyTargetsToolStripMenuItem"
        Me.DeepskyTargetsToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.DeepskyTargetsToolStripMenuItem.Text = "Deepsky Targets"
        '
        'VariableTargetsToolStripMenuItem
        '
        Me.VariableTargetsToolStripMenuItem.Name = "VariableTargetsToolStripMenuItem"
        Me.VariableTargetsToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.VariableTargetsToolStripMenuItem.Text = "HADS Targets"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(168, 6)
        '
        'CalibrationFramesToolStripMenuItem
        '
        Me.CalibrationFramesToolStripMenuItem.Name = "CalibrationFramesToolStripMenuItem"
        Me.CalibrationFramesToolStripMenuItem.Size = New System.Drawing.Size(171, 22)
        Me.CalibrationFramesToolStripMenuItem.Text = "Calibration frames"
        '
        'WindowToolStripMenuItem
        '
        Me.WindowToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.WindowToolStripMenuItem.Name = "WindowToolStripMenuItem"
        Me.WindowToolStripMenuItem.Size = New System.Drawing.Size(63, 20)
        Me.WindowToolStripMenuItem.Text = "Window"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(107, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'StatusStrip
        '
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 689)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(1134, 22)
        Me.StatusStrip.TabIndex = 7
        Me.StatusStrip.Text = "StatusStrip"
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(39, 17)
        Me.ToolStripStatusLabel.Text = "Status"
        '
        'FrmMDIParent
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(1134, 711)
        Me.Controls.Add(Me.MenuStrip)
        Me.Controls.Add(Me.StatusStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip
        Me.Name = "FrmMDIParent"
        Me.Text = "Robotic Observatory"
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents FileMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolStripMenuProperties As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripMenuDebug As ToolStripMenuItem
    Friend WithEvents ToolStripMenuTools As ToolStripMenuItem
    Friend WithEvents ObservationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeepskyTargetsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VariableTargetsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItemSound As ToolStripMenuItem
    Friend WithEvents WindowToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StartSentinelToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents CalibrationFramesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
End Class
