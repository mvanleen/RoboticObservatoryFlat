Public Class FrmMessageBox
    Public pMessageTitle As String
    Public pMessageText As String
    Public pMessageType As String


    Private Sub FrmMessageBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub FrmMessageBox_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        DialogResult = DialogResult.OK
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class