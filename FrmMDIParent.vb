Imports System.ComponentModel

Public Class FrmMDIParent


    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        If pStartRun = True Then
            MsgBox("First abort the run !", vbCritical, "Abort")
        Else
            Me.Close()
        End If
    End Sub



    Private Sub ToolStripMenuProperties_Click(sender As Object, e As EventArgs) Handles ToolStripMenuProperties.Click
        FrmProperties.MdiParent = Me
        FrmProperties.Show()
    End Sub

    Private Sub ToolStripMenuDebug_Click(sender As Object, e As EventArgs) Handles ToolStripMenuDebug.Click
        FrmDebug.MdiParent = Me
        FrmDebug.Show()
    End Sub

    Private Sub ToolStripMenuTools_Click(sender As Object, e As EventArgs) Handles ToolStripMenuTools.Click
        FrmTools.MdiParent = Me
        FrmTools.StartPosition = FormStartPosition.Manual
        FrmTools.Location = New Point(0, 150)
        FrmTools.Show()
    End Sub

    Private Sub DeepskyTargetsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeepskyTargetsToolStripMenuItem.Click
        FrmTarget.MdiParent = Me
        FrmTarget.Show()
    End Sub

    Private Sub ToolStripMenuItemSound_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemSound.Click
        StopSound()
    End Sub

    Private Sub StartSentinelToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartSentinelToolStripMenuItem.Click
        'try to start the sentinel
        StartProcess(My.Settings.sSentinelEXE)
    End Sub

    Private Sub FrmMDIParent_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Try
            LogInitializeArray()

            If pStartRun = True Then
                MsgBox("First abort the run !", vbCritical, "Abort")
                e.Cancel = True
            End If
        Catch ex As Exception
            MsgBox("FrmMain_Closing: " + ex.Message)
        End Try
    End Sub

    Private Sub CalibrationFramesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CalibrationFramesToolStripMenuItem.Click
        FrmCalibration.MdiParent = Me
        FrmCalibration.Show()
    End Sub

    Private Sub VariableTargetsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VariableTargetsToolStripMenuItem.Click
        FrmHADS.MdiParent = Me
        FrmHADS.Show()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        FrmSplash.MdiParent = Me
        FrmSplash.StartPosition = FormStartPosition.CenterScreen
        FrmSplash.Show()
    End Sub

    Private Sub FrmMDIParent_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        My.Application.DoEvents()
        FrmSplash.MdiParent = Me
        FrmSplash.StartPosition = FormStartPosition.CenterScreen
        My.Application.DoEvents()
        FrmSplash.Show()
        My.Application.DoEvents()

        WaitSeconds(3, True, False)
        FrmSplash.Close()

        FrmMain.MdiParent = Me
        FrmMain.StartPosition = FormStartPosition.CenterScreen
        FrmMain.Show()
    End Sub

End Class