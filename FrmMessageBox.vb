
Public Class FrmMessageBox
    'CRITICAL, OKONLY, YESNO, OKCANCEL
    Private Sub FrmMessageBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If pMessageBoxType = "CRITICAL" Then
            LblCritical.Visible = True
        Else
            LblCritical.Visible = False
        End If

        LblMessage.Text = pMessageBoxText
        LblTitle.Text = pMessageBoxTitle

        Select Case pMessageBoxType
            Case "CRITICAL"
                BtnTwoAlfa.Visible = False
                BtnTwoBeta.Visible = False
                BtnOne.Visible = True
                BtnOne.Text = "OK"
            Case "OKONLY"
                BtnTwoAlfa.Visible = False
                BtnTwoBeta.Visible = False
                BtnOne.Visible = True
                BtnOne.Text = "OK"
            Case "YESNO"
                BtnTwoAlfa.Visible = True
                BtnTwoBeta.Visible = True
                BtnOne.Visible = False
                BtnTwoAlfa.Text = "Yes"
                BtnTwoBeta.Text = "No"
            Case "OKCANCEL"
                BtnTwoAlfa.Visible = True
                BtnTwoBeta.Visible = True
                BtnOne.Visible = False
                BtnTwoAlfa.Text = "OK"
                BtnTwoBeta.Text = "Cancel"
        End Select

    End Sub

    Private Sub BtnExit_Click(sender As Object, e As EventArgs) Handles BtnExit.Click
        DialogResult = Nothing
        Me.Close()
    End Sub

    Private Sub BtnTwoAlfa_Click(sender As Object, e As EventArgs) Handles BtnTwoAlfa.Click
        Select Case pMessageBoxType
            Case "YESNO"
                DialogResult = DialogResult.Yes
            Case "OKCANCEL"
                DialogResult = DialogResult.OK
        End Select
        Me.Close()
    End Sub

    Private Sub BtnTwoBeta_Click(sender As Object, e As EventArgs) Handles BtnTwoBeta.Click
        Select Case pMessageBoxType
            Case "YESNO"
                DialogResult = DialogResult.No
            Case "OKCANCEL"
                DialogResult = DialogResult.Cancel
        End Select
        Me.Close()
    End Sub

    Private Sub BtnOne_Click(sender As Object, e As EventArgs) Handles BtnOne.Click
        Select Case pMessageBoxType
            Case "CRITICAL"
                DialogResult = DialogResult.OK
            Case "OKONLY"
                DialogResult = DialogResult.OK
        End Select
        Me.Close()
    End Sub
End Class