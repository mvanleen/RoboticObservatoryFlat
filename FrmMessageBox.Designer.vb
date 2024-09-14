<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMessageBox
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMessageBox))
        Me.LblMessage = New System.Windows.Forms.Label()
        Me.LblTitle = New System.Windows.Forms.Label()
        Me.BtnExit = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.LblCritical = New System.Windows.Forms.Label()
        Me.BtnTwoAlfa = New System.Windows.Forms.Button()
        Me.BtnTwoBeta = New System.Windows.Forms.Button()
        Me.BtnOne = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'LblMessage
        '
        Me.LblMessage.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMessage.ForeColor = System.Drawing.Color.White
        Me.LblMessage.Location = New System.Drawing.Point(53, 26)
        Me.LblMessage.Name = "LblMessage"
        Me.LblMessage.Size = New System.Drawing.Size(249, 56)
        Me.LblMessage.TabIndex = 1
        Me.LblMessage.Text = "Here is the message that needs to be reviewed"
        Me.LblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblTitle
        '
        Me.LblTitle.AutoSize = True
        Me.LblTitle.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTitle.ForeColor = System.Drawing.Color.White
        Me.LblTitle.Location = New System.Drawing.Point(7, 7)
        Me.LblTitle.Name = "LblTitle"
        Me.LblTitle.Size = New System.Drawing.Size(133, 16)
        Me.LblTitle.TabIndex = 5
        Me.LblTitle.Text = "This is the title text"
        '
        'BtnExit
        '
        Me.BtnExit.BackgroundImage = CType(resources.GetObject("BtnExit.BackgroundImage"), System.Drawing.Image)
        Me.BtnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.BtnExit.FlatAppearance.BorderSize = 0
        Me.BtnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnExit.Location = New System.Drawing.Point(320, 0)
        Me.BtnExit.Name = "BtnExit"
        Me.BtnExit.Size = New System.Drawing.Size(34, 34)
        Me.BtnExit.TabIndex = 6
        Me.BtnExit.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(51, Byte), Integer), CType(CType(102, Byte), Integer))
        Me.Panel1.Controls.Add(Me.BtnExit)
        Me.Panel1.Controls.Add(Me.LblTitle)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(354, 33)
        Me.Panel1.TabIndex = 4
        '
        'LblCritical
        '
        Me.LblCritical.Image = CType(resources.GetObject("LblCritical.Image"), System.Drawing.Image)
        Me.LblCritical.Location = New System.Drawing.Point(0, 36)
        Me.LblCritical.Name = "LblCritical"
        Me.LblCritical.Size = New System.Drawing.Size(64, 64)
        Me.LblCritical.TabIndex = 5
        '
        'BtnTwoAlfa
        '
        Me.BtnTwoAlfa.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnTwoAlfa.FlatAppearance.BorderSize = 0
        Me.BtnTwoAlfa.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnTwoAlfa.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTwoAlfa.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnTwoAlfa.ForeColor = System.Drawing.Color.White
        Me.BtnTwoAlfa.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnTwoAlfa.Location = New System.Drawing.Point(99, 77)
        Me.BtnTwoAlfa.Name = "BtnTwoAlfa"
        Me.BtnTwoAlfa.Size = New System.Drawing.Size(75, 38)
        Me.BtnTwoAlfa.TabIndex = 6
        Me.BtnTwoAlfa.Text = "Alfa"
        Me.BtnTwoAlfa.UseVisualStyleBackColor = False
        '
        'BtnTwoBeta
        '
        Me.BtnTwoBeta.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnTwoBeta.FlatAppearance.BorderSize = 0
        Me.BtnTwoBeta.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnTwoBeta.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnTwoBeta.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnTwoBeta.ForeColor = System.Drawing.Color.White
        Me.BtnTwoBeta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnTwoBeta.Location = New System.Drawing.Point(180, 77)
        Me.BtnTwoBeta.Name = "BtnTwoBeta"
        Me.BtnTwoBeta.Size = New System.Drawing.Size(75, 38)
        Me.BtnTwoBeta.TabIndex = 7
        Me.BtnTwoBeta.Text = "Beta"
        Me.BtnTwoBeta.UseVisualStyleBackColor = False
        '
        'BtnOne
        '
        Me.BtnOne.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.BtnOne.FlatAppearance.BorderSize = 0
        Me.BtnOne.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(151, Byte), Integer), CType(CType(230, Byte), Integer))
        Me.BtnOne.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnOne.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnOne.ForeColor = System.Drawing.Color.White
        Me.BtnOne.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnOne.Location = New System.Drawing.Point(140, 77)
        Me.BtnOne.Name = "BtnOne"
        Me.BtnOne.Size = New System.Drawing.Size(75, 38)
        Me.BtnOne.TabIndex = 0
        Me.BtnOne.Text = "One"
        Me.BtnOne.UseVisualStyleBackColor = False
        '
        'FrmMessageBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Silver
        Me.ClientSize = New System.Drawing.Size(354, 117)
        Me.Controls.Add(Me.BtnOne)
        Me.Controls.Add(Me.BtnTwoBeta)
        Me.Controls.Add(Me.BtnTwoAlfa)
        Me.Controls.Add(Me.LblCritical)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.LblMessage)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "FrmMessageBox"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "FrmMessageBox"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LblMessage As Label
    Friend WithEvents LblTitle As Label
    Friend WithEvents BtnExit As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents LblCritical As Label
    Friend WithEvents BtnTwoAlfa As Button
    Friend WithEvents BtnTwoBeta As Button
    Friend WithEvents BtnOne As Button
End Class
